using CoffeShopApplication;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AuthorizationPage authorizationPage = new AuthorizationPage();
            mainFrame.Navigate(authorizationPage);
        }
    }
}