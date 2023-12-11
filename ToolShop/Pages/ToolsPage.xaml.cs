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
        public ToolsPage()
        {
            InitializeComponent();
            var categories = App.Context.ProductTypes.ToList();
            foreach (var category in categories)
            {
                filterBox.Items.Add(category.Title);
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
                    //Код удаления
                }
            }
            else
            {
                //Код добавления товара в корзину
            }
            
        }
    }
}
