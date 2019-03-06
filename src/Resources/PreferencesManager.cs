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
        public XElement PreferenceBranch { get; set; }
        public XElement DefaultPreferenceBranch { get; set; }

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
                PreferenceBranch = XElement.Load(value);
            }
        }

        // Getters
        public bool GetBoolPref(string name)
        {
            IEnumerable<XElement> element =
                from el in PreferenceBranch.Elements("bool")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, value {0}", node.Value, name));
#endif
            return Boolean.Parse(node.Value);
        }
        public int GetIntPref(string name)
        {
            IEnumerable<XElement> element =
                from el in PreferenceBranch.Elements("int")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, value {0}", node.Value, name));
#endif
            return Int32.Parse(node.Value);
        }
        public string GetCharPref(string name)
        {
            IEnumerable<XElement> element =
                from el in PreferenceBranch.Elements("string")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, value {0}", node.Value, name));
#endif
            return node.Value;
        }

        // Setters
        public bool SetBoolPref(string name, bool value)
        {
            IEnumerable<XElement> element =
                from el in PreferenceBranch.Elements("bool")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            if (node == null)
                return false;
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, old value {0}, new value {2}", node.Value, name, value));
#endif
            node.Value = value.ToString();
            return true;
        }
        public bool SetIntPref(string name, int value)
        {
            IEnumerable<XElement> element =
                from el in PreferenceBranch.Elements("int")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            if (node == null)
                return false;
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, old value {0}, new value {2}", node.Value, name, value));
#endif
            node.Value = value.ToString();
            return true;
        }
        public bool SetCharPref(string name, string value)
        {
            IEnumerable<XElement> element =
                from el in PreferenceBranch.Elements("string")
                where (string)el.Attribute("id") == name
                select el;
            XElement node = element.ElementAtOrDefault(0);
            if (node == null)
                return false;
#if DEBUG
            Console.WriteLine(String.Format("Preferences Manager: name {1}, old value {0}, new value {2}", node.Value, name, value));
#endif
            node.Value = value;
            return true;
        }

        // Misc
        public bool PrefHasUserValue(string name)
        {
            throw new NotImplementedException();
        }
        public bool ClearUserPref(string name)
        {
            throw new NotImplementedException();
        }

        public void Save(bool createDefault = false)
        {
            if (createDefault)
            {
                DefaultPreferenceBranch.Save(filename);
                return;
            }
            PreferenceBranch.Save(filename);
        }
    }
}
