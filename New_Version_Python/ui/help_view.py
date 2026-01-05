import customtkinter as ctk

class HelpView(ctk.CTkFrame):
    def __init__(self, parent):
        super().__init__(parent, fg_color="transparent")
        
        self.scroll = ctk.CTkScrollableFrame(self, fg_color="transparent")
        self.scroll.pack(fill="both", expand=True, padx=20, pady=20)
        
        # Title
        ctk.CTkLabel(self.scroll, text="Help & Support", font=("Arial", 28, "bold")).pack(anchor="w", pady=(0, 20))
        
        # --- About ---
        self.create_section("About System 2.0", 
                            "This Inventory Management System (v2.0) is a modern, professional solution "
                            "designed for efficiency. It features real-time analytics, user accountability,"
                            "and improved compatibility for all screen sizes.")
                            
        # --- FAQ ---
        self.create_section("How do I add stock?", 
                            "Go to the 'Receive Supply' tab (via 'Dashboard'). Scan your items and confirm.")
                            
        self.create_section("How do I make a sale?", 
                            "Go to 'Point of Sale'. Scan items or search by name. Click 'Checkout' to finish.")
                            
        self.create_section("Why is the screen small?", 
                            "You can adjust the Zoom Level in 'Settings'. Try 120% for large monitors.")
                            
        self.create_section("Where are the reports?", 
                            "Click 'Reports' in the sidebar to generate PDF summaries of inventory and sales.")

        # --- Support ---
        ctk.CTkLabel(self.scroll, text="Need more help? Contact Admin.", text_color="gray70").pack(pady=30)

    def create_section(self, title, content):
        f = ctk.CTkFrame(self.scroll, fg_color=("gray90", "gray20"))
        f.pack(fill="x", pady=10)
        
        ctk.CTkLabel(f, text=title, font=("Arial", 16, "bold"), anchor="w").pack(fill="x", padx=15, pady=(10, 5))
        ctk.CTkLabel(f, text=content, font=("Arial", 14), anchor="w", justify="left", wraplength=800).pack(fill="x", padx=15, pady=(0, 10))
