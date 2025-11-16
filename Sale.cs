using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq; // Required for .Sum()

namespace InventorySystem // It's good practice to wrap classes in a namespace
{
    public class SaleItem
    {
        public int ProductID { get; set; }
        public int SaleID { get; set; }
        public int? TransactionID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public Product Product { get; set; }
    }

    public class Sale
    {
        public int SaleID { get; set; }
        public int? TransactionID { get; set; }
        public string CustomerName { get; set; }
        public DateTime SaleDate { get; set; }
        public BindingList<SaleItem> Items { get; set; }

        // Simplified using a LINQ expression
        public decimal Subtotal => Items.Sum(item => item.Total);

        public Sale()
        {
            Items = new BindingList<SaleItem>();
            SaleDate = DateTime.Now;
        }
    }
}