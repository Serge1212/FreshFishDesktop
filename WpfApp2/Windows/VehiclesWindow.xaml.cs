using System;
using System.Collections.Generic;
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

namespace WpfApp2.Windows
{
    /// <summary>
    /// Логика взаимодействия для VehiclesWindow.xaml
    /// </summary>
    public partial class VehiclesWindow : Window
    {
        private bool edited = true; 
        VehiclesHelper vehiclesHelper = new VehiclesHelper(); 
        Vehicles Vehicle { get; set; }
        public VehiclesWindow(Vehicles vehicle = null)
        {
            InitializeComponent();

            Vehicle = vehicle;
            if (vehicle == null)
            {
                Vehicle = new Vehicles();
                DeleteVehicleStackPanel.Visibility = Visibility.Hidden;
                edited = false;
            }

            DataContext = Vehicle;
        }

        private void ProductsTextBoxes_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        async void ButtonDeleteProduct_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            await vehiclesHelper.DeleteVehicle(Vehicle.ID);
        }

        async void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            if (!edited)
            {
                await vehiclesHelper.AddVehicles(
                    modelTextBox.Text,
                    markTextBox.Text,
                    manufactureDateDatePicker.Text,
                    mileageTextBox.Text,
                    fuelConsumptionTextBox.Text
                );
            }
            else if (edited)
            {
                await vehiclesHelper.UpdateVehicle(
                    Vehicle.ID,
                    modelTextBox.Text,
                    markTextBox.Text,
                    manufactureDateDatePicker.Text,
                    mileageTextBox.Text,
                    fuelConsumptionTextBox.Text
                );
            }
        }

        private void Button_close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
