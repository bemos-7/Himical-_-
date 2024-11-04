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
    public partial class AddNewAdminPage : Page
    {
        DatabaseLoad database = new DatabaseLoad();

        public AddNewAdminPage()
        {
            InitializeComponent();
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordTextBox.Text;

            database.AddNewAdminItemInDatabase(username, password);

            this.NavigationService.Navigate(new DatabasePage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
