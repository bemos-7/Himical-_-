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
    public partial class AddNewItemPage : Page
    {
        DatabaseLoad database = new DatabaseLoad();

        public ObservableCollection<Category> CategoryCollection { get; set; } = new ObservableCollection<Category>();

        public int SelectedCategoryId { get; set; }

        public AddNewItemPage()
        {
            InitializeComponent();
            DatabaseLoad load = new DatabaseLoad();
            CategoryCollection = load.LoadCategoryFromDatabase();
            CategoryComboBox.ItemsSource = CategoryCollection;
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            string prodName = NameTextBox.Text;
            int category = SelectedCategoryId;
            int quantity = int.Parse(QuantityTextBox.Text);
            decimal price = decimal.Parse(PriceTextBox.Text);
            string description = DescriptionTextBox.Text;
            DateTime productionDate = ProductionDatePicker.SelectedDate.HasValue ? ProductionDatePicker.SelectedDate.Value : DateTime.Now;
            DateTime expiryDate = ExpiryDatePicker.SelectedDate.HasValue ? ExpiryDatePicker.SelectedDate.Value : DateTime.Now;
            string unit = WeightTextBox.Text;

            database.AddNewProductItemInDatabase(prodName, category, quantity, price, description, productionDate, expiryDate, unit);
            this.NavigationService.Navigate(new DatabasePage());
        }
    }
}
