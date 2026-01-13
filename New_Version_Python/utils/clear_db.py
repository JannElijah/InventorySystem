import sqlite3
import os

DB_PATH = "inventory.db"

def clear_data():
    if not os.path.exists(DB_PATH):
        print(f"Database file not found at {DB_PATH}")
        return

    print(f"Connecting to {DB_PATH}...")
    conn = sqlite3.connect(DB_PATH)
    cursor = conn.cursor()

    # Tables to clear (Client Data)
    # We KEEP: Users, Suppliers, Settings (if any)
    tables_to_clear = [
        "Products",
        "Transactions",
        "Sales",
        "SaleItems",
        "PurchaseOrders", # Assuming these exist
        "POItems",
        "Notifications"
    ]

    try:
        # Check tables exist first to avoid errors
        cursor.execute("SELECT name FROM sqlite_master WHERE type='table'")
        existing_tables = [row[0] for row in cursor.fetchall()]

        for table in tables_to_clear:
            if table in existing_tables:
                cursor.execute(f"DELETE FROM {table}")
                # Reset Auto-Increment ID
                cursor.execute("DELETE FROM sqlite_sequence WHERE name=?", (table,))
                print(f"Cleared table: {table}")
            else:
                print(f"Skipped {table} (Not found)")

        # Verify Counts
        print("-" * 20)
        cursor.execute("SELECT Count(*) FROM Users")
        print(f"Users remaining: {cursor.fetchone()[0]}")
        
        cursor.execute("SELECT Count(*) FROM Suppliers")
        print(f"Suppliers remaining: {cursor.fetchone()[0]}")

        conn.commit()
        print("Database cleanup completed successfully.")

    except Exception as e:
        print(f"Error clearing data: {e}")
        conn.rollback()
    finally:
        conn.close()

if __name__ == "__main__":
    clear_data()
