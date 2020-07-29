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
using WpfApp2.MainFrame;

namespace WpfApp2.Windows
{
    /// <summary>
    /// Interaction logic for BreedingWeeksWindow.xaml
    /// </summary>
    public partial class BreedingWeeksWindow : Window
    {
        bool edited = true;
        string BreedingDetailsId = FishBreedingPage.BreedingDetailsFromDataGridSelected?.ID;
        //List<BreedingDetails> breedingDetails = new List<BreedingDetails>();
        BreedingHelper bHelper = new BreedingHelper();
        BreedingWeeks BreedingWeek { get; set; }
        public BreedingWeeksWindow(BreedingWeeks breedingWeeks = null)
        {
            InitializeComponent();
            BreedingWeek = breedingWeeks;
            if (breedingWeeks == null)
            {
                BreedingWeek = new BreedingWeeks();
                edited = false;
                DeleteBreedingWeekStackPanel.Visibility = Visibility.Visible;
            }
        }



        async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Close();

            if (!edited)
            {
                await bHelper.AddBreedingWeek(
                    WeekNumberTextBox.Text,
                    WeekDateTextBox.Text,
                    WaterLevelTextBox.Text,
                    BreedingDetailsId
                    );
            }
            else if (edited)
            {
                await bHelper.UpdateBreedingWeek(
                    BreedingWeek.ID,
                    WeekNumberTextBox.Text,
                    WeekDateTextBox.Text,
                    WaterLevelTextBox.Text,
                    BreedingDetailsId
                    );
            }
        }

        async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            await bHelper.DeleteBreedingWeek(BreedingWeek.ID);
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
