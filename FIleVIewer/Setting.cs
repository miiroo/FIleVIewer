using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FIleVIewer
{
    [Serializable, XmlRoot(Namespace = "http://MyCompany.com")]
    class Setting
    {
        [XmlAttribute]
        public string Extension { get; set; }
        [XmlAttribute]
        public string AssamblyName { get; set; }
        [XmlAttribute]
        public string AssamblyPath { get; set; }

        public static string SettingFileName = "pluginsList.xml";
        public Setting() { }

        public static List<Setting> readFromFile() {
            if (File.Exists(SettingFileName)) {
                List<Setting> settings = new List<Setting>();
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Setting>));
                using (Stream fStream = new FileStream(SettingFileName, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    settings = (List<Setting>)xmlFormat.Deserialize(fStream);
                }
                return settings;
            }
            else return null;
        }

        public static void addToFile(object SettingList) {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Setting>));
            using (Stream fStream = new FileStream(SettingFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
                xmlFormat.Serialize(fStream, SettingList);
            }
        }
    }
}
