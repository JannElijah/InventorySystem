namespace InventorySystem
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvInventory = new System.Windows.Forms.DataGridView();
            this.ProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Application = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PurchaseCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellingPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowStockThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvInventoryContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editProductToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteProductToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printBarcodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblFilterBrand = new System.Windows.Forms.Label();
            this.cmbBrandFilter = new System.Windows.Forms.ComboBox();
            this.lblFilterType = new System.Windows.Forms.Label();
            this.cmbTypeFilter = new System.Windows.Forms.ComboBox();
            this.btnViewHistory = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageListActions = new System.Windows.Forms.ImageList(this.components);
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pnlheader = new System.Windows.Forms.Panel();
            this.btnBulkEdit = new System.Windows.Forms.Button();
            this.btnBackToDashboard = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.btnPrintBarcode = new System.Windows.Forms.Button();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlCurrentTransaction = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnEditSaleItem = new System.Windows.Forms.Button();
            this.btnCancelTransaction = new System.Windows.Forms.Button();
            this.btnConfirmTransaction = new System.Windows.Forms.Button();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.dgvCurrentSale = new System.Windows.Forms.DataGridView();
            this.SaleItemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaleItemQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaleItemUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaleItemTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaleItemProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblContextName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).BeginInit();
            this.dgvInventoryContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productBindingSource)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.pnlheader.SuspendLayout();
            this.pnlCurrentTransaction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentSale)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInventory
            // 
            this.dgvInventory.AutoGenerateColumns = false;
            this.dgvInventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInventory.BackgroundColor = System.Drawing.Color.White;
            this.dgvInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductID,
            this.Barcode,
            this.Brand,
            this.Description,
            this.Volume,
            this.Type,
            this.Application,
            this.Notes,
            this.PartNumber,
            this.PurchaseCost,
            this.SellingPrice,
            this.StockQuantity,
            this.LowStockThreshold});
            this.dgvInventory.ContextMenuStrip = this.dgvInventoryContextMenu;
            this.dgvInventory.DataSource = this.productBindingSource;
            this.dgvInventory.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvInventory.GridColor = System.Drawing.Color.Gainsboro;
            this.dgvInventory.Location = new System.Drawing.Point(0, 100);
            this.dgvInventory.Name = "dgvInventory";
            this.dgvInventory.RowHeadersVisible = false;
            this.dgvInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInventory.Size = new System.Drawing.Size(1213, 739);
            this.dgvInventory.TabIndex = 0;
            this.dgvInventory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvInventory_CellDoubleClick);
            this.dgvInventory.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvInventory_CellPainting);
            this.dgvInventory.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DgvInventory_DataError);
            this.dgvInventory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DgvInventory_MouseDown);
            // 
            // ProductID
            // 
            this.ProductID.DataPropertyName = "ProductID";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ProductID.DefaultCellStyle = dataGridViewCellStyle17;
            this.ProductID.FillWeight = 60F;
            this.ProductID.HeaderText = "ProductID";
            this.ProductID.Name = "ProductID";
            // 
            // Barcode
            // 
            this.Barcode.DataPropertyName = "Barcode";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Barcode.DefaultCellStyle = dataGridViewCellStyle18;
            this.Barcode.HeaderText = "Barcode";
            this.Barcode.Name = "Barcode";
            // 
            // Brand
            // 
            this.Brand.DataPropertyName = "Brand";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Brand.DefaultCellStyle = dataGridViewCellStyle19;
            this.Brand.HeaderText = "Brand";
            this.Brand.Name = "Brand";
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Description.DefaultCellStyle = dataGridViewCellStyle20;
            this.Description.FillWeight = 250F;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            // 
            // Volume
            // 
            this.Volume.DataPropertyName = "Volume";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Volume.DefaultCellStyle = dataGridViewCellStyle21;
            this.Volume.FillWeight = 60F;
            this.Volume.HeaderText = "Volume";
            this.Volume.Name = "Volume";
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Type.DefaultCellStyle = dataGridViewCellStyle22;
            this.Type.FillWeight = 80F;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // Application
            // 
            this.Application.DataPropertyName = "Application";
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Application.DefaultCellStyle = dataGridViewCellStyle23;
            this.Application.HeaderText = "Application";
            this.Application.Name = "Application";
            // 
            // Notes
            // 
            this.Notes.DataPropertyName = "Notes";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Notes.DefaultCellStyle = dataGridViewCellStyle24;
            this.Notes.HeaderText = "Notes";
            this.Notes.Name = "Notes";
            // 
            // PartNumber
            // 
            this.PartNumber.DataPropertyName = "PartNumber";
            this.PartNumber.FillWeight = 120F;
            this.PartNumber.HeaderText = "Part Number";
            this.PartNumber.Name = "PartNumber";
            // 
            // PurchaseCost
            // 
            this.PurchaseCost.DataPropertyName = "PurchaseCost";
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle25.Format = "c";
            dataGridViewCellStyle25.NullValue = null;
            this.PurchaseCost.DefaultCellStyle = dataGridViewCellStyle25;
            this.PurchaseCost.FillWeight = 80F;
            this.PurchaseCost.HeaderText = "Purchase Cost";
            this.PurchaseCost.Name = "PurchaseCost";
            // 
            // SellingPrice
            // 
            this.SellingPrice.DataPropertyName = "SellingPrice";
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle26.NullValue = "C";
            this.SellingPrice.DefaultCellStyle = dataGridViewCellStyle26;
            this.SellingPrice.FillWeight = 80F;
            this.SellingPrice.HeaderText = "Selling Price";
            this.SellingPrice.Name = "SellingPrice";
            // 
            // StockQuantity
            // 
            this.StockQuantity.DataPropertyName = "StockQuantity";
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.StockQuantity.DefaultCellStyle = dataGridViewCellStyle27;
            this.StockQuantity.FillWeight = 60F;
            this.StockQuantity.HeaderText = "Stock Quantity";
            this.StockQuantity.Name = "StockQuantity";
            // 
            // LowStockThreshold
            // 
            this.LowStockThreshold.DataPropertyName = "LowStockThreshold";
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.LowStockThreshold.DefaultCellStyle = dataGridViewCellStyle28;
            this.LowStockThreshold.HeaderText = "Low Stock Threshold";
            this.LowStockThreshold.Name = "LowStockThreshold";
            this.LowStockThreshold.Visible = false;
            // 
            // dgvInventoryContextMenu
            // 
            this.dgvInventoryContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editProductToolStripMenuItem,
            this.deleteProductToolStripMenuItem,
            this.printBarcodeToolStripMenuItem});
            this.dgvInventoryContextMenu.Name = "contextMenuStrip1";
            this.dgvInventoryContextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // editProductToolStripMenuItem
            // 
            this.editProductToolStripMenuItem.Image = global::InventorySystem.Properties.Resources.edit_2__1_;
            this.editProductToolStripMenuItem.Name = "editProductToolStripMenuItem";
            this.editProductToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editProductToolStripMenuItem.Text = "Edit Product";
            this.editProductToolStripMenuItem.Click += new System.EventHandler(this.EditProductToolStripMenuItem_Click);
            // 
            // deleteProductToolStripMenuItem
            // 
            this.deleteProductToolStripMenuItem.Image = global::InventorySystem.Properties.Resources.trash_2__1_;
            this.deleteProductToolStripMenuItem.Name = "deleteProductToolStripMenuItem";
            this.deleteProductToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteProductToolStripMenuItem.Text = "Delete Product";
            this.deleteProductToolStripMenuItem.Click += new System.EventHandler(this.DeleteProductToolStripMenuItem_Click);
            // 
            // printBarcodeToolStripMenuItem
            // 
            this.printBarcodeToolStripMenuItem.Image = global::InventorySystem.Properties.Resources.printer;
            this.printBarcodeToolStripMenuItem.Name = "printBarcodeToolStripMenuItem";
            this.printBarcodeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.printBarcodeToolStripMenuItem.Text = "Print Barcode";
            this.printBarcodeToolStripMenuItem.Click += new System.EventHandler(this.PrintBarcodeToolStripMenuItem_Click);
            // 
            // lblFilterBrand
            // 
            this.lblFilterBrand.AutoSize = true;
            this.lblFilterBrand.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilterBrand.Location = new System.Drawing.Point(7, 48);
            this.lblFilterBrand.Name = "lblFilterBrand";
            this.lblFilterBrand.Size = new System.Drawing.Size(95, 17);
            this.lblFilterBrand.TabIndex = 1;
            this.lblFilterBrand.Text = "Filter by Brand:";
            // 
            // cmbBrandFilter
            // 
            this.cmbBrandFilter.AccessibleName = "cmbBrandFilter";
            this.cmbBrandFilter.FormattingEnabled = true;
            this.cmbBrandFilter.Location = new System.Drawing.Point(99, 47);
            this.cmbBrandFilter.Name = "cmbBrandFilter";
            this.cmbBrandFilter.Size = new System.Drawing.Size(121, 21);
            this.cmbBrandFilter.TabIndex = 2;
            this.cmbBrandFilter.SelectedIndexChanged += new System.EventHandler(this.CmbBrandFilter_SelectedIndexChanged);
            // 
            // lblFilterType
            // 
            this.lblFilterType.AutoSize = true;
            this.lblFilterType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilterType.Location = new System.Drawing.Point(226, 49);
            this.lblFilterType.Name = "lblFilterType";
            this.lblFilterType.Size = new System.Drawing.Size(88, 17);
            this.lblFilterType.TabIndex = 3;
            this.lblFilterType.Text = "Filter by Type:";
            // 
            // cmbTypeFilter
            // 
            this.cmbTypeFilter.AccessibleName = "cmbTypeFilter";
            this.cmbTypeFilter.FormattingEnabled = true;
            this.cmbTypeFilter.Location = new System.Drawing.Point(320, 47);
            this.cmbTypeFilter.Name = "cmbTypeFilter";
            this.cmbTypeFilter.Size = new System.Drawing.Size(121, 21);
            this.cmbTypeFilter.TabIndex = 4;
            this.cmbTypeFilter.SelectedIndexChanged += new System.EventHandler(this.CmbTypeFilter_SelectedIndexChanged);
            // 
            // btnViewHistory
            // 
            this.btnViewHistory.Location = new System.Drawing.Point(774, 43);
            this.btnViewHistory.Name = "btnViewHistory";
            this.btnViewHistory.Size = new System.Drawing.Size(115, 23);
            this.btnViewHistory.TabIndex = 6;
            this.btnViewHistory.Text = "View History";
            this.btnViewHistory.UseVisualStyleBackColor = true;
            this.btnViewHistory.Click += new System.EventHandler(this.BtnViewHistory_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 839);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1688, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // imageListActions
            // 
            this.imageListActions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListActions.ImageStream")));
            this.imageListActions.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListActions.Images.SetKeyName(0, "trash-2 (1).png");
            this.imageListActions.Images.SetKeyName(1, "plus-circle (1).png");
            this.imageListActions.Images.SetKeyName(2, "x-circle.png");
            this.imageListActions.Images.SetKeyName(3, "eye.png");
            this.imageListActions.Images.SetKeyName(4, "printer.png");
            this.imageListActions.Images.SetKeyName(5, "users.png");
            this.imageListActions.Images.SetKeyName(6, "arrow-left-circle.png");
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnManageUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageUsers.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageUsers.Location = new System.Drawing.Point(895, 49);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(105, 30);
            this.btnManageUsers.TabIndex = 10;
            this.btnManageUsers.Text = "Manage Users";
            this.btnManageUsers.UseVisualStyleBackColor = false;
            this.btnManageUsers.Visible = false;
            this.btnManageUsers.Click += new System.EventHandler(this.BtnManageUsers_Click);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.Location = new System.Drawing.Point(1015, 55);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(50, 17);
            this.lblSearch.TabIndex = 11;
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(1071, 55);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 20);
            this.txtSearch.TabIndex = 12;
            this.toolTip1.SetToolTip(this.txtSearch, "Filter the grid by typing a product\'s Name, Brand, or Part Number.");
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // pnlheader
            // 
            this.pnlheader.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlheader.Controls.Add(this.btnBulkEdit);
            this.pnlheader.Controls.Add(this.btnBackToDashboard);
            this.pnlheader.Controls.Add(this.lblFilterBrand);
            this.pnlheader.Controls.Add(this.cmbBrandFilter);
            this.pnlheader.Controls.Add(this.lblFilterType);
            this.pnlheader.Controls.Add(this.cmbTypeFilter);
            this.pnlheader.Controls.Add(this.btnDeleteProduct);
            this.pnlheader.Controls.Add(this.btnManageUsers);
            this.pnlheader.Controls.Add(this.lblSearch);
            this.pnlheader.Controls.Add(this.txtSearch);
            this.pnlheader.Controls.Add(this.btnClearFilters);
            this.pnlheader.Controls.Add(this.btnPrintBarcode);
            this.pnlheader.Controls.Add(this.btnAddProduct);
            this.pnlheader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlheader.Location = new System.Drawing.Point(0, 0);
            this.pnlheader.Name = "pnlheader";
            this.pnlheader.Size = new System.Drawing.Size(1688, 100);
            this.pnlheader.TabIndex = 16;
            // 
            // btnBulkEdit
            // 
            this.btnBulkEdit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnBulkEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBulkEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBulkEdit.Location = new System.Drawing.Point(512, 13);
            this.btnBulkEdit.Name = "btnBulkEdit";
            this.btnBulkEdit.Size = new System.Drawing.Size(75, 29);
            this.btnBulkEdit.TabIndex = 16;
            this.btnBulkEdit.Text = "Bulk Edit";
            this.btnBulkEdit.UseVisualStyleBackColor = false;
            this.btnBulkEdit.Click += new System.EventHandler(this.BtnBulkEdit_Click);
            // 
            // btnBackToDashboard
            // 
            this.btnBackToDashboard.FlatAppearance.BorderSize = 0;
            this.btnBackToDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackToDashboard.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackToDashboard.ForeColor = System.Drawing.Color.White;
            this.btnBackToDashboard.ImageIndex = 6;
            this.btnBackToDashboard.ImageList = this.imageListActions;
            this.btnBackToDashboard.Location = new System.Drawing.Point(13, 4);
            this.btnBackToDashboard.Name = "btnBackToDashboard";
            this.btnBackToDashboard.Size = new System.Drawing.Size(164, 37);
            this.btnBackToDashboard.TabIndex = 14;
            this.btnBackToDashboard.Text = "Back to Dashboard";
            this.btnBackToDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBackToDashboard.UseVisualStyleBackColor = true;
            this.btnBackToDashboard.Visible = false;
            this.btnBackToDashboard.Click += new System.EventHandler(this.BtnBackToDashboard_Click);
            // 
            // btnDeleteProduct
            // 
            this.btnDeleteProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDeleteProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteProduct.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteProduct.ImageIndex = 0;
            this.btnDeleteProduct.ImageList = this.imageListActions;
            this.btnDeleteProduct.Location = new System.Drawing.Point(666, 48);
            this.btnDeleteProduct.Name = "btnDeleteProduct";
            this.btnDeleteProduct.Size = new System.Drawing.Size(134, 46);
            this.btnDeleteProduct.TabIndex = 9;
            this.btnDeleteProduct.Text = "Delete Selected";
            this.btnDeleteProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnDeleteProduct, "Deletes the currently selected product from the database.");
            this.btnDeleteProduct.UseVisualStyleBackColor = false;
            this.btnDeleteProduct.Click += new System.EventHandler(this.BtnDeleteProduct_Click);
            this.btnDeleteProduct.MouseEnter += new System.EventHandler(this.BtnAddProduct_MouseEnter);
            this.btnDeleteProduct.MouseLeave += new System.EventHandler(this.BtnAddProduct_MouseLeave);
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClearFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFilters.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearFilters.ImageIndex = 2;
            this.btnClearFilters.ImageList = this.imageListActions;
            this.btnClearFilters.Location = new System.Drawing.Point(1177, 50);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(115, 44);
            this.btnClearFilters.TabIndex = 13;
            this.btnClearFilters.Text = "Clear Filters";
            this.btnClearFilters.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnClearFilters, "Clears all active filters and shows all products.");
            this.btnClearFilters.UseVisualStyleBackColor = false;
            this.btnClearFilters.Click += new System.EventHandler(this.BtnClearFilters_Click);
            this.btnClearFilters.MouseEnter += new System.EventHandler(this.BtnAddProduct_MouseEnter);
            this.btnClearFilters.MouseLeave += new System.EventHandler(this.BtnAddProduct_MouseLeave);
            // 
            // btnPrintBarcode
            // 
            this.btnPrintBarcode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPrintBarcode.Enabled = false;
            this.btnPrintBarcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintBarcode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintBarcode.ImageIndex = 4;
            this.btnPrintBarcode.ImageList = this.imageListActions;
            this.btnPrintBarcode.Location = new System.Drawing.Point(1554, 20);
            this.btnPrintBarcode.Name = "btnPrintBarcode";
            this.btnPrintBarcode.Size = new System.Drawing.Size(122, 45);
            this.btnPrintBarcode.TabIndex = 15;
            this.btnPrintBarcode.Text = "Print Barcode";
            this.btnPrintBarcode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnPrintBarcode, "Generates and prints a barcode label for the selected product.");
            this.btnPrintBarcode.UseVisualStyleBackColor = false;
            this.btnPrintBarcode.Click += new System.EventHandler(this.btnPrintBarcode_Click);
            this.btnPrintBarcode.MouseEnter += new System.EventHandler(this.BtnAddProduct_MouseEnter);
            this.btnPrintBarcode.MouseLeave += new System.EventHandler(this.BtnAddProduct_MouseLeave);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProduct.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddProduct.ImageIndex = 1;
            this.btnAddProduct.ImageList = this.imageListActions;
            this.btnAddProduct.Location = new System.Drawing.Point(512, 48);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(148, 46);
            this.btnAddProduct.TabIndex = 8;
            this.btnAddProduct.Text = "Add New Product";
            this.btnAddProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnAddProduct, "(Ctrl+N) Opens the form to add a new product.");
            this.btnAddProduct.UseVisualStyleBackColor = false;
            this.btnAddProduct.Click += new System.EventHandler(this.BtnAddProduct_Click);
            this.btnAddProduct.MouseEnter += new System.EventHandler(this.BtnAddProduct_MouseEnter);
            this.btnAddProduct.MouseLeave += new System.EventHandler(this.BtnAddProduct_MouseLeave);
            // 
            // pnlCurrentTransaction
            // 
            this.pnlCurrentTransaction.Controls.Add(this.lblDate);
            this.pnlCurrentTransaction.Controls.Add(this.btnEditSaleItem);
            this.pnlCurrentTransaction.Controls.Add(this.btnCancelTransaction);
            this.pnlCurrentTransaction.Controls.Add(this.btnConfirmTransaction);
            this.pnlCurrentTransaction.Controls.Add(this.lblSubtotal);
            this.pnlCurrentTransaction.Controls.Add(this.dgvCurrentSale);
            this.pnlCurrentTransaction.Controls.Add(this.lblContextName);
            this.pnlCurrentTransaction.Location = new System.Drawing.Point(1219, 106);
            this.pnlCurrentTransaction.Name = "pnlCurrentTransaction";
            this.pnlCurrentTransaction.Size = new System.Drawing.Size(457, 429);
            this.pnlCurrentTransaction.TabIndex = 17;
            this.pnlCurrentTransaction.Visible = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(12, 52);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 17);
            this.lblDate.TabIndex = 9;
            // 
            // btnEditSaleItem
            // 
            this.btnEditSaleItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditSaleItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditSaleItem.Location = new System.Drawing.Point(13, 357);
            this.btnEditSaleItem.Name = "btnEditSaleItem";
            this.btnEditSaleItem.Size = new System.Drawing.Size(111, 23);
            this.btnEditSaleItem.TabIndex = 7;
            this.btnEditSaleItem.Text = "Edit";
            this.btnEditSaleItem.UseVisualStyleBackColor = true;
            this.btnEditSaleItem.Click += new System.EventHandler(this.BtnEditSaleItem_Click);
            // 
            // btnCancelTransaction
            // 
            this.btnCancelTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTransaction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelTransaction.Location = new System.Drawing.Point(349, 386);
            this.btnCancelTransaction.Name = "btnCancelTransaction";
            this.btnCancelTransaction.Size = new System.Drawing.Size(90, 23);
            this.btnCancelTransaction.TabIndex = 6;
            this.btnCancelTransaction.Text = "Cancel";
            this.btnCancelTransaction.UseVisualStyleBackColor = true;
            this.btnCancelTransaction.Click += new System.EventHandler(this.btnCancelTransaction_Click);
            // 
            // btnConfirmTransaction
            // 
            this.btnConfirmTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmTransaction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmTransaction.Location = new System.Drawing.Point(13, 386);
            this.btnConfirmTransaction.Name = "btnConfirmTransaction";
            this.btnConfirmTransaction.Size = new System.Drawing.Size(111, 23);
            this.btnConfirmTransaction.TabIndex = 5;
            this.btnConfirmTransaction.Text = "Confirm Delivery";
            this.btnConfirmTransaction.UseVisualStyleBackColor = true;
            this.btnConfirmTransaction.Click += new System.EventHandler(this.btnConfirmTransaction_Click);
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.Location = new System.Drawing.Point(324, 348);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(43, 17);
            this.lblSubtotal.TabIndex = 2;
            this.lblSubtotal.Text = "label1";
            // 
            // dgvCurrentSale
            // 
            this.dgvCurrentSale.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCurrentSale.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SaleItemDescription,
            this.SaleItemQuantity,
            this.SaleItemUnitPrice,
            this.SaleItemTotal,
            this.SaleItemProductID});
            this.dgvCurrentSale.Location = new System.Drawing.Point(9, 82);
            this.dgvCurrentSale.Name = "dgvCurrentSale";
            this.dgvCurrentSale.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCurrentSale.Size = new System.Drawing.Size(445, 263);
            this.dgvCurrentSale.TabIndex = 1;
            // 
            // SaleItemDescription
            // 
            this.SaleItemDescription.DataPropertyName = "Description";
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.SaleItemDescription.DefaultCellStyle = dataGridViewCellStyle29;
            this.SaleItemDescription.FillWeight = 200F;
            this.SaleItemDescription.HeaderText = "Description";
            this.SaleItemDescription.Name = "SaleItemDescription";
            // 
            // SaleItemQuantity
            // 
            this.SaleItemQuantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SaleItemQuantity.DefaultCellStyle = dataGridViewCellStyle30;
            this.SaleItemQuantity.FillWeight = 50F;
            this.SaleItemQuantity.HeaderText = "Quantity";
            this.SaleItemQuantity.Name = "SaleItemQuantity";
            // 
            // SaleItemUnitPrice
            // 
            this.SaleItemUnitPrice.DataPropertyName = "UnitPrice";
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle31.Format = "c";
            this.SaleItemUnitPrice.DefaultCellStyle = dataGridViewCellStyle31;
            this.SaleItemUnitPrice.FillWeight = 80F;
            this.SaleItemUnitPrice.HeaderText = "Unit Price";
            this.SaleItemUnitPrice.Name = "SaleItemUnitPrice";
            // 
            // SaleItemTotal
            // 
            this.SaleItemTotal.DataPropertyName = "Total";
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle32.Format = "c";
            this.SaleItemTotal.DefaultCellStyle = dataGridViewCellStyle32;
            this.SaleItemTotal.FillWeight = 90F;
            this.SaleItemTotal.HeaderText = "Total";
            this.SaleItemTotal.Name = "SaleItemTotal";
            // 
            // SaleItemProductID
            // 
            this.SaleItemProductID.DataPropertyName = "ProductID";
            this.SaleItemProductID.HeaderText = "Product ID\t";
            this.SaleItemProductID.Name = "SaleItemProductID";
            this.SaleItemProductID.Visible = false;
            // 
            // lblContextName
            // 
            this.lblContextName.AutoSize = true;
            this.lblContextName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContextName.Location = new System.Drawing.Point(12, 17);
            this.lblContextName.Name = "lblContextName";
            this.lblContextName.Size = new System.Drawing.Size(43, 17);
            this.lblContextName.TabIndex = 0;
            this.lblContextName.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1688, 861);
            this.Controls.Add(this.pnlCurrentTransaction);
            this.Controls.Add(this.dgvInventory);
            this.Controls.Add(this.pnlheader);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnViewHistory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1623, 900);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).EndInit();
            this.dgvInventoryContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.productBindingSource)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlheader.ResumeLayout(false);
            this.pnlheader.PerformLayout();
            this.pnlCurrentTransaction.ResumeLayout(false);
            this.pnlCurrentTransaction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentSale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInventory;
        private System.Windows.Forms.Label lblFilterBrand;
        private System.Windows.Forms.ComboBox cmbBrandFilter;
        private System.Windows.Forms.Label lblFilterType;
        private System.Windows.Forms.ComboBox cmbTypeFilter;
        private System.Windows.Forms.Button btnViewHistory;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnDeleteProduct;
        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnBackToDashboard;
        private System.Windows.Forms.Button btnPrintBarcode;
        private System.Windows.Forms.Panel pnlheader;
        private System.Windows.Forms.ImageList imageListActions;
        private System.Windows.Forms.BindingSource productBindingSource;
        private System.Windows.Forms.ContextMenuStrip dgvInventoryContextMenu;
        private System.Windows.Forms.ToolStripMenuItem editProductToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteProductToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printBarcodeToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnBulkEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn partNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn brandDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn volumeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lowStockThresholdDataGridViewTextBoxColumn;
        private System.Windows.Forms.Panel pnlCurrentTransaction;
        private System.Windows.Forms.DataGridView dgvCurrentSale;
        private System.Windows.Forms.Label lblContextName;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Button btnEditSaleItem;
        private System.Windows.Forms.Button btnCancelTransaction;
        private System.Windows.Forms.Button btnConfirmTransaction;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Volume;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Application;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchaseCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellingPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowStockThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn SaleItemDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn SaleItemQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn SaleItemUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn SaleItemTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn SaleItemProductID;
    }
}

