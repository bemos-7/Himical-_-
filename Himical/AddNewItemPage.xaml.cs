using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
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
using System.Configuration;
using System.Data.SqlClient;

namespace Himical
{
    /// <summary>
    /// Логика взаимодействия для AddNewItemPage.xaml
    /// </summary>
    public partial class AddNewItemPage : Page
    {
        public AddNewItemPage()
        {
            InitializeComponent();
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            string prodName = NameTextBox.Text;
            int category = int.Parse(CategoryTextBox.Text);
            int quantity = int.Parse(QuantityTextBox.Text);
            decimal price = decimal.Parse(PriceTextBox.Text);
            string description = DescriptionTextBox.Text;
            DateTime productionDate = ProductionDatePicker.SelectedDate.HasValue ? ProductionDatePicker.SelectedDate.Value : DateTime.Now;
            DateTime expiryDate = ExpiryDatePicker.SelectedDate.HasValue ? ExpiryDatePicker.SelectedDate.Value : DateTime.Now;
            string unit = WeightTextBox.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["Himical.Properties.Settings.ConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Products 
                        (name, category_id, quantity_in_stock, price_per_unit, description, production_date, expiry_date, unit_of_measurement) 
                        VALUES (@ProductName, @CategoryId, @Quantity, @Price, @Description, @ProductionDate, @ExpiryDate, @Unit)";

                    using (SqlCommand command = new SqlCommand(query, connection)) {
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

            this.NavigationService.Navigate(new DatabasePage());
        }
    }
}
