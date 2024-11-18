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
            string name = NameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Имя продукта не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _product.name = name;

            // Проверка категории
            if (SelectedCategoryId <= 0) // Предполагается, что ID категории должен быть положительным
            {
                MessageBox.Show("Выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _product.category_id = SelectedCategoryId;

            // Проверка количества на складе
            if (!int.TryParse(QuantityTextBox.Text, out int quantityInStock) || quantityInStock < 0)
            {
                MessageBox.Show("Введите корректное количество на складе (неотрицательное целое число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _product.quantity_in_stock = quantityInStock;

            // Проверка цены за единицу
            if (!decimal.TryParse(PriceTextBox.Text, out decimal pricePerUnit) || pricePerUnit <= 0)
            {
                MessageBox.Show("Введите корректную цену за единицу (положительное число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _product.price_per_unit = pricePerUnit;

            // Проверка описания
            string description = DescriptionTextBox.Text.Trim();
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Описание продукта не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _product.description = description;

            // Проверка дат
            DateTime? productionDate = ProductionDatePicker.SelectedDate;
            DateTime? expiryDate = ExpiryDatePicker.SelectedDate;

            if (!productionDate.HasValue)
            {
                MessageBox.Show("Укажите дату производства.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!expiryDate.HasValue)
            {
                MessageBox.Show("Укажите дату истечения срока годности.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (expiryDate.Value <= productionDate.Value)
            {
                MessageBox.Show("Дата истечения срока годности должна быть позже даты производства.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _product.production_date = productionDate;
            _product.expiry_date = expiryDate;

            // Проверка единицы измерения
            string unit = WeightTextBox.Text.Trim();
            if (string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("Единица измерения не может быть пустой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _product.unit_of_measurement = unit;

            database.UpdateProductInDatabase(_product);
            this.NavigationService.Navigate(new DatabasePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
