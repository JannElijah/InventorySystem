using System;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class NotificationDetailForm : Form
    {
        // This is the constructor that receives the necessary information.
        public NotificationDetailForm(Product product, Transaction transaction)
        {
            InitializeComponent();

            // Check for nulls as a safety measure
            if (product == null || transaction == null)
            {
                MessageBox.Show("Could not retrieve all the necessary details.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Populate all the labels with the data we received.
            this.Text = "Notification Details"; // Set the window title
            lblProductName.Text = product.Description;
            lblBarcodeValue.Text = product.Barcode;
            lblTransactionTypeValue.Text = transaction.TransactionType;
            lblStockBeforeValue.Text = transaction.StockBefore.ToString();
            lblStockAfterValue.Text = transaction.StockAfter.ToString();
            lblTimestampValue.Text = transaction.TransactionDate.ToString("g"); // 'g' is for general date/time format
        }

        // This event handler is for the "OK" button.
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Because we set the DialogResult property, this will automatically close the form.
            this.Close();
        }
    }
}