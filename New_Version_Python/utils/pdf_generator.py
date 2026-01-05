from reportlab.lib.pagesizes import letter
from reportlab.pdfgen import canvas
from reportlab.lib.utils import ImageReader
import os
from datetime import datetime
from models import Sale

class PDFReportGenerator:
    @staticmethod
    def open_pdf(path):
        try:
            if os.name == 'nt':
                os.startfile(path)
            else:
                import subprocess
                subprocess.call(('xdg-open', path))
        except:
            pass

    @staticmethod
    def generate_report(title, headers, data, filename_prefix="report"):
        output_dir = "reports"
        if not os.path.exists(output_dir):
            os.makedirs(output_dir)
            
        filename = f"{filename_prefix}_{datetime.now().strftime('%Y%m%d_%H%M%S')}.pdf"
        filepath = os.path.join(output_dir, filename)
        
        try:
            c = canvas.Canvas(filepath, pagesize=letter)
            w, h = letter
            
            # Title
            c.setFont("Helvetica-Bold", 18)
            c.drawString(50, h - 50, title)
            c.setFont("Helvetica", 10)
            c.drawString(50, h - 70, f"Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
            
            # Table Logic (Simplified)
            y = h - 100
            col_width = (w - 100) / len(headers)
            
            # Headers
            c.setFont("Helvetica-Bold", 10)
            for i, header in enumerate(headers):
                c.drawString(50 + (i * col_width), y, str(header))
            y -= 20
            c.line(50, y+15, w-50, y+15)
            
            # Data
            c.setFont("Helvetica", 10)
            for row in data:
                if y < 50:
                    c.showPage()
                    y = h - 50
                
                for i, cell in enumerate(row):
                    c.drawString(50 + (i * col_width), y, str(cell))
                y -= 15
                
            c.save()
            return filepath
        except Exception as e:
            print(f"PDF Gen Error: {e}")
            return None


class PDFGenerator: # For Invoices
    def __init__(self, output_dir="invoices"):
        self.output_dir = output_dir
        if not os.path.exists(output_dir):
            os.makedirs(output_dir)

    def generate_invoice(self, sale: Sale):
        filename = f"Invoice_{datetime.now().strftime('%Y%m%d_%H%M%S')}.pdf"
        filepath = os.path.join(self.output_dir, filename)
        
        c = canvas.Canvas(filepath, pagesize=letter)
        w, h = letter
        
        # Header
        c.setFont("Helvetica-Bold", 24)
        c.drawString(50, h - 50, "INVOICE")
        
        c.setFont("Helvetica", 12)
        c.drawString(50, h - 80, f"Date: {sale.sale_date.strftime('%Y-%m-%d %H:%M')}")
        c.drawString(50, h - 100, f"Customer: {sale.customer_name}")
        c.drawString(400, h - 80, f"Invoice #: {filename.split('.')[0]}")
        
        # Table Header
        y = h - 140
        c.setFont("Helvetica-Bold", 12)
        c.drawString(50, y, "Item")
        c.drawString(300, y, "Qty")
        c.drawString(380, y, "Price")
        c.drawString(480, y, "Total")
        c.line(50, y-5, 550, y-5)
        
        y -= 25
        c.setFont("Helvetica", 12)
        
        for item in sale.items:
            name = item.product.description if item.product else f"Item #{item.product_id}"
            c.drawString(50, y, name[:35])
            c.drawString(300, y, str(item.quantity))
            c.drawString(380, y, f"${item.unit_price:.2f}")
            c.drawString(480, y, f"${item.total:.2f}")
            y -= 20
            
            if y < 100: # New page if needed
                c.showPage()
                y = h - 50
        
        # Total
        c.line(50, y+10, 550, y+10)
        c.setFont("Helvetica-Bold", 14)
        c.drawString(380, y-20, "Grand Total:")
        c.drawString(480, y-20, f"${sale.subtotal:.2f}")
        
        # Footer
        c.setFont("Helvetica", 10)
        c.drawString(50, 50, "Thank you for your business!")
        
        c.save()
        return filepath
