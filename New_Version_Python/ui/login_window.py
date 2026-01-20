import customtkinter as ctk
import bcrypt
from tkinter import messagebox
from database import DatabaseRepository
from ui.styles import Colors

class LoginWindow(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository, on_login_success):
        super().__init__(parent)
        self.db = db
        self.on_login_success = on_login_success

        # Center Frame
        self.place(relx=0.5, rely=0.5, anchor="center")
        self.configure(fg_color="transparent")

        # UI Elements Container
        self.frame = ctk.CTkFrame(self, width=320, height=400, corner_radius=15, fg_color=Colors.BG_CARD)
        self.frame.pack(padx=20, pady=20)

        self.label_title = ctk.CTkLabel(self.frame, text="Welcome Back", font=("Roboto Medium", 24), text_color=Colors.TEXT_PRIMARY)
        self.label_title.place(relx=0.5, rely=0.15, anchor="center")

        self.entry_username = ctk.CTkEntry(self.frame, width=220, placeholder_text="Username", 
                                           fg_color=Colors.BG_INPUT, border_color=Colors.BORDER, text_color=Colors.TEXT_INPUT)
        self.entry_username.place(relx=0.5, rely=0.35, anchor="center")

        self.entry_password = ctk.CTkEntry(self.frame, width=220, placeholder_text="Password", show="*", 
                                           fg_color=Colors.BG_INPUT, border_color=Colors.BORDER, text_color=Colors.TEXT_INPUT)
        self.entry_password.place(relx=0.5, rely=0.48, anchor="center")
        
        self.check_show_pass = ctk.CTkCheckBox(self.frame, text="Show Password", command=self.toggle_password, font=("Arial", 11), width=20, height=20, text_color=Colors.TEXT_SECONDARY)
        self.check_show_pass.place(relx=0.5, rely=0.58, anchor="center")

        self.btn_login = ctk.CTkButton(self.frame, text="Login", width=220, command=self.login_event, corner_radius=6, 
                                       fg_color=Colors.PRIMARY, text_color="white", hover_color=Colors.BG_HOVER) # Force white text on Blue Button
        self.btn_login.place(relx=0.5, rely=0.7, anchor="center")
        
        self.lbl_error = ctk.CTkLabel(self.frame, text="", text_color=Colors.DANGER, font=("Arial", 12))
        self.lbl_error.place(relx=0.5, rely=0.8, anchor="center")

        
        self.entry_password.bind('<Return>', lambda event: self.login_event())

    def toggle_password(self):
        if self.check_show_pass.get() == 1:
            self.entry_password.configure(show="")
        else:
            self.entry_password.configure(show="*")

    def login_event(self):
        username = self.entry_username.get()
        password = self.entry_password.get()

        if not username or not password:
            self.lbl_error.configure(text="Please fill in all fields.")
            return

        user = self.db.get_user_by_username(username)
        
        if user:
            try:
                if bcrypt.checkpw(password.encode('utf-8'), user.password_hash.encode('utf-8')):
                    self.on_login_success(user)
                else:
                    self.lbl_error.configure(text="Invalid password.")
            except ValueError:
                self.lbl_error.configure(text="Invalid password hash.")
        else:
            self.lbl_error.configure(text="User not found.")
