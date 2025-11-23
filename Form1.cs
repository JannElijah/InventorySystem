using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks; // === NEW: Required for the "flash" feedback ===
using System.Windows.Forms;
using ZXing;
using System.Drawing.Printing;
using System.Net.NetworkInformation;
using InventoryManagementSystem;
using System.Text;
using Microsoft.VisualBasic;
namespace InventorySystem
{
    /// <summary>
    /// The main inventory management and transaction processing form.
    /// Handles data display, filtering, and stock updates for both Admin and Staff users.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly Color headerButtonIdleColor = Color.White;
        private readonly Color headerButtonHoverColor = Color.Gainsboro; // A very light gray
        private readonly User _loggedInUser;
        private readonly int productIdToSelect;
        private readonly DatabaseRepository _repository;
        private readonly string _initialFilter = null;
        private Sale _currentSale;
        private SupplyOrder _currentSupplyOrder; // === NEW: To hold the supply cart ===
        private string _currentTransactionMode;

        // === FIX #1: Initialize the list and change the barcode buffer ===
        private List<Product> _allProducts = new List<Product>();
        private List<Supplier> allSuppliers = new List<Supplier>();
        private readonly StringBuilder _barcodeBuffer = new StringBuilder();
        private DateTime _lastKeystrokeTime = DateTime.Now;
        private List<Product> _productsToPrint;
        private int _currentProductIndex;

        #region Initialization

        /// <summary>
        /// Initializes the main form, stores the logged-in user, and prepares the UI based on the user's role.
        /// </summary>
        /// <summary>
        /// Initializes the main form, stores the logged-in user, and prepares the UI based on the user's role.
        /// </summary>
        public Form1(User user, int productIdToSelect = 0, string initialFilter = null)
        {
            InitializeComponent();
            dgvCurrentSale.AutoGenerateColumns = false;
            // Store the values passed in
            _loggedInUser = user;
            this.productIdToSelect = productIdToSelect;
            _initialFilter = initialFilter; // Now this works, because 'initialFilter' is a parameter

            // The rest of your existing setup code
            _repository = new DatabaseRepository();
            this.Text = $"Inventory System - Logged in as: {user.Username} ({user.Role})";
            this.KeyPreview = true;
        }

        /// <summary>
        /// Handles the initial setup when the form loads.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            SetupCartGrid();
            _allProducts = _repository.GetAllProducts();
            allSuppliers = _repository.GetAllSuppliers();
            SetupDataGridView();
            PopulateFilterComboBoxes();
            ApplyAuthorization();
            ApplyFilters(); // Initial filter to display all data correctly.
            UpdateButtonStates(); // === NEW: Set initial button states on load ===
            if (!string.IsNullOrEmpty(_initialFilter))
            {
                // If it did, apply the filter now.
                ApplyDashboardFilter(_initialFilter);
            }
        }

        /// <summary>
        /// Sets up the visual properties and formatting of the DataGridView.
        /// </summary>
        private void SetupDataGridView()
        {
            // All column definitions and formatting are now correctly handled by the visual designer.
            // This method's only job now is to connect the data to the grid.
            dgvInventory.DataSource = _allProducts;
            dgvInventory.ReadOnly = true;
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.MultiSelect = true;
        }
        /// <summary>
        /// Populates the Brand and Type ComboBoxes with distinct values from the product list.
        /// </summary>
        private void PopulateFilterComboBoxes()
        {
            List<string> brandNames = _allProducts.Select(p => p.Brand).Distinct().ToList();
            brandNames.Insert(0, "All Brands");
            cmbBrandFilter.DataSource = brandNames;

            List<string> typeNames = _allProducts.Select(p => p.Type).Distinct().ToList();
            typeNames.Insert(0, "All Types");
            cmbTypeFilter.DataSource = typeNames;
        }

        /// <summary>
        /// Configures the UI controls based on the logged-in user's role.
        /// </summary>
        private void ApplyAuthorization()
        {
            // Only Admins have access to these management buttons.
            bool isAdmin = _loggedInUser.Role == "Admin";

            btnBackToDashboard.Visible = isAdmin;
            btnManageUsers.Visible = isAdmin;
            btnAddProduct.Enabled = isAdmin;
            btnDeleteProduct.Enabled = isAdmin;
        }


        #endregion


        #region Data Handling and Filtering

        /// <summary>
        /// A central method that applies all active filters (dropdowns, search box) to the product list.
        /// </summary>
        private void ApplyFilters()
        {
            if (cmbBrandFilter.SelectedItem == null || cmbTypeFilter.SelectedItem == null || _allProducts == null)
                return;

            IEnumerable<Product> filteredProducts = _allProducts;

            // Apply dropdown filters.
            string selectedBrand = cmbBrandFilter.SelectedItem.ToString();
            if (selectedBrand != "All Brands")
            {
                filteredProducts = filteredProducts.Where(p => p.Brand == selectedBrand);
            }

            string selectedType = cmbTypeFilter.SelectedItem.ToString();
            if (selectedType != "All Types")
            {
                filteredProducts = filteredProducts.Where(p => p.Type == selectedType);
            }

            // Apply text search filter.
            string searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredProducts = filteredProducts.Where(p =>
                    (p.PartNumber?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.Brand?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.Description?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            dgvInventory.DataSource = filteredProducts.ToList();
            ApplyRowStyling();

            // This logic to update the status bar is already perfect.
            lblStatus.Text = $"Displaying {dgvInventory.Rows.Count} of {_allProducts.Count} total products.";
            dgvInventory.Invalidate();
        }

        /// <summary>
        /// Reloads all product data from the database and refreshes the grid.
        /// </summary>
        private void RefreshDataGrid()
        {
            _allProducts = _repository.GetAllProducts();
            ApplyFilters();
        }
        public void ApplyDashboardFilter(string filterType)
        {
            // This method is called by the dashboard to apply a filter.
            ClearFilters();

            // CORRECTED: Using _allProducts
            IEnumerable<Product> filteredList = _allProducts;

            switch (filterType)
            {
                case "OUT_OF_STOCK":
                    // CORRECTED: Using _allProducts
                    filteredList = _allProducts.Where(p => p.StockQuantity == 0);
                    break;

                case "LOW_STOCK":
                    // CORRECTED: Using _allProducts
                    filteredList = _allProducts.Where(p => p.StockQuantity > 0 && p.StockQuantity <= p.LowStockThreshold);
                    break;

                case "TOTAL_PRODUCTS":
                default:
                    break;
            }

            dgvInventory.DataSource = filteredList.ToList();

            ApplyRowStyling();
            UpdateButtonStates();
        }

        private void ClearFilters()
        {
            // Logic from your button click
            cmbBrandFilter.SelectedIndex = 0;
            cmbTypeFilter.SelectedIndex = 0;
            txtSearch.Clear();

            // Calling ApplyFilters() is correct to reset the view
            ApplyFilters();
        }
        /// <summary>
        /// Applies color-coded styling to rows based on their stock quantity.
        /// </summary>
        private void ApplyRowStyling()
        {
            foreach (DataGridViewRow row in dgvInventory.Rows)
            {
                if (row.DataBoundItem is Product product)
                {
                    if (product.StockQuantity <= product.LowStockThreshold && product.StockQuantity > 0) // Low Stock
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else if (product.StockQuantity == 0) // Out of Stock
                    {
                        row.DefaultCellStyle.BackColor = Color.DarkRed;
                        row.DefaultCellStyle.ForeColor = Color.White;
                    }
                    else if (product.StockQuantity >= 40) // Healthy Stock
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else // Normal Stock
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        /// <summary>
        /// Public method allowing other forms to select a product in this grid.
        /// </summary>
        public void SelectProductById(int productId)
        {
            foreach (DataGridViewRow row in dgvInventory.Rows)
            {
                if (row.DataBoundItem is Product product && product.ProductID == productId)
                {
                    dgvInventory.ClearSelection();
                    row.Selected = true;
                    dgvInventory.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        #endregion

        #region Transaction Logic

        /// <summary>
        /// Processes a supply or delivery transaction for the selected product.
        /// </summary>
        // === MODIFIED: Added 'async' keyword for the flash feedback feature ===
        

        /// <summary>
        /// Clears the transaction panel fields and deselects the product.
        /// </summary>


        #endregion

        #region Product Management (Admin Only)

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddProductForm())
            {
                addForm.ShowDialog();
            }
            RefreshDataGrid();
        }

        private void DgvInventory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvInventory.Rows[e.RowIndex].DataBoundItem is Product selectedProduct)
            {
                if (_loggedInUser.Role != "Admin") return;

                using (var editForm = new AddProductForm(selectedProduct))
                {
                    editForm.ShowDialog();
                }
                RefreshDataGrid();
            }
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            // === MODIFIED: Now handles multiple selections ===
            if (dgvInventory.SelectedRows.Count > 0)
            {
                string itemText = dgvInventory.SelectedRows.Count == 1 ? "product" : "products";
                string productName = (dgvInventory.SelectedRows[0].DataBoundItem as Product)?.Description ?? "the selected item";

                string message = dgvInventory.SelectedRows.Count == 1
                    ? $"Are you sure you want to permanently delete '{productName}'?"
                    : $"Are you sure you want to permanently delete these {dgvInventory.SelectedRows.Count} {itemText}?";

                var confirmation = MessageBox.Show(message, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmation == DialogResult.Yes)
                {
                    try
                    {
                        foreach (DataGridViewRow row in dgvInventory.SelectedRows)
                        {
                            if (row.DataBoundItem is Product productToDelete)
                            {
                                _repository.DeleteProduct(productToDelete.ProductID);
                            }
                        }
                        MessageBox.Show($"{dgvInventory.SelectedRows.Count} {itemText} deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while deleting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region UI Event Handlers

        // === NEW: Central method for enabling/disabling controls ===
        // PASTE THIS NEW VERSION INTO Form1.cs
        // PASTE THIS NEW VERSION INTO Form1.cs
        private void UpdateButtonStates()
        {
            // Enable the print and delete buttons if one OR MORE rows are selected.
            bool atLeastOneRowSelected = dgvInventory.SelectedRows.Count > 0;

            // Admin check is still important
            bool isAdmin = _loggedInUser.Role == "Admin";

            btnDeleteProduct.Enabled = atLeastOneRowSelected && isAdmin;
            btnPrintBarcode.Enabled = atLeastOneRowSelected;

            // Bulk edit is only useful if more than one row is selected
            btnBulkEdit.Enabled = dgvInventory.SelectedRows.Count > 1 && isAdmin;
        }

        // Paste this inside Form1.cs
        // Paste this inside Form1.cs
        private void SetupCartGrid()
        {
            dgvCurrentSale.Columns.Clear();

            // Standard Description
            var colDescription = new DataGridViewTextBoxColumn
            {
                Name = "colDescription",
                HeaderText = "Description",
                DataPropertyName = "Description",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            // Standard Quantity
            var colQuantity = new DataGridViewTextBoxColumn
            {
                Name = "colQuantity",
                HeaderText = "Quantity",
                DataPropertyName = "Quantity",
                ReadOnly = false,
                Width = 70
            };

            // Standard Unit Price
            var colUnitPrice = new DataGridViewTextBoxColumn
            {
                Name = "colUnitPrice",
                HeaderText = "Unit Price",
                DataPropertyName = "UnitPrice",
                ReadOnly = true,
                Width = 100
            };
            colUnitPrice.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-PH");
            colUnitPrice.DefaultCellStyle.Format = "c";
            colUnitPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Standard Total
            var colTotal = new DataGridViewTextBoxColumn
            {
                Name = "colTotal",
                HeaderText = "Total",
                DataPropertyName = "Total",
                ReadOnly = true,
                Width = 120
            };
            colTotal.DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("en-PH");
            colTotal.DefaultCellStyle.Format = "c";
            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvCurrentSale.Columns.AddRange(new DataGridViewColumn[] {
        colDescription, colQuantity, colUnitPrice, colTotal
    });
        }


        private void BtnViewHistory_Click(object sender, EventArgs e)
        {
            using (var historyForm = new TransactionHistoryForm(_repository))
            {
                historyForm.ShowDialog();
            }
        }

        private void BtnManageUsers_Click(object sender, EventArgs e)
        {
            using (var userForm = new UserManagementForm(_loggedInUser))
            {
                userForm.ShowDialog();
            }
        }

        private void BtnBackToDashboard_Click(object sender, EventArgs e)
        {
            if (this.Owner is DashboardForm ownerDashboard)
            {
                ownerDashboard.LoadDashboardData();
                ownerDashboard.Show();
                this.Close();
            }
        }

        // === NEW: Keyboard Shortcut Handler ===
        // THIS IS YOUR NEW, COMBINED KeyDown METHOD
        // PASTE THESE TWO REPLACEMENT METHODS INTO Form1.cs

        #region Keyboard Event Handling (Refactored)

        // This event handles ACTION keys (like Enter, Ctrl+F)
        // This handles the ACTION of the Enter key
        // THIS IS THE FINAL, CORRECTED VERSION
        // REPLACE your current KeyDown with this correct version
        // PASTE THIS NEW, OVERRIDE METHOD INTO Form1.cs

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // This method is called BEFORE KeyDown. It's our best chance to capture command keys.

            // We are only interested in the Enter key.
            if (keyData == Keys.Enter)
            {
                // If the buffer has data, we know it's a scan from the barcode reader.
                if (_barcodeBuffer.Length > 0)
                {
                    // Run our scan logic.
                    HandleBarcodeScan();
                }

                // CRITICAL: Return 'true' to indicate that we have handled this key.
                // This stops the key from being passed on to any other control (like the DataGridView).
                // This is what will finally stop the row from moving down.
                return true;
            }

            // For all other keys, let the base functionality handle it as normal.
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // This handles gathering the CHARACTERS from the scanner
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // FIX: Only allow normal typing if the cursor is specifically in the Search Box.
            // If the "ActiveControl" is the Grid (or a cell inside it), we want to treat it as a scan.
            if (this.ActiveControl == txtSearch) return;

            // Ignore the Enter key here (handled in ProcessCmdKey)
            if (e.KeyChar == (char)Keys.Enter) return;

            // Reset buffer if too much time passed (manual typing vs fast scan)
            if ((DateTime.Now - _lastKeystrokeTime).TotalMilliseconds > 100)
            {
                _barcodeBuffer.Clear();
            }

            _barcodeBuffer.Append(e.KeyChar);
            _lastKeystrokeTime = DateTime.Now;

            // Stop the character from being typed into the Grid
            e.Handled = true;
        }



        #endregion
        // PASTE THIS NEW METHOD INTO Form1.cs


        // Filter event handlers simply call the central ApplyFilters method.
        private void CmbBrandFilter_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void CmbTypeFilter_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void TxtSearch_TextChanged(object sender, EventArgs e) => ApplyFilters();

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            // Now it's clean and simple. Its only job is to call the helper method.
            ClearFilters();
        }

        #endregion

        private void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count > 0)
            {
                _productsToPrint = new List<Product>();
                foreach (DataGridViewRow row in dgvInventory.SelectedRows)
                {
                    if (row.DataBoundItem is Product product && !string.IsNullOrEmpty(product.Barcode))
                    {
                        _productsToPrint.Add(product);
                    }
                }

                if (_productsToPrint.Count == 0)
                {
                    MessageBox.Show("None of the selected products have a barcode to print.", "No Barcodes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _currentProductIndex = 0;

                PrintDialog printDialog = new PrintDialog();
                PrintDocument printDocument = new PrintDocument();

                // --- THE REAL FIX IS HERE ---
                // 1. Define the desired filename. A timestamp makes it unique.
                string defaultFileName = $"Product Barcodes - {DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";

                // 2. Set the PrintFileName property on the PRINTER SETTINGS object.
                // This is what the "Microsoft Print to PDF" driver looks for.
                printDocument.PrinterSettings.PrintFileName = defaultFileName;

                // It's also good practice to set the DocumentName for the print queue.
                printDocument.DocumentName = "Product Barcode Sheet";
                // --- END OF FIX ---

                // Assign the configured document to the dialog.
                printDialog.Document = printDocument;

                // Wire up our multi-page printing logic.
                printDocument.PrintPage += PrintDocument_PrintPage_Multi;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    // Now, when the user clicks "Print", the driver will use the PrintFileName we set.
                    printDocument.Print();
                }

                // Unhook the event handler to prevent potential memory leaks.
                printDocument.PrintPage -= PrintDocument_PrintPage_Multi;
            }
        }

        // PASTE THIS ENTIRE NEW METHOD INTO Form1.cs
        private void PrintDocument_PrintPage_Multi(object sender, PrintPageEventArgs e)
        {
            // 1. Define the Layout Grid for the page.
            // You can adjust these values to fit your labels/paper.
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            int columns = 3; // How many barcodes to print across the page.
            int rows = 7;    // How many barcodes to print down the page.
            float cellWidth = e.MarginBounds.Width / columns;
            float cellHeight = e.MarginBounds.Height / rows;

            // 2. Setup Drawing Tools.
            Font descriptionFont = new Font("Arial", 8, FontStyle.Bold);
            Font barcodeTextFont = new Font("Arial", 7);
            SolidBrush brush = new SolidBrush(Color.Black);
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 200,  // The width of the generated barcode image.
                    Height = 40, // The height of the generated barcode image.
                    Margin = 2
                }
            };

            // 3. Loop through the grid cells on the page and draw each barcode.
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    // If we have printed all products in our list, stop.
                    if (_currentProductIndex >= _productsToPrint.Count)
                    {
                        break;
                    }

                    Product product = _productsToPrint[_currentProductIndex];

                    // Calculate the top-left corner position for the current barcode cell.
                    float xPos = leftMargin + (x * cellWidth);
                    float yPos = topMargin + (y * cellHeight);

                    // Generate the barcode image from the product's barcode string.
                    Bitmap barcodeImage = writer.Write(product.Barcode);

                    // Create a layout rectangle for this cell for better alignment.
                    RectangleF cellRect = new RectangleF(xPos, yPos, cellWidth, cellHeight);

                    // Draw Product Description (centered in the top part of the cell).
                    e.Graphics.DrawString(product.Description, descriptionFont, brush, cellRect, new StringFormat { Alignment = StringAlignment.Center });

                    // Draw Barcode Image (centered in the middle of the cell).
                    float imageX = cellRect.Left + (cellRect.Width - barcodeImage.Width) / 2;
                    float imageY = cellRect.Top + 20; // Move it down a bit from the description.
                    e.Graphics.DrawImage(barcodeImage, imageX, imageY);

                    // Draw Barcode Text (centered below the image).
                    RectangleF textRect = new RectangleF(cellRect.Left, imageY + barcodeImage.Height, cellWidth, 20);
                    e.Graphics.DrawString(product.Barcode, barcodeTextFont, brush, textRect, new StringFormat { Alignment = StringAlignment.Center });

                    // Move to the next product in our list.
                    _currentProductIndex++;
                }
                if (_currentProductIndex >= _productsToPrint.Count)
                {
                    break;
                }
            }

            // 4. Check if there are more products left to print.
            if (_currentProductIndex < _productsToPrint.Count)
            {
                e.HasMorePages = true; // If so, trigger the PrintPage event again for a new page.
            }
            else
            {
                e.HasMorePages = false; // Otherwise, this is the last page.
            }
        }

        private void BtnAddProduct_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = headerButtonHoverColor;
        }

        private void BtnAddProduct_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = headerButtonIdleColor;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (this.Tag != null && Tag is int && (int)this.Tag != -1)
            {
                int idToSelect = (int)this.Tag;
                foreach (DataGridViewRow row in dgvInventory.Rows)
                {
                    if (row.DataBoundItem is Product product && product.ProductID == idToSelect)
                    {
                        row.Selected = true;
                        dgvInventory.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        private void DgvInventory_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            string searchText = txtSearch.Text;
            if (!string.IsNullOrEmpty(searchText))
            {
                string cellText = e.Value?.ToString() ?? string.Empty;
                int startIndex = cellText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
                if (startIndex > -1)
                {
                    bool isDarkBackground = (e.CellStyle.BackColor == Color.DarkRed || e.CellStyle.BackColor == Color.LightCoral);
                    Brush highlightBrush = isDarkBackground ? Brushes.DodgerBlue : Brushes.Yellow;
                    Brush highlightTextBrush = isDarkBackground ? Brushes.White : Brushes.Black;
                    string textBeforeHighlight = cellText.Substring(0, startIndex);
                    string highlightedText = cellText.Substring(startIndex, searchText.Length);
                    float startX = e.CellBounds.Left + TextRenderer.MeasureText(e.Graphics, textBeforeHighlight, e.CellStyle.Font, e.CellBounds.Size, TextFormatFlags.NoPadding).Width;
                    float highlightWidth = TextRenderer.MeasureText(e.Graphics, highlightedText, e.CellStyle.Font, e.CellBounds.Size, TextFormatFlags.NoPadding).Width;
                    RectangleF highlightRect = new RectangleF(startX, e.CellBounds.Top, highlightWidth, e.CellBounds.Height);
                    e.Graphics.FillRectangle(highlightBrush, highlightRect);
                    e.Graphics.DrawString(highlightedText, e.CellStyle.Font, highlightTextBrush, highlightRect);
                    e.Handled = true;
                }
            }
        }

        private void DgvInventory_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = dgvInventory.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    if (!dgvInventory.Rows[hitTestInfo.RowIndex].Selected)
                    {
                        dgvInventory.ClearSelection();
                        dgvInventory.Rows[hitTestInfo.RowIndex].Selected = true;
                    }
                }
            }
        }

        private void EditProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count > 0)
            {
                DgvInventory_CellDoubleClick(sender, new DataGridViewCellEventArgs(0, dgvInventory.SelectedRows[0].Index));
            }
        }

        private void DeleteProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnDeleteProduct_Click(sender, e);
        }

        private void PrintBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnPrintBarcode_Click(sender, e);
        }

        

        private void BtnBulkEdit_Click(object sender, EventArgs e)
        {
            // Create an instance of our new form
            using (var bulkEditForm = new BulkEditForm())
            {
                // Show the form as a dialog and wait for the user to close it.
                // If the user clicks "Save Changes"...
                if (bulkEditForm.ShowDialog() == DialogResult.OK)
                {
                    // Get the list of IDs from the selected rows in the grid.
                    List<int> productIdsToUpdate = dgvInventory.SelectedRows
                                                       .Cast<DataGridViewRow>()
                                                       .Select(row => (row.DataBoundItem as Product).ProductID)
                                                       .ToList();

                    // Get the choices the user made on the bulk edit form.
                    string field = bulkEditForm.FieldToUpdate;
                    string value = bulkEditForm.NewValue;

                    try
                    {
                        // Call the repository to perform the update.
                        _repository.BulkUpdateField(productIdsToUpdate, field, value);
                        MessageBox.Show($"{productIdsToUpdate.Count} products have been updated successfully.", "Bulk Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // --- CORRECTED REFRESH LOGIC ---
                        // This is the correct method to call for your project.
                        RefreshDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred during the bulk update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        // === NEW METHOD ===
        /// <summary>
        /// Populates the transaction details panel based on a given product.
        /// </summary>
        // === NEW METHOD ===
        /// <summary>
        /// Populates the transaction details panel based on a given product.
        /// </summary>
        /// <param name="product">The product to display. If null, the panel is cleared.</param>

        // In Form1.cs
        // In Form1.cs
        // PASTE THIS COMPLETE REPLACEMENT into Form1.cs

        private async void HandleBarcodeScan()
        {
            // 1. Get the scanned barcode
            string barcode = _barcodeBuffer.ToString();
            _barcodeBuffer.Clear();

            // 2. Find the product
            Product product = _allProducts.FirstOrDefault(p => p.Barcode == barcode);

            if (product == null)
            {
                MessageBox.Show("Product not found.", "Scan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Determine if we need to pass a pre-selected supplier or customer
            int? preselectedSupplierId = null;
            string currentCustomerName = null;

            if (_currentTransactionMode == "Stock-In" && _currentSupplyOrder != null)
            {
                preselectedSupplierId = _currentSupplyOrder.SupplierId;
            }
            else if (_currentTransactionMode == "Stock-Out" && _currentSale != null)
            {
                currentCustomerName = _currentSale.CustomerName;
            }

            // 4. Open the Transaction Dialog
            using (var transactionDialog = new TransactionForm(product, allSuppliers, currentCustomerName, preselectedSupplierId))
            {
                if (transactionDialog.ShowDialog(this) == DialogResult.OK)
                {
                    int quantity = transactionDialog.SelectedQuantity;
                    string rawType = transactionDialog.TransactionType; // Get the raw text from the button click

                    // === FIX START: NORMALIZE THE NAME IMMEDIATELY ===
                    // This ensures that "Supply", "Stock-in", and "Stock-In" are all treated as the same thing.
                    string newMode = "";

                    if (rawType.Equals("Stock-In", StringComparison.OrdinalIgnoreCase) ||
                        rawType.Equals("Supply", StringComparison.OrdinalIgnoreCase))
                    {
                        newMode = "Stock-In";
                    }
                    else if (rawType.Equals("Stock-Out", StringComparison.OrdinalIgnoreCase) ||
                             rawType.Equals("Deliver", StringComparison.OrdinalIgnoreCase) ||
                             rawType.Equals("Delivery", StringComparison.OrdinalIgnoreCase))
                    {
                        newMode = "Stock-Out";
                    }
                    // === FIX END ===

                    // 5. Check if we are starting a NEW type of transaction while another is active.
                    // Now we compare 'newMode' (which is clean) against '_currentTransactionMode'
                    if (!string.IsNullOrEmpty(_currentTransactionMode) && _currentTransactionMode != newMode)
                    {
                        MessageBox.Show($"A '{_currentTransactionMode}' transaction is already in progress. Please complete or cancel it before starting a new transaction type.", "Transaction in Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 6. Set the mode to the clean version
                    _currentTransactionMode = newMode;

                    // 7. Add to the correct list
                    if (_currentTransactionMode == "Stock-In")
                    {
                        if (_currentSupplyOrder == null)
                        {
                            _currentSupplyOrder = new SupplyOrder
                            {
                                SupplierId = transactionDialog.SelectedSupplierId.Value,
                                SupplierName = allSuppliers.FirstOrDefault(s => s.SupplierID == transactionDialog.SelectedSupplierId.Value)?.Name
                            };
                        }

                        var supplyItem = new SupplyItem
                        {
                            ProductID = product.ProductID,
                            Description = product.Description,
                            Quantity = quantity,
                            PurchaseCost = product.PurchaseCost,
                            Total = quantity * product.PurchaseCost,
                            Product = product
                        };
                        _currentSupplyOrder.Items.Add(supplyItem);
                    }
                    else if (_currentTransactionMode == "Stock-Out")
                    {
                        if (_currentSale == null)
                        {
                            _currentSale = new Sale { CustomerName = transactionDialog.CustomerName };
                        }

                        var saleItem = new SaleItem
                        {
                            ProductID = product.ProductID,
                            Description = product.Description,
                            Quantity = quantity,
                            UnitPrice = product.SellingPrice,
                            Total = quantity * product.SellingPrice,
                            Product = product
                        };
                        _currentSale.Items.Add(saleItem);
                    }

                    UpdateCartPanelUI();
                }
            }
        }
        // PASTE THIS COMPLETE REPLACEMENT into Form1.cs

        // Paste this inside Form1.cs
        private void UpdateCartPanelUI()
        {
            // Reset the datasource to prevent glitches
            dgvCurrentSale.DataSource = null;

            // Logic for Stock-In
            if (_currentTransactionMode == "Stock-In" && _currentSupplyOrder != null)
            {
                pnlCurrentTransaction.Visible = true;

                // Standard Labels
                lblContextName.Text = $"Supplier: {_currentSupplyOrder.SupplierName}";
                lblDate.Text = $"Date: {DateTime.Now:yyyy-MM-dd}";
                btnConfirmTransaction.Text = "Confirm Stock-In";

                // Setup for Cost
                dgvCurrentSale.Columns["colUnitPrice"].HeaderText = "Purchase Cost";
                dgvCurrentSale.Columns["colUnitPrice"].DataPropertyName = "PurchaseCost";

                // Bind Data
                dgvCurrentSale.DataSource = _currentSupplyOrder.Items;
                lblSubtotal.Text = $"Subtotal: {_currentSupplyOrder.Subtotal:C}";
            }
            // Logic for Stock-Out
            else if (_currentTransactionMode == "Stock-Out" && _currentSale != null)
            {
                pnlCurrentTransaction.Visible = true;

                // Standard Labels
                lblContextName.Text = $"Customer: {_currentSale.CustomerName}";
                lblDate.Text = $"Date: {_currentSale.SaleDate:yyyy-MM-dd}";
                btnConfirmTransaction.Text = "Confirm Stock-Out";

                // Setup for Price
                dgvCurrentSale.Columns["colUnitPrice"].HeaderText = "Unit Price";
                dgvCurrentSale.Columns["colUnitPrice"].DataPropertyName = "UnitPrice";

                // Bind Data
                dgvCurrentSale.DataSource = _currentSale.Items;
                lblSubtotal.Text = $"Subtotal: {_currentSale.Subtotal:C}";
            }
            else
            {
                // Hide panel if nothing is happening
                pnlCurrentTransaction.Visible = false;
            }
        }
        private void UpdateSubtotal()
        {
            if (_currentSale != null)
            {
                lblSubtotal.Text = $"Subtotal: {_currentSale.Subtotal:C}";
            }
        }

        
        private async Task<Transaction> ProcessSingleTransaction(Product product, int quantity, string transactionType, int? supplierId)
        {
            int newStock;

            // CHANGE "Supply" -> "Stock-In"
            if (transactionType == "Stock-In")
            {
                newStock = product.StockQuantity + quantity;
            }
            else // It is Stock-Out (formerly Delivery)
            {
                if (quantity > product.StockQuantity)
                {
                    MessageBox.Show($"Insufficient stock for '{product.Description}'. Only {product.StockQuantity} units are available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                newStock = product.StockQuantity - quantity;
            }

            var transaction = new Transaction
            {
                ProductID = product.ProductID,
                QuantityChange = quantity,
                StockBefore = product.StockQuantity,
                StockAfter = newStock,
                TransactionType = transactionType,
                TransactionDate = DateTime.Now,
                SupplierID = supplierId
            };

            _repository.AddTransaction(transaction);
            _repository.UpdateProductStock(product.ProductID, newStock);

            lblStatus.Text = $"Success: {transactionType} of {quantity} units for '{product.Description}' processed.";
            RefreshDataGrid();

            // Visual Flash Feedback Logic
            DataGridViewRow rowToUpdate = dgvInventory.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => ((Product)r.DataBoundItem).ProductID == product.ProductID);

            if (rowToUpdate != null)
            {
                // Flash Green for Stock-In, Gold for Stock-Out
                var flashColor = (transactionType == "Stock-In") ? Color.PaleGreen : Color.Gold;
                rowToUpdate.DefaultCellStyle.BackColor = flashColor;
                await Task.Delay(1000);
                ApplyRowStyling();
            }

            return transaction;
        }

        // In Form1.cs

        // This replaces your old btnConfirmDelivery_Click
        // PASTE THIS AS A COMPLETE REPLACEMENT for your btnConfirmTransaction_Click method in Form1.cs

        // PASTE THIS COMPLETE REPLACEMENT into Form1.cs

        // PASTE THIS COMPLETE REPLACEMENT into Form1.cs

        private async void btnConfirmTransaction_Click(object sender, EventArgs e)
        {
            btnConfirmTransaction.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // CHANGE "Deliver" -> "Stock-Out"
                if (_currentTransactionMode == "Stock-Out")
                {
                    if (_currentSale == null || _currentSale.Items.Count == 0)
                    {
                        MessageBox.Show("Cart is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _repository.ProcessCompleteSale(_currentSale);

                    MessageBox.Show("Stock-Out processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // CHANGE "Supply" -> "Stock-In"
                else if (_currentTransactionMode == "Stock-In")
                {
                    if (_currentSupplyOrder == null || _currentSupplyOrder.Items.Count == 0) return;

                    foreach (var supplyItem in _currentSupplyOrder.Items)
                    {
                        await ProcessSingleTransaction(
                            supplyItem.Product,
                            supplyItem.Quantity,
                            "Stock-In", // Pass the new name here
                            _currentSupplyOrder.SupplierId);
                    }
                    MessageBox.Show("Stock-In processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                ResetTransaction();
                RefreshDataGrid();
                btnConfirmTransaction.Enabled = true;
            }
        }

        private void btnCancelTransaction_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentTransactionMode))
            {
                if (MessageBox.Show("Are you sure you want to cancel the current transaction?", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ResetTransaction();
                }
            }
        }

        // Helper method to clean up the UI
        private void ResetTransaction()
        {
            _currentSale = null;
            _currentSupplyOrder = null;
            _currentTransactionMode = null;
            pnlCurrentTransaction.Visible = false;
            dgvCurrentSale.DataSource = null;
            lblContextName.Text = "Context:";
            lblDate.Text = "Date:";
            lblSubtotal.Text = "Subtotal:";
        }
        private void DgvInventory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

            System.Diagnostics.Debug.WriteLine($"DataError in column {e.ColumnIndex}, row {e.RowIndex}: {e.Exception.Message}");

            e.ThrowException = false;
        }

        private void BtnEditSaleItem_Click(object sender, EventArgs e)
        {
            if (dgvCurrentSale.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item from the list to edit.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dgvCurrentSale.SelectedRows[0];
            int newQuantity;

            // CHANGE "Deliver" -> "Stock-Out"
            if (_currentTransactionMode == "Stock-Out" && selectedRow.DataBoundItem is SaleItem saleItem)
            {
                string newQtyStr = Interaction.InputBox($"Enter new quantity for '{saleItem.Description}':", "Edit Quantity", saleItem.Quantity.ToString());
                if (int.TryParse(newQtyStr, out newQuantity) && newQuantity > 0)
                {
                    if (newQuantity > saleItem.Product.StockQuantity)
                    {
                        MessageBox.Show($"Insufficient stock. Only {saleItem.Product.StockQuantity} units are available.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    saleItem.Quantity = newQuantity;
                    saleItem.Total = newQuantity * saleItem.UnitPrice;
                }
            }
            // CHANGE "Supply" -> "Stock-In"
            else if (_currentTransactionMode == "Stock-In" && selectedRow.DataBoundItem is SupplyItem supplyItem)
            {
                string newQtyStr = Interaction.InputBox($"Enter new quantity for '{supplyItem.Description}':", "Edit Quantity", supplyItem.Quantity.ToString());
                if (int.TryParse(newQtyStr, out newQuantity) && newQuantity > 0)
                {
                    supplyItem.Quantity = newQuantity;
                    supplyItem.Total = newQuantity * supplyItem.PurchaseCost;
                }
            }

            dgvCurrentSale.Refresh();
            UpdateCartPanelUI();
        }
    }
    
}