import customtkinter as ctk
from ui.styles import Colors

class ProductSelectionDialog(ctk.CTkToplevel):
    def __init__(self, parent, matches):
        super().__init__(parent)
        self.title("Select Product")
        self.geometry("600x400")
        self.result = None
        self.matches = matches
        
        self.transient(parent)
        self.grab_set()
        
        ctk.CTkLabel(self, text="Multiple Items Found. Please Select:", font=("Arial", 16, "bold")).pack(pady=10)
        
        # Scrollable list
        self.scroll = ctk.CTkScrollableFrame(self, fg_color="transparent")
        self.scroll.pack(fill="both", expand=True, padx=20, pady=10)
        
        for p in matches:
            self.create_row(p)
            
        ctk.CTkButton(self, text="Cancel", fg_color="transparent", border_width=1, command=self.destroy).pack(pady=10)

    def create_row(self, p):
        row = ctk.CTkFrame(self.scroll, fg_color=Colors.BG_CARD, height=50)
        row.pack(fill="x", pady=2)
        
        # Content
        info = f"{p.brand} - {p.description}"
        sub = f"Barcode: {p.barcode} | Stock: {p.stock_quantity}"
        
        # Text Frame
        tf = ctk.CTkFrame(row, fg_color="transparent")
        tf.pack(side="left", fill="both", expand=True, padx=10)
        ctk.CTkLabel(tf, text=info, font=("Arial", 12, "bold"), anchor="w").pack(fill="x")
        ctk.CTkLabel(tf, text=sub, font=("Arial", 10), text_color="gray", anchor="w").pack(fill="x")
        
        # Select Button
        ctk.CTkButton(row, text="Select", width=80, command=lambda: self.on_select(p)).pack(side="right", padx=10, pady=5)
        
    def on_select(self, p):
        self.result = p
        self.destroy()
