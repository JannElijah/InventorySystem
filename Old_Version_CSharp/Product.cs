namespace InventorySystem
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Barcode { get; set; }
        public string PartNumber { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string Volume { get; set; }
        public string Type { get; set; }
        public string Application { get; set; }  
        public decimal PurchaseCost { get; set; }
        public decimal SellingPrice { get; set; }    
        public int StockQuantity { get; set; }
        public string Notes { get; set; }
        public int LowStockThreshold { get; set; }
        public string DateCreated { get; set; } = "";
        public string DateModified { get; set; } = "";
        public string DisplayInfo
        {
            get
            {
                // This combines key details into one clear string.
                // Example Output: "HONDA - ATF DW-1 (HOND001)"
                return $"{Brand} - {Description} ({PartNumber})";
            }
        }
    }
}