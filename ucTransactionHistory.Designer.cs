namespace InventorySystem
{
    partial class ucTransactionHistory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chkFilterByDate = new System.Windows.Forms.CheckBox();
            this.btnExportToCSV = new System.Windows.Forms.Button();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpDateFilter = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTypeFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.TransactionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuantityChange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockBefore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockAfter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplierID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplierName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // chkFilterByDate
            // 
            this.chkFilterByDate.AutoSize = true;
            this.chkFilterByDate.Location = new System.Drawing.Point(419, 27);
            this.chkFilterByDate.Name = "chkFilterByDate";
            this.chkFilterByDate.Size = new System.Drawing.Size(59, 17);
            this.chkFilterByDate.TabIndex = 19;
            this.chkFilterByDate.Text = "Enable";
            this.chkFilterByDate.UseVisualStyleBackColor = true;
            this.chkFilterByDate.CheckedChanged += new System.EventHandler(this.chkFilterByDate_CheckedChanged);
            // 
            // btnExportToCSV
            // 
            this.btnExportToCSV.Location = new System.Drawing.Point(496, 21);
            this.btnExportToCSV.Name = "btnExportToCSV";
            this.btnExportToCSV.Size = new System.Drawing.Size(87, 23);
            this.btnExportToCSV.TabIndex = 18;
            this.btnExportToCSV.Text = "Export to CSV";
            this.btnExportToCSV.UseVisualStyleBackColor = true;
            this.btnExportToCSV.Click += new System.EventHandler(this.btnExportToCSV_Click);
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.Location = new System.Drawing.Point(832, 19);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(75, 23);
            this.btnClearFilters.TabIndex = 17;
            this.btnClearFilters.Text = "Clear Filters";
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new System.EventHandler(this.btnClearFilters_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(679, 21);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 20);
            this.txtSearch.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(589, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Search Product:";
            // 
            // dtpDateFilter
            // 
            this.dtpDateFilter.Location = new System.Drawing.Point(212, 25);
            this.dtpDateFilter.Name = "dtpDateFilter";
            this.dtpDateFilter.Size = new System.Drawing.Size(200, 20);
            this.dtpDateFilter.TabIndex = 14;
            this.dtpDateFilter.ValueChanged += new System.EventHandler(this.dtpDateFilter_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Filter by Date:";
            // 
            // cmbTypeFilter
            // 
            this.cmbTypeFilter.FormattingEnabled = true;
            this.cmbTypeFilter.Location = new System.Drawing.Point(7, 24);
            this.cmbTypeFilter.Name = "cmbTypeFilter";
            this.cmbTypeFilter.Size = new System.Drawing.Size(121, 21);
            this.cmbTypeFilter.TabIndex = 12;
            this.cmbTypeFilter.SelectedIndexChanged += new System.EventHandler(this.cmbTypeFilter_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-94, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Filter by Type:";
            // 
            // dgvHistory
            // 
            this.dgvHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TransactionID,
            this.ProductID,
            this.Barcode,
            this.ProductDescription,
            this.TransactionType,
            this.QuantityChange,
            this.Price,
            this.CustomerName,
            this.StockBefore,
            this.StockAfter,
            this.TransactionDate,
            this.SupplierID,
            this.SupplierName});
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvHistory.Location = new System.Drawing.Point(0, 67);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.Size = new System.Drawing.Size(931, 383);
            this.dgvHistory.TabIndex = 10;
            this.dgvHistory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellDoubleClick);
            this.dgvHistory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvHistory_CellFormatting);
            // 
            // TransactionID
            // 
            this.TransactionID.DataPropertyName = "TransactionID";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TransactionID.DefaultCellStyle = dataGridViewCellStyle1;
            this.TransactionID.FillWeight = 50F;
            this.TransactionID.HeaderText = "Transaction ID";
            this.TransactionID.Name = "TransactionID";
            // 
            // ProductID
            // 
            this.ProductID.DataPropertyName = "ProductID";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ProductID.DefaultCellStyle = dataGridViewCellStyle2;
            this.ProductID.FillWeight = 50F;
            this.ProductID.HeaderText = "Product ID";
            this.ProductID.Name = "ProductID";
            // 
            // Barcode
            // 
            this.Barcode.DataPropertyName = "Barcode";
            this.Barcode.HeaderText = "Barcode";
            this.Barcode.Name = "Barcode";
            // 
            // ProductDescription
            // 
            this.ProductDescription.DataPropertyName = "ProductDescription";
            this.ProductDescription.FillWeight = 180F;
            this.ProductDescription.HeaderText = "Product Description";
            this.ProductDescription.Name = "ProductDescription";
            // 
            // TransactionType
            // 
            this.TransactionType.DataPropertyName = "TransactionType";
            this.TransactionType.FillWeight = 70F;
            this.TransactionType.HeaderText = "Transaction Type";
            this.TransactionType.Name = "TransactionType";
            // 
            // QuantityChange
            // 
            this.QuantityChange.DataPropertyName = "QuantityChange";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.QuantityChange.DefaultCellStyle = dataGridViewCellStyle3;
            this.QuantityChange.FillWeight = 70F;
            this.QuantityChange.HeaderText = "Added/Deducted Stock";
            this.QuantityChange.Name = "QuantityChange";
            this.QuantityChange.Visible = false;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Price.DefaultCellStyle = dataGridViewCellStyle4;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            // 
            // CustomerName
            // 
            this.CustomerName.DataPropertyName = "CustomerName";
            this.CustomerName.HeaderText = "Customer Name";
            this.CustomerName.Name = "CustomerName";
            // 
            // StockBefore
            // 
            this.StockBefore.DataPropertyName = "StockBefore";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.StockBefore.DefaultCellStyle = dataGridViewCellStyle5;
            this.StockBefore.FillWeight = 60F;
            this.StockBefore.HeaderText = "Previous Stock";
            this.StockBefore.Name = "StockBefore";
            // 
            // StockAfter
            // 
            this.StockAfter.DataPropertyName = "StockAfter";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.StockAfter.DefaultCellStyle = dataGridViewCellStyle6;
            this.StockAfter.FillWeight = 60F;
            this.StockAfter.HeaderText = "Current Stock";
            this.StockAfter.Name = "StockAfter";
            // 
            // TransactionDate
            // 
            this.TransactionDate.DataPropertyName = "TransactionDate";
            this.TransactionDate.FillWeight = 120F;
            this.TransactionDate.HeaderText = "Transaction Date";
            this.TransactionDate.Name = "TransactionDate";
            // 
            // SupplierID
            // 
            this.SupplierID.DataPropertyName = "SupplierID";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SupplierID.DefaultCellStyle = dataGridViewCellStyle7;
            this.SupplierID.FillWeight = 50F;
            this.SupplierID.HeaderText = "Supplier ID";
            this.SupplierID.Name = "SupplierID";
            // 
            // SupplierName
            // 
            this.SupplierName.DataPropertyName = "SupplierName";
            this.SupplierName.FillWeight = 120F;
            this.SupplierName.HeaderText = "Supplier Name";
            this.SupplierName.Name = "SupplierName";
            // 
            // ucTransactionHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkFilterByDate);
            this.Controls.Add(this.btnExportToCSV);
            this.Controls.Add(this.btnClearFilters);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpDateFilter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbTypeFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvHistory);
            this.Name = "ucTransactionHistory";
            this.Size = new System.Drawing.Size(931, 450);
            this.Load += new System.EventHandler(this.ucTransactionHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkFilterByDate;
        private System.Windows.Forms.Button btnExportToCSV;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpDateFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTypeFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuantityChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockBefore;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockAfter;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupplierID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupplierName;
    }
}