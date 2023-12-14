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

namespace ToolShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            var statuses = App.Context.OrderStatus.ToList();
            foreach (var status in statuses)
            {
                filterBox.Items.Add(status.Title);
            }
            filterBox.SelectedIndex = 0;
            sortBox.SelectedIndex = 0;
            Update();
        }
        private void addToolButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(null));
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void sortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void filterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            var orders = App.Context.Orders.ToList();
            if (App.CurrentUser.RoleID != 1)
            {
                orders = orders.Where(o => o.UserID == App.CurrentUser.ID).ToList();
            }
            switch (sortBox.SelectedIndex)
            {
                case 0:
                    orders = orders.OrderBy(o => o.CreationDate).ToList();
                    break;
                case 1:
                    orders = orders.OrderByDescending(o => o.CreationDate).ToList();
                    break;
                case 2:
                    orders = orders.OrderBy(o => o.price).ToList();
                    break;
                case 3:
                    orders = orders.OrderByDescending(o => o.price).ToList();
                    break;
                default:
                    break;
            }
            if (filterBox.SelectedIndex != 0)
            {
                orders = orders.Where(o => o.statusName == filterBox.SelectedItem.ToString()).ToList();
            }
            orders = orders.Where(o => o.productsList.ToLower().Contains(searchBox.Text.ToLower()) || o.FIO.ToLower().Contains(searchBox.Text.ToLower())).ToList();
            var amount = App.Context.Orders.ToList().Count;
            if (orders.Count == 0)
            {
                searchResultBox.Text = "По вашему запросу ничего не найдено";
            }
            else
            {
                searchResultBox.Text = $"Найдено {orders.Count} заказов из {amount}";
            }
            ordersListView.ItemsSource = null;
            ordersListView.ItemsSource = orders;
        }
        private void performOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var order = button.DataContext as Orders;
            if (order.OrderStatusID == 2)
            {
                MessageBox.Show("Заказ уже выполнен");
                return;
            }
            if (order.OrderStatusID == 3)
            {
                MessageBox.Show("Заказ еще находится в корзине");
                return;
            }
            //Сначала идет проверка, есть ли товар на складе
            foreach (var orderProduct in App.Context.OrderProducts.Where(op => op.OrderID == order.ID).ToList())
            {
                if ((App.Context.Products.Where(p => p.ID == orderProduct.ProductID).FirstOrDefault().AmountInStock - orderProduct.Amount) < 0)
                {
                    MessageBox.Show("Товара на складе не достаточно для выполнения данного заказа");
                    return;
                }
            }
            //После проверки уже вычитаем товар со склада
            //Нельзя делать в одном цикле, чтобы не было ошибок по типу:
            //"Попытался сделать заказ, первые 2 товара успешно списали со склада, 3 недостаточно"-->
            //-->"Данные в базе не обновились, но при следующем обновлении первые 2 списания будут все равно учтены"
            foreach (var orderProduct in App.Context.OrderProducts.Where(op => op.OrderID == order.ID).ToList())
            {
                App.Context.Products.Where(p => p.ID == orderProduct.ProductID).FirstOrDefault().AmountInStock -= orderProduct.Amount;
            }
            order.OrderStatusID = 2;
            App.Context.SaveChanges();
            Update();
        }
    }
}
