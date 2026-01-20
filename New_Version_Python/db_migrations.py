from database import DatabaseRepository
from utils.logger import logger

class DBMigrator:
    def __init__(self, db: DatabaseRepository):
        self.db = db

    def migrate(self):
        """Checks current schema version and applies updates."""
        try:
            self._ensure_version_table()
            current_version = self._get_version()
            logger.info(f"Current DB Version: {current_version}")
            
            # Migration Steps
            if current_version < 1:
                self._migrate_v1()
            
            if current_version < 2:
                self._migrate_v2()

            if current_version < 3:
                self._migrate_v3()

            if current_version < 4:
                self._migrate_v4()

        except Exception as e:
            logger.error(f"Migration Failed: {e}")

    def _ensure_version_table(self):
        with self.db.get_connection() as conn:
            conn.execute("CREATE TABLE IF NOT EXISTS SchemaVersion (Version INTEGER)")
            # Init if empty
            if conn.execute("SELECT COUNT(*) FROM SchemaVersion").fetchone()[0] == 0:
                conn.execute("INSERT INTO SchemaVersion (Version) VALUES (0)")

    def _get_version(self):
        with self.db.get_connection() as conn:
            return conn.execute("SELECT Version FROM SchemaVersion").fetchone()[0]

    def _set_version(self, version):
        with self.db.get_connection() as conn:
            conn.execute("UPDATE SchemaVersion SET Version = ?", (version,))

    def _migrate_v1(self):
        """Initial baseline or specific fix."""
        logger.info("Applying Migration V1...")
        # (Already handled by db init checks, but explicit here for safety)
        with self.db.get_connection() as conn:
             # CORE TABLES
             conn.execute("""
                CREATE TABLE IF NOT EXISTS Users (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT NOT NULL,
                    LastLogin TEXT,
                    IsActive INTEGER DEFAULT 1
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
                    IsActive INTEGER DEFAULT 1
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
        self._set_version(1)
        logger.info("Migration V1 Complete.")

    def _migrate_v2(self):
        """Phase 1: Deepening Inventory & Financials tables."""
        logger.info("Applying Migration V2 (Advanced Features)...")
        with self.db.get_connection() as conn:
            # 1. New Tables
            conn.execute("""
                CREATE TABLE IF NOT EXISTS Categories (
                    CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    Description TEXT
                )
            """)
            conn.execute("""
                CREATE TABLE IF NOT EXISTS Customers (
                    CustomerID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Phone TEXT,
                    Email TEXT,
                    TotalSpend REAL DEFAULT 0
                )
            """)
            conn.execute("""
                CREATE TABLE IF NOT EXISTS Expenses (
                    ExpenseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Category TEXT,
                    Amount REAL,
                    Date TEXT,
                    Description TEXT
                )
            """)
            
            # 2. Add Columns to Products
            # Check individual columns to avoid error if re-running
            cursor = conn.execute("PRAGMA table_info(Products)")
            cols = [row[1] for row in cursor.fetchall()]
            
            if "CategoryID" not in cols:
                conn.execute("ALTER TABLE Products ADD COLUMN CategoryID INTEGER REFERENCES Categories(CategoryID)")
            if "ReorderPoint" not in cols:
                conn.execute("ALTER TABLE Products ADD COLUMN ReorderPoint INTEGER DEFAULT 5")
            if "TargetStock" not in cols:
                conn.execute("ALTER TABLE Products ADD COLUMN TargetStock INTEGER DEFAULT 10")
            if "ImagePath" not in cols:
                conn.execute("ALTER TABLE Products ADD COLUMN ImagePath TEXT")
                
        self._set_version(2)
        logger.info("Migration V2 Complete.")

    def _migrate_v3(self):
        """Phase 2: Purchasing & Sales Expansion."""
        logger.info("Applying Migration V3 (Purchasing)...")
        with self.db.get_connection() as conn:
            # 1. Purchase Orders Tables
            conn.execute("""
                CREATE TABLE IF NOT EXISTS PurchaseOrders (
                    POID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierID INTEGER,
                    OrderDate TEXT,
                    Status TEXT,
                    TotalCost REAL,
                    FOREIGN KEY(SupplierID) REFERENCES Suppliers(SupplierID)
                )
            """)
            
            conn.execute("""
                CREATE TABLE IF NOT EXISTS POItems (
                    ItemID INTEGER PRIMARY KEY AUTOINCREMENT,
                    POID INTEGER,
                    ProductID INTEGER,
                    QuantityOrdered INTEGER,
                    QuantityReceived INTEGER DEFAULT 0,
                    UnitCost REAL,
                    FOREIGN KEY(POID) REFERENCES PurchaseOrders(POID),
                    FOREIGN KEY(ProductID) REFERENCES Products(ProductID)
                )
            """)
            
            # 2. Update Suppliers Table (Ensure Email/Phone)
            # Check if table exists first (it should)
            cursor = conn.execute("PRAGMA table_info(Suppliers)")
            cols = [row[1] for row in cursor.fetchall()]
            
            # If Suppliers table missing (legacy issue), create it
            if not cols:
                 conn.execute("""
                    CREATE TABLE IF NOT EXISTS Suppliers (
                        SupplierID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL UNIQUE,
                        ContactInfo TEXT,
                        Email TEXT,
                        Phone TEXT
                    )
                """)
            else:
                 if "Email" not in cols:
                     conn.execute("ALTER TABLE Suppliers ADD COLUMN Email TEXT")
                 if "Phone" not in cols:
                     conn.execute("ALTER TABLE Suppliers ADD COLUMN Phone TEXT")
        
        self._set_version(3)
        logger.info("Migration V3 Complete.")

    def _migrate_v4(self):
        """Phase 6: Advanced Tracking (Audit Trail)."""
        logger.info("Applying Migration V4 (Audit Trail)...")
        with self.db.get_connection() as conn:
            conn.execute("""
                CREATE TABLE IF NOT EXISTS AuditLog (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductID INTEGER,
                    UserID INTEGER,
                    ActionType TEXT, -- 'SALE', 'RESTOCK', 'EDIT', 'ADJUST'
                    QuantityChange INTEGER,
                    PreviousStock INTEGER,
                    NewStock INTEGER,
                    Reason TEXT,
                    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY(ProductID) REFERENCES Products(ProductID),
                    FOREIGN KEY(UserID) REFERENCES Users(UserID)
                )
            """)
        self._set_version(4)
        logger.info("Migration V4 Complete.")
