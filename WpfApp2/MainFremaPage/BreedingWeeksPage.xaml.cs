using Firebase.Database;
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
using WpfApp2.Helpers;
using WpfApp2.MainFrame;
using WpfApp2.Windows;

namespace WpfApp2.MainFremaPage
{
    /// <summary>
    /// Interaction logic for BreedingWeeksPage.xaml
    /// </summary>
    public partial class BreedingWeeksPage : Page
    {
        static ObservableCollection<BreedingWeeks> BreedingWeeksCollection = new ObservableCollection<BreedingWeeks>();
        static FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");
        BreedingHelper BreedingHelper = new BreedingHelper();
        static bool executed = false;
        public BreedingWeeksPage()
        {
            InitializeComponent();
                GetBreedingWeeksCollection();
            BreedingWeeksDataGrid.ItemsSource = BreedingWeeksCollection;
        }
        void GetBreedingWeeksCollection()
        {

            BreedingWeeksCollection = client
             .Child("BreedingWeeks")
             .AsObservable<BreedingWeeks>().Where(bw => bw.Object.BreedingDetailsID == FishBreedingPage.BreedingDetailsFromDataGridSelected.ID)
             .ObserveOnDispatcher()
             .AsObservableCollection();

        }
        private void AddNewBreedingWeekButton_Click(object sender, RoutedEventArgs e)
        {
            BreedingWeeksWindow bw = new BreedingWeeksWindow();
            bw.ShowDialog();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!executed)
            {
                GetBreedingWeeksCollection();
                executed = true;
            }
            BreedingWeeksDataGrid.ItemsSource = BreedingWeeksCollection;
        }
   
    }
}
