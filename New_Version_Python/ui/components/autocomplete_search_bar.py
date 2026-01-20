import customtkinter as ctk
from ui.styles import Colors

class AutocompleteSearchBar(ctk.CTkFrame):
    def __init__(self, parent, db, on_select_callback, width=300, placeholder="Search Products..."):
        super().__init__(parent, fg_color="transparent", width=width, height=50)
        self.db = db
        self.on_select = on_select_callback
        
        self.pack_propagate(False)
        
        # Main Container (Visual Border)
        self.container = ctk.CTkFrame(self, fg_color=Colors.BG_INPUT, corner_radius=10, 
                                      border_width=1, border_color=Colors.BORDER)
        self.container.pack(fill="both", expand=True)
        
        # Icon
        self.lbl_icon = ctk.CTkLabel(self.container, text="🔍", text_color=Colors.TEXT_SECONDARY, font=("Arial", 16))
        self.lbl_icon.pack(side="left", padx=(15, 10))
        
        # Entry
        self.entry = ctk.CTkEntry(self.container, placeholder_text=placeholder, 
                                  fg_color="transparent", border_width=0, 
                                  text_color=Colors.TEXT_PRIMARY, font=("Arial", 14),
                                  height=40)
        self.entry.pack(side="left", fill="both", expand=True, padx=(0, 15), pady=5)
        
        self.entry.bind("<KeyRelease>", self.on_key_release)
        self.entry.bind("<FocusOut>", self.on_focus_out)
        self.entry.bind("<Return>", self.handle_enter_press)
        
        self.suggestion_window = None

        # Status Label (Hidden by default)
        self.lbl_status = ctk.CTkLabel(self, text="", font=("Arial", 12), text_color=Colors.SUCCESS)
        self.lbl_status.pack(pady=(2, 0))

    def handle_enter_press(self, event):
        query = self.entry.get().strip()
        if not query: return
        
        # Priority: Exact Barcode Match
        product = self.db.get_product_by_barcode(query)
        if product:
            self.select_item(product)
            self.show_status(f"✅ Scanned: {product.description}")
            return

        # If no exact match, try fuzzy (single result auto-select?)
        # For safety, if ambiguous, we just show suggestions.
        # But for scanner, if it sent Enter, it expects action.
        results = self.db.search_products(query, limit=1)
        if len(results) == 1:
            self.select_item(results[0])
            self.show_status(f"✅ Scanned: {results[0].description}")
        else:
            self.show_status("⚠️ Not found", is_error=True)
            # Re-trigger suggestions if valid
            self.on_key_release(event)

    def show_status(self, text, is_error=False):
        color = Colors.DANGER if is_error else Colors.SUCCESS
        self.lbl_status.configure(text=text, text_color=color)
        # Auto-hide after 3 seconds
        self.after(3000, lambda: self.lbl_status.configure(text=""))

    def on_key_release(self, event):
        if event.keysym in ["Up", "Down", "Return", "Escape"]: return
        query = self.entry.get().strip()
        if len(query) < 1:
            self.hide_suggestions()
            return
        results = self.db.search_products(query, limit=8)
        self.show_suggestions(results)

    def show_suggestions(self, results):
        if not results:
            self.hide_suggestions()
            return

        if self.suggestion_window is None:
            self.suggestion_window = ctk.CTkToplevel(self)
            self.suggestion_window.wm_overrideredirect(True)
            self.suggestion_window.configure(fg_color=Colors.BG_DARK)

        # Calculate Position & Size
        # Force update to ensure verify coordinates
        self.container.update_idletasks()
        
        x = self.container.winfo_rootx()
        y = self.container.winfo_rooty() + self.container.winfo_height() + 2
        w = self.container.winfo_width()
        
        if w < 100: w = 300 # Fallback safety
        
        # Height: min of results or max height
        row_height = 45
        total_height = min(len(results) * row_height, 300) 
        
        self.suggestion_window.geometry(f"{w}x{total_height}+{x}+{y}")
        
        # Clear old
        for widget in self.suggestion_window.winfo_children():
            widget.destroy()
            
        # Scrollable Frame for results (gives nice rounded look and scrollbar)
        self.scroll_frame = ctk.CTkScrollableFrame(self.suggestion_window, fg_color=Colors.BG_CARD, 
                                                   corner_radius=10, border_width=1, border_color=Colors.NEON_BLUE)
        self.scroll_frame.pack(fill="both", expand=True)
        
        for p in results:
            self.create_suggestion_row(p)

    def create_suggestion_row(self, p):
        # Row Button
        # Truncate long text
        desc = p.description
        if len(desc) > 35: desc = desc[:32] + "..."
        
        text = f" {p.brand} - {desc}"
        
        btn = ctk.CTkButton(self.scroll_frame, 
                            text=text, 
                            font=("Arial", 13),
                            anchor="w",
                            fg_color="transparent",
                            hover_color=Colors.BG_HOVER,
                            text_color=Colors.TEXT_PRIMARY,
                            height=40,
                            command=lambda prod=p: self.select_item(prod))
        btn.pack(fill="x", pady=1)

    def hide_suggestions(self):
        if self.suggestion_window:
            self.suggestion_window.destroy()
            self.suggestion_window = None

    def select_item(self, product):
        self.entry.delete(0, 'end')
        self.hide_suggestions()
        if self.on_select:
            self.on_select(product)

    def on_focus_out(self, event):
        self.after(200, self.hide_suggestions)
        
    def clear(self):
        self.entry.delete(0, 'end')
        self.hide_suggestions()
