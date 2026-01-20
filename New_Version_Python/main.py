import customtkinter as ctk
import os
import sys
import bcrypt
from ui.login_window import LoginWindow
from ui.dashboard_window import DashboardWindow
from database import DatabaseRepository
from models import User
from utils.config_manager import ConfigManager
from db_migrations import DBMigrator
from utils.logger import logger
from utils.backup_manager import BackupManager
from utils.app_context import get_app_path
import traceback
from tkinter import messagebox
from ui.styles import Colors
import matplotlib
matplotlib.use("TkAgg")

# Configure Theme (Defaults if config missing)
ctk.set_appearance_mode("Dark") 
ctk.set_default_color_theme("blue")

# High DPI Fix
try:
    from ctypes import windll
    windll.shcore.SetProcessDpiAwareness(1) # 1 = Process System DPI Aware
except Exception:
    pass

class App(ctk.CTk):
    def __init__(self):
        try:
            super().__init__()
            
            # Load Config (Overrides Appearance)
            logger.info("Application Started")

            # 0. Data Protection: Backup First
            # Use absolute path to ensure DB is found regardless of CWD
            base_dir = get_app_path()
            db_path = os.path.join(base_dir, "inventory.db")
            BackupManager.perform_backup(db_path)
            
            self.title("Inventory Management System")
            self.geometry("1100x700")
            
            # FORCE Professional Dark Background
            self.configure(fg_color=Colors.BG_DARK)

            self.db = DatabaseRepository()
            
            # Initialize Config with DB
            ConfigManager.initialize(self.db)
            
            # Global Exception Handler (Tkinter)
            self.report_callback_exception = self.handle_exception
            
            # Run Migrations
            migrator = DBMigrator(self.db)
            migrator.migrate()
            
            self.current_user = None
            self.current_frame = None

            self.ensure_admin_exists()
            self.show_login()
        except Exception as e:
            logger.error(f"Startup Critical Failure: {e}")
            print(f"CRITICAL ERROR: {e}")
            raise e

    def ensure_admin_exists(self):
        # Bootstrap: Ensure admin exists
        if not self.db.get_user_by_username("admin"):
            print("Creating default admin user...")
            hashed = bcrypt.hashpw("password123".encode('utf-8'), bcrypt.gensalt()).decode('utf-8')
            admin = User(
                user_id=0, # Auto-increment ignores this
                username="admin",
                password_hash=hashed,
                role="Admin"
            )
            self.db.add_user(admin)

    def clear_current_frame(self):
        if self.current_frame:
            self.current_frame.destroy()
            self.current_frame = None

    def show_login(self):
        self.clear_current_frame()
        self.title("Inventory System - Login")
        self.geometry("400x500")
        self.current_frame = LoginWindow(self, self.db, self.on_login_success)
        self.current_frame.pack(fill="both", expand=True)

    def on_login_success(self, user: User):
        print(f"Login successful for: {user.username}")
        self.current_user = user
        self.show_dashboard()

    def show_dashboard(self):
        self.clear_current_frame()
        self.title(f"Inventory System - {self.current_user.username}")
        self.geometry("1100x700")
        self.current_frame = DashboardWindow(self, self.current_user, self.db, self.logout)
        self.current_frame.pack(fill="both", expand=True)

        # Start Auto-Backup Timer (Every 1 Hour)
        self.after(3600000, self.auto_backup_loop)

    def auto_backup_loop(self):
        try:
            # Silent Backup
            base_dir = get_app_path()
            db_path = os.path.join(base_dir, "inventory.db")
            BackupManager.perform_backup(db_path)
            logger.info("Auto-Backup completed successfully")
        except Exception as e:
            logger.error(f"Auto-Backup Failed: {e}")
        
        # Schedule next
        self.after(3600000, self.auto_backup_loop)



    def logout(self):
        self.current_user = None
        self.show_login()

    def handle_exception(self, exc_type, exc_value, exc_traceback):
        """
        Global handler for runtime exceptions in the Tkinter mainloop.
        Logs the error and shows a friendly dialog to the user.
        """
        if issubclass(exc_type, KeyboardInterrupt):
            sys.__excepthook__(exc_type, exc_value, exc_traceback)
            return

        error_msg = "".join(traceback.format_exception(exc_type, exc_value, exc_traceback))
        logger.error(f"Uncaught Exception:\n{error_msg}")
        
        # Show polite error to user
        messagebox.showerror(
            "Unexpected Error", 
            f"An unexpected error occurred.\n\nError: {exc_value}\n\nPlease check logs/app.log for details."
        )

if __name__ == "__main__":
    app = App()
    app.mainloop()
