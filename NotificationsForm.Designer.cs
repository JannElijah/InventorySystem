namespace InventorySystem
{
    partial class NotificationsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationsForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvNotifications = new System.Windows.Forms.DataGridView();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnMarkAllAsRead = new System.Windows.Forms.Button();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBackToDashboard = new System.Windows.Forms.Button();
            this.imageListActions = new System.Windows.Forms.ImageList(this.components);
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnDeleteNotification = new System.Windows.Forms.Button();
            this.btnMarkAsRead = new System.Windows.Forms.Button();
            this.btnViewProduct = new System.Windows.Forms.Button();
            this.ProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotificationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsRead = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NotificationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotifications)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvNotifications
            // 
            this.dgvNotifications.AllowUserToAddRows = false;
            this.dgvNotifications.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNotifications.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvNotifications.BackgroundColor = System.Drawing.Color.White;
            this.dgvNotifications.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNotifications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotifications.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductID,
            this.Message,
            this.ProductName,
            this.NotificationType,
            this.Timestamp,
            this.IsRead,
            this.NotificationID});
            this.dgvNotifications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNotifications.Location = new System.Drawing.Point(0, 45);
            this.dgvNotifications.Name = "dgvNotifications";
            this.dgvNotifications.ReadOnly = true;
            this.dgvNotifications.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNotifications.Size = new System.Drawing.Size(907, 332);
            this.dgvNotifications.TabIndex = 0;
            this.dgvNotifications.SelectionChanged += new System.EventHandler(this.dgvNotifications_SelectionChanged);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlHeader.Controls.Add(this.btnMarkAllAsRead);
            this.pnlHeader.Controls.Add(this.cmbFilter);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.btnBackToDashboard);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(907, 45);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnMarkAllAsRead
            // 
            this.btnMarkAllAsRead.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMarkAllAsRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkAllAsRead.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMarkAllAsRead.Location = new System.Drawing.Point(775, 12);
            this.btnMarkAllAsRead.Name = "btnMarkAllAsRead";
            this.btnMarkAllAsRead.Size = new System.Drawing.Size(120, 27);
            this.btnMarkAllAsRead.TabIndex = 3;
            this.btnMarkAllAsRead.Text = "Mark All as Read";
            this.btnMarkAllAsRead.UseVisualStyleBackColor = false;
            this.btnMarkAllAsRead.Click += new System.EventHandler(this.btnMarkAllAsRead_Click);
            this.btnMarkAllAsRead.MouseEnter += new System.EventHandler(this.btnViewProduct_MouseEnter);
            this.btnMarkAllAsRead.MouseLeave += new System.EventHandler(this.btnViewProduct_MouseLeave);
            // 
            // cmbFilter
            // 
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            "All Notifications",
            "Unread Only"});
            this.cmbFilter.Location = new System.Drawing.Point(234, 18);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(121, 21);
            this.cmbFilter.TabIndex = 2;
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Show:";
            // 
            // btnBackToDashboard
            // 
            this.btnBackToDashboard.FlatAppearance.BorderSize = 0;
            this.btnBackToDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackToDashboard.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackToDashboard.ForeColor = System.Drawing.Color.White;
            this.btnBackToDashboard.ImageIndex = 3;
            this.btnBackToDashboard.ImageList = this.imageListActions;
            this.btnBackToDashboard.Location = new System.Drawing.Point(12, 4);
            this.btnBackToDashboard.Name = "btnBackToDashboard";
            this.btnBackToDashboard.Size = new System.Drawing.Size(172, 35);
            this.btnBackToDashboard.TabIndex = 0;
            this.btnBackToDashboard.Text = "Back to Dashboard";
            this.btnBackToDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBackToDashboard.UseVisualStyleBackColor = true;
            this.btnBackToDashboard.Click += new System.EventHandler(this.btnBackToDashboard_Click);
            // 
            // imageListActions
            // 
            this.imageListActions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListActions.ImageStream")));
            this.imageListActions.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListActions.Images.SetKeyName(0, "check-circle.png");
            this.imageListActions.Images.SetKeyName(1, "eye.png");
            this.imageListActions.Images.SetKeyName(2, "trash-2 (1).png");
            this.imageListActions.Images.SetKeyName(3, "arrow-left-circle.png");
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlFooter.Controls.Add(this.btnDeleteNotification);
            this.pnlFooter.Controls.Add(this.btnMarkAsRead);
            this.pnlFooter.Controls.Add(this.btnViewProduct);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 377);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(907, 73);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnDeleteNotification
            // 
            this.btnDeleteNotification.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDeleteNotification.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteNotification.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteNotification.ImageIndex = 2;
            this.btnDeleteNotification.ImageList = this.imageListActions;
            this.btnDeleteNotification.Location = new System.Drawing.Point(787, 10);
            this.btnDeleteNotification.Name = "btnDeleteNotification";
            this.btnDeleteNotification.Size = new System.Drawing.Size(108, 34);
            this.btnDeleteNotification.TabIndex = 2;
            this.btnDeleteNotification.Text = "Delete";
            this.btnDeleteNotification.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteNotification.UseVisualStyleBackColor = false;
            this.btnDeleteNotification.Click += new System.EventHandler(this.btnDeleteNotification_Click);
            this.btnDeleteNotification.MouseEnter += new System.EventHandler(this.btnViewProduct_MouseEnter);
            this.btnDeleteNotification.MouseLeave += new System.EventHandler(this.btnViewProduct_MouseLeave);
            // 
            // btnMarkAsRead
            // 
            this.btnMarkAsRead.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMarkAsRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkAsRead.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMarkAsRead.ImageIndex = 0;
            this.btnMarkAsRead.ImageList = this.imageListActions;
            this.btnMarkAsRead.Location = new System.Drawing.Point(145, 6);
            this.btnMarkAsRead.Name = "btnMarkAsRead";
            this.btnMarkAsRead.Size = new System.Drawing.Size(134, 38);
            this.btnMarkAsRead.TabIndex = 1;
            this.btnMarkAsRead.Text = "Mark as Read";
            this.btnMarkAsRead.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMarkAsRead.UseVisualStyleBackColor = false;
            this.btnMarkAsRead.Click += new System.EventHandler(this.btnMarkAsRead_Click);
            this.btnMarkAsRead.MouseEnter += new System.EventHandler(this.btnViewProduct_MouseEnter);
            this.btnMarkAsRead.MouseLeave += new System.EventHandler(this.btnViewProduct_MouseLeave);
            // 
            // btnViewProduct
            // 
            this.btnViewProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnViewProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewProduct.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewProduct.ImageIndex = 1;
            this.btnViewProduct.ImageList = this.imageListActions;
            this.btnViewProduct.Location = new System.Drawing.Point(12, 6);
            this.btnViewProduct.Name = "btnViewProduct";
            this.btnViewProduct.Size = new System.Drawing.Size(127, 38);
            this.btnViewProduct.TabIndex = 0;
            this.btnViewProduct.Text = "View Product";
            this.btnViewProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnViewProduct.UseVisualStyleBackColor = false;
            this.btnViewProduct.Click += new System.EventHandler(this.btnViewProduct_Click);
            this.btnViewProduct.MouseEnter += new System.EventHandler(this.btnViewProduct_MouseEnter);
            this.btnViewProduct.MouseLeave += new System.EventHandler(this.btnViewProduct_MouseLeave);
            // 
            // ProductID
            // 
            this.ProductID.DataPropertyName = "ProductID";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ProductID.DefaultCellStyle = dataGridViewCellStyle1;
            this.ProductID.HeaderText = "Product ID";
            this.ProductID.Name = "ProductID";
            this.ProductID.ReadOnly = true;
            // 
            // Message
            // 
            this.Message.DataPropertyName = "Message";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Message.DefaultCellStyle = dataGridViewCellStyle2;
            this.Message.FillWeight = 300F;
            this.Message.HeaderText = "Message";
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ProductName.DefaultCellStyle = dataGridViewCellStyle3;
            this.ProductName.FillWeight = 150F;
            this.ProductName.HeaderText = "Product Name";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            // 
            // NotificationType
            // 
            this.NotificationType.DataPropertyName = "NotificationType";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NotificationType.DefaultCellStyle = dataGridViewCellStyle4;
            this.NotificationType.HeaderText = "Notification Type";
            this.NotificationType.Name = "NotificationType";
            this.NotificationType.ReadOnly = true;
            // 
            // Timestamp
            // 
            this.Timestamp.DataPropertyName = "Timestamp";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Format = "g";
            this.Timestamp.DefaultCellStyle = dataGridViewCellStyle5;
            this.Timestamp.FillWeight = 120F;
            this.Timestamp.HeaderText = "Timestamp";
            this.Timestamp.Name = "Timestamp";
            this.Timestamp.ReadOnly = true;
            // 
            // IsRead
            // 
            this.IsRead.DataPropertyName = "IsRead";
            this.IsRead.FillWeight = 40F;
            this.IsRead.HeaderText = "Is Read?";
            this.IsRead.Name = "IsRead";
            this.IsRead.ReadOnly = true;
            this.IsRead.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsRead.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // NotificationID
            // 
            this.NotificationID.DataPropertyName = "NotificationID";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NotificationID.DefaultCellStyle = dataGridViewCellStyle6;
            this.NotificationID.HeaderText = "Notification ID";
            this.NotificationID.Name = "NotificationID";
            this.NotificationID.ReadOnly = true;
            this.NotificationID.Visible = false;
            // 
            // NotificationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(907, 450);
            this.Controls.Add(this.dgvNotifications);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Name = "NotificationsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Notifications";
            this.Load += new System.EventHandler(this.NotificationsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotifications)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvNotifications;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnBackToDashboard;
        private System.Windows.Forms.Button btnDeleteNotification;
        private System.Windows.Forms.Button btnMarkAsRead;
        private System.Windows.Forms.Button btnViewProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMarkAllAsRead;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.ImageList imageListActions;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotificationType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Timestamp;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsRead;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotificationID;
    }
}