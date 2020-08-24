using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp2.Helpers;
using WpfApp2.Models;

namespace WpfApp2.MainFrame
{
    /// <summary>
    /// Interaction logic for FishWondow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        bool edited = true;
        List<Workers> packers = new List<Workers>();
        List<Workers> drivers = new List<Workers>();
        string packerId;
        string driverId;
        ProductsHelper productsHelper = new ProductsHelper();
        WorkersHelper workersHelper = new WorkersHelper();
        Products Product { get; set; }
        public ProductWindow(Products product = null)
        {
            InitializeComponent();

            
            Product = product;
            if(product == null)
            {
                Product = new Products();
                DeleteProductStackPanel.Visibility = Visibility.Hidden;
                edited = false;
            }
            DataContext = Product;
            
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            ProductsTextBoxes.BindingGroup.CancelEdit();//не змінює дані в датагрід(локально), якщо натиснутка кнопка "Закрити". 
                                                        //Дані не змінювалися в firebase, тільки в датагрід. При перезавантаженні дані відновлювалися.
            Close();
        }

        #region Focus Moving
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                ButtonSave.Focus();
                e.Handled = true;
               
            }

            if (e.Key == Key.Enter && ButtonSave.IsFocused == false)
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(tRequest);
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Enter && ButtonSave.IsFocused == true)
            {
                MessageBox.Show("The product has been saved");
                Close();
            }
        }
        #endregion

        private async void ButtonDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            Close();
            await productsHelper.DeleteProduct(Product.id);
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Close();

            if (edited == false)
            {
                await productsHelper.AddProduct(
                    productNameTextBox.Text,
                    productPriceTextBox.Text,
                    productDatePicker.Text,
                    productStatusComboBox.Text,
                    packerId,
                    driverId
                    );
            }
            if (edited == true)
            {
                await productsHelper.UpdateProduct(
                    Product.id,
                    productNameTextBox.Text,
                    productPriceTextBox.Text,
                    productDatePicker.Text,
                    productStatusComboBox.Text,
                    packerId,
                    driverId);
            }
        }

        async void UploadPackersAndDrivers()
        {
            var workers = await workersHelper.GetAllWorkersAsync();

            packers = (from p in workers
                       where p.position.ToLower() == "packer"
                       select p).ToList();
            PackersComboBox.ItemsSource = packers;

            drivers =  (from p in workers
                       where p.position.ToLower() == "driver"
                       select p).ToList();
            DriversComboBox.ItemsSource = drivers;
        }

        private void DriversComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DriversComboBox.SelectedItem != null)
            {
                Workers selectedDriver = DriversComboBox.SelectedItem as Workers;
                driverId = selectedDriver.w_id;
            }
        }

        private void PackersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PackersComboBox.SelectedItem != null)
            {
                Workers selectedPacker = PackersComboBox.SelectedItem as Workers;
                packerId = selectedPacker.w_id;
            }
        }

        async void FillPackerAndDriverComboBoxes()
        {
            if (edited == true)
            {
                Workers specificDriver = await workersHelper.GetWorker(Product.driver);
                Workers specificPacker = await workersHelper.GetWorker(Product.packer);
                if (specificPacker != null && specificDriver != null)
                {
                    string driverNameSurname = specificDriver.name + " " + specificDriver.surname;
                    string packerNameSurname = specificPacker.name + " " + specificPacker.surname;

                    DriversComboBox.Text = driverNameSurname;
                    PackersComboBox.Text = packerNameSurname;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (Product != null)
            {
                UploadPackersAndDrivers();
                FillPackerAndDriverComboBoxes();   
            }
        }
    }
}
