using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Himical
{
    internal class DatabaseLoad
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Himical.Properties.Settings.ConnectionString"].ConnectionString;
        public ObservableCollection<Category> LoadCategoryFromDatabase() // загрузка Таблицы Categories из БД
        {
            ObservableCollection<Category> CategoryCollection = new ObservableCollection<Category>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT category_id, name, description FROM Categories";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryCollection.Add(new Category
                        {
                            category_id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            description = reader.IsDBNull(2) ? null : reader.GetString(2)
                        });
                    }
                }
            }
            return CategoryCollection;
        }

        public void AddNewCategoryItemInDatabase(string name, string description)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Categories 
                        (name, description) 
                        VALUES (@Name, @Desc)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Desc", description);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Продукт успешно добавлен!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error - {ex.Message}");
                }
            }
        }

        public void UpdateCategoryInDatabase(Category category) // Метод для изменения поля из таблицы Categories
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Categories SET name = @name, description = @desc WHERE category_id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", category.name);
                    command.Parameters.AddWithValue("@desc", category.description);
                    command.Parameters.AddWithValue("@id", category.category_id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCategoryFromDatabase(int categoryId) // Метод для удаления поля из таблицы Categories
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Categories WHERE category_id = @CategoryId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public ObservableCollection<Product> LoadProductsFromDatabase() // Загрузка Таблицы Produts из БД
        {
            ObservableCollection<Product> ProductCollection = new ObservableCollection<Product>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT p.product_id, p.name, p.category_id, p.quantity_in_stock, 
                                p.price_per_unit, p.description, p.production_date, 
                                p.expiry_date, p.unit_of_measurement, c.name AS category_name
                         FROM Products p
                         JOIN Categories c ON p.category_id = c.category_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductCollection.Add(new Product
                        {
                            product_id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            category_id = reader.GetInt32(2),
                            quantity_in_stock = reader.GetInt32(3),
                            price_per_unit = reader.GetDecimal(4),
                            description = reader.IsDBNull(5) ? null : reader.GetString(5),
                            production_date = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                            expiry_date = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                            unit_of_measurement = reader.IsDBNull(8) ? null : reader.GetString(8),
                            category_name = reader.GetString(9),
                        });
                    }
                }
            }
            return ProductCollection;
        }

        public ObservableCollection<Admin> LoadAdminsFormDatabase() // Метод для получения таблицы Admins из БД
        {
            ObservableCollection<Admin> AdminCollection = new ObservableCollection<Admin>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT admin_id, username, password_hash FROM Admins";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AdminCollection.Add(new Admin
                        {
                            admin_id = reader.GetInt32(0),
                            username = reader.GetString(1),
                            password_hash = reader.GetString(2),
                        });
                    }
                }
            }
            return AdminCollection;
        }

        public void AddNewAdminItemInDatabase(string username, string password_hash)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Admins 
                        (username, password_hash) 
                        VALUES (@UserName, @PasswordHash)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", username);
                        command.Parameters.AddWithValue("@PasswordHash", password_hash);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Продукт успешно добавлен!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error - {ex.Message}");
                }
            }
        }

        public void UpdateAdminInDatabase(Admin admin) // Метод для изменения поля из таблицы Products
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Admins SET username = @username, password_hash = @pass WHERE admin_id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", admin.username);
                    command.Parameters.AddWithValue("@pass", admin.password_hash);
                    command.Parameters.AddWithValue("@id", admin.admin_id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAdminFromDatabase(int adminId) // Метод для удаления поля из таблицы Admin
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Admins WHERE admin_id = @AdminId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AdminId", adminId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProductFromDatabase(int productId) // Метод для удаления поля из таблицы Products
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Products WHERE product_id = @ProductId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateProductInDatabase(Product product) // Метод для изменения поля из таблицы Products
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Products SET name = @name, category_id = @category, quantity_in_stock = @quantity, price_per_unit = @price, description = @desc, production_date = @prod_date, expiry_date = @exp, unit_of_measurement = @unit WHERE product_id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", product.name);
                    command.Parameters.AddWithValue("@category", product.category_id);
                    command.Parameters.AddWithValue("@quantity", product.quantity_in_stock);
                    command.Parameters.AddWithValue("@price", product.price_per_unit);
                    command.Parameters.AddWithValue("@desc", product.description);
                    command.Parameters.AddWithValue("@prod_date", product.production_date);
                    command.Parameters.AddWithValue("@exp", product.expiry_date);
                    command.Parameters.AddWithValue("@unit", product.unit_of_measurement);
                    command.Parameters.AddWithValue("@id", product.product_id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddNewProductItemInDatabase(string prodName, int category, int quantity, decimal price, string description, DateTime productionDate, DateTime expiryDate, string unit)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Products 
                        (name, category_id, quantity_in_stock, price_per_unit, description, production_date, expiry_date, unit_of_measurement) 
                        VALUES (@ProductName, @CategoryId, @Quantity, @Price, @Description, @ProductionDate, @ExpiryDate, @Unit)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", prodName);
                        command.Parameters.AddWithValue("@CategoryId", category);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@ProductionDate", productionDate);
                        command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                        command.Parameters.AddWithValue("@Unit", unit);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Продукт успешно добавлен!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error - {ex.Message}");
                }
            }
        }

        public bool CheckAdmin(string username, string passwordHash)
        {
            bool isValid = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Admins WHERE username = @username AND password_hash = @password_hash";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password_hash", passwordHash);

                    int count = (int)command.ExecuteScalar();
                    isValid = count > 0;
                }
            }
            return isValid;
        }

        public ObservableCollection<Product> SearchProductsByName(string searchQuery)
        {
            ObservableCollection<Product> ProductCollection = new ObservableCollection<Product>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT p.product_id, p.name, p.category_id, p.quantity_in_stock, 
                               p.price_per_unit, p.description, p.production_date, 
                               p.expiry_date, p.unit_of_measurement, c.name AS category_name
                        FROM Products p
                        JOIN Categories c ON p.category_id = c.category_id
                        WHERE p.name LIKE @SearchQuery";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductCollection.Add(new Product
                            {
                                product_id = reader.GetInt32(0),
                                name = reader.GetString(1),
                                category_id = reader.GetInt32(2),
                                quantity_in_stock = reader.GetInt32(3),
                                price_per_unit = reader.GetDecimal(4),
                                description = reader.IsDBNull(5) ? null : reader.GetString(5),
                                production_date = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                expiry_date = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                unit_of_measurement = reader.IsDBNull(8) ? null : reader.GetString(8),
                                category_name = reader.GetString(9),
                            });
                        }
                    }
                }
            }
            return ProductCollection;
        }

        public ObservableCollection<Category> SearchCategoryByName(string searchQuery)
        {
            ObservableCollection<Category> CategoryCollection = new ObservableCollection<Category>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT category_id, name, description
                                FROM Categories
                                WHERE name LIKE @SearchQuery";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryCollection.Add(new Category
                            {
                                category_id = reader.GetInt32(0),
                                name = reader.GetString(1),
                                description = reader.GetString(2),
                            });
                        }
                    }
                }
            }
            return CategoryCollection;
        }

        public ObservableCollection<Admin> SearchAdminByName(string searchQuery)
        {
            ObservableCollection<Admin> AdminCollection = new ObservableCollection<Admin>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT admin_id, username, password_hash
                        FROM Admins
                        WHERE username LIKE @SearchQuery";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AdminCollection.Add(new Admin
                            {
                                admin_id = reader.GetInt32(0),
                                username = reader.GetString(1),
                                password_hash = reader.GetString(2),
                            });
                        }
                    }
                }
            }
            return AdminCollection;
        }
    }

    public class Product
    {
        public int product_id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
        public int quantity_in_stock { get; set; }
        public decimal price_per_unit { get; set; }
        public string description { get; set; }
        public DateTime? production_date { get; set; }
        public DateTime? expiry_date { get; set; }
        public string unit_of_measurement { get; set; }
        public string category_name { get; set; }
    }

    public class Category
    {
        public int category_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Admin
    {
        public int admin_id { get; set; }
        public string username { get; set; }
        public string password_hash { get; set; }
    }
}
