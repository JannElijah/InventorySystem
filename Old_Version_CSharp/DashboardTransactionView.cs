// In DashboardTransactionView.cs

using System;

namespace InventorySystem
{
    public class DashboardTransactionView
    {
        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public string Barcode { get; set; }
        public string ProductDescription { get; set; }
        public string TransactionType { get; set; }
        public decimal Price { get; set; }
        public int StockBefore { get; set; }
        public int StockAfter { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string CustomerName { get; set; }

        // --- ADD THESE TWO LINES ---
        public int QuantityChange { get; set; }
        public decimal PurchaseCost { get; set; }
    }
}