using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Drawing;
using System.Globalization;

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

        // === 1. SETUP COLUMNS ===
        private void SetupColumns()
        {
            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.Columns.Clear();

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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TransactionType",
                HeaderText = "Type",
                Width = 100,
                Name = "TransactionType"
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Value",
                Width = 100,
                Name = "Price"
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

            SetupColumns();

            // === WIRE UP THE DATA ERROR HANDLER TO STOP POPUPS ===
            dgvHistory.DataError += DgvHistory_DataError;

            // === UPDATED FILTERS ===
            cmbTypeFilter.Items.Clear();
            cmbTypeFilter.Items.Add("All");
            cmbTypeFilter.Items.Add("Stock-In");  // Fixed Capitalization
            cmbTypeFilter.Items.Add("Stock-Out"); // Fixed Capitalization
            cmbTypeFilter.SelectedIndex = 0;

            RefreshTransactions();
            txtSearch.TextChanged -= txtSearch_TextChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        // === NEW METHOD: SUPPRESS ERRORS ===
        private void DgvHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // This stops the "System.FormatException" popup.
            // We just ignore the error because we are handling the display manually in CellFormatting.
            e.ThrowException = false;
            e.Cancel = true;
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

                // === FIX: Case Insensitive Check ===
                if (selectedType.Equals("Stock-In", StringComparison.OrdinalIgnoreCase))
                {
                    filteredList = filteredList.Where(t => t.TransactionType.Equals("Stock-In", StringComparison.OrdinalIgnoreCase) || t.TransactionType == "Supply");
                }
                else if (selectedType.Equals("Stock-Out", StringComparison.OrdinalIgnoreCase))
                {
                    filteredList = filteredList.Where(t => t.TransactionType.Equals("Stock-Out", StringComparison.OrdinalIgnoreCase) || t.TransactionType == "Delivery");
                }
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
                // === FIX: Case Insensitive Checks ===
                if (t.TransactionType.Equals("Stock-Out", StringComparison.OrdinalIgnoreCase) || t.TransactionType == "Delivery")
                {
                    totalValue += t.Price;
                }
                else if (t.TransactionType.Equals("Stock-In", StringComparison.OrdinalIgnoreCase) || t.TransactionType == "Supply")
                {
                    totalValue -= (t.PurchaseCost * t.QuantityChange);
                }
            }

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
            string type = transaction.TransactionType;

            // === FIX: Robust String Checking (Handles "Stock-In", "Stock-in", "Stock-IN") ===
            bool isStockOut = type.Equals("Stock-Out", StringComparison.OrdinalIgnoreCase) || type.Equals("Delivery", StringComparison.OrdinalIgnoreCase);
            bool isStockIn = type.Equals("Stock-In", StringComparison.OrdinalIgnoreCase) || type.Equals("Supply", StringComparison.OrdinalIgnoreCase);

            // === COLOR CODE THE TYPE ===
            if (columnName == "TransactionType")
            {
                if (isStockOut)
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.Value = "Stock-Out"; // Force nice casing
                }
                else if (isStockIn)
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.Value = "Stock-In"; // Force nice casing
                }
            }

            // === FORMAT THE PRICE ===
            if (columnName == "Price")
            {
                if (isStockOut)
                {
                    e.Value = $"+ {transaction.Price.ToString("C", phCulture)}";
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (isStockIn)
                {
                    decimal cost = transaction.PurchaseCost * transaction.QuantityChange;
                    e.Value = $"- {cost.ToString("C", phCulture)}";
                    e.CellStyle.ForeColor = Color.Red;
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
                    csv.AppendLine(string.Join(",", dgvHistory.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText)));
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