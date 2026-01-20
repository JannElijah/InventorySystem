import sys
import os

def get_app_path():
    """
    Returns the valid root directory of the application.
    Handles 'frozen' state (PyInstaller) vs script state.
    """
    if getattr(sys, 'frozen', False):
        # Running as compiled EXE
        # sys.executable is the path to the exe file
        return os.path.dirname(sys.executable)
    else:
        # Running as script
        # This file is in /utils/app_context.py
        # Parent = /utils
        # Parent of Parent = / (Root)
        return os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
