using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class AddProductForm : Form
    {
        // --- Class-level Variables ---
        private readonly DatabaseRepository _repository;
        private readonly Product _productToEdit;
        private readonly bool _isEditMode;

        // Variables for tracking manual stock adjustments
        private bool _stockWasManuallyAdjusted = false;
        private int _originalStockForLogging;

        #region Constructors and Form Load

        // "Add" Mode Constructor
        public AddProductForm()
        {
            InitializeComponent();
            _repository = new DatabaseRepository();
            _isEditMode = false;
            _productToEdit = new Product(); // Start with a fresh product
            this.Text = "Add New Product";
        }

        // "Edit" Mode Constructor
        public AddProductForm(Product productToEdit)
        {
            InitializeComponent();
            _repository = new DatabaseRepository();
            _productToEdit = productToEdit;
            _isEditMode = true;
            this.Text = "Edit Product";

            PopulateFieldsForEdit();
        }

        private void AddProductForm_Load(object sender, EventArgs e)
        {
            // Configure the form's initial state
            pnlStockAdjustment.Visible = false;
            btnEditStockQuantity.Visible = _isEditMode;

            // Populate the "Reason" dropdown
            cmbAdjustmentReason.Items.Add("Stocktaking Correction");
            cmbAdjustmentReason.Items.Add("Damaged / Expired Goods");
            cmbAdjustmentReason.Items.Add("Initial Stock Entry");
            cmbAdjustmentReason.Items.Add("Found Inventory");
            cmbAdjustmentReason.Items.Add("Lost / Stolen Inventory");
            cmbAdjustmentReason.Items.Add("Data Entry Error Correction");
            cmbAdjustmentReason.Items.Add("Others...");
            cmbAdjustmentReason.SelectedIndex = 0;

            // Hide the "Others" fields by default
            txtReasonOther.Visible = false;
        }

        #endregion

        #region Panel Switching and UI Logic

        private void BtnEditStockQuantity_Click(object sender, EventArgs e)
        {
            // Prepare and show the stock adjustment panel
            txtCurrentStock.Text = _productToEdit.StockQuantity.ToString();
            numNewStock.Value = _productToEdit.StockQuantity;
            _originalStockForLogging = _productToEdit.StockQuantity;

            pnlProductDetails.Visible = false;
            pnlStockAdjustment.Visible = true;
        }

        private void BtnBackToDetails_Click(object sender, EventArgs e)
        {
            // Hide the adjustment panel and show the details panel
            pnlStockAdjustment.Visible = false;
            pnlProductDetails.Visible = true;
        }

        #endregion

        #region Data and Save Logic

        private void PopulateFieldsForEdit()
        {
            txtBarcode.Text = _productToEdit.Barcode;
            txtBrand.Text = _productToEdit.Brand;
            txtDescription.Text = _productToEdit.Description;
            txtVolume.Text = _productToEdit.Volume;
            txtType.Text = _productToEdit.Type;
            nudPurchaseCost.Value = _productToEdit.PurchaseCost;

            // === FIX: Load the Selling Price ===
            nudSellingPrice.Value = _productToEdit.SellingPrice;

            numLowStockThreshold.Value = _productToEdit.LowStockThreshold;

            // Note: There should be no control to edit stock quantity on this main panel.
            // This forces the user to use the dedicated "Edit Stock Quantity" button.
        }

        private void BtnApplyStockChange_Click(object sender, EventArgs e)
        {
            int newStock = (int)numNewStock.Value;

            if (newStock == _originalStockForLogging)
            {
                BtnBackToDetails_Click(sender, e); // No change, just go back
                return;
            }

            var confirmResult = MessageBox.Show(
                "You are manually changing the stock quantity outside of a Supply/Delivery transaction.\n\n" +
                $"Old Stock: {_originalStockForLogging}\n" +
                $"New Stock: {newStock}\n\n" +
                "This action is for correcting inventory errors and will be logged.\nAre you sure you want to proceed?",
                "Confirm Manual Stock Adjustment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                _productToEdit.StockQuantity = newStock;
                _stockWasManuallyAdjusted = true; // Set flag for the main save button
                BtnBackToDetails_Click(sender, e); // Go back to the main view
            }
        }

        private void BtnSaveProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarcode.Text) || string.IsNullOrWhiteSpace(txtBrand.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Barcode, Brand, and Description cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _productToEdit.Barcode = txtBarcode.Text;
                _productToEdit.Brand = txtBrand.Text;
                _productToEdit.Description = txtDescription.Text;
                _productToEdit.Volume = txtVolume.Text;
                _productToEdit.Type = txtType.Text;
                _productToEdit.PurchaseCost = nudPurchaseCost.Value;

                // === FIX: Save the Selling Price ===
                _productToEdit.SellingPrice = nudSellingPrice.Value;

                _productToEdit.LowStockThreshold = (int)numLowStockThreshold.Value;

                if (_isEditMode)
                {
                    _repository.UpdateProduct(_productToEdit);

                    if (_stockWasManuallyAdjusted)
                    {
                        string reason = cmbAdjustmentReason.SelectedItem.ToString();
                        if (reason == "Others...")
                        {
                            reason = txtReasonOther.Text;
                        }

                        string message = $"Stock for '{_productToEdit.Description}' was manually adjusted from {_originalStockForLogging} to {_productToEdit.StockQuantity}. Reason: {reason}";
                        // Assumes you have a method like this in your repository
                        _repository.CreateNotification(_productToEdit.ProductID, "Manual Adjustment", message);
                    }
                }
                else // "Add" mode
                {
                    // Even when adding new products, we need to save the selling price
                    _productToEdit.StockQuantity = 0; // Default for new products
                    _repository.AddProduct(_productToEdit);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the product: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.WhiteSmoke; // Or your hover color
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.White; // Or your idle color
            }
        }

        #endregion

        private void CmbAdjustmentReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Safety check to prevent crashes during form load
            if (cmbAdjustmentReason.SelectedItem == null)
            {
                return;
            }

            // This comparison is correct, as proven by your screenshot.
            bool isOtherSelected = cmbAdjustmentReason.SelectedItem.ToString() == "Others...";

            // This logic correctly controls the visibility of all relevant controls.
            lblReasonOther.Visible = isOtherSelected;
            txtReasonOther.Visible = isOtherSelected;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}