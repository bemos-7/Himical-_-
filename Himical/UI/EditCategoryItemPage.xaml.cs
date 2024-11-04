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
    public partial class EditCategoryItemPage : Page
    {
        DatabaseLoad database = new DatabaseLoad();

        private Category _category;
        public int SelectedCategoryId { get; set; }
        public EditCategoryItemPage(Category category)
        {
            InitializeComponent();
            DataContext = this;
            _category = category;
            NameTextBox.Text = _category.name;
            DescriptionTextBox.Text = _category.description;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            _category.name = NameTextBox.Text;
            _category.description = DescriptionTextBox.Text;

            database.UpdateCategoryInDatabase(_category);
            this.NavigationService.Navigate(new DatabasePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
