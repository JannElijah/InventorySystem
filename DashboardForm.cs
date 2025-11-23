using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static DatabaseRepository;

namespace InventorySystem
{
    /// <summary>
    /// Serves as the main hub for Admin users, displaying key metrics and providing navigation to all management modules.
    /// </summary>
    public partial class DashboardForm : Form
    {
        private StringBuilder _barcodeBuffer = new StringBuilder();
        private readonly User _loggedInUser;
        private readonly DatabaseRepository _repository;
        private static readonly Color dodgerBlue = Color.DodgerBlue;
        private readonly Color navButtonIdleColor = dodgerBlue;
        private static readonly Color steelBlue1 = Color.SteelBlue;
        private static readonly Color steelBlue = steelBlue1;
        private readonly Color navButtonHoverColor = steelBlue;
        private string connectionString;
        private List<ChartDataPoint> _topProductsData;
        private List<ChartDataPoint> _dailySalesData;
        private List<ChartDataPointDecimal> _salesBreakdownData;

        #region Constructor

        /// <summary>
        /// Initializes the dashboard and stores the currently logged-in user's information.
        /// </summary>
        /// <param name="user">The authenticated User object.</param>
        public DashboardForm(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            _repository = new DatabaseRepository();
            this.Text = $"Dashboard - Logged in as: {_loggedInUser.Username}";
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Public method to refresh all data displayed on the dashboard from the database.
        /// This includes KPIs, recent transactions, and the notification count.
        /// </summary>
        public void LoadDashboardData()
        {
            try
            {
                lblTotalProductsValue.Text = _repository.GetTotalProductCount().ToString();
                lblOutOfStockValue.Text = _repository.GetOutOfStockCount().ToString();
                lblLowStockValue.Text = _repository.GetLowStockCount().ToString();
                lblInventoryValueValue.Text = _repository.GetTotalInventoryValue().ToString("C");

                ucTransactionHistory1.RefreshTransactions();
                UpdateNotificationCount();
                UpdateTodaysActivitySummary();
                UpdatePerformanceChart();
                UpdateSalesTrendChart();
                UpdateSalesBreakdownChart();
                UpdateTodaysTransactionsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load dashboard data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // In DashboardForm.cs

        /// <summary>
        /// Programmatically configures the columns and behavior of the product history DataGridView.
        /// This method should be called once, during form load.
        /// </summary>
        // In DashboardForm.cs
        // REPLACE your existing SetupProductHistoryGrid method with this new version.

        /// <summary>
        /// Programmatically configures the columns and behavior of the product history DataGridView.
        /// This method should be called once, during form load.
        /// </summary>
        private void SetupProductHistoryGrid()
        {
            // --- BEHAVIOR ---
            dgvProductHistory.AutoGenerateColumns = false;

            // --- THIS IS THE KEY PROPERTY FOR FILLING THE GRID WIDTH ---
            dgvProductHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvProductHistory.ReadOnly = true;
            dgvProductHistory.AllowUserToAddRows = false;
            dgvProductHistory.AllowUserToDeleteRows = false;
            dgvProductHistory.RowHeadersVisible = false;
            dgvProductHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // --- COLUMNS ---
            dgvProductHistory.Columns.Clear();

            // Column 1: Transaction Date
            var dateColumn = new DataGridViewTextBoxColumn
            {
                Name = "colProdHistDate",
                HeaderText = "Date",
                DataPropertyName = "TransactionDate",
                // REMOVED: Width = 120
                FillWeight = 30 // ADDED: Give this column 30% of the relative space
            };
            dateColumn.DefaultCellStyle.Format = "g";

            // Column 2: Transaction Type
            var typeColumn = new DataGridViewTextBoxColumn
            {
                Name = "colProdHistType",
                HeaderText = "Type",
                DataPropertyName = "TransactionType",
                // REMOVED: Width = 80
                FillWeight = 20 // ADDED: Give this column 20% of the relative space
            };

            // Column 3: Quantity Change
            var qtyColumn = new DataGridViewTextBoxColumn
            {
                Name = "colProdHistQty",
                HeaderText = "Qty Change",
                DataPropertyName = "QuantityChange",
                // REMOVED: Width = 80
                FillWeight = 15 // ADDED: Give this column less space
            };

            // Column 4: Stock After
            var stockAfterColumn = new DataGridViewTextBoxColumn
            {
                Name = "colProdHistStockAfter",
                HeaderText = "New Stock",
                DataPropertyName = "StockAfter",
                // REMOVED: Width = 80
                FillWeight = 15 // ADDED: Give this column less space
            };

            // Column 5: Source/Customer (Unbound)
            var sourceColumn = new DataGridViewTextBoxColumn
            {
                Name = "colProdHistSource",
                HeaderText = "Source/Customer",
                FillWeight = 40 // ADDED: Give this column the most space for names
            };

            // Add the columns to the grid in the desired order
            dgvProductHistory.Columns.Add(dateColumn);
            dgvProductHistory.Columns.Add(typeColumn);
            dgvProductHistory.Columns.Add(qtyColumn);
            dgvProductHistory.Columns.Add(stockAfterColumn);
            dgvProductHistory.Columns.Add(sourceColumn);
        }

        /// <summary>
        /// Fetches the unread notification count and updates the visibility and text of the notification badge.
        /// </summary>
        private void UpdateNotificationCount()
        {
            int unreadCount = _repository.GetUnreadNotificationCount();
            if (unreadCount > 0)
            {
                lblNotificationCount.Text = unreadCount.ToString();
                lblNotificationCount.Visible = true;
            }
            else
            {
                lblNotificationCount.Visible = false;
            }
        }

        #endregion

        #region Form Event Handlers

        /// <summary>
        /// Loads initial data when the dashboard is first opened.
        /// </summary>
        private void DashboardForm_Load(object sender, EventArgs e)
        {
            SetupProductHistoryGrid();
            SetupChartDataGrid();
            SetupTodaysTransactionsGrid();
            cmbBreakdownType.Items.Add("Product Type");
            cmbBreakdownType.Items.Add("Product Brand");
            cmbBreakdownType.SelectedIndex = 0;
            cmbBreakdownType.SelectedIndexChanged += (s, ev) => UpdateSalesBreakdownChart();
            ucTransactionHistory1.ProductClicked += HandleProductClicked;
            pnlTodaysTransactions.Visible = false;
            pnlProductMetadata.Visible = false;
            pnlChartData.Visible = false;
            LoadDashboardData();
            SetActiveButton(btnViewInventory); 
            tabAnalytics_SelectedIndexChanged(null, null);
        }

        // In DashboardForm.cs, add this new method anywhere in the class

        /// <summary>
        /// Handles the ProductClicked event from the transaction history user control.
        /// </summary>
        /// <param name="productId">The ID of the product that was clicked.</param>
        private void HandleProductClicked(int productId)
        {
            // Step 1: Fetch the data from the database
            Product selectedProduct = _repository.GetProductById(productId);
            List<DashboardTransactionView> productHistory = _repository.GetRecentTransactionsForProduct(productId);

            if (selectedProduct == null)
            {
                MessageBox.Show("Could not find details for the selected product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 2: Populate the labels with the product's details
            lblSelectedProductName.Text = $"{selectedProduct.Brand} {selectedProduct.Description}";
            lblDetailPartNumber.Text = selectedProduct.PartNumber;
            lblDetailBrand.Text = selectedProduct.Brand;
            lblDetailType.Text = selectedProduct.Type;
            lblDetailPurchaseCost.Text = selectedProduct.PurchaseCost.ToString("C"); // "C" formats as currency
            lblDetailSellingPrice.Text = selectedProduct.SellingPrice.ToString("C");
            lblSelectedProductStock.Text = selectedProduct.StockQuantity.ToString();

            // Calculate and display the total value on hand for this product
            decimal valueOnHand = selectedProduct.StockQuantity * selectedProduct.PurchaseCost;
            lblSelectedProductValue.Text = valueOnHand.ToString("C");

            // Step 3: Bind the specific transaction history to the mini-grid
            dgvProductHistory.DataSource = productHistory;
            // You may need to configure columns for dgvProductHistory just like you did for the main grid.
            // For now, this will display the data.

            // Step 4: Automatically switch to the details tab to show the user the results
            tabAnalytics.SelectedTab = tabProductDetails;
        }

        /// <summary>
        /// Reloads data every time the dashboard becomes the active window.
        /// </summary>
        private void DashboardForm_Activated(object sender, EventArgs e)
        {
            // Only load if the form is actually visible and not minimized
            if (this.WindowState != FormWindowState.Minimized && this.Visible)
            {
                LoadDashboardData();
            }
        }

        /// <summary>
        /// Configures the columns for the dgvChartData grid.
        /// </summary>
        private void SetupChartDataGrid()
        {
            dgvChartData.AutoGenerateColumns = false;
            dgvChartData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChartData.Columns.Clear();

            dgvChartData.Columns.Add("Item", "Item"); // Column for the label (e.g., Product Name)
            dgvChartData.Columns.Add("Value", "Value"); // Column for the value (e.g., Quantity Sold)
        }

        #endregion

        #region Navigation Button Handlers

        private void BtnViewInventory_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnViewInventory); // This one was already correct
            Form1 inventoryForm = new Form1(_loggedInUser)
            {
                Owner = this
            };
            inventoryForm.Show();
            this.Hide();
        }

        private void BtnManageUsers_Click(object sender, EventArgs e)
        {
            // FIXED: Pass the correct button to the method.
            SetActiveButton(btnNavManageUsers);
            UserManagementForm userForm = new UserManagementForm(_loggedInUser)
            {
                Owner = this
            };
            userForm.Show();
        }

        private void BtnManageSuppliers_Click(object sender, EventArgs e)
        {
            // FIXED: Pass the correct button to the method.
            SetActiveButton(btnManageSuppliers);
            var manageSuppliersForm = new ManageSuppliersForm
            {
                Owner = this
            };
            manageSuppliersForm.Show();
        }

        // In your DashboardForm.cs, inside the "View Notifications" button click event
        private void BtnNotifications_Click(object sender, EventArgs e)
        {
            // Make sure you are calling the constructor that accepts a User object
            var notificationsForm = new NotificationsForm(_loggedInUser);
            notificationsForm.Owner = this;
            notificationsForm.Show();
        }

        private void BtnViewReports_Click(object sender, EventArgs e)
        {
            // FIXED: Pass the correct button to the method.
            SetActiveButton(btnViewReports);
            ReportsForm reportsForm = new ReportsForm(_repository)
            {
                Owner = this
            };
            reportsForm.Show();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // IMPROVEMENT: Removed the SetActiveButton call. Logging out is an action, not a state.
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();
        }

        public List<Notification> GetAllNotifications()
        {
            var notifications = new List<Notification>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // This query joins Notifications with Products to get the product name
                string sql = @"
            SELECT n.NotificationID, n.ProductID, p.Description AS ProductName, n.Message, n.IsRead, n.Timestamp, n.NotificationType
            FROM Notifications n
            JOIN Products p ON n.ProductID = p.ProductID
            ORDER BY n.Timestamp DESC";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                NotificationID = Convert.ToInt32(reader["NotificationID"]),
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                Message = reader["Message"].ToString(),
                                IsRead = Convert.ToBoolean(reader["IsRead"]),
                                Timestamp = Convert.ToDateTime(reader["Timestamp"]),
                                NotificationType = reader["NotificationType"].ToString()
                            });
                        }
                    }
                }
            }
            return notifications;
        }
        #endregion

        #region UI Event Handlers (Hover Effects)

        // These methods are correctly reused by all navigation buttons. No changes needed.
        private void BtnViewInventory_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = navButtonHoverColor;
        }

        private void BtnViewInventory_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Tag == null || btn.Tag.ToString() != "active")
            {
                btn.BackColor = navButtonIdleColor;
            }
        }

        #endregion

        #region Helper Methods

        private void SetActiveButton(Button activeButton)
        {
            // IMPROVEMENT: Make sure your navigation panel in the designer is named "pnlNavigation".
            foreach (Control ctrl in pnlNavigation.Controls)
            {
                if (ctrl is Button)
                {
                    Button btn = ctrl as Button;

                    // IMPROVEMENT: We don't want the logout button to ever be highlighted, so we exclude it.
                    if (btn != btnNavLogout)
                    {
                        btn.BackColor = navButtonIdleColor; // Reset all buttons to the default color
                        btn.Tag = "idle";
                    }
                }
            }

            // Now, set the style for the button that was just clicked
            if (activeButton != btnNavLogout)
            {
                activeButton.BackColor = navButtonHoverColor; // Highlight the active button
                activeButton.Tag = "active";
            }
        }

        #endregion

        private void KpiPanel_Click(object sender, EventArgs e)
        {
            if (!(sender is Panel clickedPanel)) return;

            string filterType = "";
            switch (clickedPanel.Name)
            {
                case "pnlKpiTotalProducts":
                    filterType = "TOTAL_PRODUCTS";
                    break;
                case "pnlKpiOutOfStock":
                    filterType = "OUT_OF_STOCK";
                    break;
                case "pnlKpiLowStock":
                    filterType = "LOW_STOCK";
                    break;
            }

            if (string.IsNullOrEmpty(filterType)) return;

            // --- MODIFIED LOGIC ---
            // We now pass the filterType directly into the constructor.
            Form1 inventoryForm = new Form1(_loggedInUser, 0, filterType)
            {
                Owner = this
            }; // <-- MODIFIED

            // We NO LONGER call ApplyDashboardFilter() here.
            // inventoryForm.ApplyDashboardFilter(filterType); // <-- DELETE THIS LINE

            inventoryForm.Show();
            this.Hide();
        }

        private void DgvRecentTransactions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Log the error for our debugging records.
            System.Diagnostics.Debug.WriteLine($"DataError on Dashboard grid was caught: {e.Exception.Message}");

            // The context of this error is that the user is trying to leave a cell with invalid data.
            // We will cancel the edit, which reverts the cell to its original value.
            if (e.Context == DataGridViewDataErrorContexts.Commit)
            {
                // Suppress the error dialog by handling it here.
                // You can optionally inform the user. For this bug, we'll just suppress it.
                // MessageBox.Show("Invalid data entered."); 
            }

            // Cancel the cell edit that caused the validation error.
            // This is a cleaner way to handle it than just hiding the exception.
            dgvRecentTransactions.CancelEdit();

            // Prevent the default error dialog from showing.
            e.ThrowException = false;
        }
        // In DashboardForm.cs

        /// <summary>
        /// Fetches and populates the sales breakdown pie chart based on the user's selection.
        /// </summary>
        private void UpdateSalesBreakdownChart()
        {
            if (chartSalesBreakdown == null || cmbBreakdownType.SelectedItem == null) return;

            var series = chartSalesBreakdown.Series.FirstOrDefault();
            if (series == null)
            {
                // If "Series1" was deleted or renamed, add a new one.
                chartSalesBreakdown.Series.Add("SalesData");
                series = chartSalesBreakdown.Series["SalesData"];
            }

            var chartArea = chartSalesBreakdown.ChartAreas.FirstOrDefault();

            // --- CHART-WIDE STYLING ---
            chartSalesBreakdown.BackColor = Color.FromArgb(45, 45, 48);
            chartSalesBreakdown.Legends[0].ForeColor = Color.White;
            chartSalesBreakdown.Legends[0].BackColor = Color.FromArgb(45, 45, 48);

            // --- TITLE ---
            string breakdownSelection = cmbBreakdownType.SelectedItem.ToString();
            chartSalesBreakdown.Titles.Clear();
            var title = chartSalesBreakdown.Titles.Add($"Sales Revenue by {breakdownSelection} (Last 90 Days)");
            title.ForeColor = Color.White;
            title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            // --- SERIES STYLING (Pie Chart specific) ---
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series["PieLabelStyle"] = "Outside"; // Show labels outside the pie slices
            series["PieLineColor"] = "White"; // Line connecting label to slice
            series.LabelForeColor = Color.White;
            series.Font = new System.Drawing.Font("Segoe UI", 9F);
            series.Label = "#VALX (#PERCENT)"; // Format: "Engine Oil (60.00%)"

            // --- DATA BINDING ---
            string category = cmbBreakdownType.SelectedItem.ToString() == "Product Type" ? "Type" : "Brand";
            _salesBreakdownData = _repository.GetSalesBreakdownBy(category);
            series.Points.Clear();

            foreach (var dataPoint in _salesBreakdownData)
            {
                series.Points.AddXY(dataPoint.Label, dataPoint.Value);
            }

            dgvChartData.Rows.Clear();
            if (_salesBreakdownData != null)
            {
                foreach (var dataPoint in _salesBreakdownData)
                {
                    dgvChartData.Rows.Add(dataPoint.Label, dataPoint.Value.ToString("C"));
                }
            }
        }
        // In DashboardForm.cs

        /// <summary>
        /// Fetches and displays the summary of today's supply and delivery activity.
        /// </summary>
        // In DashboardForm.cs, REPLACE the old method with this one.

        /// <summary>
        /// Fetches and displays the comprehensive summary of today's activity.
        /// </summary>
        private void UpdateTodaysActivitySummary()
        {
            try
            {
                DailySummary summary = _repository.GetTodaysActivitySummary();

                // Populate the original labels
                lblItemsSuppliedToday.Text = summary.ItemsSupplied.ToString();
                lblItemsDeliveredToday.Text = summary.ItemsDelivered.ToString();
                lblSupplyTransactionsToday.Text = summary.SupplyTransactionCount.ToString();
                lblDeliveryTransactionsToday.Text = summary.DeliveryTransactionCount.ToString();
                lblCostSuppliedToday.Text = summary.CostOfSupplies.ToString("C");
                lblRevenueToday.Text = summary.GrossRevenue.ToString("C");

                // --- ADD THESE TWO LINES TO POPULATE THE NEW LABELS ---

                int netChange = summary.ItemsSupplied - summary.ItemsDelivered;
                lblNetStockChange.Text = netChange.ToString();

                // --- APPLY ALL COLOR CODING ---
                lblItemsSuppliedToday.ForeColor = Color.Green;
                lblCostSuppliedToday.ForeColor = Color.Green;


                lblItemsDeliveredToday.ForeColor = Color.Red;
                lblRevenueToday.ForeColor = Color.DarkGreen;

                if (netChange > 0)
                {
                    lblNetStockChange.ForeColor = Color.Green;
                    lblNetStockChange.Text = "+" + netChange;
                }
                else if (netChange < 0)
                {
                    lblNetStockChange.ForeColor = Color.Red;
                }
                else
                {
                    lblNetStockChange.ForeColor = SystemColors.ControlText; // Or your default text color
                }
            }
            catch (Exception ex)
            {
                // Handle error gracefully if data can't be loaded
                lblItemsSuppliedToday.Text = "N/A";
                lblItemsDeliveredToday.Text = "N/A";
                lblSupplyTransactionsToday.Text = "N/A";
                lblDeliveryTransactionsToday.Text = "N/A";
                lblCostSuppliedToday.Text = "N/A";
                lblRevenueToday.Text = "N/A";
                lblNetStockChange.Text = "N/A";
                // ... set other labels to "N/A" as well
                System.Diagnostics.Debug.WriteLine($"Error updating today's activity: {ex.Message}");
            }
        }

        private void DashboardForm_KeyPress(object sender, KeyPressEventArgs e)
        {

            Control focusedControl = GetDeepActiveControl(this);

            if (focusedControl is TextBox) return;


            if (e.KeyChar != (char)Keys.Enter)
            {
                _barcodeBuffer.Append(e.KeyChar);
            }
            e.Handled = true; 
        }
        private Control GetDeepActiveControl(Control parent)
        {
            if (parent is ContainerControl container && container.ActiveControl != null)
            {
                return GetDeepActiveControl(container.ActiveControl);
            }
            return parent;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 1. Find out what is REALLY focused
            Control focusedControl = GetDeepActiveControl(this);

            // 2. If typing in a textbox, let the Enter key work normally (don't treat as scan complete)
            if (focusedControl is TextBox)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            // 3. Standard Scanner Logic
            if (keyData == Keys.Enter && _barcodeBuffer.Length > 0)
            {
                HandleDashboardBarcodeScan();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void HandleDashboardBarcodeScan()
        {
            // This method is called when a scan is detected on the dashboard.
            // We intentionally do nothing here but clear the buffer.
            // This prevents the scan from causing an error in the grid.
            _barcodeBuffer.Clear();
            System.Diagnostics.Debug.WriteLine("Barcode scan successfully ignored on Dashboard.");
        }
        // In DashboardForm.cs
        // PASTE THIS ENTIRE METHOD INTO YOUR DASHBOARDFORM CLASS

        /// <summary>
        /// Fetches data and populates the performance chart.
        /// </summary>
        // In DashboardForm.cs
        // REPLACE your existing UpdatePerformanceChart method with this FINAL version.

        /// <summary>
        /// Fetches data and populates the performance chart with a professional dark theme.
        /// </summary>
        // In DashboardForm.cs
        // REPLACE your existing UpdatePerformanceChart method with this FINAL, HIGH-CONTRAST version.

        /// <summary>
        /// Fetches data and populates the performance chart with a professional, readable dark theme.
        /// </summary>
        private void UpdatePerformanceChart()
        {
            if (chartPerformance == null || chartPerformance.Series.Count == 0) return;

            var series = chartPerformance.Series["Series1"];
            var chartArea = chartPerformance.ChartAreas[0];

            // --- CHART-WIDE STYLING ---
            // Instead of Transparent, let's set an explicit dark color to match the form. This is more reliable.
            chartPerformance.BackColor = Color.FromArgb(45, 45, 48);
            chartPerformance.Legends[0].Enabled = false; // Hide the "Series1" legend

            // --- TITLE STYLING ---
            chartPerformance.Titles.Clear();
            var title = chartPerformance.Titles.Add("Top 5 Selling Products (Last 30 Days)");
            title.ForeColor = Color.White; // --- FIX: Use pure White for maximum contrast ---
            title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            // --- PLOT AREA STYLING (ChartArea) ---
            chartArea.BackColor = Color.FromArgb(45, 45, 48); // Match the chart's background color

            // --- AXIS STYLING (X is Product Names, Y is Quantity) ---
            chartArea.AxisX.LabelStyle.ForeColor = Color.White; // --- FIX: Use pure White ---
            chartArea.AxisY.LabelStyle.ForeColor = Color.White; // --- FIX: Use pure White ---
            chartArea.AxisX.LineColor = Color.DarkGray; // A slightly brighter line color
            chartArea.AxisY.LineColor = Color.DarkGray;

            // --- GRID LINE STYLING ---
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64); // Dark gray for subtle lines

            // --- SERIES & DATA POINT STYLING ---
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            series.IsValueShownAsLabel = true;
            series.LabelForeColor = Color.White;
            series.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // --- DATA BINDING ---
            _topProductsData = _repository.GetTopSellingProducts();
            series.Points.Clear();

            foreach (var product in _topProductsData)
            {
                int pointIndex = series.Points.AddXY(product.Label, product.Value);
                var currentPoint = series.Points[pointIndex];
                currentPoint.Color = Color.DodgerBlue;
                currentPoint.BorderColor = Color.Black;
            }
        }
        // In DashboardForm.cs

        /// <summary>
        /// Fetches and populates the daily sales volume line chart.
        /// </summary>
        private void UpdateSalesTrendChart()
        {
            if (chartSalesTrend == null || chartSalesTrend.Series.Count == 0) return;

            var series = chartSalesTrend.Series["Series1"];
            var chartArea = chartSalesTrend.ChartAreas[0];

            // --- CHART-WIDE STYLING ---
            chartSalesTrend.BackColor = Color.FromArgb(45, 45, 48);
            chartSalesTrend.Legends[0].Enabled = false;

            // --- TITLE ---
            chartSalesTrend.Titles.Clear();
            var title = chartSalesTrend.Titles.Add("Daily Sales Volume (Last 30 Days)");
            title.ForeColor = Color.White;
            title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            // --- PLOT AREA & AXES ---
            chartArea.BackColor = Color.FromArgb(45, 45, 48);
            chartArea.AxisX.LabelStyle.ForeColor = Color.White;
            chartArea.AxisY.LabelStyle.ForeColor = Color.White;
            chartArea.AxisX.LineColor = Color.DarkGray;
            chartArea.AxisY.LineColor = Color.DarkGray;
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64);

            // --- SERIES STYLING (Line Chart specific) ---
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series.Color = Color.LimeGreen; // A vibrant color for the line
            series.BorderWidth = 3; // Make the line thicker
            series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle; // Add markers for each day
            series.MarkerSize = 8;
            series.MarkerColor = Color.LimeGreen;

            // --- DATA BINDING ---
            _dailySalesData = _repository.GetDailySalesVolume();
            series.Points.Clear();

            foreach (var day in _dailySalesData)
            {
                series.Points.AddXY(day.Label, day.Value);
            }
        }

        // In DashboardForm.cs
        // In DashboardForm.cs
        // REPLACE your existing dgvProductHistory_CellFormatting method with this one.

        // In DashboardForm.cs
        // REPLACE your existing dgvProductHistory_CellFormatting method with this FINAL version.

        private void dgvProductHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Safety check to prevent errors with new or header rows
            if (e.RowIndex < 0 || dgvProductHistory.Rows[e.RowIndex].DataBoundItem == null)
                return;

            // Get the full transaction object for the current row
            var transaction = dgvProductHistory.Rows[e.RowIndex].DataBoundItem as DashboardTransactionView;

            if (transaction == null)
                return;

            // Get the name of the column we are currently formatting
            string columnName = dgvProductHistory.Columns[e.ColumnIndex].Name;

            // Use a switch statement for clarity and efficiency
            switch (columnName)
            {
                // --- LOGIC FOR THE 'Type' COLUMN ---
                case "colProdHistType":
                    if (transaction.TransactionType == "Delivery")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                    else if (transaction.TransactionType == "Supply")
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    break;

                // --- LOGIC FOR THE 'Qty Change' COLUMN ---
                case "colProdHistQty":
                    int qty = transaction.QuantityChange;

                    if (transaction.TransactionType == "Delivery")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.Value = $"-{Math.Abs(qty)}";
                        e.FormattingApplied = true;
                    }
                    else if (transaction.TransactionType == "Supply")
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.Value = $"+{qty}";
                        e.FormattingApplied = true;
                    }
                    break;

                // --- THIS IS THE CORRECTED LOGIC THAT WAS MISSING ---
                case "colProdHistSource":
                    if (transaction.TransactionType == "Supply")
                    {
                        // For a supply, the source is the supplier
                        e.Value = transaction.SupplierName;
                    }
                    else // For a Delivery
                    {
                        // For a delivery, the destination is the customer
                        e.Value = transaction.CustomerName;
                    }
                    e.FormattingApplied = true; // Tell the grid we have handled this column
                    break;
            }
        }

        // In DashboardForm.cs
        private void btnNewSupply_Click(object sender, EventArgs e)
        {
            // Open the form, telling it to start in Supply mode.
            using (var form = new MultiTransactionForm(TransactionMode.Supply))
            {
                form.ShowDialog(this);
            }
            // Optional: Refresh dashboard data after the form closes.
            LoadDashboardData();
        }

        private void btnQuickDelivery_Click(object sender, EventArgs e)
        {
            using (var form = new MultiTransactionForm(TransactionMode.Delivery))
            {
                form.ShowDialog(this);
            }
            // Optional: Refresh dashboard data after the form closes.
            LoadDashboardData();
        }

        private void tabAnalytics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabAnalytics.SelectedTab == null) return;

            string selectedTabName = tabAnalytics.SelectedTab.Name;

            // --- First, handle panel visibility ---
            pnlTodaysTransactions.Visible = (selectedTabName == "tabTodaysActivity");
            pnlProductMetadata.Visible = (selectedTabName == "tabProductDetails");
            pnlChartData.Visible = (selectedTabName == "tabMonthlyPerformance" ||
                                    selectedTabName == "tabSalesTrend" ||
                                    selectedTabName == "tabSalesBreakdown");

            // --- Second, populate the correct grid with data ---
            switch (selectedTabName)
            {
                case "tabMonthlyPerformance":
                    dgvChartData.Rows.Clear();
                    if (_topProductsData != null)
                    {
                        foreach (var product in _topProductsData)
                        {
                            dgvChartData.Rows.Add(product.Label, product.Value);
                        }
                    }
                    break;

                case "tabSalesTrend":
                    dgvChartData.Rows.Clear();
                    if (_dailySalesData != null)
                    {
                        foreach (var day in _dailySalesData)
                        {
                            dgvChartData.Rows.Add(day.Label, day.Value);
                        }
                    }
                    break;

                case "tabSalesBreakdown":
                    dgvChartData.Rows.Clear();
                    if (_salesBreakdownData != null)
                    {
                        foreach (var dataPoint in _salesBreakdownData)
                        {
                            dgvChartData.Rows.Add(dataPoint.Label, dataPoint.Value.ToString("C"));
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Configures the columns for the detailed 'Today's Transactions' grid.
        /// </summary>
        private void SetupTodaysTransactionsGrid()
        {
            dgvTodaysTransactions.AutoGenerateColumns = false;
            dgvTodaysTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTodaysTransactions.Columns.Clear();

            // Add a column for Time
            var timeCol = new DataGridViewTextBoxColumn { HeaderText = "Time", DataPropertyName = "TransactionDate", FillWeight = 20 };
            timeCol.DefaultCellStyle.Format = "hh:mm:ss tt"; // e.g., 03:30:15 PM
            dgvTodaysTransactions.Columns.Add(timeCol);

            // Add columns for Product, Type, and Price/Cost
            dgvTodaysTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Product", DataPropertyName = "ProductDescription", FillWeight = 50 });
            dgvTodaysTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Type", DataPropertyName = "TransactionType", FillWeight = 15 });
            dgvTodaysTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Value", DataPropertyName = "Price", FillWeight = 15, Name = "colValue" }); // Give this a Name for formatting
        }
        /// <summary>
        /// Fetches today's transactions and binds them to the detail grid.
        /// </summary>
        private void UpdateTodaysTransactionsGrid()
        {
            try
            {
                List<DashboardTransactionView> todaysTransactions = _repository.GetTodaysTransactions();
                dgvTodaysTransactions.DataSource = todaysTransactions;
            }
            catch (Exception ex)
            {
                // Handle error gracefully
                System.Diagnostics.Debug.WriteLine($"Error updating today's transactions grid: {ex.Message}");
            }
        }

        private void dgvTodaysTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvTodaysTransactions.Rows[e.RowIndex].DataBoundItem == null) return;

            var transaction = dgvTodaysTransactions.Rows[e.RowIndex].DataBoundItem as DashboardTransactionView;
            if (transaction == null) return;

            // Color the 'Type' column
            if (dgvTodaysTransactions.Columns[e.ColumnIndex].DataPropertyName == "TransactionType")
            {
                if (transaction.TransactionType == "Delivery") e.CellStyle.ForeColor = Color.Red;
                if (transaction.TransactionType == "Supply") e.CellStyle.ForeColor = Color.Green;
            }

            // Format the 'Value' column
            if (dgvTodaysTransactions.Columns[e.ColumnIndex].Name == "colValue")
            {
                // SIMPLIFIED: The 'Price' property now ALWAYS holds the correct total value
                // thanks to our improved SQL query.
                e.Value = transaction.Price.ToString("C");
                e.FormattingApplied = true;
            }
        }

        private void btnQuickSupply_Click(object sender, EventArgs e)
        {
            using (var form = new MultiTransactionForm(TransactionMode.Supply))
            {
                form.ShowDialog(this);
            }
            // Optional: Refresh dashboard data after the form closes.
            LoadDashboardData();
        }
    }
}