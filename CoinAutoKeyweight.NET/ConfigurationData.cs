using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinAutoKeyweight.NET
{
    public class ConfigurationData : PropertyChanges
    {
        private string _assignedKey = "Del";
        private string _assignedWindow = string.Empty;
        private bool _isSnapping = true;
        private bool _needInitialConfig = false;
        private float _holdTime = 5;
        public string AssignedWindowHandle { get; set; }
        public string AssignedKeyCode { get; set; }
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

        public bool IsSnapping
        {
            get { return _isSnapping; }
            set
            {
                _isSnapping = value;
                OnPropertyChanged("IsSnapping");
            }
        }

        public float HoldTime
        {
            get { return _holdTime; }
            set
            {
                _holdTime = value;
                OnPropertyChanged("HoldTime");
            }
        }

        public ConfigurationData(Dictionary<string, object> loadedConfig)
        {
            if(loadedConfig != null)
            {
                _assignedKey = (string)loadedConfig["AssignedKey"];
                _assignedWindow = (string)loadedConfig["AssignedActiveWindow"];
                AssignedWindowHandle = (string)loadedConfig["AssignedActiveWindowHandle"];
                AssignedKeyCode = (string)loadedConfig["AssignedKeyCode"];
                _isSnapping = bool.Parse(loadedConfig["IsSnapping"].ToString());
                _holdTime = float.Parse(loadedConfig["HoldTime"].ToString());
                NeedInitialConfig = true;
            }
        }

        public Dictionary<string, object> GetDataDic()
        {
            Dictionary<string, object> extractedValueDic = new Dictionary<string, object>();
            extractedValueDic.Add("AssignedKey", _assignedKey);
            extractedValueDic.Add("AssignedKeyCode", AssignedKeyCode);
            extractedValueDic.Add("AssignedActiveWindow", _assignedWindow);
            extractedValueDic.Add("AssignedActiveWindowHandle", AssignedWindowHandle);
            extractedValueDic.Add("IsSnapping", _isSnapping);
            extractedValueDic.Add("HoldTime", _holdTime);
            return extractedValueDic;
        }

    }
}
