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
using WpfApp2.Models;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for WorckerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        bool edited = true;
        WorkersHelper workersHelper = new WorkersHelper();
        Workers Worker = new Workers();
        public WorkerWindow(Workers worker = null)
        {
            InitializeComponent();

            Worker = worker;
            if (worker == null)
            {
                deleteButtonStackPanel.Visibility = Visibility.Hidden;
                Worker = new Workers();
                edited = false;
            }
            DataContext = Worker;
        }


        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
                if (edited == false)
                {
                    await workersHelper.AddWorker(
                        workerNameTextBox.Text,
                        workerSurnameTextBox.Text,
                        workerPatronymicTextBox.Text,
                        workerPositionTextBox.Text,
                        workerSalaryTextBox.Text,
                        workerPhonenumberTextBox.Text,
                        workerAddressTextBox.Text,
                        workerAdditionaInfoTextBox.Text);
                }
                else if (edited == true)
                {
                    await workersHelper.UpdateWorker(
                        Worker.w_id,
                        workerNameTextBox.Text,
                        workerSurnameTextBox.Text,
                        workerPatronymicTextBox.Text,
                        workerPositionTextBox.Text,
                        workerSalaryTextBox.Text,
                        workerPhonenumberTextBox.Text,
                        workerAddressTextBox.Text,
                        workerAdditionaInfoTextBox.Text
                        );

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_close_Click(object sender, RoutedEventArgs e)
        {
            WorkerTextBoxes.BindingGroup.CancelEdit(); //не змінює дані в датагрід(локально), якщо натиснутка кнопка "Закрити". 
            //Дані не змінювалися в firebase, тільки в датагрід. При перезавантаженні дані відновлювалися.
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(tRequest);
                }

                e.Handled = true;
            }
            
        }

        private async void ButtonDeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Do you really want to delete this product?", "Delete Product", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (res)
            {
                case MessageBoxResult.Yes:
                    Close();
                    await workersHelper.DeleteWorker(Worker.w_id);
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
