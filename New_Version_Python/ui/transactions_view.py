import customtkinter as ctk
import csv
from tkinter import filedialog, messagebox
from datetime import datetime
from database import DatabaseRepository
from ui.styles import Colors, Dimens

class TransactionHistoryView(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        self.transactions = []
        self.page = 1
        self.limit = 50
        self.total_pages = 1

        # Header
        self.header_frame = ctk.CTkFrame(self, fg_color="transparent")
        self.header_frame.pack(fill="x", pady=(0, 20))

        self.lbl_title = ctk.CTkLabel(self.header_frame, text="Transaction History", font=("Arial", 24, "bold"), text_color=Colors.TEXT_PRIMARY)
        self.lbl_title.pack(side="left")

        self.btn_refresh = ctk.CTkButton(self.header_frame, text="Refresh", width=100, command=self.load_data)
        self.btn_refresh.pack(side="right", padx=5)

        self.btn_export = ctk.CTkButton(self.header_frame, text="Export CSV", width=100, fg_color="#D35400", command=self.export_csv)
        self.btn_export.pack(side="right", padx=5)

        # Filters
        self.filter_frame = ctk.CTkFrame(self, fg_color="transparent")
        self.filter_frame.pack(fill="x", pady=(0, 10))
        
        # Removed Date Filters as requested
        # ctk.CTkLabel(self.filter_frame, text="Start...").pack() 
        
        self.cmb_type = ctk.CTkComboBox(self.filter_frame, values=["All", "Supply", "Delivery"], width=100, command=self.load_data,
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_type.pack(side="left", padx=5)
        
        ctk.CTkLabel(self.filter_frame, text="Sort By:", text_color=Colors.TEXT_PRIMARY).pack(side="left", padx=(10, 5))
        self.cmb_sort = ctk.CTkComboBox(self.filter_frame, values=["Date Newest", "Date Oldest", "Value High-Low", "Value Low-High", "Qty High-Low"], width=140, command=self.load_data,
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_sort.set("Date Newest")
        self.cmb_sort.pack(side="left", padx=5)
        
        self.entry_search = ctk.CTkEntry(self.filter_frame, placeholder_text="Search Product...", 
                                         fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_search.pack(side="left", fill="x", expand=True, padx=5)
        self.entry_search.bind("<KeyRelease>", self.on_search_key)
        
        # Debounce job
        self._search_job = None

        # Grid Header
        self.grid_header = ctk.CTkFrame(self, height=40, fg_color=Colors.BG_HOVER)
        self.grid_header.pack(fill="x", pady=(0, 2))
        
        # ID, Barcode, Desc, Type, Value, Qty, Cust, Supp, Date
        self.cols = [
            ("ID", 50), ("Barcode", 80), ("Description", 200), ("Type", 80), 
            ("Value", 100), ("Qty", 60), ("Customer", 120), ("Supplier", 120), ("Date", 140)
        ]
        
        for i, (text, width) in enumerate(self.cols):
            self.grid_header.grid_columnconfigure(i, weight=1 if text == "Description" else 0)
            lbl = ctk.CTkLabel(self.grid_header, text=text, width=width if text != "Description" else 0, font=("Arial", 12, "bold"), anchor="w", text_color=Colors.TEXT_PRIMARY)
            lbl.grid(row=0, column=i, padx=5, pady=5, sticky="ew")

        # Scrollable List
        self.scroll_frame = ctk.CTkScrollableFrame(self, fg_color="transparent")
        self.scroll_frame.pack(fill="both", expand=True)

        # Pagination Footer
        self.footer_frame = ctk.CTkFrame(self, height=40, fg_color="transparent")
        self.footer_frame.pack(fill="x", pady=5)
        
        self.btn_prev = ctk.CTkButton(self.footer_frame, text="< Prev", width=80, command=self.prev_page, state="disabled")
        self.btn_prev.pack(side="left", padx=10)
        
        self.lbl_page = ctk.CTkLabel(self.footer_frame, text="Page 1 of 1", text_color=Colors.TEXT_SECONDARY)
        self.lbl_page.pack(side="left", padx=10)
        
        self.btn_next = ctk.CTkButton(self.footer_frame, text="Next >", width=80, command=self.next_page, state="disabled")
        self.btn_next.pack(side="left", padx=10)

        self.load_data()

    def prev_page(self):
        if self.page > 1:
            self.page -= 1
            self.load_data()

    def next_page(self):
        if self.page < self.total_pages:
            self.page += 1
            self.load_data()

    def on_search_key(self, event=None):
        if self._search_job:
            self.after_cancel(self._search_job)
        self._search_job = self.after(500, self.load_data)

    def export_csv(self):
        if not self.transactions:
             messagebox.showwarning("No Data", "No transactions to export.")
             return

        filename = filedialog.asksaveasfilename(defaultextension=".csv", filetypes=[("CSV Files", "*.csv")])
        if not filename:
             return

        try:
            with open(filename, mode='w', newline='', encoding='utf-8') as f:
                writer = csv.writer(f)
                writer.writerow(["ID", "Barcode", "Product", "Type", "Value", "Qty", "Customer", "Supplier", "Date"]) # Header
                
                for t in self.transactions:
                    writer.writerow([
                        t.transaction_id,
                        t.barcode,
                        t.product_description, 
                        t.transaction_type, 
                        f"{t.transaction_value:.2f}",
                        t.quantity_change, 
                        t.customer_name,
                        t.supplier_name,
                        t.transaction_date
                    ])
            messagebox.showinfo("Success", f"Exported to {filename}")
        except Exception as e:
            messagebox.showerror("Export Failed", str(e))

    def load_data(self, _=None):
        for widget in self.scroll_frame.winfo_children():
            widget.destroy()

        t_type = self.cmb_type.get()
        query = self.entry_search.get().strip() or None

        # Count & Pagination
        total = self.db.count_filtered_transactions(start_date=None, end_date=None, trans_type=t_type, search_query=query)
        self.total_pages = (total + self.limit - 1) // self.limit if total > 0 else 1
        
        # Clamp Page
        if self.page > self.total_pages: self.page = 1
        offset = (self.page - 1) * self.limit

        # Fetch Page
        self.transactions = self.db.get_filtered_transactions(
            start_date=None, end_date=None, trans_type=t_type, search_query=query, 
            limit=self.limit, offset=offset
        )
        
        # Update Footer
        self.lbl_page.configure(text=f"Page {self.page} of {self.total_pages} ({total} items)")
        self.btn_prev.configure(state="normal" if self.page > 1 else "disabled")
        self.btn_next.configure(state="normal" if self.page < self.total_pages else "disabled")
        
        # Sorting (Python-side)
        sort_mode = self.cmb_sort.get()
        # We need to process values first because "Value" in DB logic might be different from UI Display Logic,
        # but here we sort based on displayed property or raw data?
        # Raw t.transaction_value is currently calc'd in DB as: Supply (-), Delivery sales (+).
        # UI logic REQUESTED: Supply = Green (+), Delivery = Red (-).
        # So effective value for sorting 'Value High-Low' should probably follow UI logic.
        
        # Let's Normalize values for UI Logic before sorting? 
        # Or Just sort based on raw magnitude if that's what user expects?
        # User said "Supply is + ... Delivery is -".
        # So I should probably recalculate the values here or rely on DB.
        # DB returns: Supply (Neg Cost), Delivery (Pos Rev).
        # UI wants: Supply (Pos Stock Value addition), Delivery (Neg Stock Value subtraction).
        
        # Let's adjust the transaction objects' values or just handle it in display?
        # If I sort "Value High-Low", Supply (Green +) should be at top.
        # Currently Supply is Negative in DB.
        # So I need to invert logic for sorting if I want Supply to be "High".
        
        def get_sort_key(t):
            # Effective UI Value
            is_supply = t.transaction_type == "Supply"
            raw_val = t.transaction_value
            # Current raw: Supply is -Cost, Delivery is +Rev.
            # Target UI: Supply is +Val, Delivery is -Val.
            # So abs(raw_val) gives magnitude.
            # Supply: +abs(raw_val)
            # Delivery: -abs(raw_val)
            
            ui_val = abs(raw_val) if is_supply else -abs(raw_val)
            
            if sort_mode == "Date Newest": return t.transaction_date.timestamp() if hasattr(t.transaction_date, 'timestamp') else 0
            if sort_mode == "Date Oldest": return t.transaction_date.timestamp() if hasattr(t.transaction_date, 'timestamp') else 0
            if sort_mode == "Value High-Low": return ui_val
            if sort_mode == "Value Low-High": return ui_val
            if sort_mode == "Qty High-Low": return abs(t.quantity_change) # Magnitude of items moved
            return 0

        reverse = True
        if sort_mode == "Date Oldest" or sort_mode == "Value Low-High":
            reverse = False
            
        self.transactions.sort(key=get_sort_key, reverse=reverse)

        from ui.styles import Colors # Ensure import

        for i, t in enumerate(self.transactions):
            bg = Colors.BG_CARD if i % 2 == 0 else Colors.BG_HOVER
            row_frame = ctk.CTkFrame(self.scroll_frame, fg_color=bg, corner_radius=0)
            row_frame.pack(fill="x", pady=0)
            
            date_str = t.transaction_date.strftime("%Y-%m-%d %I:%M %p") if isinstance(t.transaction_date, datetime) else str(t.transaction_date)
            
            val_color = Colors.TEXT_PRIMARY
            qty_color = Colors.TEXT_PRIMARY
            type_color = Colors.TEXT_PRIMARY
            is_supply = t.transaction_type == "Supply"

            # Styling
            # Supply = Green +, Delivery = Red -
            val_magnitude = abs(t.transaction_value)
            val_txt = f"₱{val_magnitude:,.2f}"
            
            if is_supply:
                val_txt = "+" + val_txt
                val_color = Colors.SUCCESS # Green
                type_color = Colors.SUCCESS
            else: # Delivery / anything else (Red -)
                val_txt = "-" + val_txt
                val_color = Colors.DANGER # Red
                if t.transaction_type == "Delivery":
                     type_color = Colors.DANGER
            
            # Columns
            vals = [
                (str(t.transaction_id), 0),
                (t.barcode or "-", 0),
                (t.product_description, 1), # expandable
                (t.transaction_type, 0, type_color),
                (val_txt, 0, val_color),
                (str(t.quantity_change), 0),
                (t.customer_name or "", 0),
                (t.supplier_name or "", 0),
                (date_str, 0)
            ]

            for c, v_data in enumerate(vals):
                txt = v_data[0]
                expand = v_data[1]
                col_prop = self.cols[c]
                width = col_prop[1]
                color = v_data[2] if len(v_data) > 2 else Colors.TEXT_PRIMARY
                
                row_frame.grid_columnconfigure(c, weight=1 if expand else 0)
                
                lbl = ctk.CTkLabel(
                    row_frame, 
                    text=txt, 
                    width=width if not expand else 0,
                    font=("Arial", 12),
                    text_color=color,
                    anchor="w"
                )
                lbl.grid(row=0, column=c, padx=5, pady=8, sticky="ew")
