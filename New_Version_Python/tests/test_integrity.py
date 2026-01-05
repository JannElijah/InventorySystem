
import unittest
import os
import sys
import pkgutil
import importlib

# Add project root to sys.path
PROJECT_ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
sys.path.append(PROJECT_ROOT)

class TestModuleIntegrity(unittest.TestCase):
    def test_import_all_modules(self):
        """
        Dynamically find and import all Python modules in the project
        to ensure no SyntaxErrors or ImportErrors exist.
        """
        # Packages to test (folders with __init__.py or just logic)
        packages = ['database', 'models', 'ui', 'utils']
        
        failures = []
        
        for package_name in packages:
            package_path = os.path.join(PROJECT_ROOT, package_name)
            if not os.path.exists(package_path):
                continue
                
            # Walk through the directory
            for root, _, files in os.walk(package_path):
                for file in files:
                    if file.endswith(".py") and file != "__init__.py":
                        # Construct module path
                        rel_path = os.path.relpath(os.path.join(root, file), PROJECT_ROOT)
                        module_name = rel_path.replace(os.sep, ".")[:-3] # Remove .py
                        
                        try:
                            importlib.import_module(module_name)
                        except Exception as e:
                            # We record the failure but continue testing others
                            failures.append(f"{module_name}: {e}")
                            
        if failures:
            self.fail(f"Integrity Check Failed. The following modules could not be imported:\n" + "\n".join(failures))

if __name__ == '__main__':
    unittest.main()
