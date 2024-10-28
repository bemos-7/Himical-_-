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
        DatabaseLoad database = new DatabaseLoad();

        public ObservableCollection<Product> ProductsCollection { get; set; } = new ObservableCollection<Product>();
        public DatabasePage()
        {
            InitializeComponent();
            ProductsCollection = database.LoadProductsFromDatabase();
            ProductsGrid.ItemsSource = ProductsCollection;
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
                    database.DeleteProductFromDatabase(product.product_id);
                    ProductsCollection = database.LoadProductsFromDatabase();
                    ProductsGrid.ItemsSource = ProductsCollection;
                }
            }
        }
    }
}
