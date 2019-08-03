using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace Maquina
{
    public class PreferencesManager
    {
        public PreferencesManager()
        {
            DefaultPreferences = new XElement("preferences");
            Filename = Global.PreferencesXml;
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
                try
                {
                    Preferences = XElement.Load(value);
                }
                catch (Exception e)
                {
#if LOG_ENABLED
                    LogManager.Error(0, string.Format("XML error: {0}", e.Message));
#endif
                    Preferences = DefaultPreferences;
                }
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
#if LOG_ENABLED
            LogManager.Info(0, string.Format("Get Bool Pref - Name: {1}, Value: {0}", value, name));
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
#if LOG_ENABLED
            LogManager.Info(0, string.Format("Get Int Pref - Name: {1}, Value: {0}", value, name));
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
#if LOG_ENABLED
            LogManager.Info(0, string.Format("Get Char Pref - Name: {1}, Value: {0}", value, name));
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
#if LOG_ENABLED
                LogManager.Info(0, string.Format("Set Bool Pref - Name: {1}, Old value: {0}, New value: {2}",
                    node.Value, name, value));
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
#if LOG_ENABLED
                LogManager.Info(0, string.Format("Set Int Pref - Name: {1}, Old value: {0}, New value: {2}",
                    node.Value, name, value));
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
#if LOG_ENABLED
                LogManager.Info(0, string.Format("Set Char Pref - Name: {1}, Old value: {0}, New value: {2}",
                    node.Value, name, value));
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
#if LOG_ENABLED
            LogManager.Info(0, string.Format("New Pref - Name: {0}, Type: {1}, Value: {2}",
                name, type, value));
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
