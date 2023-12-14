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
    /// Логика взаимодействия для ProductInfoPAge.xaml
    /// </summary>
    public partial class ProductInfoPage : Page
    {
        public Products currentProduct;
        public ProductInfoPage(Products product)
        {
            InitializeComponent();
            currentProduct = product;
            //Код вывода информации
            nameBox.Text = $"{currentProduct.Title}";
            descriptionBox.Text = $"{currentProduct.Description}";
            categoryBox.Text = $"{currentProduct.productTypeName}";
            priceBox.Text = $"{currentProduct.Price} руб.";
            amountInStock.Text = $"{currentProduct.AmountInStock} шт.";
            manufacturerBox.Text = $"{App.Context.Manufacturers.Where(m => m.ID == currentProduct.ManufacturerID).First().Title}";
            supplierBox.Text = $"{App.Context.Suppliers.Where(m => m.ID == currentProduct.SupplierID).First().Title}";
        }

        private void addToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentOrder == null)
            {
                var order = new Orders
                {
                    UserID = 0,
                    OrderStatusID = 1,
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
                if (product.ProductID == currentProduct.ID)
                {
                    if ((product.Amount + 1) > currentProduct.AmountInStock)
                    {
                        MessageBox.Show("Превышен лимит товара на складе");
                        return;
                    }
                    product.Amount++;
                    flag = true;
                }
            }
            if (currentProduct.AmountInStock == 0)
            {
                MessageBox.Show("К сожалению, товара сейчас нет на складе");
                return;
            }
            if (!flag)
            {
                var orderProduct = new OrderProducts
                {
                    OrderID = App.CurrentOrder.ID,
                    ProductID = currentProduct.ID,
                    Amount = 1
                };
                App.CurrentOrderProducts.Add(orderProduct);
            }
        }
    }
}
