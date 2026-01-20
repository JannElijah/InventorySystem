using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class BulkEditForm : Form
    {
        // Properties to hold the user's choices
        public string FieldToUpdate { get; private set; }
        public string NewValue { get; private set; }

        public BulkEditForm()
        {
            InitializeComponent();
        }

        private void BulkEditForm_Load(object sender, EventArgs e)
        {
            // Populate the dropdown with the fields the user is allowed to bulk-edit.
            cmbFieldToEdit.Items.Add("Brand");
            cmbFieldToEdit.Items.Add("Low Stock Threshold");
            cmbFieldToEdit.Items.Add("Notes");
            cmbFieldToEdit.Items.Add("Stock Quantity");
            cmbFieldToEdit.Items.Add("Volume");
            cmbFieldToEdit.Items.Add("Type");
            // Add any other text-based fields you want here.

            cmbFieldToEdit.SelectedIndex = 0; // Select the first item by default
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Check if a field is selected and a value is entered.
            if (cmbFieldToEdit.SelectedItem == null || string.IsNullOrWhiteSpace(txtNewValue.Text))
            {
                MessageBox.Show("Please select a field and enter a new value.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Store the user's choices in the public properties.
            this.FieldToUpdate = cmbFieldToEdit.SelectedItem.ToString();
            this.NewValue = txtNewValue.Text;

            // Set the DialogResult to OK and close the form.
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtNewValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This code allows only digits (0-9) and control characters (like Backspace).
            // It blocks letters, symbols, etc.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Discard the keypress
            }
        }

        private void CmbFieldToEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedField = cmbFieldToEdit.SelectedItem.ToString();

            // List of fields that should be numeric-only
            if (selectedField == "Low Stock Threshold" || selectedField == "Stock Quantity")
            {
                // If it's a numeric field, attach our validation method.
                txtNewValue.KeyPress += TxtNewValue_KeyPress;
            }
            else
            {
                // If it's a text field, detach the validation method so the user can type letters.
                txtNewValue.KeyPress -= TxtNewValue_KeyPress;
            }

            // Clear the textbox whenever the selection changes to avoid confusion.
            txtNewValue.Clear();
        }
    }
}
