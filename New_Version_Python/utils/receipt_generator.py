import os
import tempfile
from reportlab.pdfgen import canvas
from reportlab.lib.units import mm
from datetime import datetime

class ReceiptGenerator:
    def __init__(self, shop_name="Inventory System Shop", header_text="Thank you for shopping!"):
        self.shop_name = shop_name
        self.header_text = header_text

    def generate_receipt(self, transaction_id, items, total_amount, cashier_name="Admin"):
        """
        Generates a PDF receipt for a sale.
        
        :param transaction_id: String or Int ID of the sale
        :param items: List of dicts or objects with 'name', 'qty', 'price', 'total'
        :param total_amount: Float total
        :param cashier_name: String
        :return: Path to the generated PDF
        """
        try:
            temp_dir = tempfile.gettempdir()
            filename = f"receipt_{transaction_id}_{datetime.now().strftime('%Y%m%d%H%M%S')}.pdf"
            pdf_path = os.path.join(temp_dir, filename)

            # Receipt width: 80mm (Standard Thermal), Height: Variable (approx 200mm for now)
            # note: reportlab uses bottom-left origin
            page_width = 80 * mm
            page_height = 200 * mm 
            
            c = canvas.Canvas(pdf_path, pagesize=(page_width, page_height))
            
            # Cursor Y position (Start from top)
            y = page_height - 10 * mm
            
            # Helper to center text
            def draw_centered(text, y_pos, font_size=10, font="Helvetica"):
                c.setFont(font, font_size)
                text_width = c.stringWidth(text, font, font_size)
                x_pos = (page_width - text_width) / 2
                c.drawString(x_pos, y_pos, text)
                return y_pos - (font_size * 1.5) # Return next line Y

            # --- Header ---
            y = draw_centered(self.shop_name, y, 14, "Helvetica-Bold")
            y = draw_centered("1234 Market Street", y, 8)
            y = draw_centered("City, State 54321", y, 8)
            y = draw_centered(f"Tel: 555-0199", y, 8)
            y -= 5 * mm # Spacer
            
            # --- Meta Data ---
            c.setFont("Helvetica", 8)
            c.drawString(5*mm, y, f"Date: {datetime.now().strftime('%Y-%m-%d %H:%M')}")
            y -= 4 * mm
            c.drawString(5*mm, y, f"Ref: #{transaction_id}")
            y -= 4 * mm
            c.drawString(5*mm, y, f"Cashier: {cashier_name}")
            y -= 6 * mm
            
            # --- Divider ---
            c.setDash(1, 2)
            c.line(2*mm, y, page_width - 2*mm, y)
            y -= 5 * mm
            
            # --- Items Header ---
            c.setDash([])
            c.setFont("Helvetica-Bold", 8)
            c.drawString(2*mm, y, "Item")
            c.drawString(45*mm, y, "Qty")
            c.drawString(60*mm, y, "Total")
            y -= 4 * mm
            
            # --- Items Loop ---
            c.setFont("Helvetica", 8)
            for item in items:
                # Item Name (Truncate)
                name = item.get('name', 'Item')[:20]
                qty = str(item.get('qty', 0))
                # Price logic: Assuming 'total' is calculating qty * unit_price
                price = f"${item.get('total', 0.0):.2f}"
                
                c.drawString(2*mm, y, name)
                c.drawString(45*mm, y, qty)
                c.drawString(60*mm, y, price)
                y -= 4 * mm
                
                # Check for page overflow (Simple version: just stop or multipage not handled for simplicity)
                if y < 10*mm: 
                    break

            # --- Divider ---
            y -= 2 * mm
            c.setDash(1, 2)
            c.line(2*mm, y, page_width - 2*mm, y)
            y -= 5 * mm
            
            # --- Totals ---
            c.setDash([])
            c.setFont("Helvetica-Bold", 12)
            c.drawString(2*mm, y, "TOTAL:")
            
            total_str = f"${total_amount:.2f}"
            total_width = c.stringWidth(total_str, "Helvetica-Bold", 12)
            c.drawString(page_width - total_width - 5*mm, y, total_str)
            y -= 10 * mm
            
            # --- Footer ---
            y = draw_centered(self.header_text, y, 10, "Helvetica-Oblique")
            y = draw_centered("No Refunds without Receipt", y, 8)
            
            c.save()
            return pdf_path

        except Exception as e:
            print(f"Receipt Generation Error: {e}")
            return None
