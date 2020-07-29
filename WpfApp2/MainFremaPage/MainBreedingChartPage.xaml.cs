using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Helpers;
using WpfApp2.MainFrame;

namespace WpfApp2.MainFremaPage
{
    /// <summary>
    /// Interaction logic for MainBreedingChartPage.xaml
    /// </summary>
    public partial class MainBreedingChartPage : Page
    {
        BreedingHelper breedingHelper = new BreedingHelper();
        public MainBreedingChartPage()
        {
            InitializeComponent();



        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        //public Func<double, string> YFormatter { get; set; }

        async void Page_Loaded(object sender, RoutedEventArgs e)
        {

            var breedingWeekslist = await breedingHelper.GetAllBreedingWeeksAsync();

            var breedingDetailsIDs = (from id in breedingWeekslist
                                      select id.BreedingDetailsID).Distinct().ToArray();

            List<double> waterLevelsList;

            Labels = (from l in breedingWeekslist
                      select l.WeekNumber).ToArray();

            SeriesCollection = new SeriesCollection();

            for (int i = 0; i < breedingDetailsIDs.Count(); i++)
            {
                waterLevelsList = (from v in breedingWeekslist
                                   where v.BreedingDetailsID == breedingDetailsIDs[i]
                                   select Convert.ToDouble(v.WaterLevel)).ToList();
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Water Level",
                    Values = waterLevelsList.AsChartValues(),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                });
            }


            DataContext = this;

        }
    }
}
