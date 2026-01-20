using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace InventorySystem
{
    /// <summary>
    /// A form for viewing, managing, and acting on system-generated notifications.
    /// </summary>
    public partial class NotificationsForm : Form
    {
        private Color buttonIdleColor = Color.White;
        private Color buttonHoverColor = Color.WhiteSmoke;
        private readonly DatabaseRepository _repository;
        private List<Notification> _allNotifications;
        private readonly User _loggedInUser;
        private int _selectedProductId = -1; // -1 means no valid product is selected

        #region Initialization

        public NotificationsForm(User user)
        {
            InitializeComponent();
            _repository = new DatabaseRepository();

            // This line is the key: it saves the user that was passed in.
            _loggedInUser = user;
            dgvNotifications.AutoGenerateColumns = false;
        }


        private void NotificationsForm_Load(object sender, EventArgs e)
        {
            // 1. Set up the grid's visual properties first.
            dgvNotifications.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvNotifications.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // 2. Load the data from the database.
            _allNotifications = _repository.GetNotifications();

            // 3. NOW set the filter. This will call ApplyFilter(), but now _allNotifications has data.
            cmbFilter.SelectedIndex = 1; // "Unread Only"

            // 4. Update button states based on the initial load.
            UpdateButtonStates();
        }

        #endregion

        #region Data and UI Logic

        /// <summary>
        /// Fetches the latest notifications from the database and triggers the UI refresh.
        /// </summary>
        private void LoadAndDisplayNotifications()
        {
            _allNotifications = _repository.GetNotifications();
            ApplyFilter();
            UpdateButtonStates();
        }

        /// <summary>
        /// Filters the master notification list based on the user's selection and updates the grid.
        /// </summary>
        private void ApplyFilter()
        {
            if (_allNotifications == null) return;

            // Filter based on the combobox selection ("All" or "Unread Only").
            List<Notification> filteredList = (cmbFilter.SelectedIndex == 1)
                ? _allNotifications.Where(n => !n.IsRead).ToList()
                : _allNotifications.ToList();

            // --- IMPROVEMENT: More robust data binding ---
            // Binding to null first helps ensure the grid fully refreshes its visual state.
            dgvNotifications.DataSource = null;
            dgvNotifications.DataSource = filteredList;
            CustomizeGridView();
        }

        /// <summary>
        /// Applies visual styling to the grid, such as hiding columns and bolding unread rows.
        /// </summary>
        private void CustomizeGridView()
        {
            if (dgvNotifications.Columns.Count == 0) return;

            // Hide internal ID columns.
            dgvNotifications.Columns["NotificationID"].Visible = false;
            dgvNotifications.Columns["ProductID"].Visible = false;

            // Style unread rows to make them stand out.
            foreach (DataGridViewRow row in dgvNotifications.Rows)
            {
                if (row.DataBoundItem is Notification notification && !notification.IsRead)
                {
                    row.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
                }
            }
        }

        /// <summary>
        /// Enables or disables action buttons based on whether a row is selected in the grid.
        /// </summary>
        private void UpdateButtonStates()
        {
            bool isRowSelected = dgvNotifications.SelectedRows.Count > 0;
            bool hasUnread = _allNotifications.Any(n => !n.IsRead);
            btnMarkAllAsRead.Enabled = hasUnread;
            btnViewProduct.Enabled = isRowSelected;
            btnMarkAsRead.Enabled = isRowSelected;
            btnDeleteNotification.Enabled = isRowSelected;
        }

        #endregion

        #region Event Handlers

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilter();
        private void dgvNotifications_SelectionChanged(object sender, EventArgs e)
        {
            // First, we will capture the Product ID.
            // Check if a valid row is currently selected.
            if (dgvNotifications.CurrentRow != null && dgvNotifications.CurrentRow.DataBoundItem is Notification selectedNotification)
            {
                // If a row is selected, store its ProductID in our class variable.
                _selectedProductId = selectedNotification.ProductID;
            }
            else
            {
                // If no row is selected, reset our stored ID to an invalid state.
                _selectedProductId = -1;
            }

            // Now, just like before, we call your helper method to update the button states.
            UpdateButtonStates();
        }

        private void btnMarkAsRead_Click(object sender, EventArgs e)
        {
            // --- IMPROVEMENT: Guard clause for robustness ---
            if (dgvNotifications.SelectedRows.Count == 0 || !(dgvNotifications.SelectedRows[0].DataBoundItem is Notification selected))
            {
                return;
            }

            _repository.MarkNotificationAsRead(selected.NotificationID);
            // --- IMPROVEMENT: Optimized refresh ---
            // Instead of re-querying the database, just update the in-memory object.
            selected.IsRead = true;
            ApplyFilter(); // Re-apply the filter to the updated in-memory list.
            UpdateButtonStates();
        }

        private void btnMarkAllAsRead_Click(object sender, EventArgs e)
        {
            // --- IMPROVEMENT: Added confirmation for a bulk action ---
            var confirm = MessageBox.Show("Are you sure you want to mark all notifications as read?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No)
            {
                return;
            }

            _repository.MarkAllNotificationsAsRead();
            LoadAndDisplayNotifications();
        }

        private void btnDeleteNotification_Click(object sender, EventArgs e)
        {
            // --- IMPROVEMENT: Guard clause for robustness ---
            if (dgvNotifications.SelectedRows.Count == 0 || !(dgvNotifications.SelectedRows[0].DataBoundItem is Notification selected))
            {
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this notification?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                _repository.DeleteNotification(selected.NotificationID);
                // --- IMPROVEMENT: Optimized refresh ---
                // Remove the item from the in-memory list before refreshing the grid.
                _allNotifications.Remove(selected);
                ApplyFilter();
                UpdateButtonStates();
            }
        }

        /// <summary>
        /// Navigates the user directly to the main inventory form and highlights the relevant product.

        private void btnViewProduct_Click(object sender, EventArgs e)
        {
            // Safety check: make sure a row is selected.
            if (dgvNotifications.CurrentRow == null) return;

            var selectedNotification = dgvNotifications.CurrentRow.DataBoundItem as Notification;
            if (selectedNotification == null) return;

            try
            {
                // Use our new repository methods to get the full details.
                Product product = _repository.GetProductById(selectedNotification.ProductID);
                Transaction transaction = _repository.GetLatestTransactionForProduct(selectedNotification.ProductID);

                // If we found everything, create and show the new dialog.
                if (product != null && transaction != null)
                {
                    using (var detailForm = new NotificationDetailForm(product, transaction))
                    {
                        detailForm.ShowDialog(this); // ShowDialog makes it a modal pop-up
                    }
                }
                else
                {
                    MessageBox.Show("Could not find the complete details for this notification.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBackToDashboard_Click(object sender, EventArgs e)
        {
            // The existing logic here is excellent ("Active Refresh Pattern").
            if (this.Owner is DashboardForm ownerDashboard)
            {
                ownerDashboard.LoadDashboardData();
            }
            this.Owner?.Show();
            this.Close();
        }

        #endregion

        private void btnViewProduct_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonHoverColor;
        }

        private void btnViewProduct_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonIdleColor;
        }

    }
}