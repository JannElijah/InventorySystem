import customtkinter as ctk
from tkinter import messagebox
from ui.styles import Colors, Dimens
from database import DatabaseRepository

class NotificationsView(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository, user):
        super().__init__(parent, fg_color=Colors.BG_CARD)
        self.db = db
        self.user = user
        self.parent = parent
        
        # self.title("Notifications Manager") # No title for Frame
        # self.geometry("800x600") # No geometry for Frame
        
        # Header

        header = ctk.CTkFrame(self, fg_color="transparent")
        header.pack(fill="x", padx=20, pady=20)
        
        ctk.CTkLabel(header, text="System Notifications", font=Dimens.heading_l(None), 
                     text_color=Colors.TEXT_PRIMARY).pack(side="left")
        
        # Actions
        btn_frame = ctk.CTkFrame(header, fg_color="transparent")
        btn_frame.pack(side="right")
        
        ctk.CTkButton(btn_frame, text="Mark All Read", fg_color=Colors.SUCCESS, text_color=Colors.TEXT_ON_NEON,
                      command=self.mark_all_read).pack(side="left", padx=5)
                      
        ctk.CTkButton(btn_frame, text="Clear All", fg_color=Colors.DANGER, 
                      command=self.clear_all_notifications).pack(side="left", padx=5)

        # List Area
        self.scroll_frame = ctk.CTkScrollableFrame(self, fg_color="transparent")
        self.scroll_frame.pack(fill="both", expand=True, padx=20, pady=(0, 20))
        
        self.load_notifications()

    def load_notifications(self):
        for w in self.scroll_frame.winfo_children(): w.destroy()
        
        notifications = self.db.get_notifications()
        
        if not notifications:
            ctk.CTkLabel(self.scroll_frame, text="No new notifications.", text_color=Colors.TEXT_SECONDARY).pack(pady=50)
            return

        for n in notifications:
            self.create_notification_card(n)

    def create_notification_card(self, notification):
        # Card
        card = ctk.CTkFrame(self.scroll_frame, fg_color=Colors.BG_HOVER if not notification.is_read else "transparent",
                            border_width=1, border_color=Colors.BORDER)
        card.pack(fill="x", pady=5)
        
        # Icon/Status
        status_color = Colors.PRIMARY if not notification.is_read else "gray"
        icon = "🔵" if not notification.is_read else "⚪"
        
        ctk.CTkLabel(card, text=icon, font=("Arial", 16)).pack(side="left", padx=10)
        
        # Content
        content = ctk.CTkFrame(card, fg_color="transparent")
        content.pack(side="left", fill="both", expand=True, padx=5, pady=5)
        
        title_font = ("Arial", 12, "bold") if not notification.is_read else ("Arial", 12)
        ctk.CTkLabel(content, text=notification.message, font=title_font, 
                     text_color=Colors.TEXT_PRIMARY).pack(anchor="w")
                     
        ctk.CTkLabel(content, text=notification.timestamp, font=("Arial", 10), 
                     text_color=Colors.TEXT_SECONDARY).pack(anchor="w")
        
        # Actions
        actions = ctk.CTkFrame(card, fg_color="transparent")
        actions.pack(side="right", padx=10)
        
        if not notification.is_read:
            ctk.CTkButton(actions, text="Read", width=60, height=24, fg_color=Colors.BG_CARD, text_color=Colors.TEXT_PRIMARY,
                          command=lambda nid=notification.notification_id: self.mark_read(nid)).pack(side="left", padx=2)
                          
        ctk.CTkButton(actions, text="Delete", width=60, height=24, fg_color=Colors.BG_CARD, hover_color=Colors.DANGER, text_color=Colors.TEXT_PRIMARY,
                      command=lambda nid=notification.notification_id: self.delete_notification(nid)).pack(side="left", padx=2)

    def mark_all_read(self):
        self.db.mark_all_notifications_read() # Need to verify this method exists or create it
        self.load_notifications()
        # Notify parent dashboard to update badge?
        # Ideally, we should use a callback or event, but for now user can refresh Dashboard manually or we assume active refresh on close.

    def clear_all_notifications(self):
        if messagebox.askyesno("Confirm", "Delete ALL notifications? This cannot be undone."):
            self.db.clear_all_notifications() # Need to create this
            self.load_notifications()

    def mark_read(self, nid):
        self.db.mark_notification_read(nid)
        self.load_notifications()

    def delete_notification(self, nid):
        self.db.delete_notification(nid) # Need to create this
        self.load_notifications()
