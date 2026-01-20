import sys
import os

# Add src to path
sys.path.append(os.getcwd())

try:
    from utils.barcode_generator import BarcodeGenerator
    print("Import successful")
    
    gen = BarcodeGenerator()
    path = gen.generate_label("Test Product", "123456789", 19.99)
    
    if path:
        print(f"Success! PDF generated at: {path}")
    else:
        print("Failed to generate PDF (returned None)")
        
except Exception as e:
    print(f"CRASH: {e}")
    import traceback
    traceback.print_exc()
