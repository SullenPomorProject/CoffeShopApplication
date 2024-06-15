using CoffeShopApplication.DataLayer;
using CoffeShopApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            UpdateAll();
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

            List<OrderFullInfoView> OrderFullInfoViews = new List<OrderFullInfoView>();
            foreach (var order in orders)
            {
                OrderFullInfoView OrderFullInfoView = new OrderFullInfoView
                {
                    IdOrder = order.IdOrder,
                    Address = order.Address,
                    Customer = order.Customer,
                    OrderDateTime = order.OrderDateTime,
                    Composition = order.Composition,
                    Options = order.Options,
                    Cost = order.Cost,
                    Status = order.Status,
                    ChangeStatusCommand = new RelayCommand(o =>
                    {
                        // Логика изменения значения в таблице Orders.Status по IdOrder
                        using CoffeShopContext updateContext = new CoffeShopContext();
                        var orderToUpdate = updateContext.Orders.FirstOrDefault(o => o.IdOrder == order.IdOrder);
                        if (orderToUpdate != null)
                        {
                            switch (orderToUpdate.Status)
                            {
                                case "Принят":
                                    orderToUpdate.Status = "Готовится";
                                    break;
                                case "Готовится":
                                    orderToUpdate.Status = "Ожидает выдачи";
                                    break;
                                case "Ожидает выдачи":
                                    orderToUpdate.Status = "Получен";
                                    break;
                                default:
                                    // Здесь можно добавить дополнительные статусы или оставить пустым, если заказ остается в статусе "Получен"
                                    break;
                            }
                            updateContext.SaveChanges();
                        }
                        UpdateAll();
                    })
                };
                OrderFullInfoViews.Add(OrderFullInfoView);
            }

            dataGrid.ItemsSource = OrderFullInfoViews;

            //dataGrid.ItemsSource = orders.ToList();
        }
        public void ShowShopProducts()
        {

        }

        private void UpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateAll();
        }

        public void UpdateAll()
        {
            ShowOrdersFullInfo(newOrdersDataGrid, "Принят");
            ShowOrdersFullInfo(processOrdersDataGrid, "Готовится", "Ожидает выдачи");
            ShowOrdersFullInfo(complitedOrdersDataGrid, "Получен", "Отменен");
        }
    }
}
