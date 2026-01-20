namespace InventorySystem
{
    partial class AddProductForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddProductForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.txtBrand = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.imageListActions = new System.Windows.Forms.ImageList(this.components);
            this.nudPurchaseCost = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numLowStockThreshold = new System.Windows.Forms.NumericUpDown();
            this.btnSaveProduct = new System.Windows.Forms.Button();
            this.pnlProductDetails = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.btnEditStockQuantity = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlStockAdjustment = new System.Windows.Forms.Panel();
            this.lblReasons = new System.Windows.Forms.Label();
            this.btnApplyStockChange = new System.Windows.Forms.Button();
            this.cmbAdjustmentReason = new System.Windows.Forms.ComboBox();
            this.txtReasonOther = new System.Windows.Forms.TextBox();
            this.numNewStock = new System.Windows.Forms.NumericUpDown();
            this.txtCurrentStock = new System.Windows.Forms.TextBox();
            this.lblReasonOther = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblCurrentStockQuantity = new System.Windows.Forms.Label();
            this.btnBackToDetails = new System.Windows.Forms.Button();
            this.lblEditStockQuantity = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nudSellingPrice = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudPurchaseCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowStockThreshold)).BeginInit();
            this.pnlProductDetails.SuspendLayout();
            this.pnlStockAdjustment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSellingPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Barcode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 138);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Brand";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 185);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Description";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 233);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Volume";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 280);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 330);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Purchase Cost";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(151, 81);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(116, 25);
            this.txtBarcode.TabIndex = 0;
            // 
            // txtBrand
            // 
            this.txtBrand.Location = new System.Drawing.Point(151, 135);
            this.txtBrand.Margin = new System.Windows.Forms.Padding(4);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(116, 25);
            this.txtBrand.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(151, 182);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(116, 25);
            this.txtDescription.TabIndex = 2;
            // 
            // txtVolume
            // 
            this.txtVolume.Location = new System.Drawing.Point(151, 230);
            this.txtVolume.Margin = new System.Windows.Forms.Padding(4);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(116, 25);
            this.txtVolume.TabIndex = 3;
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(151, 277);
            this.txtType.Margin = new System.Windows.Forms.Padding(4);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(116, 25);
            this.txtType.TabIndex = 4;
            // 
            // imageListActions
            // 
            this.imageListActions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListActions.ImageStream")));
            this.imageListActions.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListActions.Images.SetKeyName(0, "save.png");
            // 
            // nudPurchaseCost
            // 
            this.nudPurchaseCost.DecimalPlaces = 2;
            this.nudPurchaseCost.Location = new System.Drawing.Point(151, 328);
            this.nudPurchaseCost.Margin = new System.Windows.Forms.Padding(4);
            this.nudPurchaseCost.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudPurchaseCost.Name = "nudPurchaseCost";
            this.nudPurchaseCost.Size = new System.Drawing.Size(117, 25);
            this.nudPurchaseCost.TabIndex = 13;
            this.nudPurchaseCost.ThousandsSeparator = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 416);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Low Stock Threshold:";
            // 
            // numLowStockThreshold
            // 
            this.numLowStockThreshold.Location = new System.Drawing.Point(150, 408);
            this.numLowStockThreshold.Margin = new System.Windows.Forms.Padding(4);
            this.numLowStockThreshold.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numLowStockThreshold.Name = "numLowStockThreshold";
            this.numLowStockThreshold.Size = new System.Drawing.Size(118, 25);
            this.numLowStockThreshold.TabIndex = 15;
            this.numLowStockThreshold.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnSaveProduct
            // 
            this.btnSaveProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSaveProduct.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveProduct.ImageIndex = 0;
            this.btnSaveProduct.ImageList = this.imageListActions;
            this.btnSaveProduct.Location = new System.Drawing.Point(20, 504);
            this.btnSaveProduct.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveProduct.Name = "btnSaveProduct";
            this.btnSaveProduct.Size = new System.Drawing.Size(121, 33);
            this.btnSaveProduct.TabIndex = 5;
            this.btnSaveProduct.Text = "Save Product";
            this.btnSaveProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveProduct.UseVisualStyleBackColor = false;
            this.btnSaveProduct.Click += new System.EventHandler(this.BtnSaveProduct_Click);
            this.btnSaveProduct.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnSaveProduct.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // pnlProductDetails
            // 
            this.pnlProductDetails.Controls.Add(this.nudSellingPrice);
            this.pnlProductDetails.Controls.Add(this.label9);
            this.pnlProductDetails.Controls.Add(this.label8);
            this.pnlProductDetails.Controls.Add(this.btnEditStockQuantity);
            this.pnlProductDetails.Controls.Add(this.btnCancel);
            this.pnlProductDetails.Controls.Add(this.nudPurchaseCost);
            this.pnlProductDetails.Controls.Add(this.btnSaveProduct);
            this.pnlProductDetails.Controls.Add(this.numLowStockThreshold);
            this.pnlProductDetails.Controls.Add(this.label1);
            this.pnlProductDetails.Controls.Add(this.label7);
            this.pnlProductDetails.Controls.Add(this.label2);
            this.pnlProductDetails.Controls.Add(this.label3);
            this.pnlProductDetails.Controls.Add(this.label4);
            this.pnlProductDetails.Controls.Add(this.txtType);
            this.pnlProductDetails.Controls.Add(this.label5);
            this.pnlProductDetails.Controls.Add(this.txtVolume);
            this.pnlProductDetails.Controls.Add(this.label6);
            this.pnlProductDetails.Controls.Add(this.txtDescription);
            this.pnlProductDetails.Controls.Add(this.txtBarcode);
            this.pnlProductDetails.Controls.Add(this.txtBrand);
            this.pnlProductDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProductDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlProductDetails.Name = "pnlProductDetails";
            this.pnlProductDetails.Size = new System.Drawing.Size(592, 575);
            this.pnlProductDetails.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Emoji", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(14, 15);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(165, 20);
            this.label8.TabIndex = 18;
            this.label8.Text = " Edit Product Details";
            // 
            // btnEditStockQuantity
            // 
            this.btnEditStockQuantity.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnEditStockQuantity.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnEditStockQuantity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditStockQuantity.Image = global::InventorySystem.Properties.Resources.edit_2__1_;
            this.btnEditStockQuantity.Location = new System.Drawing.Point(69, 448);
            this.btnEditStockQuantity.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditStockQuantity.Name = "btnEditStockQuantity";
            this.btnEditStockQuantity.Size = new System.Drawing.Size(158, 33);
            this.btnEditStockQuantity.TabIndex = 17;
            this.btnEditStockQuantity.Text = "Edit Stock Quantity";
            this.btnEditStockQuantity.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditStockQuantity.UseVisualStyleBackColor = false;
            this.btnEditStockQuantity.Click += new System.EventHandler(this.BtnEditStockQuantity_Click);
            this.btnEditStockQuantity.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnEditStockQuantity.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(148, 504);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(121, 33);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnCancel.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // pnlStockAdjustment
            // 
            this.pnlStockAdjustment.Controls.Add(this.lblReasons);
            this.pnlStockAdjustment.Controls.Add(this.btnApplyStockChange);
            this.pnlStockAdjustment.Controls.Add(this.cmbAdjustmentReason);
            this.pnlStockAdjustment.Controls.Add(this.txtReasonOther);
            this.pnlStockAdjustment.Controls.Add(this.numNewStock);
            this.pnlStockAdjustment.Controls.Add(this.txtCurrentStock);
            this.pnlStockAdjustment.Controls.Add(this.lblReasonOther);
            this.pnlStockAdjustment.Controls.Add(this.label10);
            this.pnlStockAdjustment.Controls.Add(this.lblCurrentStockQuantity);
            this.pnlStockAdjustment.Controls.Add(this.btnBackToDetails);
            this.pnlStockAdjustment.Controls.Add(this.lblEditStockQuantity);
            this.pnlStockAdjustment.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlStockAdjustment.Location = new System.Drawing.Point(332, 0);
            this.pnlStockAdjustment.Name = "pnlStockAdjustment";
            this.pnlStockAdjustment.Size = new System.Drawing.Size(260, 575);
            this.pnlStockAdjustment.TabIndex = 17;
            this.pnlStockAdjustment.Visible = false;
            // 
            // lblReasons
            // 
            this.lblReasons.AutoSize = true;
            this.lblReasons.Location = new System.Drawing.Point(24, 167);
            this.lblReasons.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReasons.Name = "lblReasons";
            this.lblReasons.Size = new System.Drawing.Size(54, 17);
            this.lblReasons.TabIndex = 15;
            this.lblReasons.Text = "Reason:";
            // 
            // btnApplyStockChange
            // 
            this.btnApplyStockChange.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnApplyStockChange.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnApplyStockChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyStockChange.ImageIndex = 0;
            this.btnApplyStockChange.ImageList = this.imageListActions;
            this.btnApplyStockChange.Location = new System.Drawing.Point(27, 504);
            this.btnApplyStockChange.Margin = new System.Windows.Forms.Padding(4);
            this.btnApplyStockChange.Name = "btnApplyStockChange";
            this.btnApplyStockChange.Size = new System.Drawing.Size(144, 33);
            this.btnApplyStockChange.TabIndex = 14;
            this.btnApplyStockChange.Text = "Apply Changes";
            this.btnApplyStockChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnApplyStockChange.UseVisualStyleBackColor = false;
            this.btnApplyStockChange.Click += new System.EventHandler(this.BtnApplyStockChange_Click);
            this.btnApplyStockChange.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnApplyStockChange.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // cmbAdjustmentReason
            // 
            this.cmbAdjustmentReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAdjustmentReason.FormattingEnabled = true;
            this.cmbAdjustmentReason.Location = new System.Drawing.Point(85, 164);
            this.cmbAdjustmentReason.Name = "cmbAdjustmentReason";
            this.cmbAdjustmentReason.Size = new System.Drawing.Size(163, 25);
            this.cmbAdjustmentReason.TabIndex = 13;
            this.cmbAdjustmentReason.SelectedIndexChanged += new System.EventHandler(this.CmbAdjustmentReason_SelectedIndexChanged);
            // 
            // txtReasonOther
            // 
            this.txtReasonOther.Location = new System.Drawing.Point(27, 233);
            this.txtReasonOther.Multiline = true;
            this.txtReasonOther.Name = "txtReasonOther";
            this.txtReasonOther.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReasonOther.Size = new System.Drawing.Size(221, 64);
            this.txtReasonOther.TabIndex = 12;
            this.txtReasonOther.Visible = false;
            // 
            // numNewStock
            // 
            this.numNewStock.Location = new System.Drawing.Point(173, 115);
            this.numNewStock.Name = "numNewStock";
            this.numNewStock.Size = new System.Drawing.Size(75, 25);
            this.numNewStock.TabIndex = 11;
            // 
            // txtCurrentStock
            // 
            this.txtCurrentStock.Location = new System.Drawing.Point(173, 84);
            this.txtCurrentStock.Name = "txtCurrentStock";
            this.txtCurrentStock.ReadOnly = true;
            this.txtCurrentStock.Size = new System.Drawing.Size(75, 25);
            this.txtCurrentStock.TabIndex = 7;
            // 
            // lblReasonOther
            // 
            this.lblReasonOther.AutoSize = true;
            this.lblReasonOther.Location = new System.Drawing.Point(24, 212);
            this.lblReasonOther.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReasonOther.Name = "lblReasonOther";
            this.lblReasonOther.Size = new System.Drawing.Size(50, 17);
            this.lblReasonOther.TabIndex = 6;
            this.lblReasonOther.Text = "Others:";
            this.lblReasonOther.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 122);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 17);
            this.label10.TabIndex = 4;
            this.label10.Text = "New Stock Quantity:";
            // 
            // lblCurrentStockQuantity
            // 
            this.lblCurrentStockQuantity.AutoSize = true;
            this.lblCurrentStockQuantity.Location = new System.Drawing.Point(24, 84);
            this.lblCurrentStockQuantity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentStockQuantity.Name = "lblCurrentStockQuantity";
            this.lblCurrentStockQuantity.Size = new System.Drawing.Size(141, 17);
            this.lblCurrentStockQuantity.TabIndex = 3;
            this.lblCurrentStockQuantity.Text = "Current Stock Quantity:";
            // 
            // btnBackToDetails
            // 
            this.btnBackToDetails.Location = new System.Drawing.Point(4, 4);
            this.btnBackToDetails.Name = "btnBackToDetails";
            this.btnBackToDetails.Size = new System.Drawing.Size(53, 23);
            this.btnBackToDetails.TabIndex = 2;
            this.btnBackToDetails.Text = "<-";
            this.btnBackToDetails.UseVisualStyleBackColor = true;
            this.btnBackToDetails.Click += new System.EventHandler(this.BtnBackToDetails_Click);
            // 
            // lblEditStockQuantity
            // 
            this.lblEditStockQuantity.AutoSize = true;
            this.lblEditStockQuantity.Font = new System.Drawing.Font("Segoe UI Emoji", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEditStockQuantity.Location = new System.Drawing.Point(64, 15);
            this.lblEditStockQuantity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEditStockQuantity.Name = "lblEditStockQuantity";
            this.lblEditStockQuantity.Size = new System.Drawing.Size(159, 20);
            this.lblEditStockQuantity.TabIndex = 1;
            this.lblEditStockQuantity.Text = " Edit Stock Quantity";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 368);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Selling Price";
            // 
            // nudSellingPrice
            // 
            this.nudSellingPrice.DecimalPlaces = 2;
            this.nudSellingPrice.Location = new System.Drawing.Point(147, 366);
            this.nudSellingPrice.Margin = new System.Windows.Forms.Padding(4);
            this.nudSellingPrice.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudSellingPrice.Name = "nudSellingPrice";
            this.nudSellingPrice.Size = new System.Drawing.Size(120, 25);
            this.nudSellingPrice.TabIndex = 20;
            // 
            // AddProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 575);
            this.Controls.Add(this.pnlStockAdjustment);
            this.Controls.Add(this.pnlProductDetails);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(298, 434);
            this.Name = "AddProductForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Product";
            this.Load += new System.EventHandler(this.AddProductForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPurchaseCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowStockThreshold)).EndInit();
            this.pnlProductDetails.ResumeLayout(false);
            this.pnlProductDetails.PerformLayout();
            this.pnlStockAdjustment.ResumeLayout(false);
            this.pnlStockAdjustment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSellingPrice)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Button btnSaveProduct;
        private System.Windows.Forms.NumericUpDown nudPurchaseCost;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numLowStockThreshold;
        private System.Windows.Forms.ImageList imageListActions;
        private System.Windows.Forms.Panel pnlProductDetails;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEditStockQuantity;
        private System.Windows.Forms.Panel pnlStockAdjustment;
        private System.Windows.Forms.Label lblEditStockQuantity;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnBackToDetails;
        private System.Windows.Forms.Label lblReasonOther;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblCurrentStockQuantity;
        private System.Windows.Forms.TextBox txtCurrentStock;
        private System.Windows.Forms.NumericUpDown numNewStock;
        private System.Windows.Forms.TextBox txtReasonOther;
        private System.Windows.Forms.ComboBox cmbAdjustmentReason;
        private System.Windows.Forms.Button btnApplyStockChange;
        private System.Windows.Forms.Label lblReasons;
        private System.Windows.Forms.NumericUpDown nudSellingPrice;
        private System.Windows.Forms.Label label9;
    }
}