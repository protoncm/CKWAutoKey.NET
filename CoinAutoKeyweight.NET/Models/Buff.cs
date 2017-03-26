using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinAutoKeyweight.NET.Models
{
    public class Buff : PropertyChanges
    {
        private bool _autoBuff = false;
        private int _startIn = 1; // min
        private int _nextIn = 1; // min
        public bool AutoBuff
        {
            get { return _autoBuff; }
            set
            {
                _autoBuff = value;
                OnPropertyChanged("AutoBuff");
            }
        }

        public int StartIn
        {
            get { return _startIn; }
            set
            {
                _startIn = value;
                OnPropertyChanged("StartIn");
            }
        }

        public int NextIn
        {
            get { return _nextIn; }
            set
            {
                _nextIn = value;
                OnPropertyChanged("NextIn");
            }
        }
    }
}
