using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using WpfApp2.Helpers;
using LiveCharts.Helpers;
using WpfApp2.MainFrame;

namespace WpfApp2.MainFremaPage
{
    /// <summary>
    /// Interaction logic for BreedingWeeksChartsPage.xaml
    /// </summary>
    public partial class BreedingWeeksChartsPage : Page
    {
        BreedingHelper breedingHelper = new BreedingHelper();
        public BreedingWeeksChartsPage()
        {
            InitializeComponent();         
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        //public Func<double, string> YFormatter { get; set; }

        async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var breedingWeekslist = await breedingHelper.GetAllBreedingWeeksAsync();

            List<double> waterLevelsList = (from v in breedingWeekslist
                                            where v.BreedingDetailsID == FishBreedingPage.BreedingDetailsFromDataGridSelected.ID
                                            select Convert.ToDouble(v.WaterLevel)).ToList();

            Labels = (from l in breedingWeekslist
                      select "Week " + l.WeekNumber).ToArray();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Water Level",
                    Values = waterLevelsList.AsChartValues(),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            DataContext = this;

        }
    }
}
