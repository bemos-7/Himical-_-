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
    public partial class EditOrderItemPage : Page
    {
        DatabaseLoad database = new DatabaseLoad();

        private Order _order;

        public ObservableCollection<Product> ProductCollection { get; set; } = new ObservableCollection<Product>();
        public int SelectedProductId { get; set; }
        public EditOrderItemPage(Order order)
        {
            InitializeComponent();
            DataContext = this;
            _order = order;
            QuantityTextBox.Text = Convert.ToString(_order.quantity);
            PriceTextBox.Text = Convert.ToString(_order.price);
            OrderDateDatePicker.Text = Convert.ToString(_order.order_date);

            ProductCollection = database.LoadProductsFromDatabase();

            SelectedProductId = _order.product_id;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            _order.product_id = SelectedProductId;
            _order.quantity = Convert.ToInt32(QuantityTextBox.Text);
            _order.price = Convert.ToDecimal(QuantityTextBox.Text);
            _order.order_date = OrderDateDatePicker.SelectedDate;

            database.UpdateOrderInDatabase(_order);
            this.NavigationService.Navigate(new DatabasePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
