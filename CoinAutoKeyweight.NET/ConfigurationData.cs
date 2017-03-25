using CoinAutoKeyweight.NET.Models;
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
        private List<AssignedKey> _assignedKeys = null;
        private AssignedKey _displayAssignedKey = null;
        private string _currentKey = string.Empty;
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
        public AssignedKey DisplayAssignedKey
        {
            get { return _displayAssignedKey; }
            set
            {
                _displayAssignedKey = value;
                OnPropertyChanged("DisplayAssignedKey");
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

        public List<AssignedKey> AssignedKeys
        {
            get { return _assignedKeys; }
            set
            {
                _assignedKeys = value;
                OnPropertyChanged("AssignedKeys");
            }
        }

        public string CurrentKey
        {
            get { return _currentKey; }
            set
            {
                _currentKey = value;
                OnPropertyChanged("CurrentKey");
            }
        }

        public ConfigurationData(Dictionary<string, object> loadedConfig)
        {
            if(loadedConfig != null)
            {
                var keyList = ((List<AssignedKey>)loadedConfig["AssignedKey"]);
                if (keyList != null && keyList.Count > 0)
                {
                    _assignedKeys = keyList;
                    _displayAssignedKey = keyList[0];
                }
                else
                {
                    _assignedKeys = new List<AssignedKey>();
                    _displayAssignedKey = AssignedKey.Default;
                }
                
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
            extractedValueDic.Add("AssignedKey", _assignedKeys);
            extractedValueDic.Add("AssignedKeyCode", AssignedKeyCode);
            extractedValueDic.Add("AssignedActiveWindow", _assignedWindow);
            extractedValueDic.Add("AssignedActiveWindowHandle", AssignedWindowHandle);
            extractedValueDic.Add("IsSnapping", _isSnapping);
            extractedValueDic.Add("HoldTime", _holdTime);
            return extractedValueDic;
        }

    }
}
