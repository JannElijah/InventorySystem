import customtkinter as ctk
from tkinter import messagebox
from datetime import datetime
from database import DatabaseRepository
from models import Product, PurchaseOrder

class PurchasingView(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository, user):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        self.user = user
        self.po_cart = [] # List of {product, qty, cost}

        # Tabs
        self.tabview = ctk.CTkTabview(self)
        self.tabview.pack(fill="both", expand=True, padx=20, pady=10)
        
        self.tab_orders = self.tabview.add("My Purchase Orders")
        self.tab_create = self.tabview.add("Create PO")
        self.tab_receive = self.tabview.add("Receive Stock")
        self.tab_quick = self.tabview.add("Quick Stock-In")
        
        self.setup_orders_tab()
        self.setup_create_po_tab()
        self.setup_receive_tab()
        self.setup_quick_stock_in_tab()

    def setup_orders_tab(self):
        # List all POs
        self.orders_frame = ctk.CTkScrollableFrame(self.tab_orders, fg_color="transparent")
        self.orders_frame.pack(fill="both", expand=True)
        self.refresh_orders_list()
        
        ctk.CTkButton(self.tab_orders, text="Refresh List", command=self.refresh_orders_list).pack(pady=10)

    def refresh_orders_list(self):
        for w in self.orders_frame.winfo_children(): w.destroy()
        
        pos = self.db.get_all_purchase_orders()
        headers = ["ID", "Supplier", "Date", "Status", "Total", "Action"]
        
        # Header Row
        h_frame = ctk.CTkFrame(self.orders_frame)
        h_frame.pack(fill="x", pady=2)
        for t in headers:
            ctk.CTkLabel(h_frame, text=t, width=100, font=("Arial", 12, "bold")).pack(side="left", padx=5)

        for po in pos:
            row = ctk.CTkFrame(self.orders_frame)
            row.pack(fill="x", pady=2)
            
            ctk.CTkLabel(row, text=str(po.po_id), width=100).pack(side="left", padx=5)
            ctk.CTkLabel(row, text=po.supplier_name, width=100).pack(side="left", padx=5)
            dt_str = po.order_date.strftime("%Y-%m-%d") if isinstance(po.order_date, datetime) else str(po.order_date)[:10]
            ctk.CTkLabel(row, text=dt_str, width=100).pack(side="left", padx=5)
            
            # Status Color
            color = "gray"
            if po.status == "Ordered": color = "orange"
            elif po.status == "Completed": color = "green"
            
            ctk.CTkLabel(row, text=po.status, width=100, text_color=color).pack(side="left", padx=5)
            ctk.CTkLabel(row, text=f"${po.total_cost:.2f}", width=100).pack(side="left", padx=5)
            
            if po.status != "Completed":
                ctk.CTkButton(row, text="Receive", width=80, command=lambda p=po: self.go_to_receive(p)).pack(side="left", padx=5)

    def go_to_receive(self, po):
        self.tabview.set("Receive Stock")
        self.load_receive_ui(po)

    # --- Create PO ---
    def setup_create_po_tab(self):
        # Layout: Left (Form), Right (Cart)
        self.tab_create.grid_columnconfigure(0, weight=1)
        self.tab_create.grid_columnconfigure(1, weight=1)
        self.tab_create.grid_rowconfigure(0, weight=1)
        
        # Left
        l_frame = ctk.CTkFrame(self.tab_create)
        l_frame.grid(row=0, column=0, sticky="nsew", padx=10, pady=10)
        
        ctk.CTkLabel(l_frame, text="1. Select Supplier").pack(pady=5)
        self.suppliers = self.db.get_all_suppliers()
        sup_names = [s.name for s in self.suppliers]
        self.om_supplier = ctk.CTkOptionMenu(l_frame, values=sup_names,
                                             fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, button_hover_color=Colors.BG_HOVER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.om_supplier.pack(pady=5)
        
        ctk.CTkLabel(l_frame, text="2. Add Products").pack(pady=10)
        self.entry_search = ctk.CTkEntry(l_frame, placeholder_text="Enter ID or Barcode...")
        self.entry_search.pack(fill="x", padx=10)
        
        self.entry_qty = ctk.CTkEntry(l_frame, placeholder_text="Quantity")
        self.entry_qty.pack(fill="x", padx=10, pady=5)
        
        self.entry_cost = ctk.CTkEntry(l_frame, placeholder_text="Unit Cost")
        self.entry_cost.pack(fill="x", padx=10, pady=5)
        
        ctk.CTkButton(l_frame, text="Add Line Item", command=self.add_po_line).pack(pady=10)
        
        ctk.CTkButton(l_frame, text="Auto-Fill Low Stock", command=self.autofill_low_stock, fg_color=Colors.WARNING, text_color=Colors.TEXT_ON_NEON).pack(pady=20)
        
        # Right (Cart)
        r_frame = ctk.CTkFrame(self.tab_create)
        r_frame.grid(row=0, column=1, sticky="nsew", padx=10, pady=10)
        
        ctk.CTkLabel(r_frame, text="Order Items").pack(pady=5)
        self.po_cart_frame = ctk.CTkScrollableFrame(r_frame)
        self.po_cart_frame.pack(fill="both", expand=True)
        
        ctk.CTkButton(r_frame, text="Submit Order", command=self.submit_po, fg_color=Colors.SUCCESS, text_color=Colors.TEXT_ON_NEON).pack(pady=10)

    def add_po_line(self):
        query = self.entry_search.get().strip()
        if not query: return
        
        # Find product
        prod = None
        for p in self.db.get_all_products():
             if str(p.product_id) == query or p.barcode == query or p.description.lower() == query.lower():
                 prod = p
                 break
        
        if not prod:
            messagebox.showerror("Error", "Product not found")
            return
            
        try:
            qty = int(self.entry_qty.get())
            cost = float(self.entry_cost.get())
        except:
             messagebox.showerror("Error", "Invalid Quantity or Cost")
             return

        self.po_cart.append({"product": prod, "product_id": prod.product_id, "qty": qty, "cost": cost})
        self.refresh_po_cart()
        self.entry_search.delete(0, "end")
        self.entry_qty.delete(0, "end")
        self.entry_cost.delete(0, "end")

    def refresh_po_cart(self):
        for w in self.po_cart_frame.winfo_children(): w.destroy()
        
        for item in self.po_cart:
            txt = f"{item['product'].description} x{item['qty']} @ ${item['cost']}"
            ctk.CTkLabel(self.po_cart_frame, text=txt, anchor="w").pack(fill="x")

    def autofill_low_stock(self):
        products = self.db.get_all_products()
        added_count = 0
        for p in products:
            if p.stock_quantity < p.reorder_point:
                # Calculate needed to reach target
                needed = p.target_stock - p.stock_quantity
                if needed > 0:
                    self.po_cart.append({
                        "product": p,
                        "product_id": p.product_id,
                        "qty": needed,
                        "cost": p.purchase_cost
                    })
                    added_count += 1
        self.refresh_po_cart()
        messagebox.showinfo("Autofill", f"Added {added_count} items below reorder point.")

    def submit_po(self):
        if not self.po_cart: return
        
        sup_name = self.om_supplier.get()
        sup_id = next((s.supplier_id for s in self.suppliers if s.name == sup_name), None)
        
        if not sup_id:
             messagebox.showerror("Error", "Select Supplier")
             return
             
        try:
            self.db.create_purchase_order(sup_id, self.po_cart)
            messagebox.showinfo("Success", "Purchase Order Created")
            self.po_cart = []
            self.refresh_po_cart()
            self.refresh_orders_list()
            self.tabview.set("My Purchase Orders")
        except Exception as e:
            messagebox.showerror("Error", str(e))

    # --- Receive Stock ---
    def setup_receive_tab(self):
        self.receive_label = ctk.CTkLabel(self.tab_receive, text="Select a PO from 'My Purchase Orders' to receive.")
        self.receive_label.pack(pady=20)
        self.receive_frame = ctk.CTkFrame(self.tab_receive)
        self.active_receive_po = None
        self.receive_entries = {} # {pid: entry_widget}

    def load_receive_ui(self, po):
        self.receive_label.pack_forget()
        self.receive_frame.pack(fill="both", expand=True, padx=20, pady=10)
        for w in self.receive_frame.winfo_children(): w.destroy()
        
        self.active_receive_po = po
        ctk.CTkLabel(self.receive_frame, text=f"Receiving PO #{po.po_id} from {po.supplier_name}", font=("Arial", 16, "bold")).pack(pady=10)
        
        items = self.db.get_po_items(po.po_id)
        
        self.receive_entries = {}
        for item in items:
            row = ctk.CTkFrame(self.receive_frame)
            row.pack(fill="x", pady=5)
            
            remaining = item.quantity_ordered - item.quantity_received
            
            ctk.CTkLabel(row, text=item.product_name, width=200).pack(side="left")
            ctk.CTkLabel(row, text=f"Ord: {item.quantity_ordered} | Rec: {item.quantity_received}", width=150).pack(side="left")
            
            if remaining > 0:
                e = ctk.CTkEntry(row, width=80)
                e.insert(0, str(remaining))
                e.pack(side="left", padx=10)
                self.receive_entries[item.product_id] = e
            else:
                ctk.CTkLabel(row, text="Done", text_color="green").pack(side="left")
        
        ctk.CTkButton(self.receive_frame, text="Confirm Receipt", command=self.process_receipt, fg_color=Colors.SUCCESS, text_color=Colors.TEXT_ON_NEON).pack(pady=20)

    def process_receipt(self):
        if not self.active_receive_po: return
        
        received_map = {}
        try:
            for pid, entry in self.receive_entries.items():
                qty = int(entry.get())
                received_map[pid] = qty
            
            self.db.receive_po_items(self.active_receive_po.po_id, received_map, self.user.user_id)
            messagebox.showinfo("Success", "Items received and added to inventory.")
            self.receive_frame.pack_forget()
            self.receive_label.pack()
            self.refresh_orders_list()
        except Exception as e:
            messagebox.showerror("Error", str(e))

    # --- Quick Stock In ---
    def setup_quick_stock_in_tab(self):
        # Re-implement simple logic
        ctk.CTkLabel(self.tab_quick, text="Direct Stock Addition (Legacy)", font=("Arial", 16)).pack(pady=20)
        # Simplified for brevity, user can switch to real POs
        ctk.CTkLabel(self.tab_quick, text="Use 'Create PO' for tracked purchases. Use this for adjustments.").pack()
