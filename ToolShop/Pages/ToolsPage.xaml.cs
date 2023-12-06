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
            Update();
        }
        private void Update()
        {
            var tools = App.Context.Products.ToList();
            toolsListView.ItemsSource = null;
            toolsListView.ItemsSource = tools;
        }

        private void addToolButton_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
