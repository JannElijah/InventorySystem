using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace InventorySystem
{
    /// <summary>
    /// A form for Admins to perform CRUD operations on user accounts.
    /// </summary>
    public partial class UserManagementForm : Form
    {
        // --- REFINED: Centralized colors for consistency ---
        private readonly Color _buttonIdleColor = Color.FromArgb(240, 240, 240);
        private readonly Color _buttonHoverColor = Color.LightSteelBlue;
        private readonly DatabaseRepository _repository;
        private readonly User _adminUser;

        #region Initialization

        public UserManagementForm(User adminUser)
        {
            InitializeComponent();
            _repository = new DatabaseRepository();
            _adminUser = adminUser;
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            RefreshUserGrid();
            UpdateButtonStates(); // --- NEW: Set initial button states on load ---
        }

        #endregion

        #region Data Management

        private void RefreshUserGrid()
        {
            try
            {
                var users = _repository.GetAllUsers();
                dgvUsers.DataSource = new BindingSource { DataSource = users };

                // --- NEW (CRITICAL): Hide the password hash column for security ---
                if (dgvUsers.Columns.Contains("PasswordHash"))
                {
                    dgvUsers.Columns["PasswordHash"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region UI Event Handlers

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // Pass the repository to the constructor
            using (var form = new AddEditUserForm(_repository))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _repository.AddUser(form.TheUser);
                    RefreshUserGrid();
                }
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            var userToEdit = GetSelectedUser();
            if (userToEdit == null) return;

            // Pass both the repository and the user to the constructor
            using (var form = new AddEditUserForm(_repository, userToEdit))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _repository.UpdateUser(form.TheUser);
                    dgvUsers.Refresh();
                }
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            var selectedUser = GetSelectedUser();
            if (selectedUser == null) return;

            if (selectedUser.UserID == _adminUser.UserID)
            {
                MessageBox.Show("You cannot delete your own account while you are logged in.", "Action Prohibited", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show($"Are you sure you want to permanently delete the user '{selectedUser.Username}'?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmation == DialogResult.Yes)
            {
                _repository.DeleteUser(selectedUser.UserID);
                // Refresh the entire grid to ensure data consistency
                RefreshUserGrid();
            }
        }

        private void btnBackToDashboard_Click(object sender, EventArgs e)
        {
            if (this.Owner is DashboardForm ownerDashboard)
            {
                ownerDashboard.LoadDashboardData();
            }
            this.Owner?.Show();
            this.Close();
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditUser.PerformClick();
            }
        }

        // --- NEW: Manages the enabled state of context-sensitive buttons ---
        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        #endregion

        #region Helper Methods

        private User GetSelectedUser()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                // Message is now handled by UpdateButtonStates, making this cleaner
                return null;
            }
            return dgvUsers.SelectedRows[0].DataBoundItem as User;
        }

        // --- NEW: Centralized UI state logic ---
        private void UpdateButtonStates()
        {
            bool isRowSelected = dgvUsers.SelectedRows.Count > 0;
            btnEditUser.Enabled = isRowSelected;
            btnDeleteUser.Enabled = isRowSelected;
        }

        #endregion

        #region Generic Hover Effects (Connect these in the Designer)

        private void anyButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackColor = _buttonHoverColor;
            }
        }

        private void anyButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackColor = _buttonIdleColor;
            }
        }

        #endregion
    }
}