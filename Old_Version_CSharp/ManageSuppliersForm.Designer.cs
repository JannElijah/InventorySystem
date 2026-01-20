namespace InventorySystem
{
    partial class ManageSuppliersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageSuppliersForm));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnBackToDashboard = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnDeleteSupplier = new System.Windows.Forms.Button();
            this.btnEditSupplier = new System.Windows.Forms.Button();
            this.btnAddSupplier = new System.Windows.Forms.Button();
            this.dgvSuppliers = new System.Windows.Forms.DataGridView();
            this.imageListActions = new System.Windows.Forms.ImageList(this.components);
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlHeader.Controls.Add(this.btnBackToDashboard);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(684, 50);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnBackToDashboard
            // 
            this.btnBackToDashboard.FlatAppearance.BorderSize = 0;
            this.btnBackToDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackToDashboard.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackToDashboard.ForeColor = System.Drawing.Color.White;
            this.btnBackToDashboard.ImageIndex = 2;
            this.btnBackToDashboard.ImageList = this.imageListActions;
            this.btnBackToDashboard.Location = new System.Drawing.Point(10, 12);
            this.btnBackToDashboard.Name = "btnBackToDashboard";
            this.btnBackToDashboard.Size = new System.Drawing.Size(189, 32);
            this.btnBackToDashboard.TabIndex = 0;
            this.btnBackToDashboard.Text = "Back to Dashboard";
            this.btnBackToDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBackToDashboard.UseVisualStyleBackColor = true;
            this.btnBackToDashboard.Click += new System.EventHandler(this.btnBackToDashboard_Click);
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlFooter.Controls.Add(this.btnDeleteSupplier);
            this.pnlFooter.Controls.Add(this.btnEditSupplier);
            this.pnlFooter.Controls.Add(this.btnAddSupplier);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 401);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(684, 60);
            this.pnlFooter.TabIndex = 1;
            // 
            // btnDeleteSupplier
            // 
            this.btnDeleteSupplier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteSupplier.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDeleteSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteSupplier.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSupplier.ImageIndex = 7;
            this.btnDeleteSupplier.ImageList = this.imageListActions;
            this.btnDeleteSupplier.Location = new System.Drawing.Point(312, 9);
            this.btnDeleteSupplier.Name = "btnDeleteSupplier";
            this.btnDeleteSupplier.Size = new System.Drawing.Size(140, 39);
            this.btnDeleteSupplier.TabIndex = 2;
            this.btnDeleteSupplier.Text = " Delete Supplier";
            this.btnDeleteSupplier.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteSupplier.UseVisualStyleBackColor = false;
            this.btnDeleteSupplier.Click += new System.EventHandler(this.btnDeleteSupplier_Click);
            this.btnDeleteSupplier.MouseEnter += new System.EventHandler(this.btnDeleteSupplier_MouseEnter);
            this.btnDeleteSupplier.MouseLeave += new System.EventHandler(this.btnDeleteSupplier_MouseLeave);
            // 
            // btnEditSupplier
            // 
            this.btnEditSupplier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditSupplier.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnEditSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditSupplier.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditSupplier.ImageIndex = 6;
            this.btnEditSupplier.ImageList = this.imageListActions;
            this.btnEditSupplier.Location = new System.Drawing.Point(150, 9);
            this.btnEditSupplier.Name = "btnEditSupplier";
            this.btnEditSupplier.Size = new System.Drawing.Size(139, 39);
            this.btnEditSupplier.TabIndex = 1;
            this.btnEditSupplier.Text = "Edit Supplier";
            this.btnEditSupplier.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditSupplier.UseVisualStyleBackColor = false;
            this.btnEditSupplier.Click += new System.EventHandler(this.btnEditSupplier_Click);
            this.btnEditSupplier.MouseEnter += new System.EventHandler(this.btnEditSupplier_MouseEnter);
            this.btnEditSupplier.MouseLeave += new System.EventHandler(this.btnEditSupplier_MouseLeave);
            // 
            // btnAddSupplier
            // 
            this.btnAddSupplier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddSupplier.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSupplier.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSupplier.ImageIndex = 5;
            this.btnAddSupplier.ImageList = this.imageListActions;
            this.btnAddSupplier.Location = new System.Drawing.Point(8, 9);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(123, 39);
            this.btnAddSupplier.TabIndex = 0;
            this.btnAddSupplier.Text = "Add Supplier";
            this.btnAddSupplier.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddSupplier.UseVisualStyleBackColor = false;
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);
            this.btnAddSupplier.MouseEnter += new System.EventHandler(this.btnAddSupplier_MouseEnter);
            this.btnAddSupplier.MouseLeave += new System.EventHandler(this.btnAddSupplier_MouseLeave);
            // 
            // dgvSuppliers
            // 
            this.dgvSuppliers.AllowUserToAddRows = false;
            this.dgvSuppliers.AllowUserToDeleteRows = false;
            this.dgvSuppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSuppliers.BackgroundColor = System.Drawing.Color.White;
            this.dgvSuppliers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSuppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSuppliers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSuppliers.Location = new System.Drawing.Point(0, 50);
            this.dgvSuppliers.MultiSelect = false;
            this.dgvSuppliers.Name = "dgvSuppliers";
            this.dgvSuppliers.ReadOnly = true;
            this.dgvSuppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuppliers.Size = new System.Drawing.Size(684, 351);
            this.dgvSuppliers.TabIndex = 2;
            this.dgvSuppliers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSuppliers_CellDoubleClick);
            // 
            // imageListActions
            // 
            this.imageListActions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListActions.ImageStream")));
            this.imageListActions.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListActions.Images.SetKeyName(0, "save.png");
            this.imageListActions.Images.SetKeyName(1, "x-circle.png");
            this.imageListActions.Images.SetKeyName(2, "arrow-left-circle.png");
            this.imageListActions.Images.SetKeyName(3, "eye.png");
            this.imageListActions.Images.SetKeyName(4, "printer.png");
            this.imageListActions.Images.SetKeyName(5, "plus-circle (1).png");
            this.imageListActions.Images.SetKeyName(6, "edit-2 (1).png");
            this.imageListActions.Images.SetKeyName(7, "trash-2 (1).png");
            // 
            // ManageSuppliersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.dgvSuppliers);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "ManageSuppliersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Suppliers";
            this.Load += new System.EventHandler(this.ManageSuppliersForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnBackToDashboard;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnDeleteSupplier;
        private System.Windows.Forms.Button btnEditSupplier;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.DataGridView dgvSuppliers;
        private System.Windows.Forms.ImageList imageListActions;
    }
}