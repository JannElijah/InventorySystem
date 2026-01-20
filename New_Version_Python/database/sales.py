
from typing import List, Optional, Dict
from datetime import datetime, timedelta
import sqlite3
import time
from models import Sale, SaleItem, Transaction, PurchaseOrder, POItem

class SalesRepositoryMixin:
    # Requires self.get_connection(), self.check_low_stock(), self.log_action()

    # --- POS / Transactions ---
    def process_complete_sale(self, sale: Sale, user_id: int):
        # Retry logic for SQLite locking
        max_retries = 5
        for i in range(max_retries):
            try:
                with self.get_connection() as conn:
                    # Transactions are started by the context manager? 
                    # Actually get_connection commits/rollbacks.
                    # If we nested this, it would be weird.
                    # But get_connection is a context manager yielding conn.
                    # We can execute "BEGIN" if we want manual control or rely on python sqlite3 default implicit transactions.
                    # It's safer to explicitly begin if we want atomicity across multiple inserts.
                    
                    # NOTE: BaseRepository.get_connection yields a cursor/connection 
                    # that commits on exit. If we want a multi-statement transaction, 
                    # we should rely on that commit.
                    
                    sale_id = -1
                    try:
                        # 1. Save Sale Header
                        cursor = conn.execute(
                            "INSERT INTO Sales (DeliverTo, SaleDate) VALUES (?, ?)",
                            (sale.customer_name, sale.sale_date)
                        )
                        sale_id = cursor.lastrowid

                        # 2. Process Items
                        for item in sale.items:
                            # A. Add Sale Item
                            cursor = conn.execute(
                                """
                                INSERT INTO SaleItems (SaleID, ProductID, Quantity, UnitPrice, LineTotal) 
                                VALUES (?, ?, ?, ?, ?)
                                """,
                                (sale_id, item.product_id, item.quantity, item.unit_price, item.total)
                            )
                            sale_item_id = cursor.lastrowid

                            # B. Get Current Stock
                            cur = conn.execute("SELECT StockQuantity FROM Products WHERE ProductID = ?", (item.product_id,))
                            stock_before = cur.fetchone()[0]
                            stock_after = stock_before - item.quantity
                            quantity_change = -item.quantity

                            # C. Log Transaction (Audit Trail)
                            conn.execute(
                                """
                                INSERT INTO Transactions 
                                (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, SaleItemID, UserID) 
                                VALUES (?, 'Delivery', ?, ?, ?, ?, ?, ?)
                                """,
                                (item.product_id, quantity_change, stock_before, stock_after, datetime.now(), sale_item_id, user_id)
                            )

                            # D. Update Product Stock (ATOMIC UPDATE)
                            # Fix Race Condition: changing from set value to relative decrement
                            conn.execute(
                                "UPDATE Products SET StockQuantity = StockQuantity - ? WHERE ProductID = ?",
                                (item.quantity, item.product_id)
                            )
                            
                            # Re-fetch new stock for audit log accuracy (optional but good for logs)
                            # We still calculate 'stock_after' for the transaction log based on the snapshot 'stock_before'
                            # Ideally, stock_before logic should also be inside the transaction context rigorously, 
                            # but the critical part is the UPDATE. Even if stock_before is slightly stale in the log,
                            # the physical inventory count will be correct.
                            if hasattr(self, 'get_product_stock'):
                                # Optional: you could fetch the real new stock here if strict accuracy needed for logs
                                pass
                            
                            # F. Audit Log (New V4)
                            if hasattr(self, 'log_action'):
                                self.log_action(item.product_id, user_id, 'SALE', quantity_change, f"Sale #{sale_id}", stock_before, stock_after, conn=conn)

                        # Commit implied by context manager exit
                        # But we need to return sale_id, so we do it here?
                        # No, context manager handles commit.
                        
                    except Exception as e:
                        # Context manager will rollback
                        raise e 
                        
            except sqlite3.OperationalError as e:
                if "locked" in str(e) and i < max_retries - 1:
                    time.sleep(0.2)
                    continue
                raise e
            else:
                 # Success block (no exception)
                 # Post-Transaction: Check Notifications
                 for item in sale.items:
                     if hasattr(self, 'check_low_stock'):
                         self.check_low_stock(item.product_id)
                 return sale_id

    def create_sale(self, items: list, total: float, user_id: int = 1) -> int:
        """Wrapper for POSView compatibility"""
        # items in POSView are SaleItem objects (or should be)
        sale = Sale(customer_name="Walk-in", sale_date=datetime.now(), items=items)
        return self.process_complete_sale(sale, user_id)

    def process_sale(self, customer_name, items, user_id, date=None):
        """Wrapper for StockOutView compatibility"""
        sale_items = []
        for i in items:
            sale_items.append(SaleItem(
                product_id=i['product_id'],
                quantity=i['quantity'],
                unit_price=i.get('unit_price', 0),
                total=i.get('total', 0)
            ))
            
        sale = Sale(customer_name=customer_name, sale_date=date or datetime.now(), items=sale_items)
        return self.process_complete_sale(sale, user_id)

    # --- Supply Transactions ---
    def process_supply_transaction(self, supplier_id: Optional[int], items: List[dict], user_id: int):
        max_retries = 5
        for i in range(max_retries):
            try:
                with self.get_connection() as conn:
                    try:
                        for item in items:
                            pid = item['product_id']
                            qty = item['quantity']
                            cost = item['cost']
                            
                            cur = conn.execute("SELECT StockQuantity FROM Products WHERE ProductID = ?", (pid,))
                            stock_before = cur.fetchone()[0]
                            stock_after = stock_before + qty
                            
                            conn.execute(
                                "UPDATE Products SET StockQuantity = ?, PurchaseCost = ? WHERE ProductID = ?",
                                (stock_after, cost, pid)
                            )
                            
                            conn.execute(
                                """
                                INSERT INTO Transactions 
                                (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, UserID, SupplierID) 
                                VALUES (?, 'Supply', ?, ?, ?, ?, ?, ?)
                                """,
                                (pid, qty, stock_before, stock_after, datetime.now(), user_id, supplier_id)
                            )
                            
                            if hasattr(self, 'log_action'):
                                self.log_action(pid, user_id, 'RESTOCK', qty, "Manual Stock In", stock_before, stock_after, conn=conn)
                        
                    except Exception as e:
                        raise e
            except sqlite3.OperationalError as e:
                if "locked" in str(e) and i < max_retries - 1:
                    time.sleep(0.2)
                    continue
                raise e
            else:
                for item in items:
                    if hasattr(self, 'check_low_stock'):
                        self.check_low_stock(item['product_id'])
                return

    def process_stock_adjustment(self, items: List[dict], reason: str, user_id: int):
        """
        Process stock reduction for non-sales reasons (Damage, Expired, Theft, etc.)
        items: [{'product': ProductObj, 'quantity': int, ...}]
        """
        with self.get_connection() as conn:
            for item in items:
                # Support both dict with 'product' object or direct usage if refactored
                # The UI passes a list of dicts: {'product': product_obj, 'quantity': int}
                product = item['product']
                qty = item['quantity']
                pid = product.product_id
                
                # 1. Get current stock
                cur = conn.execute("SELECT StockQuantity FROM Products WHERE ProductID = ?", (pid,))
                row = cur.fetchone()
                if not row: continue
                stock_before = row[0]
                stock_after = stock_before - qty
                change = -qty
                
                # 2. Update Product
                conn.execute("UPDATE Products SET StockQuantity = ? WHERE ProductID = ?", (stock_after, pid))
                
                # 3. Log Transaction
                conn.execute(
                    """
                    INSERT INTO Transactions 
                    (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, UserID) 
                    VALUES (?, ?, ?, ?, ?, ?, ?)
                    """,
                    (pid, reason, change, stock_before, stock_after, datetime.now(), user_id)
                )
                
                # 4. Audit Log
                if hasattr(self, 'log_action'):
                    self.log_action(pid, user_id, reason.upper(), change, f"Manual Adjustment: {reason}", stock_before, stock_after, conn=conn)

            # Check low stock after
            for item in items:
                if hasattr(self, 'check_low_stock'):
                    self.check_low_stock(item['product'].product_id)
    def get_filtered_transactions(self, 
                                  start_date: Optional[datetime] = None, 
                                  end_date: Optional[datetime] = None, 
                                  trans_type: Optional[str] = None, 
                                  search_query: Optional[str] = None,
                                  limit: int = 50,
                                  offset: int = 0) -> List[Transaction]:
        
        query_parts = ["""
            SELECT
                t.TransactionID, t.ProductID, p.Barcode, p.Description AS ProductDescription,
                t.TransactionType, t.QuantityChange, t.StockBefore, t.StockAfter,
                t.TransactionDate, t.SupplierID, sup.Name AS SupplierName,
                s.DeliverTo AS CustomerName,
                si.LineTotal AS SaleValue,
                p.PurchaseCost
            FROM Transactions t
            INNER JOIN Products p ON t.ProductID = p.ProductID
            LEFT JOIN Suppliers sup ON t.SupplierID = sup.SupplierID
            LEFT JOIN SaleItems si ON t.SaleItemID = si.SaleItemID
            LEFT JOIN Sales s ON si.SaleID = s.SaleID
            WHERE 1=1
        """]
        params = []

        if start_date:
            query_parts.append("AND DATE(t.TransactionDate) >= DATE(?)")
            params.append(start_date.strftime("%Y-%m-%d"))
        
        if end_date:
            query_parts.append("AND DATE(t.TransactionDate) <= DATE(?)")
            params.append(end_date.strftime("%Y-%m-%d"))

        if trans_type and trans_type != "All":
            query_parts.append("AND t.TransactionType = ?")
            params.append(trans_type)

        if search_query:
            q = f"%{search_query}%"
            query_parts.append("AND (p.Description LIKE ? OR p.Brand LIKE ? OR p.Barcode LIKE ?)")
            params.extend([q, q, q])

        # Optimize Sort and Pagination
        query_parts.append("ORDER BY t.TransactionDate DESC, t.TransactionID DESC LIMIT ? OFFSET ?")
        params.append(limit)
        params.append(offset)
        
        sql = " ".join(query_parts)
        
        transactions = []
        with self.get_connection() as conn:
            cursor = conn.execute(sql, tuple(params))
            for row in cursor:
                try:
                    dt = datetime.strptime(row["TransactionDate"], "%Y-%m-%d %H:%M:%S.%f")
                except ValueError:
                    try: dt = datetime.strptime(row["TransactionDate"], "%Y-%m-%d %H:%M:%S")
                    except ValueError: dt = row["TransactionDate"]
                
                # Calculate Value
                qty = row["QuantityChange"]
                val = 0.0
                if row["TransactionType"] == "Delivery":
                    val = row["SaleValue"] if row["SaleValue"] else 0.0
                elif row["TransactionType"] == "Supply":
                    cost = row["PurchaseCost"] or 0
                    val = - (abs(qty) * cost) # Supply is cost out
                else: 
                     val = 0.0

                transactions.append(Transaction(
                    transaction_id=row["TransactionID"],
                    product_id=row["ProductID"],
                    transaction_type=row["TransactionType"],
                    quantity_change=row["QuantityChange"],
                    stock_before=row["StockBefore"],
                    stock_after=row["StockAfter"],
                    transaction_date=dt,
                    supplier_id=row["SupplierID"],
                    barcode=row["Barcode"],
                    product_description=row["ProductDescription"],
                    supplier_name=str(row["SupplierName"] or ""),
                    customer_name=str(row["CustomerName"] or ""),
                    transaction_value=val
                ))
        return transactions

    def count_filtered_transactions(self, 
                                    start_date: Optional[datetime] = None, 
                                    end_date: Optional[datetime] = None, 
                                    trans_type: Optional[str] = None, 
                                    search_query: Optional[str] = None) -> int:
        """Optimized Count for Pagination"""
        query_parts = ["""
            SELECT COUNT(*)
            FROM Transactions t
            INNER JOIN Products p ON t.ProductID = p.ProductID
            WHERE 1=1
        """]
        params = []

        if start_date:
            query_parts.append("AND DATE(t.TransactionDate) >= DATE(?)")
            params.append(start_date.strftime("%Y-%m-%d"))
        
        if end_date:
            query_parts.append("AND DATE(t.TransactionDate) <= DATE(?)")
            params.append(end_date.strftime("%Y-%m-%d"))

        if trans_type and trans_type != "All":
            query_parts.append("AND t.TransactionType = ?")
            params.append(trans_type)

        if search_query:
            q = f"%{search_query}%"
            query_parts.append("AND (p.Description LIKE ? OR p.Brand LIKE ? OR p.Barcode LIKE ?)")
            params.extend([q, q, q])

        sql = " ".join(query_parts)
        with self.get_connection() as conn:
            return conn.execute(sql, tuple(params)).fetchone()[0]

    def get_dashboard_transactions(self) -> List[Transaction]:
        return self.get_filtered_transactions(search_query=None, limit=15)

    def get_all_transactions(self):
         # Legacy helper for view that might expect dicts
        transactions = []
        with self.get_connection() as conn:
            sql = """
                SELECT t.*, p.Description as ProductName 
                FROM Transactions t
                LEFT JOIN Products p ON t.ProductID = p.ProductID
                ORDER BY t.TransactionDate DESC, t.TransactionID DESC
                LIMIT 500
            """
            cursor = conn.execute(sql)
            for row in cursor:
                transactions.append({
                    "id": row["TransactionID"],
                    "date": row["TransactionDate"],
                    "product": row["ProductName"],
                    "type": row["TransactionType"],
                    "change": row["QuantityChange"],
                    "stock_after": row["StockAfter"]
                })
        return transactions

    # --- Purchase Orders ---
    def create_purchase_order(self, supplier_id: int, items: List[Dict]) -> int:
        with self.get_connection() as conn:
            total = sum(item['qty'] * item['cost'] for item in items)
            cur = conn.execute(
                "INSERT INTO PurchaseOrders (SupplierID, OrderDate, Status, TotalCost) VALUES (?, ?, ?, ?)",
                (supplier_id, datetime.now(), "Ordered", total)
            )
            po_id = cur.lastrowid
            for item in items:
                conn.execute(
                    "INSERT INTO POItems (POID, ProductID, QuantityOrdered, UnitCost) VALUES (?, ?, ?, ?)",
                    (po_id, item['product_id'], item['qty'], item['cost'])
                )
            return po_id

    def get_all_purchase_orders(self):
        pos = []
        sql = """
            SELECT po.*, s.Name as SupplierName 
            FROM PurchaseOrders po
            LEFT JOIN Suppliers s ON po.SupplierID = s.SupplierID
            ORDER BY po.OrderDate DESC
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql)
            for row in cursor:
                 # Date parsing simplified for brevity
                 pos.append(PurchaseOrder(
                    po_id=row["POID"],
                    supplier_id=row["SupplierID"],
                    order_date=row["OrderDate"], # Assuming string or simplified
                    status=row["Status"],
                    total_cost=row["TotalCost"],
                    supplier_name=row["SupplierName"]
                ))
        return pos

    def get_po_items(self, po_id: int):
        items = []
        sql = """
            SELECT poi.*, p.Description 
            FROM POItems poi
            JOIN Products p ON poi.ProductID = p.ProductID
            WHERE poi.POID = ?
        """
        with self.get_connection() as conn:
            cursor = conn.execute(sql, (po_id,))
            for row in cursor:
                items.append(POItem(
                    item_id=row["ItemID"],
                    po_id=row["POID"],
                    product_id=row["ProductID"],
                    quantity_ordered=row["QuantityOrdered"],
                    quantity_received=row["QuantityReceived"],
                    unit_cost=row["UnitCost"],
                    product_name=row["Description"]
                ))
        return items

    def receive_po_items(self, po_id: int, received_items: Dict[int, int], user_id: int):
        with self.get_connection() as conn:
            for pid, qty in received_items.items():
                if qty > 0:
                    conn.execute(
                        "UPDATE POItems SET QuantityReceived = QuantityReceived + ? WHERE POID = ? AND ProductID = ?",
                        (qty, po_id, pid)
                    )
                    
                    conn.execute("UPDATE Products SET StockQuantity = StockQuantity + ? WHERE ProductID = ?", (qty, pid))
                    
                    cur_stock = conn.execute("SELECT StockQuantity FROM Products WHERE ProductID = ?", (pid,)).fetchone()[0]
                    
                    conn.execute("""
                        INSERT INTO Transactions (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, UserID, SupplierID)
                        VALUES (?, 'Supply (PO)', ?, ?, ?, ?, ?, (SELECT SupplierID FROM PurchaseOrders WHERE POID=?))
                    """, (pid, qty, cur_stock - qty, cur_stock, datetime.now(), user_id, po_id))
                    
                    if hasattr(self, 'log_action'):
                         self.log_action(pid, user_id, 'RESTOCK', qty, f"PO #{po_id} Received", cur_stock - qty, cur_stock, conn=conn)

            items = conn.execute("SELECT QuantityOrdered, QuantityReceived FROM POItems WHERE POID = ?", (po_id,)).fetchall()
            status = "Completed"
            for row in items:
                if row["QuantityReceived"] < row["QuantityOrdered"]:
                    status = "Partially Received"
                    break
            conn.execute("UPDATE PurchaseOrders SET Status = ? WHERE POID = ?", (status, po_id))
