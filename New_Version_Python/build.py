import PyInstaller.__main__
import customtkinter
import os
import sys

def build():
    # Get CTk path
    ctk_path = os.path.dirname(customtkinter.__file__)
    print(f"CustomTkinter Path: {ctk_path}")

    # Define separator for --add-data (Windows uses ;)
    sep = ";" if os.name == "nt" else ":"

    # Base Args
    args = [
        "main.py",
        "--name=InventorySystem",
        "--noconfirm",
        "--windowed", # Hide console
        "--onefile",  # Single exe
        "--clean",
        # Include CustomTkinter Theme Data
        f"--add-data={ctk_path}{sep}customtkinter/",
        "--hidden-import=reportlab.graphics.barcode.code128",
        "--hidden-import=reportlab.graphics.barcode.code93",
        "--hidden-import=reportlab.graphics.barcode.code39",
        "--hidden-import=reportlab.graphics.barcode.common",
        "--hidden-import=reportlab.graphics.barcode.usps",
        "--hidden-import=reportlab.graphics.barcode.usps4s",
        "--hidden-import=reportlab.graphics.barcode.qr",
        "--hidden-import=reportlab.graphics.barcode.dmtx",
        "--hidden-import=reportlab.graphics.barcode.eanbc",
        "--hidden-import=reportlab.graphics.barcode.ecc200datamatrix",
        "--hidden-import=reportlab.graphics.barcode.fourstate",
        "--hidden-import=reportlab.graphics.barcode.lto",
        "--hidden-import=reportlab.graphics.barcode.qrencoder",
        "--hidden-import=reportlab.graphics.barcode.widgets",
    ]

    # Add DB file if we want it embedded? No, user data should be external.
    # But we might want to ensure it works if missing.
    # The code handles creation if missing.
    
    # Check for Icon
    icon_path = os.path.join("assets", "icon.ico")
    if os.path.exists(icon_path):
        print(f"Using Icon: {icon_path}")
        args.append(f"--icon={icon_path}")
    else:
        print("No icon.ico found in assets/, using default.")

    print("Starting Build...")
    PyInstaller.__main__.run(args)
    print("Build Complete. Check dist/ folder.")

if __name__ == "__main__":
    build()
