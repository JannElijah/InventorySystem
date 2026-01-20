
import unittest
import os
import sys
import csv
from datetime import datetime

# Add project root to sys.path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from database import DatabaseRepository
from models import Product, Sale, SaleItem, User
from db_migrations import DBMigrator

TEST_DB = "test_scenarios.db"
CSV_FILE = "test_scenario_export.csv"

class TestCommonScenarios(unittest.TestCase):
    def setUp(self):
        if os.path.exists(TEST_DB): os.remove(TEST_DB)
        if os.path.exists(CSV_FILE): os.remove(CSV_FILE)
        
        self.db = DatabaseRepository(TEST_DB)
        
        # Use Migrator to ensure consistent schema (Fixes missing table errors)
        migrator = DBMigrator(self.db)
        # We need to ensure base tables exist first if migrator relies on them or creates them
        # Actually, since core.py doesn't create Products, we need a base schema creation here OR in V0 migration.
        # Let's use the manual creation as V0 then migrate.
        
        with self.db.get_connection() as conn:
            # Base Tables (mimicking legacy state or V0)
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
            # Create others needed for Migrator to not fail or to work on
            conn.execute("CREATE TABLE IF NOT EXISTS Sales (SaleID INTEGER PRIMARY KEY, DeliverTo TEXT, SaleDate TEXT)")
            conn.execute("CREATE TABLE IF NOT EXISTS SaleItems (SaleItemID INTEGER PRIMARY KEY, SaleID INTEGER, ProductID INTEGER, Quantity INTEGER, UnitPrice REAL, LineTotal REAL)")
            conn.execute("CREATE TABLE IF NOT EXISTS Transactions (TransactionID INTEGER PRIMARY KEY, ProductID INTEGER, TransactionType TEXT, QuantityChange INTEGER, StockBefore INTEGER, StockAfter INTEGER, TransactionDate TEXT, SaleItemID INTEGER, UserID INTEGER, SupplierID INTEGER)")
            
        # Now run migrations to apply V2/V3 etc (e.g. AuditLog, new columns)
        migrator.migrate()

    def test_full_workflow(self):
        """
        Scenario:
        1. Admin creates a product.
        2. Cashier sells product -> Triggers Low Stock Notification.
        3. Manager exports inventory -> Verifies Stock is updated.
        4. Admin archives the product.
        """
        
        # 1. Admin Adds Product
        # Start with 10 items, Threshold is 5
        prod = Product(0, "SCENARIO_1", "PN_S1", "BrandZ", "Scenario Item", "", "", "", 10.0, 20.0, 10, "", 5, "", "", None)
        self.db.add_product(prod)
        p_id = self.db.get_product_by_barcode("SCENARIO_1").product_id
        
        # 2. Cashier Sells 6 items (10 -> 4)
        # This crosses the threshold (5) -> Should Notify
        item = SaleItem(p_id, 6, 20.0, 120.0)
        sale = Sale("Customer A", datetime.now(), [item])
        self.db.process_complete_sale(sale, 1) # UserID 1
        
        # Check Stock
        updated_p = self.db.get_product_by_id(p_id)
        self.assertEqual(updated_p.stock_quantity, 4, "Stock should be reduced to 4")
        
        # Check Notification
        notifs = self.db.get_notifications()
        self.assertTrue(len(notifs) > 0, "Low stock notification should exist")
        self.assertIn("Scenario Item", notifs[0].message)
        
        # 3. Manager Exports Inventory
        # Simulate Export Logic
        all_products = self.db.get_all_products()
        with open(CSV_FILE, 'w', newline='', encoding='utf-8') as f:
            writer = csv.writer(f)
            writer.writerow(["ID", "Barcode", "Description", "Stock", "Status"])
            for p in all_products:
                status = "Active" if p.is_active else "Archived"
                writer.writerow([p.product_id, p.barcode, p.description, p.stock_quantity, status])
                
        # Verify Export Content
        with open(CSV_FILE, 'r', encoding='utf-8') as f:
            lines = list(csv.reader(f))
            # Header + 1 Product
            self.assertEqual(len(lines), 2)
            # Check Stock in CSV is 4
            self.assertEqual(lines[1][3], "4", "CSV should reflect updated stock")
            # Check Status is Active
            self.assertEqual(lines[1][4], "Active")

        # 4. Admin Archives Product
        self.db.archive_product(p_id)
        
        # Verify Archive
        archived_p = self.db.get_product_by_id(p_id)
        self.assertFalse(archived_p.is_active, "Product should be inactive")
        
        # Verify it disappears from default search (logic check)
        # Assuming get_products_paginated defaults to active_only=True usually, or we verify the filter method
        # Let's verify via get_all_products(active_only=True) if it exists, or simulated query
        # The repository method `get_products_paginated` supports `active_only`
        active_list = self.db.get_products_paginated(1, 50, active_only=True)
        self.assertEqual(len(active_list), 0, "Archived item should not appear in active list")
        
        # But appears in full list
        full_list = self.db.get_products_paginated(1, 50, active_only=False)
        self.assertEqual(len(full_list), 1, "Archived item should appear when filter is off")

    def tearDown(self):
        # self.db.connection.close() # Not needed/available
        if os.path.exists(TEST_DB):
            try: os.remove(TEST_DB)
            except: pass
        if os.path.exists(CSV_FILE):
             try: os.remove(CSV_FILE)
             except: pass

if __name__ == '__main__':
    unittest.main()
