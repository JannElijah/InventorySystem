import sqlite3
import contextlib
import os
import time
import bcrypt
from datetime import datetime, timedelta
from typing import Optional, List, Dict
from models import User, Product, Transaction, Supplier, Sale, SaleItem, ChartDataPoint, Notification

class DatabaseRepository:
    def __init__(self, db_path: str = None):
        if db_path is None:
            # Default to looking for inventory.db in the same directory as this file
            base_dir = os.path.dirname(os.path.abspath(__file__))
            self.db_path = os.path.join(base_dir, "inventory.db")
        else:
            self.db_path = db_path
            
        self.initialize_database()

    @contextlib.contextmanager
    def get_connection(self):
        conn = sqlite3.connect(self.db_path)
        conn.row_factory = sqlite3.Row
        try:
            yield conn
            conn.commit()
        except Exception:
            conn.rollback()
            raise
        finally:
            conn.close()

    def initialize_database(self):
        # Ensure WAL mode is on for better concurrency
        try:
            with self.get_connection() as conn:
                conn.execute("PRAGMA journal_mode=WAL;")
                
                # Settings Table
                conn.execute("""
                    CREATE TABLE IF NOT EXISTS Settings (
                        Key TEXT PRIMARY KEY,
                        Value TEXT
                    )
                """)

                # Check for Notifications Table
                conn.execute("""
                    CREATE TABLE IF NOT EXISTS Notifications (
                        NotificationID INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductID INTEGER,
                        Message TEXT NOT NULL,
                        IsRead INTEGER DEFAULT 0,
                        DateCreated TEXT,
                        FOREIGN KEY(ProductID) REFERENCES Products(ProductID)
                    )
                """)
                
                # Check for DateCreated in Notifications (Migration for existing DBs)
                try:
                    conn.execute("SELECT DateCreated FROM Notifications LIMIT 1")
                except sqlite3.OperationalError:
                    try:
                        print("Migrating Notifications table to include DateCreated...")
                        conn.execute("ALTER TABLE Notifications ADD COLUMN DateCreated TEXT")
                    except Exception as e:
                        print(f"Migration Error (Notifications): {e}")

                # Check for UserID in Transactions (Migration)
                try:
                    conn.execute("SELECT UserID FROM Transactions LIMIT 1")
                except sqlite3.OperationalError:
                    try:
                        # Only attempt if table exists - check specifically? 
                        # Or just let it fail safely if table doesn't exist (handled by DBMigrator later)
                        conn.execute("ALTER TABLE Transactions ADD COLUMN UserID INTEGER REFERENCES Users(UserID)")
                        print("Migrated Transactions table to include UserID")
                    except Exception as e:
                         # This is expected on fresh install where Transactions table doesn't exist yet
                         pass

                # Check for ImagePath in Products (Migration)
                try:
                    conn.execute("SELECT ImagePath FROM Products LIMIT 1")
                except sqlite3.OperationalError:
                    try:
                        print("Migrating Products table to include ImagePath...")
                        conn.execute("ALTER TABLE Products ADD COLUMN ImagePath TEXT")
                    except Exception as e:
                        print(f"Migration Error (Products ImagePath): {e}")

        except Exception as e:
            print(f"DB Init Error: {e}")
            pass # Ignore if DB is locked

    # --- Settings ---
    def get_setting(self, key: str, default: str = "") -> str:
        with self.get_connection() as conn:
            row = conn.execute("SELECT Value FROM Settings WHERE Key = ?", (key,)).fetchone()
            return row["Value"] if row else default

    def set_setting(self, key: str, value: str):
        with self.get_connection() as conn:
            conn.execute("INSERT OR REPLACE INTO Settings (Key, Value) VALUES (?, ?)", (key, str(value)))


    # --- Notifications ---

    def add_notification(self, product_id: int, message: str):
        with self.get_connection() as conn:
            conn.execute(
                "INSERT INTO Notifications (ProductID, Message, DateCreated) VALUES (?, ?, ?)",
                (product_id, message, datetime.now())
            )

    def get_notifications(self):
        notifs = []
        from models import Notification # Local import
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Notifications ORDER BY DateCreated DESC")
            for row in cursor:
                # Need a Notification model or just use dict/object.
                # In view we used object access (n.message)
                # Let's define a simple helper class or use models.Notification if it exists
                # Models.py has a Notification class? Let's check or just return an object-like structure
                # For now, let's return a simple class instance dynamically to match view expectation
                class NotifObj:
                    def __init__(self, r):
                        self.notification_id = r["NotificationID"]
                        self.product_id = r["ProductID"]
                        self.message = r["Message"]
                        self.is_read = bool(r["IsRead"])
                        self.timestamp = r["DateCreated"]
                
                notifs.append(NotifObj(row))
        return notifs

    def get_unread_notifications(self):
        notifs = []
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Notifications WHERE IsRead = 0 ORDER BY DateCreated DESC")
            for row in cursor:
                notifs.append({
                    "id": row["NotificationID"],
                    "message": row["Message"],
                    "date": row["DateCreated"]
                })
        return notifs

    def mark_notification_read(self, notif_id: int):
        with self.get_connection() as conn:
            conn.execute("UPDATE Notifications SET IsRead = 1 WHERE NotificationID = ?", (notif_id,))

    def mark_all_notifications_read(self):
        with self.get_connection() as conn:
            conn.execute("UPDATE Notifications SET IsRead = 1 WHERE IsRead = 0")

    def delete_notification(self, notif_id: int):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Notifications WHERE NotificationID = ?", (notif_id,))

    def clear_all_notifications(self):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Notifications")

    def check_low_stock(self, product_id: int):
        """Checks if product low stock and triggers notification if needed."""
        with self.get_connection() as conn:
            curr = conn.execute("SELECT StockQuantity, LowStockThreshold, Description FROM Products WHERE ProductID = ?", (product_id,))
            row = curr.fetchone()
            if row:
                qty = row["StockQuantity"]
                threshold = row["LowStockThreshold"]
                name = row["Description"]
                
                if qty <= threshold:
                    # Check if unread notification already exists to avoid spam
                    check = conn.execute(
                        "SELECT 1 FROM Notifications WHERE ProductID = ? AND IsRead = 0", (product_id,)
                    ).fetchone()
                    if not check:
                        self.add_notification(product_id, f"Low Stock Warning: {name} ({qty} remaining)")

    # --- Audit Log ---
    def log_action(self, product_id: int, user_id: int, action_type: str, qty_change: int, reason: str, prev_stock: int, new_stock: int):
        with self.get_connection() as conn:
            conn.execute(
                """
                INSERT INTO AuditLog (ProductID, UserID, ActionType, QuantityChange, PreviousStock, NewStock, Reason)
                VALUES (?, ?, ?, ?, ?, ?, ?)
                """,
                (product_id, user_id, action_type, qty_change, prev_stock, new_stock, reason)
            )

    def get_audit_logs(self, product_id: int) -> List[dict]:
        logs = []
        with self.get_connection() as conn:
            cursor = conn.execute(
                """
                SELECT a.*, u.Username 
                FROM AuditLog a 
                LEFT JOIN Users u ON a.UserID = u.UserID
                WHERE a.ProductID = ? 
                ORDER BY a.Timestamp DESC
                """, 
                (product_id,)
            )
            for row in cursor:
                logs.append(dict(row))
        return logs

    # --- User Management ---
    
    def get_user_by_username(self, username: str) -> Optional[User]:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Users WHERE Username = ?", (username,))
            row = cursor.fetchone()
            if row:
                return User(
                    user_id=row["UserID"],
                    username=row["Username"],
                    password_hash=row["PasswordHash"],
                    role=row["Role"],
                    contact_number=row["ContactNumber"] if row["ContactNumber"] else None
                )
        return None

    def add_user(self, user: User):
        with self.get_connection() as conn:
            conn.execute(
                "INSERT INTO Users (Username, PasswordHash, Role, ContactNumber) VALUES (?, ?, ?, ?)",
                (user.username, user.password_hash, user.role, user.contact_number)
            )

    def get_all_users(self) -> List[User]:
        users = []
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Users")
            for row in cursor:
                users.append(User(
                    user_id=row["UserID"],
                    username=row["Username"],
                    password_hash=row["PasswordHash"],
                    role=row["Role"],
                    contact_number=row["ContactNumber"] if row["ContactNumber"] else None
                ))
        return users

    def update_user(self, user: User):
         with self.get_connection() as conn:
            # Check if password needs updating (if hash is provided)
            if user.password_hash:
                conn.execute(
                    "UPDATE Users SET Username = ?, PasswordHash = ?, Role = ?, ContactNumber = ? WHERE UserID = ?",
                    (user.username, user.password_hash, user.role, user.contact_number, user.user_id)
                )
            else:
                 conn.execute(
                    "UPDATE Users SET Username = ?, Role = ?, ContactNumber = ? WHERE UserID = ?",
                    (user.username, user.role, user.contact_number, user.user_id)
                )

    def delete_user(self, user_id: int):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Users WHERE UserID = ?", (user_id,))

    # --- Product Management ---

    def get_all_products(self) -> List[Product]:
        products = []
        with self.get_connection() as conn:
            cursor = conn.execute(
                "SELECT ProductID, Barcode, PartNumber, Brand, Description, Volume, Type, Application, "
                "PurchaseCost, SellingPrice, StockQuantity, Notes, LowStockThreshold, "
                "DateCreated, DateModified, CategoryID, ReorderPoint, TargetStock, ImagePath FROM Products"
            )
            for row in cursor:
                products.append(Product(
                    product_id=row["ProductID"],
                    barcode=str(row["Barcode"] or ""),
                    part_number=str(row["PartNumber"] or ""),
                    brand=str(row["Brand"] or ""),
                    description=str(row["Description"] or ""),
                    volume=str(row["Volume"] or ""),
                    type=str(row["Type"] or ""),
                    application=str(row["Application"] or ""),
                    purchase_cost=float(row["PurchaseCost"] or 0),
                    selling_price=float(row["SellingPrice"] or 0),
                    stock_quantity=int(row["StockQuantity"] or 0),
                    notes=str(row["Notes"] or ""),
                    low_stock_threshold=int(row["LowStockThreshold"] or 0),
                    date_created=str(row["DateCreated"] or ""),
                    date_modified=str(row["DateModified"] or ""),
                    # Safely handle new columns (might be missing in partial migration state in memory, but DB has them)
                    # We need to SELECT them first though.
                    category_id=row["CategoryID"] if "CategoryID" in row.keys() else None,
                    reorder_point=row["ReorderPoint"] if "ReorderPoint" in row.keys() else 5,
                    target_stock=row["TargetStock"] if "TargetStock" in row.keys() else 10,
                    image_path=row["ImagePath"] if "ImagePath" in row.keys() else None
                ))
        return products

    def get_all_product_ids(self, query: str = "") -> List[int]:
        """Returns list of product IDs matching the query."""
        ids = []
        with self.get_connection() as conn:
            if not query:
                cursor = conn.execute("SELECT ProductID FROM Products")
            else:
                q = f"%{query}%"
                cursor = conn.execute(
                    "SELECT ProductID FROM Products WHERE Description LIKE ? OR Brand LIKE ? OR PartNumber LIKE ? OR Barcode LIKE ?", 
                    (q, q, q, q)
                )
            for row in cursor:
                ids.append(row[0])
        return ids

    def get_product_count(self, query: str = "") -> int:
        with self.get_connection() as conn:
            if not query:
                cursor = conn.execute("SELECT COUNT(*) FROM Products")
            else:
                q = f"%{query}%"
                cursor = conn.execute(
                    "SELECT COUNT(*) FROM Products WHERE Description LIKE ? OR Brand LIKE ? OR PartNumber LIKE ? OR Barcode LIKE ?", 
                    (q, q, q, q)
                )
            return cursor.fetchone()[0]

    def get_products_paginated(self, page: int, per_page: int, query: str = "") -> List[Product]:
        offset = (page - 1) * per_page
        products = []
        with self.get_connection() as conn:
            base_query = (
                "SELECT ProductID, Barcode, PartNumber, Brand, Description, Volume, Type, Application, "
                "PurchaseCost, SellingPrice, StockQuantity, Notes, LowStockThreshold, "
                "DateCreated, DateModified, CategoryID, ReorderPoint, TargetStock, ImagePath FROM Products"
            )
            
            if query:
                q = f"%{query}%"
                sql = f"{base_query} WHERE Description LIKE ? OR Brand LIKE ? OR PartNumber LIKE ? OR Barcode LIKE ? LIMIT ? OFFSET ?"
                params = (q, q, q, q, per_page, offset)
            else:
                sql = f"{base_query} LIMIT ? OFFSET ?"
                params = (per_page, offset)
                
            cursor = conn.execute(sql, params)
            
            for row in cursor:
                products.append(Product(
                    product_id=row["ProductID"],
                    barcode=str(row["Barcode"] or ""),
                    part_number=str(row["PartNumber"] or ""),
                    brand=str(row["Brand"] or ""),
                    description=str(row["Description"] or ""),
                    volume=str(row["Volume"] or ""),
                    type=str(row["Type"] or ""),
                    application=str(row["Application"] or ""),
                    purchase_cost=float(row["PurchaseCost"] or 0),
                    selling_price=float(row["SellingPrice"] or 0),
                    stock_quantity=int(row["StockQuantity"] or 0),
                    notes=str(row["Notes"] or ""),
                    low_stock_threshold=int(row["LowStockThreshold"] or 0),
                    date_created=str(row["DateCreated"] or ""),
                    date_modified=str(row["DateModified"] or ""),
                    category_id=row["CategoryID"] if "CategoryID" in row.keys() else None,
                    reorder_point=row["ReorderPoint"] if "ReorderPoint" in row.keys() else 5,
                    target_stock=row["TargetStock"] if "TargetStock" in row.keys() else 10,
                    image_path=row["ImagePath"] if "ImagePath" in row.keys() else None
                ))
        return products

    def add_product(self, product: Product):
        with self.get_connection() as conn:
            now = datetime.now().isoformat()
            conn.execute(
                """
                INSERT INTO Products (Barcode, PartNumber, Brand, Description, Volume, Type, Application, 
                PurchaseCost, SellingPrice, StockQuantity, Notes, LowStockThreshold, DateCreated, DateModified,
                CategoryID, ReorderPoint, TargetStock, ImagePath) 
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                """,
                (product.barcode, product.part_number, product.brand, product.description, product.volume, 
                 product.type, product.application, product.purchase_cost, product.selling_price, 
                 product.stock_quantity, product.notes, product.low_stock_threshold, now, now,
                 product.category_id, product.reorder_point, product.target_stock, product.image_path)
            )
            
            # Audit Log
            new_id = conn.execute("SELECT last_insert_rowid()").fetchone()[0]
            self.log_action(new_id, 0, "CREATE", product.stock_quantity, "Initial Import/Creation", 0, product.stock_quantity)

    def update_product(self, product: Product):
        with self.get_connection() as conn:
            now = datetime.now().isoformat()
            conn.execute(
                """
                UPDATE Products SET
                Barcode = ?, PartNumber = ?, Brand = ?, Description = ?, Volume = ?, Type = ?, Application = ?, 
                PurchaseCost = ?, SellingPrice = ?, StockQuantity = ?, Notes = ?, LowStockThreshold = ?, DateModified = ?,
                CategoryID = ?, ReorderPoint = ?, TargetStock = ?, ImagePath = ?
                WHERE ProductID = ?
                """,
                (product.barcode, product.part_number, product.brand, product.description, product.volume, 
                 product.type, product.application, product.purchase_cost, product.selling_price, 
                 product.stock_quantity, product.notes, product.low_stock_threshold, now, 
                 product.category_id, product.reorder_point, product.target_stock, product.image_path,
                 product.product_id)
            )
            
            # Audit Log (Approximate change tracking not perfect without previous state fetch, but 'EDIT' action logged)
            # Fetching previous for audit would require query before update.
            # For performance, we might skip deep diff or just log that it was edited.
            # Assuming stock might have changed manually here:
            self.log_action(product.product_id, 0, "EDIT", 0, "Product Details Updated", 0, product.stock_quantity)

    def get_product_by_id(self, product_id: int) -> Optional[Product]:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Products WHERE ProductID = ?", (product_id,))
            row = cursor.fetchone()
            if row:
                return Product(
                    product_id=row["ProductID"],
                    barcode=row["Barcode"] if row["Barcode"] else "",
                    part_number=row["PartNumber"],
                    brand=row["Brand"],
                    description=row["Description"],
                    volume=row["Volume"],
                    type=row["Type"],
                    application=row["Application"],
                    purchase_cost=row["PurchaseCost"],
                    selling_price=row["SellingPrice"],
                    stock_quantity=row["StockQuantity"],
                    notes=row["Notes"],
                    low_stock_threshold=row["LowStockThreshold"],
                    date_created=row["DateCreated"],
                    date_modified=row["DateModified"],
                    category_id=row["CategoryID"],
                    reorder_point=row["ReorderPoint"] if "ReorderPoint" in row.keys() else 5,
                    target_stock=row["TargetStock"] if "TargetStock" in row.keys() else 10,
                    image_path=row["ImagePath"] if "ImagePath" in row.keys() else None
                )
        return None

    def get_product_by_barcode(self, barcode: str) -> Optional[Product]:
        """Fast lookup by barcode or part number (Exact Match)"""
        with self.get_connection() as conn:
            # Check Barcode first
            cursor = conn.execute("SELECT * FROM Products WHERE Barcode = ?", (barcode,))
            row = cursor.fetchone()
            if not row:
                # Check Part Number fallback
                cursor = conn.execute("SELECT * FROM Products WHERE PartNumber = ?", (barcode,))
                row = cursor.fetchone()
                
            if row:
                return self._map_row_to_product(row)
        return None

    def search_products(self, query: str, limit: int = 10) -> List[Product]:
        """Fast text search for description/brand"""
        products = []
        q = f"%{query}%"
        with self.get_connection() as conn:
            cursor = conn.execute(
                "SELECT * FROM Products WHERE Description LIKE ? OR Brand LIKE ? LIMIT ?", 
                (q, q, limit)
            )
            for row in cursor:
                products.append(self._map_row_to_product(row))
        return products

    def _map_row_to_product(self, row) -> Product:
        """Helper to map DB row to Product object"""
        # Identify columns dynamically or assume standard schema. 
        # For robustness, we duplicate the mapping logic here or extract it.
        # Given existing code, duplication is safer than breaking existing dependencies right now.
        return Product(
            product_id=row["ProductID"],
            barcode=row["Barcode"] if row["Barcode"] else "",
            part_number=row["PartNumber"],
            brand=row["Brand"],
            description=row["Description"],
            volume=row["Volume"],
            type=row["Type"],
            application=row["Application"],
            purchase_cost=row["PurchaseCost"],
            selling_price=row["SellingPrice"],
            stock_quantity=row["StockQuantity"],
            notes=row["Notes"],
            low_stock_threshold=row["LowStockThreshold"],
            date_created=row["DateCreated"],
            date_modified=row["DateModified"],
            category_id=row["CategoryID"],
            reorder_point=row["ReorderPoint"] if "ReorderPoint" in row.keys() else 5,
            target_stock=row["TargetStock"] if "TargetStock" in row.keys() else 10,
            image_path=row["ImagePath"] if "ImagePath" in row.keys() else None
        )

    # --- Supplier Management ---

    def get_all_suppliers(self) -> List[Supplier]:
        suppliers = []
        from models import Supplier # Local import to avoid circular dependency if any
        with self.get_connection() as conn:
            # Check if table exists (in case it wasn't in original DB)
            try:
                # Need to handle missing columns if V3 run but migration check strict
                # Safest is execute query and check keys OR assume migration ran
                cursor = conn.execute("SELECT * FROM Suppliers")
                for row in cursor:
                    suppliers.append(Supplier(
                        supplier_id=row["SupplierID"],
                        name=row["Name"],
                        contact_info=row["ContactInfo"] if row["ContactInfo"] else "",
                        email=row["Email"] if "Email" in row.keys() else "",
                        phone=row["Phone"] if "Phone" in row.keys() else ""
                    ))
            except sqlite3.OperationalError:
                pass # Table might not exist yet
        return suppliers

    def add_supplier(self, supplier: Supplier):
        with self.get_connection() as conn:
            conn.execute(
                "INSERT INTO Suppliers (Name, ContactInfo, Email, Phone) VALUES (?, ?, ?, ?)",
                (supplier.name, supplier.contact_info, supplier.email, supplier.phone)
            )

    def update_supplier(self, supplier: Supplier):
        with self.get_connection() as conn:
            conn.execute(
                "UPDATE Suppliers SET Name = ?, ContactInfo = ?, Email = ?, Phone = ? WHERE SupplierID = ?",
                (supplier.name, supplier.contact_info, supplier.email, supplier.phone, supplier.supplier_id)
            )

    # --- Customer Management ---

    def get_all_customers(self):
        from models import Customer
        customers = []
        with self.get_connection() as conn:
            try:
                cursor = conn.execute("SELECT * FROM Customers")
                for row in cursor:
                    customers.append(Customer(
                        customer_id=row["CustomerID"],
                        name=row["Name"],
                        phone=row["Phone"] or "",
                        email=row["Email"] or "",
                        total_spend=str(row["TotalSpend"] or 0)
                    ))
            except: pass
        return customers

    def add_customer(self, customer):
        with self.get_connection() as conn:
            conn.execute(
                "INSERT INTO Customers (Name, Phone, Email) VALUES (?, ?, ?)",
                (customer.name, customer.phone, customer.email)
            )

    def delete_supplier(self, supplier_id: int):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Suppliers WHERE SupplierID = ?", (supplier_id,))

    # --- Transaction/Reports ---

    def get_all_transactions(self):
        transactions = []
        with self.get_connection() as conn:
            # Join with Products to get the name
            sql = """
                SELECT t.*, p.Description as ProductName 
                FROM Transactions t
                LEFT JOIN Products p ON t.ProductID = p.ProductID
                ORDER BY t.TransactionDate DESC, t.TransactionID DESC
                LIMIT 500
            """
            cursor = conn.execute(sql)
            for row in cursor:
                # We can reuse Transaction model or just return dict/custom obj
                # Let's return a list of dicts for simplicity in the view
                transactions.append({
                    "id": row["TransactionID"],
                    "date": row["TransactionDate"],
                    "product": row["ProductName"],
                    "type": row["TransactionType"],
                    "change": row["QuantityChange"],
                    "stock_after": row["StockAfter"]
                })
        return transactions

    # --- Dashboard Metrics ---

    def get_total_product_count(self) -> int:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT COUNT(*) FROM Products")
            result = cursor.fetchone()
            return result[0] if result else 0

    def get_low_stock_count(self) -> int:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT COUNT(*) FROM Products WHERE StockQuantity <= LowStockThreshold")
            result = cursor.fetchone()
            return result[0] if result else 0

    def get_out_of_stock_count(self) -> int:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT COUNT(*) FROM Products WHERE StockQuantity <= 0")
            result = cursor.fetchone()
            return result[0] if result else 0

    def get_total_inventory_value(self) -> float:
        with self.get_connection() as conn:
            cursor = conn.execute(
                "SELECT SUM(StockQuantity * PurchaseCost) FROM Products WHERE StockQuantity > 0"
            )
            val = cursor.fetchone()[0]
            return float(val) if val else 0.0

    # --- Charts & Analytics ---

    def get_top_selling_products(self, by_revenue: bool = False, days: int = 30) -> List[ChartDataPoint]:
        data_points = []
        # thirty_days_ago = (datetime.now() - timedelta(days=30)).strftime("%Y-%m-%d") # Deprecated in favor of sqlite modifier
        
        # Note: TransactionType 'Delivery' is Stock-Out. QuantityChange is negative.
        # If by_revenue, need to join with UnitPrice (which might vary, here using Current SellingPrice for approx if Transaction doesn't have it saved... 
        # Actually SaleItem has price, but Transaction is simpler for aggregate now. 
        # Optimally we should use SaleItem table for Revenue.
        
        if by_revenue:
            # Better to Query SaleItems table for Revenue
            sql = """
                SELECT 
                    P.Description AS ProductName,
                    SUM(SI.LineTotal) as Revenue
                FROM SaleItems SI
                JOIN Sales S ON SI.SaleID = S.SaleID
                JOIN Products P ON SI.ProductID = P.ProductID
                WHERE DATE(S.SaleDate) >= ?
                GROUP BY SI.ProductID 
                ORDER BY Revenue DESC
                LIMIT 5
            """
        else:
            sql = """
            SELECT
                P.Description AS ProductName,
                SUM(SI.Quantity) AS TotalSold,
                SUM(SI.LineTotal) AS Revenue
            FROM SaleItems SI
            JOIN Sales S ON SI.SaleID = S.SaleID
            JOIN Products P ON SI.ProductID = P.ProductID
            WHERE S.SaleDate >= date('now', ?)
            GROUP BY SI.ProductID
            ORDER BY TotalSold DESC
            LIMIT 5;
            """
        modifier = f'-{days} days'
        
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            for row in cursor:
                data_points.append(ChartDataPoint(
                    label=row["ProductName"],
                    value=row["Revenue"] if by_revenue else row["TotalSold"]
                ))

        return data_points

    def get_daily_sales_volume(self, days: int = 30) -> List[ChartDataPoint]:
        """Returns daily revenue for the last N days."""
        data_points = []
        # Uses SQLite date modifier
        sql = """
            SELECT date(SaleDate) as d, SUM(LineTotal) as total 
            FROM Sales 
            JOIN SaleItems ON Sales.SaleID = SaleItems.SaleID
            WHERE SaleDate >= date('now', ?)
            GROUP BY d
            ORDER BY d ASC
        """
        modifier = f'-{days} days'
        
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            for row in cursor:
                data_points.append(ChartDataPoint(label=row[0], value=row[1]))
        return data_points

    def get_sales_by_brand(self, days: int = 30) -> List[ChartDataPoint]:
        """Returns total revenue per brand for the last N days."""
        data_points = []
        sql = """
            SELECT P.Brand, SUM(SI.LineTotal) as total 
            FROM SaleItems SI
            JOIN Sales S ON SI.SaleID = S.SaleID
            JOIN Products P ON SI.ProductID = P.ProductID
            WHERE S.SaleDate >= date('now', ?)
            GROUP BY P.Brand
            ORDER BY total DESC
        """
        modifier = f'-{days} days'
        
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            for row in cursor:
                if row["Brand"]:
                    data_points.append(ChartDataPoint(label=row["Brand"], value=row["total"]))
        return data_points

    def get_inventory_value_by_brand(self) -> List[ChartDataPoint]:
        """Returns total value of current stock by brand."""
        data_points = []
        sql = """
            SELECT Brand, SUM(StockQuantity * PurchaseCost) as Value
            FROM Products
            WHERE StockQuantity > 0
            GROUP BY Brand
            ORDER BY Value DESC
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql)
            for row in cursor:
                if row["Brand"]:
                    data_points.append(ChartDataPoint(label=row["Brand"], value=row["Value"]))
        return data_points

    def get_inventory_value_by_category(self) -> List[ChartDataPoint]:
        """Returns total value of current stock by category."""
        data_points = []
        sql = """
            SELECT C.Name as CategoryName, SUM(P.StockQuantity * P.PurchaseCost) as Value
            FROM Products P
            LEFT JOIN Categories C ON P.CategoryID = C.CategoryID
            WHERE P.StockQuantity > 0
            GROUP BY C.Name
            ORDER BY Value DESC
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql)
            for row in cursor:
                cat = row["CategoryName"] if row["CategoryName"] else "Uncategorized"
                data_points.append(ChartDataPoint(label=cat, value=row["Value"]))
        return data_points

    def get_sales_summary_stats(self, days: int = 30) -> Dict:
        """Returns aggregate sales stats for the period."""
        stats = {"revenue": 0.0, "orders": 0, "items_sold": 0}
        
        sql = """
            SELECT 
                COUNT(DISTINCT S.SaleID) as Orders,
                SUM(SI.LineTotal) as Revenue,
                SUM(SI.Quantity) as Items
            FROM Sales S
            JOIN SaleItems SI ON S.SaleID = SI.SaleID
            WHERE S.SaleDate >= date('now', ?)
        """
        modifier = f'-{days} days'
        with self.get_connection() as conn:
             row = conn.execute(sql, (modifier,)).fetchone()
             if row:
                 stats["orders"] = row[0] if row[0] else 0
                 stats["revenue"] = row[1] if row[1] else 0.0
                 stats["items_sold"] = row[2] if row[2] else 0
        return stats

        # Simple Estimate: COGS (30d) / Avg Inventory Value (Current)
        # Ideally Avg Inv is (Start + End) / 2. We only have current.
        cogs = abs(self.get_gross_profit(30)) # Wait, gross profit is (Rev - Cost). 
        # We need sum of COST for sold items.
        
        thirty_days_ago = (datetime.now() - timedelta(days=30)).strftime("%Y-%m-%d")
        sql = """
            SELECT SUM(ABS(T.QuantityChange) * P.PurchaseCost) as COGS
            FROM Transactions T
            JOIN Products P ON T.ProductID = P.ProductID
            WHERE T.TransactionType = 'Delivery' AND DATE(T.TransactionDate) >= ?
        """
        current_inv_value = self.get_total_inventory_value()
        if current_inv_value == 0: return 0.0
        
        with self.get_connection() as conn:
            res = conn.execute(sql, (thirty_days_ago,)).fetchone()
            cogs = res[0] if res and res[0] else 0.0
            
        return cogs / current_inv_value

        with self.get_connection() as conn:
            cursor = conn.execute(sql, (thirty_days_ago,))
            for row in cursor:
                date_str = row["SaleDate"]
                try:
                    date_obj = datetime.strptime(date_str, "%Y-%m-%d")
                    label = date_obj.strftime("%b %d") # e.g. "Nov 13"
                except ValueError:
                    label = date_str

                data_points.append(ChartDataPoint(
                    label=label,
                    value=abs(int(row["TotalSold"]))
                ))
        return data_points



    def global_search(self, query: str) -> List[Dict]:
        """
        Searches Products, Customers (Sales), and Settings/Nav logic? 
        Returns list of {type, text, id, detail}
        """
        results = []
        q_wild = f"%{query}%"
        with self.get_connection() as conn:
            # 1. Products
            cur = conn.execute(
                "SELECT ProductID, Description, Barcode FROM Products WHERE Description LIKE ? OR Barcode LIKE ? LIMIT 5", 
                (q_wild, q_wild)
            )
            for row in cur:
                results.append({
                    "type": "Product",
                    "text": row[1],
                    "detail": f"Barcode: {row[2]}",
                    "id": row[0]
                })

            # 2. Key Customers (from Sales) - Distinct names
            cur = conn.execute(
                "SELECT DISTINCT DeliverTo FROM Sales WHERE DeliverTo LIKE ? LIMIT 5", 
                (q_wild,)
            )
            for row in cur:
                results.append({
                    "type": "Customer",
                    "text": row[0],
                    "detail": "Customer Record",
                    "id": row[0] # Name as ID
                })
                
            # 3. Suppliers
            cur = conn.execute(
                "SELECT SupplierID, Name FROM Suppliers WHERE Name LIKE ? LIMIT 5",
                (q_wild,)
            )
            for row in cur:
                results.append({
                    "type": "Supplier",
                    "text": row[1],
                    "detail": "Supplier Record",
                    "id": row[0]
                })
        return results

    # --- POS / Transactions ---

    def process_complete_sale(self, sale: Sale, user_id: int):
        # Retry logic for SQLite locking
        max_retries = 5
        for i in range(max_retries):
            try:
                with self.get_connection() as conn:
                    conn.execute("BEGIN TRANSACTION")
                    
                    try:
                        # 1. Save Sale Header
                        cursor = conn.execute(
                            "INSERT INTO Sales (DeliverTo, SaleDate) VALUES (?, ?)",
                            (sale.customer_name, sale.sale_date)
                        )
                        sale_id = cursor.lastrowid

                        # 2. Process Items
                        for item in sale.items:
                            # A. Add Sale Item
                            cursor = conn.execute(
                                """
                                INSERT INTO SaleItems (SaleID, ProductID, Quantity, UnitPrice, LineTotal) 
                                VALUES (?, ?, ?, ?, ?)
                                """,
                                (sale_id, item.product_id, item.quantity, item.unit_price, item.total)
                            )
                            sale_item_id = cursor.lastrowid

                            # B. Get Current Stock to calculate logs
                            cur = conn.execute("SELECT StockQuantity FROM Products WHERE ProductID = ?", (item.product_id,))
                            stock_before = cur.fetchone()[0]
                            stock_after = stock_before - item.quantity
                            quantity_change = -item.quantity

                            # C. Log Transaction (Audit Trail)
                            conn.execute(
                                """
                                INSERT INTO Transactions 
                                (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, SaleItemID, UserID) 
                                VALUES (?, 'Delivery', ?, ?, ?, ?, ?, ?)
                                """,
                                (item.product_id, quantity_change, stock_before, stock_after, datetime.now(), sale_item_id, user_id)
                            )

                            # D. Update Product Stock
                            conn.execute(
                                "UPDATE Products SET StockQuantity = ? WHERE ProductID = ?",
                                (stock_after, item.product_id)
                            )
                            
                            # F. Audit Log (New V4)
                            conn.execute(
                                """
                                INSERT INTO AuditLog (ProductID, UserID, ActionType, QuantityChange, PreviousStock, NewStock, Reason)
                                VALUES (?, ?, 'SALE', ?, ?, ?, ?)
                                """,
                                (item.product_id, user_id, quantity_change, stock_before, stock_after, f"Sale #{sale_id}")
                            )

                            # E. Check Low Stock (Legacy notification trigger)
                            # We can call self.check_low_stock later or here if we pass connection, 
                            # but to keep it safe inside transaction we just commit and check after.
                            # Or we can insert notification here.
                            # Let's keep it simple: Check AFTER commit to avoid holding lock.

                        conn.commit()
                        
                        # Post-Transaction: Check Notifications
                        for item in sale.items:
                            self.check_low_stock(item.product_id)
                            
                        return sale_id # Success

                    except Exception as e:
                        conn.rollback()
                        raise e # Re-raise to caller
                        
            except sqlite3.OperationalError as e:
                if "locked" in str(e) and i < max_retries - 1:
                    time.sleep(0.2)
                    continue
                raise e

    def create_sale(self, items: list, total: float, user_id: int = 1) -> int:
        """Wrapper for POSView compatibility"""
        from models import Sale
        # items in POSView are already SaleItem objects
        sale = Sale(customer_name="Walk-in", sale_date=datetime.now(), items=items)
        return self.process_complete_sale(sale, user_id)

    def process_sale(self, customer_name, items, user_id, date=None):
        """Wrapper for StockOutView compatibility"""
        from models import Sale, SaleItem
        sale_items = []
        for i in items:
            # items here are dicts {product_id, quantity, unit_price, total}
            sale_items.append(SaleItem(
                product_id=i['product_id'],
                quantity=i['quantity'],
                unit_price=i.get('unit_price', 0),
                total=i.get('total', 0)
            ))
            
        sale = Sale(customer_name=customer_name, sale_date=date or datetime.now(), items=sale_items)
        return self.process_complete_sale(sale, user_id)

    # --- Supply Transactions ---

    def process_supply_transaction(self, supplier_id: Optional[int], items: List[dict], user_id: int):
        """
        items: List of dicts with {'product_id': int, 'quantity': int, 'cost': float}
        """
        max_retries = 5
        for i in range(max_retries):
            try:
                with self.get_connection() as conn:
                    conn.execute("BEGIN TRANSACTION")
                    try:
                        for item in items:
                            pid = item['product_id']
                            qty = item['quantity']
                            cost = item['cost']
                            
                            # 1. Get current stock
                            cur = conn.execute("SELECT StockQuantity FROM Products WHERE ProductID = ?", (pid,))
                            stock_before = cur.fetchone()[0]
                            stock_after = stock_before + qty
                            
                            # 2. Update Product
                            conn.execute(
                                "UPDATE Products SET StockQuantity = ?, PurchaseCost = ? WHERE ProductID = ?",
                                (stock_after, cost, pid)
                            )
                            
                            # 3. Log Transaction (Audit Trail)
                            conn.execute(
                                """
                                INSERT INTO Transactions 
                                (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, UserID, SupplierID) 
                                VALUES (?, 'Supply', ?, ?, ?, ?, ?, ?)
                                """,
                                (pid, qty, stock_before, stock_after, datetime.now(), user_id, supplier_id)
                            )
                            
                            # 4. Audit Log (New V4)
                            conn.execute(
                                """
                                INSERT INTO AuditLog (ProductID, UserID, ActionType, QuantityChange, PreviousStock, NewStock, Reason)
                                VALUES (?, ?, 'RESTOCK', ?, ?, ?, ?)
                                """,
                                (pid, user_id, qty, stock_before, stock_after, "Manual Stock In")
                            )

                            
                        conn.commit()
                        
                        # Check low stock (Supply might fix low stock, but maybe not enough?)
                        for item in items:
                            self.check_low_stock(item['product_id'])
                            
                        return
                        
                    except Exception as e:
                        conn.rollback()
                        raise e
                        
            except sqlite3.OperationalError as e:
                if "locked" in str(e) and i < max_retries - 1:
                    time.sleep(0.2)
                    continue
                raise e

    # --- Filtering ---

    def get_filtered_transactions(self, 
                                  start_date: Optional[datetime] = None, 
                                  end_date: Optional[datetime] = None, 
                                  trans_type: Optional[str] = None, 
                                  search_query: Optional[str] = None) -> List[Transaction]:
        
        query_parts = ["""
            SELECT
                t.TransactionID, t.ProductID, p.Barcode, p.Description AS ProductDescription,
                t.TransactionType, t.QuantityChange, t.StockBefore, t.StockAfter,
                t.TransactionDate, t.SupplierID, sup.Name AS SupplierName
            FROM Transactions t
            INNER JOIN Products p ON t.ProductID = p.ProductID
            LEFT JOIN Suppliers sup ON t.SupplierID = sup.SupplierID
            WHERE 1=1
        """]
        params = []

        if start_date:
            query_parts.append("AND DATE(t.TransactionDate) >= DATE(?)")
            params.append(start_date.strftime("%Y-%m-%d"))
        
        if end_date:
            query_parts.append("AND DATE(t.TransactionDate) <= DATE(?)")
            params.append(end_date.strftime("%Y-%m-%d"))

        if trans_type and trans_type != "All":
            query_parts.append("AND t.TransactionType = ?")
            params.append(trans_type)

        if search_query:
            q = f"%{search_query}%"
            query_parts.append("AND (p.Description LIKE ? OR p.Brand LIKE ? OR p.Barcode LIKE ?)")
            params.extend([q, q, q])

        query_parts.append("ORDER BY t.TransactionDate DESC, t.TransactionID DESC LIMIT 1000")
        
        sql = " ".join(query_parts)
        
        transactions = []
        with self.get_connection() as conn:
            cursor = conn.execute(sql, tuple(params))
            for row in cursor:
                # Handle date parsing safely
                try:
                    dt = datetime.strptime(row["TransactionDate"], "%Y-%m-%d %H:%M:%S.%f")
                except ValueError:
                    try:
                        dt = datetime.strptime(row["TransactionDate"], "%Y-%m-%d %H:%M:%S")
                    except ValueError:
                        dt = row["TransactionDate"] # Keep as string if fail

                transactions.append(Transaction(
                    transaction_id=row["TransactionID"],
                    product_id=row["ProductID"],
                    transaction_type=row["TransactionType"],
                    quantity_change=row["QuantityChange"],
                    stock_before=row["StockBefore"],
                    stock_after=row["StockAfter"],
                    transaction_date=dt,
                    supplier_id=row["SupplierID"],
                    barcode=row["Barcode"],
                    product_description=row["ProductDescription"],
                    supplier_name=str(row["SupplierName"] or "")
                ))
        return transactions

    def get_dashboard_transactions(self) -> List[Transaction]:
        return self.get_filtered_transactions(search_query=None) # Reuse for consistency but limit 50 is better

    # --- Purchase Orders (Phase 2) ---

    def create_purchase_order(self, supplier_id: int, items: List[Dict]) -> int:
        """
        Creates a new PO.
        items: List of dicts with {'product_id', 'qty', 'cost'}
        """
        with self.get_connection() as conn:
            # Calc total
            total = sum(item['qty'] * item['cost'] for item in items)
            
            # Create PO
            cur = conn.execute(
                "INSERT INTO PurchaseOrders (SupplierID, OrderDate, Status, TotalCost) VALUES (?, ?, ?, ?)",
                (supplier_id, datetime.now(), "Ordered", total)
            )
            po_id = cur.lastrowid
            
            # Add Items
            for item in items:
                conn.execute(
                    "INSERT INTO POItems (POID, ProductID, QuantityOrdered, UnitCost) VALUES (?, ?, ?, ?)",
                    (po_id, item['product_id'], item['qty'], item['cost'])
                )
            return po_id

    def get_all_purchase_orders(self):
        # Local import to avoid circular dep
        from models import PurchaseOrder
        pos = []
        sql = """
            SELECT po.*, s.Name as SupplierName 
            FROM PurchaseOrders po
            LEFT JOIN Suppliers s ON po.SupplierID = s.SupplierID
            ORDER BY po.OrderDate DESC
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql)
            for row in cursor:
                # Parse date
                try: 
                    d = datetime.strptime(row["OrderDate"], "%Y-%m-%d %H:%M:%S.%f")
                except: 
                    try:
                         d = datetime.strptime(row["OrderDate"], "%Y-%m-%d %H:%M:%S")
                    except:
                         d = row["OrderDate"]

                pos.append(PurchaseOrder(
                    po_id=row["POID"],
                    supplier_id=row["SupplierID"],
                    order_date=d,
                    status=row["Status"],
                    total_cost=row["TotalCost"],
                    supplier_name=row["SupplierName"]
                ))
        return pos

    def get_po_items(self, po_id: int):
        from models import POItem
        items = []
        sql = """
            SELECT poi.*, p.Description 
            FROM POItems poi
            JOIN Products p ON poi.ProductID = p.ProductID
            WHERE poi.POID = ?
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (po_id,))
            for row in cursor:
                items.append(POItem(
                    item_id=row["ItemID"],
                    po_id=row["POID"],
                    product_id=row["ProductID"],
                    quantity_ordered=row["QuantityOrdered"],
                    quantity_received=row["QuantityReceived"],
                    unit_cost=row["UnitCost"],
                    product_name=row["Description"]
                ))
        return items

    def receive_po_items(self, po_id: int, received_items: Dict[int, int], user_id: int):
        """
        received_items: Dict {product_id: qty_received}
        """
        with self.get_connection() as conn:
            # 1. Update PO Items and Stock
            all_complete = True
            
            for pid, qty in received_items.items():
                if qty > 0:
                    # Update PO Item Received Qty
                    conn.execute(
                        "UPDATE POItems SET QuantityReceived = QuantityReceived + ? WHERE POID = ? AND ProductID = ?",
                        (qty, po_id, pid)
                    )
                    
                    # Add to Inventory (This also logs transaction)
                    # We need to manually call the transaction logic or reuse add_stock helper if we had one.
                    # Re-implementing logic here for atomicity within this transaction
                    
                    # Update Product Stock
                    conn.execute("UPDATE Products SET StockQuantity = StockQuantity + ? WHERE ProductID = ?", (qty, pid))
                    
                    # Log Transaction
                    # Get current stock for log
                    cur_stock = conn.execute("SELECT StockQuantity FROM Products WHERE ProductID = ?", (pid,)).fetchone()[0]
                    
                    conn.execute("""
                        INSERT INTO Transactions (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, UserID, SupplierID)
                        VALUES (?, 'Supply (PO)', ?, ?, ?, ?, ?, (SELECT SupplierID FROM PurchaseOrders WHERE POID=?))
                    """, (pid, qty, cur_stock - qty, cur_stock, datetime.now(), user_id, po_id))
                    
                    # Audit Log (New V4)
                    prev = cur_stock - qty
                    conn.execute("""
                        INSERT INTO AuditLog (ProductID, UserID, ActionType, QuantityChange, PreviousStock, NewStock, Reason)
                        VALUES (?, ?, 'RESTOCK', ?, ?, ?, ?)
                    """, (pid, user_id, qty, prev, cur_stock, f"PO #{po_id} Received"))

            # 2. Check if PO is fully received
            # Get all items for PO
            items = conn.execute("SELECT QuantityOrdered, QuantityReceived FROM POItems WHERE POID = ?", (po_id,)).fetchall()
            status = "Completed"
            for row in items:
                if row["QuantityReceived"] < row["QuantityOrdered"]:
                    status = "Partially Received"
                    break
            
            conn.execute("UPDATE PurchaseOrders SET Status = ? WHERE POID = ?", (status, po_id))

    # --- Notifications ---
    
    def add_notification(self, product_id: int, message: str) -> int:
        """Adds a new notification."""
        with self.get_connection() as conn:
            cursor = conn.execute(
                "INSERT INTO Notifications (ProductID, Message, DateCreated) VALUES (?, ?, ?)",
                (product_id, message, datetime.now())
            )
            return cursor.lastrowid

    def get_notifications(self) -> List[Notification]:
        """Fetch all notifications, ordered by date desc."""
        n_list = []
        sql = """
            SELECT NotificationID, ProductID, Message, IsRead, DateCreated
            FROM Notifications
            ORDER BY IsRead ASC, DateCreated DESC
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql)
            for row in cursor:
                n_list.append(Notification(
                    notification_id=row["NotificationID"],
                    product_id=row["ProductID"],
                    message=row["Message"],
                    is_read=bool(row["IsRead"]),
                    timestamp=str(row["DateCreated"])
                ))
        return n_list

    def mark_all_notifications_read(self):
        with self.get_connection() as conn:
            conn.execute("UPDATE Notifications SET IsRead = 1")

    def mark_notification_read(self, n_id: int):
        with self.get_connection() as conn:
            conn.execute("UPDATE Notifications SET IsRead = 1 WHERE NotificationID = ?", (n_id,))

    def delete_notification(self, n_id: int):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Notifications WHERE NotificationID = ?", (n_id,))

    def clear_all_notifications(self):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Notifications")
