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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            
            ListViewMenu.SelectedIndex = 0;
        }   

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

       

        private void Closebutton1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ListViewMenu_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            switch (index)
            {
                case 0:
                    SettingsFrame.Content = 0;
                    SettingsFrame.Content = new Settings_frame.Languege();
                    break;
                default:
                    break;
            }
     
        }
    }
}
