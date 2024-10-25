using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Himical
{
    /// <summary>
    /// Логика взаимодействия для DatabasePage.xaml
    /// </summary>
    public partial class DatabasePage : Page
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Himical.Properties.Settings.ConnectionString"].ConnectionString;

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public DatabasePage()
        {
            InitializeComponent();
            LoadProductsFromDatabase();
            ProductsGrid.ItemsSource = Products;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            Product productToEdit = editButton?.Tag as Product;

            if (productToEdit != null)
            {
                EditProductItemPage editProdPage = new EditProductItemPage(productToEdit);
                this.NavigationService.Navigate(editProdPage);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddNewItemPage());
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            var product = deleteButton?.CommandParameter as Product;

            if (product != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DeleteProductFromDatabase(product.product_id);
                    LoadProductsFromDatabase();
                }
            }
        }

        private void DeleteProductFromDatabase(int productId)
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

        private void LoadProductsFromDatabase()
        {
            Products.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT product_id, name, category_id, quantity_in_stock, price_per_unit, description, production_date, expiry_date, unit_of_measurement FROM Products";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Products.Add(new Product
                        {
                            product_id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            category_id = reader.GetInt32(2),
                            quantity_in_stock = reader.GetInt32(3),
                            price_per_unit = reader.GetDecimal(4),
                            description = reader.IsDBNull(5) ? null : reader.GetString(5),
                            production_date = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                            expiry_date = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                            unit_of_measurement = reader.IsDBNull(8) ? null : reader.GetString(8)
                        });
                    }
                }
            }
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
    }
}
