using Firebase.Database;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
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
using WpfApp2.Helpers;
using WpfApp2.MainFremaPage;
using WpfApp2.Windows;

namespace WpfApp2.MainFrame
{
    public partial class FishBreedingPage : Page
    {
        public static BreedingDetails BreedingDetailsFromDataGridSelected;
        static ObservableCollection<BreedingDetails> BreedingDetailsCollection = new ObservableCollection<BreedingDetails>();
        BreedingHelper breedingHelper = new BreedingHelper();
        static bool executed = false;
        public FishBreedingPage()
        {
            InitializeComponent();
            if (!executed)
            {
                GetBreedingDetailsCollection();
                executed = true;
            }

            BreedingDetailsDataGrid.ItemsSource = BreedingDetailsCollection;
        }

        void GetBreedingDetailsCollection()
        {
            try
            {
                BreedingDetailsCollection = breedingHelper.client
                             .Child("BreedingDetails")
                             .AsObservable<BreedingDetails>()
                             .ObserveOnDispatcher()
                             .AsObservableCollection();
            }
            catch (Exception)
            {

                MessageBox.Show("No breeding details");
            }
            
        }
        private void AddNewBreedingDetails_Click(object sender, RoutedEventArgs e)
        {
            BreedingDetailsWindow bw = new BreedingDetailsWindow();
            bw.Show();
        }

        private void BreedingDetailsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BreedingDetailsFromDataGridSelected = BreedingDetailsDataGrid.SelectedItem as BreedingDetails;
            if (BreedingDetailsDataGrid.SelectedItem != null)
            {
                ShowBreedingWeeksButton.IsEnabled = true;
                EditBreedingDetails.IsEnabled = true;
                DeleteBreedingDetails.IsEnabled = true;

            }
            else
            {
                ShowBreedingWeeksButton.IsEnabled = false;
                EditBreedingDetails.IsEnabled = false;
                DeleteBreedingDetails.IsEnabled = false;
            }
        }

        private void BreedingDetailsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            
            
        }

        private void EditBreedingDetails_Click(object sender, RoutedEventArgs e)
        {
            BreedingDetails selectedBreedingDetails = BreedingDetailsDataGrid.SelectedItem as BreedingDetails;
            BreedingDetailsWindow bw = new BreedingDetailsWindow(selectedBreedingDetails);
            bw.Show();
        }

        async void DeleteBreedingDetails_Click(object sender, RoutedEventArgs e)
        {
            var myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));
            MessageSnackbar.MessageQueue = myMessageQueue;
            var messageQueue = MessageSnackbar.MessageQueue;
            var message = "Item deleted";
            await Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            BreedingDetails selectedBreedingDetails = BreedingDetailsDataGrid.SelectedItem as BreedingDetails;
            await breedingHelper.DeleteBreedingDelails(selectedBreedingDetails.ID);
            var weeks = await breedingHelper.GetAllBreedingWeeksAsync();
            foreach(var w in weeks)
            {
                if(w.BreedingDetailsID == selectedBreedingDetails.ID)
                {
                    await breedingHelper.DeleteBreedingWeek(w.ID);
                }
            }
        }
    }
}