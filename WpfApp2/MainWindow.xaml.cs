using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

using System.Windows.Interop;
using WpfApp2.MainFrame;


namespace WpfApp2
{
  
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new WindowViewModel(this);
            MainFrame.Content = new MainPage();
        }
    }
}
