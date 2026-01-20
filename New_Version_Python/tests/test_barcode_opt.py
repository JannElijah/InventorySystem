
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

class TestBarcodeOpt(unittest.TestCase):
    def setUp(self):
        self.repo = TestProductRepo()
        # Active Product
        self.repo.add_product(Product(0,"123456","PN1","BrandA","ScannerTestItem","","",0,0,10,"",5,"","",None,5,10,"",True))
        # Inactive Product with same Barcode (Shouldn't happen in real life but good to test logic ignores it if we query)
        # Actually logic is: lookup by barcode returns *a* product. 
        # Let's test "Inactive Product" lookup.
        self.repo.add_product(Product(
            product_id=0, barcode="999999", part_number="PN2", brand="BrandB", description="ArchivedItem",
            volume="", type="", application="", purchase_cost=0, selling_price=0, stock_quantity=10,
            notes="", low_stock_threshold=5, date_created="", date_modified="",
            category_id=None, target_stock=10, reorder_point=5, image_path="", is_active=False
        ))

    def test_lookup_active(self):
        p = self.repo.get_product_by_barcode("123456")
        self.assertIsNotNone(p)
        self.assertEqual(p.description, "ScannerTestItem")

    def test_search_ignore_inactive(self):
        # Search for "ArchivedItem"
        # repo.search_products should return empty list because IsActive=1 enforced
        results = self.repo.search_products("ArchivedItem")
        self.assertEqual(len(results), 0)

    def tearDown(self):
        self.repo.conn.close()

if __name__ == '__main__':
    unittest.main()
