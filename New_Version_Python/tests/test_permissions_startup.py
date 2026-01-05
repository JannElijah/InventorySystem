
import unittest
import os
import sys
import customtkinter as ctk

# Add project root to sys.path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from database import DatabaseRepository
from models import User
from ui.inventory_view import InventoryView
from ui.suppliers_view import SuppliersView

TEST_DB = "test_perms.db"

class TestPermissionsStartup(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        # Initialize CTk (headless-ish, just to allow widget creation)
        ctk.set_appearance_mode("Dark")
        cls.root = ctk.CTk()
        cls.root.withdraw() # Hide window

    def setUp(self):
        if os.path.exists(TEST_DB): os.remove(TEST_DB)
        self.db = DatabaseRepository(TEST_DB)
        self.db.initialize_database()
        
        # Manually create tables required for Views to load (since initialize_database is incomplete)
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
            conn.execute("""
                CREATE TABLE IF NOT EXISTS Suppliers (
                    SupplierID INTEGER PRIMARY KEY, 
                    Name TEXT, ContactInfo TEXT, Email TEXT, Phone TEXT, Address TEXT, 
                    IsActive INTEGER DEFAULT 1
                )
            """)
            conn.execute("CREATE TABLE IF NOT EXISTS AuditLog (LogID INTEGER PRIMARY KEY, ProductID INTEGER, UserID INTEGER, ActionType TEXT, QuantityChange INTEGER, PreviousStock INTEGER, NewStock INTEGER, Reason TEXT, Timestamp TEXT DEFAULT CURRENT_TIMESTAMP)")
            conn.execute("CREATE TABLE IF NOT EXISTS Categories (CategoryID INTEGER PRIMARY KEY, Name TEXT)")

    def test_inventory_view_admin(self):
        """Test InventoryView loads for Admin without error."""
        admin = User(1, "admin", "hash", "Admin")
        try:
            view = InventoryView(self.root, self.db, admin)
            # view.pack() # Optional, might trigger more layout logic
        except Exception as e:
            self.fail(f"InventoryView failed to load for Admin: {e}")

    def test_inventory_view_user(self):
        """Test InventoryView loads for User (Cashier) without error."""
        user = User(2, "cashier", "hash", "User")
        try:
            view = InventoryView(self.root, self.db, user)
        except Exception as e:
            self.fail(f"InventoryView failed to load for User: {e}")

    def test_suppliers_view_admin(self):
        """Test SuppliersView loads for Admin without error."""
        admin = User(1, "admin", "hash", "Admin")
        try:
            view = SuppliersView(self.root, self.db, admin)
        except Exception as e:
            self.fail(f"SuppliersView failed to load for Admin: {e}")

    def test_suppliers_view_user(self):
        """Test SuppliersView loads for User (Cashier) without error."""
        user = User(2, "cashier", "hash", "User")
        try:
            view = SuppliersView(self.root, self.db, user)
        except Exception as e:
            self.fail(f"SuppliersView failed to load for User: {e}")

    def tearDown(self):
        if os.path.exists(TEST_DB):
            try:
                os.remove(TEST_DB)
            except:
                pass

    @classmethod
    def tearDownClass(cls):
        cls.root.destroy()

if __name__ == '__main__':
    unittest.main()
