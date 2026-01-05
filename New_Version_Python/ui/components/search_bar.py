import customtkinter as ctk
from ui.styles import Colors, Icons

class SearchBar(ctk.CTkFrame):
    def __init__(self, parent, width=300, on_search=None, placeholder="Search..."):
        super().__init__(parent, fg_color="transparent", width=width)
        self.on_search_callback = on_search
        
        self.pack_propagate(False)
        
        # Container
        self.container = ctk.CTkFrame(self, fg_color=Colors.BG_CARD, corner_radius=20, border_width=1, border_color=Colors.BG_HOVER)
        self.container.pack(fill="both", expand=True)
        
        # Icon
        self.lbl_icon = ctk.CTkLabel(self.container, text="🔍", text_color=Colors.TEXT_SECONDARY)
        self.lbl_icon.pack(side="left", padx=(10, 5))
        
        # Entry
        self.entry = ctk.CTkEntry(self.container, placeholder_text=placeholder, 
                                  fg_color="transparent", border_width=0, 
                                  text_color=Colors.TEXT_PRIMARY, font=("Arial", 13))
        self.entry.pack(side="left", fill="both", expand=True, padx=(0, 10))
        
        self.entry.bind("<Return>", self.trigger_search)
        self.entry.bind("<KeyRelease>", self.on_key)

    def trigger_search(self, event=None):
        if self.on_search_callback:
            self.on_search_callback(self.entry.get())

    def on_key(self, event=None):
        pass # Optional: Real-time search if needed
        
    def get(self):
        return self.entry.get()

    def clear(self):
        self.entry.delete(0, 'end')
