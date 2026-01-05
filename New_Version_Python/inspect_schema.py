import sqlite3
import os

db_path = r"D:\InventorySystem_Github\New_Version_Python\inventory.db"
print(f"DB: {db_path}")

try:
    conn = sqlite3.connect(db_path)
    # Don't use Row factory yet, just tuples to see raw
    cursor = conn.cursor()
    
    # Check Table Info
    print("Table Info:")
    cursor.execute("PRAGMA table_info(Suppliers)")
    cols = cursor.fetchall()
    for c in cols:
        print(c)
        
    print("Select one:")
    conn.row_factory = sqlite3.Row # Switch to Row
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM Suppliers LIMIT 1")
    row = cursor.fetchone()
    if row:
        print(f"Keys: {row.keys()}")
    else:
        print("No Rows")
        
except Exception as e:
    print(e)
