import customtkinter as ctk
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
from ui.styles import Colors, Dimens

class InsightsWidget(ctk.CTkTabview):
    def __init__(self, parent, db):
        super().__init__(parent, height=300)
        self.db = db
        self.figures = []
        self.active_canvases = {} 

        # Configure Tabs
        self.add("Top Movers")
        self.add("Dead Stock")
        self.add("Profit")
        self.add("Peak Hours")
        self.add("Breakdown")
        
        # Set Tab Colors
        self.configure(fg_color=Colors.BG_CARD, segmented_button_fg_color=Colors.BG_DARK, 
                      segmented_button_selected_color=Colors.PRIMARY,
                      segmented_button_selected_hover_color="#0088cc",
                      segmented_button_unselected_color=Colors.BG_HOVER,
                      segmented_button_unselected_hover_color=Colors.BG_CARD)
        
        # Apply text color manually to the internal button
        self._segmented_button.configure(text_color=Colors.TEXT_PRIMARY)

        # --- LAZY LOADING STRATEGY ---
        # 1. Setup placeholders immediately so tabs exist
        self._setup_placeholders()
        
        # 2. Load the first tab (Top Movers) immediately for "pop"
        self.setup_top_movers()
        
        # 3. Schedule the rest to load progressively
        # This prevents the UI from freezing during startup!
        self.after(500, self.setup_profit_analysis)
        self.after(1000, self.setup_breakdown)
        self.after(1500, self.setup_dead_stock)
        self.after(2000, self.setup_peak_hours)

    def _setup_placeholders(self):
        """Create empty frames with 'Loading...' text for all tabs"""
        for tab_name in ["Dead Stock", "Profit", "Peak Hours", "Breakdown"]:
            frame = self.tab(tab_name)
            # Clear if re-initializing
            for w in frame.winfo_children(): w.destroy()
            
            ctk.CTkLabel(frame, text="Loading Analytics...", font=("Arial", 14), 
                         text_color=Colors.TEXT_SECONDARY).pack(expand=True)
            
            # Simple spinner animation placeholder (static for now, effectively concise)
            ctk.CTkProgressBar(frame, width=100, height=4, progress_color=Colors.PRIMARY).pack(pady=10)

    # --- 1. TOP MOVERS ---
    def setup_top_movers(self):
        """Best Sellers List"""
        frame = self.tab("Top Movers")
        
        # Clean loading state if present
        for w in frame.winfo_children(): w.destroy()
        
        # Filter
        header = ctk.CTkFrame(frame, fg_color="transparent")
        header.pack(fill="x", padx=5, pady=(5, 0))
        
        self.tm_filter_var = ctk.StringVar(value="30 Days")
        cbo = ctk.CTkComboBox(header, values=["7 Days", "30 Days", "90 Days", "1 Year"], 
                              width=100, height=24, font=("Arial", 11),
                              variable=self.tm_filter_var, command=self.refresh_top_movers,
                              fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        cbo.pack(side="right")
        
        ctk.CTkLabel(header, text="BEST SELLERS", font=("Arial", 11, "bold"), text_color=Colors.TEXT_SECONDARY).pack(side="left")

        # Container
        self.tm_container = ctk.CTkFrame(frame, fg_color="transparent")
        self.tm_container.pack(fill="both", expand=True, padx=5, pady=5)
        self.refresh_top_movers()

    def refresh_top_movers(self, _=None):
        for widget in self.tm_container.winfo_children(): widget.destroy()
        
        h = ctk.CTkFrame(self.tm_container, fg_color="transparent", height=20)
        h.pack(fill="x")
        ctk.CTkLabel(h, text="#", width=30, font=("Arial", 10, "bold"), text_color=Colors.TEXT_SECONDARY).pack(side="left")
        ctk.CTkLabel(h, text="Description", font=("Arial", 10, "bold"), text_color=Colors.TEXT_SECONDARY).pack(side="left", padx=10)
        ctk.CTkLabel(h, text="Sold", width=40, font=("Arial", 10, "bold"), text_color=Colors.TEXT_SECONDARY).pack(side="right")

        days = self._parse_days(self.tm_filter_var.get())
        try:
            items = self.db.get_top_selling_products(by_revenue=False, days=days)
            if not items:
                ctk.CTkLabel(self.tm_container, text="No sales data.", text_color=Colors.TEXT_SECONDARY).pack(pady=20)
                return

            for i, item in enumerate(items[:6]):
                row = ctk.CTkFrame(self.tm_container, fg_color="transparent", height=28)
                row.pack(fill="x", pady=1)
                
                rank_col = Colors.NEON_GREEN if i == 0 else Colors.TEXT_SECONDARY
                ctk.CTkLabel(row, text=f"{i+1}", width=30, font=("Arial", 11, "bold"), text_color=rank_col).pack(side="left")
                
                name = item.label[:28] + "..." if len(item.label) > 28 else item.label
                ctk.CTkLabel(row, text=name, font=("Arial", 11), text_color=Colors.TEXT_PRIMARY).pack(side="left", padx=10)
                ctk.CTkLabel(row, text=f"{int(item.value)}", width=40, font=("Arial", 11, "bold"), text_color=Colors.TEXT_PRIMARY).pack(side="right")

        except Exception as e: print(e)

    # --- 2. DEAD STOCK ---
    def setup_dead_stock(self):
        """Idle Inventory List"""
        frame = self.tab("Dead Stock")
        for w in frame.winfo_children(): w.destroy()
        
        # Header
        header = ctk.CTkFrame(frame, fg_color="transparent")
        header.pack(fill="x", padx=5, pady=(5, 0))
        
        self.ds_filter_var = ctk.StringVar(value="90 Days")
        cbo = ctk.CTkComboBox(header, values=["60 Days", "90 Days", "180 Days", "Year"], 
                              width=100, height=24, font=("Arial", 11),
                              variable=self.ds_filter_var, command=self.refresh_dead_stock,
                              fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        cbo.pack(side="right")
        
        ctk.CTkLabel(header, text="IDLE INVENTORY (No Sales)", font=("Arial", 11, "bold"), text_color=Colors.NEON_ORANGE).pack(side="left")

        self.ds_container = ctk.CTkScrollableFrame(frame, fg_color="transparent")
        self.ds_container.pack(fill="both", expand=True, padx=2, pady=5)
        self.refresh_dead_stock()

    def refresh_dead_stock(self, _=None):
        for w in self.ds_container.winfo_children(): w.destroy()
        
        days_str = self.ds_filter_var.get()
        days = 365 if "Year" in days_str else int(days_str.split()[0])
        
        try:
            items = self.db.get_dead_stock(days=days)
            if not items:
                ctk.CTkLabel(self.ds_container, text="No dead stock found (Good job!)", text_color=Colors.SUCCESS).pack(pady=20)
                return

            for item in items:
                row = ctk.CTkFrame(self.ds_container, fg_color=Colors.BG_HOVER, corner_radius=6)
                row.pack(fill="x", pady=2, padx=2)
                
                # Top Row: Name
                ctk.CTkLabel(row, text=item['name'], font=("Arial", 11, "bold"), text_color="white", anchor="w").pack(fill="x", padx=5, pady=(4,0))
                
                # Bottom Row: Details
                det = ctk.CTkFrame(row, fg_color="transparent")
                det.pack(fill="x", padx=5, pady=(0, 4))
                
                ctk.CTkLabel(det, text=f"Stock: {item['stock']}", font=("Arial", 10), text_color=Colors.TEXT_SECONDARY).pack(side="left")
                ctk.CTkLabel(det, text=f"Tied Value: ${item['value']:,.0f}", font=("Arial", 10, "bold"), text_color=Colors.NEON_RED).pack(side="right")
                
        except Exception as e: print(e)


    # --- 3. PROFIT ANALYSIS ---
    def setup_profit_analysis(self):
        """Line Chart"""
        frame = self.tab("Profit")
        for w in frame.winfo_children(): w.destroy()

        frame.columnconfigure(0, weight=3)
        frame.columnconfigure(1, weight=1)
        frame.rowconfigure(1, weight=1)

        # Header
        header = ctk.CTkFrame(frame, fg_color="transparent")
        header.grid(row=0, column=0, columnspan=2, sticky="ew", padx=5, pady=(5, 0))
        
        self.pa_filter_var = ctk.StringVar(value="30 Days")
        cbo = ctk.CTkComboBox(header, values=["7 Days", "30 Days", "90 Days", "1 Year"], 
                              width=100, height=24, font=("Arial", 11),
                              variable=self.pa_filter_var, command=self.refresh_profit_analysis,
                              fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        cbo.pack(side="right")
        ctk.CTkLabel(header, text="GROSS PROFIT", font=("Arial", 11, "bold"), text_color=Colors.TEXT_SECONDARY).pack(side="left")

        # Content
        self.pa_chart_frame = ctk.CTkFrame(frame, fg_color="transparent")
        self.pa_chart_frame.grid(row=1, column=0, sticky="nsew", padx=5, pady=5)
        
        self.pa_stats_frame = ctk.CTkFrame(frame, fg_color="transparent")
        self.pa_stats_frame.grid(row=1, column=1, sticky="nsew", padx=5, pady=5)
        
        self.refresh_profit_analysis()

    def refresh_profit_analysis(self, _=None):
        if "profit" in self.active_canvases:
            self.active_canvases["profit"].get_tk_widget().destroy()
            del self.active_canvases["profit"]
        for w in self.pa_stats_frame.winfo_children(): w.destroy()

        days = self._parse_days(self.pa_filter_var.get())
        try:
            data = self.db.get_daily_gross_profit(days=days)
            if not data:
                ctk.CTkLabel(self.pa_chart_frame, text="No Data", text_color=Colors.TEXT_SECONDARY).pack(pady=20)
                return

            # Stats
            total = sum(d.value for d in data)
            peak = max(data, key=lambda x: x.value)
            
            ctk.CTkLabel(self.pa_stats_frame, text="TOTAL PROFIT", font=("Arial", 10, "bold"), text_color=Colors.TEXT_SECONDARY).pack(anchor="w", pady=(10,0))
            ctk.CTkLabel(self.pa_stats_frame, text=f"${total:,.0f}", font=("Arial", 16, "bold"), text_color=Colors.NEON_GREEN).pack(anchor="w")
            
            ctk.CTkLabel(self.pa_stats_frame, text="BEST DAY", font=("Arial", 10, "bold"), text_color=Colors.TEXT_SECONDARY).pack(anchor="w", pady=(15,0))
            ctk.CTkLabel(self.pa_stats_frame, text=f"{peak.label[5:]}", font=("Arial", 12, "bold"), text_color="white").pack(anchor="w")

            # Chart
            fig, ax = plt.subplots(figsize=(4, 2.5))
            fig.patch.set_facecolor(Colors.BG_CARD)
            ax.set_facecolor(Colors.BG_CARD)
            self.figures.append(fig)

            labels = [d.label[5:] for d in data]
            values = [d.value for d in data]

            # Use Gold or Green for Profit
            ax.plot(labels, values, color=Colors.NEON_GREEN, marker='.', markersize=6, linewidth=2)
            ax.fill_between(labels, values, color=Colors.NEON_GREEN, alpha=0.1)
            
            # Styling
            self._style_chart(ax)
            if len(labels) > 6: ax.xaxis.set_major_locator(plt.MaxNLocator(6))
            
            fig.tight_layout()
            canvas = FigureCanvasTkAgg(fig, master=self.pa_chart_frame)
            canvas.draw()
            canvas.get_tk_widget().pack(fill="both", expand=True)
            self.active_canvases["profit"] = canvas

        except Exception as e: print(e)

    # --- 4. PEAK HOURS ---
    def setup_peak_hours(self):
        """Bar Chart by Hour"""
        frame = self.tab("Peak Hours")
        for w in frame.winfo_children(): w.destroy()
        
        try:
            data = self.db.get_peak_sales_hours()
            
            fig, ax = plt.subplots(figsize=(5, 3))
            fig.patch.set_facecolor(Colors.BG_CARD)
            ax.set_facecolor(Colors.BG_CARD)
            self.figures.append(fig)

            if not data:
                ax.text(0.5, 0.5, "No Data", ha='center', color=Colors.TEXT_SECONDARY)
            else:
                labels = [d.label.split(":")[0] for d in data] # Just Hour
                values = [d.value for d in data]
                
                ax.bar(labels, values, color=Colors.NEON_PURPLE, alpha=0.9)
                self._style_chart(ax)
                ax.set_xlabel("Hour of Day (24h)", color=Colors.TEXT_SECONDARY, fontsize=8)

            fig.tight_layout()
            canvas = FigureCanvasTkAgg(fig, master=frame)
            canvas.draw()
            canvas.get_tk_widget().pack(fill="both", expand=True, padx=5, pady=5)

        except Exception as e: 
            print(e)
            ctk.CTkLabel(frame, text="Error loading chart", text_color=Colors.DANGER).pack()

    # --- 5. BREAKDOWN ---
    def setup_breakdown(self):
        """Donut Chart with Toggle"""
        frame = self.tab("Breakdown")
        for w in frame.winfo_children(): w.destroy()

        frame.columnconfigure(0, weight=2)
        frame.columnconfigure(1, weight=3)
        frame.rowconfigure(1, weight=1)

        # Header with Toggle
        header = ctk.CTkFrame(frame, fg_color="transparent")
        header.grid(row=0, column=0, columnspan=2, sticky="ew", padx=5, pady=(5, 0))
        
        self.bd_metric_var = ctk.StringVar(value="Inventory Value")
        self.bd_metric_cbo = ctk.CTkComboBox(header, values=["Inventory Value", "Sales Revenue"],
                                             width=140, height=24, font=("Arial", 11),
                                             variable=self.bd_metric_var, command=self.refresh_breakdown,
                                             fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.bd_metric_cbo.pack(side="left")

        self.bd_time_var = ctk.StringVar(value="30 Days")
        self.bd_time_cbo = ctk.CTkComboBox(header, values=["30 Days", "90 Days", "1 Year"],
                                           width=100, height=24, font=("Arial", 11),
                                           variable=self.bd_time_var, command=self.refresh_breakdown,
                                           fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        # Initially hide time for Inventory
        self.bd_time_cbo.pack(side="right")
        self.bd_time_cbo.pack_forget() 

        # Containers
        self.bd_chart_frame = ctk.CTkFrame(frame, fg_color="transparent")
        self.bd_chart_frame.grid(row=1, column=0, sticky="nsew", padx=5, pady=5)
        
        self.bd_list_frame = ctk.CTkScrollableFrame(frame, fg_color="transparent")
        self.bd_list_frame.grid(row=1, column=1, sticky="nsew", padx=5, pady=5)
        
        self.refresh_breakdown()

    def refresh_breakdown(self, _=None):
        if "breakdown" in self.active_canvases:
            self.active_canvases["breakdown"].get_tk_widget().destroy()
            del self.active_canvases["breakdown"]
        for w in self.bd_list_frame.winfo_children(): w.destroy()

        metric = self.bd_metric_var.get()
        
        # Toggle Time Filter visibility
        if metric == "Sales Revenue":
            self.bd_time_cbo.pack(side="right")
        else:
            self.bd_time_cbo.pack_forget()

        try:
            if metric == "Inventory Value":
                data = self.db.get_inventory_value_by_brand()
            else:
                days = self._parse_days(self.bd_time_var.get())
                data = self.db.get_sales_by_brand(days=days)
            
            if not data:
                ctk.CTkLabel(self.bd_chart_frame, text="No Data", text_color=Colors.TEXT_SECONDARY).pack(pady=20)
                return

            # --- Chart ---
            fig, ax = plt.subplots(figsize=(2.5, 2.5))
            fig.patch.set_facecolor(Colors.BG_CARD)
            ax.set_facecolor(Colors.BG_CARD)
            self.figures.append(fig)

            display_data = data[:5]
            others = sum(d.value for d in data[5:])
            if others > 0: display_data.append(type('obj', (object,), {'label': 'Others', 'value': others}))
            
            labels = [d.label for d in display_data]
            values = [d.value for d in display_data]
            colors = [Colors.NEON_BLUE, Colors.NEON_PURPLE, Colors.NEON_GREEN, Colors.NEON_ORANGE, Colors.NEON_RED, Colors.TEXT_SECONDARY]

            ax.pie(values, startangle=90, colors=colors[:len(values)], 
                   wedgeprops=dict(width=0.5, edgecolor=Colors.BG_CARD))
            
            total = sum(d.value for d in data)
            ax.text(0, 0, f"${total:,.0f}", ha='center', va='center', fontsize=9, color='white', weight='bold')
            
            fig.tight_layout()
            canvas = FigureCanvasTkAgg(fig, master=self.bd_chart_frame)
            canvas.draw()
            canvas.get_tk_widget().pack(fill="both", expand=True)
            self.active_canvases["breakdown"] = canvas

        # --- List ---
            for i, d in enumerate(data[:8]): 
                row = ctk.CTkFrame(self.bd_list_frame, fg_color="transparent")
                row.pack(fill="x", pady=2)
                
                col = colors[i] if i < len(colors) else Colors.TEXT_SECONDARY
                ctk.CTkFrame(row, width=8, height=8, fg_color=col, corner_radius=4).pack(side="left", pady=5)
                ctk.CTkLabel(row, text=d.label[:12], font=("Arial", 11, "bold"), text_color="white").pack(side="left", padx=5)
                
                pct = (d.value / total) * 100
                ctk.CTkLabel(row, text=f"{pct:.0f}%", font=("Arial", 11), text_color=Colors.TEXT_SECONDARY).pack(side="right")

        except Exception as e: print(e)


    def _parse_days(self, text):
        if "7" in text: return 7
        if "30" in text: return 30
        if "60" in text: return 60
        if "90" in text: return 90
        if "1" in text: return 365
        return 30

    def _style_chart(self, ax):
        ax.tick_params(axis='x', colors=Colors.TEXT_SECONDARY, labelsize=7)
        ax.tick_params(axis='y', colors=Colors.TEXT_SECONDARY, labelsize=7)
        ax.spines['top'].set_visible(False)
        ax.spines['right'].set_visible(False)
        ax.spines['bottom'].set_color(Colors.BG_HOVER)
        ax.spines['left'].set_color(Colors.BG_HOVER)

    def destroy(self):
        for fig in self.figures: plt.close(fig)
        super().destroy()
