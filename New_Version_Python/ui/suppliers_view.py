import customtkinter as ctk
from tkinter import messagebox
from database import DatabaseRepository
from database import DatabaseRepository
from models import Supplier, User
from ui.styles import Colors, Dimens

class SuppliersView(ctk.CTkFrame):
    def __init__(self, parent, db: DatabaseRepository, user):
        super().__init__(parent, fg_color="transparent")
        self.db = db
        self.user = user
        self.suppliers = []
        self.editing_supplier = None # Track if editing
        
        # Grid Layout
        self.grid_columnconfigure(0, weight=1)
        # self.grid_rowconfigure(2, weight=1) REMOVED: Row 2 is Header, Row 3 is List (handled later)
        
        # Header
        header = ctk.CTkFrame(self, fg_color="transparent")
        header.grid(row=0, column=0, sticky="ew", padx=20, pady=10)
        ctk.CTkLabel(header, text="Supplier Management", font=("Arial", 24, "bold"), text_color=Colors.TEXT_PRIMARY).pack(side="left")
        self.btn_add_toggle = ctk.CTkButton(header, text="+ Add Supplier", command=self.toggle_form, fg_color="green")
        if self.user.role == "Admin":
            self.btn_add_toggle.pack(side="right")
        
        self.show_archived = False
        self.switch_archive = ctk.CTkSwitch(header, text="Show Archived", command=self.toggle_archived_view, text_color=Colors.TEXT_PRIMARY, progress_color=Colors.PRIMARY)
        self.switch_archive.pack(side="right", padx=15)
        
        # --- Integrated Add/Edit Form (Row 1) ---
        self.form_frame = ctk.CTkFrame(self, fg_color=Colors.BG_CARD, corner_radius=10)
        # Initially Hidden
        
        self.setup_form()
        
        # --- List Area (Row 2) ---
        # Grid Headers
        self.grid_header = ctk.CTkFrame(self, height=40, fg_color=Colors.BG_HOVER)
        self.grid_header.grid(row=2, column=0, sticky="ew", padx=20, pady=(10, 0))
        
        # Column Config: (Name, Width, Weight)
        # Width=0 means flex if weight=1
        self.col_config = [
            ("ID", 40, 0),
            ("Name", 0, 1),
            ("Contact", 150, 0),
            ("Email", 150, 0),
            ("Phone", 120, 0),
            ("Actions", 140, 0)
        ]
        
        for i, (text, width, weight) in enumerate(self.col_config):
            self.grid_header.grid_columnconfigure(i, weight=weight)
            
            # Use explicit width for fixed columns, or just padding/sticky for flex
            if weight == 1:
                lbl = ctk.CTkLabel(self.grid_header, text=text, font=("Arial", 12, "bold"), anchor="w", text_color=Colors.TEXT_PRIMARY)
                lbl.grid(row=0, column=i, padx=5, pady=5, sticky="ew")
            else:
                lbl = ctk.CTkLabel(self.grid_header, text=text, font=("Arial", 12, "bold"), width=width, anchor="w", text_color=Colors.TEXT_PRIMARY)
                lbl.grid(row=0, column=i, padx=5, pady=5, sticky="w")
            
        self.list_frame = ctk.CTkScrollableFrame(self, fg_color="transparent")
        self.list_frame.grid(row=3, column=0, sticky="nsew", padx=20, pady=(0, 20))
        self.grid_rowconfigure(3, weight=1)
        
        self.refresh_list()

    def setup_form(self):
        # 2 Columns of inputs
        self.form_frame.grid_columnconfigure(0, weight=1)
        self.form_frame.grid_columnconfigure(1, weight=1)
        
        ctk.CTkLabel(self.form_frame, text="Supplier Name", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=0, padx=20, pady=(10, 2), sticky="w")
        self.entry_name = ctk.CTkEntry(self.form_frame, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_name.grid(row=1, column=0, padx=20, pady=(0, 10), sticky="ew")
        
        ctk.CTkLabel(self.form_frame, text="Contact Person", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=1, padx=20, pady=(10, 2), sticky="w")
        self.entry_contact = ctk.CTkEntry(self.form_frame, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_contact.grid(row=1, column=1, padx=20, pady=(0, 10), sticky="ew")
        
        ctk.CTkLabel(self.form_frame, text="Email", text_color=Colors.TEXT_PRIMARY).grid(row=2, column=0, padx=20, pady=(0, 2), sticky="w")
        self.entry_email = ctk.CTkEntry(self.form_frame, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_email.grid(row=3, column=0, padx=20, pady=(0, 10), sticky="ew")
        
        ctk.CTkLabel(self.form_frame, text="Phone", text_color=Colors.TEXT_PRIMARY).grid(row=2, column=1, padx=20, pady=(0, 2), sticky="w")
        self.entry_phone = ctk.CTkEntry(self.form_frame, fg_color=Colors.BG_INPUT, text_color=Colors.TEXT_INPUT, border_color=Colors.BORDER)
        self.entry_phone.grid(row=3, column=1, padx=20, pady=(0, 10), sticky="ew")
        
        btn_box = ctk.CTkFrame(self.form_frame, fg_color="transparent")
        btn_box.grid(row=4, column=0, columnspan=2, pady=15)
        
        self.btn_save = ctk.CTkButton(btn_box, text="Save Supplier", command=self.save_supplier, fg_color="green")
        self.btn_save.pack(side="left", padx=10)
        
        ctk.CTkButton(btn_box, text="Cancel", command=self.cancel_form, fg_color="gray").pack(side="left", padx=10)

    def toggle_form(self):
        if self.form_frame.winfo_viewable():
            self.form_frame.grid_forget()
            self.btn_add_toggle.configure(text="+ Add Supplier", fg_color="green")
            self.editing_supplier = None
            self.clear_entries()
        else:
            self.form_frame.grid(row=1, column=0, sticky="ew", padx=20, pady=10)
            self.btn_add_toggle.configure(text="- Close Form", fg_color="gray")

    def edit_supplier(self, supplier):
        self.editing_supplier = supplier
        # Show form
        if not self.form_frame.winfo_viewable():
            self.toggle_form()
            
        # Fill data
        self.entry_name.delete(0, "end"); self.entry_name.insert(0, supplier.name)
        self.entry_contact.delete(0, "end"); self.entry_contact.insert(0, supplier.contact_info)
        self.entry_email.delete(0, "end"); self.entry_email.insert(0, supplier.email or "")
        self.entry_phone.delete(0, "end"); self.entry_phone.insert(0, supplier.phone or "")
        
        self.btn_save.configure(text="Update Supplier")
        self.btn_add_toggle.configure(text="Editing Mode")

    def cancel_form(self):
        self.toggle_form()

    def clear_entries(self):
        self.entry_name.delete(0, "end")
        self.entry_contact.delete(0, "end")
        self.entry_email.delete(0, "end")
        self.entry_phone.delete(0, "end")
        self.btn_save.configure(text="Save Supplier")

    def save_supplier(self):
        name = self.entry_name.get().strip()
        if not name:
             messagebox.showerror("Error", "Name required")
             return
             
        new_s = Supplier(
            supplier_id=self.editing_supplier.supplier_id if self.editing_supplier else 0,
            name=name,
            contact_info=self.entry_contact.get(),
            email=self.entry_email.get(),
            phone=self.entry_phone.get()
        )
        
        try:
            if self.editing_supplier:
                self.db.update_supplier(new_s)
                messagebox.showinfo("Success", "Supplier Updated")
            else:
                self.db.add_supplier(new_s)
                messagebox.showinfo("Success", "Supplier Added")
            
            self.refresh_list()
            self.cancel_form()
        except Exception as e:
            messagebox.showerror("Error", f"Could not save supplier.\n{e}")

    def delete_supplier_event(self, supplier):
        if messagebox.askyesno("Confirm Delete", f"Are you sure you want to delete supplier '{supplier.name}'?"):
            try:
                self.db.delete_supplier(supplier.supplier_id)
                messagebox.showinfo("Success", "Supplier Deleted")
                self.refresh_list()
            except Exception as e:
                messagebox.showerror("Error", f"Could not delete supplier.\n{e}")

    def refresh_list(self):
        for widget in self.list_frame.winfo_children():
            widget.destroy()
        
        self.suppliers = self.db.get_all_suppliers(active_only=not self.show_archived)
        
        for i, s in enumerate(self.suppliers):
            bg = Colors.BG_CARD if i % 2 == 0 else Colors.BG_HOVER
            row = ctk.CTkFrame(self.list_frame, fg_color=bg, corner_radius=6)
            row.pack(fill="x", pady=2)
            
            # Use same config
            for col_idx, (name, width, weight) in enumerate(self.col_config):
                row.grid_columnconfigure(col_idx, weight=weight)
            
            # Values
            vals = [
                str(s.supplier_id),
                s.name,
                s.contact_info,
                s.email or "-",
                s.phone or "-"
            ]
            
            # Render Cols 0-4
            for c, val in enumerate(vals):
                w = self.col_config[c][1]
                weight = self.col_config[c][2]
                
                if weight == 1:
                    ctk.CTkLabel(row, text=val, anchor="w", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=c, padx=5, pady=8, sticky="ew")
                else:
                    ctk.CTkLabel(row, text=val, width=w, anchor="w", text_color=Colors.TEXT_PRIMARY).grid(row=0, column=c, padx=5, pady=8, sticky="w")
            
            # Action Button (Col 5)
            # Edit
            # Delete (Added)
            # Remove "Delete" button if active, replace with Archive? Or keep both?
            # User request: "Archiving is the perfect solution". 
            # We keep Delete (for unused) and Archive (for used).
            
            # Enable Actions only for Admin
            if self.user.role == "Admin":
                ctk.CTkButton(row, text="Edit", width=60, height=24, fg_color="gray40", 
                              command=lambda x=s: self.edit_supplier(x)).grid(row=0, column=5, padx=5, pady=8, sticky="w")
                
                if s.is_active:
                    # Archive
                    ctk.CTkButton(row, text="Archive", width=60, height=24, fg_color=Colors.WARNING, text_color="black",
                                  command=lambda x=s: self.archive_supplier_event(x)).grid(row=0, column=6, padx=5, pady=8, sticky="w")
                else:
                    # Restore
                    ctk.CTkButton(row, text="Restore", width=60, height=24, fg_color=Colors.SUCCESS, 
                                  command=lambda x=s: self.restore_supplier_event(x)).grid(row=0, column=6, padx=5, pady=8, sticky="w")

    def archive_supplier_event(self, supplier):
        if messagebox.askyesno("Archive", f"Archive supplier '{supplier.name}'?"):
            self.db.archive_supplier(supplier.supplier_id)
            self.refresh_list()

    def restore_supplier_event(self, supplier):
        self.db.restore_supplier(supplier.supplier_id)
        self.refresh_list()

    def toggle_archived_view(self):
        self.show_archived = bool(self.switch_archive.get())
        self.refresh_list()
