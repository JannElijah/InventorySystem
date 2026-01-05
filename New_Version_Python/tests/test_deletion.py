
import unittest
import sqlite3
import os
import sys

# Add project root to sys.path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from database.products import ProductRepositoryMixin
from models import Product

# Mock Mixin
class MockBaseRepository:
    def __init__(self, db_name=":memory:"):
        self.conn = sqlite3.connect(db_name)
        self.conn.row_factory = sqlite3.Row
        self.create_tables()

    def get_connection(self):
        return self.conn
        
    def __enter__(self):
        return self.conn
        
    def __exit__(self, exc_type, exc_val, exc_tb):
        pass

    def create_tables(self):
        self.conn.execute("PRAGMA foreign_keys = ON;")
        self.conn.execute("""
            CREATE TABLE IF NOT EXISTS Products (
                ProductID INTEGER PRIMARY KEY AUTOINCREMENT, 
                Barcode TEXT, PartNumber TEXT, Brand TEXT, Description TEXT, 
                Volume TEXT, Type TEXT, Application TEXT, PurchaseCost REAL, 
                SellingPrice REAL, StockQuantity INTEGER, Notes TEXT, 
                LowStockThreshold INTEGER, DateCreated TEXT, DateModified TEXT, 
                CategoryID INTEGER, ReorderPoint INTEGER, TargetStock INTEGER, ImagePath TEXT
            )
        """)
        # Dependent Table
        self.conn.execute("""
            CREATE TABLE IF NOT EXISTS SaleItems (
                ItemID INTEGER PRIMARY KEY, 
                ProductID INTEGER,
                FOREIGN KEY(ProductID) REFERENCES Products(ProductID) ON DELETE RESTRICT
            )
        """)

class TestProductRepo(MockBaseRepository, ProductRepositoryMixin):
    pass

class TestDeletion(unittest.TestCase):
    def setUp(self):
        self.repo = TestProductRepo()
        # Add Product
        self.repo.add_product(Product(0,"","","","TestItem","","",0,0,10,"",5,"","",None,5,10,""))
        self.pid = self.repo.get_all_products()[0].product_id

    def test_delete_unused_product(self):
        # Should succeed
        success = self.repo.delete_product(self.pid)
        self.assertTrue(success)
        self.assertEqual(len(self.repo.get_all_products()), 0)

    def test_delete_product_with_sales_constraint(self):
        # Add a SaleItem referencing this product
        self.repo.conn.execute("INSERT INTO SaleItems (ProductID) VALUES (?)", (self.pid,))
        
        # Should fail due to Foreign Key RESTRICT (simulated)
        # Note: SQLite defaults usually OFF for FK unless PRAGMA foreign_keys = ON.
        # Our mock setup enabled it.
        success = self.repo.delete_product(self.pid)
        self.assertFalse(success)
        self.assertEqual(len(self.repo.get_all_products()), 1)

    def tearDown(self):
        self.repo.conn.close()

if __name__ == '__main__':
    unittest.main()
