import os
import tempfile
from reportlab.graphics.barcode import code128
from reportlab.graphics.shapes import Drawing
from reportlab.lib.pagesizes import mm
from reportlab.pdfgen import canvas
from reportlab.graphics import renderPDF

class BarcodeGenerator:
    def __init__(self):
        pass

    def generate_label(self, product_name, barcode_value, price):
        """
        Generates a barcode label PDF (approx 50mm x 30mm for thermal printers).
        """
        if not barcode_value or not barcode_value.strip():
            print("Error: Empty barcode value")
            return None
            
        # Code128 Support Check (Basic ASCII)
        if not all(ord(c) < 128 for c in barcode_value):
             print("Error: Barcode contains non-ASCII characters")
             return None

        try:
            temp_dir = tempfile.gettempdir()
            filename = f"label_{barcode_value}.pdf"
            pdf_path = os.path.join(temp_dir, filename)

            # Label Size (Standard Address Label approx 2x1 inch -> 50x25mm)
            # Adjusting to 60mm x 40mm for better readability
            width = 60 * mm
            height = 40 * mm

            c = canvas.Canvas(pdf_path, pagesize=(width, height))
            
            # Draw Product Name (Truncated)
            c.setFont("Helvetica-Bold", 10)
            c.drawCentredString(width / 2, height - 8 * mm, product_name[:25])

            # Draw Barcode
            # Code128 is robust. height=15mm, barWidth=1.2 (adjust for scaling)
            barcode = code128.Code128(barcode_value, barHeight=15*mm, barWidth=1.2)
            
            # Draw directly on canvas
            # Center roughly: x = (width - barcode_width) / 2
            # For simplicity, we assume a reasonable starting X
            barcode.drawOn(c, 5*mm, 15*mm)

            # Draw Human Readable Code
            c.setFont("Helvetica", 10)
            c.drawCentredString(width / 2, 8 * mm, barcode_value)

            # Draw Price
            c.setFont("Helvetica-Bold", 12)
            c.drawCentredString(width / 2, 3 * mm, f"${price:.2f}")

            c.save()
            return pdf_path

        except Exception as e:
            print(f"Barcode Generation Error: {e}")
            return None

    def generate_sheet(self, products):
        """
        Generates a full sheet of barcodes (A4 grid).
        products: list of dicts with 'name', 'code', 'price'
        """
        try:
            from reportlab.lib.pagesizes import A4
            temp_dir = tempfile.gettempdir()
            filename = f"barcode_sheet_{len(products)}.pdf"
            pdf_path = os.path.join(temp_dir, filename)

            c = canvas.Canvas(pdf_path, pagesize=A4)
            width, height = A4
            
            # Grid Config (3 cols x 7 rows = 21 per page)
            cols = 3
            rows = 7
            margin_x = 10 * mm
            margin_y = 15 * mm
            
            # Cell dimensions
            col_width = (width - 2 * margin_x) / cols
            row_height = (height - 2 * margin_y) / rows
            
            current_col = 0
            current_row = 0 
            
            for p in products:
                # Calculate coordinates (Top-Left origin logic for grid, mapped to PDF Bottom-Left)
                # Row 0 is at the top.
                x = margin_x + (current_col * col_width)
                y = height - margin_y - ((current_row + 1) * row_height)
                
                # Center of current cell
                center_x = x + (col_width / 2)
                
                # 1. Product Name (Truncated)
                c.setFont("Helvetica-Bold", 8)
                name = p.get('name', 'Unknown')
                if len(name) > 30: name = name[:27] + "..."
                c.drawCentredString(center_x, y + row_height - 10*mm, name)
                
                # 2. Barcode
                code = p.get('code', '')
                if code:
                    try:
                        # Barcode
                        barcode = code128.Code128(code, barHeight=10*mm, barWidth=1.0)
                        # Center it: approximate width
                        b_width = barcode.width
                        b_x = center_x - (b_width / 2)
                        barcode.drawOn(c, b_x, y + row_height - 22*mm)
                        
                        # 3. Code Text
                        c.setFont("Helvetica", 8)
                        c.drawCentredString(center_x, y + row_height - 26*mm, code)
                    except:
                        c.drawString(x, y, "Invalid Barcode")
                
                # 4. Price
                price = p.get('price', 0)
                if isinstance(price, (int, float)):
                    c.setFont("Helvetica-Bold", 10)
                    c.drawCentredString(center_x, y + row_height - 32*mm, f"${price:.2f}")

                # Move Grid Pointer
                current_col += 1
                if current_col >= cols:
                    current_col = 0
                    current_row += 1
                    
                # New Page if needed
                if current_row >= rows:
                    c.showPage()
                    current_col = 0
                    current_row = 0

            c.save()
            return pdf_path

        except Exception as e:
            print(f"Sheet Generation Error: {e}")
            return None
