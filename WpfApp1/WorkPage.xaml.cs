using CoffeShopApplication.DataLayer;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary>
    public partial class WorkPage : Page
    {
        public WorkPage()
        {
            InitializeComponent();
            ShowOrdersFullInfo(newOrdersDataGrid, "Принят");
            ShowOrdersFullInfo(processOrdersDataGrid, "Готовится", "Ожидает выдачи");
            ShowOrdersFullInfo(complitedOrdersDataGrid, "Получен", "Отменен");
        }
        public void ShowOrdersFullInfo(DataGrid dataGrid, string status, string status2 = "")
        {
            using CoffeShopContext context = new CoffeShopContext();
            var orders = context.OrderFullInfoViews.FromSqlRaw($"SELECT 'Query Error' AS Message;");
            string plus = "";
            if (status2 != "")
            {
                plus = $"or Status = '{status2}'";
            }
            orders = context.OrderFullInfoViews.FromSqlRaw($"Select * from OrderFullInfoView where Status = '{status}' {plus} and Address = '{CoffeShopApplication.Properties.Settings.Default.ShopAddress}' order by OrderDateTime, Status");

            dataGrid.ItemsSource = orders.ToList();
        }

        private void UpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowOrdersFullInfo(newOrdersDataGrid, "Принят");
            ShowOrdersFullInfo(processOrdersDataGrid, "Готовится", "Ожидает выдачи");
            ShowOrdersFullInfo(complitedOrdersDataGrid, "Получен", "Отменен");
        }
    }
}
