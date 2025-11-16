namespace InventorySystem
{
    // A simple helper class to manage items in our "shopping cart" grid.
    public class TransactionItem
    {
        public int ProductID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceCost { get; set; }
        public decimal LineTotal => Quantity * UnitPriceCost; // Calculated property
    }
}