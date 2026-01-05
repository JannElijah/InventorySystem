
import sqlite3
import contextlib
import os
from datetime import datetime
from utils.app_context import get_app_path

class BaseRepository:
    def __init__(self, db_path: str = None):
        if db_path is None:
            # Default to checking "inventory.db" in the PARENT directory of this package
            # original database.py was in root, so inventory.db is in root.
            # this file is in database/core.py, so parent is database/, parent of that is root.
            # Default to checking "inventory.db" in the PARENT directory of this package
            # original database.py was in root, so inventory.db is in root.
            # this file is in database/core.py, so parent is database/, parent of that is root.
            base_dir = get_app_path()
            self.db_path = os.path.join(base_dir, "inventory.db")
        else:
            self.db_path = db_path
            
        self.initialize_database()

    @contextlib.contextmanager
    def get_connection(self):
        # Increased timeout to 20s to handle "database is locked" during heavy IO
        conn = sqlite3.connect(self.db_path, timeout=20.0)
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
        # Ensure WAL mode is on
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
                
                # --- CORE TABLES (Enforce Existence) ---
                conn.execute("""
                    CREATE TABLE IF NOT EXISTS Users (
                        UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT UNIQUE NOT NULL,
                        PasswordHash TEXT NOT NULL,
                        Role TEXT NOT NULL, 
                        LastLogin TEXT,
                        IsActive INTEGER DEFAULT 1,
                        ContactNumber TEXT
                    )
                """)
                conn.execute("""
                    CREATE TABLE IF NOT EXISTS Products (
                        ProductID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Barcode TEXT UNIQUE,
                        PartNumber TEXT,
                        Brand TEXT,
                        Description TEXT NOT NULL,
                        Volume TEXT,
                        Type TEXT,
                        Application TEXT,
                        PurchaseCost REAL DEFAULT 0,
                        SellingPrice REAL DEFAULT 0,
                        StockQuantity INTEGER DEFAULT 0,
                        Notes TEXT,
                        LowStockThreshold INTEGER DEFAULT 5,
                        DateCreated TEXT,
                        DateModified TEXT,
                        IsActive INTEGER DEFAULT 1,
                        CategoryID INTEGER,
                        ReorderPoint INTEGER DEFAULT 5,
                        TargetStock INTEGER DEFAULT 10,
                        ImagePath TEXT,
                        SupplierID INTEGER
                    )
                """)  
                conn.execute("""
                    CREATE TABLE IF NOT EXISTS Sales (
                        SaleID INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerName TEXT, 
                        SaleDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        TotalAmount REAL, 
                        PaymentMethod TEXT,
                        UserID INTEGER,
                        FOREIGN KEY(UserID) REFERENCES Users(UserID)
                    )
                """)
                conn.execute("""
                    CREATE TABLE IF NOT EXISTS SaleItems (
                        SaleItemID INTEGER PRIMARY KEY AUTOINCREMENT,
                        SaleID INTEGER,
                        ProductID INTEGER,
                        Quantity INTEGER,
                        UnitPrice REAL,
                        LineTotal REAL,
                        FOREIGN KEY(SaleID) REFERENCES Sales(SaleID),
                        FOREIGN KEY(ProductID) REFERENCES Products(ProductID)
                    )
                """)
                conn.execute("""
                    CREATE TABLE IF NOT EXISTS Transactions (
                        TransactionID INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductID INTEGER,
                        TransactionType TEXT,
                        QuantityChange INTEGER,
                        StockBefore INTEGER,
                        StockAfter INTEGER,
                        TransactionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        SaleItemID INTEGER,
                        UserID INTEGER,
                        SupplierID INTEGER,
                        FOREIGN KEY(ProductID) REFERENCES Products(ProductID),
                        FOREIGN KEY(UserID) REFERENCES Users(UserID)
                    )
                """)
                conn.execute("""
                   CREATE TABLE IF NOT EXISTS Suppliers (
                        SupplierID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL UNIQUE,
                        ContactInfo TEXT,
                        Email TEXT,
                        Phone TEXT,
                        IsActive INTEGER DEFAULT 1
                    )
                """)

                # --- AUTO-REPAIR: Add columns if they are missing from existing tables ---
                # This handles cases where the DB exists but is from an older schema version
                try:
                    # check Users.ContactNumber
                    cols = [row[1] for row in conn.execute("PRAGMA table_info(Users)").fetchall()]
                    if "ContactNumber" not in cols:
                        conn.execute("ALTER TABLE Users ADD COLUMN ContactNumber TEXT")
                    
                    # check Products.SupplierID
                    cols = [row[1] for row in conn.execute("PRAGMA table_info(Products)").fetchall()]
                    if "SupplierID" not in cols:
                        conn.execute("ALTER TABLE Products ADD COLUMN SupplierID INTEGER")
                        
                    # check Products.ImagePath (just in case)
                    if "ImagePath" not in cols:
                        conn.execute("ALTER TABLE Products ADD COLUMN ImagePath TEXT")

                    # Check Suppliers.IsActive
                    cols = [row[1] for row in conn.execute("PRAGMA table_info(Suppliers)").fetchall()]
                    if "IsActive" not in cols:
                         conn.execute("ALTER TABLE Suppliers ADD COLUMN IsActive INTEGER DEFAULT 1")

                except Exception as e:
                    print(f"Auto-Repair Warning: {e}")

                # Notifications Table (Core Feature)
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
                
                # Check for DateCreated in Notifications (Migration)
                try:
                    conn.execute("SELECT DateCreated FROM Notifications LIMIT 1")
                except sqlite3.OperationalError:
                    try:
                        print("Migrating Notifications to include DateCreated...")
                        conn.execute("ALTER TABLE Notifications ADD COLUMN DateCreated TEXT")
                    except Exception as e:
                        print(f"Migration Error (Notifications): {e}")

                # Check for UserID in Transactions (Migration)
                try:
                    conn.execute("SELECT UserID FROM Transactions LIMIT 1")
                except sqlite3.OperationalError:
                    try:
                        conn.execute("ALTER TABLE Transactions ADD COLUMN UserID INTEGER REFERENCES Users(UserID)")
                    except Exception:
                        pass # Table might not exist yet

                # ADD INDEXES (Performance Optimization)
                # ADD INDEXES (Performance Optimization)
                # Defined in list below for clean execution
                indexes = [
                    "CREATE INDEX IF NOT EXISTS idx_products_barcode ON Products(Barcode)",
                    "CREATE INDEX IF NOT EXISTS idx_products_brand ON Products(Brand)",
                    "CREATE INDEX IF NOT EXISTS idx_products_desc ON Products(Description)",
                    "CREATE INDEX IF NOT EXISTS idx_sales_date ON Sales(SaleDate)",
                    "CREATE INDEX IF NOT EXISTS idx_saleitems_product ON SaleItems(ProductID)",
                    "CREATE INDEX IF NOT EXISTS idx_transactions_date ON Transactions(TransactionDate)"
                ]
                for idx_sql in indexes:
                    try:
                        conn.execute(idx_sql)
                    except Exception as e:
                        print(f"Index Warning: {e}")



                # [MIGRATION] Add IsActive to Main Entities
                entities = ["Products", "Suppliers", "Customers"]
                for table in entities:
                    try:
                        conn.execute(f"SELECT IsActive FROM {table} LIMIT 1")
                    except sqlite3.OperationalError:
                        print(f"Migrating {table} -> Adding IsActive...")
                        try:
                            # Default 1 (Active)
                            conn.execute(f"ALTER TABLE {table} ADD COLUMN IsActive INTEGER DEFAULT 1")
                        except Exception as e:
                            print(f"Migration Error ({table}): {e}")

        except Exception as e:
            print(f"DB Init Error: {e}")
            pass

    # --- Settings ---
    def get_setting(self, key: str, default: str = "") -> str:
        with self.get_connection() as conn:
            row = conn.execute("SELECT Value FROM Settings WHERE Key = ?", (key,)).fetchone()
            return row["Value"] if row else default

    def set_setting(self, key: str, value: str):
        with self.get_connection() as conn:
            conn.execute("INSERT OR REPLACE INTO Settings (Key, Value) VALUES (?, ?)", (key, str(value)))
