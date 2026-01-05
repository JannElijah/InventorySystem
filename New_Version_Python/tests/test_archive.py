
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
        self.conn.execute("""
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

class TestProductRepo(MockBaseRepository, ProductRepositoryMixin):
    pass

class TestArchive(unittest.TestCase):
    def setUp(self):
        self.repo = TestProductRepo()
        # Add Product
        self.repo.add_product(Product(0,"","","","TestItem","","",0,0,10,"",5,"","",None,5,10,"",True))
        self.pid = self.repo.get_all_products()[0].product_id

    def test_default_view_active(self):
        # Default view should show 1 item
        prods = self.repo.get_products_paginated(1, 10, active_only=True)
        self.assertEqual(len(prods), 1)

    def test_archive_hides_item(self):
        # Archive it
        self.repo.archive_product(self.pid)
        
        # Default view should show 0 items
        prods = self.repo.get_products_paginated(1, 10, active_only=True)
        self.assertEqual(len(prods), 0)

    def test_show_archived_shows_item(self):
        self.repo.archive_product(self.pid)
        # Disable active_only filter
        prods = self.repo.get_products_paginated(1, 10, active_only=False)
        # Note: Logic in get_products_paginated was:
        # if active_only criteria.append("IsActive = 1")
        # else: It shows ALL (both active and inactive).
        # We need to verify if user meant "Show ONLY Archived" or "Show All (including Archived)".
        # Typically "Show Archived" toggle means "Include Archived" or "Switch to Archive View".
        # My implementation was `active_only=not self.show_archived`. 
        # So if show_archived is True, active_only is False -> Shows ALL.
        
        # Let's verify our query construction logic in `products.py`
        # if active_only: criteria.append("IsActive=1") -> WHERE IsActive=1
        # if not active_only: criteria not appended -> WHERE 1=1 -> Shows Active AND Inactive.
        
        self.assertEqual(len(prods), 1)

    def test_restore_item(self):
        self.repo.archive_product(self.pid)
        self.repo.restore_product(self.pid)
        prods = self.repo.get_products_paginated(1, 10, active_only=True)
        self.assertEqual(len(prods), 1)

    def tearDown(self):
        self.repo.conn.close()

if __name__ == '__main__':
    unittest.main()
