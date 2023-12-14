using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ToolsPage.xaml
    /// </summary>
    public partial class ToolsPage : Page
    {
        public Orders currentOrder = null;
        public ToolsPage()
        {
            InitializeComponent();
            var categories = App.Context.ProductTypes.ToList();
            if (App.CurrentUser != null)
            {
                if (App.CurrentUser.RoleID == 1)
                {
                    addToolOrCartButton.Content = "Добавить товар";
                    addToolOrCartButton.Visibility = Visibility.Visible;
                }
            }
            foreach (var category in categories)
            {
                filterBox.Items.Add(category.Title);
            }
            filterBox.SelectedIndex = 0;
            sortBox.SelectedIndex = 0;
            if (App.CurrentUser != null)
            {
                if (App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault() != null)
                {
                    currentOrder = App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault();
                }
                else if (App.CurrentOrder != null)
                {
                    App.CurrentOrder.UserID = App.CurrentUser.ID;
                    App.CurrentOrder.CreationDate = DateTime.Today;
                    App.CurrentOrder.OrderStatusID = 3;
                    App.Context.Orders.Add(App.CurrentOrder);
                    App.Context.SaveChanges();
                    currentOrder = App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault();
                    foreach (var orderProduct in App.CurrentOrderProducts)
                    {
                        orderProduct.OrderID = App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault().ID;
                        App.Context.OrderProducts.Add(orderProduct);
                    }
                    App.Context.SaveChanges();
                    App.CurrentOrder = null;
                    App.CurrentOrderProducts = null;
                }
            }
            Update();
        }

        private void addToolOrCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (addToolOrCartButton.Content.ToString() == "Добавить товар")
            {
                NavigationService.Navigate(new AddEditProductPage(null));
            }
            else
            {
                NavigationService.Navigate(new CartPage());
            }
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
            var tools = App.Context.Products.ToList();
            switch (sortBox.SelectedIndex)
            {
                case 0:
                    tools = tools.OrderBy(t => t.Title).ToList();
                    break;
                case 1:
                    tools = tools.OrderByDescending(t => t.Title).ToList();
                    break;
                case 2:
                    tools = tools.OrderBy(t => t.Price).ToList();
                    break;
                case 3:
                    tools = tools.OrderByDescending(t => t.Price).ToList();
                    break;
                default:
                    break;
            }
            if (filterBox.SelectedIndex != 0)
            {
                tools = tools.Where(t => t.productTypeName == filterBox.SelectedItem.ToString()).ToList();
            }
            tools = tools.Where(t => t.Title.ToLower().Contains(searchBox.Text.ToLower()) || t.Description.ToLower().Contains(searchBox.Text.ToLower())).ToList();
            var amount = App.Context.Products.ToList().Count;
            if (tools.Count == 0)
            {
                searchResultBox.Text = "По вашему запросу ничего не найдено";
            }
            else
            {
                searchResultBox.Text = $"Найдено {tools.Count} инструментов из {amount}";
            }
            toolsListView.ItemsSource = null;
            toolsListView.ItemsSource = tools;
            if (App.CurrentUser == null)
            {
                if (App.CurrentOrder != null)
                {
                    addToolOrCartButton.Visibility = Visibility.Visible;
                }
            }
            else if (App.CurrentUser.RoleID != 1)
            {
                if (App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault() != null)
                {
                    addToolOrCartButton.Visibility = Visibility.Visible;
                }
            }
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
                if (MessageBox.Show($"Вы уверены, что хотите удалить товар: \"{currentTool.Title}\", также будет удалено {orderProduct.Count} записей из таблицы \"OrderProducts\"",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    App.Context.Products.Remove(currentTool);
                    App.Context.SaveChanges();
                }
            }
            else
            {
                if (App.CurrentUser == null)
                {
                    if (App.CurrentOrder == null)
                    {
                        var order = new Orders
                        {
                            UserID = 0,
                            OrderStatusID = 3,
                            CreationDate = DateTime.Today,
                        };
                        App.CurrentOrder = order;
                    }
                    if (App.CurrentOrderProducts == null)
                    {
                        App.CurrentOrderProducts = new List<OrderProducts>();
                    }
                    bool flag = false;
                    foreach (var product in App.CurrentOrderProducts)
                    {
                        if (product.ProductID == currentTool.ID)
                        {
                            if ((product.Amount + 1) > currentTool.AmountInStock)
                            {
                                MessageBox.Show("Превышен лимит товара на складе");
                                return;
                            }
                            product.Amount++;
                            flag = true;
                        }
                    }
                    if (currentTool.AmountInStock == 0)
                    {
                        MessageBox.Show("К сожалению, товара сейчас нет на складе");
                        return;
                    }
                    if (!flag)
                    {
                        var orderProduct = new OrderProducts
                        {
                            OrderID = App.CurrentOrder.ID,
                            ProductID = currentTool.ID,
                            Amount = 1
                        };
                        App.CurrentOrderProducts.Add(orderProduct);
                    }
                }
                else
                {
                    if (App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault() == null)
                    {
                        var order = new Orders
                        {
                            UserID = App.CurrentUser.ID,
                            OrderStatusID = 3,
                            CreationDate = DateTime.Today,
                        };
                        App.Context.Orders.Add(order);
                        App.Context.SaveChanges();
                        currentOrder = App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault();
                    }
                    var currentOrderProducts = App.Context.OrderProducts.Where(op => op.OrderID == currentOrder.ID).ToList();
                    bool flag = false;
                    foreach (var product in currentOrderProducts)
                    {
                        if (product.ProductID == currentTool.ID)
                        {
                            if ((product.Amount + 1) > currentTool.AmountInStock)
                            {
                                MessageBox.Show("Превышен лимит товара на складе");
                                return;
                            }
                            product.Amount++;
                            App.Context.SaveChanges();
                            flag = true;
                        }
                    }
                    if (currentTool.AmountInStock == 0)
                    {
                        MessageBox.Show("К сожалению, товара сейчас нет на складе");
                        return;
                    }
                    if (!flag)
                    {
                        var orderProduct = new OrderProducts
                        {
                            OrderID = currentOrder.ID,
                            ProductID = currentTool.ID,
                            Amount = 1
                        };
                        App.Context.OrderProducts.Add(orderProduct);
                        App.Context.SaveChanges();
                    }
                }
            }
            Update();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
