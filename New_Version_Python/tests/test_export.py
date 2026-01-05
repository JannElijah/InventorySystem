
import unittest
import csv
import os
import sys

# Add project root to sys.path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from database import DatabaseRepository
from models import Product
from db_migrations import DBMigrator

TEST_DB = "test_export.db"
CSV_FILE = "test_export.csv"

class TestCSVExport(unittest.TestCase):
    def setUp(self):
        if os.path.exists(TEST_DB): os.remove(TEST_DB)
        if os.path.exists(CSV_FILE): os.remove(CSV_FILE)
        
        self.db = DatabaseRepository(TEST_DB)
        self.db.initialize_database()
        
        # Create Tables (Simplified for this test needed?)
        # Actually export relies on get_products_paginated which relies on Products table.
        # Use Migrator to ensure consistent schema
        migrator = DBMigrator(self.db)
        
        with self.db.get_connection() as conn:
            # Base Tables V0
            conn.execute("""
                CREATE TABLE IF NOT EXISTS Products (
                    ProductID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Barcode TEXT, PartNumber TEXT, Brand TEXT, Description TEXT, 
                    Volume TEXT, Type TEXT, Application TEXT, PurchaseCost REAL, 
                    SellingPrice REAL, StockQuantity INTEGER, Notes TEXT, 
                    LowStockThreshold INTEGER, DateCreated TEXT, DateModified TEXT, 
                    CategoryID INTEGER, ReorderPoint INTEGER, TargetStock INTEGER, ImagePath TEXT,
                    IsActive INTEGER DEFAULT 1
                )
            """)
            # Create others needed
            conn.execute("CREATE TABLE IF NOT EXISTS Sales (SaleID INTEGER PRIMARY KEY, DeliverTo TEXT, SaleDate TEXT)")
            conn.execute("CREATE TABLE IF NOT EXISTS SaleItems (SaleItemID INTEGER PRIMARY KEY, SaleID INTEGER, ProductID INTEGER, Quantity INTEGER, UnitPrice REAL, LineTotal REAL)")
            conn.execute("CREATE TABLE IF NOT EXISTS Transactions (TransactionID INTEGER PRIMARY KEY, ProductID INTEGER, TransactionType TEXT, QuantityChange INTEGER, StockBefore INTEGER, StockAfter INTEGER, TransactionDate TEXT, SaleItemID INTEGER, UserID INTEGER, SupplierID INTEGER)")
        
        migrator.migrate()
            
        self.db.add_product(Product(0, "123", "PN1", "BrandX", "Item1", "", "", "", 10.0, 20.0, 5, "", 2, "", "", None))
        self.db.add_product(Product(0, "456", "PN2", "BrandY", "Item2", "", "", "", 15.0, 30.0, 10, "", 2, "", "", None))

    def test_csv_generation(self):
        # Simulate Logic
        products = self.db.get_products_paginated(1, 100)
        
        with open(CSV_FILE, 'w', newline='', encoding='utf-8') as f:
            writer = csv.writer(f)
            writer.writerow(["ID", "Barcode", "Brand", "Description", "Stock"])
            for p in products:
                writer.writerow([p.product_id, p.barcode, p.brand, p.description, p.stock_quantity])
                
        # Verify
        with open(CSV_FILE, 'r', encoding='utf-8') as f:
            reader = csv.reader(f)
            rows = list(reader)
            self.assertEqual(len(rows), 3) # Header + 2 rows
            self.assertEqual(rows[1][3], "Item1") # Description
            self.assertEqual(rows[2][2], "BrandY") # Brand

    def tearDown(self):
        if os.path.exists(TEST_DB): os.remove(TEST_DB)
        if os.path.exists(CSV_FILE): os.remove(CSV_FILE)

if __name__ == '__main__':
    unittest.main()
