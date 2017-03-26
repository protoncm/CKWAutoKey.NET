using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinAutoKeyweight.NET.Models
{
    public class Profile : PropertyChanges
    {
        private string _name = string.Empty;
        private List<AssignedKey> _actionKeys = new List<AssignedKey>();
        private List<AssignedKey> _buffKeys = new List<AssignedKey>();
        private Buff _buff = new Buff();
        private bool _isSnapping = true;
        private string _assignedWindowName = string.Empty;
        public string _assignedWindowHandle = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string AssignedWindowName
        {
            get { return _assignedWindowName; }
            set
            {
                _assignedWindowName = value;
                OnPropertyChanged("AssignedWindowName");
            }
        }
        public string AssignedWindowHandle
        {
            get { return _assignedWindowHandle; }
            set
            {
                _assignedWindowHandle = value;
                OnPropertyChanged("AssignedWindowHandle");
            }
        }
        public Buff Buff
        {
            get { return _buff; }
            set
            {
                _buff = value;
                OnPropertyChanged("Buff");
            }
        }
        public List<AssignedKey> ActionKeys
        {
            get { return _actionKeys; }
            set
            {
                _actionKeys = value;
                OnPropertyChanged("ActionKeys");
            }
        }

        public List<AssignedKey> BuffKeys
        {
            get { return _buffKeys; }
            set
            {
                _buffKeys = value;
                OnPropertyChanged("BuffKeys");
            }
        }

        public bool IsSnapping
        {
            get { return _isSnapping; }
            set
            {
                _isSnapping = value;
                OnPropertyChanged("IsSnapping");
            }
        }
    }
}
