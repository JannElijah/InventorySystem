
import threading
import sqlite3
import time
import os
from datetime import datetime

# Setup Test DB
DB_PATH = "test_race_condition.db"
if os.path.exists(DB_PATH):
    os.remove(DB_PATH)

conn = sqlite3.connect(DB_PATH)
conn.execute("CREATE TABLE Products (ProductID INTEGER PRIMARY KEY, StockQuantity INTEGER)")
conn.execute("CREATE TABLE Sales (SaleID INTEGER PRIMARY KEY, DeliverTo TEXT, SaleDate TEXT)")
conn.execute("CREATE TABLE SaleItems (SaleItemID INTEGER PRIMARY KEY, SaleID INTEGER, ProductID INTEGER, Quantity INTEGER, UnitPrice REAL, LineTotal REAL)")
conn.execute("CREATE TABLE Transactions (TransactionID INTEGER PRIMARY KEY, ProductID INTEGER, TransactionType TEXT, QuantityChange INTEGER, StockBefore INTEGER, StockAfter INTEGER, TransactionDate TEXT, SaleItemID INTEGER, UserID INTEGER)")

# Initial Stock = 100
conn.execute("INSERT INTO Products (ProductID, StockQuantity) VALUES (1, 100)")
conn.commit()
conn.close()

# Worker Function
def process_sale(thread_id):
    # Simulate the logic in sales.py
    try:
        # Use a new connection per thread (like the app)
        t_conn = sqlite3.connect(DB_PATH, timeout=10)
        
        # 1. Atomic Update (The Fix)
        t_conn.execute("UPDATE Products SET StockQuantity = StockQuantity - 1 WHERE ProductID = 1")
        
        t_conn.commit()
        t_conn.close()
    except Exception as e:
        print(f"Thread {thread_id} failed: {e}")

# Run concurrent threads
threads = []
print("Starting 50 concurrent sales...")
for i in range(50):
    t = threading.Thread(target=process_sale, args=(i,))
    threads.append(t)
    t.start()

for t in threads:
    t.join()

# Verify
conn = sqlite3.connect(DB_PATH)
final_stock = conn.execute("SELECT StockQuantity FROM Products WHERE ProductID = 1").fetchone()[0]
print(f"Final Stock: {final_stock}")

if final_stock == 50:
    print("SUCCESS: Race Condition Avoided (100 - 50 = 50)")
else:
    print(f"FAILURE: Race Condition Detected (Expected 50, got {final_stock})")

conn.close()
# Cleanup
try:
    os.remove(DB_PATH)
except: pass
