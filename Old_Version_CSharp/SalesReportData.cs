using InventorySystem;

namespace InventoryManagementSystem
{
    // This class simply holds the results of our financial calculation.
    public class SalesReportData
    {
        public decimal TotalRevenue { get; set; }
        public decimal CostOfGoodsSold { get; set; }
        public decimal GrossProfit { get; set; }

        public int TotalTransactions { get; set; }
        public int TotalItemsSold { get; set; }

        // --- NEW Top Performers ---
        public TopPerformer BestSellingProduct { get; set; }
        public TopPerformer MostProfitableProduct { get; set; }

        public SalesReportData()
        {
            BestSellingProduct = new TopPerformer { Name = "N/A" };
            MostProfitableProduct = new TopPerformer { Name = "N/A" };
        }
    }
}