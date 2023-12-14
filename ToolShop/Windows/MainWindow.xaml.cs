using ToolShop.Windows;
using ToolShop.Pages;
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

namespace ToolShop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            fullUserName();
            FrameMain.Navigate(new ToolsPage());
        }
        private void fullUserName()
        {
            var userName = "Гость";
            if (App.CurrentUser != null)
            {
                userName = $"{App.CurrentUser.Surname} {App.CurrentUser.Firstname} {App.CurrentUser.Patronymic}";
            }
            userNameBlock.Text = userName;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (FrameMain.CanGoBack)
            {
                FrameMain.GoBack();
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            App.CurrentUser = null;
            this.Close();
        }

        private void ToolsPageButton_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new ToolsPage());
        }

        private void OrdersPageButton_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new OrdersPage());
        }
    }
}
