import sqlite3
import random
import os

def update_stock():
    # Path is relative to where we run it (New_Version_Python)
    db_path = "inventory.db"
    
    if not os.path.exists(db_path):
        print(f"Error: {db_path} not found.")
        return

    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Get all product IDs
    cursor.execute("SELECT ProductID, Description FROM Products")
    products = cursor.fetchall()
    
    if not products:
        print("No products found.")
        return

    random.shuffle(products)
    total = len(products)
    
    # Ratios
    # 10% No Stock
    # 15% Low Stock (1-9)
    # 25% Normal Stock (10-40)
    # 50% High/Majority Stock (50)
    
    count_out = int(total * 0.10)
    count_low = int(total * 0.15)
    count_normal = int(total * 0.25)
    # Remainder is High/50
    
    idx = 0
    updates = []
    
    # 1. Out of Stock
    for _ in range(count_out):
        if idx >= total: break
        pid = products[idx][0]
        updates.append((0, pid))
        idx += 1
        
    # 2. Low Stock (1-9)
    for _ in range(count_low):
        if idx >= total: break
        pid = products[idx][0]
        qty = random.randint(1, 9)
        updates.append((qty, pid))
        idx += 1
        
    # 3. Normal Stock (10-45)
    for _ in range(count_normal):
        if idx >= total: break
        pid = products[idx][0]
        qty = random.randint(12, 45)
        updates.append((qty, pid))
        idx += 1
        
    # 4. Majority (50)
    while idx < total:
        pid = products[idx][0]
        updates.append((50, pid))
        idx += 1
        
    # Execute Batch Update
    cursor.executemany("UPDATE Products SET StockQuantity = ? WHERE ProductID = ?", updates)
    conn.commit()
    conn.close()
    
    print(f"Successfully updated {total} products:")
    print(f"- {count_out} Out of Stock")
    print(f"- {count_low} Low Stock")
    print(f"- {count_normal} Normal Stock")
    print(f"- {total - count_out - count_low - count_normal} at 50 Qty")

if __name__ == "__main__":
    update_stock()
