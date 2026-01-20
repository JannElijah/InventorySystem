
import unittest
import sqlite3
import os
import sys
from datetime import datetime, timedelta

# Add project root to sys.path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from database.analytics import AnalyticsRepositoryMixin
from models import ChartDataPoint

# Mock Mixin
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
            CREATE TABLE IF NOT EXISTS Sales (SaleID INTEGER PRIMARY KEY, SaleDate TEXT, DeliverTo TEXT)
        """)
        self.conn.execute("""
            CREATE TABLE IF NOT EXISTS SaleItems (ItemID INTEGER PRIMARY KEY, SaleID INTEGER, ProductID INTEGER, Quantity INTEGER, LineTotal REAL)
        """)

class TestAnalytics(MockBaseRepository, AnalyticsRepositoryMixin):
    pass

class TestSparklineData(unittest.TestCase):
    def setUp(self):
        self.repo = TestAnalytics()
        self.today = datetime.now()
        
        # Helper to add sales
        def add_sale(days_ago, qty, total):
            d_str = (self.today - timedelta(days=days_ago)).strftime("%Y-%m-%d %H:%M:%S")
            cursor = self.repo.conn.execute("INSERT INTO Sales (SaleDate) VALUES (?)", (d_str,))
            sid = cursor.lastrowid
            self.repo.conn.execute("INSERT INTO SaleItems (SaleID, Quantity, LineTotal) VALUES (?, ?, ?)", (sid, qty, total))

        # Add Data
        # Today: 10 items, $100
        add_sale(0, 10, 100)
        # Yesterday: 5 items, $50
        add_sale(1, 5, 50)
        # 3 Days ago: 20 items, $200
        add_sale(3, 20, 200)
        # 9 Days ago: 2 items, $20
        add_sale(9, 2, 20)
        # 11 Days ago (Should be ignored by 10-day window): 100 items
        add_sale(11, 100, 1000)

    def test_sparkline_returns_10_days(self):
        data = self.repo.get_sparkline_data("items", 10)
        self.assertEqual(len(data), 10)
    
    def test_sparkline_values_items(self):
        data = self.repo.get_sparkline_data("items", 10)
        # Index 9 = Today (last element)
        self.assertEqual(data[-1], 10.0) # Today
        self.assertEqual(data[-2], 5.0)  # Yesterday
        self.assertEqual(data[-3], 0.0)  # 2 days ago (gap)
        self.assertEqual(data[-4], 20.0) # 3 days ago

    def test_sparkline_values_revenue(self):
        data = self.repo.get_sparkline_data("revenue", 10)
        self.assertEqual(data[-1], 100.0)
        self.assertEqual(data[-2], 50.0)
        self.assertEqual(data[-4], 200.0)

    def tearDown(self):
        self.repo.conn.close()

if __name__ == '__main__':
    unittest.main()
