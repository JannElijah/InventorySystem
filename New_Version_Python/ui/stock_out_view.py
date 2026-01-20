import customtkinter as ctk
from tkinter import messagebox
from ui.styles import Colors, Dimens
from utils.tooltip import ToolTip
from ui.components.search_bar import SearchBar
from ui.components.product_selection_dialog import ProductSelectionDialog

class StockOutView(ctk.CTkFrame):
    def __init__(self, parent, db, user):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        self.user = user
        self.cart = [] # List of dicts: product, quantity, unit_price
        
        # Grid
        self.grid_columnconfigure(0, weight=1)
        self.grid_rowconfigure(1, weight=1)

        # --- Header ---
        header = ctk.CTkFrame(self, fg_color="transparent")
        header.grid(row=0, column=0, sticky="ew", padx=20, pady=(0, 20))
        
        ctk.CTkLabel(header, text="Stock Out", font=Dimens.heading_l(None), text_color=Colors.TEXT_PRIMARY).pack(side="left")

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
        
        # Grid Config matches StockInView
        h_frame.grid_columnconfigure(0, weight=1) # Product (Flex)
        h_frame.grid_columnconfigure(1, weight=0) # Stock (Fixed)
        h_frame.grid_columnconfigure(2, weight=0) # Qty (Fixed)
        h_frame.grid_columnconfigure(3, weight=0) # Unit Cost (Fixed)
        h_frame.grid_columnconfigure(4, weight=0) # Total Cost (Fixed)
        h_frame.grid_columnconfigure(5, weight=0) # Action (Fixed)

        headers = ["Product", "Available", "Qty to Sell", "Unit Price", "Total", "Action"]
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
        
        self.lbl_total_items = ctk.CTkLabel(action_panel, text="Items: 0", font=("Arial", 14), text_color=Colors.TEXT_PRIMARY)
        self.lbl_total_items.pack(pady=5)
        
        self.lbl_total_price = ctk.CTkLabel(action_panel, text="Total Price: $0.00", font=("Arial", 18, "bold"), text_color=Colors.NEON_GREEN)
        self.lbl_total_price.pack(pady=20)
        
        # Customer Name Input
        ctk.CTkLabel(action_panel, text="Customer Name:", font=("Arial", 12, "bold"), text_color=Colors.TEXT_SECONDARY).pack(pady=(10,0))
        self.entry_customer = ctk.CTkEntry(action_panel, placeholder_text="Enter Name (e.g. Walk-in)")
        self.entry_customer = ctk.CTkEntry(action_panel, placeholder_text="Enter Name (e.g. Walk-in)", fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_customer.pack(fill="x", padx=20, pady=5)
        
        # Transaction Type Selector
        ctk.CTkLabel(action_panel, text="Transaction Type:", font=("Arial", 12, "bold"), text_color=Colors.TEXT_SECONDARY).pack(pady=(10,0))
        self.reason_option = ctk.CTkOptionMenu(action_panel, values=["Delivery", "Damage", "Expired", "Loss", "Theft", "Personal Use"],
                                               fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, button_hover_color=Colors.BG_HOVER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.reason_option.pack(fill="x", padx=20, pady=5)
        self.reason_option.set("Delivery")
        
        self.btn_process = ctk.CTkButton(action_panel, text="Process Quick Sale", fg_color=Colors.DANGER, height=50, 
                      font=("Arial", 14, "bold"), command=self.process_transaction)
        self.btn_process.pack(fill="x", padx=20, pady=20)
        
        ctk.CTkButton(action_panel, text="Clear", fg_color=Colors.BG_HOVER, 
                      command=self.clear_cart).pack(fill="x", padx=20, pady=5)

        # Recent History Panel
        ctk.CTkLabel(action_panel, text="Recent Transactions", font=("Arial", 14, "bold"), text_color=Colors.TEXT_SECONDARY).pack(pady=(20, 5))
        self.history_frame = ctk.CTkFrame(action_panel, fg_color="transparent")
        self.history_frame.pack(fill="x", padx=20)


    def handle_search(self, query):
        if not query: return
        
        # 1. Try Exact Barcode/Part Match (Fastest)
        match = self.db.get_product_by_barcode(query)
        
        # 2. If no exact match, try Text Search (Name/Description)

        if not match:
             # Search for products with similar name
             matches = self.db.search_products(query, limit=10)
             if len(matches) == 1:
                 match = matches[0]
             elif len(matches) > 1:
                 # Ambiguous: Show Dialog
                 dialog = ProductSelectionDialog(self, matches)
                 self.wait_window(dialog)
                 match = dialog.result
        
        if match:
            if match.stock_quantity <= 0:
                 if not messagebox.askyesno("Stock Warning", f"{match.description} is Out of Stock! Add anyway?"):
                     return
            self.add_to_cart(match)
            self.search_bar.clear()
        else:
            messagebox.showwarning("Not Found", "Product not found.")

    def add_to_cart(self, product):
        existing = next((item for item in self.cart if item['product'].product_id == product.product_id), None)
        if existing:
            existing['quantity'] += 1
        else:
            self.cart.append({
                'product': product,
                'quantity': 1,
                'unit_price': product.selling_price
            })
        self.update_cart_ui()

        self.update_cart_ui()

    def add_history_entry(self, items, customer):
        # Mini-History: Show last 3
        total_qty = sum(item['quantity'] for item in items)
        
        # Format product names
        if items:
            first_prod = items[0]['product'].description
            if len(items) > 1:
                prod_txt = f"{first_prod} (+{len(items)-1} others)"
            else:
                prod_txt = first_prod
        else:
            prod_txt = "Unknown"

        txt = f"Sold {total_qty} units of [{prod_txt}] to {customer}"
        lbl = ctk.CTkLabel(self.history_frame, text=f"• {txt}", font=("Arial", 11), anchor="w")
        lbl.pack(fill="x", pady=2)
        
        # Limit to 3 (This is a simplified UI-only approach)
        if len(self.history_frame.winfo_children()) > 3:
             self.history_frame.winfo_children()[0].destroy()

    def update_cart_ui(self):
        for w in self.scroll_cart.winfo_children(): w.destroy()
        
        total_items = 0
        total_price = 0.0
        
        for item in self.cart:
            p = item['product']
            q = item['quantity']
            c = item['unit_price']
            subtotal = q * c
            
            total_items += q
            total_price += subtotal
            
        col_widths = [0, 80, 100, 100, 100, 60]
        
        for item in self.cart:
            p = item['product']
            q = item['quantity']
            c = item['unit_price']
            subtotal = q * c
            
            total_items += q
            total_price += subtotal
            
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
            
            # Available (Color Coded)
            stock_color = Colors.TEXT_PRIMARY
            if p.stock_quantity <= 0: stock_color = Colors.DANGER
            elif p.stock_quantity < 10: stock_color = Colors.WARNING
            
            ctk.CTkLabel(row, text=str(p.stock_quantity), text_color=stock_color, width=col_widths[1], anchor="e").grid(row=0, column=1, sticky="e", padx=5)
            
            # Qty
            q_frame = ctk.CTkFrame(row, fg_color="transparent", width=col_widths[2], height=36)
            q_frame.grid(row=0, column=2, sticky="e", padx=5)
            # Center content in fixed frame
            q_inner = ctk.CTkFrame(q_frame, fg_color="transparent")
            q_inner.place(relx=0.5, rely=0.5, anchor="center")
            
            # Ensure buttons work
            btn_minus = ctk.CTkButton(q_inner, text="-", width=25, height=25, fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY,
                                      command=lambda i=item: self.update_qty(i, -1))
            btn_minus.pack(side="left", padx=2)
            
            # Edit Entry
            entry_qty = ctk.CTkEntry(q_inner, width=40, height=25, justify="center", fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
            entry_qty.insert(0, str(q))
            entry_qty.pack(side="left", padx=2)
            
            entry_qty.bind("<FocusOut>", lambda e, i=item, ent=entry_qty: self.set_qty_from_entry(i, ent))
            entry_qty.bind("<Return>", lambda e, i=item, ent=entry_qty: self.set_qty_from_entry(i, ent))
            
            btn_plus = ctk.CTkButton(q_inner, text="+", width=25, height=25, fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY,
                                     command=lambda i=item: self.update_qty(i, 1))
            btn_plus.pack(side="left", padx=2)
            
            # Price
            ctk.CTkLabel(row, text=f"${c:.2f}", text_color=Colors.TEXT_PRIMARY, width=col_widths[3], anchor="e").grid(row=0, column=3, sticky="e", padx=5)
            
            # Total (Subtotal)
            lbl_total_row = ctk.CTkLabel(row, text=f"${subtotal:.2f}", text_color=Colors.TEXT_PRIMARY, width=col_widths[4], anchor="e")
            lbl_total_row.grid(row=0, column=4, sticky="e", padx=5)
            
            # Use closure
            entry_qty.bind("<KeyRelease>", lambda e, i=item, lbl=lbl_total_row: self.on_qty_change_live(e, i, lbl))
            
            # Remove
            remove_frame = ctk.CTkFrame(row, fg_color="transparent", width=col_widths[5], height=36)
            remove_frame.grid(row=0, column=5, sticky="e", padx=5)
            ctk.CTkButton(remove_frame, text="x", width=30, fg_color=Colors.DANGER, 
                          command=lambda i=item: self.remove_item(i)).place(relx=0.5, rely=0.5, anchor="center")

        self.lbl_total_items.configure(text=f"Items: {total_items}")
        self.lbl_total_price.configure(text=f"Total Price: ${total_price:,.2f}")

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
                self.after(10, self.update_cart_ui)
        except ValueError:
            entry_widget.delete(0, "end")
            entry_widget.insert(0, str(item['quantity']))
            self.focus_set()

            entry_widget.insert(0, str(item['quantity']))
            self.focus_set()

    def on_qty_change_live(self, event, item, lbl_total_row):
        entry_widget = event.widget
        try:
            val = entry_widget.get().strip()
            if not val: return
            qty = int(val)
            if qty < 0: return

            item['quantity'] = qty
            
            # Update Row
            subtotal = qty * item['price']
            lbl_total_row.configure(text=f"${subtotal:.2f}")
            
            # Update Grand 
            self.recalc_totals()
        except ValueError:
            pass

    def recalc_totals(self):
        total_items = sum(i['quantity'] for i in self.cart)
        total_price = sum(i['quantity'] * i['price'] for i in self.cart)
        self.lbl_total_items.configure(text=f"Items: {total_items}")
        self.lbl_total_price.configure(text=f"Total Price: ${total_price:,.2f}")

    def remove_item(self, item):
        self.cart.remove(item)
        self.update_cart_ui()
        
    def clear_cart(self):
        self.cart.clear()
        self.update_cart_ui()

    def process_transaction(self):
        if not self.cart:
            messagebox.showwarning("Empty Cart", "Please add items to cart first.")
            return
            
        # Get Reason from OptionMenu
        reason_map = {
            "Sale": "Sale",
            "Delivery": "Delivery",
            "Damage": "Damage",
            "Expired": "Expired",
            "Loss": "Loss",
            "Theft": "Theft"
        }
        selected = self.reason_option.get()
        reason = reason_map.get(selected, "Sale")
        
        # Disable Button
        self.btn_process.configure(state="disabled", text="Processing...")
        
        # Note: Stock Out is currently synchronous in this implementation plan.
        # To match Stock In robustly, we should thread it or at least use after logic if slow.
        # Given existing code is sync, we keep sync but manage button state,
        # OR we can wrap in try/finally to ensure button re-enables.
        
        # Better: use simple try/finally on main thread for now, or thread it.
        # Let's thread it for consistency and safety against UI freeze.
        import threading
        
        def _process():
            try:
                msg = ""
                hist_txt = ""
                
                # Sceario A: SALE
                if reason == "Sale":
                    # Get customer name (UI access needs to happen before thread or via queue, 
                    # but reading stringvar/entry from thread *can* be unsafe or work accidentally. 
                    # Better to read before thread.)
                    pass 

                # REFACTOR: Read inputs BEFORE thread
                
            except Exception as e:
                self.after(0, lambda: [messagebox.showerror("Error", f"Transaction failed: {e}"), 
                                       self.btn_process.configure(state="normal", text="Process Quick Sale")])

        # CORRECT APPROACH: Read UI values, then start thread.
        customer_name = self.entry_customer.get().strip()
        
        if reason == "Delivery" and not customer_name:
            messagebox.showwarning("Missing Info", "Please enter a Customer Name.")
            self.btn_process.configure(state="normal", text="Process Quick Sale")
            return
            
        def _bg_work():
            try:
                from models import SaleItem
                msg = ""
                hist_txt = ""

                if reason == "Delivery":
                    items_for_db = []
                    for item in self.cart:
                        items_for_db.append({
                            'product_id': item['product'].product_id,
                            'quantity': item['quantity'],
                            'unit_price': item['unit_price'],
                            'total': item['quantity'] * item['unit_price']  
                        })
                    
                    self.db.process_sale(
                        customer_name=customer_name,
                        items=items_for_db,
                        user_id=self.user.user_id if hasattr(self.user, 'user_id') else 1,
                        date=None
                    )
                    msg = "Delivery Processed Successfully!"
                    hist_txt = customer_name

                else:
                    self.db.process_stock_adjustment(
                        items=self.cart, 
                        reason=reason,
                        user_id=self.user.user_id if hasattr(self.user, 'user_id') else 1
                    )
                    msg = f"Stock Adjusted ({reason}) Successfully!"
                    hist_txt = f"[{reason}]"
                
                # Success Callback
                def _success():
                    messagebox.showinfo("Success", msg)
                    self.add_history_entry(self.cart, hist_txt)
                    self.clear_cart()
                    self.btn_process.configure(state="normal", text="Process Quick Sale")
                    
                self.after(0, _success)
                
            except Exception as e:
                err_msg = str(e)
                def _error():
                    messagebox.showerror("Error", f"Transaction failed: {err_msg}")
                    import traceback
                    traceback.print_exc()
                    self.btn_process.configure(state="normal", text="Process Quick Sale")
                self.after(0, _error)

        threading.Thread(target=_bg_work, daemon=True).start()


    def on_product_selected(self, product):
        if product:
             if product.stock_quantity <= 0:
                 if not messagebox.askyesno('Stock Warning', f'{product.description} is Out of Stock! Add anyway?'):
                     return
             self.add_to_cart(product)






