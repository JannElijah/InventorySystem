
from typing import List, Dict
from models import ChartDataPoint

class AnalyticsRepositoryMixin:
    # Requires self.get_connection()

    def get_top_selling_products(self, by_revenue: bool = False, days: int = 30) -> List[ChartDataPoint]:
        data_points = []
        if by_revenue:
            sql = """
                SELECT 
                    P.Description AS ProductName,
                    SUM(SI.LineTotal) as Revenue
                FROM SaleItems SI
                JOIN Sales S ON SI.SaleID = S.SaleID
                JOIN Products P ON SI.ProductID = P.ProductID
                WHERE DATE(S.SaleDate) >= ?
                GROUP BY SI.ProductID 
                ORDER BY Revenue DESC
                LIMIT 5
            """
        else:
            sql = """
            SELECT
                P.Description AS ProductName,
                SUM(SI.Quantity) AS TotalSold
            FROM SaleItems SI
            JOIN Sales S ON SI.SaleID = S.SaleID
            JOIN Products P ON SI.ProductID = P.ProductID
            WHERE S.SaleDate >= date('now', ?)
            GROUP BY SI.ProductID
            ORDER BY TotalSold DESC
            LIMIT 5;
            """
        modifier = f'-{days} days'
        
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            for row in cursor:
                data_points.append(ChartDataPoint(
                    label=row["ProductName"],
                    value=row["Revenue"] if by_revenue else row["TotalSold"]
                ))
        return data_points

    def get_daily_sales_volume(self, days: int = 30) -> List[ChartDataPoint]:
        data_points = []
        sql = """
            SELECT date(SaleDate) as d, SUM(LineTotal) as total 
            FROM Sales 
            JOIN SaleItems ON Sales.SaleID = SaleItems.SaleID
            WHERE SaleDate >= date('now', ?)
            GROUP BY d
            ORDER BY d ASC
        """
        modifier = f'-{days} days'
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            for row in cursor:
                data_points.append(ChartDataPoint(label=row[0], value=row[1]))
        return data_points

    def get_daily_gross_profit(self, days: int = 30) -> List[ChartDataPoint]:
        data_points = []
        # Note: Uses current Product Cost as proxy for historical cost if not stored in SaleItems
        sql = """
            SELECT 
                date(S.SaleDate) as d, 
                SUM(SI.LineTotal - (SI.Quantity * P.PurchaseCost)) as Profit
            FROM Sales S
            JOIN SaleItems SI ON S.SaleID = SI.SaleID
            JOIN Products P ON SI.ProductID = P.ProductID
            WHERE S.SaleDate >= date('now', ?)
            GROUP BY d
            ORDER BY d ASC
        """
        modifier = f'-{days} days'
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            for row in cursor:
                val = row["Profit"] if row["Profit"] else 0.0
                data_points.append(ChartDataPoint(label=row["d"], value=val))
        return data_points

    def get_sales_by_brand(self, days: int = 30) -> List[ChartDataPoint]:
        data_points = []
        sql = """
            SELECT P.Brand, SUM(SI.LineTotal) as total 
            FROM SaleItems SI
            JOIN Sales S ON SI.SaleID = S.SaleID
            JOIN Products P ON SI.ProductID = P.ProductID
            WHERE S.SaleDate >= date('now', ?)
            GROUP BY P.Brand
            ORDER BY total DESC
        """
        modifier = f'-{days} days'
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            for row in cursor:
                if row["Brand"]:
                    data_points.append(ChartDataPoint(label=row["Brand"], value=row["total"]))
        return data_points

    def get_inventory_value_by_brand(self) -> List[ChartDataPoint]:
        data_points = []
        sql = """
            SELECT Brand, SUM(StockQuantity * PurchaseCost) as Value
            FROM Products
            WHERE StockQuantity > 0
            GROUP BY Brand
            ORDER BY Value DESC
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql)
            for row in cursor:
                if row["Brand"]:
                    data_points.append(ChartDataPoint(label=row["Brand"], value=row["Value"]))
        return data_points

    def get_inventory_value_by_category(self) -> List[ChartDataPoint]:
        # Fallback/Placeholder if needed, or implement proper Category logic
        return []

    def get_dead_stock(self, days: int = 90) -> List[dict]:
        """Items with Stock > 0 but NO sales in the last X days"""
        results = []
        sql = """
            SELECT P.ProductID, P.Description, P.StockQuantity, P.PurchaseCost, P.SellingPrice, MAX(S.SaleDate) as LastSale
            FROM Products P
            LEFT JOIN SaleItems SI ON P.ProductID = SI.ProductID
            LEFT JOIN Sales S ON SI.SaleID = S.SaleID
            WHERE P.StockQuantity > 0
            GROUP BY P.ProductID
            HAVING LastSale < date('now', ?) OR LastSale IS NULL
            ORDER BY (P.StockQuantity * P.PurchaseCost) DESC
            LIMIT 50
        """
        modifier = f'-{days} days'
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            for row in cursor:
                # Handle NULL LastSale (Never sold)
                last_sale = row["LastSale"] if row["LastSale"] else "Never"
                
                results.append({
                    "id": row["ProductID"],
                    "name": row["Description"],
                    "stock": row["StockQuantity"],
                    "cost": row["PurchaseCost"],
                    "value": row["StockQuantity"] * row["PurchaseCost"],
                    "last_sale": last_sale
                })
        return results

    def get_peak_sales_hours(self) -> List[ChartDataPoint]:
        """Total sales count aggregated by Hour of Day (0-23)"""
        data_points = []
        # Extract Hour from SaleDate (Format: YYYY-MM-DD HH:MM:SS)
        # SQLite strftime('%H', SaleDate) returns 00-23 string
        sql = """
            SELECT strftime('%H', SaleDate) as Hour, COUNT(*) as TxCount
            FROM Sales
            GROUP BY Hour
            ORDER BY Hour ASC
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql)
            hours_map = {h: 0 for h in range(24)} # Pre-fill 0-23
            
            for row in cursor:
                if row["Hour"] is not None:
                     h = int(row["Hour"])
                     hours_map[h] = row["TxCount"]
            
            # Convert to list
            for h in range(24):
                label = f"{h:02d}:00"
                data_points.append(ChartDataPoint(label=label, value=hours_map[h]))
                
        return data_points
        sql = """
            SELECT C.Name as CategoryName, SUM(P.StockQuantity * P.PurchaseCost) as Value
            FROM Products P
            LEFT JOIN Categories C ON P.CategoryID = C.CategoryID
            WHERE P.StockQuantity > 0
            GROUP BY C.Name
            ORDER BY Value DESC
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql)
            for row in cursor:
                cat = row["CategoryName"] if row["CategoryName"] else "Uncategorized"
                data_points.append(ChartDataPoint(label=cat, value=row["Value"]))
        return data_points

    def get_sales_summary_stats(self, days: int = 30) -> Dict:
        stats = {"revenue": 0.0, "orders": 0, "items_sold": 0}
        sql = """
            SELECT 
                COUNT(DISTINCT S.SaleID) as Orders,
                SUM(SI.LineTotal) as Revenue,
                SUM(SI.Quantity) as Items
            FROM Sales S
            JOIN SaleItems SI ON S.SaleID = SI.SaleID
            WHERE S.SaleDate >= date('now', ?)
        """
        modifier = f'-{days} days'
        with self.get_connection() as conn:
             row = conn.execute(sql, (modifier,)).fetchone()
             if row:
                 stats["orders"] = row[0] if row[0] else 0
                 stats["revenue"] = row[1] if row[1] else 0.0
                 stats["items_sold"] = row[2] if row[2] else 0
        return stats

    def get_sparkline_data(self, metric: str, days: int = 10) -> List[float]:
        """
        Returns a list of floats representing the daily trend for the last `days`.
        metric: 'revenue', 'items', 'activity' (transactions)
        """
        data = [0.0] * days
        
        if metric == 'revenue':
            sql = """
                SELECT date(SaleDate) as d, SUM(LineTotal) 
                FROM Sales 
                JOIN SaleItems ON Sales.SaleID = SaleItems.SaleID
                WHERE SaleDate >= date('now', ?)
                GROUP BY d
            """
        elif metric == 'items':
            sql = """
                SELECT date(SaleDate) as d, SUM(Quantity) 
                FROM Sales 
                JOIN SaleItems ON Sales.SaleID = SaleItems.SaleID
                WHERE SaleDate >= date('now', ?)
                GROUP BY d
            """
        elif metric == 'activity':
            sql = """
                SELECT date(TransactionDate) as d, COUNT(*)
                FROM Transactions
                WHERE TransactionDate >= date('now', ?)
                GROUP BY d
            """
        else:
            return data

        modifier = f'-{days} days'
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (modifier,))
            # Map results to the last N days array
            # This is a bit tricky with SQLite dates. 
            # We'll just return the values we found, padded with 0s if sparse, 
            # but simpler for sparkline: just return the raw sequence of known values?
            # Better: Return the last N known values or 0.
            # Let's map properly to today-i
            
            results = {} # {date_str: val}
            for row in cursor:
                results[row[0]] = float(row[1])

        # Construct safe list (Today -> Backwards, then reversed)
        from datetime import datetime, timedelta
        final_list = []
        for i in range(days):
            d_str = (datetime.now() - timedelta(days=i)).strftime("%Y-%m-%d")
            final_list.append(results.get(d_str, 0.0))
        
        return list(reversed(final_list))

    # --- Dashboard Metrics ---
    def get_total_product_count(self) -> int:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT COUNT(*) FROM Products")
            result = cursor.fetchone()
            return result[0] if result else 0

    def get_low_stock_count(self) -> int:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT COUNT(*) FROM Products WHERE StockQuantity <= LowStockThreshold")
            result = cursor.fetchone()
            return result[0] if result else 0

    def get_out_of_stock_count(self) -> int:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT COUNT(*) FROM Products WHERE StockQuantity <= 0")
            result = cursor.fetchone()
            return result[0] if result else 0

    def get_total_inventory_value(self) -> float:
        with self.get_connection() as conn:
            cursor = conn.execute(
                "SELECT SUM(StockQuantity * PurchaseCost) FROM Products WHERE StockQuantity > 0"
            )
            val = cursor.fetchone()[0]
            return float(val) if val else 0.0

    def global_search(self, query: str) -> List[Dict]:
        """
        Searches Products, Customers (Sales), and Settings/Nav logic? 
        Returns list of {type, text, id, detail}
        """
        results = []
        q_wild = f"%{query}%"
        with self.get_connection() as conn:
            # 1. Products
            cur = conn.execute(
                "SELECT ProductID, Description, Barcode FROM Products WHERE Description LIKE ? OR Barcode LIKE ? LIMIT 5", 
                (q_wild, q_wild)
            )
            for row in cur:
                results.append({
                    "type": "Product",
                    "text": row[1],
                    "detail": f"Barcode: {row[2]}",
                    "id": row[0]
                })

            # 2. Key Customers (from Sales) - Distinct names
            cur = conn.execute(
                "SELECT DISTINCT DeliverTo FROM Sales WHERE DeliverTo LIKE ? LIMIT 5", 
                (q_wild,)
            )
            for row in cur:
                results.append({
                    "type": "Customer",
                    "text": row[0],
                    "detail": "Customer Record",
                    "id": row[0] # Name as ID
                })
                
            # 3. Suppliers
            cur = conn.execute(
                "SELECT SupplierID, Name FROM Suppliers WHERE Name LIKE ? LIMIT 5",
                (q_wild,)
            )
            for row in cur:
                results.append({
                    "type": "Supplier",
                    "text": row[1],
                    "detail": "Supplier Record",
                    "id": row[0]
                })
        return results
