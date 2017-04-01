using System.Collections.Generic;

namespace CoinAutoKeyweight.NET.Models
{
    public class Profile : PropertyChanges
    {
        private string _name = string.Empty;
        private List<AssignedKey> _actionKeys = new List<AssignedKey>();
        private List<AssignedKey> _buffKeys = new List<AssignedKey>();
        private Buff _buff = new Buff();
        private bool _isSelected = false;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
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
    }
}
