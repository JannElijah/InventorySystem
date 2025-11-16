// In a new file, e.g., SupplyOrder.cs
using InventorySystem;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class SupplyItem
{
    public int ProductID { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal PurchaseCost { get; set; }
    public decimal Total { get; set; }
    public Product Product { get; set; } // Store the full product for stock checks
}

public class SupplyOrder
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public BindingList<SupplyItem> Items { get; set; }

    public decimal Subtotal => Items.Sum(item => item.Total);

    public SupplyOrder()
    {
        Items = new BindingList<SupplyItem>();
    }
}