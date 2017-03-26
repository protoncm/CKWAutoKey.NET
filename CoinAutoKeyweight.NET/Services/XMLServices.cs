using CoinAutoKeyweight.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoinAutoKeyweight.NET.Services
{
    public class XmlServices
    {
        private const string XMLPath = @"./Configs/Configuration.xml";
        public static Dictionary<string, object> Load()
        {
            try
            {
                XDocument document = XDocument.Load(XMLPath);
                Dictionary<string, object> extractedValueDic = new Dictionary<string, object>();
                var profiles = document.Element("Config").Elements("Profile");
                List<Profile> profileList = new List<Profile>();
                // add profile
                if(profiles != null)
                {
                    if(profiles != null && profiles.Count() > 0)
                    {
                        Profile myProfile = new Profile();
                        foreach (var profile in profiles)
                        {
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
                            
                            myProfile.AssignedWindowName = profile.Element("AssignedActiveWindow")?.Value;
                            myProfile.AssignedWindowHandle = profile.Element("AssignedActiveWindowHandle")?.Value;
                            myProfile.IsSnapping = Convert.ToBoolean(profile.Element("IsSnapping")?.Value);
                        }
                        profileList.Add(myProfile);
                    }
                }

                extractedValueDic.Add("Profile", profileList);
                extractedValueDic.Add("CurrentProfileName", document.Element("Config").Element("CurrentProfile").Value);
                return extractedValueDic;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void Save(Dictionary<string, object> configs)
        {
            try
            {
                XDocument document = XDocument.Load(XMLPath);
                // save keys
                var profiles = configs["Profile"] as List<Profile>;
                List<XElement> profileElements = new List<XElement>();
                // remove profile node
                document.Element("Config").RemoveNodes();
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
                    profileElement.Add(new XElement("AssignedActiveWindow", profile.AssignedWindowName));
                    profileElement.Add(new XElement("AssignedActiveWindowHandle", profile.AssignedWindowHandle));
                    profileElement.Add(new XElement("IsSnapping", profile.IsSnapping));
                    profileElements.Add(profileElement);
                }

                document.Element("Config").Add(profileElements);
                document.Element("Config").Add(new XElement("CurrentProfile", configs["CurrentProfileName"]));
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
