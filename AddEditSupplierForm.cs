using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class AddEditSupplierForm : Form
    {
        private Color buttonIdleColor = Color.WhiteSmoke;
        private Color buttonHoverColor = Color.LightGray;

        private readonly DatabaseRepository _repository;
        private readonly Supplier _supplierToEdit;
        private readonly bool _isEditMode;

        public AddEditSupplierForm(DatabaseRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            _isEditMode = false;
            this.Text = "Add New Supplier";
        }

        public AddEditSupplierForm(DatabaseRepository repository, Supplier supplierToEdit)
        {
            InitializeComponent();
            _repository = repository;
            _supplierToEdit = supplierToEdit;
            _isEditMode = true;
            this.Text = "Edit Supplier";
            PopulateFields();
        }

        private void PopulateFields()
        {
            if (_supplierToEdit != null)
            {
                txtName.Text = _supplierToEdit.Name;
                txtContactInfo.Text = _supplierToEdit.ContactInfo;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // --- Final Validation Block ---

            // 1. Check if the supplier name is empty.
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Supplier name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            // 2. Check if the contact info is empty.
            if (string.IsNullOrWhiteSpace(txtContactInfo.Text))
            {
                MessageBox.Show("Contact Info cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContactInfo.Focus();
                return;
            }

            // 3. --- THIS IS THE NEW FIX ---
            // Check if the contact info has exactly 11 digits.
            if (txtContactInfo.Text.Length != 11)
            {
                MessageBox.Show("Contact Info must be exactly 11 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContactInfo.Focus();
                return;
            }

            // 4. Check for duplicate supplier name.
            string newName = txtName.Text.Trim();
            int currentId = _isEditMode ? _supplierToEdit.SupplierID : 0;
            if (_repository.SupplierNameExists(newName, currentId))
            {
                MessageBox.Show("A supplier with this name already exists. Please choose a different name.", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            // If all validation passes, proceed with saving.
            try
            {
                if (_isEditMode)
                {
                    _supplierToEdit.Name = newName;
                    _supplierToEdit.ContactInfo = txtContactInfo.Text.Trim();
                    _repository.UpdateSupplier(_supplierToEdit);
                }
                else
                {
                    var newSupplier = new Supplier
                    {
                        Name = newName,
                        ContactInfo = txtContactInfo.Text.Trim()
                    };
                    _repository.AddSupplier(newSupplier);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtContactInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonHoverColor;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonIdleColor;
        }
    }
}