using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Firebase.Database;
using WpfApp2.Helpers;
using WpfApp2.MainFrame;
using WpfApp2.Models;
using WpfApp2.Windows;

namespace WpfApp2.MainFremaPage
{
    /// <summary>
    /// Interaction logic for VehiclesPage.xaml
    /// </summary>
    public partial class VehiclesPage : Page
    {
        static bool executed = true;
        static ObservableCollection<Vehicles> VehiclesCollection = new ObservableCollection<Vehicles>();
        static FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");
        VehiclesHelper vehiclesHelper = new VehiclesHelper();
        public VehiclesPage()
        {
            InitializeComponent();

            if (executed)
            {
                GetVehicles();
                executed = false;
            }

            VehiclesDataGrid.ItemsSource = VehiclesCollection;
        }

        void GetVehicles()
        {
            VehiclesCollection = client
                .Child("vehicles")
                .AsObservable<Vehicles>()
                .ObserveOnDispatcher()
                .AsObservableCollection();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            VehiclesWindow v = new VehiclesWindow();
            v.ShowDialog();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Vehicles selectedVehicle = VehiclesDataGrid.SelectedItem as Vehicles;
            VehiclesWindow vw = new VehiclesWindow(selectedVehicle);
            vw.Show();
        }

        private async void DeleteVehicle()
        {
            MessageBoxResult res = MessageBox.Show("Do you really want to delete this vehicle?", "Delete Vehicle", MessageBoxButton.YesNo);//запитання для видалення продукту(так\ні)
            switch (res)
            {
                case MessageBoxResult.Yes://якщо так - видаляємо
                    if (VehiclesDataGrid.SelectedItem != null)//якщо є вибраний рядок у датагрід
                    {
                        Vehicles toDeleteVehicle = VehiclesDataGrid.SelectedItem as Vehicles;//тоді приводимо вибраний рядок в Products
                        await vehiclesHelper.DeleteVehicle(toDeleteVehicle.ID);//і видаляємо дані з конкретним айді продукту
                    }
                    break;
                case MessageBoxResult.No://якщо ні, то ні
                    break;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TODO: implement searching vehicles
        }

        private void VehiclesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VehiclesDataGrid.SelectedItem != null) //якщо є вибраний рядок
            {//дозволимо видаляти або редагувати його
                ButtonDelete.IsEnabled = true;
                ButtonEdit.IsEnabled = true;
            }
            else
            {//інакше, не дозволимо
                ButtonDelete.IsEnabled = false;
                ButtonEdit.IsEnabled = false;
            }
        }

        private void VehiclesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vehiclesList = VehiclesCollection;
            if (VehiclesDataGrid.SelectedIndex >= 0 && VehiclesDataGrid.SelectedIndex < vehiclesList.Count)//якщо вибраний індекс елементу є більшим за 0 і, водночас, не перевищує максимальний індекс даних
            {
                Vehicles specificVehicle = VehiclesDataGrid.SelectedItem as Vehicles;//тоді приводимо вибраний рядок до даних продуктів
                VehiclesWindow v = new VehiclesWindow(specificVehicle);//передаємо в екземпляр вікна продукт
                v.Show();//відкриваємо вікно з переданими даними
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteVehicle();
        }
    }
}
