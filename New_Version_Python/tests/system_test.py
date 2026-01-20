import sys
import os

# Add parent dir to path
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from database import DatabaseRepository
from utils.data_manager import DataManager
from utils.backup_manager import BackupManager
from models import Product

def test_system():
    print("Initializing DB...")
    # Copy existing DB for realistic testing
    import shutil
    if os.path.exists("inventory.db"):
        shutil.copy2("inventory.db", "test_inventory.db")
        print("Copied production DB for testing.")
    else:
        print("Warning: inventory.db not found, creating empty (tests might fail table checks)")
        
    db = DatabaseRepository("test_inventory.db")
    
    print("\n--- Testing Analytics Queries ---")
    try:
        vol = db.get_daily_sales_volume(30)
        print(f"Daily Volume (30d): {len(vol)} points")
        
        brands = db.get_sales_by_brand(30)
        print(f"Brand Revenue (30d): {len(brands)} points")
        
        top = db.get_top_selling_products(by_revenue=True)
        print(f"Top Products (Rev): {len(top)} items")
        
        val = db.get_inventory_value_by_brand()
        print(f"Brand Value: {len(val)} points")
        
        cat_val = db.get_inventory_value_by_category()
        print(f"Category Value: {len(cat_val)} points")
        
        stats = db.get_sales_summary_stats(30)
        print(f"Sales Stats: {stats}")

        print("Analytics: PASS")
    except Exception as e:
        print(f"Analytics: FAIL - {e}")
        import traceback
        traceback.print_exc()

    print("\n--- Testing Alerts ---")
    try:
        cnt = db.get_low_stock_count()
        print(f"Low Stock Count: {cnt}")
        print("Alerts: PASS")
    except Exception as e:
        print(f"Alerts: FAIL - {e}")

    print("\n--- Testing Export ---")
    try:
        # Use a real product query from DB if possible, or construct carefully
        products = db.get_all_products()
        if not products:
             # Construct dummy if empty DB
             # Product(product_id, barcode, part_number, brand, description, volume, type, application, 
             # purchase_cost, selling_price, stock_quantity, notes, low_stock_threshold, date_created, date_modified, ...)
             p1 = Product(1, "123", "P1", "BrandA", "Desc", "1L", "Type", "App", 10.0, 20.0, 5, "Note", 10, "2023", "2023")
             res = DataManager.export_products_to_csv([p1], "test_export.csv")
        else:
             res = DataManager.export_products_to_csv(products[:5], "test_export.csv")
             
        if res and os.path.exists("test_export.csv"):
            print("Export: PASS")
            # os.remove("test_export.csv") # Keep for manual check if needed
        else:
             print("Export: FAIL (No file)")
    except Exception as e:
        print(f"Export: FAIL - {e}")

    print("\n--- Testing Transactions (Stock Out) ---")
    try:
        # Create a dummy sale
        # process_sale(self, customer_name, items, user_id, date=None)
        # item dict: {'product_id', 'quantity', 'unit_price', 'total'}
        
        # Need a valid product ID. Let's find one or create one.
        p_ids = db.get_all_product_ids()
        if not p_ids:
             print("Transaction: SKIP (No products)")
        else:
            pid = p_ids[0]
            items = [{'product_id': pid, 'quantity': 1, 'unit_price': 100, 'total': 100}]
            
            # Test with Custom Name
            sale_id = db.process_sale("Test Customer", items, 1)
            
            # Verify in DB
            with db.get_connection() as conn:
                res = conn.execute("SELECT DeliverTo FROM Sales WHERE SaleID = ?", (sale_id,)).fetchone()
                if res and res[0] == "Test Customer":
                    print("Transaction (Customer Name): PASS")
                else:
                    print(f"Transaction (Customer Name): FAIL - Got {res[0] if res else 'None'}")
                    
    except Exception as e:
        print(f"Transaction: FAIL - {e}")

    print("\n--- Testing Backup ---")
    try:
        # Create dummy db file if not exists
        if not os.path.exists("test_inventory.db"):
            with open("test_inventory.db", "w") as f: f.write("dummy")
            
        res = BackupManager.perform_backup("test_inventory.db", "test_backups")
        if res:
             print("Backup: PASS")
             # cleanup
        else:
             print("Backup: FAIL")
    except Exception as e:
        print(f"Backup: FAIL - {e}")
        
    # Cleanup
    try:
        if os.path.exists("test_inventory.db"): os.remove("test_inventory.db")
        if os.path.exists("test_backups"): 
            import shutil
            shutil.rmtree("test_backups")
    except: pass

if __name__ == "__main__":
    test_system()
