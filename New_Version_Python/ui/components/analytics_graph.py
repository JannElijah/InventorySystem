import customtkinter as ctk
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
from ui.styles import Colors, Dimens

class AnalyticsGraph(ctk.CTkFrame):
    def __init__(self, parent, db):
        super().__init__(parent, fg_color=Colors.BG_CARD, corner_radius=Dimens.R_CARD, 
                         border_width=1, border_color=Colors.BG_HOVER)
        self.db = db
        
        # Header / Toolbar
        self.toolbar = ctk.CTkFrame(self, fg_color="transparent", height=40)
        self.toolbar.pack(fill="x", padx=15, pady=10)
        
        ctk.CTkLabel(self.toolbar, text="ANALYTICS", font=("Arial", 12, "bold"), text_color=Colors.TEXT_SECONDARY).pack(side="left")
        
        # Controls
        self.var_metric = ctk.StringVar(value="Sales Trend")
        self.cmb_metric = ctk.CTkComboBox(self.toolbar, width=120, variable=self.var_metric, 
                                          values=["Sales Trend", "Top Products", "Brand Share", "Inventory Value"],
                                          command=self.on_change,
                                          fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_metric.pack(side="right", padx=5)
        
        self.var_type = ctk.StringVar(value="Line")
        self.cmb_type = ctk.CTkComboBox(self.toolbar, width=100, variable=self.var_type,
                                        values=["Line", "Bar", "Area", "Donut"],
                                        command=self.on_change,
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_type.pack(side="right", padx=5)
        
        self.var_time = ctk.StringVar(value="30 Days")
        self.cmb_time = ctk.CTkComboBox(self.toolbar, width=90, variable=self.var_time,
                                        values=["7 Days", "30 Days", "90 Days", "Year"],
                                        command=self.on_change,
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_time.pack(side="right", padx=5)

        # Chart Area
        self.chart_frame = ctk.CTkFrame(self, fg_color="transparent")
        self.chart_frame.pack(fill="both", expand=True, padx=10, pady=(0, 10))
        
        self.canvas = None
        self.fig: plt.Figure = None
        
        # Init
        self.refresh_chart()

    def on_change(self, choice):
        self.refresh_chart()

    def refresh_chart(self):
        # Clear previous
        if self.canvas:
            self.canvas.get_tk_widget().destroy()
        
        # Settings
        metric = self.var_metric.get()
        chart_type = self.var_type.get()
        time_str = self.var_time.get()
        
        days = 30
        if "7" in time_str: days = 7
        elif "90" in time_str: days = 90
        elif "Year" in time_str: days = 365
        
        # Fetch Data
        data = [] # List of ChartDataPoint (label, value)
        title = metric
        
        if metric == "Sales Trend":
            data = self.db.get_daily_sales_volume(days=days)
        elif metric == "Top Products":
            data = self.db.get_top_selling_products(by_revenue=True)
        elif metric == "Brand Share":
            data = self.db.get_sales_by_brand(days=days)
        elif metric == "Inventory Value":
            # Switch to Brand as Category is often empty
            data = self.db.get_inventory_value_by_brand()


        # Create Figure
        self.fig, ax = plt.subplots(figsize=(5, 3)) # Size adaptable
        self.fig.patch.set_facecolor(Colors.BG_CARD)
        ax.set_facecolor(Colors.BG_CARD)
        
        if not data:
            ax.text(0.5, 0.5, "No Data Available", ha='center', va='center', color=Colors.TEXT_SECONDARY)
            ax.set_axis_off()
        else:
            labels = [d.label for d in data]
            values = [d.value for d in data]
            
            color = Colors.NEON_BLUE
            
            if chart_type == "Line":
                ax.plot(labels, values, color=Colors.NEON_GREEN, marker='o', linewidth=2)
                ax.fill_between(labels, values, color=Colors.NEON_GREEN, alpha=0.1)
                plt.xticks(rotation=45)
            
            elif chart_type == "Bar":
                # If Top Products, maybe horizontal?
                if metric == "Top Products":
                    y_pos = range(len(labels))
                    ax.barh(y_pos, values, color=Colors.NEON_BLUE)
                    ax.set_yticks(y_pos)
                    ax.set_yticklabels(labels)
                    ax.invert_yaxis()
                else:
                    ax.bar(labels, values, color=Colors.NEON_BLUE)
                    plt.xticks(rotation=45)
                    
            elif chart_type == "Area":
                 ax.plot(labels, values, color=Colors.NEON_PURPLE)
                 ax.fill_between(labels, values, color=Colors.NEON_PURPLE, alpha=0.3)
                 plt.xticks(rotation=45)
                 
            elif chart_type == "Donut":
                colors = [Colors.NEON_BLUE, Colors.NEON_PURPLE, Colors.NEON_GREEN, Colors.NEON_ORANGE, Colors.NEON_RED]
                wedges, texts = ax.pie(values, labels=None, startangle=90, colors=colors, wedgeprops=dict(width=0.4))
                ax.legend(wedges, labels, title="Legend", loc="center left", bbox_to_anchor=(1, 0, 0.5, 1), frameon=False)

            # Styling
            ax.tick_params(axis='x', colors=Colors.TEXT_SECONDARY)
            ax.tick_params(axis='y', colors=Colors.TEXT_SECONDARY)
            for spine in ax.spines.values():
                spine.set_edgecolor(Colors.BG_HOVER)
                
            if len(labels) > 10 and chart_type != "Donut":
                 # Simplify ticks
                 ax.xaxis.set_major_locator(plt.MaxNLocator(6))

        self.fig.tight_layout()
        
        self.canvas = FigureCanvasTkAgg(self.fig, master=self.chart_frame)
        self.canvas.draw()
        self.canvas.get_tk_widget().pack(fill="both", expand=True)

    def destroy(self):
        plt.close(self.fig)
        super().destroy()
