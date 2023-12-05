using ToolShop.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = App.Context.Users.Where(u => u.Login == loginBox.Text && u.Password.Equals(passwordBox.Password)).FirstOrDefault();
            if (currentUser != null)
            {
                App.CurrentUser = currentUser;
                if (App.CurrentUser.RoleID == 1)
                {
                    //Вход админ

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    //Вход клиент

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                Window.GetWindow(this).Close();
            }
            else
            {
                currentUser = App.Context.Users.Where(u => u.Login == loginBox.Text).FirstOrDefault();
                if (currentUser != null)
                {
                    MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void guestButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }
    }
}
