using Microsoft.Win32;
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
using System.IO;
using System.Data.Odbc;
using System.Text.RegularExpressions;

namespace ToolShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        public Products currentProduct;
        public byte[] imageData = null;
        public string currentImage = null;
        public string extension = ".png";
        public string selectedFileName;
        public string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + $"/Resources/";

        public Regex intNumber = new Regex(@"^\d+$");
        public Regex doubleNumber = new Regex(@"^\d+(,\d+)?$");
        MatchCollection matches;
        public AddEditProductPage(Products product)
        {
            InitializeComponent();
            FillComboBoxes();
            if (product != null)
            {
                currentProduct = product;
                Title = "Редактирование товара";
                saveButton.Content = "Сохранить";
                FillData();
            }
            else
            {
                currentProduct = null;
                Title = "Добавление товара";
                saveButton.Content = "Добавить";
            }
        }
        private void FillComboBoxes()
        {
            categoryBox.ItemsSource = App.Context.ProductTypes.ToList().Select(c => c.Title);
            manufacturerBox.ItemsSource = App.Context.Manufacturers.ToList().Select(m => m.Title);
            supplierBox.ItemsSource = App.Context.Suppliers.ToList().Select(s => s.Title);
        }
        private void FillData()
        {
            nameBox.Text = currentProduct.Title;
            descriptionBox.Text = currentProduct.Description;
            categoryBox.SelectedItem = App.Context.ProductTypes.Where(pt => pt.ID == currentProduct.ProductTypeID).First().Title;
            priceBox.Text = currentProduct.Price.ToString();
            amountInStock.Text = currentProduct.AmountInStock.ToString();
            manufacturerBox.SelectedItem = App.Context.Manufacturers.Where(m => m.ID == currentProduct.ManufacturerID).First().Title;
            supplierBox.SelectedItem = App.Context.Suppliers.Where(s => s.ID == currentProduct.SupplierID).First().Title;
            currentImage = currentProduct.Image;
            if (currentImage != null)
            {
                imageData = File.ReadAllBytes(path + currentImage);
                imageBox.Source = new ImageSourceConverter().ConvertFrom(imageData) as ImageSource;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = checkErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage);
                return;
            }
            if (currentImage != null)
            {
                currentImage = nameBox.Text + extension;

                int a = 0;
                while (File.Exists(path + currentImage))
                {
                    a++;
                    currentImage = $"{nameBox.Text}({a}){extension}";
                }
                File.Copy(selectedFileName, path + currentImage);
            }
            else if (currentProduct.Image != null)
            {
                currentImage = currentProduct.Image;
            }
            if (currentProduct != null)
            {
                currentProduct.Title = nameBox.Text;
                currentProduct.Description = descriptionBox.Text;
                currentProduct.ProductTypeID = App.Context.ProductTypes.Where(pt => pt.Title == categoryBox.Text).First().ID;
                currentProduct.Price = (decimal)(double.Parse(priceBox.Text));
                currentProduct.AmountInStock = int.Parse(amountInStock.Text);
                currentProduct.ManufacturerID = App.Context.Manufacturers.Where(m => m.Title == manufacturerBox.Text).First().ID;
                currentProduct.SupplierID = App.Context.Suppliers.Where(s => s.Title == supplierBox.Text).First().ID;
                currentProduct.Image = currentImage;
                App.Context.SaveChanges();
                MessageBox.Show("Товар успешно обновлен");
            }
            else
            {
                Products product = new Products();

                product.Title = nameBox.Text;
                product.Description = descriptionBox.Text;
                product.ProductTypeID = App.Context.ProductTypes.Where(pt => pt.Title == categoryBox.Text).First().ID;
                product.Price = (decimal)(double.Parse(priceBox.Text));
                product.AmountInStock = int.Parse(amountInStock.Text);
                product.ManufacturerID = App.Context.Manufacturers.Where(m => m.Title == manufacturerBox.Text).First().ID;
                product.SupplierID = App.Context.Suppliers.Where(s => s.Title == supplierBox.Text).First().ID;
                product.Image = currentImage;

                App.Context.Products.Add(product);
                App.Context.SaveChanges();
                MessageBox.Show("Товар успешно добавлен");
            }
            NavigationService.Navigate(new ToolsPage());
        }

        private void selectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Image | *.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                selectedFileName = ofd.FileName;
                currentImage = Path.GetFileName(selectedFileName);
                extension = Path.GetExtension(currentImage);
                imageData = File.ReadAllBytes(selectedFileName);
                imageBox.Source = new ImageSourceConverter().ConvertFrom(imageData) as ImageSource;
            }
        }
        private string checkErrors()
        {
            var errors = new StringBuilder();

            matches = intNumber.Matches(amountInStock.Text.ToString());
            if (matches.Count == 0)
            {
                errors.AppendLine("Неверно введен остаток на складе");
            }
            matches = doubleNumber.Matches(priceBox.Text.ToString());
            if (matches.Count == 0)
            {
                errors.AppendLine("Неверно введена стоимость");
            }
            if (errors.Length > 0)
            {
                errors.Insert(0, "Устраните следующие ошибки:\n");
            }

            return errors.ToString();
        }
    }
}
