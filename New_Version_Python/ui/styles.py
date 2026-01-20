
import customtkinter as ctk

class Colors:
    # Professional Dark Palette (Less White, More Sleek)
    BG_DARK = "#0d1117"      # Deep Navy/Black (GitHub Dark style)
    BG_CARD = "#161b22"      # Card Background
    BG_HOVER = "#21262d"     # Hover/Input Background
    BG_INPUT = "#0d1117"     # Dark Input Background (vs White)
    BORDER = "#30363d"       # Subtle Border
    
    # Accents (Professional Muted)
    NEON_BLUE = "#58a6ff"    # Professional Blue
    NEON_GREEN = "#238636"   # Success Green (Darker)
    NEON_RED = "#da3633"     # Error Red
    NEON_ORANGE = "#d29922"  # Warning
    NEON_PURPLE = "#8957e5"  # Accent

    # Semantic Mapping
    PRIMARY = "#1f6feb"      # Key Action Blue
    SUCCESS = NEON_GREEN
    WARNING = NEON_ORANGE
    DANGER = NEON_RED
    INFO = NEON_PURPLE
    
    # Text
    TEXT_PRIMARY = "#c9d1d9"     # High readability Off-White
    TEXT_SECONDARY = "#8b949e"   # Muted Grey
    TEXT_ON_NEON = "#ffffff"     # White text on colored buttons
    TEXT_INPUT = "#ffffff"       # White text in inputs
    
    # Gradients/Specials
    G_BLUE = ("#3b8ed0", "#1f6aa5")
    G_GREEN = ("#2cc985", "#1fa56e")
    
class Dimens:
    # Padding
    PAD_XS = 5
    PAD_S = 10
    PAD_M = 20
    PAD_L = 30
    
    # Radius
    R_SMALL = 6
    R_CARD = 12
    R_BUTTON = 8
    
    # Fonts
    def heading_xl(self): return ("Arial", 32, "bold")
    def heading_l(self): return ("Arial", 24, "bold")
    def heading_m(self): return ("Arial", 18, "bold")
    def body_l(self): return ("Arial", 14)
    def body_m(self): return ("Arial", 12)
    def body_s(self): return ("Arial", 10)
    def heading_s(self): return ("Arial", 14, "bold")

# Icons (Unicode placeholders or path references if we added assets)
class Icons:
    DASHBOARD = "📊"
    INVENTORY = "📦"
    POS = "🛒"
    CUSTOMERS = "👥"
    REPORTS = "📈"
    SETTINGS = "⚙️"
    TREND_UP = "↗"
    TREND_DOWN = "↘"
    ALERT = "⚠️"

# Helper to apply standard card styling
def style_card(frame: ctk.CTkFrame):
    frame.configure(fg_color=Colors.BG_CARD, corner_radius=Dimens.R_CARD, border_width=1, border_color="#333")

def apply_hover(widget, default_color, hover_color):
    widget.bind("<Enter>", lambda e: widget.configure(fg_color=hover_color))
    widget.bind("<Leave>", lambda e: widget.configure(fg_color=default_color))
