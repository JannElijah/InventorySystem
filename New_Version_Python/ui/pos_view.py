
import customtkinter as ctk
import threading
from datetime import datetime
from tkinter import messagebox
from database import DatabaseRepository
from models import Sale, SaleItem
from utils.receipt_generator import ReceiptGenerator
from utils.printer_utils import print_file
from utils.tooltip import ToolTip
from ui.styles import Colors, Dimens, Icons
import os

class POSView(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository, user):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        self.user = user
        self.cart_items = []
        # optimization: remove full load
        # self.all_products = self.db.get_all_products() 
        self.view_mode = "grid"  # "grid" or "search"
        
        # Pagination State
        self.current_page = 1
        self.items_per_page = 21 # 3 columns x 7 rows
        self.total_pages = 1

        # Layout: Left (Cart), Middle (Product Grid), Right (Controls)
        self.grid_columnconfigure(0, weight=2) # Cart
        self.grid_columnconfigure(1, weight=3) # Product Grid
        self.grid_columnconfigure(2, weight=1) # Controls
        self.grid_rowconfigure(0, weight=1)

        # --- LEFT: CART ---
        self.cart_frame = ctk.CTkFrame(self, fg_color=Colors.BG_CARD, corner_radius=Dimens.R_CARD)
        self.cart_frame.grid(row=0, column=0, sticky="nsew", padx=(0, 10))
        
        # Cart Header
        cart_header_frame = ctk.CTkFrame(self.cart_frame, fg_color="transparent")
        cart_header_frame.pack(fill="x", pady=10, padx=10)
        ctk.CTkLabel(cart_header_frame, text="🛒 Cart", font=Dimens.heading_m(None), text_color=Colors.TEXT_PRIMARY).pack(side="left")
        
        # Cart List
        self.cart_scroll = ctk.CTkScrollableFrame(self.cart_frame, fg_color="transparent")
        self.cart_scroll.pack(fill="both", expand=True, padx=10)
        
        # Totals
        self.total_frame = ctk.CTkFrame(self.cart_frame, fg_color=Colors.BG_HOVER, corner_radius=Dimens.R_SMALL)
        self.total_frame.pack(fill="x", pady=10, padx=10)
        
        self.lbl_total = ctk.CTkLabel(self.total_frame, text="Total: $0.00", font=Dimens.heading_l(None), text_color=Colors.SUCCESS)
        self.lbl_total.pack(pady=15)

        # --- MIDDLE: PRODUCT GRID ---
        self.product_container = ctk.CTkFrame(self, fg_color="transparent")
        self.product_container.grid(row=0, column=1, sticky="nsew", padx=10)
        
        # Grid Header with Toggle
        grid_header = ctk.CTkFrame(self.product_container, fg_color="transparent")
        grid_header.pack(fill="x", pady=(0, 10))
        
        ctk.CTkLabel(grid_header, text="Products", font=Dimens.heading_m(None)).pack(side="left")
        
        # View Toggle
        self.btn_toggle_view = ctk.CTkButton(grid_header, text="🔍 Search Mode", width=120, 
                                             fg_color=Colors.BG_CARD, command=self.toggle_view_mode)
        self.btn_toggle_view.pack(side="right")
        
        # Product Grid (Scrollable)
        self.product_grid = ctk.CTkScrollableFrame(self.product_container, fg_color="transparent")
        self.product_grid.pack(fill="both", expand=True)
        
        # Pagination Controls
        self.create_pagination_controls()
        
        self.render_product_grid()

        # --- RIGHT: CONTROLS ---
        self.controls_frame = ctk.CTkFrame(self, fg_color=Colors.BG_CARD, corner_radius=Dimens.R_CARD)
        self.controls_frame.grid(row=0, column=2, sticky="nsew")
        
        # Customer Selection
        ctk.CTkLabel(self.controls_frame, text="Customer:", font=Dimens.body_l(None)).pack(pady=(20, 5), padx=10, anchor="w")
        
        self.customers = self.db.get_all_customers() if hasattr(self.db, 'get_all_customers') else [] 
        cust_names = [c.name for c in self.customers]
        if not cust_names: cust_names = ["Walk-in"]
        
        self.om_customer = ctk.CTkOptionMenu(self.controls_frame, values=cust_names, fg_color=Colors.BG_INPUT,
                                             text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, button_hover_color=Colors.BG_HOVER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.om_customer.set("Walk-in") 
        self.om_customer.pack(fill="x", padx=10, pady=5)
        
        # New Customer Button
        self.btn_new_cust = ctk.CTkButton(self.controls_frame, text="+ New Customer", width=100, 
                                          fg_color=Colors.PRIMARY, text_color=Colors.TEXT_ON_NEON, command=self.toggle_customer_add)
        self.btn_new_cust.pack(pady=5)
        
        # Add Customer Form (Hidden)
        self.add_cust_frame = ctk.CTkFrame(self.controls_frame, fg_color="transparent")
        
        ctk.CTkLabel(self.add_cust_frame, text="Name").pack(anchor="w")
        self.entry_new_cust_name = ctk.CTkEntry(self.add_cust_frame, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_new_cust_name.pack(fill="x", pady=2)
        
        ctk.CTkLabel(self.add_cust_frame, text="Phone").pack(anchor="w")
        self.entry_new_cust_phone = ctk.CTkEntry(self.add_cust_frame, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_new_cust_phone.pack(fill="x", pady=2)
        
        ctk.CTkLabel(self.add_cust_frame, text="Email").pack(anchor="w")
        self.entry_new_cust_email = ctk.CTkEntry(self.add_cust_frame, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_new_cust_email.pack(fill="x", pady=2)
        
        row_btns = ctk.CTkFrame(self.add_cust_frame, fg_color="transparent")
        row_btns.pack(fill="x", pady=10)
        ctk.CTkButton(row_btns, text="Save", width=60, fg_color=Colors.SUCCESS, command=self.save_new_customer).pack(side="left", padx=5)
        ctk.CTkButton(row_btns, text="Cancel", width=60, fg_color=Colors.BG_HOVER, command=self.toggle_customer_add).pack(side="left", padx=5)
        
        # Actions
        ctk.CTkFrame(self.controls_frame, height=2, fg_color=Colors.BG_HOVER).pack(fill="x", pady=20)
        
        self.btn_checkout = ctk.CTkButton(self.controls_frame, text="✓ Checkout", fg_color=Colors.SUCCESS, 
                                          height=50, font=Dimens.body_l(None), text_color=Colors.TEXT_ON_NEON, command=self.checkout_event)
        self.btn_checkout.pack(fill="x", padx=10, pady=10)
        ToolTip.create(self.btn_checkout, "Complete transaction and print receipt")
        
        self.btn_clear = ctk.CTkButton(self.controls_frame, text="Clear Cart", fg_color=Colors.DANGER, command=self.clear_cart)
        self.btn_clear.pack(fill="x", padx=10, pady=5)
        ToolTip.create(self.btn_clear, "Remove all items from current sale")

    def toggle_view_mode(self):
        if self.view_mode == "grid":
            self.view_mode = "search"
            self.btn_toggle_view.configure(text="📋 Grid Mode")
            # Clear grid and show search
            for widget in self.product_grid.winfo_children():
                widget.destroy()
            
            # Search UI
            ctk.CTkLabel(self.product_grid, text="Search Products", font=Dimens.heading_m(None)).pack(pady=10)
            self.entry_search = ctk.CTkEntry(self.product_grid, placeholder_text="Enter ID or Barcode...", height=40,
                                             fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
            self.entry_search.pack(fill="x", padx=20, pady=10)
            self.entry_search.bind('<Return>', self.add_product_event)
            self.entry_search.focus_set()
            
            ctk.CTkButton(self.product_grid, text="Add to Cart", command=self.add_product_event, 
                         fg_color=Colors.PRIMARY, text_color=Colors.TEXT_ON_NEON, height=40).pack(fill="x", padx=20, pady=5)
        else:
            self.view_mode = "grid"
            self.btn_toggle_view.configure(text="🔍 Search Mode")
            self.render_product_grid()

    def create_pagination_controls(self):
        self.pagination_frame = ctk.CTkFrame(self.product_container, fg_color="transparent", height=40)
        self.pagination_frame.pack(fill="x", pady=5)
        
        self.btn_prev = ctk.CTkButton(self.pagination_frame, text="< Prev", width=80, 
                                      fg_color=Colors.BG_CARD, command=lambda: self.change_page(-1))
        self.btn_prev.pack(side="left", padx=10)
        
        self.lbl_page = ctk.CTkLabel(self.pagination_frame, text="Page 1", font=("Arial", 12))
        self.lbl_page.pack(side="left", expand=True)
        
        self.btn_next = ctk.CTkButton(self.pagination_frame, text="Next >", width=80, 
                                      fg_color=Colors.BG_CARD, command=lambda: self.change_page(1))
        self.btn_next.pack(side="right", padx=10)

    def change_page(self, delta):
        new_page = self.current_page + delta
        if 1 <= new_page <= self.total_pages:
            self.current_page = new_page
            self.render_product_grid()

    def render_product_grid(self):
        for widget in self.product_grid.winfo_children():
            widget.destroy()
        
        # Calculate Total Pages
        total_count = self.db.get_product_count()
        self.total_pages = max(1, (total_count + self.items_per_page - 1) // self.items_per_page)
        
        # Update Controls
        self.lbl_page.configure(text=f"Page {self.current_page} of {self.total_pages}")
        self.btn_prev.configure(state="normal" if self.current_page > 1 else "disabled")
        self.btn_next.configure(state="normal" if self.current_page < self.total_pages else "disabled")

        # Fetch Page Data
        products_to_show = self.db.get_products_paginated(self.current_page, self.items_per_page)
        
        row_frame = None
        for idx, product in enumerate(products_to_show):  # Show first 30 for performance
            if idx % 3 == 0:
                row_frame = ctk.CTkFrame(self.product_grid, fg_color="transparent")
                row_frame.pack(fill="x", pady=5)
            
            # Product Card
            card = ctk.CTkFrame(row_frame, fg_color=Colors.BG_CARD, corner_radius=Dimens.R_CARD, 
                               width=150, height=180)
            card.pack(side="left", padx=5, expand=True, fill="both")
            card.pack_propagate(False)
            
            # Image placeholder or actual image
            img_label = ctk.CTkLabel(card, text="📦", font=("Arial", 40))
            img_label.pack(pady=(10, 5))
            
            if product.image_path and os.path.exists(product.image_path):
                # Async Load
                from utils.async_image_loader import loader
                def on_image_loaded(img_tk, label_ref=img_label):
                    if label_ref.winfo_exists():
                        label_ref.configure(image=img_tk, text="")
                
                loader.load_image(product.image_path, (100, 100), on_image_loaded)
            
            # Product Name
            name_text = product.description[:20] + "..." if len(product.description) > 20 else product.description
            ctk.CTkLabel(card, text=name_text, font=Dimens.body_s(None), wraplength=130).pack()
            
            # Price
            ctk.CTkLabel(card, text=f"${product.selling_price:.2f}", font=Dimens.body_l(None), 
                        text_color=Colors.SUCCESS).pack(pady=2)
            
            # Stock indicator
            stock_color = Colors.DANGER if product.stock_quantity <= 0 else Colors.TEXT_SECONDARY
            ctk.CTkLabel(card, text=f"Stock: {product.stock_quantity}", font=Dimens.body_s(None),
                        text_color=stock_color).pack()
            
            # Add button
            btn = ctk.CTkButton(card, text="+ Add", width=100, height=30, fg_color=Colors.PRIMARY,
                               text_color=Colors.TEXT_ON_NEON,
                               command=lambda p=product: self.add_to_cart(p))
            btn.pack(pady=(5, 10))

    def toggle_customer_add(self):
        if self.add_cust_frame.winfo_viewable():
            self.add_cust_frame.pack_forget()
            self.btn_new_cust.configure(text="+ New Customer")
        else:
            self.add_cust_frame.pack(after=self.btn_new_cust, fill="x", padx=10, pady=10)
            self.btn_new_cust.configure(text="- Close")

    def save_new_customer(self):
        name = self.entry_new_cust_name.get().strip()
        if not name: return
        from models import Customer
        
        self.db.add_customer(Customer(0, name, self.entry_new_cust_phone.get(), self.entry_new_cust_email.get()))
        
        # Refresh List
        self.customers = self.db.get_all_customers()
        names = [c.name for c in self.customers]
        self.om_customer.configure(values=names)
        self.om_customer.set(name)
        
        # Reset and Hide
        self.entry_new_cust_name.delete(0, "end")
        self.entry_new_cust_phone.delete(0, "end")
        self.entry_new_cust_email.delete(0, "end")
        self.toggle_customer_add()

    def add_product_event(self, event=None):
        if self.view_mode != "search":
            return
        
        query = self.entry_search.get().strip()
        if not query:
            return

        product = self.find_product(query)
        if product:
            self.add_to_cart(product)
            self.entry_search.delete(0, "end")
            # Turbo Scan: Keep focus
            self.entry_search.focus_set()
        else:
            messagebox.showerror("Product Not Found", f"No product found for '{query}'")
            # Refocus even on error
            self.entry_search.focus_set()

    def find_product(self, query):
        # SQL Optimized Search
        
        # 1. Exact Barcode Match (Fastest + Most Common for Scanners)
        p = self.db.get_product_by_barcode(query)
        if p and p.is_active: return p
        
        # 2. Try ID if numeric
        if query.isdigit():
            p = self.db.get_product_by_id(int(query))
            if p and p.is_active: return p
            
        # 3. Fallback to Search (Description/Brand/PartNo)
        # Only active items
        results = self.db.search_products(query, limit=1)
        # Note: search_products needs to support Active Only? 
        # Checking implementation of search_products...
        # It's in products.py. It does NOT filter IsActive yet.
        # However, for POS we generally only want to sell active items.
        if results:
            # Check active status of result
            if results[0].is_active:
                return results[0]
            
        return None

    def add_to_cart(self, product):
        if product.stock_quantity <= 0:
             messagebox.showwarning("Out of Stock", f"{product.description} is out of stock!")
             return

        # Check if already in cart
        for item in self.cart_items:
            if item.product_id == product.product_id:
                if item.quantity >= product.stock_quantity:
                     messagebox.showwarning("Stock Limit", f"Cannot add more. Only {product.stock_quantity} available.")
                     return
                item.quantity += 1
                item.total = item.quantity * item.unit_price
                self.refresh_cart_list()
                return

        # New Item
        new_item = SaleItem(
            product_id=product.product_id,
            quantity=1,
            unit_price=product.selling_price,
            total=product.selling_price,
            product=product
        )
        self.cart_items.append(new_item)
        self.refresh_cart_list()

    def update_qty(self, item, delta):
        new_qty = item.quantity + delta
        if new_qty <= 0:
            self.cart_items.remove(item)
        else:
            if new_qty > item.product.stock_quantity:
                 messagebox.showwarning("Stock Limit", f"Only {item.product.stock_quantity} available.")
                 return
            item.quantity = new_qty
            item.total = item.quantity * item.unit_price
        
        self.refresh_cart_list()


    def refresh_cart_list(self):
        # Clear UI
        for widget in self.cart_scroll.winfo_children():
            widget.destroy()
            
        grand_total = 0.0
        
        for item in self.cart_items:
            row = ctk.CTkFrame(self.cart_scroll, fg_color=Colors.BG_HOVER, corner_radius=Dimens.R_SMALL)
            row.pack(fill="x", pady=3, padx=5)
            
            # Product info
            info_frame = ctk.CTkFrame(row, fg_color="transparent")
            info_frame.pack(fill="x", padx=10, pady=8)
            
            ctk.CTkLabel(info_frame, text=item.product.description[:25], anchor="w", 
                        font=Dimens.body_m(None)).pack(side="left", fill="x", expand=True)
            
            # Quantity and price
            qty_price = ctk.CTkFrame(row, fg_color="transparent")
            qty_price.pack(fill="x", padx=10, pady=(0, 8))
            
            # Interactive Quantity Controls
            btn_minus = ctk.CTkButton(qty_price, text="-", width=25, height=25, fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY,
                                      command=lambda i=item: self.update_qty(i, -1))
            btn_minus.pack(side="left", padx=2)
            
            # Edit Entry
            entry_qty = ctk.CTkEntry(qty_price, width=40, height=25, justify="center", fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
            entry_qty.insert(0, str(item.quantity))
            entry_qty.pack(side="left", padx=2)
            
            entry_qty.bind("<FocusOut>", lambda e, i=item, ent=entry_qty: self.set_qty_from_entry(i, ent))
            entry_qty.bind("<Return>", lambda e, i=item, ent=entry_qty: self.set_qty_from_entry(i, ent))

            btn_plus = ctk.CTkButton(qty_price, text="+", width=25, height=25, fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY,
                                     command=lambda i=item: self.update_qty(i, 1))
            btn_plus.pack(side="left", padx=2)
            
            # Total Label (Ref needed)
            lbl_total_row = ctk.CTkLabel(qty_price, text=f"${item.total:.2f}", anchor="e", 
                        font=Dimens.body_l(None), text_color=Colors.SUCCESS)
            lbl_total_row.pack(side="right")
            
            # Use closure
            entry_qty.bind("<KeyRelease>", lambda e, i=item, lbl=lbl_total_row: self.on_qty_change_live(e, i, lbl))
            
            grand_total += item.total

        self.lbl_total.configure(text=f"Total: ${grand_total:.2f}")

    def clear_cart(self):
        self.cart_items = []
        self.refresh_cart_list()

    def set_qty_from_entry(self, item, entry_widget):
        try:
            val = int(entry_widget.get())
            if val <= 0:
                self.update_qty(item, -item.quantity) # Remove item
            else:
                item.quantity = val
                item.total = val * item.product.selling_price
                # Schedule refresh to avoid destroying widget during event
                self.after(10, self.refresh_cart_list)
        except ValueError:
            entry_widget.delete(0, "end")
            entry_widget.insert(0, str(item.quantity))
            self.focus_set()

            entry_widget.insert(0, str(item.quantity))
            self.focus_set()

    def on_qty_change_live(self, event, item, lbl_total_row):
        entry_widget = event.widget
        try:
            val = entry_widget.get().strip()
            if not val: return
            qty = int(val)
            if qty < 0: return

            item.quantity = qty
            item.total = qty * item.product.selling_price
            
            # Update Row
            lbl_total_row.configure(text=f"${item.total:.2f}")
            
            # Update Grand 
            self.recalc_totals()
        except ValueError:
            pass

    def recalc_totals(self):
        grand_total = sum(i.total for i in self.cart_items)
        self.lbl_total.configure(text=f"Total: ${grand_total:.2f}")

    def checkout_event(self):
        if not self.cart_items: 
            messagebox.showwarning("Empty Cart", "Cannot checkout an empty cart.")
            return
            
        total = sum(item.quantity * item.product.selling_price for item in self.cart_items)
        
        # Confirm
        if not messagebox.askyesno("Checkout", f"Proceed with checkout?\nTotal: ${total:.2f}"):
            return
            
        # Run in Background Thread
        self.btn_checkout.configure(state="disabled", text="Processing...")
        threading.Thread(target=self._checkout_thread, args=(total,), daemon=True).start()

    def _checkout_thread(self, total):
        # Create Sale Record
        try:
            # COPY cart items to avoid race conditions if UI modified (though we blocked UI)
            # Actually we disabled the button, but cart_items is shared memory.
            # Best practice: make a shallow copy if needed, but for now strict list is ok.
            sale_id = self.db.create_sale(list(self.cart_items), total) 
            
            # Use after() to schedule UI updates on main thread
            self.after(0, lambda: self._on_checkout_success(sale_id, total))
            
        except Exception as e:
            self.after(0, lambda: self._on_checkout_error(str(e)))

    def _on_checkout_error(self, error_msg):
        self.btn_checkout.configure(state="normal", text="✓ Checkout")
        messagebox.showerror("Error", f"Checkout failed: {error_msg}")

    def _on_checkout_success(self, sale_id, total):
        self.btn_checkout.configure(state="normal", text="✓ Checkout")
        
        if messagebox.askyesno("Receipt", "Sale Complete! Print Receipt?"):
            receipt_items = []
            for item in self.cart_items:
                receipt_items.append({
                    'name': item.product.description,
                    'qty': item.quantity,
                    'total': item.quantity * item.product.selling_price
                })
            
            # Valid to run generation in thread? ReportLab is CPU bound.
            # Printing is IO bound.
            # For simplicity, we do this on main thread OR we could have done it in worker.
            # Let's keep it simple here as the heavy DB part is done.
            try:
                generator = ReceiptGenerator()
                pdf_path = generator.generate_receipt(str(sale_id), receipt_items, total)
                
                if pdf_path:
                    success = print_file(pdf_path)
                    if not success:
                        messagebox.showwarning("Print Error", "Could not send receipt to printer.")
            except Exception as e:
                 print(f"Receipt Error: {e}")

        messagebox.showinfo("Success", "Transaction completed successfully!")
        self.clear_cart()
        
        # Refresh grid if in grid mode
        if self.view_mode == "grid":
            self.render_product_grid()
            

