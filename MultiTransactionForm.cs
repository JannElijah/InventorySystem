using InventoryManagementSystem; // Or your project's namespace
using Microsoft.VisualBasic; // Required for the simple InputBox
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace InventorySystem
{
    // A simple helper class to manage items in our "shopping cart" grid.
    // In MultiTransactionForm.cs, ABOVE the "public partial class..." line
    public enum TransactionMode { Supply, Delivery }

    public partial class MultiTransactionForm : Form
    {
        private readonly DatabaseRepository _repository;

        // This is our "shopping cart". A list that will hold all items for the transaction.
        private BindingList<TransactionItem> _transactionItems = new BindingList<TransactionItem>();

        public MultiTransactionForm()
        {
            InitializeComponent();
            _repository = new DatabaseRepository();
        }

        private void MultiTransactionForm_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            SetupTransactionGrid(); // NEW: Configure the main grid
                                    // --- CHANGE THIS PART ---
                                    // Set the radio buttons based on the mode the form was opened with.
            if (_initialMode == TransactionMode.Supply)
            {
                rbSupply.Checked = true;
            }
            else
            {
                rbDelivery.Checked = true;
            }

            // This will now correctly set the UI based on the checked radio button.
            UpdateUIVisibility();
        }

        // In MultiTransactionForm.cs, add this new constructor below the existing one.

        private readonly TransactionMode _initialMode;

        // This is our new constructor
        public MultiTransactionForm(TransactionMode initialMode) : this() // ": this()" calls the default constructor first
        {
            _initialMode = initialMode;
        }

        /// <summary>
        /// Configures the columns for our main transaction DataGridView.
        /// </summary>
        // In MultiTransactionForm.cs
        // REPLACE your entire SetupTransactionGrid method with this corrected version.

        /// <summary>
        /// Configures the columns for our main transaction DataGridView.
        /// </summary>
        private void SetupTransactionGrid()
        {
            dgvTransactionItems.AutoGenerateColumns = false;
            dgvTransactionItems.DataSource = _transactionItems;

            // Define the simple columns first.
            dgvTransactionItems.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Product Description", DataPropertyName = "Description", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvTransactionItems.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Quantity", DataPropertyName = "Quantity", ReadOnly = false, Width = 70 });

            // --- CORRECTED METHOD FOR COLUMNS WITH FORMATTING ---

            // 1. Create the "Unit Price/Cost" column object.
            var priceColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Unit Price/Cost",
                DataPropertyName = "UnitPriceCost",
                ReadOnly = true,
                Width = 100
            };
            // 2. Set the format on its DefaultCellStyle property. "c" formats it as local currency.
            priceColumn.DefaultCellStyle.Format = "c";
            // 3. Add the fully configured column to the grid.
            dgvTransactionItems.Columns.Add(priceColumn);


            // --- DO THE SAME FOR THE "Line Total" COLUMN ---

            // 1. Create the "Line Total" column object.
            var totalColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Line Total",
                DataPropertyName = "LineTotal",
                ReadOnly = true,
                Width = 120
            };
            // 2. Set the format on its DefaultCellStyle property.
            totalColumn.DefaultCellStyle.Format = "c";
            // 3. Add the column to the grid.
            dgvTransactionItems.Columns.Add(totalColumn);
        }

        private void LoadSuppliers()
        {
            List<Supplier> suppliers = _repository.GetAllSuppliers();
            var placeholder = new Supplier { SupplierID = 0, Name = "Select a supplier..." };
            suppliers.Insert(0, placeholder);

            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.ValueMember = "SupplierID";
        }

        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIVisibility();
        }

        private void UpdateUIVisibility()
        {
            bool isSupplyMode = rbSupply.Checked;

            lblSupplier.Visible = isSupplyMode;
            cmbSuppliers.Visible = isSupplyMode;
            lblCustomerName.Visible = !isSupplyMode;
            txtCustomerName.Visible = !isSupplyMode;
            btnProcessTransaction.Text = isSupplyMode ? "Process Supply" : "Process Delivery";

            // Clear the cart if the mode changes, as prices/costs will be different.
            dgvTransactionItems.BackgroundColor = isSupplyMode ? Color.LightGreen : Color.LightSteelBlue;
            _transactionItems.Clear();
            UpdateSubtotal();
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearchProduct.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                lstSearchResults.DataSource = null;
                return;
            }
            List<Product> searchResults = _repository.SearchProducts(searchTerm);
            lstSearchResults.DataSource = searchResults;
            lstSearchResults.DisplayMember = "DisplayInfo";
        }

        // --- NEW METHOD for "Add Selected Item" button ---
        // In MultiTransactionForm.cs
        // REPLACE your old btnAddItem_Click method with this one.

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (lstSearchResults.SelectedItem is Product selectedProduct)
            {
                string input = Interaction.InputBox($"Enter quantity for '{selectedProduct.Description}':", "Add Quantity", "1");

                if (int.TryParse(input, out int quantityToAdd) && quantityToAdd > 0)
                {
                    // --- NEW LOGIC STARTS HERE ---

                    // Check if this product is ALREADY in our transaction list.
                    var existingItem = _transactionItems.FirstOrDefault(item => item.ProductID == selectedProduct.ProductID);

                    if (existingItem != null)
                    {
                        // --- ITEM ALREADY EXISTS: Just update its quantity ---
                        int newQuantity = existingItem.Quantity + quantityToAdd;

                        if (rbDelivery.Checked && newQuantity > selectedProduct.StockQuantity)
                        {
                            MessageBox.Show($"Insufficient stock. You already have {existingItem.Quantity} in the list, and only {selectedProduct.StockQuantity} total units are available.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        existingItem.Quantity = newQuantity;
                    }
                    else
                    {
                        // --- ITEM IS NEW: Add it to the list ---
                        if (rbDelivery.Checked && quantityToAdd > selectedProduct.StockQuantity)
                        {
                            MessageBox.Show($"Insufficient stock. Only {selectedProduct.StockQuantity} units are available.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var newItem = new TransactionItem
                        {
                            ProductID = selectedProduct.ProductID,
                            Description = selectedProduct.Description,
                            Quantity = quantityToAdd,
                            UnitPriceCost = rbSupply.Checked ? selectedProduct.PurchaseCost : selectedProduct.SellingPrice
                        };
                        _transactionItems.Add(newItem);
                    }

                    // Refresh the entire grid to show changes and recalculate totals.
                    dgvTransactionItems.Refresh();
                    UpdateSubtotal();
                }
            }
            else
            {
                MessageBox.Show("Please select a product from the search results to add.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- NEW METHOD for "Remove Selected" button ---
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvTransactionItems.CurrentRow != null && dgvTransactionItems.CurrentRow.DataBoundItem is TransactionItem selectedItem)
            {
                _transactionItems.Remove(selectedItem);
                UpdateSubtotal();
            }
        }

        // --- NEW METHOD for real-time calculations ---
        private void dgvTransactionItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure the edit happened in a valid row and is in the "Quantity" column.
            if (e.RowIndex >= 0 && dgvTransactionItems.Columns[e.ColumnIndex].HeaderText == "Quantity")
            {
                // Force the grid and the underlying data to refresh.
                dgvTransactionItems.Refresh();
                UpdateSubtotal();
            }
        }

        // --- NEW METHOD to update the subtotal label ---
        private void UpdateSubtotal()
        {
            decimal subtotal = _transactionItems.Sum(item => item.LineTotal);
            lblSubtotal.Text = $"Subtotal: {subtotal:C}";
        }

        // --- NEW METHOD for the "Process Transaction" button ---
        private void btnProcessTransaction_Click(object sender, EventArgs e)
        {
            // --- 1. Validation ---
            if (_transactionItems.Count == 0)
            {
                MessageBox.Show("There are no items to process.", "Empty Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbSupply.Checked && (int)cmbSuppliers.SelectedValue <= 0)
            {
                MessageBox.Show("Please select a supplier.", "Supplier Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbDelivery.Checked && string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Please enter a customer name.", "Customer Name Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- 2. Process based on mode ---
            try
            {
                if (rbSupply.Checked)
                {
                    // --- Process a Supply ---
                    int? supplierId = (int)cmbSuppliers.SelectedValue;
                    foreach (var item in _transactionItems)
                    {
                        Product p = _repository.GetProductById(item.ProductID);
                        int stockBefore = p.StockQuantity;
                        int stockAfter = stockBefore + item.Quantity;

                        _repository.UpdateProductStock(item.ProductID, stockAfter);
                        _repository.AddTransaction(new Transaction
                        {
                            ProductID = item.ProductID,
                            TransactionType = "Supply",
                            QuantityChange = item.Quantity,
                            StockBefore = stockBefore,
                            StockAfter = stockAfter,
                            TransactionDate = DateTime.Now,
                            SupplierID = supplierId
                        });
                    }
                    MessageBox.Show("Supply processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // --- Process a Delivery (similar to your Form1 logic) ---
                    Sale newSale = new Sale { CustomerName = txtCustomerName.Text.Trim(), SaleDate = DateTime.Now };
                    int newSaleId = _repository.AddSale(newSale);

                    foreach (var item in _transactionItems)
                    {
                        SaleItem saleItem = new SaleItem { ProductID = item.ProductID, Quantity = item.Quantity, UnitPrice = item.UnitPriceCost, Total = item.LineTotal };
                        int newSaleItemId = _repository.AddSaleItem(newSaleId, saleItem);

                        Product p = _repository.GetProductById(item.ProductID);
                        int stockBefore = p.StockQuantity;
                        int stockAfter = stockBefore - item.Quantity;

                        _repository.UpdateProductStock(item.ProductID, stockAfter);
                        _repository.AddTransaction(new Transaction
                        {
                            ProductID = item.ProductID,
                            TransactionType = "Delivery",
                            QuantityChange = -item.Quantity, // Use negative for deliveries
                            StockBefore = stockBefore,
                            StockAfter = stockAfter,
                            TransactionDate = DateTime.Now,
                            SaleItemID = newSaleItemId
                        });
                    }
                    MessageBox.Show("Delivery processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A critical error occurred while processing the transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // In MultiTransactionForm.cs
        private void lstSearchResults_DoubleClick(object sender, EventArgs e)
        {
            // This simply calls the same logic as our "Add" button.
            btnAddItem_Click(sender, e);
        }

        // In MultiTransactionForm.cs
        private void lstSearchResults_KeyDown(object sender, KeyEventArgs e)
        {
            // If the user pressed the Enter key...
            if (e.KeyCode == Keys.Enter)
            {
                // ...run the same logic as the "Add" button.
                btnAddItem_Click(sender, e);
                // Mark the event as handled to prevent any "ding" sounds.
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // In MultiTransactionForm.cs
        private void dgvTransactionItems_KeyDown(object sender, KeyEventArgs e)
        {
            // If the user pressed the Delete key...
            if (e.KeyCode == Keys.Delete)
            {
                // ...run the same logic as the "Remove" button.
                btnRemoveItem_Click(sender, e);
            }
        }
    }
}