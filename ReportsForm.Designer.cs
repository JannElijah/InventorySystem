namespace InventorySystem
{
    partial class ReportsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportsForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageInventoryStatus = new System.Windows.Forms.TabPage();
            this.dgvInventoryStatus = new System.Windows.Forms.DataGridView();
            this.colPartNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBrand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageTransactionHistory = new System.Windows.Forms.TabPage();
            this.dgvTransactionHistory = new System.Windows.Forms.DataGridView();
            this.dgvTransactionHistory_TransactionIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_ProductIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_BarcodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_ProductDescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_TransactionTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_QuantityChangeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_StockBeforeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_StockAfterColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_TransactionDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTransactionHistory_SupplierNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpSalesReport = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblTotalItemsSold = new System.Windows.Forms.Label();
            this.lblGrossProfitMargin = new System.Windows.Forms.Label();
            this.lblTotalTransactions = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblGrossProfit = new System.Windows.Forms.Label();
            this.lblCostOfGoodsSold = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMostProfitableProduct = new System.Windows.Forms.Label();
            this.lblBestSellingProduct = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReportPeriod = new System.Windows.Forms.Label();
            this.btnFilterTransactions = new System.Windows.Forms.Button();
            this.imageListActions = new System.Windows.Forms.ImageList(this.components);
            this.btnPrintReport = new System.Windows.Forms.Button();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnBackToDashboard = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblTransactionSearch = new System.Windows.Forms.Label();
            this.lblTransactionType = new System.Windows.Forms.Label();
            this.lblInventorySearch = new System.Windows.Forms.Label();
            this.lblInventoryType = new System.Windows.Forms.Label();
            this.lblInventoryBrand = new System.Windows.Forms.Label();
            this.txtTransactionSearch = new System.Windows.Forms.TextBox();
            this.cmbTransactionType = new System.Windows.Forms.ComboBox();
            this.txtInventorySearch = new System.Windows.Forms.TextBox();
            this.cmbInventoryType = new System.Windows.Forms.ComboBox();
            this.cmbInventoryBrand = new System.Windows.Forms.ComboBox();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tabControl1.SuspendLayout();
            this.tabPageInventoryStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryStatus)).BeginInit();
            this.tabPageTransactionHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionHistory)).BeginInit();
            this.tpSalesReport.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageInventoryStatus);
            this.tabControl1.Controls.Add(this.tabPageTransactionHistory);
            this.tabControl1.Controls.Add(this.tpSalesReport);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 70);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1060, 509);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabPageInventoryStatus
            // 
            this.tabPageInventoryStatus.Controls.Add(this.dgvInventoryStatus);
            this.tabPageInventoryStatus.Location = new System.Drawing.Point(4, 22);
            this.tabPageInventoryStatus.Name = "tabPageInventoryStatus";
            this.tabPageInventoryStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInventoryStatus.Size = new System.Drawing.Size(1052, 483);
            this.tabPageInventoryStatus.TabIndex = 0;
            this.tabPageInventoryStatus.Text = "Inventory Status";
            this.tabPageInventoryStatus.UseVisualStyleBackColor = true;
            // 
            // dgvInventoryStatus
            // 
            this.dgvInventoryStatus.BackgroundColor = System.Drawing.Color.White;
            this.dgvInventoryStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvInventoryStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventoryStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPartNumber,
            this.colBrand,
            this.colDescription,
            this.colStockQuantity,
            this.colPurchaseCost,
            this.colTotalValue});
            this.dgvInventoryStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInventoryStatus.Location = new System.Drawing.Point(3, 3);
            this.dgvInventoryStatus.Name = "dgvInventoryStatus";
            this.dgvInventoryStatus.Size = new System.Drawing.Size(1046, 477);
            this.dgvInventoryStatus.TabIndex = 1;
            // 
            // colPartNumber
            // 
            this.colPartNumber.DataPropertyName = "PartNumber";
            this.colPartNumber.FillWeight = 59.7015F;
            this.colPartNumber.HeaderText = "Part Number";
            this.colPartNumber.Name = "colPartNumber";
            // 
            // colBrand
            // 
            this.colBrand.DataPropertyName = "Brand";
            this.colBrand.FillWeight = 78.08495F;
            this.colBrand.HeaderText = "Brand";
            this.colBrand.Name = "colBrand";
            this.colBrand.Width = 131;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "Description";
            this.colDescription.FillWeight = 94.63921F;
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Width = 158;
            // 
            // colStockQuantity
            // 
            this.colStockQuantity.DataPropertyName = "StockQuantity";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colStockQuantity.DefaultCellStyle = dataGridViewCellStyle1;
            this.colStockQuantity.FillWeight = 109.5463F;
            this.colStockQuantity.HeaderText = "Stock Quantity";
            this.colStockQuantity.Name = "colStockQuantity";
            this.colStockQuantity.Width = 184;
            // 
            // colPurchaseCost
            // 
            this.colPurchaseCost.DataPropertyName = "PurchaseCost";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "c";
            this.colPurchaseCost.DefaultCellStyle = dataGridViewCellStyle2;
            this.colPurchaseCost.FillWeight = 122.97F;
            this.colPurchaseCost.HeaderText = "Purchase Cost";
            this.colPurchaseCost.Name = "colPurchaseCost";
            this.colPurchaseCost.Width = 206;
            // 
            // colTotalValue
            // 
            this.colTotalValue.DataPropertyName = "TotalValue";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "c";
            this.colTotalValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTotalValue.FillWeight = 135.0581F;
            this.colTotalValue.HeaderText = "Total Value";
            this.colTotalValue.Name = "colTotalValue";
            this.colTotalValue.Width = 226;
            // 
            // tabPageTransactionHistory
            // 
            this.tabPageTransactionHistory.Controls.Add(this.dgvTransactionHistory);
            this.tabPageTransactionHistory.Location = new System.Drawing.Point(4, 22);
            this.tabPageTransactionHistory.Name = "tabPageTransactionHistory";
            this.tabPageTransactionHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTransactionHistory.Size = new System.Drawing.Size(1052, 483);
            this.tabPageTransactionHistory.TabIndex = 1;
            this.tabPageTransactionHistory.Text = "Transaction History";
            this.tabPageTransactionHistory.UseVisualStyleBackColor = true;
            // 
            // dgvTransactionHistory
            // 
            this.dgvTransactionHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransactionHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransactionHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactionHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTransactionHistory_TransactionIDColumn,
            this.dgvTransactionHistory_ProductIDColumn,
            this.dgvTransactionHistory_BarcodeColumn,
            this.dgvTransactionHistory_ProductDescriptionColumn,
            this.dgvTransactionHistory_TransactionTypeColumn,
            this.dgvTransactionHistory_QuantityChangeColumn,
            this.dgvTransactionHistory_StockBeforeColumn,
            this.dgvTransactionHistory_StockAfterColumn,
            this.dgvTransactionHistory_TransactionDateColumn,
            this.dgvTransactionHistory_SupplierNameColumn});
            this.dgvTransactionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransactionHistory.Location = new System.Drawing.Point(3, 3);
            this.dgvTransactionHistory.Name = "dgvTransactionHistory";
            this.dgvTransactionHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransactionHistory.Size = new System.Drawing.Size(1046, 477);
            this.dgvTransactionHistory.TabIndex = 1;
            // 
            // dgvTransactionHistory_TransactionIDColumn
            // 
            this.dgvTransactionHistory_TransactionIDColumn.DataPropertyName = "TransactionID";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvTransactionHistory_TransactionIDColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTransactionHistory_TransactionIDColumn.FillWeight = 60F;
            this.dgvTransactionHistory_TransactionIDColumn.HeaderText = "Transaction ID";
            this.dgvTransactionHistory_TransactionIDColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_TransactionIDColumn.Name = "dgvTransactionHistory_TransactionIDColumn";
            // 
            // dgvTransactionHistory_ProductIDColumn
            // 
            this.dgvTransactionHistory_ProductIDColumn.DataPropertyName = "ProductID";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvTransactionHistory_ProductIDColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTransactionHistory_ProductIDColumn.FillWeight = 50F;
            this.dgvTransactionHistory_ProductIDColumn.HeaderText = "Product ID";
            this.dgvTransactionHistory_ProductIDColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_ProductIDColumn.Name = "dgvTransactionHistory_ProductIDColumn";
            // 
            // dgvTransactionHistory_BarcodeColumn
            // 
            this.dgvTransactionHistory_BarcodeColumn.DataPropertyName = "Barcode";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvTransactionHistory_BarcodeColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvTransactionHistory_BarcodeColumn.HeaderText = "Barcode";
            this.dgvTransactionHistory_BarcodeColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_BarcodeColumn.Name = "dgvTransactionHistory_BarcodeColumn";
            // 
            // dgvTransactionHistory_ProductDescriptionColumn
            // 
            this.dgvTransactionHistory_ProductDescriptionColumn.DataPropertyName = "ProductDescription";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTransactionHistory_ProductDescriptionColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvTransactionHistory_ProductDescriptionColumn.FillWeight = 250F;
            this.dgvTransactionHistory_ProductDescriptionColumn.HeaderText = "Product Description";
            this.dgvTransactionHistory_ProductDescriptionColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_ProductDescriptionColumn.Name = "dgvTransactionHistory_ProductDescriptionColumn";
            // 
            // dgvTransactionHistory_TransactionTypeColumn
            // 
            this.dgvTransactionHistory_TransactionTypeColumn.DataPropertyName = "TransactionType";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvTransactionHistory_TransactionTypeColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvTransactionHistory_TransactionTypeColumn.FillWeight = 80F;
            this.dgvTransactionHistory_TransactionTypeColumn.HeaderText = "Type";
            this.dgvTransactionHistory_TransactionTypeColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_TransactionTypeColumn.Name = "dgvTransactionHistory_TransactionTypeColumn";
            // 
            // dgvTransactionHistory_QuantityChangeColumn
            // 
            this.dgvTransactionHistory_QuantityChangeColumn.DataPropertyName = "QuantityChange";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvTransactionHistory_QuantityChangeColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvTransactionHistory_QuantityChangeColumn.FillWeight = 50F;
            this.dgvTransactionHistory_QuantityChangeColumn.HeaderText = "Quantity";
            this.dgvTransactionHistory_QuantityChangeColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_QuantityChangeColumn.Name = "dgvTransactionHistory_QuantityChangeColumn";
            // 
            // dgvTransactionHistory_StockBeforeColumn
            // 
            this.dgvTransactionHistory_StockBeforeColumn.DataPropertyName = "StockBefore";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvTransactionHistory_StockBeforeColumn.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvTransactionHistory_StockBeforeColumn.FillWeight = 60F;
            this.dgvTransactionHistory_StockBeforeColumn.HeaderText = "Stock Before";
            this.dgvTransactionHistory_StockBeforeColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_StockBeforeColumn.Name = "dgvTransactionHistory_StockBeforeColumn";
            // 
            // dgvTransactionHistory_StockAfterColumn
            // 
            this.dgvTransactionHistory_StockAfterColumn.DataPropertyName = "StockAfter";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvTransactionHistory_StockAfterColumn.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvTransactionHistory_StockAfterColumn.FillWeight = 60F;
            this.dgvTransactionHistory_StockAfterColumn.HeaderText = "Stock After";
            this.dgvTransactionHistory_StockAfterColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_StockAfterColumn.Name = "dgvTransactionHistory_StockAfterColumn";
            // 
            // dgvTransactionHistory_TransactionDateColumn
            // 
            this.dgvTransactionHistory_TransactionDateColumn.DataPropertyName = "TransactionDate";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.Format = "yyyy-MM-dd HH:mm";
            this.dgvTransactionHistory_TransactionDateColumn.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvTransactionHistory_TransactionDateColumn.FillWeight = 120F;
            this.dgvTransactionHistory_TransactionDateColumn.HeaderText = "Date";
            this.dgvTransactionHistory_TransactionDateColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_TransactionDateColumn.Name = "dgvTransactionHistory_TransactionDateColumn";
            // 
            // dgvTransactionHistory_SupplierNameColumn
            // 
            this.dgvTransactionHistory_SupplierNameColumn.DataPropertyName = "SupplierName";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvTransactionHistory_SupplierNameColumn.DefaultCellStyle = dataGridViewCellStyle13;
            this.dgvTransactionHistory_SupplierNameColumn.HeaderText = "Supplier";
            this.dgvTransactionHistory_SupplierNameColumn.MinimumWidth = 40;
            this.dgvTransactionHistory_SupplierNameColumn.Name = "dgvTransactionHistory_SupplierNameColumn";
            // 
            // tpSalesReport
            // 
            this.tpSalesReport.Controls.Add(this.groupBox3);
            this.tpSalesReport.Controls.Add(this.groupBox1);
            this.tpSalesReport.Controls.Add(this.groupBox2);
            this.tpSalesReport.Controls.Add(this.lblReportPeriod);
            this.tpSalesReport.Location = new System.Drawing.Point(4, 22);
            this.tpSalesReport.Name = "tpSalesReport";
            this.tpSalesReport.Size = new System.Drawing.Size(1052, 483);
            this.tpSalesReport.TabIndex = 2;
            this.tpSalesReport.Text = "Sales & Profitability";
            this.tpSalesReport.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblTotalItemsSold);
            this.groupBox3.Controls.Add(this.lblGrossProfitMargin);
            this.groupBox3.Controls.Add(this.lblTotalTransactions);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(10, 182);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(383, 125);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Key Metrics";
            // 
            // lblTotalItemsSold
            // 
            this.lblTotalItemsSold.AutoSize = true;
            this.lblTotalItemsSold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalItemsSold.Location = new System.Drawing.Point(191, 90);
            this.lblTotalItemsSold.Name = "lblTotalItemsSold";
            this.lblTotalItemsSold.Size = new System.Drawing.Size(47, 15);
            this.lblTotalItemsSold.TabIndex = 11;
            this.lblTotalItemsSold.Text = "label11";
            // 
            // lblGrossProfitMargin
            // 
            this.lblGrossProfitMargin.AutoSize = true;
            this.lblGrossProfitMargin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrossProfitMargin.Location = new System.Drawing.Point(191, 39);
            this.lblGrossProfitMargin.Name = "lblGrossProfitMargin";
            this.lblGrossProfitMargin.Size = new System.Drawing.Size(40, 15);
            this.lblGrossProfitMargin.TabIndex = 9;
            this.lblGrossProfitMargin.Text = "label9";
            // 
            // lblTotalTransactions
            // 
            this.lblTotalTransactions.AutoSize = true;
            this.lblTotalTransactions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTransactions.Location = new System.Drawing.Point(191, 64);
            this.lblTotalTransactions.Name = "lblTotalTransactions";
            this.lblTotalTransactions.Size = new System.Drawing.Size(47, 15);
            this.lblTotalTransactions.TabIndex = 10;
            this.lblTotalTransactions.Text = "label10";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(10, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 15);
            this.label6.TabIndex = 6;
            this.label6.Text = "Gross Profit Margin:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "Total Transactions:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(10, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 15);
            this.label8.TabIndex = 8;
            this.label8.Text = "Total Items Sold:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblGrossProfit);
            this.groupBox1.Controls.Add(this.lblCostOfGoodsSold);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblTotalRevenue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(464, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 118);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Financial Summary";
            // 
            // lblGrossProfit
            // 
            this.lblGrossProfit.AutoSize = true;
            this.lblGrossProfit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrossProfit.Location = new System.Drawing.Point(181, 87);
            this.lblGrossProfit.Name = "lblGrossProfit";
            this.lblGrossProfit.Size = new System.Drawing.Size(38, 15);
            this.lblGrossProfit.TabIndex = 5;
            this.lblGrossProfit.Text = "$0.00";
            // 
            // lblCostOfGoodsSold
            // 
            this.lblCostOfGoodsSold.AutoSize = true;
            this.lblCostOfGoodsSold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostOfGoodsSold.Location = new System.Drawing.Point(181, 62);
            this.lblCostOfGoodsSold.Name = "lblCostOfGoodsSold";
            this.lblCostOfGoodsSold.Size = new System.Drawing.Size(38, 15);
            this.lblCostOfGoodsSold.TabIndex = 3;
            this.lblCostOfGoodsSold.Text = "$0.00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Gross Profit:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Revenue:";
            // 
            // lblTotalRevenue
            // 
            this.lblTotalRevenue.AutoSize = true;
            this.lblTotalRevenue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalRevenue.Location = new System.Drawing.Point(181, 38);
            this.lblTotalRevenue.Name = "lblTotalRevenue";
            this.lblTotalRevenue.Size = new System.Drawing.Size(38, 15);
            this.lblTotalRevenue.TabIndex = 1;
            this.lblTotalRevenue.Text = "$0.00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Cost of Goods Sold:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblMostProfitableProduct);
            this.groupBox2.Controls.Add(this.lblBestSellingProduct);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 99);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Top Performers";
            // 
            // lblMostProfitableProduct
            // 
            this.lblMostProfitableProduct.AutoSize = true;
            this.lblMostProfitableProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMostProfitableProduct.Location = new System.Drawing.Point(191, 68);
            this.lblMostProfitableProduct.Name = "lblMostProfitableProduct";
            this.lblMostProfitableProduct.Size = new System.Drawing.Size(116, 15);
            this.lblMostProfitableProduct.TabIndex = 3;
            this.lblMostProfitableProduct.Text = "MostProfitableData";
            // 
            // lblBestSellingProduct
            // 
            this.lblBestSellingProduct.AutoSize = true;
            this.lblBestSellingProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBestSellingProduct.Location = new System.Drawing.Point(191, 37);
            this.lblBestSellingProduct.Name = "lblBestSellingProduct";
            this.lblBestSellingProduct.Size = new System.Drawing.Size(95, 15);
            this.lblBestSellingProduct.TabIndex = 2;
            this.lblBestSellingProduct.Text = "BestSellingData";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Most Profitable Product:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Best-Selling Product:";
            // 
            // lblReportPeriod
            // 
            this.lblReportPeriod.AutoSize = true;
            this.lblReportPeriod.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportPeriod.Location = new System.Drawing.Point(282, 14);
            this.lblReportPeriod.Name = "lblReportPeriod";
            this.lblReportPeriod.Size = new System.Drawing.Size(111, 21);
            this.lblReportPeriod.TabIndex = 7;
            this.lblReportPeriod.Text = "ReportPeriod";
            // 
            // btnFilterTransactions
            // 
            this.btnFilterTransactions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFilterTransactions.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFilterTransactions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterTransactions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilterTransactions.ImageList = this.imageListActions;
            this.btnFilterTransactions.Location = new System.Drawing.Point(12, 59);
            this.btnFilterTransactions.Name = "btnFilterTransactions";
            this.btnFilterTransactions.Size = new System.Drawing.Size(110, 32);
            this.btnFilterTransactions.TabIndex = 0;
            this.btnFilterTransactions.Text = "Filter by Date";
            this.btnFilterTransactions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFilterTransactions.UseVisualStyleBackColor = false;
            this.btnFilterTransactions.Click += new System.EventHandler(this.BtnFilterTransactions_Click);
            // 
            // imageListActions
            // 
            this.imageListActions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListActions.ImageStream")));
            this.imageListActions.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListActions.Images.SetKeyName(0, "arrow-left-circle.png");
            this.imageListActions.Images.SetKeyName(1, "printer.png");
            // 
            // btnPrintReport
            // 
            this.btnPrintReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintReport.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPrintReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintReport.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintReport.ImageIndex = 1;
            this.btnPrintReport.ImageList = this.imageListActions;
            this.btnPrintReport.Location = new System.Drawing.Point(934, 59);
            this.btnPrintReport.Name = "btnPrintReport";
            this.btnPrintReport.Size = new System.Drawing.Size(114, 32);
            this.btnPrintReport.TabIndex = 1;
            this.btnPrintReport.Text = "Print Report";
            this.btnPrintReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrintReport.UseVisualStyleBackColor = false;
            this.btnPrintReport.Click += new System.EventHandler(this.BtnPrintReport_Click);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dtpStartDate.Location = new System.Drawing.Point(269, 70);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(200, 20);
            this.dtpStartDate.TabIndex = 3;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.DtpStartDate_ValueChanged);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dtpEndDate.Location = new System.Drawing.Point(486, 71);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(200, 20);
            this.dtpEndDate.TabIndex = 4;
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.DtpEndDate_ValueChanged);
            // 
            // btnBackToDashboard
            // 
            this.btnBackToDashboard.FlatAppearance.BorderSize = 0;
            this.btnBackToDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackToDashboard.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackToDashboard.ForeColor = System.Drawing.Color.White;
            this.btnBackToDashboard.ImageIndex = 0;
            this.btnBackToDashboard.ImageList = this.imageListActions;
            this.btnBackToDashboard.Location = new System.Drawing.Point(10, 12);
            this.btnBackToDashboard.Name = "btnBackToDashboard";
            this.btnBackToDashboard.Size = new System.Drawing.Size(191, 35);
            this.btnBackToDashboard.TabIndex = 2;
            this.btnBackToDashboard.Text = "Back to Dashboard";
            this.btnBackToDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBackToDashboard.UseVisualStyleBackColor = true;
            this.btnBackToDashboard.Click += new System.EventHandler(this.BtnBackToDashboard_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlHeader.Controls.Add(this.btnBackToDashboard);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1060, 70);
            this.pnlHeader.TabIndex = 2;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlFooter.Controls.Add(this.lblTransactionSearch);
            this.pnlFooter.Controls.Add(this.lblTransactionType);
            this.pnlFooter.Controls.Add(this.lblInventorySearch);
            this.pnlFooter.Controls.Add(this.lblInventoryType);
            this.pnlFooter.Controls.Add(this.lblInventoryBrand);
            this.pnlFooter.Controls.Add(this.txtTransactionSearch);
            this.pnlFooter.Controls.Add(this.cmbTransactionType);
            this.pnlFooter.Controls.Add(this.txtInventorySearch);
            this.pnlFooter.Controls.Add(this.cmbInventoryType);
            this.pnlFooter.Controls.Add(this.cmbInventoryBrand);
            this.pnlFooter.Controls.Add(this.btnFilterTransactions);
            this.pnlFooter.Controls.Add(this.btnPrintReport);
            this.pnlFooter.Controls.Add(this.dtpEndDate);
            this.pnlFooter.Controls.Add(this.dtpStartDate);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 579);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1060, 103);
            this.pnlFooter.TabIndex = 2;
            // 
            // lblTransactionSearch
            // 
            this.lblTransactionSearch.AutoSize = true;
            this.lblTransactionSearch.Location = new System.Drawing.Point(201, 21);
            this.lblTransactionSearch.Name = "lblTransactionSearch";
            this.lblTransactionSearch.Size = new System.Drawing.Size(84, 13);
            this.lblTransactionSearch.TabIndex = 16;
            this.lblTransactionSearch.Text = "Search Product:";
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.AutoSize = true;
            this.lblTransactionType.Location = new System.Drawing.Point(27, 21);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(34, 13);
            this.lblTransactionType.TabIndex = 15;
            this.lblTransactionType.Text = "Type:";
            // 
            // lblInventorySearch
            // 
            this.lblInventorySearch.AutoSize = true;
            this.lblInventorySearch.Location = new System.Drawing.Point(465, 21);
            this.lblInventorySearch.Name = "lblInventorySearch";
            this.lblInventorySearch.Size = new System.Drawing.Size(44, 13);
            this.lblInventorySearch.TabIndex = 14;
            this.lblInventorySearch.Text = "Search:";
            // 
            // lblInventoryType
            // 
            this.lblInventoryType.AutoSize = true;
            this.lblInventoryType.Location = new System.Drawing.Point(27, 18);
            this.lblInventoryType.Name = "lblInventoryType";
            this.lblInventoryType.Size = new System.Drawing.Size(34, 13);
            this.lblInventoryType.TabIndex = 13;
            this.lblInventoryType.Text = "Type:";
            // 
            // lblInventoryBrand
            // 
            this.lblInventoryBrand.AutoSize = true;
            this.lblInventoryBrand.Location = new System.Drawing.Point(201, 22);
            this.lblInventoryBrand.Name = "lblInventoryBrand";
            this.lblInventoryBrand.Size = new System.Drawing.Size(38, 13);
            this.lblInventoryBrand.TabIndex = 12;
            this.lblInventoryBrand.Text = "Brand:";
            // 
            // txtTransactionSearch
            // 
            this.txtTransactionSearch.Location = new System.Drawing.Point(291, 18);
            this.txtTransactionSearch.Name = "txtTransactionSearch";
            this.txtTransactionSearch.Size = new System.Drawing.Size(100, 20);
            this.txtTransactionSearch.TabIndex = 9;
            this.txtTransactionSearch.TextChanged += new System.EventHandler(this.TxtTransactionSearch_TextChanged);
            // 
            // cmbTransactionType
            // 
            this.cmbTransactionType.FormattingEnabled = true;
            this.cmbTransactionType.Location = new System.Drawing.Point(67, 16);
            this.cmbTransactionType.Name = "cmbTransactionType";
            this.cmbTransactionType.Size = new System.Drawing.Size(121, 21);
            this.cmbTransactionType.TabIndex = 8;
            this.cmbTransactionType.SelectedIndexChanged += new System.EventHandler(this.CmbTransactionType_SelectedIndexChanged);
            // 
            // txtInventorySearch
            // 
            this.txtInventorySearch.Location = new System.Drawing.Point(515, 15);
            this.txtInventorySearch.Name = "txtInventorySearch";
            this.txtInventorySearch.Size = new System.Drawing.Size(100, 20);
            this.txtInventorySearch.TabIndex = 7;
            this.txtInventorySearch.TextChanged += new System.EventHandler(this.TxtInventorySearch_TextChanged);
            // 
            // cmbInventoryType
            // 
            this.cmbInventoryType.FormattingEnabled = true;
            this.cmbInventoryType.Location = new System.Drawing.Point(67, 15);
            this.cmbInventoryType.Name = "cmbInventoryType";
            this.cmbInventoryType.Size = new System.Drawing.Size(121, 21);
            this.cmbInventoryType.TabIndex = 6;
            this.cmbInventoryType.SelectedIndexChanged += new System.EventHandler(this.CmbInventoryType_SelectedIndexChanged);
            // 
            // cmbInventoryBrand
            // 
            this.cmbInventoryBrand.FormattingEnabled = true;
            this.cmbInventoryBrand.Location = new System.Drawing.Point(245, 15);
            this.cmbInventoryBrand.Name = "cmbInventoryBrand";
            this.cmbInventoryBrand.Size = new System.Drawing.Size(121, 21);
            this.cmbInventoryBrand.TabIndex = 5;
            this.cmbInventoryBrand.SelectedIndexChanged += new System.EventHandler(this.CmbInventoryBrand_SelectedIndexChanged);
            // 
            // printDocument1
            // 
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.PrintDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument1_PrintPage);
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1060, 682);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Reports";
            this.Load += new System.EventHandler(this.ReportsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageInventoryStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryStatus)).EndInit();
            this.tabPageTransactionHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionHistory)).EndInit();
            this.tpSalesReport.ResumeLayout(false);
            this.tpSalesReport.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageInventoryStatus;
        private System.Windows.Forms.TabPage tabPageTransactionHistory;
        private System.Windows.Forms.DataGridView dgvTransactionHistory;
        private System.Windows.Forms.DataGridView dgvInventoryStatus;
        private System.Windows.Forms.Button btnFilterTransactions;
        private System.Windows.Forms.Button btnPrintReport;
        private System.Windows.Forms.Button btnBackToDashboard;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ImageList imageListActions;
        private System.Windows.Forms.TabPage tpSalesReport;
        private System.Windows.Forms.Label lblGrossProfit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCostOfGoodsSold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblReportPeriod;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblMostProfitableProduct;
        private System.Windows.Forms.Label lblBestSellingProduct;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalItemsSold;
        private System.Windows.Forms.Label lblTotalTransactions;
        private System.Windows.Forms.Label lblGrossProfitMargin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPartNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBrand;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalValue;
        private System.Windows.Forms.TextBox txtTransactionSearch;
        private System.Windows.Forms.ComboBox cmbTransactionType;
        private System.Windows.Forms.TextBox txtInventorySearch;
        private System.Windows.Forms.ComboBox cmbInventoryType;
        private System.Windows.Forms.ComboBox cmbInventoryBrand;
        private System.Windows.Forms.Label lblInventoryType;
        private System.Windows.Forms.Label lblInventoryBrand;
        private System.Windows.Forms.Label lblTransactionSearch;
        private System.Windows.Forms.Label lblTransactionType;
        private System.Windows.Forms.Label lblInventorySearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_TransactionIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_ProductIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_BarcodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_ProductDescriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_TransactionTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_QuantityChangeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_StockBeforeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_StockAfterColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_TransactionDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionHistory_SupplierNameColumn;
    }
}