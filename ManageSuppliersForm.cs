using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class ManageSuppliersForm : Form
    {
        // These colors will control our button hover effect
        private Color buttonIdleColor = Color.White;
        private Color buttonHoverColor = Color.WhiteSmoke;
        private readonly DatabaseRepository _repository;

        public ManageSuppliersForm()
        {
            InitializeComponent();
            _repository = new DatabaseRepository();
        }

        private void ManageSuppliersForm_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        /// <summary>
        /// Fetches all suppliers from the database and refreshes the DataGridView.
        /// </summary>
        private void LoadSuppliers()
        {
            try
            {
                var suppliers = _repository.GetAllSuppliers();
                dgvSuppliers.DataSource = suppliers;

                if (dgvSuppliers.Columns["SupplierID"] != null)
                {
                    dgvSuppliers.Columns["SupplierID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load suppliers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddEditSupplierForm(_repository))
            {
                var result = addForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    LoadSuppliers(); // A full reload is best after an add to get the new ID.
                }
            }
        }

        private void btnEditSupplier_Click(object sender, EventArgs e)
        {
            var selectedSupplier = GetSelectedSupplier();
            if (selectedSupplier == null) return;

            using (var editForm = new AddEditSupplierForm(_repository, selectedSupplier))
            {
                var result = editForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // --- IMPROVEMENT: More efficient refresh ---
                    // Instead of hitting the DB again, just refresh the grid's display.
                    dgvSuppliers.Refresh();
                }
            }
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            var selectedSupplier = GetSelectedSupplier();
            if (selectedSupplier == null) return;

            var confirmResult = MessageBox.Show($"Are you sure you want to delete '{selectedSupplier.Name}'?",
                                                 "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    _repository.DeleteSupplier(selectedSupplier.SupplierID);

                    // --- IMPROVEMENT: More efficient refresh ---
                    // Remove the item from the in-memory list for an instant UI update.
                    int selectedIndex = dgvSuppliers.CurrentRow.Index;
                    var supplierList = dgvSuppliers.DataSource as List<Supplier>;
                    supplierList?.Remove(selectedSupplier);

                    // Re-bind the modified list
                    dgvSuppliers.DataSource = null;
                    dgvSuppliers.DataSource = supplierList;

                    // --- IMPROVEMENT: Better UX by preserving selection ---
                    if (supplierList != null && supplierList.Any())
                    {
                        int newIndex = Math.Min(selectedIndex, supplierList.Count - 1);
                        if (newIndex >= 0)
                        {
                            dgvSuppliers.CurrentCell = dgvSuppliers.Rows[newIndex].Cells[0];
                            dgvSuppliers.Rows[newIndex].Selected = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- IMPROVEMENT: Robust double-click handling ---
        private void dgvSuppliers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the double-click was on a valid data row, not the header.
            if (e.RowIndex >= 0)
            {
                btnEditSupplier.PerformClick();
            }
        }

        private void btnBackToDashboard_Click(object sender, EventArgs e)
        {
            // --- IMPROVEMENT: Consistent "Active Refresh" navigation pattern ---
            if (this.Owner is DashboardForm ownerDashboard)
            {
                ownerDashboard.LoadDashboardData();
            }
            this.Owner?.Show();
            this.Close();
        }

        #region Helper Methods

        /// <summary>
        /// --- IMPROVEMENT: Centralized logic to get the selected supplier ---
        /// Gets the currently selected supplier from the grid, handling nulls and showing a message.
        /// </summary>
        private Supplier GetSelectedSupplier()
        {
            if (dgvSuppliers.CurrentRow == null)
            {
                MessageBox.Show("Please select a supplier.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            return dgvSuppliers.CurrentRow.DataBoundItem as Supplier;
        }

        #endregion

        private void btnAddSupplier_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonHoverColor;
        }

        private void btnAddSupplier_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonIdleColor;
        }

        private void btnEditSupplier_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonHoverColor;
        }

        private void btnEditSupplier_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonIdleColor;
        }

        private void btnDeleteSupplier_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonHoverColor;
        }

        private void btnDeleteSupplier_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = buttonIdleColor;
        }
    }
}