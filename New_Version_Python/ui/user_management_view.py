import customtkinter as ctk
from tkinter import messagebox
import bcrypt
from database import DatabaseRepository
from models import User
from ui.styles import Colors

class UserManagementView(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        self.users = []
        self.editing_user = None

        # Layout
        self.grid_columnconfigure(0, weight=1)
        # self.grid_rowconfigure(2, weight=1) REMOVED

        # Header
        self.header_frame = ctk.CTkFrame(self, fg_color="transparent")
        self.header_frame.grid(row=0, column=0, sticky="ew", padx=20, pady=(0, 20))

        self.lbl_title = ctk.CTkLabel(self.header_frame, text="User Management", font=("Arial", 24, "bold"), text_color=Colors.TEXT_PRIMARY)
        self.lbl_title.pack(side="left")

        self.btn_refresh = ctk.CTkButton(self.header_frame, text="Refresh", width=100, command=self.load_data)
        self.btn_refresh.pack(side="right", padx=5)

        self.btn_add = ctk.CTkButton(self.header_frame, text="+ Add User", width=120, command=self.toggle_form, fg_color="green")
        self.btn_add.pack(side="right", padx=5)
        
        # --- Integrated Form (Row 1) ---
        self.form_frame = ctk.CTkFrame(self, fg_color=Colors.BG_CARD, corner_radius=10)
        self.setup_form()
        
        # Grid Header
        self.grid_header = ctk.CTkFrame(self, height=40, fg_color=Colors.BG_HOVER)
        self.grid_header.grid(row=2, column=0, sticky="ew", padx=20, pady=(0, 5))
        
        # Column Config
        self.col_config = [
            ("ID", 40, 0),
            ("Username", 0, 1),
            ("Role", 100, 0),
            ("Contact", 150, 0),
            ("Actions", 120, 0)
        ]
        
        for i, (text, width, weight) in enumerate(self.col_config):
            self.grid_header.grid_columnconfigure(i, weight=weight)
            if weight == 1:
                ctk.CTkLabel(self.grid_header, text=text, font=("Arial", 12, "bold"), anchor="w", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=i, padx=5, pady=5, sticky="ew")
            else:
                ctk.CTkLabel(self.grid_header, text=text, font=("Arial", 12, "bold"), width=width, anchor="w", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=i, padx=5, pady=5, sticky="w")

        # Scrollable List
        self.scroll_frame = ctk.CTkScrollableFrame(self, fg_color="transparent")
        self.scroll_frame.grid(row=3, column=0, sticky="nsew", padx=20, pady=(0, 20))
        self.grid_rowconfigure(3, weight=1)

        self.load_data()

    def setup_form(self):
        self.form_frame.grid_columnconfigure(1, weight=1)
        
        # Username
        ctk.CTkLabel(self.form_frame, text="Username:", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=0, padx=10, pady=5, sticky="w")
        self.entry_user = ctk.CTkEntry(self.form_frame, width=200, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_user.grid(row=0, column=1, padx=10, pady=5, sticky="w")
        
        # Role
        ctk.CTkLabel(self.form_frame, text="Role:", text_color=Colors.TEXT_PRIMARY).grid(row=1, column=0, padx=10, pady=5, sticky="w")
        self.role_var = ctk.StringVar(value="User")
        self.cmb_role = ctk.CTkComboBox(self.form_frame, values=["Admin", "User"], variable=self.role_var, width=200,
                                        fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, button_color=Colors.BG_INPUT, border_width=1, border_color=Colors.BORDER, dropdown_fg_color=Colors.BG_CARD, dropdown_text_color=Colors.TEXT_PRIMARY, dropdown_hover_color=Colors.PRIMARY)
        self.cmb_role.grid(row=1, column=1, padx=10, pady=5, sticky="w")
        
        # Contact
        ctk.CTkLabel(self.form_frame, text="Contact:", text_color=Colors.TEXT_PRIMARY).grid(row=2, column=0, padx=10, pady=5, sticky="w")
        self.entry_contact = ctk.CTkEntry(self.form_frame, width=200, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_contact.grid(row=2, column=1, padx=10, pady=5, sticky="w")
        
        # Password
        self.lbl_pass = ctk.CTkLabel(self.form_frame, text="Password:", text_color=Colors.TEXT_PRIMARY)
        self.lbl_pass.grid(row=3, column=0, padx=10, pady=5, sticky="w")
        self.entry_pass = ctk.CTkEntry(self.form_frame, show="*", width=200, placeholder_text="Required for new users",
                                       fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_pass.grid(row=3, column=1, padx=10, pady=5, sticky="w")
        
        # Buttons
        btn_box = ctk.CTkFrame(self.form_frame, fg_color="transparent")
        btn_box.grid(row=4, column=0, columnspan=2, pady=10)
        ctk.CTkButton(btn_box, text="Save User", command=self.save_user, fg_color="green", width=100).pack(side="left", padx=5)
        ctk.CTkButton(btn_box, text="Cancel", command=self.toggle_form, fg_color="gray", width=100).pack(side="left", padx=5)

    def toggle_form(self):
        if self.form_frame.winfo_viewable():
            self.form_frame.grid_forget()
            self.btn_add.configure(text="+ Add User", fg_color="green")
            self.editing_user = None
            self.entry_user.delete(0, "end")
            self.entry_pass.delete(0, "end")
            self.entry_contact.delete(0, "end")
            self.lbl_pass.configure(text="Password:")
            self.entry_pass.configure(placeholder_text="Required for new users")
        else:
            self.form_frame.grid(row=1, column=0, sticky="nsew", padx=20, pady=10)
            self.btn_add.configure(text="- Close Form", fg_color="gray")

    def load_data(self):
        for widget in self.scroll_frame.winfo_children():
            widget.destroy()

        self.users = self.db.get_all_users()
        
        for i, u in enumerate(self.users):
            bg = Colors.BG_CARD if i % 2 == 0 else Colors.BG_HOVER
            row = ctk.CTkFrame(self.scroll_frame, fg_color=bg, corner_radius=6)
            row.pack(fill="x", pady=2)
            
            for col_idx, (name, width, weight) in enumerate(self.col_config):
                row.grid_columnconfigure(col_idx, weight=weight)
            
            vals = [str(u.user_id), u.username, u.role, u.contact_number or "-", ""]
            
            for c, val in enumerate(vals):
                if c == 4: continue # Action col
                w = self.col_config[c][1]
                weight = self.col_config[c][2]
                
                if weight == 1:
                    ctk.CTkLabel(row, text=val, anchor="w", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=c, padx=5, pady=8, sticky="ew")
                else:
                    ctk.CTkLabel(row, text=val, width=w, anchor="w", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=c, padx=5, pady=8, sticky="w")
            
            act = ctk.CTkFrame(row, fg_color="transparent")
            act.grid(row=0, column=4, padx=5)
            ctk.CTkButton(act, text="Edit", width=50, command=lambda x=u: self.edit_user(x)).pack(side="left", padx=2)
            ctk.CTkButton(act, text="Del", width=50, fg_color="red", command=lambda x=u: self.delete_user(x)).pack(side="left", padx=2)

    def edit_user(self, user):
        self.editing_user = user
        if not self.form_frame.winfo_viewable():
            self.toggle_form()
            
        self.entry_user.delete(0, "end"); self.entry_user.insert(0, user.username)
        self.role_var.set(user.role)
        self.entry_contact.delete(0, "end"); 
        if user.contact_number: self.entry_contact.insert(0, user.contact_number)
        
        self.lbl_pass.configure(text="New Password:")
        self.entry_pass.configure(placeholder_text="Leave blank to keep current")
        self.btn_add.configure(text="Editing User")

    def delete_user(self, user):
        if messagebox.askyesno("Confirm Delete", f"Are you sure you want to delete user '{user.username}'?"):
            self.db.delete_user(user.user_id)
            self.load_data()

    def save_user(self):
        username = self.entry_user.get().strip()
        password = self.entry_pass.get().strip()
        role = self.role_var.get()
        contact = self.entry_contact.get().strip()

        if not username:
             messagebox.showerror("Error", "Username is required.")
             return

        password_hash = ""
        if password:
             password_hash = bcrypt.hashpw(password.encode('utf-8'), bcrypt.gensalt()).decode('utf-8')
        elif not self.editing_user:
             messagebox.showerror("Error", "Password is required for new users.")
             return

        new_user = User(
            user_id=self.editing_user.user_id if self.editing_user else 0,
            username=username,
            password_hash=password_hash, 
            role=role,
            contact_number=contact
        )

        try:
            if self.editing_user:
                self.db.update_user(new_user)
                messagebox.showinfo("Success", "User updated!")
            else:
                self.db.add_user(new_user)
                messagebox.showinfo("Success", "User added!")
            
            self.load_data()
            self.toggle_form()
            
        except Exception as e:
            messagebox.showerror("Error", f"Failed to save user: {e}")
