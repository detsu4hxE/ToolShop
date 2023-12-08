﻿using System;
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
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        public Products currentProduct;
        public AddEditProductPage(Products product)
        {
            InitializeComponent();
            if (product != null)
            {
                currentProduct = product;
                Title = "Редактирование товара";
                //код редактирования
            }
            else
            {
                currentProduct = null;
                Title = "Добавление товара";
                //код добавления
            }
        }
    }
}
