using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class AddEditUserForm : Form
    {
        private readonly Color _buttonIdleColor = Color.White;
        private readonly Color _buttonHoverColor = Color.WhiteSmoke;

        // --- MODIFIED: Repository is now a class-level field ---
        private readonly DatabaseRepository _repository;
        private readonly bool _isEditMode;

        public User TheUser { get; private set; }

        #region Constructors

        // --- MODIFIED: Constructor now requires the repository ---
        public AddEditUserForm(DatabaseRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            _isEditMode = false;
            this.Text = "Add New User";
        }

        public AddEditUserForm(DatabaseRepository repository, User userToEdit)
        {
            InitializeComponent();
            _repository = repository;
            _isEditMode = true;
            this.Text = "Edit User";
            TheUser = userToEdit;
        }

        #endregion

        #region Event Handlers

        private void AddEditUserForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.AddRange(new object[] { "Admin", "Staff" });

            if (_isEditMode)
            {
                txtUsername.Text = TheUser.Username;
                cmbRole.SelectedItem = TheUser.Role;
                // In Edit mode, the password is not required, so we can leave the textbox blank.
                // We'll only update the password if the user types a new one.
                lblPassword.Text = "New Password (optional):";
                txtContactNumber.Text = TheUser.ContactNumber;
            }
            else
            {
                cmbRole.SelectedItem = "Staff";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // --- STEP 1: GATHER ALL DATA ---
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string contactNumber = txtContactNumber.Text; // NEW: Get the contact number

            // --- STEP 2: PERFORM ALL VALIDATION ---

            // Username validation (existing)
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Password validation for new users (existing)
            if (!_isEditMode && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password is required for new users.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- NEW: Contact Number 11-Digit Validation ---
            // This check is only performed if the user has entered something.
            if (!string.IsNullOrEmpty(contactNumber) && contactNumber.Length != 11)
            {
                MessageBox.Show("Contact Number must be exactly 11 digits long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Username uniqueness validation (existing)
            bool usernameChanged = _isEditMode ? (username != TheUser.Username) : true;
            if (usernameChanged && _repository.UserExists(username))
            {
                MessageBox.Show("A user with this username already exists. Please choose a different name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- STEP 3: PROCESS AND SAVE DATA ---

            if (_isEditMode)
            {
                TheUser.Username = username;
                TheUser.Role = cmbRole.SelectedItem.ToString();
                TheUser.ContactNumber = contactNumber; // NEW: Update the contact number

                if (!string.IsNullOrWhiteSpace(password))
                {
                    TheUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                }
            }
            else // "Add" mode
            {
                TheUser = new User
                {
                    Username = username,
                    Role = cmbRole.SelectedItem.ToString(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    ContactNumber = contactNumber // NEW: Add the contact number
                };
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // --- NEW: Event handler for the "Show Password" checkbox ---
        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        #endregion

        #region Generic Hover Effects (Connect these in the Designer)

        private void AnyButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackColor = _buttonHoverColor;
            }
        }

        private void AnyButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackColor = _buttonIdleColor;
            }
        }

        #endregion

        private void TxtContactNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control characters (like backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Block the character
            }
        }
    }
}