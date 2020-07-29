using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfApp2.Helpers
{
    public class BreedingWeeks : INotifyPropertyChanged
    {
        private string _id;
        private string _weekNumber;
        private string _weekDate;
        private string _waterLevel;
        private string _breedingDetailsID;

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        public string WeekNumber
        {
            get
            {
                return _weekNumber;
            }
            set
            {
                _weekNumber = value;
                OnPropertyChanged("WeekNumber");

            }
        }

        public string WeekDate
        {
            get
            {
                return _weekDate;
            }
            set
            {
                _weekDate = value;
                OnPropertyChanged("WeekDate");

            }
        }
        public string WaterLevel
        {
            get
            {
                return _waterLevel;
            }
            set
            {
                _waterLevel = value;
                OnPropertyChanged("WaterLevel");

            }
        }

        public string BreedingDetailsID
        {
            get
            {
                return _breedingDetailsID;
            }
            set
            {
                _breedingDetailsID = value;
                OnPropertyChanged("BreedingDetailsID");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
