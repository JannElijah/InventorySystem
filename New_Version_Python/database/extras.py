
from typing import List, Optional
from datetime import datetime

class AuditLogMixin:
    # Requires self.get_connection()

    def log_action(self, product_id: int, user_id: int, action_type: str, qty_change: int, reason: str, prev_stock: int, new_stock: int, conn=None):
        sql = """
            INSERT INTO AuditLog (ProductID, UserID, ActionType, QuantityChange, PreviousStock, NewStock, Reason)
            VALUES (?, ?, ?, ?, ?, ?, ?)
        """
        params = (product_id, user_id, action_type, qty_change, prev_stock, new_stock, reason)

        if conn:
            conn.execute(sql, params)
        else:
            with self.get_connection() as new_conn:
                new_conn.execute(sql, params)

    def get_audit_logs(self, product_id: int) -> List[dict]:
        logs = []
        with self.get_connection() as conn:
            cursor = conn.execute(
                """
                SELECT a.*, u.Username 
                FROM AuditLog a 
                LEFT JOIN Users u ON a.UserID = u.UserID
                WHERE a.ProductID = ? 
                ORDER BY a.Timestamp DESC
                """, 
                (product_id,)
            )
            for row in cursor:
                logs.append(dict(row))
        return logs

class NotificationMixin:
    # Requires self.get_connection()

    def add_notification(self, product_id: int, message: str, notif_type: str = "System"):
        with self.get_connection() as conn:
            now = datetime.now().isoformat()
            try:
                # Primary attempt: Full schema (Version with Timestamp and NotificationType)
                conn.execute(
                    """
                    INSERT INTO Notifications 
                    (ProductID, Message, DateCreated, NotificationType, Timestamp) 
                    VALUES (?, ?, ?, ?, ?)
                    """,
                    (product_id, message, now, notif_type, now)
                )
            except Exception as e:
                # Fallback 1: Maybe Timestamp exists but NotificationType doesn't?
                # Fallback 2: Old standard schema?
                try:
                    # Retry with just standard columns + Timestamp if that was the issue?
                    # Or just standard.
                    # Let's try standard legacy schema
                    conn.execute(
                        "INSERT INTO Notifications (ProductID, Message, DateCreated) VALUES (?, ?, ?)",
                        (product_id, message, now)
                    )
                except Exception as e2:
                    # If that fails too, try including just Timestamp (rare case but possible)
                     conn.execute(
                        "INSERT INTO Notifications (ProductID, Message, DateCreated, Timestamp) VALUES (?, ?, ?, ?)",
                        (product_id, message, now, now)
                    )

    def get_notifications(self):
        notifs = []
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Notifications ORDER BY DateCreated DESC")
            for row in cursor:
                class NotifObj:
                    def __init__(self, r):
                        self.notification_id = r["NotificationID"]
                        self.product_id = r["ProductID"]
                        self.message = r["Message"]
                        self.is_read = bool(r["IsRead"])
                        self.timestamp = r["DateCreated"]
                notifs.append(NotifObj(row))
        return notifs

    def get_unread_notifications(self):
        notifs = []
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Notifications WHERE IsRead = 0 ORDER BY DateCreated DESC")
            for row in cursor:
                notifs.append({
                    "id": row["NotificationID"],
                    "message": row["Message"],
                    "date": row["DateCreated"]
                })
        return notifs

    def mark_notification_read(self, notif_id: int):
        with self.get_connection() as conn:
            conn.execute("UPDATE Notifications SET IsRead = 1 WHERE NotificationID = ?", (notif_id,))

    def mark_all_notifications_read(self):
        with self.get_connection() as conn:
            conn.execute("UPDATE Notifications SET IsRead = 1 WHERE IsRead = 0")

    def delete_notification(self, notif_id: int):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Notifications WHERE NotificationID = ?", (notif_id,))

    def clear_all_notifications(self):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Notifications")

    def check_low_stock(self, product_id: int):
        """Checks if product low stock and triggers notification if needed."""
        with self.get_connection() as conn:
            curr = conn.execute("SELECT StockQuantity, LowStockThreshold, Description FROM Products WHERE ProductID = ?", (product_id,))
            row = curr.fetchone()
            if row:
                qty = row["StockQuantity"]
                threshold = row["LowStockThreshold"]
                name = row["Description"]
                
                if qty <= threshold:
                    check = conn.execute(
                        "SELECT 1 FROM Notifications WHERE ProductID = ? AND IsRead = 0", (product_id,)
                    ).fetchone()
                    if not check:
                        self.add_notification(product_id, f"Low Stock Warning: {name} ({qty} remaining)", "LowStock")
