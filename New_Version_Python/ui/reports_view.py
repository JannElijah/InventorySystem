import customtkinter as ctk
from tkinter import messagebox
from database import DatabaseRepository
from utils.pdf_generator import PDFReportGenerator
from datetime import datetime
from ui.components.analytics_graph import AnalyticsGraph
from ui.styles import Colors, Dimens

class ReportsView(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        
        # Grid Layout
        self.grid_columnconfigure(0, weight=1)
        self.grid_rowconfigure(1, weight=1)
        
        # Header
        self.header_frame = ctk.CTkFrame(self, fg_color="transparent")
        self.header_frame.grid(row=0, column=0, sticky="ew", padx=10, pady=10)
        
        ctk.CTkLabel(self.header_frame, text="Business Intelligence Reports", font=("Arial", 20, "bold"), text_color=Colors.TEXT_PRIMARY).pack(side="left")
        
        # Controls (Date Picker Simulation)
        self.ctrl_frame = ctk.CTkFrame(self.header_frame)
        self.ctrl_frame.pack(side="right")
        
        ctk.CTkLabel(self.ctrl_frame, text="Range (Days):").pack(side="left", padx=5)
        self.cmb_days = ctk.CTkComboBox(self.ctrl_frame, values=["7", "30", "90", "365", "Custom"], width=100, command=self.on_range_change,
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_days.set("30")
        self.cmb_days.pack(side="left", padx=5)
        
        # Custom Date Inputs (Hidden by default)
        self.custom_frame = ctk.CTkFrame(self.ctrl_frame, fg_color="transparent")
        self.entry_start = ctk.CTkEntry(self.custom_frame, width=90, placeholder_text="YYYY-MM-DD", 
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_start.pack(side="left", padx=2)
        ctk.CTkLabel(self.custom_frame, text="-").pack(side="left")
        self.entry_end = ctk.CTkEntry(self.custom_frame, width=90, placeholder_text="YYYY-MM-DD",
                                      fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_end.pack(side="left", padx=2)
        
        # Tabs
        self.tabview = ctk.CTkTabview(self, fg_color=Colors.BG_DARK, segmented_button_fg_color=Colors.BG_CARD, 
                                      segmented_button_selected_color=Colors.PRIMARY, segmented_button_unselected_color=Colors.BG_HOVER,
                                      text_color=Colors.TEXT_PRIMARY)
        self.tabview.grid(row=1, column=0, sticky="nsew", padx=10, pady=10)
        
        self.tab_inv = self.tabview.add("Inventory Value")
        self.tab_sales = self.tabview.add("Sales Summary")
        
        self.setup_inventory_tab()
        self.setup_sales_tab()
        
    def setup_inventory_tab(self):
        # Description
        ctk.CTkLabel(self.tab_inv, text="Current value of all stock on hand.", text_color=Colors.TEXT_PRIMARY).pack(pady=10)
        
        btn_export = ctk.CTkButton(self.tab_inv, text="Export Inventory Report (PDF)", 
                                   command=self.export_inventory_pdf, fg_color="#e67e22")
        btn_export.pack(pady=20)
        
        # Summary KPI
        val = self.db.get_total_inventory_value()
        ctk.CTkLabel(self.tab_inv, text=f"Total Asset Value: ${val:,.2f}", font=("Arial", 24, "bold"), text_color="#2ecc71").pack(pady=10)
        
        # Graph
        graph = AnalyticsGraph(self.tab_inv, self.db)
        graph.pack(fill="both", expand=True, padx=20, pady=10)
        
        # Pre-select Inventory Metric
        graph.var_metric.set("Inventory Value")
        graph.var_type.set("Bar")
        graph.refresh_chart()
        
    def setup_sales_tab(self):
        ctk.CTkLabel(self.tab_sales, text="Performance over selected period.", text_color=Colors.TEXT_PRIMARY).pack(pady=10)
        
        btn_export = ctk.CTkButton(self.tab_sales, text="Export Sales Report (PDF)", 
                                   command=self.export_sales_pdf, fg_color="#3498db")
        btn_export.pack(pady=(0, 20))

        # --- Summary Cards ---
        self.stats_frame = ctk.CTkFrame(self.tab_sales, fg_color="transparent")
        self.stats_frame.pack(fill="x", padx=20, pady=10)
        
        # We need to refresh these when range changes. For now load default (30d)
        self.refresh_sales_stats(30)
        
        # --- Graph ---
        self.sales_graph = AnalyticsGraph(self.tab_sales, self.db)
        self.sales_graph.pack(fill="both", expand=True, padx=20, pady=10)

    def refresh_sales_stats(self, days):
        # Clear existing
        for w in self.stats_frame.winfo_children(): w.destroy()
        
        stats = self.db.get_sales_summary_stats(days)
        
        # Helper to create card
        def create_card(parent, title, value, color):
            f = ctk.CTkFrame(parent, fg_color=Colors.BG_CARD, corner_radius=Dimens.R_CARD)
            f.pack(side="left", fill="both", expand=True, padx=5)
            ctk.CTkLabel(f, text=title, font=("Arial", 12), text_color=Colors.TEXT_SECONDARY).pack(pady=(10,5))
            ctk.CTkLabel(f, text=value, font=("Arial", 20, "bold"), text_color=color).pack(pady=(0,10))
            
        create_card(self.stats_frame, "Total Revenue", f"${stats['revenue']:,.2f}", Colors.SUCCESS)
        create_card(self.stats_frame, "Orders", str(stats['orders']), Colors.PRIMARY)
        create_card(self.stats_frame, "Items Sold", str(stats['items_sold']), Colors.WARNING)

        
    def export_inventory_pdf(self):
        # 1. Show Loading State
        btn = self.tab_inv.winfo_children()[1] # Button is 2nd child in setup_inventory_tab logic
        # Ideally keep reference to button, but strictly speaking we can find it or just show global busy
        self.configure(cursor="watch")
        
        # 2. Define Background Task
        def task():
            # DB Access in thread is safe-ish with separate cursor/conn used by Repository, 
            # BUT Repository uses 'db_path' to open NEW connection per method call. 
            # So this is THREAD SAFE because new sqlite3.connect is called.
            products = self.db.get_all_products()
            headers = ["ID", "Brand", "Description", "Stock", "Cost", "Value"]
            data = []
            total_val = 0
            for p in products:
                val = p.stock_quantity * p.purchase_cost
                total_val += val
                data.append([
                    str(p.product_id),
                    p.brand,
                    p.description,
                    str(p.stock_quantity),
                    f"${p.purchase_cost:.2f}",
                    f"${val:.2f}"
                ])
            data.append(["", "", "TOTAL", "", "", f"${total_val:.2f}"])
            return PDFReportGenerator.generate_report("Inventory Asset Report", headers, data, "inventory_report")

        # 3. Define Completion Handler (Main Thread Bridge)
        def on_complete(path):
            self.configure(cursor="")
            if path:
                PDFReportGenerator.open_pdf(path)
                messagebox.showinfo("Success", "Report generated successfully!")
            else:
                messagebox.showerror("Error", "Failed to generate report.")

        def on_err(e):
            self.configure(cursor="")
            messagebox.showerror("Error", f"Export failed: {str(e)}")

        # 4. Run Async
        from utils.async_task import AsyncTask
        # Wrapping on_complete in lambda calling after to ensure main thread execution
        AsyncTask.run(task, 
                      on_success=lambda p: self.after(0, lambda: on_complete(p)), 
                      on_error=lambda e: self.after(0, lambda: on_err(e)))

    def on_range_change(self, value):
        if value == "Custom":
            self.custom_frame.pack(side="left", padx=5)
        else:
            self.custom_frame.pack_forget()

    def export_sales_pdf(self):
        # 1. Gather UI Inputs (Must be on Main Thread)
        try:
            selection = self.cmb_days.get()
            days = 30
            title_suffix = ""

            if selection == "Custom":
                s_txt = self.entry_start.get().strip()
                e_txt = self.entry_end.get().strip()
                if not s_txt or not e_txt:
                    messagebox.showwarning("Missing Dates", "Please enter Start and End dates (YYYY-MM-DD)")
                    return
                try:
                    s_date = datetime.strptime(s_txt, "%Y-%m-%d")
                    e_date = datetime.strptime(e_txt, "%Y-%m-%d")
                    delta = (e_date - s_date).days
                    days = delta if delta > 0 else 1
                    title_suffix = f"({s_txt} to {e_txt})"
                except ValueError:
                    messagebox.showerror("Invalid Date", "Format must be YYYY-MM-DD")
                    return
            else:
                days = int(selection)
                title_suffix = f"({days} Days)"
            
            # Update Stats UI if not custom (simpler for now)
            if selection != "Custom":
                 self.refresh_sales_stats(days)
                 # Ideally we sync the graph too, but graph has its own controls.
                 # Syncing them would require exposing graph methods.
                 # For now, let's at least update the graph's time dropdown if it matches
                 if self.sales_graph:
                     time_map = {"7": "7 Days", "30": "30 Days", "90": "90 Days", "365": "Year"}
                     if selection in time_map:
                         self.sales_graph.var_time.set(time_map[selection])
                         self.sales_graph.refresh_chart()
        except Exception as e:
            messagebox.showerror("Error", f"Input Error: {e}")
            return

        # 2. Show Loading
        self.configure(cursor="watch")

        # 3. Define Task
        def task():
            # DB connection is safe here
            data_points = self.db.get_sales_by_brand(days)
            headers = ["Brand", "Revenue (Est)"]
            data = []
            total = 0
            for dp in data_points:
                data.append([dp.label, f"${dp.value:.2f}"])
                total += dp.value
            data.append(["TOTAL REVENUE", f"${total:.2f}"])
            
            return PDFReportGenerator.generate_report(f"Sales Summary {title_suffix}", headers, data, "sales_report")

        # 4. Completion
        def on_complete(path):
            self.configure(cursor="")
            if path:
                PDFReportGenerator.open_pdf(path)
                messagebox.showinfo("Success", "Report generated successfully!")
            else:
                messagebox.showerror("Error", "Failed.")

        def on_err(e):
            self.configure(cursor="")
            messagebox.showerror("Error", f"Export failed: {str(e)}")

        # 5. Run
        from utils.async_task import AsyncTask
        AsyncTask.run(task, 
                      on_success=lambda p: self.after(0, lambda: on_complete(p)), 
                      on_error=lambda e: self.after(0, lambda: on_err(e)))
