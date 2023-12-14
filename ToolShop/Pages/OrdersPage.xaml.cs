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

        private void infoOrEditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentTool = button.DataContext as Products;
            if (button.Content.ToString() == "Изменить")
            {
                NavigationService.Navigate(new AddEditProductPage(currentTool));
            }
            else
            {
                NavigationService.Navigate(new ProductInfoPage(currentTool));
            }
        }

        private void addOrDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentTool = button.DataContext as Products;
            if (button.Content.ToString() == "Удалить")
            {
                var orderProduct = App.Context.OrderProducts.Where(op => op.ProductID == currentTool.ID).ToList();
                if (MessageBox.Show($"Вы уверены, что хотите удалить товар: \"{currentTool.Title}\", также будет удалено {orderProduct.Count} записей из таблицы \"OrderProducts\"", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    App.Context.Products.Remove(currentTool);
                    App.Context.SaveChanges();
                    Update();
                }
            }
            else
            {
                //Код добавления товара в корзину
            }

        }
    }
}
