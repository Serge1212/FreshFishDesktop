using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace WpfApp2.Models
{
    public class Vehicles : BaseViewModel, IDataErrorInfo
    {
        private string _model;
        private string _mark;
        private string _manufactureDate;
        private string _mileage;
        private string _fuelConsumption;

        public string Error
        {
            get => null;
        }
    
        public Dictionary<string, string> VehiclesErrorsCollection = new Dictionary<string, string>();

        public string this[string columnName]
        { 
            get
            {
                string result = null;
                Regex digitAndDotRegex = new Regex(@"^[0-9.]+$");//дозволяє тільки цифри і крапку
                switch(columnName)
                {
                    case "_model":
                        if(string.IsNullOrEmpty(_model))
                        {
                            result = "Vehicle's model cannot be empty";
                        }
                        break;
                    case "_mark":
                        if (string.IsNullOrEmpty(_mark))
                        {
                            result = "Vehicle's mark cannot be empty";
                        }
                       break;
                    case "_manufactureDate":
                        if (string.IsNullOrEmpty(_manufactureDate))
                        {
                            result = "The date of manufacture cannot be empty";
                        }
                        break;
                    case "_mileage":
                        if (string.IsNullOrEmpty(_mileage))
                        {
                            result = "The vehicle's mileage cannot be empty";
                        }
                        else if (!digitAndDotRegex.IsMatch(_mileage))
                        {
                            result = "The vehicle's mileage accepts only numbers";
                        }

                        break;
                    case "_fuelConsumption":
                        if (string.IsNullOrEmpty(_fuelConsumption))
                        {
                            result = "The vehicle's fuel consumption cannot be empty";
                        }
                        else if (!digitAndDotRegex.IsMatch(_fuelConsumption))
                        {
                            result = "The vehicle's fuel consumption accepts only numbers";
                        }

                        break;
                }
                if (VehiclesErrorsCollection.ContainsKey(columnName))
                {
                    VehiclesErrorsCollection[columnName] = result;
                }
                else if (result != null)
                    VehiclesErrorsCollection.Add(columnName, result);

                OnPropertyChanged("VehiclesErrorsCollection");

                return result == null ? string.Empty : "!";
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged("Model");
            }
            
        }
        public string Mark
        {
            get => _mark;
            set
            {
                _mark = value;
                OnPropertyChanged("Mark");
            }

        }
        public string ManufactureDate
        {
            get => _manufactureDate;
            set
            {
                _manufactureDate = value;
                OnPropertyChanged("ManufactureDate");
            }

        }
        public string Mileage
        {
            get => _mileage;
            set
            {
                _mileage = value;
                OnPropertyChanged("Mileage");
            }

        }
        public string FuelConsumption
        {
            get => _fuelConsumption;
            set
            {
                _fuelConsumption = value;
                OnPropertyChanged("FuelConsumption");
            }

        }

    }
}
