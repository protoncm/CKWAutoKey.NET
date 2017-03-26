using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinAutoKeyweight.NET.Models
{
    public class AssignedKey : PropertyChanges
    {
        private string _key = string.Empty;
        private double _duration = 10; // default is 10 sec.
        private int _order = 0;
        private bool _isChecked = false;
        private double _delay = 0.1;
        public double Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                OnPropertyChanged("Delay");
            }
        }
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }
        public double Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public int Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged("Order");
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public static AssignedKey Default
        {
            get
            {
                return new AssignedKey() { Key = "-", Duration = 0 };
            }
        }
    }
}
