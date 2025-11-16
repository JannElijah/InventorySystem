namespace InventorySystem
{
    partial class MultiTransactionForm
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
            this.pnlSourceDestination = new System.Windows.Forms.Panel();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.cmbSuppliers = new System.Windows.Forms.ComboBox();
            this.pnlCurrentTransaction = new System.Windows.Forms.Panel();
            this.pnlTransactionItemsModeSelection = new System.Windows.Forms.Panel();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.btnProcessTransaction = new System.Windows.Forms.Button();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.dgvTransactionItems = new System.Windows.Forms.DataGridView();
            this.pnlProductSearch = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.lstSearchResults = new System.Windows.Forms.ListBox();
            this.txtSearchProduct = new System.Windows.Forms.TextBox();
            this.pnlModeSelection = new System.Windows.Forms.Panel();
            this.rbDelivery = new System.Windows.Forms.RadioButton();
            this.rbSupply = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSourceDestination.SuspendLayout();
            this.pnlCurrentTransaction.SuspendLayout();
            this.pnlTransactionItemsModeSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionItems)).BeginInit();
            this.pnlProductSearch.SuspendLayout();
            this.pnlModeSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSourceDestination
            // 
            this.pnlSourceDestination.Controls.Add(this.lblCustomerName);
            this.pnlSourceDestination.Controls.Add(this.lblSupplier);
            this.pnlSourceDestination.Controls.Add(this.txtCustomerName);
            this.pnlSourceDestination.Controls.Add(this.cmbSuppliers);
            this.pnlSourceDestination.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSourceDestination.Location = new System.Drawing.Point(344, 83);
            this.pnlSourceDestination.Name = "pnlSourceDestination";
            this.pnlSourceDestination.Size = new System.Drawing.Size(658, 67);
            this.pnlSourceDestination.TabIndex = 0;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(13, 7);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(106, 17);
            this.lblCustomerName.TabIndex = 3;
            this.lblCustomerName.Text = "Customer Name:";
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplier.Location = new System.Drawing.Point(13, 7);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(59, 17);
            this.lblSupplier.TabIndex = 2;
            this.lblSupplier.Text = "Supplier:";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerName.Location = new System.Drawing.Point(13, 29);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(121, 22);
            this.txtCustomerName.TabIndex = 1;
            // 
            // cmbSuppliers
            // 
            this.cmbSuppliers.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSuppliers.FormattingEnabled = true;
            this.cmbSuppliers.Location = new System.Drawing.Point(13, 29);
            this.cmbSuppliers.Name = "cmbSuppliers";
            this.cmbSuppliers.Size = new System.Drawing.Size(121, 21);
            this.cmbSuppliers.TabIndex = 0;
            // 
            // pnlCurrentTransaction
            // 
            this.pnlCurrentTransaction.Controls.Add(this.pnlTransactionItemsModeSelection);
            this.pnlCurrentTransaction.Controls.Add(this.dgvTransactionItems);
            this.pnlCurrentTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCurrentTransaction.Location = new System.Drawing.Point(344, 150);
            this.pnlCurrentTransaction.Name = "pnlCurrentTransaction";
            this.pnlCurrentTransaction.Size = new System.Drawing.Size(658, 314);
            this.pnlCurrentTransaction.TabIndex = 1;
            // 
            // pnlTransactionItemsModeSelection
            // 
            this.pnlTransactionItemsModeSelection.Controls.Add(this.btnRemoveItem);
            this.pnlTransactionItemsModeSelection.Controls.Add(this.btnProcessTransaction);
            this.pnlTransactionItemsModeSelection.Controls.Add(this.lblSubtotal);
            this.pnlTransactionItemsModeSelection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTransactionItemsModeSelection.Location = new System.Drawing.Point(0, 265);
            this.pnlTransactionItemsModeSelection.Name = "pnlTransactionItemsModeSelection";
            this.pnlTransactionItemsModeSelection.Size = new System.Drawing.Size(658, 49);
            this.pnlTransactionItemsModeSelection.TabIndex = 1;
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveItem.Location = new System.Drawing.Point(169, 7);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(168, 30);
            this.btnRemoveItem.TabIndex = 2;
            this.btnRemoveItem.Text = "Remove Selected Item";
            this.btnRemoveItem.UseVisualStyleBackColor = true;
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // btnProcessTransaction
            // 
            this.btnProcessTransaction.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessTransaction.Location = new System.Drawing.Point(13, 7);
            this.btnProcessTransaction.Name = "btnProcessTransaction";
            this.btnProcessTransaction.Size = new System.Drawing.Size(150, 30);
            this.btnProcessTransaction.TabIndex = 1;
            this.btnProcessTransaction.Text = "Process Transaction";
            this.btnProcessTransaction.UseVisualStyleBackColor = true;
            this.btnProcessTransaction.Click += new System.EventHandler(this.btnProcessTransaction_Click);
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.Location = new System.Drawing.Point(503, 14);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(100, 17);
            this.lblSubtotal.TabIndex = 0;
            this.lblSubtotal.Text = "Subtotal: $0.00";
            // 
            // dgvTransactionItems
            // 
            this.dgvTransactionItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactionItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransactionItems.Location = new System.Drawing.Point(0, 0);
            this.dgvTransactionItems.Name = "dgvTransactionItems";
            this.dgvTransactionItems.Size = new System.Drawing.Size(658, 314);
            this.dgvTransactionItems.TabIndex = 0;
            this.dgvTransactionItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransactionItems_CellValueChanged);
            this.dgvTransactionItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTransactionItems_KeyDown);
            // 
            // pnlProductSearch
            // 
            this.pnlProductSearch.Controls.Add(this.label2);
            this.pnlProductSearch.Controls.Add(this.btnAddItem);
            this.pnlProductSearch.Controls.Add(this.lstSearchResults);
            this.pnlProductSearch.Controls.Add(this.txtSearchProduct);
            this.pnlProductSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlProductSearch.Location = new System.Drawing.Point(0, 83);
            this.pnlProductSearch.MinimumSize = new System.Drawing.Size(300, 0);
            this.pnlProductSearch.Name = "pnlProductSearch";
            this.pnlProductSearch.Size = new System.Drawing.Size(344, 381);
            this.pnlProductSearch.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Search:";
            // 
            // btnAddItem
            // 
            this.btnAddItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddItem.Location = new System.Drawing.Point(15, 315);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(124, 30);
            this.btnAddItem.TabIndex = 2;
            this.btnAddItem.Text = "Add Selected Item";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // lstSearchResults
            // 
            this.lstSearchResults.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSearchResults.FormattingEnabled = true;
            this.lstSearchResults.ItemHeight = 17;
            this.lstSearchResults.Location = new System.Drawing.Point(3, 40);
            this.lstSearchResults.Name = "lstSearchResults";
            this.lstSearchResults.Size = new System.Drawing.Size(335, 259);
            this.lstSearchResults.TabIndex = 1;
            this.lstSearchResults.DoubleClick += new System.EventHandler(this.lstSearchResults_DoubleClick);
            this.lstSearchResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstSearchResults_KeyDown);
            // 
            // txtSearchProduct
            // 
            this.txtSearchProduct.Location = new System.Drawing.Point(65, 7);
            this.txtSearchProduct.Name = "txtSearchProduct";
            this.txtSearchProduct.Size = new System.Drawing.Size(229, 20);
            this.txtSearchProduct.TabIndex = 0;
            this.txtSearchProduct.TextChanged += new System.EventHandler(this.txtSearchProduct_TextChanged);
            // 
            // pnlModeSelection
            // 
            this.pnlModeSelection.Controls.Add(this.rbDelivery);
            this.pnlModeSelection.Controls.Add(this.rbSupply);
            this.pnlModeSelection.Controls.Add(this.label1);
            this.pnlModeSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlModeSelection.Location = new System.Drawing.Point(0, 0);
            this.pnlModeSelection.Name = "pnlModeSelection";
            this.pnlModeSelection.Size = new System.Drawing.Size(1002, 83);
            this.pnlModeSelection.TabIndex = 3;
            // 
            // rbDelivery
            // 
            this.rbDelivery.AutoSize = true;
            this.rbDelivery.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDelivery.Location = new System.Drawing.Point(34, 49);
            this.rbDelivery.Name = "rbDelivery";
            this.rbDelivery.Size = new System.Drawing.Size(132, 21);
            this.rbDelivery.TabIndex = 2;
            this.rbDelivery.TabStop = true;
            this.rbDelivery.Text = "Process a Delivery";
            this.rbDelivery.UseVisualStyleBackColor = true;
            this.rbDelivery.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // rbSupply
            // 
            this.rbSupply.AutoSize = true;
            this.rbSupply.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSupply.Location = new System.Drawing.Point(34, 25);
            this.rbSupply.Name = "rbSupply";
            this.rbSupply.Size = new System.Drawing.Size(125, 21);
            this.rbSupply.TabIndex = 1;
            this.rbSupply.TabStop = true;
            this.rbSupply.Text = "Process a Supply";
            this.rbSupply.UseVisualStyleBackColor = true;
            this.rbSupply.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Transaction Mode:";
            // 
            // MultiTransactionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 464);
            this.Controls.Add(this.pnlCurrentTransaction);
            this.Controls.Add(this.pnlSourceDestination);
            this.Controls.Add(this.pnlProductSearch);
            this.Controls.Add(this.pnlModeSelection);
            this.MinimumSize = new System.Drawing.Size(1018, 503);
            this.Name = "MultiTransactionForm";
            this.Text = "Multi Transaction Form";
            this.Load += new System.EventHandler(this.MultiTransactionForm_Load);
            this.pnlSourceDestination.ResumeLayout(false);
            this.pnlSourceDestination.PerformLayout();
            this.pnlCurrentTransaction.ResumeLayout(false);
            this.pnlTransactionItemsModeSelection.ResumeLayout(false);
            this.pnlTransactionItemsModeSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionItems)).EndInit();
            this.pnlProductSearch.ResumeLayout(false);
            this.pnlProductSearch.PerformLayout();
            this.pnlModeSelection.ResumeLayout(false);
            this.pnlModeSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSourceDestination;
        private System.Windows.Forms.Panel pnlCurrentTransaction;
        private System.Windows.Forms.Panel pnlProductSearch;
        private System.Windows.Forms.Panel pnlModeSelection;
        private System.Windows.Forms.ComboBox cmbSuppliers;
        private System.Windows.Forms.RadioButton rbDelivery;
        private System.Windows.Forms.RadioButton rbSupply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.ListBox lstSearchResults;
        private System.Windows.Forms.TextBox txtSearchProduct;
        private System.Windows.Forms.Panel pnlTransactionItemsModeSelection;
        private System.Windows.Forms.DataGridView dgvTransactionItems;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnProcessTransaction;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblSupplier;
    }
}