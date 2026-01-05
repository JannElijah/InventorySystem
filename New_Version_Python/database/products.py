
from typing import List, Optional
from datetime import datetime
from models import Product, Supplier, Customer

class ProductRepositoryMixin:
    # Requires self.get_connection() from BaseRepository
    # Requires self.log_action() from AuditLogMixin (to be defined)

    def get_all_products(self) -> List[Product]:
        products = []
        with self.get_connection() as conn:
            cursor = conn.execute(
                "SELECT ProductID, Barcode, PartNumber, Brand, Description, Volume, Type, Application, "
                "PurchaseCost, SellingPrice, StockQuantity, Notes, LowStockThreshold, "
                "DateCreated, DateModified, CategoryID, ReorderPoint, TargetStock, ImagePath FROM Products"
            )
            for row in cursor:
                products.append(self._map_row_to_product(row))
        return products

    def get_all_product_ids(self, query: str = "") -> List[int]:
        ids = []
        with self.get_connection() as conn:
            if not query:
                cursor = conn.execute("SELECT ProductID FROM Products WHERE IsActive = 1")
            else:
                q = f"%{query}%"
                cursor = conn.execute(
                    "SELECT ProductID FROM Products WHERE (Description LIKE ? OR Brand LIKE ? OR PartNumber LIKE ? OR Barcode LIKE ?) AND IsActive = 1", 
                    (q, q, q, q)
                )
            for row in cursor:
                ids.append(row[0])
        return ids

    def get_product_count(self, query: str = "", active_only: bool = True, stock_status: str = "ALL") -> int:
        with self.get_connection() as conn:
            criteria = ["1=1"]
            params = []
            
            if active_only:
                criteria.append("IsActive = 1")

            if query:
                criteria.append("(Description LIKE ? OR Brand LIKE ? OR PartNumber LIKE ? OR Barcode LIKE ?)")
                q = f"%{query}%"
                params.extend([q, q, q, q])
                
            if stock_status == "OUT":
                criteria.append("StockQuantity <= 0")
            elif stock_status == "LOW":
                 criteria.append("StockQuantity > 0 AND StockQuantity <= LowStockThreshold")
            
            where_clause = " AND ".join(criteria)
            
            cursor = conn.execute(f"SELECT COUNT(*) FROM Products WHERE {where_clause}", params)
            return cursor.fetchone()[0]

    def get_low_stock_count(self) -> int:
        """Cnt of products where StockQuantity <= LowStockThreshold and > 0 (Out of stock is separate)"""
        # Actually usually 'Low Stock' includes Out of Stock or not? 
        # User says "KPI for Low stock", and we already have "Out of Stock" KPI.
        # So "Low Stock" should probably be items that are low BUT NOT ZERO.
        # Or it can be inclusive. 
        # Implementation decision: Let's make it inclusive or exclusive?
        # Typically Low Stock means <= Threshold. 
        # If I have 0, it IS <= Threshold.
        # But if we display "Out of Stock" separately, Low Stock usually implies "Warning".
        # Let's count items where StockQuantity > 0 AND StockQuantity <= LowStockThreshold
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT COUNT(*) FROM Products WHERE StockQuantity > 0 AND StockQuantity <= LowStockThreshold AND IsActive = 1")
            return cursor.fetchone()[0]

            if query:
                q = f"%{query}%"
                # Filter by IsActive=1 by default unless we implement a show_archived flag??
                # For compliance with "Archive", default view should only show IsActive=1
                # However, the method signature doesn't have `show_archived`. 
                # Let's assume for now we ALWAYS filter active, unless we add a param.
                # Actually, let's add `show_archived` param to method? 
                # Better: The user wants a toggle. So yes, add param or make a separate method? 
                # Modifying signature breaks safety? No, Python kwargs.
                # Let's add `show_archived: bool = False` to the signature in a separate edit, or just inject filtering here.
                # Wait, I cannot change signature easily across all calls? 
                # Let's filter WHERE (IsActive=1 OR ?)
                # Actually, best practice: add `active_only: bool = True`
                pass 
                
            # RE-WRITING METHOD WITH active_only support
    def get_products_paginated(self, page: int, per_page: int, query: str = "", sort_by: str = "ProductID", sort_order: str = "ASC", active_only: bool = True, stock_status: str = "ALL") -> List[Product]:
        offset = (page - 1) * per_page
        products = []
        
        # Allowlist columns for safety
        valid_columns = {
            "ProductID": "ProductID",
            "Brand": "Brand",
            "Description": "Description",
            "SellingPrice": "SellingPrice",
            "StockQuantity": "StockQuantity"
        }
        col = valid_columns.get(sort_by, "ProductID")
        order = "DESC" if sort_order.upper() == "DESC" else "ASC"

        with self.get_connection() as conn:
            base_query = (
                "SELECT * FROM Products"
            )
            
            criteria = ["1=1"]
            params = []
            
            if active_only:
                criteria.append("IsActive = 1")
            
            if query:
                criteria.append("(Description LIKE ? OR Brand LIKE ? OR PartNumber LIKE ? OR Barcode LIKE ?)")
                q = f"%{query}%"
                params.extend([q, q, q, q])
                
            # Filtering Logic
            if stock_status == "OUT":
                criteria.append("StockQuantity <= 0")
            elif stock_status == "LOW":
                 # Low but NOT out? or Inclusive? Low Stock KPI usually implies needs attention.
                 # Let's match the KPI logic: >0 and <= Threshold
                 criteria.append("StockQuantity > 0 AND StockQuantity <= LowStockThreshold")
            
            where_clause = " AND ".join(criteria)
            
            sql = f"{base_query} WHERE {where_clause} ORDER BY {col} {order} LIMIT ? OFFSET ?"
            params.extend([per_page, offset])
            
            cursor = conn.execute(sql, params)
            for row in cursor:
                products.append(self._map_row_to_product(row))
        return products

    def add_product(self, product: Product):
        with self.get_connection() as conn:
            now = datetime.now().isoformat()
            conn.execute(
                """
                INSERT INTO Products (Barcode, PartNumber, Brand, Description, Volume, Type, Application, 
                PurchaseCost, SellingPrice, StockQuantity, Notes, LowStockThreshold, DateCreated, DateModified,
                CategoryID, ReorderPoint, TargetStock, ImagePath, IsActive) 
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                """,
                (product.barcode, product.part_number, product.brand, product.description, product.volume, 
                 product.type, product.application, product.purchase_cost, product.selling_price, 
                 product.stock_quantity, product.notes, product.low_stock_threshold, now, now,
                 product.category_id, product.reorder_point, product.target_stock, product.image_path,
                 1 if product.is_active else 0)
            )
            
            new_id = conn.execute("SELECT last_insert_rowid()").fetchone()[0]
            if hasattr(self, 'log_action'):
                self.log_action(new_id, 0, "CREATE", product.stock_quantity, "Initial Import/Creation", 0, product.stock_quantity, conn=conn)

    def update_product(self, product: Product):
        with self.get_connection() as conn:
            now = datetime.now().isoformat()
            conn.execute(
                """
                UPDATE Products SET
                Barcode = ?, PartNumber = ?, Brand = ?, Description = ?, Volume = ?, Type = ?, Application = ?, 
                PurchaseCost = ?, SellingPrice = ?, StockQuantity = ?, Notes = ?, LowStockThreshold = ?, DateModified = ?,
                CategoryID = ?, ReorderPoint = ?, TargetStock = ?, ImagePath = ?, IsActive = ?
                WHERE ProductID = ?
                """,
                (product.barcode, product.part_number, product.brand, product.description, product.volume, 
                 product.type, product.application, product.purchase_cost, product.selling_price, 
                 product.stock_quantity, product.notes, product.low_stock_threshold, now, 
                 product.category_id, product.reorder_point, product.target_stock, product.image_path,
                 1 if product.is_active else 0,
                 product.product_id)
            )
            
            if hasattr(self, 'log_action'):
                self.log_action(product.product_id, 0, "EDIT", 0, "Product Details Updated", 0, product.stock_quantity, conn=conn)

    def get_product_by_id(self, product_id: int) -> Optional[Product]:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Products WHERE ProductID = ?", (product_id,))
            row = cursor.fetchone()
            if row:
                return self._map_row_to_product(row)
        return None

    def get_product_by_barcode(self, barcode: str) -> Optional[Product]:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Products WHERE Barcode = ?", (barcode,))
            row = cursor.fetchone()
            if not row:
                cursor = conn.execute("SELECT * FROM Products WHERE PartNumber = ?", (barcode,))
                row = cursor.fetchone()
            if row:
                return self._map_row_to_product(row)
        return None

    def delete_product(self, product_id: int) -> bool:
        """
        Deletes a product. Returns True if successful, False if foreign key constraint fails (e.g. has sales).
        """
        try:
            with self.get_connection() as conn:
                conn.execute("DELETE FROM Products WHERE ProductID = ?", (product_id,))
            return True
        except Exception as e:
            # Likely IntegrityError due to SaleItems or InventoryTransactions referencing this ID
            print(f"Delete Error: {e}")
            print(f"Delete Error: {e}")
            return False

    def archive_product(self, product_id: int) -> bool:
        """Sets IsActive = 0"""
        try:
            with self.get_connection() as conn:
                conn.execute("UPDATE Products SET IsActive = 0 WHERE ProductID = ?", (product_id,))
            return True
        except Exception as e:
            print(f"Archive Error: {e}")
            return False

    def restore_product(self, product_id: int) -> bool:
        """Sets IsActive = 1"""
        try:
            with self.get_connection() as conn:
                conn.execute("UPDATE Products SET IsActive = 1 WHERE ProductID = ?", (product_id,))
            return True
        except Exception as e:
            print(f"Restore Error: {e}")
            return False

    def search_products(self, query: str, limit: int = 10) -> List[Product]:
        products = []
        q = f"%{query}%"
        with self.get_connection() as conn:
            cursor = conn.execute(
                "SELECT * FROM Products WHERE (Description LIKE ? OR Brand LIKE ? OR PartNumber LIKE ? OR Barcode LIKE ?) AND IsActive = 1 LIMIT ?", 
                (q, q, q, q, limit)
            )
            for row in cursor:
                products.append(self._map_row_to_product(row))
        return products

    def _map_row_to_product(self, row) -> Product:
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
            category_id=row["CategoryID"] if "CategoryID" in row.keys() else None,
            reorder_point=row["ReorderPoint"] if "ReorderPoint" in row.keys() else 5,
            target_stock=row["TargetStock"] if "TargetStock" in row.keys() else 10,
            image_path=row["ImagePath"] if "ImagePath" in row.keys() else None,
            is_active=bool(row["IsActive"]) if "IsActive" in row.keys() else True
        )

    # --- Supplier Management ---
    def get_all_suppliers(self, active_only: bool = True) -> List[Supplier]:
        suppliers = []
        with self.get_connection() as conn:
            try:
                base_sql = "SELECT * FROM Suppliers"
                if active_only:
                    base_sql += " WHERE IsActive = 1"
                
                cursor = conn.execute(base_sql)
                for row in cursor:
                    suppliers.append(Supplier(
                        supplier_id=row["SupplierID"],
                        name=row["Name"],
                        contact_info=row["ContactInfo"] if row["ContactInfo"] else "",
                        email=row["Email"] if "Email" in row.keys() else "",
                        phone=row["Phone"] if "Phone" in row.keys() else "",
                        is_active=bool(row["IsActive"]) if "IsActive" in row.keys() else True
                    ))
            except Exception: pass
        return suppliers

    def archive_supplier(self, supplier_id: int):
        with self.get_connection() as conn:
            conn.execute("UPDATE Suppliers SET IsActive = 0 WHERE SupplierID = ?", (supplier_id,))

    def restore_supplier(self, supplier_id: int):
        with self.get_connection() as conn:
            conn.execute("UPDATE Suppliers SET IsActive = 1 WHERE SupplierID = ?", (supplier_id,))

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

    def delete_supplier(self, supplier_id: int):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Suppliers WHERE SupplierID = ?", (supplier_id,))

    # --- Customer Management ---
    def get_all_customers(self):
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

    def get_low_stock_products(self) -> list:
        """
        Returns a list of dictionaries containing product details + supplier name
        for items with stock <= low_stock_threshold.
        """
        products = []
        with self.get_connection() as conn:
            # We want detailed info, so we join with Suppliers
            sql = """
                SELECT 
                    p.ProductID, p.Description, p.StockQuantity, p.LowStockThreshold, p.Barcode,
                    s.Name as SupplierName, s.ContactInfo, s.Phone
                FROM Products p
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                WHERE p.IsActive = 1 AND p.StockQuantity <= p.LowStockThreshold
                ORDER BY p.StockQuantity ASC
            """
            cursor = conn.execute(sql)
            for row in cursor:
                # Return dict for easy usage in UI
                products.append({
                    "id": row["ProductID"],
                    "description": row["Description"],
                    "stock": row["StockQuantity"],
                    "threshold": row["LowStockThreshold"],
                    "barcode": row["Barcode"],
                    "supplier": row["SupplierName"] or "Unknown",
                    "contact": row["ContactInfo"] or "",
                    "phone": row["Phone"] or ""
                })
        return products
