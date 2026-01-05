
import os
import shutil
import glob
from datetime import datetime

class BackupManager:
    @staticmethod
    def perform_backup(db_path: str, backup_dir: str = "backups", max_backups: int = 10):
        """
        Creates a time-stamped copy of the database.
        Keeps only the 'max_backups' most recent files.
        """
        try:
            if not os.path.exists(db_path):
                print(f"Backup skipped: Database not found at {db_path}")
                return

            # Ensure backup dir exists
            if not os.path.exists(backup_dir):
                os.makedirs(backup_dir)

            # Generate filename
            timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
            filename = os.path.basename(db_path)
            name, ext = os.path.splitext(filename)
            backup_filename = f"{name}_{timestamp}{ext}"
            backup_path = os.path.join(backup_dir, backup_filename)

            # Copy file
            shutil.copy2(db_path, backup_path)
            print(f"Backup created: {backup_path}")

            # Cleanup old backups
            BackupManager.cleanup_old_backups(backup_dir, name, ext, max_backups)
            return True

        except Exception as e:
            print(f"Backup failed: {e}")
            return False

    @staticmethod
    def cleanup_old_backups(backup_dir, name, ext, max_backups):
        """
        Deletes oldest backups if count exceeds max_backups.
        """
        try:
            # Pattern to match backups: name_*.ext
            pattern = os.path.join(backup_dir, f"{name}_*{ext}")
            files = sorted(glob.glob(pattern), key=os.path.getmtime)
            
            if len(files) > max_backups:
                to_delete = files[:len(files) - max_backups]
                for f in to_delete:
                    try:
                        os.remove(f)
                        print(f"Deleted old backup: {f}")
                    except OSError as e:
                        print(f"Error deleting old backup {f}: {e}")
        except Exception as e:
             print(f"Cleanup error: {e}")
