using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;
using Geocoding;
using Geocoding.Microsoft;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Overlays;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml;
using WpfApp2.Helpers;
using WpfApp2.Models;

namespace WpfApp2.MainFremaPage
{
    /// <summary>
    /// Interaction logic for DeliveryPage.xaml
    /// </summary>
    public partial class DeliveryPage : Page
    {
        #region Private Properties

        private string BingMapsKey = "AttsGkqIHCOIEA11KtQZDphl5bi8lppin64jeg-ZOOhiS4cdHA_EXJwHSbyZi4Xo";

        private string SessionKey;

        private Regex CoordinateRx = new Regex(@"^[\s\r\n\t]*(-?[0-9]{0,2}(\.[0-9]*)?)[\s\t]*,[\s\t]*(-?[0-9]{0,3}(\.[0-9]*)?)[\s\r\n\t]*$");

        #endregion
        public DeliveryPage()
        {
            InitializeComponent();

            MyMap.CredentialsProvider = new ApplicationIdCredentialsProvider(BingMapsKey);
            MyMap.CredentialsProvider.GetCredentials((c) =>
            {
                SessionKey = c.ApplicationId;
            });

            InputTbx.Text = "Київ\r\nНовоукраїнка, вул. Леніна";
           
        }

        async void CalculateRouteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            MyMap.Children.Clear();
            OutputTbx.Text = string.Empty;
            LoadingBar.Visibility = Visibility.Visible;

            var waypoints = GetWaypoints();
            if(waypoints.Count < 2)
            {
                MessageBox.Show("Need a minimum of 2 waypoints to calculate a route.");
                return;
            }

            //var travelMode = (TravelModeType)Enum.Parse(typeof(TravelModeType), (string)(TravelModeTypeCbx.SelectedItem as ComboBoxItem).Content);
            //var tspOptimization = (TspOptimizationType)Enum.Parse(typeof(TspOptimizationType), (string)(TspOptimizationTypeCbx.SelectedItem as ComboBoxItem).Tag);
            var travelMode = TravelModeType.Driving;
            var tspOptimization = TspOptimizationType.StraightLineDistance;

            try
            {
                //Calculate a route between the waypoints so we can draw the path on the map. 
                var routeRequest = new RouteRequest()
                {
                    Waypoints = waypoints,

                    //Specify that we want the route to be optimized
                    WaypointOptimization = tspOptimization,

                    RouteOptions = new RouteOptions()
                    {
                        TravelMode = travelMode,
                        RouteAttributes = new List<RouteAttributeType>()
                        {
                            RouteAttributeType.RoutePath,
                            RouteAttributeType.ExcludeItinerary
                        }
                    },
                    //When straight line distances are used, the distance matrix API is not used, so a session key can be used.
                    BingMapsKey = (tspOptimization == TspOptimizationType.StraightLineDistance) ? SessionKey : BingMapsKey
                };

                //Only use traffic based routing when travel mode is driving.
                if(routeRequest.RouteOptions.TravelMode != TravelModeType.Driving)
                {
                    routeRequest.RouteOptions.Optimize = RouteOptimizationType.Time;
                }
                else
                {
                    routeRequest.RouteOptions.Optimize = RouteOptimizationType.TimeWithTraffic;
                }

                var r = await routeRequest.Execute();
               
                RenderRouteResponse(routeRequest, r);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }

            LoadingBar.Visibility = Visibility.Collapsed;
        }

        private void RenderRouteResponse(RouteRequest routeRequest, Response response)
        {
            //Render the route on the map.
            if (response != null && response.ResourceSets != null && response.ResourceSets.Length > 0 &&
               response.ResourceSets[0].Resources != null && response.ResourceSets[0].Resources.Length > 0
               && response.ResourceSets[0].Resources[0] is Route)
            {
                var route = response.ResourceSets[0].Resources[0] as Route;
                var timeSpan = new TimeSpan(0, 0, (int)Math.Round(route.TravelDurationTraffic) / 2);
                var distance = route.TravelDistance / 2;
                
                if (timeSpan.Days > 0)
                {
                    if (VehiclesComboBox.SelectedItem != null && FuelPriceTextBox.Text != null)
                    {
                        var selectedVehicle = VehiclesComboBox.SelectedItem as Vehicles;
                        int wayCost = (int)( Convert.ToDouble(selectedVehicle.FuelConsumption) * distance / 100 * Convert.ToDouble(FuelPriceTextBox.Text));

                        OutputTbx.Text = string.Format("Travel Time: {3} days {0} hr {1} min {2} sec\r\nKm:{4}\r\nWay cost: {5}(hrn)"
                            , timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Days, distance, wayCost);
                    }
                    else
                    {
                        OutputTbx.Text = string.Format("Travel Time: {3} days {0} hr {1} min {2} sec\r\nKm:{4}"
                            , timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Days, distance);
                    }
                }
                else
                {
                    if (VehiclesComboBox.SelectedItem != null && FuelPriceTextBox.Text != null)
                    {
                        var selectedVehicle = VehiclesComboBox.SelectedItem as Vehicles;
                        int wayCost = (int)(Convert.ToDouble(selectedVehicle.FuelConsumption) * distance / 100 * Convert.ToDouble(FuelPriceTextBox.Text));

                        OutputTbx.Text = string.Format("Travel Time: {0} hr {1} min {2} sec\r\nKm:{3}\r\nWay cost: {4}(hrn)"
                            , timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, distance, wayCost);
                    }
                    else
                    {
                        OutputTbx.Text = string.Format("Travel Time: {3} days {0} hr {1} min {2} sec\r\nKm: {4}"
                            , timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Days, distance);
                    }

                }

                var routeLine = route.RoutePath.Line.Coordinates;
                var routePath = new LocationCollection();

                for (int i = 0; i < routeLine.Length; i++)
                {
                    routePath.Add(new Microsoft.Maps.MapControl.WPF.Location(routeLine[i][0], routeLine[i][1]));
                }

                var routePolyline = new MapPolyline()
                {
                    Locations = routePath,
                    Stroke = new SolidColorBrush(Colors.DarkRed),
                    StrokeThickness = 3,
                    Opacity = 0.6
                   
                };

                MyMap.Children.Add(routePolyline);

                var locs = new List<Microsoft.Maps.MapControl.WPF.Location>();

                //Create pushpins for the optimized waypoints.
                //The waypoints in the request were optimized for us.
                for (var i = 0; i < routeRequest.Waypoints.Count; i++)
                {
                    var loc = new Microsoft.Maps.MapControl.WPF.Location(routeRequest.Waypoints[i].Coordinate.Latitude, routeRequest.Waypoints[i].Coordinate.Longitude);

                    //Only render the last waypoint when it is not a round trip.
                    if (i < routeRequest.Waypoints.Count - 1)
                    {
                        MyMap.Children.Add(new Pushpin()
                        {
                            Location = loc,
                            Content = i
                        });
                    }

                    locs.Add(loc);
                }

                MyMap.SetView(locs, new Thickness(50), 0);
            }
            else if (response != null && response.ErrorDetails != null && response.ErrorDetails.Length > 0)
            {
                throw new Exception(String.Join("", response.ErrorDetails));
            }
        }

        private List<SimpleWaypoint> GetWaypoints()
        {
            var places = InputTbx.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var waypoints = new List<SimpleWaypoint>();

            foreach (var p in places)
            {
                if (!string.IsNullOrWhiteSpace(p))
                {
                    var m = CoordinateRx.Match(p);

                    if (m.Success)
                    {
                        waypoints.Add(new SimpleWaypoint(double.Parse(m.Groups[1].Value), double.Parse(m.Groups[3].Value)));
                    }
                    else
                    {
                        waypoints.Add(new SimpleWaypoint(p));
                    }
                }
            }

            return waypoints;
        }

        //async void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    VehiclesHelper vh = new VehiclesHelper();
        //    var vehiclesList = await vh.GetAllVehiclesAsync();
        //    VehiclesComboBox.ItemsSource = vehiclesList;
        //}
    }
}
