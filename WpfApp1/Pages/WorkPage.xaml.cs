using CoffeShopApplication.DataLayer;
using CoffeShopApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary>
    public partial class WorkPage : System.Windows.Controls.Page
    {
        public WorkPage()
        {
            InitializeComponent();
            shopAddressLabel.Content = CoffeShopApplication.Properties.Settings.Default.ShopAddress;
            UpdateAll();
            ShowShopProducts();
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
        }
        public void ShowShopProducts()
        {
            using CoffeShopContext context = new CoffeShopContext();
            var shopProducts = context.ShopInfoViews.FromSqlRaw($"SELECT * from ShopInfoView where ShopAddress = '{CoffeShopApplication.Properties.Settings.Default.ShopAddress}' order by CategoryName;");

            List<ShopInfoView> ShopInfoViews2 = new List<ShopInfoView>();
            foreach (var product in shopProducts)
            {
                ShopInfoView ShopInfoView = new ShopInfoView
                {
                    IdShop = product.IdShop,
                    IdProduct = product.IdProduct,
                    ShopAddress = product.ShopAddress,
                    CategoryName = product.CategoryName,
                    ProductName = product.ProductName,
                    Count = product.Count
                };
                ShopInfoViews2.Add(ShopInfoView);
            }
            shopProductsDataGrid.ItemsSource = ShopInfoViews2;
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
            ShowShopProducts();
        }


        private void ShopProductsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Count")
            {
                var dataGrid = sender as DataGrid;

                if (dataGrid != null)
                {
                    var selectedItem = dataGrid.SelectedItem as ShopInfoView;
                    if (selectedItem != null)
                    {
                        using (var dbContext = new CoffeShopContext())
                        {
                            if (int.TryParse((e.EditingElement as TextBox).Text, out int newCount))
                            {
                                // Значение успешно преобразовано в тип int и сохранено в переменную newCount
                                string updateQuery = $"UPDATE ShopProducts SET Count = {newCount} WHERE IdShop = {selectedItem.IdShop} AND IdProduct = {selectedItem.IdProduct}";
                                dbContext.Database.ExecuteSqlRaw(updateQuery);
                                dbContext.SaveChanges();
                                System.Windows.Forms.MessageBox.Show("Данные изменены");
                            }
                            else
                            {
                                // При ошибке преобразования будет выполнен этот блок кода
                                System.Windows.Forms.MessageBox.Show("Неверный формат числа");
                            }
                        }
                    }
                }
            }
            ShowShopProducts();
        }
    }
}
