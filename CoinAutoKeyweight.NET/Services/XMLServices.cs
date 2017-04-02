using CoinAutoKeyweight.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CoinAutoKeyweight.NET.Services
{
    public enum SourceChanged
    {
        Profile, Settings
    }
    public class XmlServices
    {
        private const string XMLPath = @"./Configs/Configuration.xml";
        public static Dictionary<string, object> Load()
        {
            try
            {
                XDocument document = XDocument.Load(XMLPath);
                Dictionary<string, object> extractedValueDic = new Dictionary<string, object>();
                // Load profile
                var profiles = document.Element("Config").Elements("Profile");
                List<Profile> profileList = new List<Profile>();
                // add profile
                if(profiles != null)
                {
                    if(profiles != null && profiles.Count() > 0)
                    {
                        foreach (var profile in profiles)
                        {
                            Profile myProfile = new Profile();
                            myProfile.Name = profile.Attribute("Name").Value; // profile name
                            myProfile.ActionKeys = profile.Element("ActionKey") != null ? BuildAssignedKey(profile.Element("ActionKey").Elements()) : new List<AssignedKey>();
                            myProfile.BuffKeys = profile.Element("BuffKey") != null ? BuildAssignedKey(profile.Element("BuffKey").Elements()) : new List<AssignedKey>();
                            // buff
                            var buffElement = profile.Element("Buff");
                            if(buffElement != null)
                            {
                                myProfile.Buff.AutoBuff = buffElement.Attribute("AutoBuff").GetValue<bool>();
                                myProfile.Buff.StartIn = buffElement.Attribute("StartIn").GetValue<int>();
                                myProfile.Buff.NextIn = buffElement.Attribute("NextIn").GetValue<int>();
                            }
                            
                            profileList.Add(myProfile);
                        }
                    }
                }

                extractedValueDic.Add("Profile", profileList);
                extractedValueDic.Add("CurrentProfileName", document.Element("Config").Element("CurrentProfile").Value);
                // load setting
                var settings = document.Element("Config").Element("Settings");
                Settings _settings = new Settings();
                _settings.AssignedWindowName = settings.Element("AssignedActiveWindow")?.Value;
                _settings.AssignedWindowHandle = settings.Element("AssignedActiveWindowHandle")?.Value;
                _settings.IsSnapping = (bool)Convert.ChangeType(settings.Element("IsSnapping")?.Value, typeof(bool));
                extractedValueDic.Add("Settings", _settings);

                return extractedValueDic;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void Save(Dictionary<string, object> configs, SourceChanged sourceChanged)
        {
            try
            {
                XDocument document = XDocument.Load(XMLPath);
                // Save Profile
                if(sourceChanged == SourceChanged.Profile)
                {
                    var profiles = configs["Profile"] as List<Profile>;
                    List<XElement> profileElements = new List<XElement>();
                    // remove profile node
                    document.Element("Config").Elements("Profile").Remove();
                    foreach (var profile in profiles)
                    {
                        XElement profileElement = new XElement("Profile");
                        XElement actionKeyElement = new XElement("ActionKey");
                        XElement buffKeyElement = new XElement("BuffKey");
                        XElement buffElement = new XElement("Buff");
                        // set profile name
                        profileElement.SetAttributeValue("Name", profile.Name);
                        // action Key
                        actionKeyElement.Add(BuildElements(profile.ActionKeys));
                        // buff key
                        buffKeyElement.Add(BuildElements(profile.BuffKeys));
                        // buff detail
                        buffElement.SetAttributeValue("AutoBuff", profile.Buff.AutoBuff);
                        buffElement.SetAttributeValue("StartIn", profile.Buff.StartIn);
                        buffElement.SetAttributeValue("NextIn", profile.Buff.NextIn);

                        profileElement.Add(actionKeyElement);
                        profileElement.Add(buffKeyElement);
                        profileElement.Add(buffElement);
                        profileElements.Add(profileElement);
                    }

                    document.Element("Config").Add(profileElements);
                    XElement currentProfile = document.Element("Config").Element("CurrentProfile");
                    if(currentProfile != null)
                    {
                        currentProfile.SetValue(configs["CurrentProfileName"]);
                    }
                    else
                    {
                        document.Element("Config").Add(new XElement("CurrentProfile", configs["CurrentProfileName"]));
                    }
                    
                }
                else if(sourceChanged == SourceChanged.Settings)
                {
                    var settings = (Settings)configs["Settings"];
                    if (settings != null)
                    {
                        document.Element("Config").Elements("Settings").Remove();
                        XElement settingElm = new XElement("Settings");
                        settingElm.Add(new XElement("AssignedActiveWindow", settings.AssignedWindowName));
                        settingElm.Add(new XElement("AssignedActiveWindowHandle", settings.AssignedWindowHandle));
                        settingElm.Add(new XElement("IsSnapping", settings.IsSnapping));
                        document.Element("Config").Add(settingElm);
                    }
                }

                document.Save(XMLPath);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static List<XElement> BuildElements(List<AssignedKey> assignedKeys)
        {
            List<XElement> xKeys = new List<XElement>();
            foreach (var actionKey in assignedKeys)
            {
                var k = new XElement("Key", actionKey.Key);
                k.SetAttributeValue("Duration", actionKey.Duration);
                k.SetAttributeValue("Delay", actionKey.Delay);
                xKeys.Add(k);
            }
            return xKeys;
        }

        private static List<AssignedKey> BuildAssignedKey(IEnumerable<XElement> elements)
        {
            List<AssignedKey> actionKeys = new List<AssignedKey>();
            foreach (var akey in elements)
            {
                AssignedKey ak = new AssignedKey()
                {
                    Duration = akey.Attribute("Duration").GetValue<double>(),
                    Delay = akey.Attribute("Delay").GetValue<double>(),
                    Key = akey.Value,
                };
                actionKeys.Add(ak);
            }
            return actionKeys;
        }
    }

    public static class XmlHelper
    {
        public static T GetValue<T>(this XAttribute attr)
        {
            if(attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                return (T)Convert.ChangeType(attr.Value, typeof(T));
            }
            return default(T);
        }


    }
}
