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

namespace WpfApp2.MainFrame
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            MainFrame.Content = new WorkerPage1();
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            BordermenuClose.Visibility = Visibility.Visible;
            BordermenuOpen.Visibility = Visibility.Collapsed;
            MainFrame.IsEnabled = false;
        }
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            BordermenuClose.Visibility = Visibility.Collapsed;
            BordermenuOpen.Visibility = Visibility.Visible;
            MainFrame.IsEnabled = true;
        }
        private void ListViewMenu_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            switch (index)
            {
                case 0:
                    MainFrame.Content = null;
                    MainFrame.Content = new WorkerPage1();
                    break;
                case 1:
                    MainFrame.Content = null;
                    MainFrame.Content = new FishPage1();
                    break;
                case 2:
                    MainFrame.Content = null;
                    MainFrame.Content = new IncomePage1();
                    break;
                case 3:
                    MainFrame.Content = null;
                    MainFrame.Content = new DiagramsPage();
                    break;
                case 4:
                    MainFrame.Content = null;
                    MainFrame.Content = new FishBreedingPage();
                    break;
                default:
                    break;
            }

        }
    }
}
