using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Drawing;

// The namespace stays the same
namespace InventorySystem
{
    // The class name is changed to ucTransactionHistory and it is now a UserControl, not a Form.
    public partial class ucTransactionHistory : UserControl
    {
        // --- These variables are the same ---
        private List<DashboardTransactionView> _masterTransactionList;
        private DatabaseRepository _repository;
        public event Action<int> ProductClicked;
        // --- The Constructor is Changed ---
        // A UserControl needs a simple constructor like this.
        // It now creates its own instance of the DatabaseRepository.
        public ucTransactionHistory()
        {
            InitializeComponent();
            _repository = new DatabaseRepository();
        }

        // --- PASTE THIS NEW METHOD INTO ucTransactionHistory.cs ---

        public void RefreshTransactions()
        {
            if (_repository != null)
            {
                _masterTransactionList = _repository.GetDashboardTransactions();
                ApplyFilters();
            }
        }

        // --- The Load Event is Renamed ---
        // This is the correct Load event for a UserControl.

        // --- All the methods below are exactly the same as before. No changes needed. ---

        private void ApplyFilters()
        {
            if (_masterTransactionList == null) return;

            IEnumerable<DashboardTransactionView> filteredList = _masterTransactionList;

            // Filter by Type
            if (cmbTypeFilter.SelectedItem != null && cmbTypeFilter.SelectedItem.ToString() != "All")
            {
                string selectedType = cmbTypeFilter.SelectedItem.ToString();
                filteredList = filteredList.Where(t => t.TransactionType == selectedType);
            }

            // Filter by Date
            if (chkFilterByDate.Checked)
            {
                filteredList = filteredList.Where(t => t.TransactionDate.Date == dtpDateFilter.Value.Date);
            }

            // Filter by Search Text
            string searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredList = filteredList.Where(t =>
                    (t.Barcode != null && t.Barcode.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (t.ProductDescription != null && t.ProductDescription.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (t.SupplierName != null && t.SupplierName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (t.CustomerName != null && t.CustomerName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            dgvHistory.DataSource = filteredList.ToList();
            UpdateTransactionDetails();
        }

        private void cmbTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dtpDateFilter_ValueChanged(object sender, EventArgs e)
        {
            // Only apply the filter if the checkbox is checked.
            if (chkFilterByDate.Checked)
            {
                ApplyFilters();
            }
        }

        private void UpdateTransactionDetails()
        {
            // Find the labels on the parent form
            var lblTransactionValue = this.ParentForm.Controls.Find("lblTransactionValue", true).FirstOrDefault() as Label;
            var lblDateRange = this.ParentForm.Controls.Find("lblDateRange", true).FirstOrDefault() as Label;

            if (lblTransactionValue == null || lblDateRange == null) return;

            var displayedTransactions = dgvHistory.DataSource as List<DashboardTransactionView>;
            if (displayedTransactions == null || displayedTransactions.Count == 0)
            {
                lblTransactionValue.Text = "";
                lblDateRange.Text = "No transactions to display";
                return;
            }

            // --- The calculation logic is the same ---
            decimal supplyValue = 0;
            decimal deliveryCost = 0;
            foreach (var transaction in displayedTransactions)
            {
                if (transaction.TransactionType == "Supply") { supplyValue += (transaction.PurchaseCost * transaction.QuantityChange); }
                else if (transaction.TransactionType == "Delivery") { deliveryCost += transaction.Price; }
            }

            // --- Check if columns exist before trying to access them (for safety) ---
            bool customerNameExists = dgvHistory.Columns.Contains("CustomerName");
            bool supplierIdExists = dgvHistory.Columns.Contains("SupplierID");
            bool supplierNameExists = dgvHistory.Columns.Contains("SupplierName");
            bool priceExists = dgvHistory.Columns.Contains("Price");

            // Update UI based on the current filter
            string selectedFilter = cmbTypeFilter.SelectedItem.ToString();

            if (selectedFilter == "Supply")
            {
                lblTransactionValue.Text = $"+ {supplyValue:C}";
                lblTransactionValue.ForeColor = System.Drawing.Color.Green;

                // --- NEW: Column Visibility Logic for Supply ---
                if (customerNameExists) dgvHistory.Columns["CustomerName"].Visible = false;
                if (supplierIdExists) dgvHistory.Columns["SupplierID"].Visible = true;
                if (supplierNameExists) dgvHistory.Columns["SupplierName"].Visible = true;

                if (priceExists) dgvHistory.Columns["Price"].HeaderText = "Total Cost";
            }
            else if (selectedFilter == "Delivery")
            {
                lblTransactionValue.Text = $"- {deliveryCost:C}";
                lblTransactionValue.ForeColor = System.Drawing.Color.Red;

                // --- NEW: Column Visibility Logic for Delivery ---
                if (customerNameExists) dgvHistory.Columns["CustomerName"].Visible = true;
                if (supplierIdExists) dgvHistory.Columns["SupplierID"].Visible = false;
                if (supplierNameExists) dgvHistory.Columns["SupplierName"].Visible = false;

                if (priceExists) dgvHistory.Columns["Price"].HeaderText = "Total Price";
            }
            else // "All" is selected
            {
                decimal netChange = supplyValue - deliveryCost;
                lblTransactionValue.Text = $"{netChange:C}";
                lblTransactionValue.ForeColor = netChange >= 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;

                // --- NEW: Column Visibility Logic for All ---
                if (customerNameExists) dgvHistory.Columns["CustomerName"].Visible = true;
                if (supplierIdExists) dgvHistory.Columns["SupplierID"].Visible = true;
                if (supplierNameExists) dgvHistory.Columns["SupplierName"].Visible = true;

                if (priceExists) dgvHistory.Columns["Price"].HeaderText = "Price";
            }

            // --- The date range logic is the same ---
            if (chkFilterByDate.Checked)
            {
                lblDateRange.Text = $"Showing transactions for: {dtpDateFilter.Value.ToShortDateString()}";
            }
            else
            {
                var minDate = displayedTransactions.Min(t => t.TransactionDate);
                var maxDate = displayedTransactions.Max(t => t.TransactionDate);
                lblDateRange.Text = $"Showing From: {minDate.ToShortDateString()} To: {maxDate.ToShortDateString()}";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cmbTypeFilter.SelectedIndex = 0;
            chkFilterByDate.Checked = false;
            dtpDateFilter.Value = DateTime.Now;
            txtSearch.Clear();
            ApplyFilters();
        }

        private void btnExportToCSV_Click(object sender, EventArgs e)
        {
            // First, check if there is anything to export
            if (dgvHistory.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Exit the method if the grid is empty
            }

            // Use a SaveFileDialog to ask the user where to save the file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog.Title = "Save Transaction History";
            // Suggest a unique filename with a timestamp
            saveFileDialog.FileName = $"TransactionHistory_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Use StringBuilder for better performance when building the string
                    var csvContent = new StringBuilder();

                    // Get the column headers
                    var headers = dgvHistory.Columns.Cast<DataGridViewColumn>();
                    csvContent.AppendLine(string.Join(",", headers.Select(column => $"\"{column.HeaderText}\"").ToArray()));

                    // Loop through each row in the DataGridView
                    foreach (DataGridViewRow row in dgvHistory.Rows)
                    {
                        // Get the cell values for the current row
                        var cells = row.Cells.Cast<DataGridViewCell>();
                        // Enclose each value in quotes to handle any commas within the data
                        csvContent.AppendLine(string.Join(",", cells.Select(cell => $"\"{cell.Value}\"").ToArray()));
                    }

                    // Write the complete string to the selected file
                    File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());

                    MessageBox.Show("Data exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void chkFilterByDate_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ucTransactionHistory_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.ReadOnly = true;


            // The rest of your code remains the same.
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _masterTransactionList = _repository.GetDashboardTransactions();

            cmbTypeFilter.Items.Add("All");
            cmbTypeFilter.Items.Add("Supply");
            cmbTypeFilter.Items.Add("Delivery");
            cmbTypeFilter.SelectedIndex = 0;

            ApplyFilters();
        }

        private void dgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Safety check to prevent errors with new or header rows
            if (e.RowIndex < 0 || dgvHistory.Rows[e.RowIndex].DataBoundItem == null)
                return;

            // Get the full transaction object for the current row
            var transaction = dgvHistory.Rows[e.RowIndex].DataBoundItem as DashboardTransactionView;

            if (transaction == null)
                return;

            // Get the name of the column we are currently formatting
            string columnName = dgvHistory.Columns[e.ColumnIndex].Name;

            // --- LOGIC FOR TRANSACTION TYPE COLUMN ---
            // Assumes your column's (Name) property is "TransactionType"
            if (columnName.Equals("TransactionType"))
            {
                if (transaction.TransactionType == "Delivery")
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
                else if (transaction.TransactionType == "Supply")
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
            }

            // --- LOGIC FOR PRICE COLUMN ---
            // Assumes your column's (Name) property is "Price"
            if (columnName.Equals("Price"))
            {
                if (transaction.TransactionType == "Delivery")
                {
                    decimal priceValue = transaction.Price;
                    e.Value = $"- {priceValue:C}"; // Format as "- $X.XX"
                    e.CellStyle.ForeColor = Color.Red;
                    e.FormattingApplied = true; // Tell the grid we've handled it
                }
                else if (transaction.TransactionType == "Supply")
                {
                    // For Supply, the value is the PurchaseCost * Quantity
                    decimal totalCost = transaction.PurchaseCost * transaction.QuantityChange;
                    e.Value = $"+ {totalCost:C}"; // Format as "+ $X.XX"
                    e.CellStyle.ForeColor = Color.Green;
                    e.FormattingApplied = true; // Tell the grid we've handled it
                }
            }
        }

        // In ucTransactionHistory.cs

        private void dgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the user clicked a valid row, not the header
            if (e.RowIndex < 0) return;

            // Get the transaction object bound to the clicked row
            if (dgvHistory.Rows[e.RowIndex].DataBoundItem is DashboardTransactionView clickedTransaction)
            {
                // Get the ProductID from the object
                int productId = clickedTransaction.ProductID;

                // Fire the event, passing the ProductID to the parent form (DashboardForm)
                ProductClicked?.Invoke(productId);
            }
        }
    }
}