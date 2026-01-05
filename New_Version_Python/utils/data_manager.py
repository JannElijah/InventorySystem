import csv
import os
from typing import List, Dict, Optional
from models import Product

class DataManager:
    @staticmethod
    def export_products_to_csv(products: List[Product], filepath: str) -> bool:
        """Exports a list of product objects to a CSV file."""
        try:
            headers = [
                "product_id", "barcode", "part_number", "brand", 
                "description", "volume", "type", "application", 
                "purchase_cost", "selling_price", "stock_quantity", 
                "notes", "low_stock_threshold"
            ]
            
            with open(filepath, mode='w', newline='', encoding='utf-8') as f:
                writer = csv.DictWriter(f, fieldnames=headers)
                writer.writeheader()
                for p in products:
                    row = {
                        "product_id": p.product_id,
                        "barcode": p.barcode,
                        "part_number": p.part_number,
                        "brand": p.brand,
                        "description": p.description,
                        "volume": p.volume,
                        "type": p.type,
                        "application": p.application,
                        "purchase_cost": p.purchase_cost,
                        "selling_price": p.selling_price,
                        "stock_quantity": p.stock_quantity,
                        "notes": p.notes,
                        "low_stock_threshold": p.low_stock_threshold
                    }
                    writer.writerow(row)
            return True
        except Exception as e:
            print(f"Export Error: {e}")
            return False

    @staticmethod
    def import_products_from_csv(filepath: str) -> List[Dict]:
        """Reads a CSV file and returns a list of product dictionaries."""
        products_data = []
        try:
            with open(filepath, mode='r', newline='', encoding='utf-8') as f:
                reader = csv.DictReader(f)
                for row in reader:
                    # Basic validation/cleaning
                    try:
                        row['purchase_cost'] = float(row.get('purchase_cost', 0))
                        row['selling_price'] = float(row.get('selling_price', 0))
                        row['stock_quantity'] = int(row.get('stock_quantity', 0))
                        row['low_stock_threshold'] = int(row.get('low_stock_threshold', 5))
                        products_data.append(row)
                    except ValueError:
                        print(f"Skipping invalid row: {row}")
                        continue
            return products_data
        except Exception as e:
            print(f"Import Error: {e}")
            return []
