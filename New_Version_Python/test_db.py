from database import DatabaseRepository

def test_connection():
    try:
        print("Initializing Repository...")
        repo = DatabaseRepository(db_path="../inventory.db")
        
        print("Testing User Fetch...")
        admin = repo.get_user_by_username("admin")
        if admin:
             print(f"SUCCESS: Found admin user: {admin.username}")
        else:
             print("WARNING: 'admin' user not found in existing DB.")
             
        print("Testing Product Fetch...")
        products = repo.get_all_products()
        print(f"SUCCESS: Retrieved {len(products)} products.")
        
        if products:
            print(f"Sample Product: {products[0].description} (Stock: {products[0].stock_quantity})")
            
        print("Testing Charts Data...")
        top = repo.get_top_selling_products()
        print(f"SUCCESS: Retrieved top selling products count: {len(top)}")
        
        print("\nALL SYSTEM CHECKS PASSED.")
        
    except Exception as e:
        print(f"\nCRITICAL FAILURE: {e}")

if __name__ == "__main__":
    test_connection()
