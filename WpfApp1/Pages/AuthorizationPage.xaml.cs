using CoffeShopApplication.DataLayer;
using System;
using System.Collections.Generic;
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
using WpfApp1;

namespace CoffeShopApplication
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        MainWindow _mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        WorkPage workPage = new WorkPage();

        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            using CoffeShopContext context = new CoffeShopContext();

            string enteredAddress = addressTextBox.Text;
            string enteredPassword = passwordPasswordBox.Password;

            var shop = context.Shops.FirstOrDefault(s => s.Address == enteredAddress);

            if (shop != null && shop.Password == enteredPassword)
            {
                Properties.Settings.Default.ShopAddress = enteredAddress;
                Properties.Settings.Default.ShopPassword = enteredPassword;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Upgrade();
                _mainWindow.mainFrame.Navigate(workPage);
            }
            else
            {
                MessageBox.Show("Неверный адрес или пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
