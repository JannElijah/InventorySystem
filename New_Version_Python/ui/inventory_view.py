import customtkinter as ctk
from tkinter import messagebox, filedialog
from database import DatabaseRepository
from models import Product, User
from ui.styles import Colors, Dimens, Icons
from utils.tooltip import ToolTip
from utils.barcode_generator import BarcodeGenerator
from utils.printer_utils import print_file
from ui.components.bulk_edit_dialog import BulkEditDialog
import random
import string

class InventoryView(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository, user: User):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        self.user = user
        # Remove self.products cache to save memory
        self.editing_product = None
        self.entries = {}
        
        # Pagination
        self.ITEMS_PER_PAGE = 50
        self.current_page = 1
        self.total_count = 0 
        self.current_query = ""
        self.show_archived = False
        self.stock_filter = "ALL" # Default to showing all
        self.selection_mode = False
        self.selection_mode = False
        self.selected_ids = set() # Set of product_ids
        self.check_vars = {} # {product_id: ctk.BooleanVar}
        self.row_indices = {} # {product_id: index}
        self.products_by_index = {} # {index: product}
        self.last_clicked_index = -1
        
        self.last_clicked_index = -1

        
        # Sorting
        self.sort_options = {
            "ID (Newest)": ("ProductID", "DESC"),
            "ID (Oldest)": ("ProductID", "ASC"),
            "Name (A-Z)": ("Description", "ASC"),
            "Name (Z-A)": ("Description", "DESC"),
            "Price (Low-High)": ("SellingPrice", "ASC"),
            "Price (High-Low)": ("SellingPrice", "DESC"),
            "Stock (Low-High)": ("StockQuantity", "ASC"),
            "Stock (High-Low)": ("StockQuantity", "DESC")
        }
        self.current_sort_key = "ID (Newest)"

        
        # Bind Shortcuts (Use toplevel since CTk forbids bind_all)
        # We delay slighty to ensure toplevel exists or just bind on the parent's master if available.
        # Safest is to wait for visibility or bind to the parent's root.
        # But 'parent' passed in is usually the main_frame.
        
        # We will use a dedicated method to bind when the frame is mapped
        self.bind("<Map>", self._bind_shortcuts)
        self.bind("<Unmap>", self._unbind_shortcuts)
        
        # Grid Layout
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure(0, weight=1)

        # Header
        self.header_frame = ctk.CTkFrame(self, fg_color="transparent")
        self.header_frame.grid(row=0, column=0, sticky="ew", padx=20, pady=(0, 20))
        
        self.lbl_title = ctk.CTkLabel(self.header_frame, text="Inventory Management", font=Dimens.heading_l(None), text_color=Colors.TEXT_PRIMARY)
        self.lbl_title.pack(side="left")

        self.btn_refresh = ctk.CTkButton(self.header_frame, text="Refresh", width=100, command=self.load_data, fg_color=Colors.BG_CARD)
        self.btn_refresh.pack(side="right", padx=5)
        ToolTip.create(self.btn_refresh, "Reload list from database")

        self.btn_add = ctk.CTkButton(self.header_frame, text="+ Add Product", width=120, command=self.toggle_form, 
                                     fg_color=Colors.SUCCESS, text_color=Colors.TEXT_ON_NEON)
        if self.user.role == "Admin":
            self.btn_add.pack(side="right", padx=5)
        ToolTip.create(self.btn_add, "Create a new inventory item")
        
        # Bulk Edit Toggle
        self.btn_bulk = ctk.CTkButton(self.header_frame, text="Select Items", width=100, command=self.toggle_selection_mode, 
                                     fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY, border_width=1, border_color=Colors.PRIMARY)
        self.btn_bulk.pack(side="right", padx=5)

        # Export Button
        self.btn_export = ctk.CTkButton(self.header_frame, text="Export CSV", width=100, command=self.export_csv,
                                        fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY, border_width=1, border_color=Colors.SUCCESS)
        self.btn_export.pack(side="right", padx=5)

        self.switch_archive = ctk.CTkSwitch(self.header_frame, text="Show Archived", command=self.toggle_archived_view, text_color=Colors.TEXT_PRIMARY, progress_color=Colors.PRIMARY)
        # Only show for admins? Or all? Usually helps to keep inventory clean. 
        # Assuming all users might need to search old stock.
        self.switch_archive.pack(side="right", padx=15)

        # Search Bar
        self.search_var = ctk.StringVar()
        # Debounce: Remove trace and use key bindings or after loop
        self.search_job = None
        self.entry_search = ctk.CTkEntry(self.header_frame, placeholder_text="Search Products...", width=250, textvariable=self.search_var, 
                                         fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_search.pack(side="left", padx=20)
        # Sort Dropdown
        self.cmb_sort = ctk.CTkComboBox(self.header_frame, values=list(self.sort_options.keys()), width=150, command=self.on_sort_change,
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_sort.set(self.current_sort_key)
        self.cmb_sort.pack(side="left", padx=5)

        self.search_var.trace("w", self.on_search_change)
        
        # --- Integrated Form (Row 1) ---
        self.form_frame = ctk.CTkFrame(self, fg_color=Colors.BG_CARD, corner_radius=10)
        # ... (rest is same until filter_data)

    # ... (skipping lines 85-243)

        # --- Integrated Form (Row 1) ---
        # Initially Hidden
        self.setup_form()

        # Grid Header
        self.grid_header = ctk.CTkFrame(self, height=40, fg_color=Colors.BG_HOVER)
        self.grid_header.grid(row=2, column=0, sticky="ew", padx=20, pady=(0, 5))
        
        # Configure Grid Columns for Header and Rows
        # 0: ID, 1: Brand, 2: Description, 3: Stock, 4: Price, 5: Image, 6: Action
        self.grid_header.grid_columnconfigure(2, weight=1) # Description expands
        
        # Headers with Fixed Widths to match Row Data
        # [Text, Width, Anchor]
        header_config = [
            ("ID", 40, "w"),
            ("Brand", 120, "w"),
            ("Description", 0, "w"), # 0 = Flex
            ("Stock", 80, "e"),
            ("Price", 80, "e"),
            # Image column removed as per user request
            ("", 150, "e") # Action Area
        ]
        
        for i, (text, w, anchor) in enumerate(header_config):
            if i == 2: # Description (Flex)
                lbl = ctk.CTkLabel(self.grid_header, text=text, font=("Arial", 12, "bold"), anchor=anchor, text_color=Colors.TEXT_PRIMARY)
                sticky_val = "ew"
            else:
                lbl = ctk.CTkLabel(self.grid_header, text=text, font=("Arial", 12, "bold"), width=w, anchor=anchor, text_color=Colors.TEXT_PRIMARY)
                sticky_val = anchor if anchor != "center" else ""
            
            lbl.grid(row=0, column=i, padx=10, pady=5, sticky=sticky_val)

        # Scrollable List
        self.scroll_frame = ctk.CTkScrollableFrame(self, fg_color="transparent")
        self.scroll_frame.grid(row=3, column=0, sticky="nsew", padx=20, pady=(0, 20))
        self.grid_rowconfigure(3, weight=1)
        
        # Pagination Controls
        self.pagination_frame = ctk.CTkFrame(self, fg_color="transparent", height=40)
        self.pagination_frame.grid(row=4, column=0, sticky="ew", padx=20, pady=5)
        
        self.btn_prev = ctk.CTkButton(self.pagination_frame, text="< Previous", width=80, command=self.prev_page, state="disabled")
        self.btn_prev.pack(side="left")
        
        self.lbl_page_info = ctk.CTkLabel(self.pagination_frame, text="Page 1 of 1", font=("Arial", 12), text_color=Colors.TEXT_SECONDARY)
        self.lbl_page_info.pack(side="left", padx=20)
        
        self.btn_next = ctk.CTkButton(self.pagination_frame, text="Next >", width=80, command=self.next_page)
        self.btn_next.pack(side="left")
        
        # Bulk Action Bar (Floating at bottom)
        self.bulk_bar = ctk.CTkFrame(self, fg_color=Colors.PRIMARY, height=50)
        self.lbl_selected_count = ctk.CTkLabel(self.bulk_bar, text="0 Items Selected", font=("Arial", 14, "bold"), text_color="white")
        self.lbl_selected_count.pack(side="left", padx=20)
        
        ctk.CTkButton(self.bulk_bar, text="Edit Selected", command=self.open_bulk_edit, 
                      fg_color="white", text_color="black").pack(side="right", padx=10, pady=10)

        ctk.CTkButton(self.bulk_bar, text="🖨️ Print Selected", command=self.print_selected_labels, 
                      fg_color=Colors.INFO, text_color="black").pack(side="right", padx=10, pady=10)
        
        self.load_data()

    def load_data(self):
        self.current_page = 1 
        self.current_query = ""
        self.search_var.set("") # Clear search box visually
        self.refresh_grid()

    def on_sort_change(self, choice):
        self.current_sort_key = choice
        self.refresh_grid()

    def on_search_change(self, *args):
        # Debounce
        if self.search_job:
            self.after_cancel(self.search_job)
        self.search_job = self.after(300, self.perform_search)

    def destroy(self):
        if self.search_job:
            try:
                self.after_cancel(self.search_job)
            except Exception:
                pass
        super().destroy()
        
    def perform_search(self):
        new_query = self.search_var.get().strip()
        if new_query != self.current_query:
            self.current_query = new_query
            self.current_page = 1 # Reset to page 1 on new search
            self.load_data() # Use load_data instead of refresh_grid

    def set_filter(self, filter_type: str):
        """Called by Dashboard to set view mode"""
        self.stock_filter = filter_type
        self.current_page = 1
        self.search_var.set("") 
        self.current_query = ""
        
        # Update Title
        title_map = {
            "ALL": "Inventory Management",
            "LOW": "Low Stock Items",
            "OUT": "Out of Stock Items"
        }
        self.lbl_title.configure(text=title_map.get(filter_type, "Inventory"))
        self.load_data()

    def setup_form(self):
        # Clear existing
        for widget in self.form_frame.winfo_children():
            widget.destroy()
            
        # Title
        ctk.CTkLabel(self.form_frame, text="Product Details", font=Dimens.heading_m(None)).grid(row=0, column=0, columnspan=2, pady=10, padx=20, sticky="w")
        
        # Container for fields (2 Columns)
        fields_container = ctk.CTkFrame(self.form_frame, fg_color="transparent")
        fields_container.grid(row=1, column=0, columnspan=2, padx=20, pady=5, sticky="ew")
        
        fields = [
            ("Barcode", "barcode"), ("Part Number", "part_number"), 
            ("Brand", "brand"), ("Description", "description"), 
            ("Volume", "volume"), ("Type", "type"),
            ("Application", "application"), ("Purchase Cost", "purchase_cost"), 
            ("Selling Price", "selling_price"), ("Stock Quantity", "stock_quantity"), 
            ("Low Stock Threshold", "low_stock_threshold"), ("Reorder Point", "reorder_point"),
            ("Target Stock", "target_stock"), ("Notes", "notes")
        ]
        
        self.entries = {}
        for i, (label_text, field_name) in enumerate(fields):
            r = i // 2
            c = i % 2
            
            f_wrapper = ctk.CTkFrame(fields_container, fg_color="transparent")
            f_wrapper.grid(row=r, column=c, sticky="ew", padx=10, pady=5)
            
            ctk.CTkLabel(f_wrapper, text=label_text, font=("Arial", 12), text_color=Colors.TEXT_SECONDARY).pack(anchor="w")
            
            if field_name == "barcode":
                 sub = ctk.CTkFrame(f_wrapper, fg_color="transparent")
                 sub.pack(fill="x")
                 entry = ctk.CTkEntry(sub, height=35, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
                 entry.pack(side="left", fill="x", expand=True)
                 ctk.CTkButton(sub, text="Generate", width=60, height=35, fg_color=Colors.BG_HOVER, 
                              command=self.generate_barcode_value).pack(side="right", padx=5)
            else:
                entry = ctk.CTkEntry(f_wrapper, height=35, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
                entry.pack(fill="x")
                
            self.entries[field_name] = entry

        # Image Selection
        img_container = ctk.CTkFrame(self.form_frame, fg_color="transparent")
        img_container.grid(row=2, column=0, columnspan=2, sticky="ew", padx=30, pady=10)
        
        ctk.CTkLabel(img_container, text="Product Image:", font=("Arial", 12), text_color=Colors.TEXT_SECONDARY).pack(side="left")
        self.entry_image = ctk.CTkEntry(img_container, width=300, height=35, placeholder_text="No image selected", 
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_image.pack(side="left", padx=10)
        ctk.CTkButton(img_container, text="Browse...", width=100, height=35, fg_color=Colors.BG_HOVER, command=self.browse_image).pack(side="left")

        # Action Buttons
        btn_box = ctk.CTkFrame(self.form_frame, fg_color="transparent")
        btn_box.grid(row=3, column=0, columnspan=2, pady=20)
        
        self.btn_save = ctk.CTkButton(btn_box, text="Save Product", width=150, height=40, 
                                     fg_color=Colors.SUCCESS, text_color=Colors.TEXT_ON_NEON, 
                                     font=Dimens.body_m(None), command=self.save_product)
        self.btn_save.pack(side="left", padx=10)
        
        ctk.CTkButton(btn_box, text="Cancel", width=100, height=40, fg_color=Colors.BG_HOVER, 
                     command=self.toggle_form).pack(side="left", padx=10)
        
        self.btn_print = ctk.CTkButton(btn_box, text="Print Label", width=120, height=40, 
                                      fg_color=Colors.PRIMARY, text_color=Colors.TEXT_ON_NEON,
                                      command=self.print_label)
        self.btn_print.pack(side="right", padx=10)
    
    def browse_image(self):
        filename = filedialog.askopenfilename(title="Select Product Image", filetypes=[("Images", "*.png;*.jpg;*.jpeg;*.webp")])
        if filename:
            self.entry_image.delete(0, "end")
            self.entry_image.insert(0, filename)

    def toggle_form(self):
        if self.form_frame.winfo_viewable():
            self.form_frame.grid_forget()
            self.btn_add.configure(text="+ Add Product", fg_color="green")
            self.editing_product = None
            self.clear_entries()
        else:
            self.form_frame.grid(row=1, column=0, sticky="ew", padx=20, pady=10)
            self.btn_add.configure(text="- Close Form", fg_color="gray")

    def load_data(self):
        self.current_page = 1 
        self.current_query = ""
        self.search_var.set("") # Clear search box visually
        self.refresh_grid()

    def filter_data(self, *args):
        # Triggered by search var trace
        new_query = self.search_var.get().strip()
        if new_query != self.current_query:
            self.current_query = new_query
            self.current_page = 1 # Reset to page 1 on new search
            self.refresh_grid()

    def prev_page(self):
        if self.current_page > 1:
            self.current_page -= 1
            self.refresh_grid()

    def next_page(self):
        # Calculate max pages based on total_count
        total_pages = max(1, (self.total_count + self.ITEMS_PER_PAGE - 1) // self.ITEMS_PER_PAGE)
        if self.current_page < total_pages:
            self.current_page += 1
            self.refresh_grid()

    def refresh_grid(self):
        # 1. Get Count (for Pagination UI) - NOW INCLUDES FILTER
        self.total_count = self.db.get_product_count(
            query=self.current_query, 
            active_only=not self.show_archived,
            stock_status=self.stock_filter
        )
        
        # 2. Get Page Data (SQL Optimized)
        sort_col, sort_order = self.sort_options[self.current_sort_key]
        products_to_show = self.db.get_products_paginated(
            self.current_page, 
            self.ITEMS_PER_PAGE, 
            self.current_query,
            sort_by=sort_col,
            sort_order=sort_order,
            active_only=not self.show_archived,
            stock_status=self.stock_filter # PASS FILTER
        )
        
        # 3. Update Pagination UI
        total_pages = max(1, (self.total_count + self.ITEMS_PER_PAGE - 1) // self.ITEMS_PER_PAGE)
        
        # Clamp page if needed (e.g. if we were on page 10 and filtered to 5 results)
        if self.current_page > total_pages: 
            self.current_page = total_pages
            # Re-fetch if we adjusted page
            products_to_show = self.db.get_products_paginated(self.current_page, self.ITEMS_PER_PAGE, self.current_query)
            
        start_idx = (self.current_page - 1) * self.ITEMS_PER_PAGE + 1
        end_idx = min(start_idx + len(products_to_show) - 1, self.total_count)
        if self.total_count == 0: start_idx, end_idx = 0, 0
        
        self.lbl_page_info.configure(text=f"Page {self.current_page} of {total_pages} ({start_idx}-{end_idx} of {self.total_count})")
        self.btn_prev.configure(state="normal" if self.current_page > 1 else "disabled")
        self.btn_next.configure(state="normal" if self.current_page < total_pages else "disabled")

        # 4. Render
        self.display_products(products_to_show)

    def display_products(self, products_to_show):
        for widget in self.scroll_frame.winfo_children():
            widget.destroy()
        
        self.check_vars = {} 
        self.row_indices = {}
        self.products_by_index = {}
        self.row_widgets = {} # New: Store references for efficient updates

        for row_idx, p in enumerate(products_to_show):
            # Row Frame
            is_selected = p.product_id in self.selected_ids
            
            if is_selected:
                row_bg = "#204a87" # High vis dark blue
            elif row_idx % 2 == 0:
                row_bg = Colors.BG_CARD
            else:
                row_bg = Colors.BG_HOVER
                
            row_frame = ctk.CTkFrame(self.scroll_frame, fg_color=row_bg, corner_radius=6)
            row_frame.pack(fill="x", pady=2)
            self.row_widgets[p.product_id] = row_frame # Store ref
            
            # Match column configuration in Header
            row_frame.grid_columnconfigure(2, weight=1)
            
            # Map for shortcuts
            self.row_indices[p.product_id] = row_idx
            self.products_by_index[row_idx] = p
            
            # Click Events
            # Single Click: Select/Highlight (Logic handles modifiers)
            row_frame.bind("<Button-1>", lambda e, idx=row_idx, prod=p: self.handle_row_click(e, idx, prod))
            
            # Double Click: Edit (Only in normal mode)
            if not self.selection_mode:
                 row_frame.bind("<Double-Button-1>", lambda e, prod=p: self.edit_product(prod))

            # Selection Checkbox (Only if in Selection Mode)
            if self.selection_mode:
                var = ctk.BooleanVar(value=p.product_id in self.selected_ids)
                self.check_vars[p.product_id] = var
                cb = ctk.CTkCheckBox(row_frame, text="", variable=var, width=24, 
                                     command=lambda pid=p.product_id: self.on_item_select(pid))
                cb.grid(row=0, column=0, sticky="w", padx=(5,0))
                
                lbl_id = ctk.CTkLabel(row_frame, text=str(p.product_id), width=40)
                lbl_id.grid(row=0, column=0, padx=(30, 10), pady=8, sticky="w") 
                
                # Bind Row Click to toggle check
                row_frame.bind("<Button-1>", lambda e, idx=row_idx, prod=p: self.handle_row_click(e, idx, prod))
                
            else:
                ctk.CTkLabel(row_frame, text=str(p.product_id), width=40, text_color=Colors.TEXT_SECONDARY).grid(row=0, column=0, padx=10, pady=8, sticky="w")
            
            # Data
            # Col 1: Brand
            ctk.CTkLabel(row_frame, text=p.brand, width=120, text_color=Colors.TEXT_PRIMARY).grid(row=0, column=1, padx=10, pady=8, sticky="w")
            # Col 2: Description (inc Part No)
            desc_text = f"{p.description}\n{p.part_number}" if p.part_number else p.description
            ctk.CTkLabel(row_frame, text=desc_text, anchor="w", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=2, padx=10, pady=8, sticky="ew")
            
            # Col 3: Stock (Color coded)
            stock_color = Colors.TEXT_PRIMARY
            if p.stock_quantity <= 0: stock_color = Colors.DANGER
            elif p.stock_quantity <= p.low_stock_threshold: stock_color = Colors.WARNING
            
            lbl_stock = ctk.CTkLabel(row_frame, text=str(p.stock_quantity), width=80)
            lbl_stock.configure(text_color=stock_color)
            lbl_stock.grid(row=0, column=3, padx=10, pady=8, sticky="e")

            # Col 4: Price
            ctk.CTkLabel(row_frame, text=f"${p.selling_price:,.2f}", width=80, text_color=Colors.TEXT_PRIMARY).grid(row=0, column=4, padx=10, pady=8, sticky="e")
            
            # Col 5: Actions (Was Image, now Actions)
            action_frame = ctk.CTkFrame(row_frame, fg_color="transparent")
            action_frame.grid(row=0, column=5, padx=5, pady=8) # Changed col from 6 to 5
            
            # Edit
            if self.user.role == "Admin":
                btn_edit = ctk.CTkButton(action_frame, text="Edit", width=50, height=24, fg_color=Colors.BG_HOVER, hover_color=Colors.PRIMARY,
                                        command=lambda prod=p: self.edit_product(prod))
                btn_edit.pack(side="left", padx=2)
            
            # Print
            btn_print = ctk.CTkButton(action_frame, text="Print Label", width=80, height=24, fg_color=Colors.INFO, hover_color=Colors.NEON_PURPLE,
                                     text_color=Colors.TEXT_ON_NEON,
                                     command=lambda prod=p: self.print_barcode_event(prod))
            btn_print.pack(side="left", padx=2)
            ToolTip.create(btn_print, "Print Barcode Label")

            # Archive / Restore
            if self.user.role == "Admin":
                if p.is_active:
                    btn_arch = ctk.CTkButton(action_frame, text="Archive", width=60, height=24, fg_color=Colors.WARNING, text_color="black",
                                           command=lambda prod=p: self.archive_product_event(prod))
                    btn_arch.pack(side="left", padx=2)
                else:
                    btn_rest = ctk.CTkButton(action_frame, text="Restore", width=60, height=24, fg_color=Colors.SUCCESS,
                                           command=lambda prod=p: self.restore_product_event(prod))
                    btn_rest.pack(side="left", padx=2)
                
            # Bind click to labels too to ensure row click works
            for child in row_frame.winfo_children():
                if isinstance(child, ctk.CTkLabel):
                    child.bind("<Button-1>", lambda e, idx=row_idx, prod=p: self.handle_row_click(e, idx, prod))
                    if not self.selection_mode:
                        child.bind("<Double-Button-1>", lambda e, prod=p: self.edit_product(prod))

    def edit_product(self, product):
        if self.selection_mode: return # Disable edit in select mode
        
        self.editing_product = product
        if not self.form_frame.winfo_viewable():
            self.toggle_form()
            
        self.btn_save.configure(text="Update Product")
        self.btn_add.configure(text="Editing Mode")
        
        # Populate
        for field, entry in self.entries.items():
            entry.delete(0, "end")
            val = getattr(product, field, "")
            if val is not None:
                entry.insert(0, str(val))
        
        # Populate Image
        self.entry_image.delete(0, "end")
        if product.image_path:
            self.entry_image.insert(0, product.image_path)

    def clear_entries(self):
        for entry in self.entries.values():
            entry.delete(0, "end")
        self.entry_image.delete(0, "end")
        self.btn_save.configure(text="Save Product")

    def save_product(self):
        try:
            data = {}
            for field, entry in self.entries.items():
                val = entry.get().strip()
                # Numeric validation
                if field in ["purchase_cost", "selling_price"]:
                    val = float(val) if val else 0.0
                elif field in ["stock_quantity", "low_stock_threshold", "reorder_point", "target_stock"]:
                    val = int(val) if val else 0
                data[field] = val
            
            image_path = self.entry_image.get().strip()
            
            new_product = Product(
                product_id=self.editing_product.product_id if self.editing_product else 0,
                barcode=data["barcode"],
                part_number=data["part_number"],
                brand=data["brand"],
                description=data["description"],
                volume=data["volume"],
                type=data["type"],
                application=data["application"],
                purchase_cost=data["purchase_cost"],
                selling_price=data["selling_price"],
                stock_quantity=data["stock_quantity"],
                notes=data["notes"],
                low_stock_threshold=data["low_stock_threshold"],
                date_created="", 
                date_modified="", 
                reorder_point=data.get("reorder_point", 5),
                target_stock=data.get("target_stock", 10),
                image_path=image_path
            )

            if self.editing_product:
                self.db.update_product(new_product)
                messagebox.showinfo("Success", "Product updated successfully!")
            else:
                self.db.add_product(new_product)
                messagebox.showinfo("Success", "Product added successfully!")

            self.load_data()
            self.toggle_form()

        except ValueError:
            messagebox.showerror("Validation Error", "Please ensure numeric fields (Price, Stock) contain valid numbers.")
        except Exception as e:
            messagebox.showerror("Error", f"Failed to save product: {str(e)}")

    def generate_barcode_value(self):
        val = ''.join(random.choices(string.digits, k=8))
        self.entries['barcode'].delete(0, "end")
        self.entries['barcode'].insert(0, val)

    def print_label(self):
        code = self.entries['barcode'].get()
        name = self.entries['description'].get()
        try:
            price = float(self.entries['selling_price'].get())
        except:
            price = 0.0
        
        if not code:
            messagebox.showwarning("Missing", "Barcode is required.")
            return

        generator = BarcodeGenerator()
        pdf_path = generator.generate_label(name, code, price)
        
        if pdf_path:
            success = print_file(pdf_path)
            if success:
                messagebox.showinfo("Printed", "Label sent to printer.")
            else:
                messagebox.showerror("Error", "Could not send label to printer.")
        else:
            messagebox.showerror("Error", "Failed to generate label.")

    def delete_product_event(self, product):
        if messagebox.askyesno("Confirm Delete", f"Are you sure you want to delete '{product.description}'?\n\nThis cannot be undone."):
            success = self.db.delete_product(product.product_id)
            if success:
                messagebox.showinfo("Deleted", "Product deleted successfully.")
                self.load_data()
            else:
                messagebox.showerror("Cannot Delete", "Could not delete product.\nIt likely has associated Sales or Transaction history.\n\nTip: You can edit it to be 'Inactive' or just rename it.")

    def archive_product_event(self, product):
        if messagebox.askyesno("Confirm Archive", f"Archive '{product.description}'?\nIt will be hidden from main lists."):
            self.db.archive_product(product.product_id)
            self.load_data()

    def restore_product_event(self, product):
        self.db.restore_product(product.product_id)
        self.load_data()

    def toggle_archived_view(self):
        self.show_archived = bool(self.switch_archive.get())
        self.current_page = 1
        # Change Main Title or Button style to indicate mode?
        self.refresh_grid()

    def export_csv(self):

        try:
            filename = filedialog.asksaveasfilename(
                title="Export Inventory",
                defaultextension=".csv",
                filetypes=[("CSV Files", "*.csv"), ("Excel Files", "*.xlsx")]
            )
            if not filename: return
            
            # Fetch ALL data
            products = self.db.get_products_paginated(1, 100000, self.current_query, 
                                                      sort_by=self.current_sort_key, 
                                                      active_only=not self.show_archived)
            
            data_list = []
            for p in products:
                status = "Active" if p.is_active else "Archived"
                val = p.stock_quantity * p.purchase_cost
                data_list.append({
                    "ID": p.product_id,
                    "Barcode": p.barcode,
                    "PartNumber": p.part_number,
                    "Brand": p.brand,
                    "Description": p.description,
                    "Stock": p.stock_quantity,
                    "Price": p.selling_price,
                    "Cost": p.purchase_cost,
                    "Value": val,
                    "Status": status
                })
            
            if filename.endswith(".xlsx"):
                import pandas as pd
                df = pd.DataFrame(data_list)
                df.to_excel(filename, index=False)
                messagebox.showinfo("Export Success", f"Exported {len(products)} items to Excel.")
            else:
                # CSV Fallback
                import csv
                with open(filename, 'w', newline='', encoding='utf-8') as f:
                    writer = csv.DictWriter(f, fieldnames=data_list[0].keys())
                    writer.writeheader()
                    writer.writerows(data_list)
                messagebox.showinfo("Export Success", f"Exported {len(products)} items to CSV.")
            
        except Exception as e:
            messagebox.showerror("Export Failed", str(e))

    # --- Bulk Edit Logic ---

    def toggle_selection_mode(self):
        self.selection_mode = not self.selection_mode
        self.selected_ids.clear()
        
        if self.selection_mode:
            self.btn_bulk.configure(text="Cancel Select", fg_color=Colors.DANGER)
            self.load_data() # Refresh list to show checkboxes
        else:
            self.btn_bulk.configure(text="Select Items", fg_color=Colors.BG_CARD)
            self.bulk_bar.place_forget() # Hide bottom bar
            self.load_data()

    def on_item_select(self, product_id):
        var = self.check_vars.get(product_id)
        if var and var.get():
            self.selected_ids.add(product_id)
        else:
            self.selected_ids.discard(product_id)
        
        self.update_bulk_bar()

    def update_bulk_bar(self):
        if self.selection_mode and len(self.selected_ids) > 0:
            self.lbl_selected_count.configure(text=f"{len(self.selected_ids)} Items Selected")
            self.bulk_bar.place(relx=0, rely=1, anchor="sw", relwidth=1)
        else:
            self.bulk_bar.place_forget()

    def open_bulk_edit(self):
        if not self.selected_ids: return
        
        # If single item -> Full Edit Mode
        if len(self.selected_ids) == 1:
            pid = list(self.selected_ids)[0]
            product = self.db.get_product_by_id(pid)
            if product:
                self.toggle_selection_mode() # Exit select mode
                self.edit_product(product) # Open form
            return

        # Multiple -> Bulk Dialog
        dialog = BulkEditDialog(self, len(self.selected_ids))
        self.wait_window(dialog)
        
        if dialog.result:
            field, value = dialog.result
            self.execute_bulk_update(field, value)

    def execute_bulk_update(self, field, value):
        try:
            # Optimize: In a real app, db would have a bulk_update method.
            # Here, we'll iterate. It's safe for <1000 items.
            count = 0
            for pid in self.selected_ids:
                # 1. Get Product
                p = self.db.get_product_by_id(pid)
                if not p: continue
                
                # 2. Update Attribute
                if field in ["purchase_cost", "selling_price"]:
                    val = float(value)
                elif field in ["stock_quantity", "low_stock_threshold", "reorder_point"]:
                    val = int(value)
                else:
                    val = value
                    
                setattr(p, field, val)
                
                # 3. Save
                self.db.update_product(p)
                count += 1
                
            messagebox.showinfo("Success", f"Updated {count} products successfully!")
            self.toggle_selection_mode() # Exit mode
            
        except Exception as e:
            messagebox.showerror("Error", f"Bulk update failed: {str(e)}")

    def _bind_shortcuts(self, event=None):
        # Bind to the top-level window so shortcuts work globally while this view is active
        root = self.winfo_toplevel()
        root.bind("<Control-a>", self.select_all_event)
        root.bind("<Control-n>", lambda e: self.toggle_form())

    def _unbind_shortcuts(self, event=None):
        # Unbind when view is hidden to prevent conflicts
        root = self.winfo_toplevel()
        root.unbind("<Control-a>")
        root.unbind("<Control-n>")

    def select_all_event(self, event=None):
        # Enter selection mode if not already
        if not self.selection_mode:
            self.toggle_selection_mode()
            
        # Optimization: Fetch IDs based on current query from DB
        all_ids = self.db.get_all_product_ids(self.current_query)
        self.selected_ids = set(all_ids)
        
        self.load_data() # Refresh UI
        self.update_bulk_bar()

    def handle_row_click(self, event, index, product):
        ctrl_pressed = (event.state & 0x0004) != 0
        shift_pressed = (event.state & 0x0001) != 0
        
        # If standard click in normal mode -> Edit
        if shift_pressed and self.last_clicked_index != -1:
             start = min(self.last_clicked_index, index)
             end = max(self.last_clicked_index, index)
             
             # Efficient Range Add
             for i in range(start, end + 1):
                 p = self.products_by_index.get(i)
                 if p: self.selected_ids.add(p.product_id)
        
        elif ctrl_pressed:
             if product.product_id in self.selected_ids:
                 self.selected_ids.discard(product.product_id)
             else:
                 self.selected_ids.add(product.product_id)
        else:
             # Standard Click: Select Single (Clear others)
             # Logic: If I click row 5, I select row 5 and deselect everything else.
             # This feels standard for lists.
             
             # Check if we should Toggle or set Single?
             # Standard OS: Click selects single.
             # User issue: "Instantly goes to edit" -> Now fixed by double click.
             
             if not self.selection_mode:
                # If NOT in selection mode, maybe we just highlight this one row?
                # Or do we treat it as selection?
                # Behaving like standard list: 
                self.selected_ids.clear()
                self.selected_ids.add(product.product_id)
             else:
                 # In explicit checkbox mode, maybe toggle?
                 if product.product_id in self.selected_ids:
                     self.selected_ids.discard(product.product_id)
                 else:
                     self.selected_ids.add(product.product_id)
                 
        self.last_clicked_index = index
        self.update_row_visuals() # Fast update
        self.update_bulk_bar()

    def update_row_visuals(self):
        """Diff update of row colors"""
        for pid, frame in self.row_widgets.items():
            if not frame.winfo_exists(): continue
            
            is_selected = pid in self.selected_ids
            # Get index to determine stripe
            idx = self.row_indices.get(pid, 0)
            
            if is_selected:
                target_color = "#204a87" 
            elif idx % 2 == 0:
                target_color = Colors.BG_CARD
            else:
                target_color = Colors.BG_HOVER
                
            if frame._fg_color != target_color:
                frame.configure(fg_color=target_color)
                
            # Update Checkbox if exists
            if pid in self.check_vars:
                self.check_vars[pid].set(is_selected)

    def print_selected_labels(self):
        if not self.selected_ids: return
        
        if not messagebox.askyesno("Confirm Print", f"Print labels for {len(self.selected_ids)} items?"):
            return
            
        generator = BarcodeGenerator()
        count = 0
        failed = 0
        
        for pid in self.selected_ids:
            product = self.db.get_product_by_id(pid)
            if not product or not product.barcode:
                failed += 1
                continue
                
            pdf = generator.generate_label(product.description, product.barcode, product.selling_price)
            if pdf and print_file(pdf):
                count += 1
            else:
                failed += 1
        
        msg = f"Successfully sent {count} labels to printer."
        if failed > 0:
            msg += f"\nFailed: {failed} (Missing barcode or print error)"
            
        messagebox.showinfo("Result", msg)

    def print_barcode_event(self, product):
        if not product.barcode:
             messagebox.showwarning("No Barcode", f"Product '{product.description}' has no barcode.")
             return
             
        if messagebox.askyesno("Print Label", f"Print barcode label for:\n{product.description}\nBarcode: {product.barcode}?"):
            generator = BarcodeGenerator()
            pdf_path = generator.generate_label(product.description, product.barcode, product.selling_price)
            
            if pdf_path:
                success = print_file(pdf_path)
                if success:
                    messagebox.showinfo("Printed", "Label sent to printer.")
                else:
                    messagebox.showerror("Error", "Failed to print label.")
            else:
                 messagebox.showerror("Error", "Failed to generate label.")
    def export_csv(self):
        try:
            filename = filedialog.asksaveasfilename(defaultextension=".csv", filetypes=[("CSV Files", "*.csv")])
            if not filename: return
            
            # Use current filter if any, or all products
            # For simplicity, let's export ALL products for now, or use visible.
            # Let's export ALL to be consistent with Settings generic export.
            products = self.db.get_all_products()
            
            # We can use DataManager if available, or inline csv writing.
            # Given we saw DataManager in SettingsView, let's try to import it or just use csv module effectively.
            import csv
            with open(filename, mode='w', newline='', encoding='utf-8') as f:
                writer = csv.writer(f)
                writer.writerow(["ID", "Barcode", "Brand", "Description", "Stock", "Price", "Cost"])
                for p in products:
                    writer.writerow([
                        p.product_id,
                        p.barcode,
                        p.brand,
                        p.description,
                        p.stock_quantity,
                        p.selling_price,
                        p.purchase_cost
                    ])
                    
            messagebox.showinfo("Success", f"Exported {len(products)} products to {filename}")
            
        except Exception as e:
            messagebox.showerror("Export Failed", str(e))
