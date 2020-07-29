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

namespace WpfApp2.Windows
{
    /// <summary>
    /// Interaction logic for ProgressBarWindow.xaml
    /// </summary>
    public partial class ProgressBarWindow : Window
    {
        public ProgressBarWindow()
        {
            InitializeComponent();
            matrhe1();

        }

        private void matrhe1()
        {
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(15);
            int maxvalue = 288;

            timer.Tick += (ss, ee) =>
            {
                int match = 0;

                if (progr1.Width < maxvalue)
                {
                    SKfish.Margin = new Thickness(progr1.Width, 58, 5, 25);
                    SKfish.Width = 340 - progr1.Width;

                    lable12.Content = (progr1.Width / 3) + "%";
                    progr1.Width += 3;
                }
                else match++;
                if (match == 1)
                {
                    timer.Stop();
                    lable12.Content = 100 + "%";
                }

            };
            timer.Start();

        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
