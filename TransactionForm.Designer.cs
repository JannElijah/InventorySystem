namespace InventorySystem
{
    partial class TransactionForm
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
            this.lblEnterQuantity = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.btnSupply = new System.Windows.Forms.Button();
            this.btnDeliver = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCurrentStockData = new System.Windows.Forms.Label();
            this.lblBrandData = new System.Windows.Forms.Label();
            this.lblBarcodeData = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cmbSuppliers = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDeliverTo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEnterQuantity
            // 
            this.lblEnterQuantity.AutoSize = true;
            this.lblEnterQuantity.Location = new System.Drawing.Point(27, 200);
            this.lblEnterQuantity.Name = "lblEnterQuantity";
            this.lblEnterQuantity.Size = new System.Drawing.Size(77, 13);
            this.lblEnterQuantity.TabIndex = 4;
            this.lblEnterQuantity.Text = "Enter Quantity:";
            this.lblEnterQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(176, 193);
            this.numQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(79, 20);
            this.numQuantity.TabIndex = 5;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnSupply
            // 
            this.btnSupply.Enabled = false;
            this.btnSupply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSupply.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSupply.Location = new System.Drawing.Point(31, 404);
            this.btnSupply.Name = "btnSupply";
            this.btnSupply.Size = new System.Drawing.Size(75, 30);
            this.btnSupply.TabIndex = 6;
            this.btnSupply.Text = "Supply";
            this.btnSupply.UseVisualStyleBackColor = true;
            this.btnSupply.Click += new System.EventHandler(this.BtnSupply_Click);
            // 
            // btnDeliver
            // 
            this.btnDeliver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeliver.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeliver.Location = new System.Drawing.Point(184, 404);
            this.btnDeliver.Name = "btnDeliver";
            this.btnDeliver.Size = new System.Drawing.Size(75, 30);
            this.btnDeliver.TabIndex = 7;
            this.btnDeliver.Text = "Deliver";
            this.btnDeliver.UseVisualStyleBackColor = true;
            this.btnDeliver.Click += new System.EventHandler(this.BtnDeliver_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(110, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(71, 20);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(168, 23);
            this.lblDescription.TabIndex = 9;
            this.lblDescription.Text = "Product Description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Barcode:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Brand:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Current Stock:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentStockData
            // 
            this.lblCurrentStockData.AutoSize = true;
            this.lblCurrentStockData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentStockData.Location = new System.Drawing.Point(175, 164);
            this.lblCurrentStockData.Name = "lblCurrentStockData";
            this.lblCurrentStockData.Size = new System.Drawing.Size(80, 13);
            this.lblCurrentStockData.TabIndex = 15;
            this.lblCurrentStockData.Text = "Current Stock:";
            this.lblCurrentStockData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrandData
            // 
            this.lblBrandData.AutoSize = true;
            this.lblBrandData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrandData.Location = new System.Drawing.Point(175, 128);
            this.lblBrandData.Name = "lblBrandData";
            this.lblBrandData.Size = new System.Drawing.Size(41, 13);
            this.lblBrandData.TabIndex = 14;
            this.lblBrandData.Text = "Brand:";
            this.lblBrandData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBarcodeData
            // 
            this.lblBarcodeData.AutoSize = true;
            this.lblBarcodeData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBarcodeData.Location = new System.Drawing.Point(175, 89);
            this.lblBarcodeData.Name = "lblBarcodeData";
            this.lblBarcodeData.Size = new System.Drawing.Size(52, 13);
            this.lblBarcodeData.TabIndex = 13;
            this.lblBarcodeData.Text = "Barcode:";
            this.lblBarcodeData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(29, 235);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(48, 13);
            this.lblSupplier.TabIndex = 16;
            this.lblSupplier.Text = "Supplier:";
            // 
            // cmbSuppliers
            // 
            this.cmbSuppliers.FormattingEnabled = true;
            this.cmbSuppliers.Location = new System.Drawing.Point(176, 227);
            this.cmbSuppliers.Name = "cmbSuppliers";
            this.cmbSuppliers.Size = new System.Drawing.Size(79, 21);
            this.cmbSuppliers.TabIndex = 17;
            this.cmbSuppliers.SelectedIndexChanged += new System.EventHandler(this.CmbSuppliers_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 267);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Deliver to";
            // 
            // txtDeliverTo
            // 
            this.txtDeliverTo.Location = new System.Drawing.Point(176, 259);
            this.txtDeliverTo.Name = "txtDeliverTo";
            this.txtDeliverTo.Size = new System.Drawing.Size(100, 20);
            this.txtDeliverTo.TabIndex = 19;
            // 
            // TransactionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 508);
            this.Controls.Add(this.txtDeliverTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbSuppliers);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.lblCurrentStockData);
            this.Controls.Add(this.lblBrandData);
            this.Controls.Add(this.lblBarcodeData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDeliver);
            this.Controls.Add(this.btnSupply);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.lblEnterQuantity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransactionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Form";
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblEnterQuantity;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Button btnSupply;
        private System.Windows.Forms.Button btnDeliver;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCurrentStockData;
        private System.Windows.Forms.Label lblBrandData;
        private System.Windows.Forms.Label lblBarcodeData;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox cmbSuppliers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDeliverTo;
    }
}