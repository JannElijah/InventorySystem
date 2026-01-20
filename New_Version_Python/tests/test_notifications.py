
import unittest
import sqlite3
import os
import sys
from datetime import datetime

# Add project root to sys.path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from database import DatabaseRepository
from models import Sale, SaleItem, Product

# Create a test-specific DB
TEST_DB = "test_notifications.db"

class TestSmartNotifications(unittest.TestCase):
    def setUp(self):
        if os.path.exists(TEST_DB):
            os.remove(TEST_DB)
        self.db = DatabaseRepository(TEST_DB)
        self.db.initialize_database()
        
        # Manually create tables that are expected to exist
        with self.db.get_connection() as conn:
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
            conn.execute("CREATE TABLE IF NOT EXISTS Sales (SaleID INTEGER PRIMARY KEY, DeliverTo TEXT, SaleDate TEXT)")
            conn.execute("CREATE TABLE IF NOT EXISTS SaleItems (SaleItemID INTEGER PRIMARY KEY, SaleID INTEGER, ProductID INTEGER, Quantity INTEGER, UnitPrice REAL, LineTotal REAL)")
            conn.execute("CREATE TABLE IF NOT EXISTS Transactions (TransactionID INTEGER PRIMARY KEY, ProductID INTEGER, TransactionType TEXT, QuantityChange INTEGER, StockBefore INTEGER, StockAfter INTEGER, TransactionDate TEXT, SaleItemID INTEGER, UserID INTEGER, SupplierID INTEGER)")
            conn.execute("CREATE TABLE IF NOT EXISTS AuditLog (LogID INTEGER PRIMARY KEY, ProductID INTEGER, UserID INTEGER, ActionType TEXT, QuantityChange INTEGER, PreviousStock INTEGER, NewStock INTEGER, Reason TEXT, Timestamp TEXT DEFAULT CURRENT_TIMESTAMP)")

        # Add Product with Threshold=10
        # Product(product_id, barcode, part_number, brand, description, volume, type, application, purchase_cost, selling_price, stock_quantity, ...)
        # We use strict positional args up to stock_quantity:
        # 0: id
        # 1: barcode
        # 2: part
        # 3: brand
        # 4: desc
        # 5: vol
        # 6: type
        # 7: app
        # 8: cost
        # 9: price
        # 10: stock
        self.db.add_product(Product(0, "111", "PN", "Brand", "TestItem", "", "", "", 0.0, 100.0, 
                                    stock_quantity=15, 
                                    notes="",
                                    low_stock_threshold=10, 
                                    date_created="", date_modified="", category_id=None))
        
        self.pid = self.db.get_product_by_barcode("111").product_id

    def test_notification_trigger(self):
        # 1. Sell 6 items -> Stock becomes 9 (Below 10)
        # We need to construct a Sale object
        item = SaleItem(self.pid, 6, 10.0, 60.0)
        sale = Sale("Customer", datetime.now(), [item])
        
        self.db.process_complete_sale(sale, 1)
        
        # 2. Check Notifications
        repo_notifs = self.db.get_notifications()
        self.assertTrue(len(repo_notifs) > 0, "Notification should be created")
        self.assertIn("Low Stock Warning", repo_notifs[0].message)
        self.assertIn("9 remaining", repo_notifs[0].message)

    def test_no_spam(self):
        # Trigger first notification
        item = SaleItem(self.pid, 6, 10.0, 60.0) # 15 -> 9
        sale = Sale("Customer", datetime.now(), [item])
        self.db.process_complete_sale(sale, 1)
        
        # Trigger again (9 -> 8)
        item2 = SaleItem(self.pid, 1, 10.0, 10.0)
        sale2 = Sale("Customer", datetime.now(), [item2])
        self.db.process_complete_sale(sale2, 1)
        
        # Should still be only 1 notification (since it's unread)
        repo_notifs = self.db.get_notifications()
        self.assertEqual(len(repo_notifs), 1, "Should not spam duplicate notifications")

    def tearDown(self):
        # Clean up
        try:
            self.db.connection.close() # Actually BaseRepository doesn't keep open connection attribute `connection`?
            # It relies on get_connection context manager.
            # But we might have hanging connections if not careful.
            pass
        except: pass
        if os.path.exists(TEST_DB):
            try:
                os.remove(TEST_DB)
            except: pass

if __name__ == '__main__':
    unittest.main()
