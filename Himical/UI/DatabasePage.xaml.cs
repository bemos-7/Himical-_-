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
        public ObservableCollection<Category> CategoryCollection { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Admin> AdminCollection { get; set; } = new ObservableCollection<Admin>();
        public ObservableCollection<Order> OrderCollection { get; set; } = new ObservableCollection<Order>();
        public DatabasePage()
        {
            InitializeComponent();
            ProductsCollection = database.LoadProductsFromDatabase();
            CategoryCollection = database.LoadCategoryFromDatabase();
            AdminCollection = database.LoadAdminsFormDatabase();
            OrderCollection = database.LoadOrdersFromDatabase();
            ProductsGrid.ItemsSource = ProductsCollection;
            CategoriesGrid.ItemsSource = CategoryCollection;
            AdminsGrid.ItemsSource = AdminCollection;
            OrdersGrid.ItemsSource = OrderCollection;
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

        private void BtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddNewCategoryPage());
        }

        private void BtnCategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            Category category = editButton?.CommandParameter as Category;

            if (category != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    database.DeleteCategoryFromDatabase(category.category_id);
                    CategoryCollection = database.LoadCategoryFromDatabase();
                    CategoriesGrid.ItemsSource = CategoryCollection;
                }
            }
        }

        private void BtnCategoryEdit_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            Category categoryToEdit = editButton?.Tag as Category;

            if (categoryToEdit != null)
            {
                EditCategoryItemPage editProdPage = new EditCategoryItemPage(categoryToEdit);
                this.NavigationService.Navigate(editProdPage);
            }
        }

        private void BtnAddAdmin_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddNewAdminPage());
        }

        private void BtnAdminDelete_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            Admin admin = editButton?.CommandParameter as Admin;

            if (admin != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    database.DeleteAdminFromDatabase(admin.admin_id);
                    AdminCollection = database.LoadAdminsFormDatabase();
                    AdminsGrid.ItemsSource = AdminCollection;
                }
            }
        }

        private void BtnAdminEdit_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            Admin adminToEdit = editButton?.Tag as Admin;

            if (adminToEdit != null)
            {
                EditAdminItemPage editProdPage = new EditAdminItemPage(adminToEdit);
                this.NavigationService.Navigate(editProdPage);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            ProductsCollection.Clear();
            string searchText = Search_TextBox.Text;
            ProductsCollection = database.SearchProductsByName(searchText);
            ProductsGrid.ItemsSource = ProductsCollection;
        }

        private void BtnSearchCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryCollection.Clear();
            string searchText = Search_Category_TextBox.Text;
            CategoryCollection = database.SearchCategoryByName(searchText);
            CategoriesGrid.ItemsSource = CategoryCollection;
        }

        private void BtnSearchAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminCollection.Clear();
            string searchText = Search_Admin_TextBox.Text;
            AdminCollection = database.SearchAdminByName(searchText);
            AdminsGrid.ItemsSource = AdminCollection;
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            database.SaveProductsToFile(ProductsCollection);
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddNewOrderPage());
        }
        private void BtnOrderEdit_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            Order orderToEdit = editButton?.Tag as Order;

            if (orderToEdit != null)
            {
                EditOrderItemPage editOrderPage = new EditOrderItemPage(orderToEdit);
                this.NavigationService.Navigate(editOrderPage);
            }
        }

        private void BtnOrderDelete_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            var order = deleteButton?.CommandParameter as Order;

            if (order != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    database.DeleteOrderFromDatabase(order.order_id);
                    OrderCollection = database.LoadOrdersFromDatabase();
                    OrdersGrid.ItemsSource = OrderCollection;
                }
            }
        }

        private void CreateOrderReport_Click(object sender, RoutedEventArgs e)
        {
            database.SaveOrdersToFile(OrderCollection);
        }

        private void BtnSearchOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderCollection.Clear();
            string searchQuery = Search_Order_TextBox.Text;
            OrderCollection = database.SearchOrderById(searchQuery);
            OrdersGrid.ItemsSource = OrderCollection;
        }
    }
}
