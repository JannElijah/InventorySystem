using System;
using System.Windows.Forms;

namespace InventorySystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Includes a one-time check to create a default admin user if none exist.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DatabaseRepository repo = new DatabaseRepository();

            // Ensures a default administrator account exists on the first run of the application.
            if (repo.GetUserByUsername("admin") == null)
            {
                User adminUser = new User
                {
                    Username = "admin",
                    Role = "Admin",
                    // The default password is "password123".
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                };

                repo.AddUser(adminUser);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}