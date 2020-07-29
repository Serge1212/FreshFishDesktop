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

namespace WpfApp2.Windows
{
    /// <summary>
    /// Interaction logic for BreedingDetailsWindow.xaml
    /// </summary>
    public partial class BreedingDetailsWindow : Window
    {
        bool edited = true;
        BreedingHelper bHelper = new BreedingHelper();
        BreedingDetails BreedingDetails { get; set; }
        public BreedingDetailsWindow(BreedingDetails breedingDetails = null)
        {
            InitializeComponent();

            BreedingDetails = breedingDetails;
            if (breedingDetails == null)
            {
                BreedingDetails = new BreedingDetails();
                ButtonDelete.Visibility = Visibility.Hidden;
                edited = false;
            }
            DataContext = BreedingDetails;
        }

        async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Close();

            if (!edited)
            {
                await bHelper.AddBreedingDetails(
                    InitialWaterLevelTextBox.Text,
                    TemperatureTextBox.Text,
                    NitrogenAmountTextBox.Text,
                    FishLaunchDateTextBox.Text,
                    FishVolumeTextBox.Text
                    );
            }
            else if (edited)
            {
                await bHelper.UpdateBreedingDetails(
                    BreedingDetails.ID,
                    InitialWaterLevelTextBox.Text,
                    TemperatureTextBox.Text,
                    NitrogenAmountTextBox.Text,
                    FishLaunchDateTextBox.Text,
                    FishVolumeTextBox.Text
                    );
            }
        }

        async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Close();
            await bHelper.DeleteBreedingDelails(BreedingDetails.ID);
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
