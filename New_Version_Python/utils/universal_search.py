
import customtkinter as ctk
from ui.styles import Colors, Dimens

class UniversalSearchBar:
    def __init__(self, parent_window, db, callback):
        self.db = db
        self.callback = callback
        self.parent = parent_window
        
        # Create Overlay (Toplevel or Frame)
        # Using Toplevel for modal-ish behavior
        self.window = ctk.CTkToplevel(parent_window)
        self.window.title("Search")
        self.window.geometry("600x400")
        self.window.transient(parent_window)
        self.window.configure(fg_color=Colors.BG_DARK)
        
        # Center the window
        # self.center_window() # optional
        
        # Search Entry
        self.entry = ctk.CTkEntry(self.window, placeholder_text="Search Products, Customers...", 
                                  height=50, font=("Arial", 16),
                                  fg_color=Colors.BG_CARD, border_color=Colors.NEON_BLUE)
        self.entry.pack(fill="x", padx=20, pady=20)
        self.entry.bind("<KeyRelease>", self.on_search)
        self.entry.focus_set()
        
        # Results List
        self.scroll = ctk.CTkScrollableFrame(self.window, fg_color="transparent")
        self.scroll.pack(fill="both", expand=True, padx=20, pady=(0, 20))
        
        # Close on Escape
        self.window.bind("<Escape>", lambda e: self.window.destroy())

    def on_search(self, event):
        query = self.entry.get().strip().lower()
        if len(query) < 2:
            self.clear_results()
            return
            
        results = []
        
        # Search Products
        products = self.db.get_all_products() # Inefficient for real usage, but ok for prototype
        for p in products:
            if query in p.description.lower() or query in p.brand.lower() or query in p.barcode:
                results.append({"type": "Product", "label": f"📦 {p.description}", "obj": p})
                
        # Search Customers (if available)
        if hasattr(self.db, 'get_all_customers'):
            customers = self.db.get_all_customers()
            for c in customers:
                if query in c.name.lower():
                    results.append({"type": "Customer", "label": f"👥 {c.name}", "obj": c})

        self.show_results(results)

    def clear_results(self):
        for w in self.scroll.winfo_children():
            w.destroy()

    def show_results(self, results):
        self.clear_results()
        
        if not results:
            ctk.CTkLabel(self.scroll, text="No results found", text_color=Colors.TEXT_SECONDARY).pack(pady=20)
            return

        for r in results[:15]: # Limit
            btn = ctk.CTkButton(self.scroll, text=r['label'], fg_color=Colors.BG_CARD, 
                                hover_color=Colors.BG_HOVER, anchor="w", height=40,
                                command=lambda res=r: self.select_result(res))
            btn.pack(fill="x", pady=2)

    def select_result(self, result):
        self.window.destroy()
        if self.callback:
            self.callback(result)
