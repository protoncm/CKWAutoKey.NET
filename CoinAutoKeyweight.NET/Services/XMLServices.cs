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
                var assignedKeyNode = document.Element("Config").Element("AssignedKey");
                List<AssignedKey> assignedKeys = new List<AssignedKey>();
                if(assignedKeyNode != null)
                {
                    var keys = assignedKeyNode.Elements();
                    if(keys != null && keys.Count() > 0)
                    {
                        int index = 1;
                        foreach (var key in keys)
                        {
                            AssignedKey ak = new AssignedKey()
                            {
                                Duration = key.Attribute("Duration").ToInt(),
                                Key = key.Value,
                                Order = index++
                            };
                            assignedKeys.Add(ak);
                        }
                    }
                }
                extractedValueDic.Add("AssignedKey", assignedKeys);
                extractedValueDic.Add("AssignedKeyCode", document.Element("Config").Element("AssignedKeyCode").Value);
                extractedValueDic.Add("AssignedActiveWindow", document.Element("Config").Element("AssignedActiveWindow").Value);
                extractedValueDic.Add("AssignedActiveWindowHandle", document.Element("Config").Element("AssignedActiveWindowHandle").Value);
                extractedValueDic.Add("IsSnapping", document.Element("Config").Element("IsSnapping").Value);
                extractedValueDic.Add("HoldTime", document.Element("Config").Element("HoldTime").Value);
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
                var keys = configs["AssignedKey"] as List<AssignedKey>;
                document.Element("Config").Element("AssignedKey").RemoveNodes();
                if (keys != null && keys.Count > 0)
                {
                    List<XElement> xKeys = new List<XElement>();
                    foreach(var key in keys)
                    {
                        var k = new XElement("Key", key.Key);
                        k.SetAttributeValue("Duration", key.Duration);
                        xKeys.Add(k);
                    }

                    document.Element("Config").Element("AssignedKey").Add(xKeys);
                }
                document.Element("Config").Element("AssignedKeyCode").SetValue(configs["AssignedKeyCode"]);
                document.Element("Config").Element("AssignedActiveWindow").SetValue(configs["AssignedActiveWindow"]);
                document.Element("Config").Element("AssignedActiveWindowHandle").SetValue(configs["AssignedActiveWindowHandle"]);
                document.Element("Config").Element("IsSnapping").SetValue(configs["IsSnapping"]);
                document.Element("Config").Element("HoldTime").SetValue(configs["HoldTime"]);
                document.Save(XMLPath);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public static class XmlHelper
    {
        public static int ToInt(this XAttribute attr)
        {
            if(attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                int result = 0;
                if(int.TryParse(attr.Value, out result))
                {
                    return result;
                }
            }
            return 0;
        }
    }
}
