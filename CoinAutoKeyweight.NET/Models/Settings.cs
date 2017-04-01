using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinAutoKeyweight.NET.Models
{
    public class Settings : PropertyChanges
    {
        private bool _isSnapping = true;
        private string _assignedWindowName = "MapleStory";
        private string _assignedWindowHandle = "MapleStory";
        public bool IsSnapping
        {
            get { return _isSnapping; }
            set
            {
                _isSnapping = value;
                OnPropertyChanged("IsSnapping");
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
    }
}
