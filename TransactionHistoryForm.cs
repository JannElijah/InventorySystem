using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InventorySystem
{
    /// <summary>
    /// A form for viewing, filtering, searching, and exporting historical transaction data.
    /// </summary>
    public partial class TransactionHistoryForm : Form
    {
        private readonly DatabaseRepository _repository;
        private List<Transaction> _masterTransactionList;
        private readonly Timer _searchDebounceTimer; // --- IMPROVEMENT: Timer for responsive search ---

        #region Initialization

        public TransactionHistoryForm(DatabaseRepository repository)
        {
            InitializeComponent();
            _repository = repository;

            // --- IMPROVEMENT: Initialize and configure the search timer ---
            _searchDebounceTimer = new Timer();
            _searchDebounceTimer.Interval = 300; // 300ms delay
            _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
        }

        private void TransactionHistoryForm_Load(object sender, EventArgs e)
        {
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadMasterDataAndSetupFilters();
        }

        private void LoadMasterDataAndSetupFilters()
        {
            _masterTransactionList = _repository.GetAllTransactions();

            cmbTypeFilter.Items.AddRange(new object[] { "All", "Supply", "Delivery" });
            cmbTypeFilter.SelectedIndex = 0;

            ApplyFilters();
        }

        #endregion

        #region Filtering Logic

        private void ApplyFilters()
        {
            IEnumerable<Transaction> filteredTransactions = _masterTransactionList;

            if (cmbTypeFilter.SelectedItem?.ToString() != "All")
            {
                filteredTransactions = filteredTransactions.Where(t => t.TransactionType == cmbTypeFilter.SelectedItem.ToString());
            }

            if (chkFilterByDate.Checked)
            {
                filteredTransactions = filteredTransactions.Where(t => t.TransactionDate.Date == dtpDateFilter.Value.Date);
            }

            string searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredTransactions = filteredTransactions.Where(t =>
                    // --- IMPROVEMENT: Expanded search to include SupplierName ---
                    (t.Barcode?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (t.ProductDescription?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (t.SupplierName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            dgvHistory.DataSource = null; // Ensures a clean refresh
            dgvHistory.DataSource = filteredTransactions.ToList();
        }

        #endregion

        #region UI Event Handlers

        private void cmbTypeFilter_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void dtpDateFilter_ValueChanged(object sender, EventArgs e) => ApplyFilters();
        private void chkFilterByDate_CheckedChanged(object sender, EventArgs e) => ApplyFilters();

        // --- IMPROVEMENT: Debounced search for performance ---
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Instead of filtering on every keystroke, restart the timer.
            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        private void SearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            // The timer has ticked, meaning the user has stopped typing. Now we filter.
            _searchDebounceTimer.Stop();
            ApplyFilters();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cmbTypeFilter.SelectedIndex = 0;
            chkFilterByDate.Checked = false;
            dtpDateFilter.Value = DateTime.Now;
            txtSearch.Clear();

            // --- IMPROVEMENT: Explicit call for clarity ---
            // An explicit call is clearer than relying on the CheckChanged event side effect.
            ApplyFilters();
        }

        private void btnExportToCSV_Click(object sender, EventArgs e)
        {
            if (dgvHistory.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV File (*.csv)|*.csv";
                saveFileDialog.Title = "Save Transaction History";
                saveFileDialog.FileName = $"TransactionHistory_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var csvBuilder = new StringBuilder();

                        var headerNames = dgvHistory.Columns.Cast<DataGridViewColumn>()
                            .Select(column => EscapeCsvField(column.HeaderText));
                        csvBuilder.AppendLine(string.Join(",", headerNames));

                        foreach (DataGridViewRow row in dgvHistory.Rows)
                        {
                            var cellValues = row.Cells.Cast<DataGridViewCell>()
                                .Select(cell => EscapeCsvField(FormatCellForCsv(cell))); // Use helper methods
                            csvBuilder.AppendLine(string.Join(",", cellValues));
                        }

                        File.WriteAllText(saveFileDialog.FileName, csvBuilder.ToString());
                        MessageBox.Show("Data exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while exporting the data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// --- IMPROVEMENT: Formats cell data for clean CSV output ---
        /// Formats special data types like dates into a consistent, readable string.
        /// </summary>
        private string FormatCellForCsv(DataGridViewCell cell)
        {
            if (cell.Value is DateTime date)
            {
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return cell.Value?.ToString() ?? "";
        }

        /// <summary>
        /// --- IMPROVEMENT: Robustly escapes fields for CSV format ---
        /// Wraps the text in quotes and escapes any internal quotes.
        /// </summary>
        private string EscapeCsvField(string text)
        {
            if (text == null) return "\"\"";
            // Replace any quote with a double quote, then wrap the whole thing in quotes.
            return $"\"{text.Replace("\"", "\"\"")}\"";
        }

        #endregion
    }
}