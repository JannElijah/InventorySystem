using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Drawing;
using System.Globalization; // Needed for Peso formatting

namespace InventorySystem
{
    public partial class ucTransactionHistory : UserControl
    {
        private List<DashboardTransactionView> _masterTransactionList;
        private readonly DatabaseRepository _repository;
        public event Action<int> ProductClicked;

        // Create specific culture info for Philippines to force ₱ symbol
        private readonly CultureInfo phCulture = new CultureInfo("en-PH");

        public ucTransactionHistory()
        {
            InitializeComponent();
            _repository = new DatabaseRepository();
        }

        // === 1. FORCE THE GRID TO CREATE THE CORRECT COLUMNS ===
        private void SetupColumns()
        {
            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.Columns.Clear();

            // Define columns programmatically to ensure they match the data exactly
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TransactionID",
                HeaderText = "ID",
                Width = 50
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Barcode",
                HeaderText = "Barcode",
                Width = 100
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductDescription",
                HeaderText = "Product Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Fill remaining space
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TransactionType",
                HeaderText = "Type",
                Width = 80,
                Name = "TransactionType" // Name needed for Formatting logic
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Value",
                Width = 100,
                Name = "Price" // Name needed for Formatting logic
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "QuantityChange",
                HeaderText = "Qty",
                Width = 60
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CustomerName",
                HeaderText = "Customer",
                Width = 120
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SupplierName",
                HeaderText = "Supplier",
                Width = 120
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TransactionDate",
                HeaderText = "Date",
                Width = 140
            });

            // Format the Date Column immediately
            dgvHistory.Columns[dgvHistory.Columns.Count - 1].DefaultCellStyle.Format = "g";
        }

        private void ucTransactionHistory_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            // Call the setup method to fix the gray/empty grid issue
            SetupColumns();

            // Populate filters
            cmbTypeFilter.Items.Clear();
            cmbTypeFilter.Items.Add("All");
            cmbTypeFilter.Items.Add("Supply");
            cmbTypeFilter.Items.Add("Delivery");
            cmbTypeFilter.SelectedIndex = 0;

            RefreshTransactions();
            txtSearch.TextChanged -= txtSearch_TextChanged; // Prevent double-clicking
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        public void RefreshTransactions()
        {
            if (_repository != null)
            {
                try
                {
                    _masterTransactionList = _repository.GetDashboardTransactions();
                    ApplyFilters();
                }
                catch (Exception ex)
                {
                    // Fail silently in UI but log if possible, helps prevent crash on load
                    System.Diagnostics.Debug.WriteLine("Error refreshing history: " + ex.Message);
                }
            }
        }

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

            // Filter by Date (Only if checked)
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

        private void cmbTypeFilter_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();

        private void dtpDateFilter_ValueChanged(object sender, EventArgs e)
        {
            if (chkFilterByDate.Checked) ApplyFilters();
        }

        private void chkFilterByDate_CheckedChanged(object sender, EventArgs e) => ApplyFilters();

        private void txtSearch_TextChanged(object sender, EventArgs e) => ApplyFilters();

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cmbTypeFilter.SelectedIndex = 0;
            chkFilterByDate.Checked = false;
            dtpDateFilter.Value = DateTime.Now;
            txtSearch.Clear();
            ApplyFilters();
        }

        private void UpdateTransactionDetails()
        {
            // Helper to update the summary labels on the Dashboard
            var lblTransactionValue = this.ParentForm?.Controls.Find("lblTransactionValue", true).FirstOrDefault() as Label;
            var lblDateRange = this.ParentForm?.Controls.Find("lblDateRange", true).FirstOrDefault() as Label;

            if (lblTransactionValue == null || lblDateRange == null) return;

            var displayedTransactions = dgvHistory.DataSource as List<DashboardTransactionView>;
            if (displayedTransactions == null || displayedTransactions.Count == 0)
            {
                lblTransactionValue.Text = "₱0.00";
                lblDateRange.Text = "No transactions";
                return;
            }

            decimal totalValue = 0;
            foreach (var t in displayedTransactions)
            {
                // Supply is Cost (Negative cash flow or Asset addition), Delivery is Revenue
                if (t.TransactionType == "Delivery") totalValue += t.Price;
                else if (t.TransactionType == "Supply") totalValue -= (t.PurchaseCost * t.QuantityChange);
            }

            // Just showing the net sum of displayed items for now
            lblTransactionValue.Text = totalValue.ToString("C", phCulture);
            lblTransactionValue.ForeColor = totalValue >= 0 ? Color.Green : Color.Red;

            if (chkFilterByDate.Checked)
                lblDateRange.Text = $"Date: {dtpDateFilter.Value:MMM dd, yyyy}";
            else
                lblDateRange.Text = "Showing All";
        }

        private void dgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvHistory.Rows[e.RowIndex].DataBoundItem == null) return;

            var transaction = dgvHistory.Rows[e.RowIndex].DataBoundItem as DashboardTransactionView;
            if (transaction == null) return;

            string columnName = dgvHistory.Columns[e.ColumnIndex].Name;

            // Color Code the Type
            if (columnName == "TransactionType")
            {
                if (transaction.TransactionType == "Delivery") e.CellStyle.ForeColor = Color.Red;
                else if (transaction.TransactionType == "Supply") e.CellStyle.ForeColor = Color.Green;
            }

            // Format the Price with Peso sign
            if (columnName == "Price")
            {
                if (transaction.TransactionType == "Delivery")
                {
                    e.Value = $"+ {transaction.Price.ToString("C", phCulture)}";
                    e.CellStyle.ForeColor = Color.Green; // Money coming in
                }
                else if (transaction.TransactionType == "Supply")
                {
                    decimal cost = transaction.PurchaseCost * transaction.QuantityChange;
                    e.Value = $"- {cost.ToString("C", phCulture)}";
                    e.CellStyle.ForeColor = Color.Red; // Money going out
                }
                e.FormattingApplied = true;
            }
        }

        private void dgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvHistory.Rows[e.RowIndex].DataBoundItem is DashboardTransactionView clickedTransaction)
            {
                ProductClicked?.Invoke(clickedTransaction.ProductID);
            }
        }

        private void btnExportToCSV_Click(object sender, EventArgs e)
        {
            if (dgvHistory.Rows.Count == 0) { MessageBox.Show("No data to export."); return; }

            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = $"Transactions_{DateTime.Now:yyyyMMdd}.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder csv = new StringBuilder();
                    // Headers
                    csv.AppendLine(string.Join(",", dgvHistory.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText)));
                    // Rows
                    foreach (DataGridViewRow row in dgvHistory.Rows)
                    {
                        var cells = row.Cells.Cast<DataGridViewCell>().Select(c => $"\"{c.Value}\"");
                        csv.AppendLine(string.Join(",", cells));
                    }
                    File.WriteAllText(sfd.FileName, csv.ToString());
                    MessageBox.Show("Export Successful!");
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }
    }
}