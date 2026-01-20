
from typing import List, Optional
import bcrypt
from models import User

class UserRepositoryMixin:
    # Requires self.get_connection() from BaseRepository

    def get_user_by_username(self, username: str) -> Optional[User]:
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Users WHERE Username = ?", (username,))
            row = cursor.fetchone()
            if row:
                return User(
                    user_id=row["UserID"],
                    username=row["Username"],
                    password_hash=row["PasswordHash"],
                    role=row["Role"],
                    contact_number=row["ContactNumber"] if row["ContactNumber"] else None
                )
        return None

    def add_user(self, user: User):
        with self.get_connection() as conn:
            conn.execute(
                "INSERT INTO Users (Username, PasswordHash, Role, ContactNumber) VALUES (?, ?, ?, ?)",
                (user.username, user.password_hash, user.role, user.contact_number)
            )

    def get_all_users(self) -> List[User]:
        users = []
        with self.get_connection() as conn:
            cursor = conn.execute("SELECT * FROM Users")
            for row in cursor:
                users.append(User(
                    user_id=row["UserID"],
                    username=row["Username"],
                    password_hash=row["PasswordHash"],
                    role=row["Role"],
                    contact_number=row["ContactNumber"] if row["ContactNumber"] else None
                ))
        return users

    def update_user(self, user: User):
         with self.get_connection() as conn:
            if user.password_hash:
                conn.execute(
                    "UPDATE Users SET Username = ?, PasswordHash = ?, Role = ?, ContactNumber = ? WHERE UserID = ?",
                    (user.username, user.password_hash, user.role, user.contact_number, user.user_id)
                )
            else:
                 conn.execute(
                    "UPDATE Users SET Username = ?, Role = ?, ContactNumber = ? WHERE UserID = ?",
                    (user.username, user.role, user.contact_number, user.user_id)
                )

    def delete_user(self, user_id: int):
        with self.get_connection() as conn:
            conn.execute("DELETE FROM Users WHERE UserID = ?", (user_id,))
