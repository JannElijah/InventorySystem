import os
import sys
import subprocess

def print_file(filepath: str):
    """
    Sends a file to the default printer using the operating system's shell.
    This works for PDF, TXT, PNG, etc. by invoking the associated application's print command
    or the 'print' verb of the OS shell.

    :param filepath: Absolute path to the file to be printed.
    """
    if not os.path.exists(filepath):
        print(f"Error: File not found for printing: {filepath}")
        return False

    try:
        if os.name == 'nt': # Windows
            # os.startfile with "print" verb is the standard way to print standard filetypes on Windows
            # This relies on the file extension having a "print" association (PDFs usually do via Acrobat/Edge)
            try:
                os.startfile(filepath, "print")
            except OSError as e:
                # If no association (WinError 1155), try to generic open it for manual printing
                if hasattr(e, 'winerror') and e.winerror == 1155:
                    print("No print association found. Opening file for manual printing.")
                    os.startfile(filepath)
                else:
                    # Fallback: Powershell for other errors
                    # -WindowStyle Hidden attempts to suppress the window, though Acrobat/Reader might still flash
                    subprocess.run(
                        ['powershell', 'Start-Process', '-FilePath', f'"{filepath}"', '-Verb', 'Print', '-WindowStyle', 'Hidden'], 
                        check=True
                    )
            return True
        else:
            # Linux/Unix (CUPS) - Fallback for future compatibility
            # 'lp' is the standard command for printing files
            subprocess.run(['lp', filepath], check=True)
            return True
    except Exception as e:
        print(f"Printing Error: {e}")
        return False

def print_receipt_thermal(receipt_text: str, printer_ip: str = None):
    """
    Stub for Direct Thermal Printing (ESC/POS).
    If python-escpos is available, this would use it.
    Otherwise, it logs or prints to console.
    """
    try:
        # Check if library exists (Optimization: Lazy import)
        try:
            from escpos.printer import Network, Usb
            # Implementation example:
            # p = Network(printer_ip)
            # p.text(receipt_text)
            # p.cut()
            print("Printing to thermal printer via escpos...")
        except ImportError:
            # Fallback
            print("Thermal Printer Lib not found. Simulating print:")
            print("-" * 30)
            print(receipt_text)
            print("-" * 30)
            print("(Cut Paper)")
        return True
    except Exception as e:
        print(f"Thermal Print Error: {e}")
        return False
