using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventorySystem
{
    /// <summary>
    /// The application's entry point form, responsible for user authentication.
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly Color buttonIdleColor = Color.White;
        private readonly Color buttonHoverColor = Color.WhiteSmoke;
        private readonly DatabaseRepository _repository;

        /// <summary>
        /// Initializes the login form and its database repository.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
            _repository = new DatabaseRepository();
        }

        /// <summary>
        /// Handles the login button click event. Validates credentials and directs the user based on their role.
        /// </summary>
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user = _repository.GetUserByUsername(username);

            // Verify that the user exists and the provided password matches the stored hash.
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                this.Hide(); // Hide the login form before opening the main application window.

                if (user.Role == "Admin")
                {
                    // For Admins, open the main Dashboard.
                    DashboardForm adminDashboard = new DashboardForm(user);
                    adminDashboard.FormClosed += (s, args) => this.Close(); // Ensure the application exits when the dashboard is closed.
                    adminDashboard.Show();
                }
                else // Any other role (e.g., "Staff").
                {
                    // For Staff, open the more restricted main transaction form.
                    Form1 mainForm = new Form1(user);
                    mainForm.FormClosed += (s, args) => this.Close(); // Ensure the application exits when the main form is closed.
                    mainForm.Show();
                }
            }
            else
            {
                // If authentication fails, show an error message.
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the cancel button click event, terminating the application.
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Toggles the visibility of the password text in the password field.
        /// </summary>
        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Use the null character '\0' to show the password, and '*' to hide it.
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void BtnLogin_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonHoverColor;
        }

        private void BtnLogin_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonIdleColor;
        }
    }
}