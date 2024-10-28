using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        DatabaseLoad database = new DatabaseLoad();

        private Product _product;

        public ObservableCollection<Category> CategoryCollection { get; set; } = new ObservableCollection<Category>();
        public int SelectedCategoryId { get; set; }
        public EditProductItemPage(Product product)
        {
            InitializeComponent();
            DataContext = this;
            _product = product;
            NameTextBox.Text = _product.name;
            QuantityTextBox.Text = Convert.ToString(_product.quantity_in_stock);
            PriceTextBox.Text = Convert.ToString(_product.price_per_unit);
            DescriptionTextBox.Text = _product.description;
            ProductionDatePicker.SelectedDate = _product.production_date;
            ExpiryDatePicker.SelectedDate = _product.expiry_date;
            WeightTextBox.Text = Convert.ToString(_product.unit_of_measurement);

            CategoryCollection = database.LoadCategoryFromDatabase();

            SelectedCategoryId = _product.category_id;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            _product.name = NameTextBox.Text;
            _product.category_id = SelectedCategoryId;
            _product.quantity_in_stock = Convert.ToInt32(QuantityTextBox.Text);
            _product.price_per_unit = Convert.ToDecimal(PriceTextBox.Text);
            _product.description = DescriptionTextBox.Text;
            _product.production_date = ProductionDatePicker.SelectedDate;
            _product.expiry_date = ExpiryDatePicker.SelectedDate;
            _product.unit_of_measurement = WeightTextBox.Text;

            database.UpdateProductInDatabase(_product);
            this.NavigationService.Navigate(new DatabasePage());
        }
    }
}
