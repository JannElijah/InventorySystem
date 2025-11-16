using InventoryManagementSystem;
using InventorySystem;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

public class DatabaseRepository
{
    // The connection string now uses a relative path.
    // This allows the application to find the database file in its own folder, making it portable.
    private readonly string connectionString = "Data Source=inventory.db; Version=3;";

    #region User Management

    /// <summary>
    /// Adds a new user to the database with a pre-hashed password.
    /// </summary>
    public void AddUser(User user)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Users (Username, PasswordHash, Role, ContactNumber) VALUES (@username, @passwordHash, @role, @contactNumber)";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@role", user.Role);
                command.Parameters.AddWithValue("@contactNumber", (object)user.ContactNumber ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
        }
    }
    // Add this class definition above your DatabaseRepository class
    public class DailySummary
    {
        public int ItemsSupplied { get; set; }
        public int ItemsDelivered { get; set; }
        public int SupplyTransactionCount { get; set; }
        public int DeliveryTransactionCount { get; set; }
        public decimal CostOfSupplies { get; set; }
        public decimal GrossRevenue { get; set; }
    }

    // Add this new class definition in DatabaseRepository.cs
    public class ChartDataPointDecimal
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }

    // Add this class definition above your DatabaseRepository class
    public class ChartDataPoint
    {
        public string Label { get; set; } // e.g., "Product Name"
        public int Value { get; set; }    // e.g., "Total Quantity Sold"
    }

    /// <summary>
    /// Retrieves a user from the database by their username.
    /// </summary>
    /// <returns>A User object or null if not found.</returns>
    public User GetUserByUsername(string username)
    {
        User user = null;
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM Users WHERE Username = @username";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
        }
        return user;
    }
    /// <summary>
    /// Checks if a user with the given username already exists.
    /// </summary>
    public bool UserExists(string username)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT COUNT(1) FROM Users WHERE Username = @username";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                long count = (long)command.ExecuteScalar();
                return count > 0;
            }
        }
    }

    /// <summary>
    /// Retrieves a list of all users without their password hashes for security.
    /// </summary>
    public List<User> GetAllUsers()
    {
        var users = new List<User>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT UserID, Username, PasswordHash, Role, ContactNumber FROM Users";
            using (var command = new SQLiteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Username = reader["Username"].ToString(),
                            Role = reader["Role"].ToString(),
                            // PasswordHash is intentionally not loaded.
                            ContactNumber = reader["ContactNumber"] == DBNull.Value ? "" : reader["ContactNumber"].ToString() // --- ADDED ---
                        });
                    }
                }
            }
        }
        return users;
    }

    /// <summary>
    /// Updates a user's details, including their password hash.
    /// </summary>
    public void UpdateUser(User user)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // --- STEP 1: Build the SQL string dynamically ---
            // Start with the fields that are always updated.
            string sql = "UPDATE Users SET Username = @username, Role = @role, ContactNumber = @contactNumber ";

            // Check if a new password has been provided in the user object.
            // The AddEditUserForm logic ensures the hash is only set if the user typed a new password.
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                // If yes, add the PasswordHash update to the SQL string.
                sql += ", PasswordHash = @passwordHash ";
            }

            // --- STEP 2: ALWAYS add the WHERE clause to target the specific user ---
            sql += "WHERE UserID = @userID";

            using (var command = new SQLiteCommand(sql, connection))
            {
                // --- STEP 3: Add parameters that are always present ---
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@role", user.Role);
                command.Parameters.AddWithValue("@contactNumber", (object)user.ContactNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@userID", user.UserID);

                // --- STEP 4: Conditionally add the password parameter, matching the SQL string logic ---
                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                }

                command.ExecuteNonQuery();
            }
        }
    }
    // In DatabaseRepository.cs

    /// <summary>
    /// Gets the top 5 best-selling products by quantity sold over the last 30 days.
    /// </summary>
    public List<ChartDataPoint> GetTopSellingProducts()
    {
        var dataPoints = new List<ChartDataPoint>();
        string thirtyDaysAgo = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");

        // This SQL query finds all 'Delivery' transactions in the last 30 days,
        // groups them by product, sums the quantity change, orders by the highest sum,
        // and takes the top 5.
        string sql = @"
        SELECT
            P.Description AS ProductName,
            SUM(T.QuantityChange) AS TotalSold
        FROM Transactions T
        JOIN Products P ON T.ProductID = P.ProductID
        WHERE T.TransactionType = 'Delivery' AND DATE(T.TransactionDate) >= @thirtyDaysAgo
        GROUP BY T.ProductID
        ORDER BY TotalSold ASC  -- ASC because quantities are negative
        LIMIT 5;
    ";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@thirtyDaysAgo", thirtyDaysAgo);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataPoints.Add(new ChartDataPoint
                        {
                            Label = reader["ProductName"].ToString(),
                            Value = Math.Abs(Convert.ToInt32(reader["TotalSold"])) // Use Abs for a positive value
                        });
                    }
                }
            }
        }
        return dataPoints;
    }

    // In DatabaseRepository.cs

    /// <summary>
    /// Gets the total quantity of items sold per day over the last 30 days.
    /// </summary>
    public List<ChartDataPoint> GetDailySalesVolume()
    {
        var dataPoints = new List<ChartDataPoint>();
        string thirtyDaysAgo = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");

        // This query groups all delivery transactions by date and sums the items sold.
        string sql = @"
        SELECT
            DATE(TransactionDate) AS SaleDate,
            SUM(QuantityChange) AS TotalSold
        FROM Transactions
        WHERE TransactionType = 'Delivery' AND DATE(TransactionDate) >= @thirtyDaysAgo
        GROUP BY SaleDate
        ORDER BY SaleDate ASC; -- Order by date is crucial for a line chart
    ";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@thirtyDaysAgo", thirtyDaysAgo);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataPoints.Add(new ChartDataPoint
                        {
                            Label = Convert.ToDateTime(reader["SaleDate"]).ToString("MMM dd"), // Format date as "Nov 13"
                            Value = Math.Abs(Convert.ToInt32(reader["TotalSold"]))
                        });
                    }
                }
            }
        }
        return dataPoints;
    }
    public int AddSale(Sale sale)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Sales (DeliverTo, SaleDate, TotalAmount) VALUES (@DeliverTo, @SaleDate, @TotalAmount)";

            using (var command = new SQLiteCommand(sql, connection))
            {
                // Set the parameters for the INSERT command
                command.Parameters.AddWithValue("@DeliverTo", sale.CustomerName);
                command.Parameters.AddWithValue("@SaleDate", sale.SaleDate.ToString("o"));
                command.Parameters.AddWithValue("@TotalAmount", sale.Subtotal);

                // Step 1: Execute the INSERT. We use ExecuteNonQuery because it doesn't return a value.
                command.ExecuteNonQuery();
            }

            // Step 2: Now, execute a new command to get the ID of the row we JUST inserted.
            string idSql = "SELECT last_insert_rowid()";
            using (var idCommand = new SQLiteCommand(idSql, connection))
            {
                long saleId = (long)idCommand.ExecuteScalar();
                return (int)saleId;
            }
        }
    }

    // In your DatabaseRepository.cs file, replace the old AddSaleItem method with this one.

    public int AddSaleItem(int saleId, SaleItem item)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            // This command now includes LineTotal to match your database schema perfectly.
            string sql = @"INSERT INTO SaleItems (SaleID, ProductID, Quantity, UnitPrice, LineTotal) 
                       VALUES (@SaleID, @ProductID, @Quantity, @UnitPrice, @LineTotal);
                       SELECT last_insert_rowid();";

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SaleID", saleId);
                command.Parameters.AddWithValue("@ProductID", item.ProductID);
                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                command.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                // This is the final missing piece of data.
                command.Parameters.AddWithValue("@LineTotal", item.Total); // <-- CRITICAL FIX

                long saleItemId = (long)command.ExecuteScalar();
                return (int)saleItemId;
            }
        }
    }
    /// <summary>
    /// Deletes a user from the database by their ID.
    /// </summary>
    public void DeleteUser(int userId)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM Users WHERE UserID = @userID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@userID", userId);
                command.ExecuteNonQuery();
            }
        }
    }

    #endregion

    #region Product Management

    /// <summary>
    /// Retrieves a list of all products from the database.
    /// </summary>
    public List<Product> GetAllProducts()
    {
        List<Product> products = new List<Product>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            // UPDATED: Added DateCreated and DateModified to the SELECT list.
            string sql = "SELECT ProductID, Barcode, PartNumber, Brand, Description, Volume, Type, Application, PurchaseCost, SellingPrice, StockQuantity, Notes, LowStockThreshold, DateCreated, DateModified FROM Products";
            using (var command = new SQLiteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            // --- Existing Properties ---
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            StockQuantity = reader["StockQuantity"] is DBNull ? 0 : Convert.ToInt32(reader["StockQuantity"]),
                            PurchaseCost = reader["PurchaseCost"] is DBNull ? 0m : Convert.ToDecimal(reader["PurchaseCost"]),
                            SellingPrice = reader["SellingPrice"] is DBNull ? 0m : Convert.ToDecimal(reader["SellingPrice"]),
                            LowStockThreshold = reader["LowStockThreshold"] is DBNull ? 0 : Convert.ToInt32(reader["LowStockThreshold"]),
                            Barcode = reader["Barcode"] is DBNull ? "" : reader["Barcode"].ToString(),
                            PartNumber = reader["PartNumber"] is DBNull ? "" : reader["PartNumber"].ToString(),
                            Brand = reader["Brand"] is DBNull ? "" : reader["Brand"].ToString(),
                            Description = reader["Description"] is DBNull ? "" : reader["Description"].ToString(),
                            Volume = reader["Volume"] is DBNull ? "" : reader["Volume"].ToString(),
                            Type = reader["Type"] is DBNull ? "" : reader["Type"].ToString(),
                            Application = reader["Application"] is DBNull ? "" : reader["Application"].ToString(),
                            Notes = reader["Notes"] is DBNull ? "" : reader["Notes"].ToString(),

                            // --- NEW: Read the new audit trail properties ---
                            DateCreated = reader["DateCreated"] is DBNull ? "" : reader["DateCreated"].ToString(),
                            DateModified = reader["DateModified"] is DBNull ? "" : reader["DateModified"].ToString()
                        });
                    }
                }
            }
        }
        return products;
    }

    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    public void AddProduct(Product newProduct)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            // UPDATED: Added DateCreated and DateModified to the INSERT statement.
            string sql = @"INSERT INTO Products (Barcode, PartNumber, Brand, Description, Volume, Type, Application, PurchaseCost, SellingPrice, StockQuantity, Notes, LowStockThreshold, DateCreated, DateModified) 
                       VALUES (@barcode, @partNumber, @brand, @description, @volume, @type, @application, @purchaseCost, @sellingPrice, @stockQuantity, @notes, @lowStockThreshold, @dateCreated, @dateModified)";
            using (var command = new SQLiteCommand(sql, connection))
            {
                // --- Existing Parameters ---
                command.Parameters.AddWithValue("@barcode", newProduct.Barcode);
                command.Parameters.AddWithValue("@partNumber", newProduct.PartNumber);
                command.Parameters.AddWithValue("@brand", newProduct.Brand);
                command.Parameters.AddWithValue("@description", newProduct.Description);
                command.Parameters.AddWithValue("@volume", newProduct.Volume);
                command.Parameters.AddWithValue("@type", newProduct.Type);
                command.Parameters.AddWithValue("@application", newProduct.Application);
                command.Parameters.AddWithValue("@purchaseCost", newProduct.PurchaseCost);
                command.Parameters.AddWithValue("@sellingPrice", newProduct.SellingPrice);
                command.Parameters.AddWithValue("@stockQuantity", newProduct.StockQuantity);
                command.Parameters.AddWithValue("@notes", newProduct.Notes);
                command.Parameters.AddWithValue("@lowStockThreshold", newProduct.LowStockThreshold);

                // --- NEW: Add the new audit trail parameters ---
                // We set both to the current time, formatted as a standard sortable string.
                string now = DateTime.Now.ToString("o"); // ISO 8601 format (e.g., "2025-10-24T15:30:00.1234567")
                command.Parameters.AddWithValue("@dateCreated", now);
                command.Parameters.AddWithValue("@dateModified", now);

                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Updates all fields for a given product and triggers the notification check.
    /// </summary>
    public void UpdateProduct(Product product)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            // UPDATED: Added DateModified to the SET clause.
            string sql = @"UPDATE Products SET
                        Barcode = @Barcode, PartNumber = @PartNumber, Brand = @Brand, Description = @Description, 
                        Volume = @Volume, Type = @Type, Application = @Application, PurchaseCost = @PurchaseCost, 
                        SellingPrice = @SellingPrice, StockQuantity = @StockQuantity, Notes = @Notes, 
                        LowStockThreshold = @LowStockThreshold, DateModified = @DateModified
                      WHERE ProductID = @ProductID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                // --- Existing Parameters ---
                command.Parameters.AddWithValue("@Barcode", product.Barcode);
                command.Parameters.AddWithValue("@PartNumber", product.PartNumber);
                command.Parameters.AddWithValue("@Brand", product.Brand);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Volume", product.Volume);
                command.Parameters.AddWithValue("@Type", product.Type);
                command.Parameters.AddWithValue("@Application", product.Application);
                command.Parameters.AddWithValue("@PurchaseCost", product.PurchaseCost);
                command.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                command.Parameters.AddWithValue("@Notes", product.Notes);
                command.Parameters.AddWithValue("@LowStockThreshold", product.LowStockThreshold);
                command.Parameters.AddWithValue("@ProductID", product.ProductID);

                // --- NEW: Add the DateModified parameter ---
                command.Parameters.AddWithValue("@DateModified", DateTime.Now.ToString("o"));

                command.ExecuteNonQuery();
            }
        }

        // After the update is complete, check if a notification needs to be created.
        CheckStockLevelAndCreateNotification(product.ProductID, product.StockQuantity);
    }
    /// <summary>
    /// Gets a specialized list of transactions for the main dashboard view,
    /// joining with Products, Suppliers, and Sales to get all necessary details.
    /// </summary>
    /// <returns>A list of transactions formatted for the dashboard.</returns>
    // In DatabaseRepository.cs

    // PASTE THIS COMPLETE REPLACEMENT into DatabaseRepository.cs

    // PASTE THIS COMPLETE REPLACEMENT into DatabaseRepository.cs

    public List<DashboardTransactionView> GetDashboardTransactions()
    {
        var transactionViews = new List<DashboardTransactionView>();

        string sql = @"
        SELECT 
            t.TransactionID,
            t.ProductID,
            p.Barcode,
            p.Description AS ProductDescription,
            t.TransactionType,
            CASE 
                WHEN t.TransactionType = 'Delivery' THEN si.UnitPrice 
                ELSE p.PurchaseCost 
            END AS Price,
            s.DeliverTo AS CustomerName,
            t.StockBefore,
            t.StockAfter,
            t.TransactionDate,
            t.SupplierID,
            sup.Name AS SupplierName
        FROM 
            Transactions t
        INNER JOIN 
            Products p ON t.ProductID = p.ProductID
        LEFT JOIN 
            Suppliers sup ON t.SupplierID = sup.SupplierID
        LEFT JOIN 
            SaleItems si ON t.SaleItemID = si.SaleItemID
        LEFT JOIN 
            Sales s ON si.SaleID = s.SaleID
        ORDER BY 
            t.TransactionID DESC
        LIMIT 50;
    ";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // --- THIS IS THE CORRECTED PART ---
                        // Property names now correctly match your DashboardTransactionView class.
                        var view = new DashboardTransactionView
                        {
                            TransactionID = Convert.ToInt32(reader["TransactionID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            Barcode = reader["Barcode"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            TransactionType = reader["TransactionType"].ToString(),
                            Price = reader["Price"] is DBNull ? 0 : Convert.ToDecimal(reader["Price"]),
                            CustomerName = reader["CustomerName"] is DBNull ? "" : reader["CustomerName"].ToString(),
                            StockBefore = Convert.ToInt32(reader["StockBefore"]), // CORRECTED
                            StockAfter = Convert.ToInt32(reader["StockAfter"]),   // CORRECTED
                            TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                            SupplierID = reader["SupplierID"] is DBNull ? (int?)null : Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"] is DBNull ? "" : reader["SupplierName"].ToString()
                        };
                        transactionViews.Add(view);
                    }
                }
            }
        }
        return transactionViews;
    }

    // PASTE THIS ENTIRE NEW METHOD into DatabaseRepository.cs

    // ADD THIS ENTIRE NEW METHOD to DatabaseRepository.cs

    // PASTE THIS ENTIRE METHOD INTO YOUR DatabaseRepository.cs FILE

    public void ProcessCompleteSale(Sale sale)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Step 1: Save the Sale, using the correct column name 'DeliverTo'.
                    // --- FIX #1: Corrected column name from 'CustomerName' to 'DeliverTo' ---
                    string saleSql = "INSERT INTO Sales (DeliverTo, SaleDate) VALUES (@CustomerName, @SaleDate); SELECT last_insert_rowid();";
                    int newSaleId;
                    using (var saleCommand = new SQLiteCommand(saleSql, connection))
                    {
                        saleCommand.Parameters.AddWithValue("@CustomerName", sale.CustomerName);
                        saleCommand.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
                        newSaleId = Convert.ToInt32(saleCommand.ExecuteScalar());
                    }

                    foreach (var saleItem in sale.Items)
                    {
                        // Step 2: Save the SaleItem, now including the 'LineTotal'.
                        // --- FIX #2: Added LineTotal to the INSERT statement and parameters ---
                        string saleItemSql = "INSERT INTO SaleItems (SaleID, ProductID, Quantity, UnitPrice, LineTotal) VALUES (@SaleID, @ProductID, @Quantity, @UnitPrice, @LineTotal); SELECT last_insert_rowid();";
                        int newSaleItemId;
                        using (var saleItemCommand = new SQLiteCommand(saleItemSql, connection))
                        {
                            saleItemCommand.Parameters.AddWithValue("@SaleID", newSaleId);
                            saleItemCommand.Parameters.AddWithValue("@ProductID", saleItem.ProductID);
                            saleItemCommand.Parameters.AddWithValue("@Quantity", saleItem.Quantity);
                            saleItemCommand.Parameters.AddWithValue("@UnitPrice", saleItem.UnitPrice);
                            saleItemCommand.Parameters.AddWithValue("@LineTotal", saleItem.Total); // This line was added
                            newSaleItemId = Convert.ToInt32(saleItemCommand.ExecuteScalar());
                        }

                        // Step 3: Create the Transaction log with a negative QuantityChange for deliveries.
                        int stockBefore = saleItem.Product.StockQuantity;
                        int stockAfter = stockBefore - saleItem.Quantity;

                        // --- FIX #3: QuantityChange is now correctly stored as a negative number for deliveries ---
                        int quantityChange = -saleItem.Quantity;

                        string transactionSql = @"INSERT INTO Transactions (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, SaleItemID) VALUES (@ProductID, 'Delivery', @QuantityChange, @StockBefore, @StockAfter, @TransactionDate, @SaleItemID)";
                        using (var transactionCommand = new SQLiteCommand(transactionSql, connection))
                        {
                            transactionCommand.Parameters.AddWithValue("@ProductID", saleItem.ProductID);
                            transactionCommand.Parameters.AddWithValue("@QuantityChange", quantityChange); // Using the negative value
                            transactionCommand.Parameters.AddWithValue("@StockBefore", stockBefore);
                            transactionCommand.Parameters.AddWithValue("@StockAfter", stockAfter);
                            transactionCommand.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                            transactionCommand.Parameters.AddWithValue("@SaleItemID", newSaleItemId);
                            transactionCommand.ExecuteNonQuery();
                        }

                        // Step 4: Update the product's stock.
                        UpdateProductStock(saleItem.ProductID, stockAfter);
                    }

                    // If all steps succeeded, commit the changes to the database.
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // If any step failed, roll back all changes from this transaction.
                    transaction.Rollback();
                    throw; // Re-throw the exception to notify the UI
                }
            }
        }
    }
    // PASTE THIS NEW, SPECIALIZED METHOD INTO YOUR DatabaseRepository.cs FILE

    public void UpdateProductStock(int productId, int newStock)
    {
        string sql = "UPDATE Products SET StockQuantity = @newStock WHERE ProductID = @productId";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@newStock", newStock);
                command.Parameters.AddWithValue("@productId", productId);
                command.ExecuteNonQuery();
            }
        }

        // We also need to check for notifications after the stock is updated
        CheckStockLevelAndCreateNotification(productId, newStock);
    }

    /// <summary>
    /// Deletes a product from the database by its ID.
    /// </summary>
    public void DeleteProduct(int productID)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM Products WHERE ProductID = @productID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@productID", productID);
                command.ExecuteNonQuery();
            }
        }
    }

    #endregion

    #region Transaction Management

    /// <summary>
    /// Retrieves a list of all transactions, ordered from newest to oldest.
    /// </summary>
    // In DatabaseRepository.cs

    public List<Transaction> GetAllTransactions()
    {
        var transactions = new List<Transaction>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // --- CORRECTED SQL QUERY ---
            // This query now joins all three tables to gather all necessary information.
            string sql = @"SELECT 
                         t.TransactionID, t.ProductID, t.TransactionType, t.QuantityChange, 
                         t.StockBefore, t.StockAfter, t.TransactionDate, t.SupplierID,
                         p.Barcode, p.Description AS ProductDescription, 
                         s.Name AS SupplierName 
                       FROM Transactions t
                       LEFT JOIN Products p ON t.ProductID = p.ProductID
                       LEFT JOIN Suppliers s ON t.SupplierID = s.SupplierID
                       ORDER BY t.TransactionID DESC";

            using (var command = new SQLiteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(new Transaction
                        {
                            TransactionID = Convert.ToInt32(reader["TransactionID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),

                            // Read product-specific details from the joined Products table
                            Barcode = reader["Barcode"] == DBNull.Value ? "" : reader["Barcode"].ToString(),
                            ProductDescription = reader["ProductDescription"] == DBNull.Value ? "" : reader["ProductDescription"].ToString(),

                            TransactionType = reader["TransactionType"].ToString(),
                            QuantityChange = Convert.ToInt32(reader["QuantityChange"]),
                            StockBefore = Convert.ToInt32(reader["StockBefore"]),
                            StockAfter = Convert.ToInt32(reader["StockAfter"]),
                            TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),

                            // Handle nullable supplier data from the joined Suppliers table
                            SupplierID = reader["SupplierID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"] == DBNull.Value ? "" : reader["SupplierName"].ToString()
                        });
                    }
                }
            }
        }
        return transactions;
    }

    // PASTE THIS ENTIRE METHOD INTO YOUR DatabaseRepository.cs FILE


    public SalesReportData GetSalesReport(DateTime startDate, DateTime endDate)
    {
        var reportData = new SalesReportData();

        // This upgraded SQL query uses a Common Table Expression (CTE) to calculate all financial data, KPIs, 
        // and top performers in a single, efficient operation.
        string sql = @"
        -- First, create a temporary result set (CTE) containing only the relevant 'Delivery' transactions.
        WITH Deliveries AS (
            SELECT 
                T.ProductID, 
                T.QuantityChange,
                P.Description,
                P.SellingPrice,
                P.PurchaseCost
            FROM Transactions T
            JOIN Products P ON T.ProductID = P.ProductID
            WHERE T.TransactionType = 'Delivery' AND T.TransactionDate BETWEEN @startDate AND @endDate
        )
        -- Now, run calculations on that temporary result set.
        SELECT
            -- Financial Summary Calculations
            COALESCE(SUM(D.SellingPrice * D.QuantityChange), 0) AS TotalRevenue,
            COALESCE(SUM(D.PurchaseCost * D.QuantityChange), 0) AS CostOfGoodsSold,
            
            -- Additional KPI Calculations
            COUNT(*) AS TotalTransactions,
            COALESCE(SUM(D.QuantityChange), 0) AS TotalItemsSold,

            -- Subquery to find the Best-Selling Product (by total quantity sold)
            (SELECT Description FROM Deliveries GROUP BY ProductID ORDER BY SUM(QuantityChange) DESC LIMIT 1) AS BestSellerName,
            (SELECT SUM(QuantityChange) FROM Deliveries GROUP BY ProductID ORDER BY SUM(QuantityChange) DESC LIMIT 1) AS BestSellerValue,
            
            -- Subquery to find the Most Profitable Product (by total gross profit)
            (SELECT Description FROM Deliveries GROUP BY ProductID ORDER BY SUM((SellingPrice - PurchaseCost) * QuantityChange) DESC LIMIT 1) AS MostProfitableName,
            (SELECT SUM((SellingPrice - PurchaseCost) * QuantityChange) FROM Deliveries GROUP BY ProductID ORDER BY SUM((SellingPrice - PurchaseCost) * QuantityChange) DESC LIMIT 1) AS MostProfitableValue

        FROM Deliveries D;
    ";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Read Financials
                        reportData.TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"]);
                        reportData.CostOfGoodsSold = Convert.ToDecimal(reader["CostOfGoodsSold"]);

                        // Read KPIs
                        reportData.TotalTransactions = Convert.ToInt32(reader["TotalTransactions"]);
                        reportData.TotalItemsSold = Convert.ToInt32(reader["TotalItemsSold"]);

                        // Read Top Performers, checking for DBNull in case there were no sales
                        if (reader["BestSellerName"] != DBNull.Value)
                        {
                            reportData.BestSellingProduct.Name = reader["BestSellerName"].ToString();
                            reportData.BestSellingProduct.Value = Convert.ToDecimal(reader["BestSellerValue"]);
                        }

                        if (reader["MostProfitableName"] != DBNull.Value)
                        {
                            reportData.MostProfitableProduct.Name = reader["MostProfitableName"].ToString();
                            reportData.MostProfitableProduct.Value = Convert.ToDecimal(reader["MostProfitableValue"]);
                        }
                    }
                }
            }
        }

        // Calculate Gross Profit in C# for clarity.
        reportData.GrossProfit = reportData.TotalRevenue - reportData.CostOfGoodsSold;

        return reportData;
    }

    /// <summary>
    /// Adds a new transaction record to the database.
    /// </summary>
    // In DatabaseRepository.cs

    /// <summary>
    /// Logs a new transaction to the database, including supplier information if provided.
    /// </summary>
    public void AddTransaction(Transaction transaction)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // This SQL command is the final, complete version. It includes SaleItemID.
            string sql = @"INSERT INTO Transactions 
                     (ProductID, TransactionType, QuantityChange, StockBefore, StockAfter, TransactionDate, SupplierID, SaleItemID) 
                     VALUES 
                     (@ProductID, @TransactionType, @QuantityChange, @StockBefore, @StockAfter, @TransactionDate, @SupplierID, @SaleItemID)";

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ProductID", transaction.ProductID);
                command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                command.Parameters.AddWithValue("@QuantityChange", transaction.QuantityChange);
                command.Parameters.AddWithValue("@StockBefore", transaction.StockBefore);
                command.Parameters.AddWithValue("@StockAfter", transaction.StockAfter);
                command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);

                // This now correctly handles both nullable foreign keys.
                command.Parameters.AddWithValue("@SupplierID", (object)transaction.SupplierID ?? DBNull.Value);
                command.Parameters.AddWithValue("@SaleItemID", (object)transaction.SaleItemID ?? DBNull.Value); // <-- THIS LINE IS THE FIX

                command.ExecuteNonQuery();
            }
        }
    }

    public void BulkUpdateField(List<int> productIds, string fieldName, string newValue)
    {
        // Use a transaction for performance and safety.
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                // IMPORTANT: Sanitize the fieldName to prevent SQL injection.
                // Only allow updates to a specific list of columns.
                string safeFieldName;
                switch (fieldName.ToLower())
                {
                    case "brand":
                        safeFieldName = "Brand";
                        break;
                    case "low stock threshold":
                        safeFieldName = "LowStockThreshold";
                        break;
                    case "notes":
                        safeFieldName = "Notes";
                        break;
                    default:
                        // If the field name is not in our safe list, throw an exception.
                        throw new ArgumentException("Invalid field name provided for bulk update.");
                }

                // Construct the SQL command. Using the safeFieldName prevents injection.
                string sql = $"UPDATE Products SET {safeFieldName} = @newValue WHERE ProductID = @productID";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    // Loop through each product ID and execute the update command.
                    foreach (int id in productIds)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@newValue", newValue);
                        command.Parameters.AddWithValue("@productID", id);
                        command.ExecuteNonQuery();
                    }
                }

                // If all commands succeeded, commit the transaction to the database.
                transaction.Commit();
            }
        }


        // ... rest of the method is the same
    }
    // In DatabaseRepository.cs

    /// <summary>
    /// Gets the total sales revenue broken down by a specified product category.
    /// </summary>
    /// <param name="categoryColumn">The name of the column to group by (e.g., 'Type' or 'Brand').</param>
    public List<ChartDataPointDecimal> GetSalesBreakdownBy(string categoryColumn)
    {
        var dataPoints = new List<ChartDataPointDecimal>();
        string ninetyDaysAgo = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");

        // --- This switch statement is a security measure to prevent SQL injection ---
        // It ensures only valid column names can be used in the query.
        string safeCategoryColumn;
        switch (categoryColumn)
        {
            case "Type":
                safeCategoryColumn = "P.Type";
                break;
            case "Brand":
                safeCategoryColumn = "P.Brand";
                break;
            default:
                throw new ArgumentException("Invalid category for sales breakdown.");
        }

        // The query joins sales data with products, groups by the selected category,
        // and sums the total revenue for each group.
        string sql = $@"
        SELECT 
            {safeCategoryColumn} AS CategoryLabel,
            COALESCE(SUM(SI.LineTotal), 0) AS TotalRevenue
        FROM Transactions T
        JOIN SaleItems SI ON T.SaleItemID = SI.SaleItemID
        JOIN Products P ON T.ProductID = P.ProductID
        WHERE T.TransactionType = 'Delivery' AND DATE(T.TransactionDate) >= @ninetyDaysAgo
        GROUP BY CategoryLabel
        HAVING TotalRevenue > 0
        ORDER BY TotalRevenue DESC;
    ";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ninetyDaysAgo", ninetyDaysAgo);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataPoints.Add(new ChartDataPointDecimal
                        {
                            Label = reader["CategoryLabel"].ToString(),
                            Value = Convert.ToDecimal(reader["TotalRevenue"])
                        });
                    }
                }
            }
        }
        return dataPoints;
    }
    /// <summary>
    /// Retrieves a specified number of the most recent transactions.
    /// </summary>
    /// <summary>
    /// Retrieves a specified number of the most recent transactions, including supplier names.
    /// </summary>
    public List<Transaction> GetRecentTransactions(int limit)
    {
        var transactions = new List<Transaction>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            // Updated query to match the structure of GetAllTransactions
            string sql = $@"SELECT 
                          t.TransactionID, t.ProductID, t.Barcode, t.ProductDescription, 
                          t.TransactionType, t.QuantityChange, t.StockBefore, t.StockAfter, 
                          t.TransactionDate, t.SupplierID,
                          s.Name AS SupplierName 
                        FROM Transactions t
                        LEFT JOIN Suppliers s ON t.SupplierID = s.SupplierID
                        ORDER BY t.TransactionID DESC LIMIT {limit}";

            using (var command = new SQLiteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Full object mapping
                        transactions.Add(new Transaction
                        {
                            TransactionID = Convert.ToInt32(reader["TransactionID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            Barcode = reader["Barcode"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            TransactionType = reader["TransactionType"].ToString(),
                            QuantityChange = Convert.ToInt32(reader["QuantityChange"]),
                            StockBefore = Convert.ToInt32(reader["StockBefore"]),
                            StockAfter = Convert.ToInt32(reader["StockAfter"]),
                            TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                            SupplierID = reader["SupplierID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"] == DBNull.Value ? "" : reader["SupplierName"].ToString()
                        });
                    }
                }
            }
        }
        return transactions;
    }

    #endregion

    #region Dashboard Metrics

    // PASTE THIS ENTIRE METHOD INTO YOUR DatabaseRepository.cs FILE (Inside the #region Dashboard Metrics)

    /// <summary>
    /// Calculates the total number of items supplied and delivered for the current day.
    /// </summary>
    /// <returns>A tuple containing the count of items supplied and items delivered.</returns>
    // In DatabaseRepository.cs, REPLACE the old method with this one.

    /// <summary>
    /// Calculates a comprehensive summary of all activity for the current day.
    /// </summary>
    public DailySummary GetTodaysActivitySummary()
    {
        var summary = new DailySummary();
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Query 1: Get Supply stats (total items and transaction count)
            string supplyQuery = @"
            SELECT COALESCE(SUM(QuantityChange), 0), COUNT(TransactionID)
            FROM Transactions
            WHERE TransactionType = 'Supply' AND DATE(TransactionDate) = @today";
            using (var cmd = new SQLiteCommand(supplyQuery, connection))
            {
                cmd.Parameters.AddWithValue("@today", today);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        summary.ItemsSupplied = reader.GetInt32(0);
                        summary.SupplyTransactionCount = reader.GetInt32(1);
                    }
                }
            }

            // Query 2: Get Delivery stats (total items and transaction count)
            string deliveryQuery = @"
            SELECT COALESCE(SUM(QuantityChange), 0), COUNT(TransactionID)
            FROM Transactions
            WHERE TransactionType = 'Delivery' AND DATE(TransactionDate) = @today";
            using (var cmd = new SQLiteCommand(deliveryQuery, connection))
            {
                cmd.Parameters.AddWithValue("@today", today);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        summary.ItemsDelivered = Math.Abs(reader.GetInt32(0));
                        summary.DeliveryTransactionCount = reader.GetInt32(1);
                    }
                }
            }

            // Query 3: Get Financial stats (Revenue from deliveries and Cost of supplies)
            string financialQuery = @"
            SELECT
                COALESCE(SUM(CASE WHEN T.TransactionType = 'Delivery' THEN SI.UnitPrice ELSE 0 END), 0) as Revenue,
                COALESCE(SUM(CASE WHEN T.TransactionType = 'Supply' THEN P.PurchaseCost * T.QuantityChange ELSE 0 END), 0) as Cost
            FROM Transactions T
            LEFT JOIN SaleItems SI ON T.SaleItemID = SI.SaleItemID
            LEFT JOIN Products P ON T.ProductID = P.ProductID
            WHERE DATE(T.TransactionDate) = @today";
            using (var cmd = new SQLiteCommand(financialQuery, connection))
            {
                cmd.Parameters.AddWithValue("@today", today);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        summary.GrossRevenue = reader.GetDecimal(0);
                        summary.CostOfSupplies = reader.GetDecimal(1);
                    }
                }
            }
        }
        return summary;
    }



    /// <summary>
    /// Gets the total count of distinct products in the inventory.
    /// </summary>
    public int GetTotalProductCount()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT COUNT(*) FROM Products";
            using (var command = new SQLiteCommand(sql, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
    // PASTE THIS METHOD INTO YOUR DatabaseRepository.cs FILE

    /// <summary>
    /// Gets the 10 most recent transactions for a specific product ID.
    /// </summary>
    /// <param name="productId">The ID of the product to get history for.</param>
    /// <returns>A list of transactions formatted for the dashboard.</returns>
    public List<DashboardTransactionView> GetRecentTransactionsForProduct(int productId)
    {
        var transactionViews = new List<DashboardTransactionView>();

        // This query is optimized to get history for only one product.
        string sql = @"
    SELECT 
        t.TransactionID,
        t.ProductID,
        p.Barcode,
        p.Description AS ProductDescription,
        t.TransactionType,
        t.StockBefore AS PreviousStock,
        t.StockAfter AS CurrentStock,
        t.TransactionDate,
        t.SupplierID,
        sup.Name AS SupplierName,
        s.DeliverTo AS CustomerName,
        si.UnitPrice AS Price,
        t.QuantityChange,
        p.PurchaseCost
    FROM Transactions t
    INNER JOIN Products p ON t.ProductID = p.ProductID
    LEFT JOIN Suppliers sup ON t.SupplierID = sup.SupplierID
    LEFT JOIN SaleItems si ON t.SaleItemID = si.SaleItemID
    LEFT JOIN Sales s ON si.SaleID = s.SaleID
    WHERE t.ProductID = @productID  -- The critical filter
    ORDER BY t.TransactionDate DESC
    LIMIT 10;                     -- Limit to the last 10
";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@productID", productId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var view = new DashboardTransactionView
                        {
                            TransactionID = Convert.ToInt32(reader["TransactionID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            Barcode = reader["Barcode"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            TransactionType = reader["TransactionType"].ToString(),
                            Price = reader["Price"] is DBNull ? 0 : Convert.ToDecimal(reader["Price"]),
                            StockBefore = Convert.ToInt32(reader["PreviousStock"]),
                            StockAfter = Convert.ToInt32(reader["CurrentStock"]),
                            TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                            SupplierID = reader["SupplierID"] is DBNull ? (int?)null : Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"] is DBNull ? "" : reader["SupplierName"].ToString(),
                            CustomerName = reader["CustomerName"] is DBNull ? "" : reader["CustomerName"].ToString(),
                            QuantityChange = Convert.ToInt32(reader["QuantityChange"]),
                            PurchaseCost = Convert.ToDecimal(reader["PurchaseCost"])
                        };
                        transactionViews.Add(view);
                    }
                }
            }
        }
        return transactionViews;
    }


    /// <summary>
    /// Gets the count of products with stock levels considered "low" (e.g., between 1 and 10).
    /// </summary>
    public int GetLowStockCount()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            // THIS QUERY IS NOW CORRECT: It only counts items with stock between 1 and 10.
            string sql = "SELECT COUNT(*) FROM Products WHERE StockQuantity > 0 AND StockQuantity <= 10";
            using (var command = new SQLiteCommand(sql, connection))
            {
                // The result will be an integer, so we use Convert.ToInt32
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

    /// <summary>
    /// Gets the count of products that are completely out of stock (quantity is 0).
    /// </summary>
    public int GetOutOfStockCount()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT COUNT(*) FROM Products WHERE StockQuantity = 0";
            using (var command = new SQLiteCommand(sql, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

    /// <summary>
    /// Calculates the total monetary value of all inventory (PurchaseCost * StockQuantity).
    /// </summary>
    public decimal GetTotalInventoryValue()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT SUM(PurchaseCost * StockQuantity) FROM Products";
            using (var command = new SQLiteCommand(sql, connection))
            {
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }
    }

    #endregion

    #region Notification Management

    /// <summary>
    /// Retrieves notifications, joining with the Products table to include product names.
    /// </summary>
    /// <param name="includeRead">Whether to include notifications that have already been marked as read.</param>
    public List<Notification> GetNotifications(bool includeRead = true)
    {
        var notifications = new List<Notification>();
        string sql = @"
            SELECT
                n.NotificationID, n.ProductID, n.NotificationType, n.Message, n.IsRead, n.Timestamp,
                p.Brand || ' ' || p.Description AS ProductName
            FROM Notifications n
            JOIN Products p ON n.ProductID = p.ProductID";

        if (!includeRead)
        {
            sql += " WHERE n.IsRead = 0";
        }
        sql += " ORDER BY n.Timestamp DESC";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notifications.Add(new Notification
                        {
                            NotificationID = Convert.ToInt32(reader["NotificationID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"].ToString(),
                            NotificationType = reader["NotificationType"].ToString(),
                            Message = reader["Message"].ToString(),
                            IsRead = Convert.ToInt32(reader["IsRead"]) == 1,
                            Timestamp = DateTime.Parse(reader["Timestamp"].ToString())
                        });
                    }
                }
            }
        }
        return notifications;
    }
    public void UpdateTransactionWithSaleItemId(int transactionId, int saleItemId)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "UPDATE Transactions SET SaleItemID = @SaleItemID WHERE TransactionID = @TransactionID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SaleItemID", saleItemId);
                command.Parameters.AddWithValue("@TransactionID", transactionId);
                command.ExecuteNonQuery();
            }
        }
    }
    /// <summary>
    /// Gets the count of unread notifications.
    /// </summary>
    public int GetUnreadNotificationCount()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT COUNT(*) FROM Notifications WHERE IsRead = 0";
            using (var command = new SQLiteCommand(sql, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

    /// <summary>
    /// Marks a single notification as read.
    /// </summary>
    public void MarkNotificationAsRead(int notificationId)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "UPDATE Notifications SET IsRead = 1 WHERE NotificationID = @NotificationID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@NotificationID", notificationId);
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Marks all unread notifications as read.
    /// </summary>
    public void MarkAllNotificationsAsRead()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "UPDATE Notifications SET IsRead = 1 WHERE IsRead = 0";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Deletes a single notification from the database.
    /// </summary>
    public void DeleteNotification(int notificationId)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM Notifications WHERE NotificationID = @NotificationID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@NotificationID", notificationId);
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Checks a product's stock level against its threshold and creates a notification if necessary.
    /// </summary>
    // In DatabaseRepository.cs

    /// <summary>
    /// Checks the stock level of a product against its threshold and creates a notification if necessary.
    /// </summary>
    /// <param name="productId">The ID of the product to check.</param>
    /// <param name="newStock">The new, updated stock quantity.</param>
    private void CheckStockLevelAndCreateNotification(int productId, int newStock)
    {
        // First, get the product's rules and name from the database.
        string sql = "SELECT LowStockThreshold, Description, Brand FROM Products WHERE ProductID = @ProductID";
        int lowStockThreshold = 0;
        string productName = "";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ProductID", productId);
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return; // Product not found, cannot create notification.
                    }
                    lowStockThreshold = Convert.ToInt32(reader["LowStockThreshold"]);
                    productName = $"{reader["Brand"]} {reader["Description"]}";
                }
            }
        }

        // Now, perform the logic using the newStock value that was passed in.
        string notificationType = null;
        string message = null;

        if (newStock == 0)
        {
            notificationType = "Out of Stock";
            message = $"CRITICAL: '{productName}' is now out of stock.";
        }
        else if (newStock > 0 && newStock <= lowStockThreshold)
        {
            notificationType = "Low Stock";
            message = $"Low Stock Alert: '{productName}' has only {newStock} units remaining.";
        }

        // If a notification needs to be created...
        if (notificationType != null)
        {
            // ...and an unread one doesn't already exist...
            if (!HasExistingUnreadAlert(productId))
            {
                // ...create it.
                var newNotification = new Notification
                {
                    ProductID = productId,
                    NotificationType = notificationType,
                    Message = message,
                    IsRead = false,
                    Timestamp = DateTime.Now
                };
                AddNotification(newNotification);
            }
        }
    }

    // In DatabaseRepository.cs

    /// <summary>
    /// Inserts a new notification record into the database.
    /// This method is self-contained and manages its own database connection.
    /// </summary>
    /// <param name="notification">The notification object to save.</param>
    private void AddNotification(Notification notification)
    {
        string sql = @"INSERT INTO Notifications (ProductID, NotificationType, Message, IsRead, Timestamp) 
                   VALUES (@ProductID, @NotificationType, @Message, @IsRead, @Timestamp)";

        // This 'using' block creates, opens, and automatically closes the connection.
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ProductID", notification.ProductID);
                command.Parameters.AddWithValue("@NotificationType", notification.NotificationType);
                command.Parameters.AddWithValue("@Message", notification.Message);
                command.Parameters.AddWithValue("@IsRead", notification.IsRead ? 1 : 0); // Correctly converts bool to int
                command.Parameters.AddWithValue("@Timestamp", notification.Timestamp); // SQLite driver handles DateTime conversion
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Helper method to check for existing unread alerts for a product. Requires an open connection.
    /// </summary>
    // In DatabaseRepository.cs

    /// <summary>
    /// Checks if an unread alert already exists for a specific product.
    /// This method is self-contained and manages its own database connection.
    /// </summary>
    /// <param name="productId">The ID of the product to check.</param>
    /// <returns>True if an unread alert exists, otherwise false.</returns>
    private bool HasExistingUnreadAlert(int productId)
    {
        // This query is slightly more efficient as it stops searching after finding the first match.
        string sql = "SELECT 1 FROM Notifications WHERE ProductID = @ProductID AND IsRead = 0 LIMIT 1";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ProductID", productId);

                // ExecuteScalar returns the first result or null if no results are found.
                // This is the most efficient way to check for the existence of a record.
                var result = command.ExecuteScalar();

                // If the result is not null, it means a record was found.
                return (result != null);
            }
        }
    }

    #endregion

    #region Supplier Management Methods

    /// <summary>
    /// Retrieves a list of all suppliers from the database.
    /// </summary>
    public List<Supplier> GetAllSuppliers()
    {
        var suppliers = new List<Supplier>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT SupplierID, Name, ContactInfo FROM Suppliers";
            using (var command = new SQLiteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suppliers.Add(new Supplier
                        {
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            Name = reader["Name"].ToString(),
                            ContactInfo = reader["ContactInfo"].ToString()
                        });
                    }
                }
            }
        }
        return suppliers;
    }

    /// <summary>
    /// Adds a new supplier to the database.
    /// </summary>
    public void AddSupplier(Supplier supplier)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Suppliers (Name, ContactInfo) VALUES (@Name, @ContactInfo)";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", supplier.Name);
                command.Parameters.AddWithValue("@ContactInfo", supplier.ContactInfo);
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Updates an existing supplier in the database.
    /// </summary>
    public void UpdateSupplier(Supplier supplier)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "UPDATE Suppliers SET Name = @Name, ContactInfo = @ContactInfo WHERE SupplierID = @SupplierID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", supplier.Name);
                command.Parameters.AddWithValue("@ContactInfo", supplier.ContactInfo);
                command.Parameters.AddWithValue("@SupplierID", supplier.SupplierID);
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Deletes a supplier from the database.
    /// </summary>
    public void DeleteSupplier(int supplierId)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM Suppliers WHERE SupplierID = @SupplierID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SupplierID", supplierId);
                command.ExecuteNonQuery();
            }
        }
    }

    #endregion
    
    public void CreateNotification(int productId, string notificationType, string message)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = @"INSERT INTO Notifications (ProductID, NotificationType, Message, IsRead, Timestamp) 
                       VALUES (@ProductID, @NotificationType, @Message, 0, @Timestamp)";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@NotificationType", notificationType);
                command.Parameters.AddWithValue("@Message", message);
                command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("o")); // ISO 8601 format
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Checks if a supplier name already exists in the database.
    /// </summary>
    /// <param name="name">The name to check for.</param>
    /// <param name="currentSupplierId">The ID of the supplier being edited. Use 0 for new suppliers.</param>
    /// <returns>True if the name exists for a different supplier, otherwise false.</returns>
    public bool SupplierNameExists(string name, int currentSupplierId)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            // This query checks for any supplier with the same name BUT a different ID.
            // This allows an existing supplier to be saved without changing their name.
            string sql = "SELECT COUNT(*) FROM Suppliers WHERE Name = @Name AND SupplierID != @SupplierID";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@SupplierID", currentSupplierId);
                long count = (long)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
    // --- PASTE THIS NEW METHOD ---
    public Product GetProductById(int productId)
    {
        Product product = null;
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM Products WHERE ProductID = @id";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", productId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // This is your defensive logic, applied to a single product object.
                        product = new Product
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            StockQuantity = reader["StockQuantity"] is DBNull ? 0 : Convert.ToInt32(reader["StockQuantity"]),
                            PurchaseCost = reader["PurchaseCost"] is DBNull ? 0m : Convert.ToDecimal(reader["PurchaseCost"]),
                            SellingPrice = reader["SellingPrice"] is DBNull ? 0m : Convert.ToDecimal(reader["SellingPrice"]),
                            LowStockThreshold = Convert.ToInt32(reader["LowStockThreshold"]),

                            Barcode = reader["Barcode"] is DBNull ? "" : reader["Barcode"].ToString(),
                            PartNumber = reader["PartNumber"] is DBNull ? "" : reader["PartNumber"].ToString(),
                            Brand = reader["Brand"] is DBNull ? "" : reader["Brand"].ToString(),
                            Description = reader["Description"] is DBNull ? "" : reader["Description"].ToString(),
                            Volume = reader["Volume"] is DBNull ? "" : reader["Volume"].ToString(),
                            Type = reader["Type"] is DBNull ? "" : reader["Type"].ToString(),
                            Application = reader["Application"] is DBNull ? "" : reader["Application"].ToString(),
                            Notes = reader["Notes"] is DBNull ? "" : reader["Notes"].ToString()
                        };
                    }
                }
            }
        }
        return product;
    }
    // In DatabaseRepository.cs

    /// <summary>
    /// Gets a list of all transactions that occurred on the current date.
    /// </summary>
    /// <returns>A list of transactions formatted for the dashboard view.</returns>
    public List<DashboardTransactionView> GetTodaysTransactions()
    {
        var transactionViews = new List<DashboardTransactionView>();

        // This query is similar to GetDashboardTransactions but is filtered to today's date.
        // NOTE: DATE('now', 'localtime') is the SQLite way to get the current date.
        string sql = @"
    SELECT 
        t.TransactionID, t.ProductID, p.Barcode, p.Description AS ProductDescription,
        t.TransactionType, t.StockBefore AS PreviousStock, t.StockAfter AS CurrentStock,
        t.TransactionDate, t.SupplierID, sup.Name AS SupplierName, s.DeliverTo AS CustomerName,
        si.UnitPrice AS Price, t.QuantityChange, p.PurchaseCost
    FROM Transactions t
    INNER JOIN Products p ON t.ProductID = p.ProductID
    LEFT JOIN Suppliers sup ON t.SupplierID = sup.SupplierID
    LEFT JOIN SaleItems si ON t.SaleItemID = si.SaleItemID
    LEFT JOIN Sales s ON si.SaleID = s.SaleID
    WHERE DATE(t.TransactionDate) = DATE('now', 'localtime')
    ORDER BY t.TransactionDate DESC;
    ";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // This mapping reuses the same DashboardTransactionView object
                        var view = new DashboardTransactionView
                        {
                            TransactionID = Convert.ToInt32(reader["TransactionID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            Barcode = reader["Barcode"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            TransactionType = reader["TransactionType"].ToString(),
                            Price = reader["Price"] is DBNull ? 0 : Convert.ToDecimal(reader["Price"]),
                            StockBefore = Convert.ToInt32(reader["PreviousStock"]),
                            StockAfter = Convert.ToInt32(reader["CurrentStock"]),
                            TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                            SupplierID = reader["SupplierID"] is DBNull ? (int?)null : Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"] is DBNull ? "" : reader["SupplierName"].ToString(),
                            CustomerName = reader["CustomerName"] is DBNull ? "" : reader["CustomerName"].ToString(),
                            QuantityChange = Convert.ToInt32(reader["QuantityChange"]),
                            PurchaseCost = Convert.ToDecimal(reader["PurchaseCost"])
                        };
                        transactionViews.Add(view);
                    }
                }
            }
        }
        return transactionViews;
    }
    // --- AND PASTE THIS NEW METHOD ---
    // ----- RENAME THE METHOD -----
    public Transaction GetLatestTransactionForProduct(int productId)
    {
        Transaction transaction = null;
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // ----- USE THE SIMPLER, MORE RELIABLE QUERY -----
            // It's also slightly better to order by the TransactionID, which is guaranteed to be sequential.
            string sql = "SELECT * FROM Transactions WHERE ProductID = @productId ORDER BY TransactionID DESC LIMIT 1";

            using (var command = new SQLiteCommand(sql, connection))
            {
                // ----- REMOVE THE TIMESTAMP PARAMETER -----
                command.Parameters.AddWithValue("@productId", productId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        transaction = new Transaction
                        {
                            // Your existing mapping logic is perfect
                            TransactionID = Convert.ToInt32(reader["TransactionID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            TransactionType = reader["TransactionType"].ToString(),
                            QuantityChange = Convert.ToInt32(reader["QuantityChange"]),
                            StockBefore = Convert.ToInt32(reader["StockBefore"]),
                            StockAfter = Convert.ToInt32(reader["StockAfter"]),
                            TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                            SupplierID = reader["SupplierID"] is DBNull ? (int?)null : Convert.ToInt32(reader["SupplierID"])
                        };
                    }
                }
            }
        }
        return transaction;
    }
    // In DatabaseRepository.cs

    /// <summary>
    /// Searches for products based on a term, matching against Description, Brand, or Part Number.
    /// </summary>
    /// <param name="searchTerm">The text to search for.</param>
    /// <returns>A list of matching products.</returns>
    public List<Product> SearchProducts(string searchTerm)
    {
        var products = new List<Product>();
        // The LIKE query with '%' wildcards finds the search term anywhere in the text.
        string sql = @"SELECT ProductID, Barcode, PartNumber, Brand, Description, Volume, Type, Application, PurchaseCost, SellingPrice, StockQuantity, Notes, LowStockThreshold, DateCreated, DateModified 
                   FROM Products 
                   WHERE Description LIKE @term 
                   OR Brand LIKE @term 
                   OR PartNumber LIKE @term
                   LIMIT 25;"; // We limit results to keep the UI responsive.

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                // We wrap the search term in '%' to find it anywhere in the string.
                command.Parameters.AddWithValue("@term", $"%{searchTerm}%");
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // This reuses the same mapping logic from your GetAllProducts method.
                        products.Add(new Product
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            StockQuantity = reader["StockQuantity"] is DBNull ? 0 : Convert.ToInt32(reader["StockQuantity"]),
                            PurchaseCost = reader["PurchaseCost"] is DBNull ? 0m : Convert.ToDecimal(reader["PurchaseCost"]),
                            SellingPrice = reader["SellingPrice"] is DBNull ? 0m : Convert.ToDecimal(reader["SellingPrice"]),
                            LowStockThreshold = reader["LowStockThreshold"] is DBNull ? 0 : Convert.ToInt32(reader["LowStockThreshold"]),
                            Barcode = reader["Barcode"] is DBNull ? "" : reader["Barcode"].ToString(),
                            PartNumber = reader["PartNumber"] is DBNull ? "" : reader["PartNumber"].ToString(),
                            Brand = reader["Brand"] is DBNull ? "" : reader["Brand"].ToString(),
                            Description = reader["Description"] is DBNull ? "" : reader["Description"].ToString(),
                            Volume = reader["Volume"] is DBNull ? "" : reader["Volume"].ToString(),
                            Type = reader["Type"] is DBNull ? "" : reader["Type"].ToString(),
                            Application = reader["Application"] is DBNull ? "" : reader["Application"].ToString(),
                            Notes = reader["Notes"] is DBNull ? "" : reader["Notes"].ToString(),
                            DateCreated = reader["DateCreated"] is DBNull ? "" : reader["DateCreated"].ToString(),
                            DateModified = reader["DateModified"] is DBNull ? "" : reader["DateModified"].ToString()
                        });
                    }
                }
            }
        }
        return products;
    }
}