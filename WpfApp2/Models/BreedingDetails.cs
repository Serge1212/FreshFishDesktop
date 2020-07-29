using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Channels;

namespace WpfApp2.Helpers
{
    public class BreedingDetails : INotifyPropertyChanged
    {
        private string _id;
        private string _initialWaterLevel;
        private string _temperature;
        private string _nitrogenAmount;
        private string _fishLaunchDate;
        private string _fishVolume;

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        public  string InitialWaterLevel 
        {
            get { return _initialWaterLevel; }
            set
            {
                _initialWaterLevel = value;
                OnPropertyChanged("InitialWaterLevel");
            }
        }

        public string Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        public string NitrogenAmount
        {
            get { return _nitrogenAmount; }
            set
            {
                _nitrogenAmount = value;
                OnPropertyChanged("NitrogenAmount");
            }
        }

        public string FishLaunchDate
        {
            get { return _fishLaunchDate; }
            set
            {
                _fishLaunchDate = value;
                OnPropertyChanged("FishLaunchDate");
            }
        }

        public string FishVolume
        {
            get { return _fishVolume; }
            set
            {
                _fishVolume = value;
                OnPropertyChanged("FishVolume");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
