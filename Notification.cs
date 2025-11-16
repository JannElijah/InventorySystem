using System;

namespace InventorySystem // Make sure this namespace matches your project's
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int ProductID { get; set; }

        // This property is for display purposes and will be filled by a JOIN query later
        public string ProductName { get; set; }

        public string NotificationType { get; set; } // e.g., "Low Stock" or "Out of Stock"
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Timestamp { get; set; }
    }
}