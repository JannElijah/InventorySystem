namespace InventoryManagementSystem
{
    // This class defines the structure for one row in our Inventory Status report.
    public class InventoryReportItem
    {
        public string PartNumber { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; } // We need this for the Type filter
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal TotalValue { get; set; }
    }
}