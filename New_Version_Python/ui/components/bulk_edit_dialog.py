import customtkinter as ctk
from ui.styles import Colors, Dimens

class BulkEditDialog(ctk.CTkToplevel):
    def __init__(self, parent, count):
        super().__init__(parent)
        self.title("Bulk Edit Products")
        self.geometry("400x450")
        self.resizable(False, False)
        self.transient(parent)
        self.grab_set()
        
        self.configure(fg_color=Colors.BG_CARD)
        
        self.result = None
        
        # Title
        ctk.CTkLabel(self, text=f"Editing {count} Items", font=Dimens.heading_m(None), 
                     text_color=Colors.TEXT_PRIMARY).pack(pady=20)
        
        # Field Selection
        ctk.CTkLabel(self, text="Select Field to Update:", text_color=Colors.TEXT_SECONDARY).pack(pady=(10, 5))
        self.cmb_field = ctk.CTkComboBox(self, values=[
            "Brand", "Category/Type", "Volume", "Stock Quantity", "Low Stock Threshold", "Reorder Point", "Notes"
        ], width=250, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_field.pack(pady=5)
        self.cmb_field.set("Brand")
        
        # Value Input
        ctk.CTkLabel(self, text="New Value:", text_color=Colors.TEXT_SECONDARY).pack(pady=(20, 5))
        self.entry_value = ctk.CTkEntry(self, width=250, placeholder_text="Enter new value...")
        self.entry_value.pack(pady=5)
        
        # Helper text
        self.lbl_hint = ctk.CTkLabel(self, text="* Numeric fields must contain valid numbers", 
                                     font=("Arial", 10), text_color=Colors.TEXT_SECONDARY)
        self.lbl_hint.pack(pady=5)

        # Buttons
        btn_frame = ctk.CTkFrame(self, fg_color="transparent")
        btn_frame.pack(pady=20)
        
        ctk.CTkButton(btn_frame, text="Apply Changes", command=self.on_apply, 
                      fg_color=Colors.SUCCESS, text_color=Colors.TEXT_ON_NEON).pack(side="left", padx=10)
        ctk.CTkButton(btn_frame, text="Cancel", command=self.on_cancel, 
                      fg_color=Colors.BG_HOVER).pack(side="left", padx=10)
        
        # Map friendly names to DB fields
        self.field_map = {
            "Brand": "brand",
            "Category/Type": "type",
            "Volume": "volume",
            "Stock Quantity": "stock_quantity",
            "Low Stock Threshold": "low_stock_threshold",
            "Reorder Point": "reorder_point",
            "Notes": "notes"
        }

    def on_apply(self):
        field = self.cmb_field.get()
        value = self.entry_value.get().strip()
        
        if not value and field not in ["Notes"]: # Notes can be cleared
            ctk.CTkLabel(self, text="Value is required!", text_color=Colors.DANGER).pack()
            return

        db_field = self.field_map.get(field)
        
        # Simple validation
        if db_field in ["stock_quantity", "low_stock_threshold", "reorder_point"]:
            if not value.isdigit():
                 ctk.CTkLabel(self, text="Must be a whole number!", text_color=Colors.DANGER).pack()
                 return
        
        self.result = (db_field, value)
        self.destroy()

    def on_cancel(self):
        self.destroy()
