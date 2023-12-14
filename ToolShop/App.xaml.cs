using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ToolShop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ToolShopEntities Context { get;} = new ToolShopEntities();
        public static Users CurrentUser = null;
        public static Orders CurrentOrder = null;
        public static List<OrderProducts> CurrentOrderProducts = null;
    }
}
