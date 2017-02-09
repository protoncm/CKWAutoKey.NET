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
                extractedValueDic.Add("AssignedKey", document.Element("Config").Element("AssignedKey").Value);
                extractedValueDic.Add("AssignedKeyCode", document.Element("Config").Element("AssignedKeyCode").Value);
                extractedValueDic.Add("AssignedActiveWindow", document.Element("Config").Element("AssignedActiveWindow").Value);
                extractedValueDic.Add("AssignedActiveWindowHandle", document.Element("Config").Element("AssignedActiveWindowHandle").Value);
                extractedValueDic.Add("IsSnapping", document.Element("Config").Element("IsSnapping").Value);
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
                document.Element("Config").Element("AssignedKey").SetValue(configs["AssignedKey"]);
                document.Element("Config").Element("AssignedKeyCode").SetValue(configs["AssignedKeyCode"]);
                document.Element("Config").Element("AssignedActiveWindow").SetValue(configs["AssignedActiveWindow"]);
                document.Element("Config").Element("AssignedActiveWindowHandle").SetValue(configs["AssignedActiveWindowHandle"]);
                document.Element("Config").Element("IsSnapping").SetValue(configs["IsSnapping"]);
                document.Save(XMLPath);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
