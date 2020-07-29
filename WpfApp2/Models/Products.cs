using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WpfApp2.Models
{
    public class Products : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Validation
        public string Error { get { return null; } }

        public Dictionary<string, string> ProductsErrorCollection { get; private set; } = new Dictionary<string, string>();
        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch(columnName)
                {
                    case "productname":
                        if(string.IsNullOrEmpty(productname))
                        {
                            result = "Product name cannot be empty";
                        }
                        break;
                    case "price":
                        Regex regex = new Regex(@"^[0-9.]+$");//дозволяє тільки цифри і крапку
                        //Match match;
                        if (string.IsNullOrEmpty(price))
                        {
                            result = "Price cannot be empty";
                        }
                        else if(!regex.IsMatch(price))
                        {
                            result = "Price can contain only digits and a single dot";
                        }
                        break;
                    case "status":
                        if (string.IsNullOrEmpty(status))
                        {
                            result = "Status cannot be empty";
                        }
                        break;
                }
                if (ProductsErrorCollection.ContainsKey(columnName))
                {
                    ProductsErrorCollection[columnName] = result;
                }
                else if (result != null)
                    ProductsErrorCollection.Add(columnName, result);

                OnPropertyChanged("ProductsErrorCollection");

                return result == null ? string.Empty : "!";
            }
        }
        #endregion

        public string id { get; set; }
        public string productname { get; set; }
        public string price { get; set; }
        public string date { get; set; }
        public string status { get; set; }
        public string packer { get; set; }
        public string driver { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
