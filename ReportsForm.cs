using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Linq;
using InventoryManagementSystem;

namespace InventorySystem
{
    public partial class ReportsForm : Form
    {
        private const string Format = "yyyy-MM-dd HH:mm";

        // --- CLASS-LEVEL VARIABLES ---
        private readonly DatabaseRepository _repository;
        private DataGridView _dgvToPrint;
        private float[] _columnWidths;
        private int _currentRowIndex;

        // Master lists
        private List<InventoryReportItem> _masterInventoryList;
        private List<Transaction> _masterTransactionList;

        public static string Format1 => Format;

        public ReportsForm(DatabaseRepository repository)
        {
            InitializeComponent();
            this.dgvInventoryStatus.AutoGenerateColumns = false;
            this.dgvTransactionHistory.AutoGenerateColumns = false;
            _repository = repository;
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            // Set up initial state
            dtpEndDate.Value = DateTime.Now;
            dtpStartDate.Value = DateTime.Now.AddMonths(-1);

            // Load data
            LoadInventoryStatusReport();
            LoadTransactionHistoryReport();

            // Populate filters
            PopulateInventoryFilters();
            PopulateTransactionTypeFilter();

            // Generate initial Sales Report
            GenerateSalesReport();

            // Update UI
            UpdateFooterControls();

            // === FIX 1: Manually Wire up the Date Events to the New Logic ===
            // We do this here to ensure they handle BOTH tabs correctly
            dtpStartDate.ValueChanged += HandleDateChange;
            dtpEndDate.ValueChanged += HandleDateChange;
        }

        // === FIX 1: Central Date Handler ===
        private void HandleDateChange(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpSalesReport)
            {
                GenerateSalesReport();
            }
            else if (tabControl1.SelectedTab == tabPageTransactionHistory)
            {
                ApplyTransactionFilters();
            }
        }

        private void LoadTransactionHistoryReport()
        {
            _masterTransactionList = _repository.GetAllTransactions();
            dgvTransactionHistory.DataSource = _masterTransactionList;
        }

        private void LoadInventoryStatusReport()
        {
            var products = _repository.GetAllProducts();
            _masterInventoryList = products.Select(p => new InventoryReportItem
            {
                PartNumber = p.PartNumber,
                Brand = p.Brand,
                Type = p.Type,
                Description = p.Description,
                StockQuantity = p.StockQuantity,
                PurchaseCost = p.PurchaseCost,
                TotalValue = p.StockQuantity * p.PurchaseCost
            }).ToList();

            ApplyInventoryFilters();
        }

        private void PopulateInventoryFilters()
        {
            if (_masterInventoryList == null) return;
            var brands = _masterInventoryList.Select(p => p.Brand).Where(b => !string.IsNullOrEmpty(b)).Distinct().OrderBy(b => b).ToList();
            brands.Insert(0, "All Brands");
            cmbInventoryBrand.DataSource = brands;

            var types = _masterInventoryList.Select(p => p.Type).Where(t => !string.IsNullOrEmpty(t)).Distinct().OrderBy(t => t).ToList();
            types.Insert(0, "All Types");
            cmbInventoryType.DataSource = types;
        }

        private void PopulateTransactionTypeFilter()
        {
            cmbTransactionType.Items.Clear();
            cmbTransactionType.Items.Add("All");
            cmbTransactionType.Items.Add("Supply");
            cmbTransactionType.Items.Add("Delivery");
            cmbTransactionType.SelectedIndex = 0;
        }

        private void ApplyInventoryFilters()
        {
            if (_masterInventoryList == null) return;
            IEnumerable<InventoryReportItem> filteredData = _masterInventoryList;

            if (cmbInventoryBrand.SelectedItem != null && cmbInventoryBrand.SelectedItem.ToString() != "All Brands")
            {
                filteredData = filteredData.Where(p => p.Brand == cmbInventoryBrand.SelectedItem.ToString());
            }
            if (cmbInventoryType.SelectedItem != null && cmbInventoryType.SelectedItem.ToString() != "All Types")
            {
                filteredData = filteredData.Where(p => p.Type == cmbInventoryType.SelectedItem.ToString());
            }
            string searchText = txtInventorySearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredData = filteredData.Where(p =>
                    (p.PartNumber?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.Description?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.Brand?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }
            dgvInventoryStatus.DataSource = filteredData.ToList();
        }

        private void ApplyTransactionFilters()
        {
            if (_masterTransactionList == null) return;

            IEnumerable<Transaction> filteredData = _masterTransactionList;

            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date.AddDays(1).AddTicks(-1);
            filteredData = filteredData.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);

            if (cmbTransactionType.SelectedItem != null && cmbTransactionType.SelectedItem.ToString() != "All")
            {
                filteredData = filteredData.Where(t => t.TransactionType == cmbTransactionType.SelectedItem.ToString());
            }

            string searchText = txtTransactionSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredData = filteredData.Where(t =>
                    t.ProductDescription?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                );
            }

            dgvTransactionHistory.DataSource = filteredData.ToList();
        }

        // --- UI EVENT HANDLERS ---

        private void BtnBackToDashboard_Click(object sender, EventArgs e)
        {
            this.Owner?.Show();
            this.Close();
        }

        // NOTE: The BtnFilterTransactions_Click method has been removed entirely.

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFooterControls();

            // Optional: Immediately refresh the data when switching tabs to match the dates
            if (tabControl1.SelectedTab == tpSalesReport) GenerateSalesReport();
            else if (tabControl1.SelectedTab == tabPageTransactionHistory) ApplyTransactionFilters();
        }

        // Event handlers for INVENTORY tab
        private void CmbInventoryBrand_SelectedIndexChanged(object sender, EventArgs e) => ApplyInventoryFilters();
        private void CmbInventoryType_SelectedIndexChanged(object sender, EventArgs e) => ApplyInventoryFilters();
        private void TxtInventorySearch_TextChanged(object sender, EventArgs e) => ApplyInventoryFilters();

        // Event handlers for TRANSACTION HISTORY tab
        // Note: DtpStartDate/EndDate are now handled by HandleDateChange in Form_Load
        private void CmbTransactionType_SelectedIndexChanged(object sender, EventArgs e) => ApplyTransactionFilters();
        private void TxtTransactionSearch_TextChanged(object sender, EventArgs e) => ApplyTransactionFilters();

        // --- FINANCIAL & PRINTING LOGIC ---

        private void GenerateSalesReport()
        {
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date.AddDays(1).AddTicks(-1);
            SalesReportData reportData = _repository.GetSalesReport(startDate, endDate);

            // === FIX 2: FORCE POSITIVE NUMBERS USING MATH.ABS ===

            // Revenue and COGS
            decimal revenue = Math.Abs(reportData.TotalRevenue);
            decimal cogs = Math.Abs(reportData.CostOfGoodsSold);
            decimal grossProfit = revenue - cogs;

            lblTotalRevenue.Text = revenue.ToString("C");
            lblCostOfGoodsSold.Text = cogs.ToString("C");
            lblGrossProfit.Text = grossProfit.ToString("C");

            // Margins
            decimal margin = (revenue > 0) ? (grossProfit / revenue) : 0;
            lblGrossProfitMargin.Text = margin.ToString("P");

            // Counts
            lblTotalTransactions.Text = reportData.TotalTransactions.ToString();
            lblTotalItemsSold.Text = Math.Abs(reportData.TotalItemsSold).ToString(); // Fix negative item count

            // Best Seller (Fix negative units)
            int bestSellerUnits = (int)Math.Abs(reportData.BestSellingProduct.Value);
            lblBestSellingProduct.Text = !string.IsNullOrEmpty(reportData.BestSellingProduct.Name)
                ? $"{reportData.BestSellingProduct.Name} ({bestSellerUnits} units)"
                : "N/A (0 units)";

            // Most Profitable (Fix negative value if calculated from negative quantity)
            decimal profitableVal = Math.Abs(reportData.MostProfitableProduct.Value);
            lblMostProfitableProduct.Text = !string.IsNullOrEmpty(reportData.MostProfitableProduct.Name)
                ? $"{reportData.MostProfitableProduct.Name} ({profitableVal:C})"
                : "N/A ($0.00)";

            lblReportPeriod.Text = $"Reporting for {startDate.ToShortDateString()} to {endDate.ToShortDateString()}";
        }

        private void BtnPrintReport_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpSalesReport)
            {
                try
                {
                    using (PrintDialog printDialog = new PrintDialog())
                    {
                        printDialog.Document = printDocument1;
                        if (printDialog.ShowDialog() == DialogResult.OK)
                        {
                            printDocument1.DocumentName = "Sales_and_Profitability_Report";
                            printDocument1.DefaultPageSettings.Landscape = false;
                            _dgvToPrint = null;
                            printDocument1.Print();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error printing the sales report: " + ex.Message, "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            _dgvToPrint = (tabControl1.SelectedTab == tabPageInventoryStatus) ? dgvInventoryStatus : dgvTransactionHistory;

            if (_dgvToPrint.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to print.", "Print Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (tabControl1.SelectedTab == tabPageInventoryStatus)
            {
                _columnWidths = new float[] { 0.15f, 0.15f, 0.30f, 0.10f, 0.15f, 0.15f };
            }
            else
            {
                _columnWidths = new float[] { 0.08f, 0.08f, 0.12f, 0.22f, 0.08f, 0.08f, 0.08f, 0.08f, 0.10f, 0.08f };
            }

            try
            {
                using (PrintDialog printDialog = new PrintDialog())
                {
                    printDialog.Document = printDocument1;
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        printDocument1.DocumentName = (_dgvToPrint == dgvInventoryStatus) ? "Inventory_Status_Report" : "Transaction_History_Report";
                        printDocument1.DefaultPageSettings.Landscape = (_dgvToPrint == dgvTransactionHistory);
                        printDocument1.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error printing the grid report: " + ex.Message, "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
            _currentRowIndex = 0;
        }

        private void PrintDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_dgvToPrint == null)
            {
                Graphics g = e.Graphics;
                float yPos = e.MarginBounds.Top;
                Font titleFont = new Font("Segoe UI", 16, FontStyle.Bold);
                Font headerFont = new Font("Segoe UI", 12, FontStyle.Bold);
                Font bodyFont = new Font("Segoe UI", 10);
                float leftMargin = e.MarginBounds.Left;

                g.DrawString("Sales & Profitability Report", titleFont, Brushes.Black, leftMargin, yPos);
                yPos += titleFont.GetHeight(g) + 10;
                g.DrawString(lblReportPeriod.Text, bodyFont, Brushes.Black, leftMargin, yPos);
                yPos += bodyFont.GetHeight(g) * 2;
                g.DrawString("Financial Summary", headerFont, Brushes.Black, leftMargin, yPos);
                yPos += headerFont.GetHeight(g) + 8;
                yPos = DrawReportLine(g, "Total Revenue:", lblTotalRevenue.Text, bodyFont, leftMargin, yPos);
                yPos = DrawReportLine(g, "Cost of Goods Sold:", lblCostOfGoodsSold.Text, bodyFont, leftMargin, yPos);
                yPos = DrawReportLine(g, "Gross Profit:", lblGrossProfit.Text, bodyFont, leftMargin, yPos);
                yPos += 25;
                g.DrawString("Key Metrics", headerFont, Brushes.Black, leftMargin, yPos);
                yPos += headerFont.GetHeight(g) + 8;
                yPos = DrawReportLine(g, "Gross Profit Margin:", lblGrossProfitMargin.Text, bodyFont, leftMargin, yPos);
                yPos = DrawReportLine(g, "Total Transactions:", lblTotalTransactions.Text, bodyFont, leftMargin, yPos);
                yPos = DrawReportLine(g, "Total Items Sold:", lblTotalItemsSold.Text, bodyFont, leftMargin, yPos);
                yPos += 25;
                g.DrawString("Top Performers", headerFont, Brushes.Black, leftMargin, yPos);
                yPos += headerFont.GetHeight(g) + 8;
                yPos = DrawReportLine(g, "Best-Selling Product:", lblBestSellingProduct.Text, bodyFont, leftMargin, yPos);
                DrawReportLine(g, "Most Profitable Product:", lblMostProfitableProduct.Text, bodyFont, leftMargin, yPos);

                e.HasMorePages = false;
                return;
            }

            Graphics gridGraphics = e.Graphics;
            float gridYPos = e.MarginBounds.Top;
            Font gridHeaderFont = new Font("Segoe UI", 9, FontStyle.Bold);
            Font gridBodyFont = new Font("Segoe UI", 9);
            StringFormat stringFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.Word };

            float maxHeaderHeight = 0;
            for (int i = 0; i < _dgvToPrint.Columns.Count; i++)
            {
                if (!_dgvToPrint.Columns[i].Visible) continue;
                float colWidth = e.MarginBounds.Width * _columnWidths[i];
                string headerText = _dgvToPrint.Columns[i].HeaderText;
                SizeF headerSize = gridGraphics.MeasureString(headerText, gridHeaderFont, (int)colWidth);
                if (headerSize.Height > maxHeaderHeight) maxHeaderHeight = headerSize.Height;
            }
            maxHeaderHeight += 10;

            float currentLeftMargin = e.MarginBounds.Left;
            for (int i = 0; i < _dgvToPrint.Columns.Count; i++)
            {
                if (!_dgvToPrint.Columns[i].Visible) continue;
                float colWidth = e.MarginBounds.Width * _columnWidths[i];
                RectangleF headerBounds = new RectangleF(currentLeftMargin, gridYPos, colWidth, maxHeaderHeight);
                gridGraphics.DrawString(_dgvToPrint.Columns[i].HeaderText, gridHeaderFont, Brushes.Black, headerBounds, stringFormat);
                currentLeftMargin += colWidth;
            }
            gridYPos += maxHeaderHeight;

            while (_currentRowIndex < _dgvToPrint.Rows.Count)
            {
                DataGridViewRow row = _dgvToPrint.Rows[_currentRowIndex];
                float currentRowHeight = 0;
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (!_dgvToPrint.Columns[i].Visible) continue;
                    float colWidth = e.MarginBounds.Width * _columnWidths[i];
                    string cellText = FormatCellText(row.Cells[i]);
                    SizeF cellSize = gridGraphics.MeasureString(cellText, gridBodyFont, (int)colWidth);
                    if (cellSize.Height > currentRowHeight) currentRowHeight = cellSize.Height;
                }
                currentRowHeight += 5;

                if (gridYPos + currentRowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                currentLeftMargin = e.MarginBounds.Left;
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (!_dgvToPrint.Columns[i].Visible) continue;
                    float colWidth = e.MarginBounds.Width * _columnWidths[i];
                    string cellText = FormatCellText(row.Cells[i]);
                    RectangleF cellBounds = new RectangleF(currentLeftMargin, gridYPos, colWidth, currentRowHeight);
                    gridGraphics.DrawString(cellText, gridBodyFont, Brushes.Black, cellBounds, stringFormat);
                    currentLeftMargin += colWidth;
                }

                gridYPos += currentRowHeight;
                _currentRowIndex++;
            }
            e.HasMorePages = false;
        }

        private void UpdateFooterControls()
        {
            dtpStartDate.Visible = false;
            dtpEndDate.Visible = false;
            // The button logic has been removed from here

            lblInventoryBrand.Visible = false;
            cmbInventoryBrand.Visible = false;
            lblInventoryType.Visible = false;
            cmbInventoryType.Visible = false;
            lblInventorySearch.Visible = false;
            txtInventorySearch.Visible = false;
            lblTransactionType.Visible = false;
            cmbTransactionType.Visible = false;
            lblTransactionSearch.Visible = false;
            txtTransactionSearch.Visible = false;
            btnPrintReport.Visible = true;

            if (tabControl1.SelectedTab == tabPageInventoryStatus)
            {
                lblInventoryBrand.Visible = true;
                cmbInventoryBrand.Visible = true;
                lblInventoryType.Visible = true;
                cmbInventoryType.Visible = true;
                lblInventorySearch.Visible = true;
                txtInventorySearch.Visible = true;
            }
            else if (tabControl1.SelectedTab == tabPageTransactionHistory)
            {
                dtpStartDate.Visible = true;
                dtpEndDate.Visible = true;
                lblTransactionType.Visible = true;
                cmbTransactionType.Visible = true;
                lblTransactionSearch.Visible = true;
                txtTransactionSearch.Visible = true;
            }
            else if (tabControl1.SelectedTab == tpSalesReport)
            {
                dtpStartDate.Visible = true;
                dtpEndDate.Visible = true;
                // No button needed
            }
        }

        private float DrawReportLine(Graphics g, string label, string value, Font font, float x, float y)
        {
            float labelColumnWidth = 220;
            g.DrawString(label, new Font(font, FontStyle.Bold), Brushes.Black, x, y);
            g.DrawString(value, font, Brushes.Black, x + labelColumnWidth, y);
            return y + font.GetHeight(g) + 8;
        }

        private string FormatCellText(DataGridViewCell cell)
        {
            if (cell.Value == null) return "N/A";
            string cellText = cell.Value.ToString();

            if (cell.OwningColumn.DefaultCellStyle.Format == "c" && decimal.TryParse(cell.Value.ToString(), out decimal val))
            {
                return val.ToString("C");
            }
            if (cell.Value is DateTime date)
            {
                return date.ToString(Format1);
            }
            return cellText;
        }
    }
}