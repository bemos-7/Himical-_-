using System;
using System.Collections.Generic;
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
        public DatabasePage()
        {
            InitializeComponent();
            ProductsGrid.ItemsSource = ProductsDbEntities.GetContext().Products.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            Product productToEdit = editButton.Tag as Product;

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
        public string production_date { get; set; }
        public string expiry_date { get; set; }
        public int unit_of_measurement { get; set; }
    }
}
