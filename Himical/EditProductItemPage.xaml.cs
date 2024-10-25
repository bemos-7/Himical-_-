using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для EditProductItemPage.xaml
    /// </summary>
    public partial class EditProductItemPage : Page
    {
        private Product _product;
        public EditProductItemPage(Product product)
        {
            InitializeComponent();
            _product = product;
            NameTextBox.Text = _product.name;
            CategoryTextBox.Text = Convert.ToString(_product.category_id);
            QuantityTextBox.Text = Convert.ToString(_product.quantity_in_stock);
            PriceTextBox.Text = Convert.ToString(_product.price_per_unit);
            DescriptionTextBox.Text = _product.description;
            ProductionDatePicker.SelectedDate = _product.production_date;
            ExpiryDatePicker.SelectedDate = _product.expiry_date;
            WeightTextBox.Text = Convert.ToString(_product.unit_of_measurement);
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            _product.name = NameTextBox.Text;
            _product.category_id = Convert.ToInt32(CategoryTextBox.Text);
            _product.quantity_in_stock = Convert.ToInt32(QuantityTextBox.Text);
            _product.price_per_unit = Convert.ToDecimal(PriceTextBox.Text);
            _product.description = DescriptionTextBox.Text;
            _product.production_date = ProductionDatePicker.SelectedDate;
            _product.expiry_date = ExpiryDatePicker.SelectedDate;
            _product.unit_of_measurement = WeightTextBox.Text;

            UpdateProductInDatabase(_product);
            this.NavigationService.Navigate(new DatabasePage());
        }

        private void UpdateProductInDatabase(Product product)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Himical.Properties.Settings.ConnectionString"].ConnectionString;

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
    }
}
