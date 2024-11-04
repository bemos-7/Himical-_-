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
using System.Collections.ObjectModel;

namespace Himical
{
    /// <summary>
    /// Логика взаимодействия для AddNewItemPage.xaml
    /// </summary>
    public partial class AddNewOrderPage : Page
    {
        DatabaseLoad database = new DatabaseLoad();

        public ObservableCollection<Product> ProductCollection { get; set; } = new ObservableCollection<Product>();

        public int SelectedProductId { get; set; }

        public AddNewOrderPage()
        {
            InitializeComponent();
            ProductCollection = database.LoadProductsFromDatabase();
            ProductComboBox.ItemsSource = ProductCollection;
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            int product_id = SelectedProductId;
            int quantity = int.Parse(QuantityTextBox.Text);
            decimal price = decimal.Parse(PriceTextBox.Text);
            DateTime order_date = OrderDateDatePicker.SelectedDate.HasValue ? OrderDateDatePicker.SelectedDate.Value : DateTime.Now;

            database.AddNewOrderInDatabase(product_id, quantity, price, order_date);
            this.NavigationService.Navigate(new DatabasePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
