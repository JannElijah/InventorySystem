using InventorySystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// MAKE SURE YOUR NAMESPACE MATCHES YOUR PROJECT'S NAME
namespace InventorySystem
{
    public partial class TransactionForm : Form
    {
        // --- Public Properties to return data to Form1 ---
        public int SelectedQuantity => (int)numQuantity.Value;
        public string TransactionType { get; private set; }
        public int? SelectedSupplierId { get; private set; }
        // ADD THIS LINE INSTEAD:
        public string CustomerName { get; set; }

        private readonly Product _product;

        public int StockBefore { get; set; }
        public int StockAfter { get; set; }

        private readonly string _transactionType;

        // PASTE THIS COMPLETE REPLACEMENT into TransactionForm.cs

        public TransactionForm(Product product, List<Supplier> suppliers, string currentCustomerName = null, int? preselectedSupplierId = null)
        {
            InitializeComponent();

            // Store the product and populate UI details
            _product = product;
            this.Text = $"Transaction for: {_product.Description}";
            lblBarcodeData.Text = _product.Barcode;
            lblBrandData.Text = _product.Brand;
            lblCurrentStockData.Text = _product.StockQuantity.ToString();

            // Configure and populate the Supplier ComboBox
            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.ValueMember = "SupplierID";
            cmbSuppliers.SelectedIndex = -1; // Default to no selection

            // --- NEW LOGIC TO HANDLE PRE-SELECTED SUPPLIER ---
            if (preselectedSupplierId.HasValue)
            {
                // Automatically select the supplier passed from Form1
                cmbSuppliers.SelectedValue = preselectedSupplierId.Value;

                // Lock the dropdown to prevent changes during the transaction
                cmbSuppliers.Enabled = false;
            }

            // --- YOUR EXISTING LOGIC FOR CUSTOMER NAME (it's good) ---
            if (!string.IsNullOrEmpty(currentCustomerName))
            {
                txtDeliverTo.Text = currentCustomerName;
                txtDeliverTo.Enabled = false;
            }
            else
            {
                txtDeliverTo.Text = "";
                txtDeliverTo.Enabled = true;
            }
        }

        private void BtnSupply_Click(object sender, EventArgs e)
        {
            if (cmbSuppliers.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier.", "Supplier Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.TransactionType = "Supply";
            this.SelectedSupplierId = (int)cmbSuppliers.SelectedValue;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnDeliver_Click(object sender, EventArgs e)
        {
            this.TransactionType = "Deliver";
            if (string.IsNullOrWhiteSpace(txtDeliverTo.Text))
            {
                MessageBox.Show("Please enter a customer name for the delivery.", "Customer Name Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDeliverTo.Focus();
                return;
            }

            if (numQuantity.Value > _product.StockQuantity)
            {
                MessageBox.Show($"Insufficient stock. Only {_product.StockQuantity} units are available.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numQuantity.Focus();
                return;
            }

            this.CustomerName = txtDeliverTo.Text.Trim();
            this.TransactionType = "Deliver";
            this.SelectedSupplierId = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSupply.Enabled = (cmbSuppliers.SelectedIndex > -1);
        }
    }
}