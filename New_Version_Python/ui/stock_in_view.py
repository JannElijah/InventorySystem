import customtkinter as ctk
from tkinter import messagebox
from ui.styles import Colors, Dimens
from utils.tooltip import ToolTip
from ui.components.search_bar import SearchBar
from ui.components.product_selection_dialog import ProductSelectionDialog

class StockInView(ctk.CTkFrame):
    def __init__(self, parent, db, user):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        self.user = user
        self.cart = [] # List of dicts: product, quantity, cost
        
        # Grid
        self.grid_columnconfigure(0, weight=1)
        self.grid_rowconfigure(1, weight=1)

        # --- Header ---
        header = ctk.CTkFrame(self, fg_color="transparent")
        header.grid(row=0, column=0, sticky="ew", padx=20, pady=(0, 20))
        
        ctk.CTkLabel(header, text="Stock In / Supply", font=Dimens.heading_l(None), text_color=Colors.TEXT_PRIMARY).pack(side="left")

        # Search
        from ui.components.autocomplete_search_bar import AutocompleteSearchBar
        self.search_bar = AutocompleteSearchBar(header, 
                                                db=self.db,
                                                width=300, 
                                                on_select_callback=self.on_product_selected, 
                                                placeholder="Scan Barcode or Search Product...")
        self.search_bar.pack(side="right")
        self.after(500, lambda: self.search_bar.entry.focus_set())


        # --- Content (Split: Left=Cart, Right=Details/Actions) ---
        content = ctk.CTkFrame(self, fg_color="transparent")
        content.grid(row=1, column=0, sticky="nsew", padx=20, pady=(0, 20))
        content.grid_columnconfigure(0, weight=3)
        content.grid_columnconfigure(1, weight=1)
        content.grid_rowconfigure(0, weight=1)

        # Cart Grid
        self.cart_frame = ctk.CTkFrame(content, fg_color=Colors.BG_CARD)
        self.cart_frame.grid(row=0, column=0, sticky="nsew", padx=(0, 20))
        
        # Cart Header
        h_frame = ctk.CTkFrame(self.cart_frame, fg_color=Colors.BG_HOVER, height=40)
        h_frame.pack(fill="x", padx=2, pady=2)
        
        # Grid Config matches StockOutView
        h_frame.grid_columnconfigure(0, weight=1) # Product (Flex)
        h_frame.grid_columnconfigure(1, weight=0) # Stock (Fixed)
        h_frame.grid_columnconfigure(2, weight=0) # Qty (Fixed)
        h_frame.grid_columnconfigure(3, weight=0) # Unit Cost (Fixed)
        h_frame.grid_columnconfigure(4, weight=0) # Total Cost (Fixed)
        h_frame.grid_columnconfigure(5, weight=0) # Action (Fixed)

        headers = ["Product", "Current Stock", "Qty to Add", "Unit Cost", "Total Cost", "Action"]
        col_widths = [0, 80, 100, 100, 100, 60]
        
        for i, h in enumerate(headers):
            anchor = "w" if i < 5 else "center"
            if i == 0:
                 lbl = ctk.CTkLabel(h_frame, text=h, font=("Arial", 12, "bold"), text_color=Colors.TEXT_PRIMARY, anchor="w")
                 # sticky="ew" makes it stretch
                 lbl.grid(row=0, column=i, sticky="ew", padx=5, pady=5)
            else:
                 # Fixed width container/label
                 lbl = ctk.CTkLabel(h_frame, text=h, font=("Arial", 12, "bold"), text_color=Colors.TEXT_PRIMARY, width=col_widths[i], anchor=anchor)
                 lbl.grid(row=0, column=i, sticky="e", padx=5, pady=5)

        self.scroll_cart = ctk.CTkScrollableFrame(self.cart_frame, fg_color="transparent")
        self.scroll_cart.pack(fill="both", expand=True, padx=5, pady=5)

        # Actions Panel (Right)
        action_panel = ctk.CTkFrame(content, fg_color=Colors.BG_CARD)
        action_panel.grid(row=0, column=1, sticky="nsew")
        
        ctk.CTkLabel(action_panel, text="Summary", font=("Arial", 16, "bold"), text_color=Colors.TEXT_PRIMARY).pack(pady=20)
        
        # Supplier Selection (Moved here)
        self.supplier_map = {}
        suppliers = self.db.get_all_suppliers(active_only=True)
        supplier_names = ["General Supply"] # Default
        self.supplier_map["General Supply"] = None
        
        for s in suppliers:
            self.supplier_map[s.name] = s.supplier_id
            supplier_names.append(s.name)
            
        ctk.CTkLabel(action_panel, text="Supplier Source:", font=("Arial", 12), text_color=Colors.TEXT_SECONDARY).pack(pady=(10, 0))
        self.supplier_var = ctk.StringVar(value=supplier_names[0])
        self.cmb_supplier = ctk.CTkComboBox(action_panel, values=supplier_names, variable=self.supplier_var, width=200,
                                            fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_supplier.pack(pady=(5, 20))
        
        self.lbl_total_items = ctk.CTkLabel(action_panel, text="Items: 0", font=("Arial", 14), text_color=Colors.TEXT_PRIMARY)
        self.lbl_total_items.pack(pady=5)
        
        self.lbl_total_cost = ctk.CTkLabel(action_panel, text="Total Cost: $0.00", font=("Arial", 18, "bold"), text_color=Colors.NEON_GREEN)
        self.lbl_total_cost.pack(pady=20)
        
        self.btn_process = ctk.CTkButton(action_panel, text="Process Stock In", fg_color=Colors.SUCCESS, text_color="black", height=50, 
                      font=("Arial", 14, "bold"), command=self.process_transaction)
        self.btn_process.pack(fill="x", padx=20, pady=20)
        
        ctk.CTkButton(action_panel, text="Clear", fg_color=Colors.DANGER, 
                      command=self.clear_cart).pack(fill="x", padx=20, pady=5)

    def handle_search(self, query):
        if not query: return
        
        # Simulate simple search match (can use UniversalSearch logic)
        # For simplify, iterate all products
        all_p = self.db.get_all_products()
        match = next((p for p in all_p if p.barcode == query or p.part_number == query), None)
        

        if not match:
             # Try partial name search if not exact code
             matches = [p for p in all_p if query.lower() in p.description.lower() or query.lower() in p.brand.lower()]
             
             if len(matches) == 1:
                 match = matches[0]
             elif len(matches) > 1:
                 dialog = ProductSelectionDialog(self, matches)
                 self.wait_window(dialog)
                 match = dialog.result
        
        if match:
            self.add_to_cart(match)
            self.search_bar.clear()
        else:
            messagebox.showwarning("Not Found", "Product not found.")

    def add_to_cart(self, product):
        # Check if already in cart
        existing = next((item for item in self.cart if item['product'].product_id == product.product_id), None)
        if existing:
            existing['quantity'] += 1
        else:
            self.cart.append({
                'product': product,
                'quantity': 1,
                'cost': product.purchase_cost
            })
        self.update_cart_ui()

    def update_cart_ui(self):
        for w in self.scroll_cart.winfo_children(): w.destroy()
        
        total_items = 0
        total_cost = 0.0
        
        for item in self.cart:
            p = item['product']
            q = item['quantity']
            c = item['cost']
            subtotal = q * c
            
            total_items += q
            total_cost += subtotal
            
            col_widths = [0, 80, 100, 100, 100, 60]
            
            row = ctk.CTkFrame(self.scroll_cart, fg_color=Colors.BG_HOVER)
            row.pack(fill="x", pady=2)
            
            # Use Grid
            row.grid_columnconfigure(0, weight=1)
            row.grid_columnconfigure(1, weight=0)
            row.grid_columnconfigure(2, weight=0)
            row.grid_columnconfigure(3, weight=0)
            row.grid_columnconfigure(4, weight=0)
            row.grid_columnconfigure(5, weight=0)

            # Name
            ctk.CTkLabel(row, text=p.description, anchor="w", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=0, sticky="ew", padx=5)
            
            # Stock
            ctk.CTkLabel(row, text=str(p.stock_quantity), width=col_widths[1], text_color=Colors.TEXT_SECONDARY, anchor="e").grid(row=0, column=1, sticky="e", padx=5)
            
            # Qty (Frame Fixed) - ADDED HEIGHT
            q_frame = ctk.CTkFrame(row, fg_color="transparent", width=col_widths[2], height=36)
            q_frame.grid(row=0, column=2, sticky="e", padx=5)
            q_frame.grid_propagate(False) # Ensure size is respected if we used grid inside, but here we use place
            
            # Center content in fixed frame
            q_inner = ctk.CTkFrame(q_frame, fg_color="transparent")
            q_inner.place(relx=0.5, rely=0.5, anchor="center")
            
            # Ensure buttons work and are visible
            btn_minus = ctk.CTkButton(q_inner, text="-", width=25, height=25, fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY,
                                      command=lambda i=item: self.update_qty(i, -1))
            btn_minus.pack(side="left", padx=2)
            
            # Edit Entry
            entry_qty = ctk.CTkEntry(q_inner, width=40, height=25, justify="center", fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
            entry_qty.insert(0, str(q))
            entry_qty.pack(side="left", padx=2)
            # Bind events
            entry_qty.bind("<FocusOut>", lambda e, i=item, ent=entry_qty: self.set_qty_from_entry(i, ent))
            entry_qty.bind("<Return>", lambda e, i=item, ent=entry_qty: self.set_qty_from_entry(i, ent))
            
            btn_plus = ctk.CTkButton(q_inner, text="+", width=25, height=25, fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY,
                                     command=lambda i=item: self.update_qty(i, 1))
            btn_plus.pack(side="left", padx=2)
            
            # Cost
            ctk.CTkLabel(row, text=f"${c:.2f}", width=col_widths[3], text_color=Colors.TEXT_PRIMARY, anchor="e").grid(row=0, column=3, sticky="e", padx=5)
            
            # Total (Label ref needed for live update)
            lbl_total_row = ctk.CTkLabel(row, text=f"${subtotal:.2f}", width=col_widths[4], text_color=Colors.TEXT_PRIMARY, anchor="e")
            lbl_total_row.grid(row=0, column=4, sticky="e", padx=5)
            
            # Use closure to capture specific row items
            entry_qty.bind("<KeyRelease>", lambda e, i=item, lbl=lbl_total_row: self.on_qty_change_live(e, i, lbl))
            
            # Remove - ADDED HEIGHT and Propagate False
            remove_frame = ctk.CTkFrame(row, fg_color="transparent", width=col_widths[5], height=36)
            remove_frame.grid(row=0, column=5, sticky="e", padx=5)
            # Important: place() does not trigger geometry expansion, but explicit height helps.
            # We can also force propagation off just in case.
            remove_frame.grid_propagate(False) 
            ctk.CTkButton(remove_frame, text="x", width=30, fg_color=Colors.DANGER, 
                          command=lambda i=item: self.remove_item(i)).place(relx=0.5, rely=0.5, anchor="center")

        self.lbl_total_items.configure(text=f"Items: {total_items}")
        self.lbl_total_cost.configure(text=f"Total Cost: ${total_cost:,.2f}")

    def update_qty(self, item, delta):
        item['quantity'] += delta
        if item['quantity'] <= 0:
            self.remove_item(item)
        else:
            self.update_cart_ui()

    def set_qty_from_entry(self, item, entry_widget):
        try:
            val = int(entry_widget.get())
            if val <= 0:
                self.remove_item(item)
            else:
                item['quantity'] = val
                # Re-render to update totals (delayed to avoid widget destruction error)
                self.after(10, self.update_cart_ui)
                # Focus back to avoid sticky focus issues? No, re-render destroys widget.
        except ValueError:
            # Revert to old value if invalid
            entry_widget.delete(0, "end")
            entry_widget.insert(0, str(item['quantity']))
            self.focus_set() # Unfocus

    def on_qty_change_live(self, event, item, lbl_total_row):
        # Calculate without re-rendering
        entry_widget = event.widget
        try:
            val = entry_widget.get().strip()
            if not val: return # Handle empty temporary state
            
            qty = int(val)
            if qty < 0: return # Ignore negative for now
            
            # Update Model
            item['quantity'] = qty
            
            # Update Row UI
            subtotal = qty * item['cost']
            lbl_total_row.configure(text=f"${subtotal:.2f}")
            
            # Update Grand Totals
            self.recalc_totals()
            
        except ValueError:
            pass # Ignore non-integers while typing

    def recalc_totals(self):
        total_items = sum(i['quantity'] for i in self.cart)
        total_cost = sum(i['quantity'] * i['cost'] for i in self.cart)
        self.lbl_total_items.configure(text=f"Items: {total_items}")
        self.lbl_total_cost.configure(text=f"Total Cost: ${total_cost:,.2f}")

    def remove_item(self, item):
        self.cart.remove(item)
        self.update_cart_ui()
        
    def clear_cart(self):
        self.cart.clear()
        self.update_cart_ui()

    def process_transaction(self):
        if not self.cart:
            messagebox.showwarning("Empty Cart", "Please add items to the cart first.")
            return

        if not messagebox.askyesno("Confirm", "Process Stock In transaction?"):
            return
            
        # Get Supplier
        supp_name = self.supplier_var.get()
        supplier_id = self.supplier_map.get(supp_name)
        
        # Disable button to prevent double-click
        self.btn_process.configure(state="disabled", text="Processing...")
        
        import threading
        def _process():
            try:
                # Prepare data logic: list of dicts {product_id, quantity, cost}
                items_data = []
                for item in self.cart:
                    items_data.append({
                        'product_id': item['product'].product_id,
                        'quantity': item['quantity'],
                        'cost': item['cost']
                    })
                
                self.db.process_supply_transaction(supplier_id, items_data, self.user.user_id if hasattr(self.user, 'user_id') else 1)
                
                def _success():
                    messagebox.showinfo("Success", "Stock Added Successfully!")
                    self.clear_cart()
                    self.btn_process.configure(state="normal", text="Process Stock In")

                self.after(0, _success)
                
            except Exception as e:
                err_msg = str(e)
                def _error():
                    messagebox.showerror("Error", f"Transaction failed: {err_msg}")
                    self.btn_process.configure(state="normal", text="Process Stock In")
                    
                self.after(0, _error)

        threading.Thread(target=_process, daemon=True).start()

    def on_product_selected(self, product):
        if product:
            self.add_to_cart(product)
