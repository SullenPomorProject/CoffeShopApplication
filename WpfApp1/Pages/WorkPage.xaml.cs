using CoffeShopApplication.DataLayer;
using CoffeShopApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
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
        }
        public void ShowShopProducts()
        {
            using CoffeShopContext context = new CoffeShopContext();
            var shopProducts = context.ShopInfoViews.FromSqlRaw($"SELECT * from ShopInfoView where ShopAddress = '{CoffeShopApplication.Properties.Settings.Default.ShopAddress}' order by CategoryName;");

            List<ShopInfoView> ShopInfoViews = new List<ShopInfoView>();
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
                ShopInfoViews.Add(ShopInfoView);
            }
            shopProductsDataGrid.ItemsSource = ShopInfoViews;
        }
        private void SaveShopProductsChanges()
        {
            using CoffeShopContext context = new CoffeShopContext();
            var result = MessageBox.Show(
                $"Вы действительно хотите сохранить {shopProductsDataGrid.SelectedItems.Count} записей?",
                "Сохранение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );
            if (result != MessageBoxResult.Yes)
            {
                var x = shopProductsDataGrid.SelectedItem;
                var y = shopProductsDataGrid.SelectedValue;
                var z = shopProductsDataGrid.SelectedCells;
                System.Windows.Forms.MessageBox.Show(x.ToString());
                System.Windows.Forms.MessageBox.Show(y.ToString());
                System.Windows.Forms.MessageBox.Show(z.ToString());
                return;
            }
            try
            {
                var editedProducts = shopProductsDataGrid.SelectedItems.OfType<ShopInfoView>();
                System.Windows.Forms.MessageBox.Show(editedProducts.ToString());
                foreach (var product in editedProducts)
                {
                    context.Entry(product).State = EntityState.Modified;
                }
                context.SaveChanges();
                MessageBox.Show("Изменения успешно сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить изменения. Причина: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DelShopProducts()
        {
            using CoffeShopContext context = new CoffeShopContext();
            var result = MessageBox.Show(
                $"Вы действительно хотите удалить {shopProductsDataGrid.SelectedItems.Count} записей?",
                "Удаление",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            try
            {
                var products = shopProductsDataGrid.SelectedItems.OfType<ShopInfoView>();
                context.ShopInfoViews.RemoveRange(products);
                context.SaveChanges();
                MessageBox.Show("Данные успешно удалены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось удалить записи. Причина: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void SaveShopChangesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveShopProductsChanges();
            ShowShopProducts();
        }
        private void DelShopProductsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DelShopProducts();
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
                            var shopProduct = selectedItem.ToShopProduct();
                            dbContext.ShopProducts.Update(shopProduct);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            ShowShopProducts();
        }
    }
}
