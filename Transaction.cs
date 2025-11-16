using System;

namespace InventorySystem
{
    public class Transaction
    {
        // --- Your Existing Properties ---
        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public string Barcode { get; set; }
        public string ProductDescription { get; set; }
        public string TransactionType { get; set; }
        public int QuantityChange { get; set; }
        public int StockBefore { get; set; }
        public int StockAfter { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? SaleItemID { get; set; }
        // --- NEW PROPERTIES TO ADD ---

        /// <summary>
        /// Stores the ID of the supplier for a "Supply" transaction.
        /// Will be NULL for "Delivery" transactions.
        /// </summary>
        public int? SupplierID { get; set; }

        /// <summary>
        /// A helper property to hold the supplier's name for display in the history grid.
        /// This is not stored in the Transactions table itself.
        /// </summary>
        public string SupplierName { get; set; }
    }
}