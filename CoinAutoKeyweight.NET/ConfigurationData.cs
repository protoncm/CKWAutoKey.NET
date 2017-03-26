using CoinAutoKeyweight.NET.Commands;
using CoinAutoKeyweight.NET.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoinAutoKeyweight.NET
{
    public class ConfigurationData : PropertyChanges
    {
        private List<Profile> _profiles = new List<Profile>();
        private Profile _currentProfile = null;
        private List<AssignedKey> _assignedKeys = null;
        private AssignedKey _displayAssignedKey = null;
        private string _currentKey = string.Empty;
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
        public AssignedKey DisplayAssignedKey
        {
            get { return _displayAssignedKey; }
            set
            {
                _displayAssignedKey = value;
                OnPropertyChanged("DisplayAssignedKey");
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
        public List<Profile> Profiles
        {
            get
            {
                return _profiles;
            }
            set
            {
                _profiles = value;
                OnPropertyChanged("Profiles");
            }
        }
        public Profile CurrentProfile
        {
            get { return _currentProfile; }
            set
            {
                _currentProfile = value;
                OnPropertyChanged("CurrentProfile");
            }
        }
        public ConfigurationData(Dictionary<string, object> loadedConfig)
        {
            if(loadedConfig != null)
            {
                Profiles = (List<Profile>)loadedConfig["Profile"];
                if(Profiles != null && Profiles.Count > 0)
                {
                    string currentProFileName = (string)loadedConfig["CurrentProfileName"];
                    var currentProfile = Profiles.FirstOrDefault(a => a.Name == currentProFileName);
                    if(currentProfile != null)
                    {
                        currentProfile.IsSelected = true;
                        CurrentProfile = currentProfile;
                        _assignedKeys = currentProfile.ActionKeys;
                        if (currentProfile.ActionKeys != null && currentProfile.ActionKeys.Count > 0)
                        {
                            _displayAssignedKey = currentProfile.ActionKeys[0];
                        }
                        else
                        {
                            // create new key
                            _displayAssignedKey = AssignedKey.Default;
                        }
                    }
                    else
                    {
                        // create new profile
                    }
                }

                NeedInitialConfig = true;
                //var keyList = ((List<Pr>)loadedConfig["ActionKey"]);
                //if (keyList != null && keyList.Count > 0)
                //{
                //    _assignedKeys = keyList;
                //    _displayAssignedKey = keyList[0];
                //}
                //else
                //{
                //    _assignedKeys = new List<AssignedKey>();
                //    _displayAssignedKey = AssignedKey.Default;
                //}

                //_assignedWindow = (string)loadedConfig["AssignedActiveWindow"];
                //AssignedWindowHandle = (string)loadedConfig["AssignedActiveWindowHandle"];
                //_isSnapping = bool.Parse(loadedConfig["IsSnapping"].ToString());
                //NeedInitialConfig = true;
            }
        }

        

        public Dictionary<string, object> GetDataDic()
        {
            Dictionary<string, object> extractedValueDic = new Dictionary<string, object>();
            extractedValueDic.Add("Profile", Profiles);
            extractedValueDic.Add("CurrentProfileName", CurrentProfile.Name);
            //extractedValueDic.Add("AssignedActiveWindow", _assignedWindow);
            //extractedValueDic.Add("AssignedActiveWindowHandle", AssignedWindowHandle);
            //extractedValueDic.Add("IsSnapping", _isSnapping);
            return extractedValueDic;
        }

        public void CreateNewProfile(string newName)
        {
            Profile newProfile = new Profile() { Name = newName, IsSelected = true };
            Profiles.Add(newProfile);
            if(CurrentProfile != null)
            {
                CurrentProfile.IsSelected = false;
            }
            CurrentProfile = newProfile;
            AssignedKeys = CurrentProfile.ActionKeys;
            DisplayAssignedKey = AssignedKey.Default;
        }

        public void LoadProfile(Profile selectedProfile)
        {
            var previousProfile = CurrentProfile;
            previousProfile.IsSelected = false;
            if (selectedProfile != null)
            {
                selectedProfile.IsSelected = true;
                CurrentProfile = selectedProfile;
                _assignedKeys = selectedProfile.ActionKeys;
                if (selectedProfile.ActionKeys != null && selectedProfile.ActionKeys.Count > 0)
                {
                    _displayAssignedKey = selectedProfile.ActionKeys[0];
                }
                else
                {
                    // create new key
                    _displayAssignedKey = AssignedKey.Default;
                }
            }
        }
    }
}
