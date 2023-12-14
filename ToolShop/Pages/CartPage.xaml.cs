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
using ToolShop.Windows;

namespace ToolShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public Orders currentOrder = App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault();
        public CartPage()
        {
            InitializeComponent();
            Update();
        }
        private void Update()
        {
            var currentOrderProducts = App.Context.OrderProducts.Where(op => op.OrderID == currentOrder.ID).ToList();
            orderProductsListView.ItemsSource = null;
            orderProductsListView.ItemsSource = currentOrderProducts;
            double sum = 0;
            foreach (var product in currentOrderProducts)
            {
                var currentProduct = App.Context.Products.Where(p => p.ID == product.ProductID).First();
                double productPrice = (double)currentProduct.Price;
                sum += productPrice * product.Amount;
            }
            OrderPriceBox.Text = $"Стоимость заказа: {sum} руб.";
        }

        private void createOrder_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null)
            {
                if(MessageBox.Show("Чтобы оформить заказ необходимо авторизироваться. Хотите продолжить?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                    authorizationWindow.Show();
                    Window.GetWindow(this).Close();
                }
                return;
            }
            else
            {
                App.Context.Orders.Where(o => o.UserID == App.CurrentUser.ID && o.OrderStatusID == 3).FirstOrDefault().OrderStatusID = 1;
                App.Context.SaveChanges();
            }
        }
        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var currentOrderProduct = button.DataContext as OrderProducts;
            if ((currentOrderProduct.Amount - 1) == 0)
            {
                if (App.CurrentUser == null)
                {
                    if (MessageBox.Show($"Вы уверены, что хотите удалить из заказа товар: {currentOrderProduct.productName}", "Внимание",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        App.CurrentOrderProducts.Remove(currentOrderProduct);
                        if (App.CurrentOrderProducts.Count == 0)
                        {
                            App.CurrentOrder = null;
                            App.CurrentOrderProducts = null;
                            NavigationService.Navigate(new ToolsPage());
                            return;
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show($"Вы уверены, что хотите удалить из заказа товар: {currentOrderProduct.productName}", "Внимание",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        App.Context.OrderProducts.Remove(currentOrderProduct);
                        if (App.Context.OrderProducts.Where(op => op.OrderID == currentOrder.ID).ToList().Count == 0)
                        {
                            App.Context.Orders.Remove(currentOrder);
                            App.Context.SaveChanges();
                            NavigationService.Navigate(new ToolsPage());
                            return;
                        }
                        App.Context.SaveChanges();
                    }
                }
            }
            else
            {
                currentOrderProduct.Amount--;
                App.Context.SaveChanges();
            }
            Update();
        }
        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var currentOrderProduct = button.DataContext as OrderProducts;
            if ((currentOrderProduct.Amount + 1) > currentOrderProduct.amountInStock)
            {
                MessageBox.Show("Превышен лимит товара на складе");
            }
            else
            {
                currentOrderProduct.Amount++;
                if (App.CurrentUser != null)
                {
                    App.Context.SaveChanges();
                }
            }
            Update();
        }
        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var currentOrderProduct = button.DataContext as OrderProducts;
            var currentTool = App.Context.Products.Where(p => p.ID == currentOrderProduct.ProductID).FirstOrDefault();
            NavigationService.Navigate(new ProductInfoPage(currentTool));
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var currentOrderProduct = button.DataContext as OrderProducts;
            if (MessageBox.Show($"Вы уверены, что хотите удалить из заказа товар: {currentOrderProduct.productName}", "Внимание", 
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (App.CurrentUser == null)
                {
                    App.CurrentOrderProducts.Remove(currentOrderProduct);
                    if (App.CurrentOrderProducts.Count == 0)
                    {
                        App.CurrentOrder = null;
                        App.CurrentOrderProducts = null;
                        NavigationService.Navigate(new ToolsPage());
                        return;
                    }
                }
                else
                {
                    App.Context.OrderProducts.Remove(currentOrderProduct);
                    if (App.Context.OrderProducts.Where(op => op.OrderID == currentOrder.ID).ToList().Count == 0)
                    {
                        App.Context.Orders.Remove(currentOrder);
                        App.Context.SaveChanges();
                        NavigationService.Navigate(new ToolsPage());
                        return;
                    }
                    App.Context.SaveChanges();
                }
            }
            Update();
        }
    }
}
