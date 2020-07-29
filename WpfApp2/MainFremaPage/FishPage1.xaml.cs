using Firebase.Database;
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

namespace WpfApp2.MainFrame
{
    /// <summary>
    /// Interaction logic for FishPage1.xaml
    /// </summary>
    public partial class FishPage1 : Page
    {
        static bool executed = true;//булеве значення, яке перевіряє, чи був вже запущений метод відправки даних в датагрід
        static ObservableCollection<Products> ProductsCollection = new ObservableCollection<Products>();//колекція, в яку передаватимуться всі дані про продукти з серверу
        static FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");//змінна, яка налаштовує зв'язок з сервером
        ProductsHelper helper = new ProductsHelper();//екземпляр класу ProductsHelper() для маніпуляції з даними продуктів
        public FishPage1()
        {
            InitializeComponent();
            if(executed)//якщо ще не виконаний метод
            {
                GetProducts();
                executed = false;//після цього метод не буде викликатися знову
            }

            ProductsDataGrid.ItemsSource = ProductsCollection; //установка джерела даних для датагріда
        }

        void GetProducts()// метод, який передає дані з сервера в колекцію, і ця колекція буде відслідковувати зміни в реальному часі і змінювати дані в датагрід
        {
            ProductsCollection = client
            .Child("freshfish")
            .AsObservable<Products>()
            .ObserveOnDispatcher()
            .AsObservableCollection();
        }

        //обробник події кнопки "додати", який відкриває вікно для додавання нового продукту
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow p = new ProductWindow();
            p.ShowDialog();//метод, який відкриває вікно
        }

        //обробник події кнопки "видалити", який запускає метод DeleteProduct()
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteProduct();
        }
        //обробник події кнопки "редагувати", який відкриває вікно додавання, передаючи дані з серверу
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Products specificProduct = ProductsDataGrid.SelectedItem as Products;//приводимо вибраний елемент з датагріда в тип Products
            ProductWindow fw = new ProductWindow(specificProduct);
            fw.Show();
        }

        async void DeleteProduct()
        {
            MessageBoxResult res = MessageBox.Show("Do you really want to delete this product?", "Delete Product", MessageBoxButton.YesNo);//запитання для видалення продукту(так\ні)
            switch (res)
            {
                case MessageBoxResult.Yes://якщо так - видаляємо
                    if (ProductsDataGrid.SelectedItem != null)//якщо є вибраний рядок у датагрід
                    {
                        Products toDeleteProduct = ProductsDataGrid.SelectedItem as Products;//тоді приводимо вибраний рядок в Products
                        await helper.DeleteProduct(toDeleteProduct.id);//і видаляємо дані з конкретним айді продукту
                    }
                    break;
                case MessageBoxResult.No://якщо ні, то ні
                    break;
            }
        }

        //Обробник події в датагрід, який спрацьовує при зміні вибору рядка в таблиці
        private void ProductsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null) //якщо є вибраний рядок
            {//дозволимо видаляти або редагувати його
                ButtonDelete.IsEnabled = true;
                ButtonEdit.IsEnabled = true;
            }
            else
            {//інакше, не дозволимо
                ButtonDelete.IsEnabled = false;
                ButtonEdit.IsEnabled = false;
            }
        }

        //обробник зміни тексту в пошуковому рядку
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchByCombobox.SelectedIndex == 0)
            {
                var SearchedList = (from product in ProductsCollection
                                    where product.productname.ToLower().StartsWith(SearchTextBox.Text.ToLower())
                                    select product).ToList();
                ProductsDataGrid.ItemsSource = SearchedList;
            }
            if (SearchByCombobox.SelectedIndex == 1)
            {
                var SearchedList = (from product in ProductsCollection
                                    where product.price.ToLower().Contains(SearchTextBox.Text.ToLower())
                                    select product).ToList();
                ProductsDataGrid.ItemsSource = SearchedList;
            }
            if (SearchByCombobox.SelectedIndex == 2)
            {
                var SearchedList = (from product in ProductsCollection
                                    where product.date.ToLower().StartsWith(SearchTextBox.Text.ToLower())
                                    select product).ToList();
                ProductsDataGrid.ItemsSource = SearchedList;
            }
            if (SearchByCombobox.SelectedIndex == 3)
            {
                var SearchedList = (from product in ProductsCollection
                                    where product.status.ToLower().Contains(SearchTextBox.Text.ToLower())
                                    select product).ToList();
                ProductsDataGrid.ItemsSource = SearchedList;
            }
        }

        //обробник подвійного нажаття клавіші по датагріду
        private void ProductsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var productsList = ProductsCollection;
            if (ProductsDataGrid.SelectedIndex >= 0 && ProductsDataGrid.SelectedIndex < productsList.Count)//якщо вибраний індекс елементу є більшим за 0 і, водночас, не перевищує максимальний індекс даних
            {
                Products specificProduct = ProductsDataGrid.SelectedItem as Products;//тоді приводимо вибраний рядок до даних продуктів
                ProductWindow fw = new ProductWindow(specificProduct);//передаємо в екземпляр вікна продукт
                fw.Show();//відкриваємо вікно з переданими даними
            }
        }

    }
}
