using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace Maquina.Resources
{
    public class PreferencesManager
    {
        public PreferencesManager()
        {
            DefaultPreferences = new XElement("preferences");
            Filename = Platform.PreferencesXml;
        }

        public XElement Preferences { get; set; }

        private XElement defaultPreferences;
        public XElement DefaultPreferences
        {
            get { return defaultPreferences; }
            set { defaultPreferences = value; }
        }
        
        private string filename;
        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
                if (!File.Exists(value))
                {
                    Save(true);
                }
                Preferences = XElement.Load(value);
            }
        }

        // Getters
        public bool GetBoolPref(string name, bool defaultValue = false)
        {
            IEnumerable<XElement> element =
                from el in Preferences.Elements("bool")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            bool value = defaultValue;
            if (node != null)
            {
                Boolean.TryParse(node.Value, out value);
            }
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, value {0}", value, name));
#endif
            return value;
        }
        public int GetIntPref(string name, int defaultValue = 0)
        {
            IEnumerable<XElement> element =
                from el in Preferences.Elements("int")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            int value = defaultValue;
            if (node != null)
            {
                Int32.TryParse(node.Value, out value);
            }
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, value {0}", value, name));
#endif
            return value;
        }
        public string GetCharPref(string name, string defaultValue = "")
        {
            IEnumerable<XElement> element =
                from el in Preferences.Elements("string")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            string value = defaultValue;
            if (node != null)
            {
                value = node.Value;
            }
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, value {0}", value, name));
#endif
            return value;
        }

        // Setters
        public void SetBoolPref(string name, bool value)
        {
            IEnumerable<XElement> element =
                from el in Preferences.Elements("bool")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            if (node != null)
            {
#if DEBUG
                Console.WriteLine(String.Format("Preferences Manager: name {1}, old value {0}, new value {2}", node.Value, name, value));
#endif
                node.Value = value.ToString();
            }
            else
            {
                CreateNewPref(name, "bool", value.ToString());
            }
            Save();
        }
        public void SetIntPref(string name, int value)
        {
            IEnumerable<XElement> element =
                from el in Preferences.Elements("int")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            if (node != null)
            {
#if DEBUG
                Console.WriteLine(String.Format("Preferences Manager: name {1}, old value {0}, new value {2}", node.Value, name, value));
#endif
                node.Value = value.ToString();
            }
            else
            {
                CreateNewPref(name, "int", value.ToString());
            }
            Save();
        }
        public void SetCharPref(string name, string value)
        {
            IEnumerable<XElement> element =
                from el in Preferences.Elements("string")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            if (node != null)
            {
#if DEBUG
                Console.WriteLine(String.Format("Preferences Manager: name {1}, old value {0}, new value {2}", node.Value, name, value));
#endif
                node.Value = value;
            }
            else
            {
                CreateNewPref(name, "string", value.ToString());
            }
            Save();
        }

        private void CreateNewPref(string name, string type, string value)
        {
            Preferences.Add(new XElement(type, new XAttribute("id", name), value));
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: created - name {0}, type {1}, new value {2}", name, type, value));
#endif
        }
        // Misc
        public void Save(bool createDefault = false)
        {
            if (createDefault)
            {
                DefaultPreferences.Save(filename);
                return;
            }
            Preferences.Save(filename);
        }
    }
}
