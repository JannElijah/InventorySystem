import os
import sqlite3
from PIL import Image, ImageDraw, ImageFont

ASSETS_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "assets", "products")
if not os.path.exists(ASSETS_DIR):
    os.makedirs(ASSETS_DIR)

def generate_placeholder(filename, text, bg_color, text_color="white"):
    try:
        width, height = 300, 300
        img = Image.new('RGB', (width, height), color=bg_color)
        d = ImageDraw.Draw(img)
        
        # Try to use a default font
        try:
            font = ImageFont.truetype("arial.ttf", 36)
        except IOError:
            font = ImageFont.load_default()
            
        # Draw text centered (roughly)
        d.text((width/2, height/2), text, fill=text_color, anchor="mm", font=font)
        
        path = os.path.join(ASSETS_DIR, filename)
        img.save(path)
        print(f"Generated {filename}")
    except Exception as e:
        print(f"Failed to generate {filename}: {e}")

def download_images():
    # Attempt to download failed, so we generate placeholders
    print("Generating placeholder images...")
    generate_placeholder("toyota_atf.jpg", "Toyota\nATF T-IV", "silver", "black")
    generate_placeholder("toyota_logo.png", "TOYOTA", "#EB0A1E", "white")
    generate_placeholder("hyundai_logo.png", "HYUNDAI\nXTeer", "#002C5F", "white")

def update_database():
    print("Updating database...")
    db_path = "inventory.db"
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()
    
    # Get all products
    cursor.execute("SELECT ProductID, Brand, Description FROM Products")
    products = cursor.fetchall()
    
    updates = 0
    for pid, brand, desc in products:
        brand = brand.lower()
        desc = desc.lower()
        
        image_file = None
        
        # Logic to assign images
        if "toyota" in brand:
            if "atf" in desc or "t-iv" in desc:
                image_file = "toyota_atf.jpg"
            else:
                image_file = "toyota_logo.png"
        elif "hyundai" in brand:
            image_file = "hyundai_logo.png"
            
        if image_file:
            full_path = os.path.join(ASSETS_DIR, image_file)
            cursor.execute("UPDATE Products SET ImagePath = ? WHERE ProductID = ?", (full_path, pid))
            updates += 1
            
    conn.commit()
    conn.close()
    print(f"Updated {updates} products with images.")

if __name__ == "__main__":
    download_images()
    update_database()
