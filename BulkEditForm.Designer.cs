namespace InventorySystem
{
    partial class BulkEditForm
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
            this.lblFieldEdit = new System.Windows.Forms.Label();
            this.lblNewValue = new System.Windows.Forms.Label();
            this.cmbFieldToEdit = new System.Windows.Forms.ComboBox();
            this.txtNewValue = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFieldEdit
            // 
            this.lblFieldEdit.AutoSize = true;
            this.lblFieldEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFieldEdit.Location = new System.Drawing.Point(12, 38);
            this.lblFieldEdit.Name = "lblFieldEdit";
            this.lblFieldEdit.Size = new System.Drawing.Size(80, 17);
            this.lblFieldEdit.TabIndex = 0;
            this.lblFieldEdit.Text = "Field to Edit:";
            // 
            // lblNewValue
            // 
            this.lblNewValue.AutoSize = true;
            this.lblNewValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewValue.Location = new System.Drawing.Point(12, 87);
            this.lblNewValue.Name = "lblNewValue";
            this.lblNewValue.Size = new System.Drawing.Size(72, 17);
            this.lblNewValue.TabIndex = 2;
            this.lblNewValue.Text = "New Value:";
            // 
            // cmbFieldToEdit
            // 
            this.cmbFieldToEdit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cmbFieldToEdit.FormattingEnabled = true;
            this.cmbFieldToEdit.Location = new System.Drawing.Point(108, 38);
            this.cmbFieldToEdit.Name = "cmbFieldToEdit";
            this.cmbFieldToEdit.Size = new System.Drawing.Size(121, 21);
            this.cmbFieldToEdit.TabIndex = 3;
            this.cmbFieldToEdit.SelectedIndexChanged += new System.EventHandler(this.CmbFieldToEdit_SelectedIndexChanged);
            // 
            // txtNewValue
            // 
            this.txtNewValue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNewValue.Location = new System.Drawing.Point(108, 87);
            this.txtNewValue.Name = "txtNewValue";
            this.txtNewValue.Size = new System.Drawing.Size(121, 20);
            this.txtNewValue.TabIndex = 4;
            this.txtNewValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtNewValue_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(12, 217);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(154, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BulkEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 259);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtNewValue);
            this.Controls.Add(this.cmbFieldToEdit);
            this.Controls.Add(this.lblNewValue);
            this.Controls.Add(this.lblFieldEdit);
            this.MaximumSize = new System.Drawing.Size(257, 298);
            this.Name = "BulkEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bulk Edit";
            this.Load += new System.EventHandler(this.BulkEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFieldEdit;
        private System.Windows.Forms.Label lblNewValue;
        private System.Windows.Forms.ComboBox cmbFieldToEdit;
        private System.Windows.Forms.TextBox txtNewValue;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}