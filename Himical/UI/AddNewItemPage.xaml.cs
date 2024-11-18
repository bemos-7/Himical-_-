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
            string prodName = NameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(prodName))
            {
                MessageBox.Show("Название продукта не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedCategoryId == 0)
            {
                MessageBox.Show("Выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int category = SelectedCategoryId;

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество (положительное целое число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (положительное число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string description = DescriptionTextBox.Text.Trim();
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Описание не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime productionDate = ProductionDatePicker.SelectedDate ?? DateTime.Now;

            DateTime expiryDate = ExpiryDatePicker.SelectedDate ?? DateTime.Now;
            if (expiryDate <= productionDate)
            {
                MessageBox.Show("Дата истечения срока годности должна быть позже даты производства.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string unit = WeightTextBox.Text.Trim();
            if (string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("Единица измерения не может быть пустой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            database.AddNewProductItemInDatabase(prodName, category, quantity, price, description, productionDate, expiryDate, unit);
            this.NavigationService.Navigate(new DatabasePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
