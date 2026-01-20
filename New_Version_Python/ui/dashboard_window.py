
import customtkinter as ctk
from tkinter import messagebox
from datetime import datetime
from ui.styles import Colors, Dimens, Icons, style_card, apply_hover
from ui.inventory_view import InventoryView
from ui.pos_view import POSView
from ui.reports_view import ReportsView
from ui.settings_view import SettingsView
from ui.user_management_view import UserManagementView
from ui.suppliers_view import SuppliersView
from ui.transactions_view import TransactionHistoryView
from ui.notifications_view import NotificationsView
from ui.stock_in_view import StockInView
from ui.stock_out_view import StockOutView
from ui.components.analytics_graph import AnalyticsGraph
from ui.components.insights_widget import InsightsWidget
from ui.help_view import HelpView
from utils.universal_search import UniversalSearchBar
from database import DatabaseRepository
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg

# Neon/Dark Theme Chart Style
plt.style.use('dark_background')
plt.rcParams['axes.facecolor'] = Colors.BG_CARD
plt.rcParams['figure.facecolor'] = Colors.BG_CARD
plt.rcParams['text.color'] = Colors.TEXT_PRIMARY
plt.rcParams['xtick.color'] = Colors.TEXT_SECONDARY
plt.rcParams['ytick.color'] = Colors.TEXT_SECONDARY
plt.rcParams['axes.edgecolor'] = Colors.BG_HOVER
plt.rcParams['axes.labelcolor'] = Colors.TEXT_SECONDARY

class DashboardWindow(ctk.CTkFrame):
    def __init__(self, master, user_info, db: DatabaseRepository, logout_callback):
        super().__init__(master, fg_color=Colors.BG_DARK)
        self.user = user_info
        self.db = db
        self.logout_callback = logout_callback

        # Layout: Sidebar (0) | Content (1)
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(0, weight=1)

        # --- Sidebar ---
        self.sidebar_frame = ctk.CTkFrame(self, width=240, corner_radius=0, fg_color=Colors.BG_DARK)
        self.sidebar_frame.grid(row=0, column=0, sticky="nsew")
        self.sidebar_frame.grid_rowconfigure(9, weight=1) 

        # Branding
        branding = ctk.CTkFrame(self.sidebar_frame, fg_color="transparent")
        branding.grid(row=0, column=0, padx=20, pady=(40, 40), sticky="w")
        ctk.CTkLabel(branding, text="Inventory", font=("Arial", 22, "bold"), text_color=Colors.TEXT_PRIMARY).pack(anchor="w")
        ctk.CTkLabel(branding, text="Flow", font=("Arial", 22, "bold"), text_color=Colors.NEON_BLUE).pack(anchor="w")

        # Nav Buttons
        self.nav_btns = {}
        self.create_nav_button("Dashboard", Icons.DASHBOARD, self.show_dashboard_view, 1)
        self.create_nav_button("Inventory", Icons.INVENTORY, self.show_inventory_view, 2)
        self.create_nav_button("Stock-In", "📥", self.show_stock_in_view, 3) 
        self.create_nav_button("Stock-Out", "📤", self.show_stock_out_view, 4)
        # self.create_nav_button("POS Register", Icons.POS, self.show_pos_view, 5) # Removed per user request
        self.create_nav_button("Reports", Icons.REPORTS, self.show_reports_view, 6)
        
        if self.user.role.lower() == 'admin':
             self.create_nav_button("Suppliers", "🚚", self.show_suppliers_view, 7)
             self.create_nav_button("Users", Icons.CUSTOMERS, self.show_users_view, 8)
             self.create_nav_button("Settings", Icons.SETTINGS, self.show_settings_view, 9)

        # Logout
        self.create_nav_button("Logout", "🚪", self.logout_callback, 10, is_logout=True)
        


        # --- Content Area ---
        self.content_frame = ctk.CTkFrame(self, fg_color=Colors.BG_DARK)
        self.content_frame.grid(row=0, column=1, sticky="nsew", padx=20, pady=20)
        
        # Header (search, user, notif) - Slightly simplified for Neon look
        self.header_frame = ctk.CTkFrame(self.content_frame, height=60, fg_color="transparent")
        self.header_frame.pack(fill="x", pady=(0, 20))
        
        self.lbl_header = ctk.CTkLabel(self.header_frame, text="Dashboard", font=("Arial", 24, "bold"), text_color=Colors.TEXT_PRIMARY)
        self.lbl_header.pack(side="left")

        # User Profile
        user_pill = ctk.CTkFrame(self.header_frame, fg_color=Colors.BG_CARD, corner_radius=20)
        user_pill.pack(side="right")
        ctk.CTkLabel(user_pill, text=f"{self.user.username} ({self.user.role})", font=("Arial", 12), text_color=Colors.TEXT_SECONDARY).pack(padx=15, pady=8)

        # Notification Bell
        self.btn_notif = ctk.CTkButton(self.header_frame, text="🔔", width=40, height=35, 
                                       fg_color=Colors.BG_CARD, hover_color=Colors.BG_HOVER,
                                       text_color=Colors.TEXT_PRIMARY,
                                       command=self.show_notifications)
        self.btn_notif.pack(side="right", padx=10)

        # Main Content Placeholder
        self.main_frame = ctk.CTkFrame(self.content_frame, fg_color="transparent")
        self.main_frame.pack(fill="both", expand=True)

        # Notifications Overlay container
        self.notification_overlay = None
        
        # Init Dashboard
        self.show_dashboard_view()
        
        # Startup Checks (delayed slightly to allow UI to render)
        # self.alert_after_id = self.after(1000, self.check_startup_alerts) # Removed per user request

    def destroy(self):
        if hasattr(self, 'alert_after_id') and self.alert_after_id:
            try:
                self.after_cancel(self.alert_after_id)
            except Exception:
                pass
        super().destroy()


    def create_nav_button(self, text, icon, command, row, is_logout=False):
        btn = ctk.CTkButton(self.sidebar_frame, text=f"  {icon}  {text}", anchor="w",
                            fg_color="transparent", text_color=Colors.TEXT_SECONDARY,
                            hover_color=Colors.BG_HOVER, height=40, font=("Arial", 14),
                            command=command)
        btn.grid(row=row, column=0, sticky="ew", padx=10, pady=2)
        if not is_logout:
            self.nav_btns[text] = btn
            
    def update_header(self, title):
        self.lbl_header.configure(text=title)
        # Reset nav buttons
        for name, btn in self.nav_btns.items():
            btn.configure(fg_color="transparent", text_color=Colors.TEXT_SECONDARY)

    def clear_main_frame(self):
        for widget in self.main_frame.winfo_children():
            widget.destroy()

    def update_notification_badge(self):
        try:
            count = len(self.db.get_unread_notifications())
        except:
            count = 0
        
        txt = "🔔"
        if count > 0:
            self.btn_notif.configure(text_color=Colors.NEON_ORANGE)
        else:
            self.btn_notif.configure(text_color=Colors.TEXT_PRIMARY)



    def show_notifications(self):
        # If already showing, hide it
        if self.notification_overlay:
            self.notification_overlay.destroy()
            self.notification_overlay = None
            self.update_notification_badge()
            return
            
        # Create Overlay Frame (Absolute Positioning or grid covering main content)
        # We'll use absolute placement to float over content
        self.notification_overlay = ctk.CTkFrame(self.content_frame, fg_color=Colors.BG_CARD, 
                                                 corner_radius=10, border_width=1, border_color=Colors.BORDER,
                                                 width=400, height=500)
        self.notification_overlay.place(relx=0.98, rely=0.08, anchor="ne")
        
        # Close Button
        header = ctk.CTkFrame(self.notification_overlay, fg_color="transparent")
        header.pack(fill="x", padx=10, pady=5)
        
        ctk.CTkLabel(header, text="Notifications", font=Dimens.heading_s(None)).pack(side="left")
        ctk.CTkButton(header, text="✕", width=30, height=30, fg_color="transparent", 
                      text_color=Colors.TEXT_SECONDARY, hover_color=Colors.BG_HOVER,
                      command=self.show_notifications).pack(side="right") # Toggles off
        
        # Determine current user? NotificationView needs user.
        # We can just embed the NotificationsView content, or refactor NotificationView to be this list.
        # Actually NotificationView is now a Frame, so let's just pack IT inside this overlay.
        
        # But NotificationsView has its own header. Let's make NotificationsView *be* the overlay content.
        # Let's instantiate NotificationsView inside our overlay container or just use it as the overlay.
        
        # Simpler: Re-instantiate NotificationsView as a child of the overlay container
        # Note: NotificationView expects a 'parent'.
        
        # Let's use NotificationsView directly as the overlay frame content.
        # But we need to handle "Close". 
        # NotificationView is designed as a full page sort of. Let's adjust it slightly or just pack it.
        
        view = NotificationsView(self.notification_overlay, self.db, self.user)
        view.pack(fill="both", expand=True, padx=5, pady=5)
        
        # Override Mark All Read to also update badge here
        original_mark_all = view.mark_all_read
        def hooked_mark_all():
            original_mark_all()
            self.update_notification_badge()
        view.mark_all_read = hooked_mark_all




    def show_dashboard_view(self):
        self.clear_main_frame()
        self.update_header("Dashboard")
        self.update_notification_badge()
        
        # Grid Layout: 3 Rows
        # Row 0: KPIs (Height ~160)
        # Row 1: Donut + Feed (Expand)
        # Row 2: Alerts + Actions (Height ~150)
        self.main_frame.grid_columnconfigure(0, weight=4) # Left col (Charts/Alerts)
        self.main_frame.grid_columnconfigure(1, weight=3) # Right col (Feed/Actions)
        self.main_frame.grid_rowconfigure(1, weight=1)

        # --- Row 0: KPI Cards ---
        kpi_container = ctk.CTkFrame(self.main_frame, fg_color="transparent")
        kpi_container.grid(row=0, column=0, columnspan=2, sticky="ew", pady=(0, Dimens.PAD_M))
        kpi_container.grid_columnconfigure((0,1,2,3), weight=1, uniform="kpi")

        try:
            total_items = self.db.get_total_product_count()
            total_value = self.db.get_total_inventory_value()
            out_stock = self.db.get_out_of_stock_count()
            low_stock = self.db.get_low_stock_count()

            items_trend = self.db.get_sparkline_data("items", 10)
            rev_trend = self.db.get_sparkline_data("revenue", 10)
            activity_trend = self.db.get_sparkline_data("activity", 10)

            self.create_neon_card(kpi_container, "TOTAL ITEMS", f"{total_items:,}", "+1.5% this month", Colors.NEON_BLUE, 0, items_trend, command=lambda: self.show_inventory_filtered("ALL"))
            self.create_neon_card(kpi_container, "TOTAL VALUE", f"${total_value:,.0f}", "▲ 280", Colors.NEON_GREEN, 1, rev_trend, command=lambda: self.show_inventory_filtered("ALL"))
            self.create_neon_card(kpi_container, "OUT OF STOCK", str(out_stock), "Needs Attention" if out_stock > 0 else "All Good", Colors.NEON_RED, 2, activity_trend, command=lambda: self.show_inventory_filtered("OUT"))
            self.create_neon_card(kpi_container, "LOW STOCK", str(low_stock), "Reorder Soon", Colors.NEON_ORANGE, 3, None, command=lambda: self.show_inventory_filtered("LOW"))


            # --- Row 1: Charts & Feed ---
            # Interactive Analytics Graph (Left) - REPLACED DONUT
            # We treat AnalyticsGraph as a Frame.
            chart_container = AnalyticsGraph(self.main_frame, self.db)
            chart_container.grid(row=1, column=0, sticky="nsew", padx=(0, Dimens.PAD_M), pady=(0, Dimens.PAD_M))
            
            # Make Chart Container Clickable -> Reports
            # Note: AnalyticsGraph might have internal widgets that capture clicks.
            # We should try to bind the container itself.
            chart_container.bind("<Button-1>", lambda e: self.show_reports_view())
            chart_container.configure(cursor="hand2")
            # Attempt to bind children (canvas might block)
            for child in chart_container.winfo_children():
                try: child.bind("<Button-1>", lambda e: self.show_reports_view())
                except: pass

            # Activity Feed (Right)
            feed_card = ctk.CTkFrame(self.main_frame, fg_color=Colors.BG_CARD, corner_radius=Dimens.R_CARD, border_width=1, border_color=Colors.BG_HOVER)
            feed_card.grid(row=1, column=1, sticky="nsew", pady=(0, Dimens.PAD_M))
            
            ctk.CTkLabel(feed_card, text="RECENT ACTIVITY", font=("Arial", 12, "bold"), text_color=Colors.TEXT_SECONDARY).pack(anchor="w", padx=20, pady=15)
            self.create_recent_activity_list(feed_card)

            # --- Row 2: Insights & Actions ---
            # Reverted to 2 columns as requested
            
            # Left: Insights Widget (Tabs)
            insights = InsightsWidget(self.main_frame, self.db)
            insights.grid(row=2, column=0, sticky="nsew", padx=(0, Dimens.PAD_M), pady=(0, 0))
            
            # Right: Actions (Now stacked or just button area)
            actions_card = ctk.CTkFrame(self.main_frame, fg_color="transparent")
            actions_card.grid(row=2, column=1, sticky="nsew", pady=(0, 0))
            
            self.create_action_buttons(actions_card)
        except Exception as e:
            print(f"Error loading dashboard: {e}")
            import traceback
            traceback.print_exc()

    def create_neon_card(self, parent, title, value, subtext, color, col_idx, data_points=None, command=None):
        card = ctk.CTkFrame(parent, height=140, fg_color=Colors.BG_CARD, corner_radius=Dimens.R_CARD, border_width=1, border_color=color)
        card.grid(row=0, column=col_idx, sticky="ew", padx=10)
        
        # Make clickable
        if command:
            card.bind("<Button-1>", lambda e: command())
            card.configure(cursor="hand2")
        
        # Inner padding
        inner = ctk.CTkFrame(card, fg_color="transparent")
        inner.pack(fill="both", expand=True, padx=20, pady=20)
        
        # Pass clicks through inner frame elements
        if command:
            inner.bind("<Button-1>", lambda e: command())
        
        lbl_t = ctk.CTkLabel(inner, text=title, font=("Arial", 11, "bold"), text_color=Colors.TEXT_SECONDARY)
        lbl_t.pack(anchor="w")
        
        lbl_v = ctk.CTkLabel(inner, text=value, font=("Arial", 32, "bold"), text_color=color)
        lbl_v.pack(anchor="w", pady=(5, 0))
        
        if command:
            lbl_t.bind("<Button-1>", lambda e: command())
            lbl_v.bind("<Button-1>", lambda e: command())
        
        # Subtext / Graph placeholder
        sub_frame = ctk.CTkFrame(inner, fg_color="transparent")
        sub_frame.pack(fill="x", pady=(10, 0))
        
        icon = "⚠" if color == Colors.NEON_RED else "📈"
        ctk.CTkLabel(sub_frame, text=f"{icon} {subtext}", font=("Arial", 11), text_color=color if color == Colors.NEON_RED else Colors.TEXT_SECONDARY).pack(side="left")

        # Sparkline (Canvas)
        if data_points and any(data_points):
            canvas = ctk.CTkCanvas(sub_frame, width=80, height=20, bg=Colors.BG_CARD, highlightthickness=0)
            canvas.pack(side="right")
            
            # Normalize data to fit 80x20
            w, h = 80, 20
            max_val = max(data_points)
            min_val = min(data_points)
            
            if max_val == min_val:
                # Flat line
                points = [0, h/2, w, h/2]
            else:
                points = []
                step_x = w / (len(data_points) - 1)
                for i, val in enumerate(data_points):
                    x = i * step_x
                    # Invert Y (higher value = lower Y)
                    y = h - ((val - min_val) / (max_val - min_val) * h)
                    points.extend([x, y])
            
            try:
                canvas.create_line(points, fill=color, width=2, smooth=True)
            except: pass


    def show_transactions_view(self):
        self.clear_main_frame()
        self.update_header("Transaction History")
        view = TransactionHistoryView(self.main_frame, self.db)
        view.pack(fill="both", expand=True)

    def create_recent_activity_list(self, parent):
        scroll = ctk.CTkScrollableFrame(parent, fg_color="transparent")
        scroll.pack(fill="both", expand=True, padx=5, pady=5)
        
        # Fetch updated fields
        txns = self.db.get_filtered_transactions()
        
        if not txns:
             ctk.CTkLabel(scroll, text="No recent activity.", text_color=Colors.TEXT_SECONDARY).pack(pady=20)
             return

        for t in txns[:8]: # Show last 8
            # Make row clickable
            row = ctk.CTkFrame(scroll, fg_color=Colors.BG_CARD, corner_radius=6, cursor="hand2")
            row.pack(fill="x", pady=3, padx=2)
            
            # Inner padding
            inner = ctk.CTkFrame(row, fg_color="transparent")
            inner.pack(fill="both", expand=True, padx=8, pady=5)
            
            # Bind Click
            for w in [row, inner]:
                w.bind("<Button-1>", lambda e: self.show_transactions_view())

            # Left Info Column
            info_frame = ctk.CTkFrame(inner, fg_color="transparent")
            info_frame.pack(side="left", fill="x", expand=True)
            info_frame.bind("<Button-1>", lambda e: self.show_transactions_view())

            p_name = t.product_description
            if len(p_name) > 22: p_name = p_name[:22] + "..."
            
            lbl_name = ctk.CTkLabel(info_frame, text=p_name, font=("Arial", 12, "bold"), text_color=Colors.TEXT_PRIMARY, anchor="w")
            lbl_name.pack(fill="x")
            lbl_name.bind("<Button-1>", lambda e: self.show_transactions_view())
            
            # Subtext: Type • Quantity
            # REQUEST: Supply = Green +, Delivery = Red -
            
            is_supply = t.transaction_type == "Supply"
            qty_sign = "+" if is_supply else "-"
            # Colors: Supply = Green (Stock In), Delivery = Red (Stock Out)
            qty_color = Colors.SUCCESS if is_supply else Colors.DANGER
            
            sub_txt = f"{t.transaction_type} • {qty_sign}{abs(t.quantity_change)} items"
            
            lbl_sub = ctk.CTkLabel(info_frame, text=sub_txt, font=("Arial", 10), text_color=Colors.TEXT_SECONDARY, anchor="w")
            lbl_sub.pack(fill="x")
            lbl_sub.bind("<Button-1>", lambda e: self.show_transactions_view())

            # Right: Value & Time
            meta_frame = ctk.CTkFrame(inner, fg_color="transparent")
            meta_frame.pack(side="right")
            meta_frame.bind("<Button-1>", lambda e: self.show_transactions_view())
            
            # Value formatting
            # User wants: Supply = Green +, Delivery = Red -
            # NOTE: Value logic usually implies: Supply = Cost (-), Delivery = Sales (+)
            # But user said "all about Inventory Total amounts" so they want COLOR to match STOCK movement?
            # "Supply is supposed to be the Green + sign... Delivery is the Red - sign"
            # It's weird to show Price as + for Supply (Cost), but I will follow the visual request.
            # OR they just mean the Quantity Sign?
            # "Supply is supposed to be the Green + sign"
            # In my previous code I had Value colored.
            # Let's Color the VALUE text based on Stock Movement?
            # Supply: +P... (Green)
            # Delivery: -P... (Red)
            # This is strictly visual for "Inventory Value Impact"? No, Supply INCREASES Inventory Value.
            
            val = abs(t.transaction_value)
            val_txt = f"₱{val:,.0f}"
            
            if is_supply:
                 val_txt = "+" + val_txt
                 val_col = Colors.SUCCESS
            else:
                 val_txt = "-" + val_txt
                 val_col = Colors.DANGER
            
            lbl_val = ctk.CTkLabel(meta_frame, text=val_txt, font=("Arial", 12, "bold"), text_color=val_col, anchor="e")
            lbl_val.pack(fill="x")
            lbl_val.bind("<Button-1>", lambda e: self.show_transactions_view())
            
            # Time formatting
            dt = t.transaction_date
            if isinstance(dt, str): 
                time_str = dt[:10]
            else:
                time_str = dt.strftime("%I:%M %p")
                
            lbl_time = ctk.CTkLabel(meta_frame, text=time_str, font=("Arial", 10), text_color=Colors.TEXT_SECONDARY, anchor="e")
            lbl_time.pack(fill="x")
            lbl_time.bind("<Button-1>", lambda e: self.show_transactions_view())

    def show_reports_view(self):
        self.clear_main_frame()
        self.update_header("Reports")
        view = ReportsView(self.main_frame, self.db)
        view.pack(fill="both", expand=True)



    def create_action_buttons(self, parent):
        btn_in = ctk.CTkButton(parent, text="STOCK IN", fg_color=Colors.NEON_BLUE, 
                               font=("Arial", 14, "bold"), height=50, hover_color="#0088cc",
                               text_color="black",
                               command=self.show_stock_in_view)
        btn_in.pack(fill="x", side="left", expand=True, padx=(10, 5))
        
        btn_out = ctk.CTkButton(parent, text="STOCK OUT", fg_color=Colors.NEON_RED, 
                                font=("Arial", 14, "bold"), height=50, hover_color="#cc0000",
                                text_color="black",
                                command=self.show_stock_out_view)
        btn_out.pack(fill="x", side="left", expand=True, padx=(5, 10))

    # --- Navigation Methods (Routing) ---
    def show_inventory_view(self):
        self.clear_main_frame()
        self.update_header("Inventory")
        view = InventoryView(self.main_frame, self.db, self.user)
        view.pack(fill="both", expand=True)

    def show_inventory_filtered(self, filter_type):
        self.clear_main_frame()
        self.update_header("Inventory" if filter_type == "ALL" else f"Inventory ({filter_type})")
        view = InventoryView(self.main_frame, self.db, self.user)
        view.set_filter(filter_type)
        view.pack(fill="both", expand=True)



    def show_stock_in_view(self):
        self.clear_main_frame()
        self.update_header("Stock In / Supply")
        view = StockInView(self.main_frame, self.db, self.user)
        view.pack(fill="both", expand=True)

    def show_stock_out_view(self):
        self.clear_main_frame()
        self.update_header("Stock Out")
        view = StockOutView(self.main_frame, self.db, self.user)
        view.pack(fill="both", expand=True)

    def show_pos_view(self):
        self.clear_main_frame()
        self.update_header("Stock Out / POS")
        view = POSView(self.main_frame, self.db, self.user)
        view.pack(fill="both", expand=True)

    def show_reports_view(self):
        self.clear_main_frame()
        self.update_header("Reports")
        view = ReportsView(self.main_frame, self.db)
        view.pack(fill="both", expand=True)

    def show_settings_view(self):
        self.clear_main_frame()
        self.update_header("Settings")
        view = SettingsView(self.main_frame, self.db)
        view.pack(fill="both", expand=True)

    def show_users_view(self):
        self.clear_main_frame()
        self.update_header("User Management")
        view = UserManagementView(self.main_frame, self.db)
        view.pack(fill="both", expand=True)

    def show_suppliers_view(self):
        self.clear_main_frame()
        self.update_header("Supplier Management")
        view = SuppliersView(self.main_frame, self.db, self.user)
        view.pack(fill="both", expand=True)
    
    def show_history_view(self):
        self.clear_main_frame()
        self.update_header("Transactions")
        view = TransactionHistoryView(self.main_frame, self.db)
        view.pack(fill="both", expand=True)
