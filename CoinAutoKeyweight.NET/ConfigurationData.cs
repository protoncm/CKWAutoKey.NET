using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinAutoKeyweight.NET
{
    public class ConfigurationData : PropertyChanges
    {
        private string _assignedKey = "Del";
        private string _assignedWindow = string.Empty;
        private bool? _isSnapping = true;
        private bool _needInitialConfig = false;

        public bool NeedInitialConfig
        {
            get
            {
                return _needInitialConfig;
            }
            set
            {
                _needInitialConfig = value;
                OnPropertyChanged("NeedInitialConfig");
            }
        }
        public string AssignedKey
        {
            get { return _assignedKey; }
            set
            {
                _assignedKey = value;
                OnPropertyChanged("AssignedKey");
            }
        }

        public string AssignedActiveWindow
        {
            get { return _assignedWindow; }
            set
            {
                _assignedWindow = value;
                OnPropertyChanged("AssignedActiveWindow");
            }
        }

        public bool? IsSnapping
        {
            get { return _isSnapping; }
            set
            {
                _isSnapping = value;
                OnPropertyChanged("IsSnapping");
            }
        }

        public ConfigurationData(Table loadedConfig)
        {
            if(loadedConfig != null)
            {
                _assignedKey = loadedConfig.AssignedKey;
                _assignedWindow = loadedConfig.AssignedActiveWindow;
                _isSnapping = loadedConfig.IsSnapping;
                NeedInitialConfig = true;
            }
        }

    }
}
