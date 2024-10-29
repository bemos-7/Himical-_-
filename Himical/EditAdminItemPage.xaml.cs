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
    public partial class EditAdminItemPage : Page
    {
        DatabaseLoad database = new DatabaseLoad();

        private Admin _admin;
        public int SelectedAdminId { get; set; }
        public EditAdminItemPage(Admin admin)
        {
            InitializeComponent();
            DataContext = this;
            _admin = admin;
            UserNameTextBox.Text = _admin.username;
            PasswordTextBox.Text = _admin.password_hash;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            _admin.username = UserNameTextBox.Text;
            _admin.password_hash = PasswordTextBox.Text;

            database.UpdateAdminInDatabase(_admin);
            this.NavigationService.Navigate(new DatabasePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
