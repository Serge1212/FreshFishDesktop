using Firebase.Database;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using WpfApp2.Models;
using WpfApp2.Windows;

namespace WpfApp2.MainFrame
{
    /// <summary>
    /// Interaction logic for WorkerPage1.xaml
    /// </summary>
    public partial class WorkerPage1 : Page
    {
        static ObservableCollection<Workers> WorkersCollection = new ObservableCollection<Workers>();
        WorkersHelper workersHelper = new WorkersHelper();
        static FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");
        static bool executed = true;
        public WorkerPage1()
        {
            InitializeComponent();

            if (executed)
            {
                GetWorkers();
                executed = false;
            }

            WorkersDataGrid.ItemsSource = WorkersCollection;

        }

        void GetWorkers()
        {
                WorkersCollection = client
                .Child("workers")
                .AsObservable<Workers>().ObserveOnDispatcher().AsObservableCollection();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            WorkerWindow w = new WorkerWindow();
            w.ShowDialog();
        }

        private void WorkersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var workersList = WorkersCollection;
            if (WorkersDataGrid.SelectedIndex >= 0 && WorkersDataGrid.SelectedIndex < workersList.Count)
            {
                Workers specificWorker = WorkersDataGrid.SelectedItem as Workers;
                WorkerWindow ew = new WorkerWindow(specificWorker);
                ew.ShowDialog();
            }
        }

        private void WorkersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(WorkersDataGrid.SelectedItem != null)
            {
                ButtonDelete.IsEnabled = true;
                ButtonEdit.IsEnabled = true;
            }
            else
            {
                ButtonDelete.IsEnabled = false;
                ButtonEdit.IsEnabled = false;
            }
        }


        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteWorker();
        }
        async void DeleteWorker()
        {
            MessageBoxResult res = MessageBox.Show("Do you really want to delete this product?", "Delete Product", MessageBoxButton.YesNo);
            switch (res)
            {
                case MessageBoxResult.Yes:
                    if (WorkersDataGrid.SelectedItem != null)
                    {
                        Workers toDeleteWorker = WorkersDataGrid.SelectedItem as Workers;
                        await workersHelper.DeleteWorker(toDeleteWorker.w_id);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void ButtonConvert_Click(object sender, RoutedEventArgs e)
        {
            WorkersDataGrid.SelectAllCells();
            WorkersDataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, WorkersDataGrid);
            string resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            string result = (string)Clipboard.GetData(DataFormats.Text);
            WorkersDataGrid.UnselectAllCells();
            System.IO.StreamWriter file1 = new System.IO.StreamWriter(@"D:\ExelConvertedFiles\Workers.xls");
            file1.WriteLine(result.Replace(',', ' '));
            file1.Close();

            MessageBox.Show(" Exporting DataGrid data to Excel file");
        }

        private void WorkersSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WorkersSearchComboBox.SelectedIndex == 0)
            {
                var SearchedList = (from worker in WorkersCollection
                                    where worker.name.ToLower().StartsWith(WorkersSearchTextBox.Text.ToLower())
                                    select worker).ToList();
                WorkersDataGrid.ItemsSource = SearchedList;
            }
            if (WorkersSearchComboBox.SelectedIndex == 1)
            {
                var SearchedList = (from worker in WorkersCollection
                                    where worker.surname.ToLower().StartsWith(WorkersSearchTextBox.Text.ToLower())
                                    select worker).ToList();
                WorkersDataGrid.ItemsSource = SearchedList;
            }
            if (WorkersSearchComboBox.SelectedIndex == 2)
            {
                var SearchedList = (from worker in WorkersCollection
                                    where worker.patronymic.ToLower().StartsWith(WorkersSearchTextBox.Text.ToLower())
                                    select worker).ToList();
                WorkersDataGrid.ItemsSource = SearchedList;
            }
            if (WorkersSearchComboBox.SelectedIndex == 3)
            {
                var SearchedList = (from worker in WorkersCollection
                                    where worker.position.ToLower().StartsWith(WorkersSearchTextBox.Text.ToLower())
                                    select worker).ToList();
                WorkersDataGrid.ItemsSource = SearchedList;
            }
            if (WorkersSearchComboBox.SelectedIndex == 4)
            {
                var SearchedList = (from worker in WorkersCollection
                                    where worker.salary.ToLower().StartsWith(WorkersSearchTextBox.Text.ToLower())
                                    select worker).ToList();
                WorkersDataGrid.ItemsSource = SearchedList;
            }
            if (WorkersSearchComboBox.SelectedIndex == 5)
            {
                var SearchedList = (from worker in WorkersCollection
                                    where worker.phonenumber.ToLower().StartsWith(WorkersSearchTextBox.Text.ToLower())
                                    select worker).ToList();
                WorkersDataGrid.ItemsSource = SearchedList;
            }
            if (WorkersSearchComboBox.SelectedIndex == 6)
            {
                var SearchedList = (from worker in WorkersCollection
                                    where worker.address.ToLower().StartsWith(WorkersSearchTextBox.Text.ToLower())
                                    select worker).ToList();
                WorkersDataGrid.ItemsSource = SearchedList;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Workers specificWorker = WorkersDataGrid.SelectedItem as Workers;
            WorkerWindow ww = new WorkerWindow(specificWorker);
            ww.Show();
        }
    }
}
