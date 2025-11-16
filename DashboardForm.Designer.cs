namespace InventorySystem
{
    partial class DashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.btnViewReports = new System.Windows.Forms.Button();
            this.imageListNav = new System.Windows.Forms.ImageList(this.components);
            this.lblNotificationCount = new System.Windows.Forms.Label();
            this.btnNavNotifications = new System.Windows.Forms.Button();
            this.btnManageSuppliers = new System.Windows.Forms.Button();
            this.btnNavManageUsers = new System.Windows.Forms.Button();
            this.btnNavLogout = new System.Windows.Forms.Button();
            this.PanelSpacer = new System.Windows.Forms.Panel();
            this.btnViewInventory = new System.Windows.Forms.Button();
            this.pnlMainContent = new System.Windows.Forms.Panel();
            this.pnlTransactionSummary = new System.Windows.Forms.Panel();
            this.lblDateRange = new System.Windows.Forms.Label();
            this.lblSummaryTitle = new System.Windows.Forms.Label();
            this.lblTransactionValue = new System.Windows.Forms.Label();
            this.pnlContextContainer = new System.Windows.Forms.Panel();
            this.pnlTodaysTransactions = new System.Windows.Forms.Panel();
            this.dgvTodaysTransactions = new System.Windows.Forms.DataGridView();
            this.pnlProductMetadata = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDetailSellingPrice = new System.Windows.Forms.Label();
            this.lblDetailPurchaseCost = new System.Windows.Forms.Label();
            this.lblDetailType = new System.Windows.Forms.Label();
            this.lblDetailPartNumber = new System.Windows.Forms.Label();
            this.lblDetailBrand = new System.Windows.Forms.Label();
            this.lblDetailPartNumberTitle = new System.Windows.Forms.Label();
            this.lblDetailBrandTitle = new System.Windows.Forms.Label();
            this.lblDetailTypeTitle = new System.Windows.Forms.Label();
            this.lblDetailPurchaseCostTitle = new System.Windows.Forms.Label();
            this.lblDetailSellingPriceTitle = new System.Windows.Forms.Label();
            this.pnlChartData = new System.Windows.Forms.Panel();
            this.dgvChartData = new System.Windows.Forms.DataGridView();
            this.tabAnalytics = new System.Windows.Forms.TabControl();
            this.tabTodaysActivity = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblItemsSuppliedTitle = new System.Windows.Forms.Label();
            this.lblItemsSuppliedToday = new System.Windows.Forms.Label();
            this.lblSupplyTransTitle = new System.Windows.Forms.Label();
            this.lblSupplyTransactionsToday = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblItemsDeliveredTitle = new System.Windows.Forms.Label();
            this.lblItemsDeliveredToday = new System.Windows.Forms.Label();
            this.lblDeliveryTransTitle = new System.Windows.Forms.Label();
            this.lblDeliveryTransactionsToday = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.lblCostTitle = new System.Windows.Forms.Label();
            this.lblCostSuppliedToday = new System.Windows.Forms.Label();
            this.lblRevenueTitle = new System.Windows.Forms.Label();
            this.lblRevenueToday = new System.Windows.Forms.Label();
            this.lblNetStockTitle = new System.Windows.Forms.Label();
            this.lblNetStockChange = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabProductDetails = new System.Windows.Forms.TabPage();
            this.dgvProductHistory = new System.Windows.Forms.DataGridView();
            this.lblSelectedProductValue = new System.Windows.Forms.Label();
            this.lblTotalValueonHandText = new System.Windows.Forms.Label();
            this.lblSelectedProductStock = new System.Windows.Forms.Label();
            this.lblCurrentStockText = new System.Windows.Forms.Label();
            this.lblSelectedProductName = new System.Windows.Forms.Label();
            this.tabMonthlyPerformance = new System.Windows.Forms.TabPage();
            this.chartPerformance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabSalesTrend = new System.Windows.Forms.TabPage();
            this.chartSalesTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabSalesBreakdown = new System.Windows.Forms.TabPage();
            this.BreakDownByPanel = new System.Windows.Forms.Panel();
            this.lblBreakdownByText = new System.Windows.Forms.Label();
            this.cmbBreakdownType = new System.Windows.Forms.ComboBox();
            this.chartSalesBreakdown = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlQuickActions = new System.Windows.Forms.Panel();
            this.btnQuickDelivery = new System.Windows.Forms.Button();
            this.btnQuickSupply = new System.Windows.Forms.Button();
            this.ucTransactionHistory1 = new InventorySystem.ucTransactionHistory();
            this.dgvRecentTransactions = new System.Windows.Forms.DataGridView();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.pnlKpiContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlKpiTotalProducts = new System.Windows.Forms.Panel();
            this.lblTotalProductsTitle = new System.Windows.Forms.Label();
            this.lblTotalProductsValue = new System.Windows.Forms.Label();
            this.pnlKpiOutOfStock = new System.Windows.Forms.Panel();
            this.lblOutOfStockTitle = new System.Windows.Forms.Label();
            this.lblOutOfStockValue = new System.Windows.Forms.Label();
            this.pnlKpiLowStock = new System.Windows.Forms.Panel();
            this.lblLowStockTitle = new System.Windows.Forms.Label();
            this.lblLowStockValue = new System.Windows.Forms.Label();
            this.pnlKpiInventoryValue = new System.Windows.Forms.Panel();
            this.blInventoryValueTitle = new System.Windows.Forms.Label();
            this.lblInventoryValueValue = new System.Windows.Forms.Label();
            this.pnlNavigation.SuspendLayout();
            this.pnlMainContent.SuspendLayout();
            this.pnlTransactionSummary.SuspendLayout();
            this.pnlContextContainer.SuspendLayout();
            this.pnlTodaysTransactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTodaysTransactions)).BeginInit();
            this.pnlProductMetadata.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlChartData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChartData)).BeginInit();
            this.tabAnalytics.SuspendLayout();
            this.tabTodaysActivity.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabProductDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductHistory)).BeginInit();
            this.tabMonthlyPerformance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPerformance)).BeginInit();
            this.tabSalesTrend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSalesTrend)).BeginInit();
            this.tabSalesBreakdown.SuspendLayout();
            this.BreakDownByPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSalesBreakdown)).BeginInit();
            this.pnlQuickActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentTransactions)).BeginInit();
            this.pnlKpiContainer.SuspendLayout();
            this.pnlKpiTotalProducts.SuspendLayout();
            this.pnlKpiOutOfStock.SuspendLayout();
            this.pnlKpiLowStock.SuspendLayout();
            this.pnlKpiInventoryValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.BackColor = System.Drawing.Color.DodgerBlue;
            this.pnlNavigation.Controls.Add(this.btnViewReports);
            this.pnlNavigation.Controls.Add(this.lblNotificationCount);
            this.pnlNavigation.Controls.Add(this.btnNavNotifications);
            this.pnlNavigation.Controls.Add(this.btnManageSuppliers);
            this.pnlNavigation.Controls.Add(this.btnNavManageUsers);
            this.pnlNavigation.Controls.Add(this.btnNavLogout);
            this.pnlNavigation.Controls.Add(this.PanelSpacer);
            this.pnlNavigation.Controls.Add(this.btnViewInventory);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNavigation.Location = new System.Drawing.Point(0, 0);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Size = new System.Drawing.Size(180, 964);
            this.pnlNavigation.TabIndex = 0;
            // 
            // btnViewReports
            // 
            this.btnViewReports.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnViewReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnViewReports.FlatAppearance.BorderSize = 0;
            this.btnViewReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewReports.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewReports.ForeColor = System.Drawing.Color.White;
            this.btnViewReports.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewReports.ImageIndex = 4;
            this.btnViewReports.ImageList = this.imageListNav;
            this.btnViewReports.Location = new System.Drawing.Point(0, 136);
            this.btnViewReports.Name = "btnViewReports";
            this.btnViewReports.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnViewReports.Size = new System.Drawing.Size(180, 36);
            this.btnViewReports.TabIndex = 7;
            this.btnViewReports.Text = "View Reports";
            this.btnViewReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewReports.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnViewReports.UseVisualStyleBackColor = false;
            this.btnViewReports.Click += new System.EventHandler(this.BtnViewReports_Click);
            this.btnViewReports.MouseEnter += new System.EventHandler(this.BtnViewInventory_MouseEnter);
            this.btnViewReports.MouseLeave += new System.EventHandler(this.BtnViewInventory_MouseLeave);
            // 
            // imageListNav
            // 
            this.imageListNav.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListNav.ImageStream")));
            this.imageListNav.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListNav.Images.SetKeyName(0, "View_Inventory_Icon.png");
            this.imageListNav.Images.SetKeyName(1, "Manage_Users_Icon.png");
            this.imageListNav.Images.SetKeyName(2, "Manage_Suppliers_Icon.png");
            this.imageListNav.Images.SetKeyName(3, "Notifications_Icon.png");
            this.imageListNav.Images.SetKeyName(4, "View_Reports_Icon.png");
            this.imageListNav.Images.SetKeyName(5, "Logout_Icon.png");
            // 
            // lblNotificationCount
            // 
            this.lblNotificationCount.BackColor = System.Drawing.Color.Red;
            this.lblNotificationCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotificationCount.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblNotificationCount.Location = new System.Drawing.Point(139, 111);
            this.lblNotificationCount.Name = "lblNotificationCount";
            this.lblNotificationCount.Size = new System.Drawing.Size(22, 22);
            this.lblNotificationCount.TabIndex = 5;
            this.lblNotificationCount.Text = "0";
            this.lblNotificationCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNavNotifications
            // 
            this.btnNavNotifications.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnNavNotifications.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavNotifications.FlatAppearance.BorderSize = 0;
            this.btnNavNotifications.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavNotifications.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavNotifications.ForeColor = System.Drawing.Color.White;
            this.btnNavNotifications.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavNotifications.ImageIndex = 3;
            this.btnNavNotifications.ImageList = this.imageListNav;
            this.btnNavNotifications.Location = new System.Drawing.Point(0, 104);
            this.btnNavNotifications.Name = "btnNavNotifications";
            this.btnNavNotifications.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnNavNotifications.Size = new System.Drawing.Size(180, 32);
            this.btnNavNotifications.TabIndex = 2;
            this.btnNavNotifications.Text = "Notifications";
            this.btnNavNotifications.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavNotifications.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNavNotifications.UseVisualStyleBackColor = false;
            this.btnNavNotifications.Click += new System.EventHandler(this.BtnNotifications_Click);
            this.btnNavNotifications.MouseEnter += new System.EventHandler(this.BtnViewInventory_MouseEnter);
            this.btnNavNotifications.MouseLeave += new System.EventHandler(this.BtnViewInventory_MouseLeave);
            // 
            // btnManageSuppliers
            // 
            this.btnManageSuppliers.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnManageSuppliers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnManageSuppliers.FlatAppearance.BorderSize = 0;
            this.btnManageSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageSuppliers.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageSuppliers.ForeColor = System.Drawing.Color.White;
            this.btnManageSuppliers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageSuppliers.ImageIndex = 2;
            this.btnManageSuppliers.ImageList = this.imageListNav;
            this.btnManageSuppliers.Location = new System.Drawing.Point(0, 71);
            this.btnManageSuppliers.Name = "btnManageSuppliers";
            this.btnManageSuppliers.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnManageSuppliers.Size = new System.Drawing.Size(180, 33);
            this.btnManageSuppliers.TabIndex = 6;
            this.btnManageSuppliers.Text = "Manage Suppliers";
            this.btnManageSuppliers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageSuppliers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnManageSuppliers.UseVisualStyleBackColor = false;
            this.btnManageSuppliers.Click += new System.EventHandler(this.BtnManageSuppliers_Click);
            this.btnManageSuppliers.MouseEnter += new System.EventHandler(this.BtnViewInventory_MouseEnter);
            this.btnManageSuppliers.MouseLeave += new System.EventHandler(this.BtnViewInventory_MouseLeave);
            // 
            // btnNavManageUsers
            // 
            this.btnNavManageUsers.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnNavManageUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavManageUsers.FlatAppearance.BorderSize = 0;
            this.btnNavManageUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavManageUsers.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavManageUsers.ForeColor = System.Drawing.Color.White;
            this.btnNavManageUsers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavManageUsers.ImageIndex = 1;
            this.btnNavManageUsers.ImageList = this.imageListNav;
            this.btnNavManageUsers.Location = new System.Drawing.Point(0, 38);
            this.btnNavManageUsers.Name = "btnNavManageUsers";
            this.btnNavManageUsers.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnNavManageUsers.Size = new System.Drawing.Size(180, 33);
            this.btnNavManageUsers.TabIndex = 1;
            this.btnNavManageUsers.Text = "Manage Users";
            this.btnNavManageUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavManageUsers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNavManageUsers.UseVisualStyleBackColor = false;
            this.btnNavManageUsers.Click += new System.EventHandler(this.BtnManageUsers_Click);
            this.btnNavManageUsers.MouseEnter += new System.EventHandler(this.BtnViewInventory_MouseEnter);
            this.btnNavManageUsers.MouseLeave += new System.EventHandler(this.BtnViewInventory_MouseLeave);
            // 
            // btnNavLogout
            // 
            this.btnNavLogout.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnNavLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnNavLogout.FlatAppearance.BorderSize = 0;
            this.btnNavLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavLogout.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavLogout.ForeColor = System.Drawing.Color.White;
            this.btnNavLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavLogout.ImageIndex = 5;
            this.btnNavLogout.ImageList = this.imageListNav;
            this.btnNavLogout.Location = new System.Drawing.Point(0, 895);
            this.btnNavLogout.Name = "btnNavLogout";
            this.btnNavLogout.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnNavLogout.Size = new System.Drawing.Size(180, 39);
            this.btnNavLogout.TabIndex = 3;
            this.btnNavLogout.Text = "Logout";
            this.btnNavLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNavLogout.UseVisualStyleBackColor = false;
            this.btnNavLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            this.btnNavLogout.MouseEnter += new System.EventHandler(this.BtnViewInventory_MouseEnter);
            this.btnNavLogout.MouseLeave += new System.EventHandler(this.BtnViewInventory_MouseLeave);
            // 
            // PanelSpacer
            // 
            this.PanelSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelSpacer.Location = new System.Drawing.Point(0, 934);
            this.PanelSpacer.Name = "PanelSpacer";
            this.PanelSpacer.Size = new System.Drawing.Size(180, 30);
            this.PanelSpacer.TabIndex = 4;
            // 
            // btnViewInventory
            // 
            this.btnViewInventory.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnViewInventory.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnViewInventory.FlatAppearance.BorderSize = 0;
            this.btnViewInventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewInventory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewInventory.ForeColor = System.Drawing.Color.White;
            this.btnViewInventory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewInventory.ImageIndex = 0;
            this.btnViewInventory.ImageList = this.imageListNav;
            this.btnViewInventory.Location = new System.Drawing.Point(0, 0);
            this.btnViewInventory.Name = "btnViewInventory";
            this.btnViewInventory.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnViewInventory.Size = new System.Drawing.Size(180, 38);
            this.btnViewInventory.TabIndex = 0;
            this.btnViewInventory.Text = "View Inventory";
            this.btnViewInventory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewInventory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnViewInventory.UseVisualStyleBackColor = false;
            this.btnViewInventory.Click += new System.EventHandler(this.BtnViewInventory_Click);
            this.btnViewInventory.MouseEnter += new System.EventHandler(this.BtnViewInventory_MouseEnter);
            this.btnViewInventory.MouseLeave += new System.EventHandler(this.BtnViewInventory_MouseLeave);
            // 
            // pnlMainContent
            // 
            this.pnlMainContent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMainContent.Controls.Add(this.pnlTransactionSummary);
            this.pnlMainContent.Controls.Add(this.pnlContextContainer);
            this.pnlMainContent.Controls.Add(this.tabAnalytics);
            this.pnlMainContent.Controls.Add(this.pnlQuickActions);
            this.pnlMainContent.Controls.Add(this.ucTransactionHistory1);
            this.pnlMainContent.Controls.Add(this.dgvRecentTransactions);
            this.pnlMainContent.Controls.Add(this.lblHistoryTitle);
            this.pnlMainContent.Controls.Add(this.pnlKpiContainer);
            this.pnlMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContent.Location = new System.Drawing.Point(180, 0);
            this.pnlMainContent.Name = "pnlMainContent";
            this.pnlMainContent.Size = new System.Drawing.Size(1447, 964);
            this.pnlMainContent.TabIndex = 1;
            // 
            // pnlTransactionSummary
            // 
            this.pnlTransactionSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTransactionSummary.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlTransactionSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTransactionSummary.Controls.Add(this.lblDateRange);
            this.pnlTransactionSummary.Controls.Add(this.lblSummaryTitle);
            this.pnlTransactionSummary.Controls.Add(this.lblTransactionValue);
            this.pnlTransactionSummary.Location = new System.Drawing.Point(1051, 618);
            this.pnlTransactionSummary.Name = "pnlTransactionSummary";
            this.pnlTransactionSummary.Size = new System.Drawing.Size(393, 100);
            this.pnlTransactionSummary.TabIndex = 9;
            // 
            // lblDateRange
            // 
            this.lblDateRange.AutoSize = true;
            this.lblDateRange.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateRange.ForeColor = System.Drawing.Color.Black;
            this.lblDateRange.Location = new System.Drawing.Point(3, 42);
            this.lblDateRange.Name = "lblDateRange";
            this.lblDateRange.Size = new System.Drawing.Size(148, 21);
            this.lblDateRange.TabIndex = 9;
            this.lblDateRange.Text = "Showing From... to...";
            this.lblDateRange.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSummaryTitle
            // 
            this.lblSummaryTitle.AutoSize = true;
            this.lblSummaryTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummaryTitle.ForeColor = System.Drawing.Color.Black;
            this.lblSummaryTitle.Location = new System.Drawing.Point(3, 9);
            this.lblSummaryTitle.Name = "lblSummaryTitle";
            this.lblSummaryTitle.Size = new System.Drawing.Size(141, 21);
            this.lblSummaryTitle.TabIndex = 0;
            this.lblSummaryTitle.Text = "Transactions Value:";
            this.lblSummaryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTransactionValue
            // 
            this.lblTransactionValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransactionValue.AutoSize = true;
            this.lblTransactionValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionValue.Location = new System.Drawing.Point(158, 9);
            this.lblTransactionValue.Name = "lblTransactionValue";
            this.lblTransactionValue.Size = new System.Drawing.Size(161, 21);
            this.lblTransactionValue.TabIndex = 8;
            this.lblTransactionValue.Text = "lblTransactionValue";
            this.lblTransactionValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlContextContainer
            // 
            this.pnlContextContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContextContainer.Controls.Add(this.pnlTodaysTransactions);
            this.pnlContextContainer.Controls.Add(this.pnlProductMetadata);
            this.pnlContextContainer.Controls.Add(this.pnlChartData);
            this.pnlContextContainer.Location = new System.Drawing.Point(672, 619);
            this.pnlContextContainer.Name = "pnlContextContainer";
            this.pnlContextContainer.Size = new System.Drawing.Size(370, 342);
            this.pnlContextContainer.TabIndex = 12;
            // 
            // pnlTodaysTransactions
            // 
            this.pnlTodaysTransactions.Controls.Add(this.dgvTodaysTransactions);
            this.pnlTodaysTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTodaysTransactions.Location = new System.Drawing.Point(0, 0);
            this.pnlTodaysTransactions.Name = "pnlTodaysTransactions";
            this.pnlTodaysTransactions.Size = new System.Drawing.Size(370, 342);
            this.pnlTodaysTransactions.TabIndex = 13;
            // 
            // dgvTodaysTransactions
            // 
            this.dgvTodaysTransactions.AllowUserToAddRows = false;
            this.dgvTodaysTransactions.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgvTodaysTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTodaysTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTodaysTransactions.Location = new System.Drawing.Point(0, 0);
            this.dgvTodaysTransactions.Name = "dgvTodaysTransactions";
            this.dgvTodaysTransactions.ReadOnly = true;
            this.dgvTodaysTransactions.RowHeadersVisible = false;
            this.dgvTodaysTransactions.Size = new System.Drawing.Size(370, 342);
            this.dgvTodaysTransactions.TabIndex = 0;
            this.dgvTodaysTransactions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvTodaysTransactions_CellFormatting);
            // 
            // pnlProductMetadata
            // 
            this.pnlProductMetadata.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlProductMetadata.Controls.Add(this.tableLayoutPanel2);
            this.pnlProductMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProductMetadata.Location = new System.Drawing.Point(0, 0);
            this.pnlProductMetadata.Name = "pnlProductMetadata";
            this.pnlProductMetadata.Size = new System.Drawing.Size(370, 342);
            this.pnlProductMetadata.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.lblDetailSellingPrice, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailPurchaseCost, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailType, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailPartNumber, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailBrand, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailPartNumberTitle, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailBrandTitle, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailTypeTitle, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailPurchaseCostTitle, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailSellingPriceTitle, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(370, 342);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // lblDetailSellingPrice
            // 
            this.lblDetailSellingPrice.AutoSize = true;
            this.lblDetailSellingPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailSellingPrice.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailSellingPrice.Location = new System.Drawing.Point(188, 272);
            this.lblDetailSellingPrice.Name = "lblDetailSellingPrice";
            this.lblDetailSellingPrice.Size = new System.Drawing.Size(179, 70);
            this.lblDetailSellingPrice.TabIndex = 9;
            this.lblDetailSellingPrice.Text = "(No Product Selected)";
            this.lblDetailSellingPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailPurchaseCost
            // 
            this.lblDetailPurchaseCost.AutoSize = true;
            this.lblDetailPurchaseCost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailPurchaseCost.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailPurchaseCost.Location = new System.Drawing.Point(188, 204);
            this.lblDetailPurchaseCost.Name = "lblDetailPurchaseCost";
            this.lblDetailPurchaseCost.Size = new System.Drawing.Size(179, 68);
            this.lblDetailPurchaseCost.TabIndex = 8;
            this.lblDetailPurchaseCost.Text = "(No Product Selected)";
            this.lblDetailPurchaseCost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailType
            // 
            this.lblDetailType.AutoSize = true;
            this.lblDetailType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailType.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailType.Location = new System.Drawing.Point(188, 136);
            this.lblDetailType.Name = "lblDetailType";
            this.lblDetailType.Size = new System.Drawing.Size(179, 68);
            this.lblDetailType.TabIndex = 7;
            this.lblDetailType.Text = "(No Product Selected)";
            this.lblDetailType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailPartNumber
            // 
            this.lblDetailPartNumber.AutoSize = true;
            this.lblDetailPartNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailPartNumber.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailPartNumber.Location = new System.Drawing.Point(188, 0);
            this.lblDetailPartNumber.Name = "lblDetailPartNumber";
            this.lblDetailPartNumber.Size = new System.Drawing.Size(179, 68);
            this.lblDetailPartNumber.TabIndex = 5;
            this.lblDetailPartNumber.Text = "(No Product Selected)";
            this.lblDetailPartNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailBrand
            // 
            this.lblDetailBrand.AutoSize = true;
            this.lblDetailBrand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailBrand.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailBrand.Location = new System.Drawing.Point(188, 68);
            this.lblDetailBrand.Name = "lblDetailBrand";
            this.lblDetailBrand.Size = new System.Drawing.Size(179, 68);
            this.lblDetailBrand.TabIndex = 6;
            this.lblDetailBrand.Text = "(No Product Selected)";
            this.lblDetailBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailPartNumberTitle
            // 
            this.lblDetailPartNumberTitle.AutoSize = true;
            this.lblDetailPartNumberTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailPartNumberTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailPartNumberTitle.Location = new System.Drawing.Point(3, 0);
            this.lblDetailPartNumberTitle.Name = "lblDetailPartNumberTitle";
            this.lblDetailPartNumberTitle.Size = new System.Drawing.Size(179, 68);
            this.lblDetailPartNumberTitle.TabIndex = 14;
            this.lblDetailPartNumberTitle.Text = "Part Number:";
            this.lblDetailPartNumberTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailBrandTitle
            // 
            this.lblDetailBrandTitle.AutoSize = true;
            this.lblDetailBrandTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailBrandTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailBrandTitle.Location = new System.Drawing.Point(3, 68);
            this.lblDetailBrandTitle.Name = "lblDetailBrandTitle";
            this.lblDetailBrandTitle.Size = new System.Drawing.Size(179, 68);
            this.lblDetailBrandTitle.TabIndex = 13;
            this.lblDetailBrandTitle.Text = "Brand:";
            this.lblDetailBrandTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailTypeTitle
            // 
            this.lblDetailTypeTitle.AutoSize = true;
            this.lblDetailTypeTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailTypeTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailTypeTitle.Location = new System.Drawing.Point(3, 136);
            this.lblDetailTypeTitle.Name = "lblDetailTypeTitle";
            this.lblDetailTypeTitle.Size = new System.Drawing.Size(179, 68);
            this.lblDetailTypeTitle.TabIndex = 12;
            this.lblDetailTypeTitle.Text = "Type:";
            this.lblDetailTypeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailPurchaseCostTitle
            // 
            this.lblDetailPurchaseCostTitle.AutoSize = true;
            this.lblDetailPurchaseCostTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailPurchaseCostTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailPurchaseCostTitle.Location = new System.Drawing.Point(3, 204);
            this.lblDetailPurchaseCostTitle.Name = "lblDetailPurchaseCostTitle";
            this.lblDetailPurchaseCostTitle.Size = new System.Drawing.Size(179, 68);
            this.lblDetailPurchaseCostTitle.TabIndex = 10;
            this.lblDetailPurchaseCostTitle.Text = "Purchase Cost:";
            this.lblDetailPurchaseCostTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailSellingPriceTitle
            // 
            this.lblDetailSellingPriceTitle.AutoSize = true;
            this.lblDetailSellingPriceTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailSellingPriceTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailSellingPriceTitle.Location = new System.Drawing.Point(3, 272);
            this.lblDetailSellingPriceTitle.Name = "lblDetailSellingPriceTitle";
            this.lblDetailSellingPriceTitle.Size = new System.Drawing.Size(179, 70);
            this.lblDetailSellingPriceTitle.TabIndex = 11;
            this.lblDetailSellingPriceTitle.Text = "Selling Price:";
            this.lblDetailSellingPriceTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlChartData
            // 
            this.pnlChartData.Controls.Add(this.dgvChartData);
            this.pnlChartData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChartData.Location = new System.Drawing.Point(0, 0);
            this.pnlChartData.Name = "pnlChartData";
            this.pnlChartData.Size = new System.Drawing.Size(370, 342);
            this.pnlChartData.TabIndex = 0;
            // 
            // dgvChartData
            // 
            this.dgvChartData.AllowUserToAddRows = false;
            this.dgvChartData.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgvChartData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChartData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChartData.Location = new System.Drawing.Point(0, 0);
            this.dgvChartData.Name = "dgvChartData";
            this.dgvChartData.ReadOnly = true;
            this.dgvChartData.RowHeadersVisible = false;
            this.dgvChartData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChartData.Size = new System.Drawing.Size(370, 342);
            this.dgvChartData.TabIndex = 0;
            // 
            // tabAnalytics
            // 
            this.tabAnalytics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabAnalytics.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabAnalytics.Controls.Add(this.tabTodaysActivity);
            this.tabAnalytics.Controls.Add(this.tabProductDetails);
            this.tabAnalytics.Controls.Add(this.tabMonthlyPerformance);
            this.tabAnalytics.Controls.Add(this.tabSalesTrend);
            this.tabAnalytics.Controls.Add(this.tabSalesBreakdown);
            this.tabAnalytics.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAnalytics.Location = new System.Drawing.Point(6, 619);
            this.tabAnalytics.Name = "tabAnalytics";
            this.tabAnalytics.SelectedIndex = 0;
            this.tabAnalytics.Size = new System.Drawing.Size(660, 342);
            this.tabAnalytics.TabIndex = 10;
            this.tabAnalytics.SelectedIndexChanged += new System.EventHandler(this.tabAnalytics_SelectedIndexChanged);
            // 
            // tabTodaysActivity
            // 
            this.tabTodaysActivity.Controls.Add(this.tableLayoutPanel1);
            this.tabTodaysActivity.Location = new System.Drawing.Point(4, 28);
            this.tabTodaysActivity.Name = "tabTodaysActivity";
            this.tabTodaysActivity.Padding = new System.Windows.Forms.Padding(3);
            this.tabTodaysActivity.Size = new System.Drawing.Size(652, 310);
            this.tabTodaysActivity.TabIndex = 0;
            this.tabTodaysActivity.Text = "Today\'s Activity";
            this.tabTodaysActivity.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 304F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 304F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 304F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(646, 304);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(209, 298);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.lblItemsSuppliedTitle, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblItemsSuppliedToday, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblSupplyTransTitle, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblSupplyTransactionsToday, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 31);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(207, 265);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // lblItemsSuppliedTitle
            // 
            this.lblItemsSuppliedTitle.AutoSize = true;
            this.lblItemsSuppliedTitle.Location = new System.Drawing.Point(3, 0);
            this.lblItemsSuppliedTitle.Name = "lblItemsSuppliedTitle";
            this.lblItemsSuppliedTitle.Size = new System.Drawing.Size(64, 32);
            this.lblItemsSuppliedTitle.TabIndex = 0;
            this.lblItemsSuppliedTitle.Text = "Items Supplied:";
            this.lblItemsSuppliedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblItemsSuppliedToday
            // 
            this.lblItemsSuppliedToday.AutoSize = true;
            this.lblItemsSuppliedToday.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemsSuppliedToday.Location = new System.Drawing.Point(106, 0);
            this.lblItemsSuppliedToday.Name = "lblItemsSuppliedToday";
            this.lblItemsSuppliedToday.Size = new System.Drawing.Size(19, 21);
            this.lblItemsSuppliedToday.TabIndex = 1;
            this.lblItemsSuppliedToday.Text = "0";
            // 
            // lblSupplyTransTitle
            // 
            this.lblSupplyTransTitle.AutoSize = true;
            this.lblSupplyTransTitle.Location = new System.Drawing.Point(3, 132);
            this.lblSupplyTransTitle.Name = "lblSupplyTransTitle";
            this.lblSupplyTransTitle.Size = new System.Drawing.Size(88, 16);
            this.lblSupplyTransTitle.TabIndex = 2;
            this.lblSupplyTransTitle.Text = "Transactions:";
            this.lblSupplyTransTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSupplyTransactionsToday
            // 
            this.lblSupplyTransactionsToday.AutoSize = true;
            this.lblSupplyTransactionsToday.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplyTransactionsToday.Location = new System.Drawing.Point(106, 132);
            this.lblSupplyTransactionsToday.Name = "lblSupplyTransactionsToday";
            this.lblSupplyTransactionsToday.Size = new System.Drawing.Size(19, 21);
            this.lblSupplyTransactionsToday.TabIndex = 3;
            this.lblSupplyTransactionsToday.Text = "0";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 10);
            this.label2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Supply Activity";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.tableLayoutPanel4);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(218, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(209, 298);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.lblItemsDeliveredTitle, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblItemsDeliveredToday, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblDeliveryTransTitle, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblDeliveryTransactionsToday, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 31);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(207, 265);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // lblItemsDeliveredTitle
            // 
            this.lblItemsDeliveredTitle.AutoSize = true;
            this.lblItemsDeliveredTitle.Location = new System.Drawing.Point(3, 0);
            this.lblItemsDeliveredTitle.Name = "lblItemsDeliveredTitle";
            this.lblItemsDeliveredTitle.Size = new System.Drawing.Size(69, 32);
            this.lblItemsDeliveredTitle.TabIndex = 0;
            this.lblItemsDeliveredTitle.Text = "Items Delivered:";
            this.lblItemsDeliveredTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblItemsDeliveredToday
            // 
            this.lblItemsDeliveredToday.AutoSize = true;
            this.lblItemsDeliveredToday.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemsDeliveredToday.Location = new System.Drawing.Point(106, 0);
            this.lblItemsDeliveredToday.Name = "lblItemsDeliveredToday";
            this.lblItemsDeliveredToday.Size = new System.Drawing.Size(19, 21);
            this.lblItemsDeliveredToday.TabIndex = 1;
            this.lblItemsDeliveredToday.Text = "0";
            // 
            // lblDeliveryTransTitle
            // 
            this.lblDeliveryTransTitle.AutoSize = true;
            this.lblDeliveryTransTitle.Location = new System.Drawing.Point(3, 132);
            this.lblDeliveryTransTitle.Name = "lblDeliveryTransTitle";
            this.lblDeliveryTransTitle.Size = new System.Drawing.Size(88, 16);
            this.lblDeliveryTransTitle.TabIndex = 2;
            this.lblDeliveryTransTitle.Text = "Transactions:";
            this.lblDeliveryTransTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDeliveryTransactionsToday
            // 
            this.lblDeliveryTransactionsToday.AutoSize = true;
            this.lblDeliveryTransactionsToday.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeliveryTransactionsToday.Location = new System.Drawing.Point(106, 132);
            this.lblDeliveryTransactionsToday.Name = "lblDeliveryTransactionsToday";
            this.lblDeliveryTransactionsToday.Size = new System.Drawing.Size(19, 21);
            this.lblDeliveryTransactionsToday.TabIndex = 3;
            this.lblDeliveryTransactionsToday.Text = "0";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(207, 10);
            this.label4.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "Delivery Activity";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.tableLayoutPanel5);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(433, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(210, 298);
            this.panel3.TabIndex = 2;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.lblCostTitle, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblCostSuppliedToday, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblRevenueTitle, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblRevenueToday, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblNetStockTitle, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.lblNetStockChange, 1, 2);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 31);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(208, 265);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // lblCostTitle
            // 
            this.lblCostTitle.AutoSize = true;
            this.lblCostTitle.Location = new System.Drawing.Point(3, 0);
            this.lblCostTitle.Name = "lblCostTitle";
            this.lblCostTitle.Size = new System.Drawing.Size(63, 32);
            this.lblCostTitle.TabIndex = 0;
            this.lblCostTitle.Text = "Cost of Supplies:";
            this.lblCostTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCostSuppliedToday
            // 
            this.lblCostSuppliedToday.AutoSize = true;
            this.lblCostSuppliedToday.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostSuppliedToday.Location = new System.Drawing.Point(107, 0);
            this.lblCostSuppliedToday.Name = "lblCostSuppliedToday";
            this.lblCostSuppliedToday.Size = new System.Drawing.Size(50, 21);
            this.lblCostSuppliedToday.TabIndex = 1;
            this.lblCostSuppliedToday.Text = "$0.00";
            // 
            // lblRevenueTitle
            // 
            this.lblRevenueTitle.AutoSize = true;
            this.lblRevenueTitle.Location = new System.Drawing.Point(3, 88);
            this.lblRevenueTitle.Name = "lblRevenueTitle";
            this.lblRevenueTitle.Size = new System.Drawing.Size(65, 32);
            this.lblRevenueTitle.TabIndex = 2;
            this.lblRevenueTitle.Text = "Gross Revenue:";
            this.lblRevenueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRevenueToday
            // 
            this.lblRevenueToday.AutoSize = true;
            this.lblRevenueToday.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRevenueToday.Location = new System.Drawing.Point(107, 88);
            this.lblRevenueToday.Name = "lblRevenueToday";
            this.lblRevenueToday.Size = new System.Drawing.Size(50, 21);
            this.lblRevenueToday.TabIndex = 3;
            this.lblRevenueToday.Text = "$0.00";
            // 
            // lblNetStockTitle
            // 
            this.lblNetStockTitle.AutoSize = true;
            this.lblNetStockTitle.Location = new System.Drawing.Point(3, 176);
            this.lblNetStockTitle.Name = "lblNetStockTitle";
            this.lblNetStockTitle.Size = new System.Drawing.Size(68, 32);
            this.lblNetStockTitle.TabIndex = 4;
            this.lblNetStockTitle.Text = "Net Stock Change:";
            this.lblNetStockTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNetStockChange
            // 
            this.lblNetStockChange.AutoSize = true;
            this.lblNetStockChange.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetStockChange.Location = new System.Drawing.Point(107, 176);
            this.lblNetStockChange.Name = "lblNetStockChange";
            this.lblNetStockChange.Size = new System.Drawing.Size(19, 21);
            this.lblNetStockChange.TabIndex = 5;
            this.lblNetStockChange.Text = "0";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(208, 10);
            this.label6.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 21);
            this.label5.TabIndex = 1;
            this.label5.Text = "Financial Summary";
            // 
            // tabProductDetails
            // 
            this.tabProductDetails.Controls.Add(this.dgvProductHistory);
            this.tabProductDetails.Controls.Add(this.lblSelectedProductValue);
            this.tabProductDetails.Controls.Add(this.lblTotalValueonHandText);
            this.tabProductDetails.Controls.Add(this.lblSelectedProductStock);
            this.tabProductDetails.Controls.Add(this.lblCurrentStockText);
            this.tabProductDetails.Controls.Add(this.lblSelectedProductName);
            this.tabProductDetails.Location = new System.Drawing.Point(4, 28);
            this.tabProductDetails.Name = "tabProductDetails";
            this.tabProductDetails.Size = new System.Drawing.Size(652, 310);
            this.tabProductDetails.TabIndex = 1;
            this.tabProductDetails.Text = "Selected Product Details";
            this.tabProductDetails.UseVisualStyleBackColor = true;
            // 
            // dgvProductHistory
            // 
            this.dgvProductHistory.AllowUserToAddRows = false;
            this.dgvProductHistory.AllowUserToDeleteRows = false;
            this.dgvProductHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvProductHistory.Location = new System.Drawing.Point(0, 120);
            this.dgvProductHistory.Name = "dgvProductHistory";
            this.dgvProductHistory.ReadOnly = true;
            this.dgvProductHistory.Size = new System.Drawing.Size(652, 190);
            this.dgvProductHistory.TabIndex = 6;
            this.dgvProductHistory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProductHistory_CellFormatting);
            // 
            // lblSelectedProductValue
            // 
            this.lblSelectedProductValue.AutoSize = true;
            this.lblSelectedProductValue.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedProductValue.Location = new System.Drawing.Point(155, 75);
            this.lblSelectedProductValue.Name = "lblSelectedProductValue";
            this.lblSelectedProductValue.Size = new System.Drawing.Size(39, 20);
            this.lblSelectedProductValue.TabIndex = 5;
            this.lblSelectedProductValue.Text = "N/A";
            // 
            // lblTotalValueonHandText
            // 
            this.lblTotalValueonHandText.AutoSize = true;
            this.lblTotalValueonHandText.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalValueonHandText.Location = new System.Drawing.Point(3, 75);
            this.lblTotalValueonHandText.Name = "lblTotalValueonHandText";
            this.lblTotalValueonHandText.Size = new System.Drawing.Size(146, 20);
            this.lblTotalValueonHandText.TabIndex = 4;
            this.lblTotalValueonHandText.Text = "Total Value on Hand:";
            // 
            // lblSelectedProductStock
            // 
            this.lblSelectedProductStock.AutoSize = true;
            this.lblSelectedProductStock.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedProductStock.Location = new System.Drawing.Point(109, 41);
            this.lblSelectedProductStock.Name = "lblSelectedProductStock";
            this.lblSelectedProductStock.Size = new System.Drawing.Size(39, 20);
            this.lblSelectedProductStock.TabIndex = 3;
            this.lblSelectedProductStock.Text = "N/A";
            // 
            // lblCurrentStockText
            // 
            this.lblCurrentStockText.AutoSize = true;
            this.lblCurrentStockText.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentStockText.Location = new System.Drawing.Point(3, 41);
            this.lblCurrentStockText.Name = "lblCurrentStockText";
            this.lblCurrentStockText.Size = new System.Drawing.Size(100, 20);
            this.lblCurrentStockText.TabIndex = 2;
            this.lblCurrentStockText.Text = "Current Stock:";
            // 
            // lblSelectedProductName
            // 
            this.lblSelectedProductName.AutoSize = true;
            this.lblSelectedProductName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedProductName.Location = new System.Drawing.Point(3, 10);
            this.lblSelectedProductName.Name = "lblSelectedProductName";
            this.lblSelectedProductName.Size = new System.Drawing.Size(163, 20);
            this.lblSelectedProductName.TabIndex = 1;
            this.lblSelectedProductName.Text = "(No Product Selected)";
            // 
            // tabMonthlyPerformance
            // 
            this.tabMonthlyPerformance.Controls.Add(this.chartPerformance);
            this.tabMonthlyPerformance.Location = new System.Drawing.Point(4, 28);
            this.tabMonthlyPerformance.Name = "tabMonthlyPerformance";
            this.tabMonthlyPerformance.Size = new System.Drawing.Size(652, 310);
            this.tabMonthlyPerformance.TabIndex = 2;
            this.tabMonthlyPerformance.Text = "Monthly Performance";
            this.tabMonthlyPerformance.UseVisualStyleBackColor = true;
            // 
            // chartPerformance
            // 
            this.chartPerformance.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            this.chartPerformance.ChartAreas.Add(chartArea1);
            this.chartPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartPerformance.Legends.Add(legend1);
            this.chartPerformance.Location = new System.Drawing.Point(0, 0);
            this.chartPerformance.Name = "chartPerformance";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartPerformance.Series.Add(series1);
            this.chartPerformance.Size = new System.Drawing.Size(652, 310);
            this.chartPerformance.TabIndex = 0;
            this.chartPerformance.Text = "chart1";
            // 
            // tabSalesTrend
            // 
            this.tabSalesTrend.Controls.Add(this.chartSalesTrend);
            this.tabSalesTrend.Location = new System.Drawing.Point(4, 28);
            this.tabSalesTrend.Name = "tabSalesTrend";
            this.tabSalesTrend.Size = new System.Drawing.Size(652, 310);
            this.tabSalesTrend.TabIndex = 3;
            this.tabSalesTrend.Text = "Sales Trend";
            this.tabSalesTrend.UseVisualStyleBackColor = true;
            // 
            // chartSalesTrend
            // 
            chartArea2.Name = "ChartArea1";
            this.chartSalesTrend.ChartAreas.Add(chartArea2);
            this.chartSalesTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartSalesTrend.Legends.Add(legend2);
            this.chartSalesTrend.Location = new System.Drawing.Point(0, 0);
            this.chartSalesTrend.Name = "chartSalesTrend";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartSalesTrend.Series.Add(series2);
            this.chartSalesTrend.Size = new System.Drawing.Size(652, 310);
            this.chartSalesTrend.TabIndex = 0;
            this.chartSalesTrend.Text = "chart1";
            // 
            // tabSalesBreakdown
            // 
            this.tabSalesBreakdown.Controls.Add(this.BreakDownByPanel);
            this.tabSalesBreakdown.Controls.Add(this.chartSalesBreakdown);
            this.tabSalesBreakdown.Location = new System.Drawing.Point(4, 28);
            this.tabSalesBreakdown.Name = "tabSalesBreakdown";
            this.tabSalesBreakdown.Size = new System.Drawing.Size(652, 310);
            this.tabSalesBreakdown.TabIndex = 4;
            this.tabSalesBreakdown.Text = "Sales Breakdown";
            this.tabSalesBreakdown.UseVisualStyleBackColor = true;
            // 
            // BreakDownByPanel
            // 
            this.BreakDownByPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.BreakDownByPanel.Controls.Add(this.lblBreakdownByText);
            this.BreakDownByPanel.Controls.Add(this.cmbBreakdownType);
            this.BreakDownByPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.BreakDownByPanel.Location = new System.Drawing.Point(0, 0);
            this.BreakDownByPanel.Name = "BreakDownByPanel";
            this.BreakDownByPanel.Size = new System.Drawing.Size(652, 30);
            this.BreakDownByPanel.TabIndex = 3;
            // 
            // lblBreakdownByText
            // 
            this.lblBreakdownByText.AutoSize = true;
            this.lblBreakdownByText.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBreakdownByText.ForeColor = System.Drawing.Color.White;
            this.lblBreakdownByText.Location = new System.Drawing.Point(3, 6);
            this.lblBreakdownByText.Name = "lblBreakdownByText";
            this.lblBreakdownByText.Size = new System.Drawing.Size(106, 20);
            this.lblBreakdownByText.TabIndex = 0;
            this.lblBreakdownByText.Text = "Breakdown By:";
            // 
            // cmbBreakdownType
            // 
            this.cmbBreakdownType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBreakdownType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBreakdownType.FormattingEnabled = true;
            this.cmbBreakdownType.Location = new System.Drawing.Point(115, 3);
            this.cmbBreakdownType.Name = "cmbBreakdownType";
            this.cmbBreakdownType.Size = new System.Drawing.Size(121, 25);
            this.cmbBreakdownType.TabIndex = 1;
            // 
            // chartSalesBreakdown
            // 
            chartArea3.Name = "ChartArea1";
            this.chartSalesBreakdown.ChartAreas.Add(chartArea3);
            this.chartSalesBreakdown.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chartSalesBreakdown.Legends.Add(legend3);
            this.chartSalesBreakdown.Location = new System.Drawing.Point(0, 0);
            this.chartSalesBreakdown.Name = "chartSalesBreakdown";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartSalesBreakdown.Series.Add(series3);
            this.chartSalesBreakdown.Size = new System.Drawing.Size(652, 310);
            this.chartSalesBreakdown.TabIndex = 2;
            this.chartSalesBreakdown.Text = "chart1";
            // 
            // pnlQuickActions
            // 
            this.pnlQuickActions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlQuickActions.Controls.Add(this.btnQuickDelivery);
            this.pnlQuickActions.Controls.Add(this.btnQuickSupply);
            this.pnlQuickActions.Location = new System.Drawing.Point(1048, 724);
            this.pnlQuickActions.Name = "pnlQuickActions";
            this.pnlQuickActions.Size = new System.Drawing.Size(396, 237);
            this.pnlQuickActions.TabIndex = 11;
            // 
            // btnQuickDelivery
            // 
            this.btnQuickDelivery.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuickDelivery.Location = new System.Drawing.Point(123, 93);
            this.btnQuickDelivery.Name = "btnQuickDelivery";
            this.btnQuickDelivery.Size = new System.Drawing.Size(190, 45);
            this.btnQuickDelivery.TabIndex = 1;
            this.btnQuickDelivery.Text = "New Delivery";
            this.btnQuickDelivery.UseVisualStyleBackColor = true;
            this.btnQuickDelivery.Click += new System.EventHandler(this.btnQuickDelivery_Click);
            // 
            // btnQuickSupply
            // 
            this.btnQuickSupply.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuickSupply.Location = new System.Drawing.Point(123, 38);
            this.btnQuickSupply.Name = "btnQuickSupply";
            this.btnQuickSupply.Size = new System.Drawing.Size(190, 43);
            this.btnQuickSupply.TabIndex = 0;
            this.btnQuickSupply.Text = "New Supply";
            this.btnQuickSupply.UseVisualStyleBackColor = true;
            this.btnQuickSupply.Click += new System.EventHandler(this.btnQuickSupply_Click);
            // 
            // ucTransactionHistory1
            // 
            this.ucTransactionHistory1.Location = new System.Drawing.Point(0, 175);
            this.ucTransactionHistory1.Name = "ucTransactionHistory1";
            this.ucTransactionHistory1.Size = new System.Drawing.Size(1444, 437);
            this.ucTransactionHistory1.TabIndex = 0;
            // 
            // dgvRecentTransactions
            // 
            this.dgvRecentTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecentTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecentTransactions.Location = new System.Drawing.Point(0, 175);
            this.dgvRecentTransactions.Name = "dgvRecentTransactions";
            this.dgvRecentTransactions.Size = new System.Drawing.Size(1447, 789);
            this.dgvRecentTransactions.TabIndex = 7;
            this.dgvRecentTransactions.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DgvRecentTransactions_DataError);
            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.AutoSize = true;
            this.lblHistoryTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHistoryTitle.Location = new System.Drawing.Point(0, 150);
            this.lblHistoryTitle.Name = "lblHistoryTitle";
            this.lblHistoryTitle.Size = new System.Drawing.Size(210, 25);
            this.lblHistoryTitle.TabIndex = 6;
            this.lblHistoryTitle.Text = "Recent Transactions";
            // 
            // pnlKpiContainer
            // 
            this.pnlKpiContainer.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlKpiContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpiContainer.Controls.Add(this.pnlKpiTotalProducts);
            this.pnlKpiContainer.Controls.Add(this.pnlKpiOutOfStock);
            this.pnlKpiContainer.Controls.Add(this.pnlKpiLowStock);
            this.pnlKpiContainer.Controls.Add(this.pnlKpiInventoryValue);
            this.pnlKpiContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpiContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlKpiContainer.Name = "pnlKpiContainer";
            this.pnlKpiContainer.Padding = new System.Windows.Forms.Padding(160, 10, 10, 10);
            this.pnlKpiContainer.Size = new System.Drawing.Size(1447, 150);
            this.pnlKpiContainer.TabIndex = 5;
            // 
            // pnlKpiTotalProducts
            // 
            this.pnlKpiTotalProducts.BackColor = System.Drawing.SystemColors.HotTrack;
            this.pnlKpiTotalProducts.Controls.Add(this.lblTotalProductsTitle);
            this.pnlKpiTotalProducts.Controls.Add(this.lblTotalProductsValue);
            this.pnlKpiTotalProducts.Location = new System.Drawing.Point(180, 13);
            this.pnlKpiTotalProducts.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.pnlKpiTotalProducts.Name = "pnlKpiTotalProducts";
            this.pnlKpiTotalProducts.Size = new System.Drawing.Size(200, 100);
            this.pnlKpiTotalProducts.TabIndex = 0;
            this.pnlKpiTotalProducts.Click += new System.EventHandler(this.KpiPanel_Click);
            // 
            // lblTotalProductsTitle
            // 
            this.lblTotalProductsTitle.AutoSize = true;
            this.lblTotalProductsTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalProductsTitle.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTotalProductsTitle.Location = new System.Drawing.Point(34, 17);
            this.lblTotalProductsTitle.Name = "lblTotalProductsTitle";
            this.lblTotalProductsTitle.Size = new System.Drawing.Size(131, 25);
            this.lblTotalProductsTitle.TabIndex = 1;
            this.lblTotalProductsTitle.Text = "Total Products";
            this.lblTotalProductsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalProductsValue
            // 
            this.lblTotalProductsValue.AutoSize = true;
            this.lblTotalProductsValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalProductsValue.ForeColor = System.Drawing.SystemColors.Window;
            this.lblTotalProductsValue.Location = new System.Drawing.Point(80, 42);
            this.lblTotalProductsValue.Name = "lblTotalProductsValue";
            this.lblTotalProductsValue.Size = new System.Drawing.Size(38, 45);
            this.lblTotalProductsValue.TabIndex = 0;
            this.lblTotalProductsValue.Text = "0";
            this.lblTotalProductsValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlKpiOutOfStock
            // 
            this.pnlKpiOutOfStock.BackColor = System.Drawing.Color.SlateGray;
            this.pnlKpiOutOfStock.Controls.Add(this.lblOutOfStockTitle);
            this.pnlKpiOutOfStock.Controls.Add(this.lblOutOfStockValue);
            this.pnlKpiOutOfStock.Location = new System.Drawing.Point(403, 13);
            this.pnlKpiOutOfStock.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.pnlKpiOutOfStock.Name = "pnlKpiOutOfStock";
            this.pnlKpiOutOfStock.Size = new System.Drawing.Size(200, 100);
            this.pnlKpiOutOfStock.TabIndex = 2;
            this.pnlKpiOutOfStock.Click += new System.EventHandler(this.KpiPanel_Click);
            // 
            // lblOutOfStockTitle
            // 
            this.lblOutOfStockTitle.AutoSize = true;
            this.lblOutOfStockTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutOfStockTitle.ForeColor = System.Drawing.SystemColors.Window;
            this.lblOutOfStockTitle.Location = new System.Drawing.Point(46, 17);
            this.lblOutOfStockTitle.Name = "lblOutOfStockTitle";
            this.lblOutOfStockTitle.Size = new System.Drawing.Size(122, 25);
            this.lblOutOfStockTitle.TabIndex = 1;
            this.lblOutOfStockTitle.Text = "Out of Stocks";
            this.lblOutOfStockTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutOfStockValue
            // 
            this.lblOutOfStockValue.AutoSize = true;
            this.lblOutOfStockValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutOfStockValue.ForeColor = System.Drawing.SystemColors.Window;
            this.lblOutOfStockValue.Location = new System.Drawing.Point(80, 42);
            this.lblOutOfStockValue.Name = "lblOutOfStockValue";
            this.lblOutOfStockValue.Size = new System.Drawing.Size(38, 45);
            this.lblOutOfStockValue.TabIndex = 0;
            this.lblOutOfStockValue.Text = "0";
            this.lblOutOfStockValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlKpiLowStock
            // 
            this.pnlKpiLowStock.BackColor = System.Drawing.Color.IndianRed;
            this.pnlKpiLowStock.Controls.Add(this.lblLowStockTitle);
            this.pnlKpiLowStock.Controls.Add(this.lblLowStockValue);
            this.pnlKpiLowStock.Location = new System.Drawing.Point(626, 13);
            this.pnlKpiLowStock.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.pnlKpiLowStock.Name = "pnlKpiLowStock";
            this.pnlKpiLowStock.Size = new System.Drawing.Size(200, 100);
            this.pnlKpiLowStock.TabIndex = 1;
            this.pnlKpiLowStock.Click += new System.EventHandler(this.KpiPanel_Click);
            // 
            // lblLowStockTitle
            // 
            this.lblLowStockTitle.AutoSize = true;
            this.lblLowStockTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowStockTitle.ForeColor = System.Drawing.SystemColors.Window;
            this.lblLowStockTitle.Location = new System.Drawing.Point(52, 17);
            this.lblLowStockTitle.Name = "lblLowStockTitle";
            this.lblLowStockTitle.Size = new System.Drawing.Size(103, 25);
            this.lblLowStockTitle.TabIndex = 1;
            this.lblLowStockTitle.Text = "Low Stocks";
            this.lblLowStockTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowStockValue
            // 
            this.lblLowStockValue.AutoSize = true;
            this.lblLowStockValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowStockValue.ForeColor = System.Drawing.SystemColors.Window;
            this.lblLowStockValue.Location = new System.Drawing.Point(80, 42);
            this.lblLowStockValue.Name = "lblLowStockValue";
            this.lblLowStockValue.Size = new System.Drawing.Size(38, 45);
            this.lblLowStockValue.TabIndex = 0;
            this.lblLowStockValue.Text = "0";
            this.lblLowStockValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlKpiInventoryValue
            // 
            this.pnlKpiInventoryValue.BackColor = System.Drawing.Color.SeaGreen;
            this.pnlKpiInventoryValue.Controls.Add(this.blInventoryValueTitle);
            this.pnlKpiInventoryValue.Controls.Add(this.lblInventoryValueValue);
            this.pnlKpiInventoryValue.Location = new System.Drawing.Point(849, 13);
            this.pnlKpiInventoryValue.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.pnlKpiInventoryValue.Name = "pnlKpiInventoryValue";
            this.pnlKpiInventoryValue.Size = new System.Drawing.Size(323, 100);
            this.pnlKpiInventoryValue.TabIndex = 3;
            // 
            // blInventoryValueTitle
            // 
            this.blInventoryValueTitle.AutoSize = true;
            this.blInventoryValueTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blInventoryValueTitle.ForeColor = System.Drawing.SystemColors.Window;
            this.blInventoryValueTitle.Location = new System.Drawing.Point(83, 17);
            this.blInventoryValueTitle.Name = "blInventoryValueTitle";
            this.blInventoryValueTitle.Size = new System.Drawing.Size(143, 25);
            this.blInventoryValueTitle.TabIndex = 1;
            this.blInventoryValueTitle.Text = "Inventory Value";
            this.blInventoryValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInventoryValueValue
            // 
            this.lblInventoryValueValue.AutoSize = true;
            this.lblInventoryValueValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInventoryValueValue.ForeColor = System.Drawing.SystemColors.Window;
            this.lblInventoryValueValue.Location = new System.Drawing.Point(58, 42);
            this.lblInventoryValueValue.Name = "lblInventoryValueValue";
            this.lblInventoryValueValue.Size = new System.Drawing.Size(38, 45);
            this.lblInventoryValueValue.TabIndex = 0;
            this.lblInventoryValueValue.Text = "0";
            this.lblInventoryValueValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1627, 964);
            this.Controls.Add(this.pnlMainContent);
            this.Controls.Add(this.pnlNavigation);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1643, 1003);
            this.Name = "DashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DashboardForm";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DashboardForm_KeyPress);
            this.pnlNavigation.ResumeLayout(false);
            this.pnlMainContent.ResumeLayout(false);
            this.pnlMainContent.PerformLayout();
            this.pnlTransactionSummary.ResumeLayout(false);
            this.pnlTransactionSummary.PerformLayout();
            this.pnlContextContainer.ResumeLayout(false);
            this.pnlTodaysTransactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTodaysTransactions)).EndInit();
            this.pnlProductMetadata.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.pnlChartData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChartData)).EndInit();
            this.tabAnalytics.ResumeLayout(false);
            this.tabTodaysActivity.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tabProductDetails.ResumeLayout(false);
            this.tabProductDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductHistory)).EndInit();
            this.tabMonthlyPerformance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPerformance)).EndInit();
            this.tabSalesTrend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSalesTrend)).EndInit();
            this.tabSalesBreakdown.ResumeLayout(false);
            this.BreakDownByPanel.ResumeLayout(false);
            this.BreakDownByPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSalesBreakdown)).EndInit();
            this.pnlQuickActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentTransactions)).EndInit();
            this.pnlKpiContainer.ResumeLayout(false);
            this.pnlKpiTotalProducts.ResumeLayout(false);
            this.pnlKpiTotalProducts.PerformLayout();
            this.pnlKpiOutOfStock.ResumeLayout(false);
            this.pnlKpiOutOfStock.PerformLayout();
            this.pnlKpiLowStock.ResumeLayout(false);
            this.pnlKpiLowStock.PerformLayout();
            this.pnlKpiInventoryValue.ResumeLayout(false);
            this.pnlKpiInventoryValue.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNavigation;
        private System.Windows.Forms.Panel pnlMainContent;
        private System.Windows.Forms.Panel pnlKpiTotalProducts;
        private System.Windows.Forms.Label lblTotalProductsValue;
        private System.Windows.Forms.Label lblTotalProductsTitle;
        private System.Windows.Forms.Panel pnlKpiInventoryValue;
        private System.Windows.Forms.Label blInventoryValueTitle;
        private System.Windows.Forms.Label lblInventoryValueValue;
        private System.Windows.Forms.Panel pnlKpiOutOfStock;
        private System.Windows.Forms.Label lblOutOfStockTitle;
        private System.Windows.Forms.Label lblOutOfStockValue;
        private System.Windows.Forms.Panel pnlKpiLowStock;
        private System.Windows.Forms.Label lblLowStockTitle;
        private System.Windows.Forms.Label lblLowStockValue;
        private System.Windows.Forms.FlowLayoutPanel pnlKpiContainer;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.Button btnViewInventory;
        private System.Windows.Forms.DataGridView dgvRecentTransactions;
        private ucTransactionHistory ucTransactionHistory1;
        private System.Windows.Forms.Button btnNavNotifications;
        private System.Windows.Forms.Button btnNavManageUsers;
        private System.Windows.Forms.Button btnNavLogout;
        private System.Windows.Forms.Panel PanelSpacer;
        private System.Windows.Forms.Label lblNotificationCount;
        private System.Windows.Forms.Button btnManageSuppliers;
        private System.Windows.Forms.Button btnViewReports;
        private System.Windows.Forms.ImageList imageListNav;
        private System.Windows.Forms.Label lblTransactionValue;
        private System.Windows.Forms.Panel pnlTransactionSummary;
        private System.Windows.Forms.Label lblSummaryTitle;
        private System.Windows.Forms.Label lblDateRange;
        private System.Windows.Forms.TabControl tabAnalytics;
        private System.Windows.Forms.TabPage tabTodaysActivity;
        private System.Windows.Forms.TabPage tabProductDetails;
        private System.Windows.Forms.Label lblSelectedProductValue;
        private System.Windows.Forms.Label lblTotalValueonHandText;
        private System.Windows.Forms.Label lblSelectedProductStock;
        private System.Windows.Forms.Label lblCurrentStockText;
        private System.Windows.Forms.Label lblSelectedProductName;
        private System.Windows.Forms.DataGridView dgvProductHistory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tabMonthlyPerformance;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPerformance;
        private System.Windows.Forms.TabPage tabSalesTrend;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSalesTrend;
        private System.Windows.Forms.TabPage tabSalesBreakdown;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSalesBreakdown;
        private System.Windows.Forms.ComboBox cmbBreakdownType;
        private System.Windows.Forms.Label lblBreakdownByText;
        private System.Windows.Forms.Panel BreakDownByPanel;
        private System.Windows.Forms.Panel pnlQuickActions;
        private System.Windows.Forms.Button btnQuickDelivery;
        private System.Windows.Forms.Button btnQuickSupply;
        private System.Windows.Forms.Panel pnlContextContainer;
        private System.Windows.Forms.Panel pnlProductMetadata;
        private System.Windows.Forms.Panel pnlChartData;
        private System.Windows.Forms.Label lblDetailSellingPrice;
        private System.Windows.Forms.Label lblDetailPurchaseCost;
        private System.Windows.Forms.Label lblDetailType;
        private System.Windows.Forms.Label lblDetailBrand;
        private System.Windows.Forms.Label lblDetailPartNumber;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dgvChartData;
        private System.Windows.Forms.Label lblDetailTypeTitle;
        private System.Windows.Forms.Label lblDetailPurchaseCostTitle;
        private System.Windows.Forms.Label lblDetailSellingPriceTitle;
        private System.Windows.Forms.Label lblDetailPartNumberTitle;
        private System.Windows.Forms.Label lblDetailBrandTitle;
        private System.Windows.Forms.Panel pnlTodaysTransactions;
        private System.Windows.Forms.DataGridView dgvTodaysTransactions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblItemsSuppliedTitle;
        private System.Windows.Forms.Label lblItemsSuppliedToday;
        private System.Windows.Forms.Label lblSupplyTransTitle;
        private System.Windows.Forms.Label lblSupplyTransactionsToday;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lblItemsDeliveredTitle;
        private System.Windows.Forms.Label lblItemsDeliveredToday;
        private System.Windows.Forms.Label lblDeliveryTransTitle;
        private System.Windows.Forms.Label lblDeliveryTransactionsToday;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label lblCostTitle;
        private System.Windows.Forms.Label lblCostSuppliedToday;
        private System.Windows.Forms.Label lblRevenueTitle;
        private System.Windows.Forms.Label lblRevenueToday;
        private System.Windows.Forms.Label lblNetStockTitle;
        private System.Windows.Forms.Label lblNetStockChange;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}