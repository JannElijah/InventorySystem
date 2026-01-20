import unittest
import sqlite3
import os
import sys

# Add project root to sys.path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from database.products import ProductRepositoryMixin
from models import Product

# Mock BaseRepository for the Mixin
class MockBaseRepository:
    def __init__(self, db_name=":memory:"):
        self.conn = sqlite3.connect(db_name)
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
                IsActive INTEGER DEFAULT 1, SupplierID INTEGER
            )
        """)

# Helper class combining MockBase and ProductRepositoryMixin
class TestRepository(MockBaseRepository, ProductRepositoryMixin):
    def _map_row_to_product(self, row):
        # Simplified mapping for test
        return Product(
            product_id=row[0],
            barcode=row[1],
            part_number=row[2],
            brand=row[3],
            description=row[4],
            volume=row[5],
            type=row[6],
            application=row[7],
            purchase_cost=row[8],
            selling_price=row[9],
            stock_quantity=row[10],
            notes=row[11],
            low_stock_threshold=row[12],
            date_created=row[13],
            date_modified=row[14],
            category_id=row[15],
            reorder_point=row[16],
            target_stock=row[17],
            image_path=row[18]
        )

class TestInventorySorting(unittest.TestCase):
    def setUp(self):
        self.repo = TestRepository()
        # Seed Data
        # 1. Cheap, Low Stock, "Apple"
        self.repo.add_product(Product(0, "1", "P1", "BrandA", "Apple", "1L", "Type1", "App1", 10.0, 100.0, 5, "", 5, "", "", 1, 10, ""))
        # 2. Expensive, High Stock, "Banana"
        self.repo.add_product(Product(0, "2", "P2", "BrandB", "Banana", "1L", "Type1", "App1", 50.0, 500.0, 50, "", 5, "", "", 1, 10, ""))
        # 3. Mid Price, Mid Stock, "Cherry"
        self.repo.add_product(Product(0, "3", "P3", "BrandC", "Cherry", "1L", "Type1", "App1", 20.0, 200.0, 25, "", 5, "", "", 1, 10, ""))

    def test_sort_by_price_asc(self):
        products = self.repo.get_products_paginated(1, 10, sort_by="SellingPrice", sort_order="ASC")
        self.assertEqual(products[0].description, "Apple") # 100.0
        self.assertEqual(products[1].description, "Cherry") # 200.0
        self.assertEqual(products[2].description, "Banana") # 500.0

    def test_sort_by_price_desc(self):
        products = self.repo.get_products_paginated(1, 10, sort_by="SellingPrice", sort_order="DESC")
        self.assertEqual(products[0].description, "Banana")
        self.assertEqual(products[1].description, "Cherry")
        self.assertEqual(products[2].description, "Apple")

    def test_sort_by_name_asc(self):
        products = self.repo.get_products_paginated(1, 10, sort_by="Description", sort_order="ASC")
        self.assertEqual(products[0].description, "Apple")
        self.assertEqual(products[1].description, "Banana")
        self.assertEqual(products[2].description, "Cherry")

    def test_sort_by_stock_asc(self):
        products = self.repo.get_products_paginated(1, 10, sort_by="StockQuantity", sort_order="ASC")
        self.assertEqual(products[0].stock_quantity, 5) # Apple
        self.assertEqual(products[1].stock_quantity, 25) # Cherry

    def test_invalid_sort_column_safety(self):
        # Should default to ProductID ASC/DESC without crashing
        try:
            products = self.repo.get_products_paginated(1, 10, sort_by="DROP TABLE Products;", sort_order="ASC")
            # If it didn't crash and returned data, it handled it safely (likely defaulted to ID)
            self.assertEqual(len(products), 3)
        except Exception as e:
            self.fail(f"Injection/Invalid sort key caused crash: {e}")

    def test_pagination_with_sort(self):
        # Page 1, Size 1, Price ASC -> Apple
        p1 = self.repo.get_products_paginated(1, 1, sort_by="SellingPrice", sort_order="ASC")
        self.assertEqual(p1[0].description, "Apple")
        
        # Page 2, Size 1, Price ASC -> Cherry
        p2 = self.repo.get_products_paginated(2, 1, sort_by="SellingPrice", sort_order="ASC")
        self.assertEqual(p2[0].description, "Cherry")

    def tearDown(self):
        self.repo.conn.close()

if __name__ == '__main__':
    unittest.main()
