using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace Maquina
{
    public enum PreferenceType
    {
        String,
        Boolean,
        Integer,
        Float
    }

    public class PreferencesManager : IDisposable
    {
        public const string PreferencesXml = "preferences.xml";
        public PreferencesManager()
        {
            Filename = PreferencesXml;
        }

        public XDocument Document { get; set; }
        public XDocument DefaultDocument
        {
            get { return new XDocument(new XElement("preferences")); }
        }
        public XElement PreferencesElement
        {
            get { return Document.Element("preferences"); }
        }

        private string filename;
        public string Filename
        {
            get { return filename; }
            set
            {
                filename = value;
                if (File.Exists(filename))
                {
                    // Load the document
                    try
                    {
                        using (FileStream filestream = new FileStream(value, FileMode.Open))
                        {
                            Document = XDocument.Load(filestream);
                        }
                        return;
                    }
                    catch (Exception e)
                    {
#if LOG_ENABLED
                        LogManager.Error(0, string.Format("Failed reading preferences file: {0}", e.Message));
#endif
                    }
                }
                // Fallback: use default document if preferences don't exist or due to something else
                Document = DefaultDocument;
#if LOG_ENABLED
                LogManager.Info(0, "Using default preferences file.");
#endif
            }
        }

        private string DeterminePreferenceType(PreferenceType typeId)
        {
            string type;
            switch (typeId)
            {
                case PreferenceType.String:
                    type = "string";
                    break;
                case PreferenceType.Boolean:
                    type = "bool";
                    break;
                case PreferenceType.Integer:
                    type = "int";
                    break;
                case PreferenceType.Float:
                    type = "float";
                    break;
                default:
                    type = "object";
                    break;
            }
            return type;
        }

        public object this[PreferenceType typeId, string name]
        {
            get
            {
                string type = DeterminePreferenceType(typeId);
                IEnumerable<XElement> element =
                    from el in PreferencesElement.Elements(type)
                    where (string)el.Attribute("id") == name
                    select el;
                XElement node = element.ElementAtOrDefault(0);
                string value = null;
                if (node != null)
                {
                    value = node.Value;
                }
#if LOG_ENABLED
                LogManager.Info(0, string.Format("Get Pref - Type: {0}, Name: {1}, Value: {2}",
                    type, name, (value != null) ? value : "default"));
#endif
                return value;
            }
            set
            {
                string type = DeterminePreferenceType(typeId);
                IEnumerable<XElement> element =
                    from el in PreferencesElement.Elements(type)
                    where (string)el.Attribute("id") == name
                    select el;
                XElement node = element.ElementAtOrDefault(0);
                if (node != null)
                {
                    node.Value = value.ToString();
#if LOG_ENABLED
                    LogManager.Info(0, string.Format("Set Pref - Type: {0}, Name: {1}, Old value: {2}, New value: {3}",
                        type, name, node.Value, value));
#endif
                    return;
                }
                // Create preference if node doesn't exist
                PreferencesElement.Add(new XElement(type, new XAttribute("id", name), value));
#if LOG_ENABLED
                LogManager.Info(0, string.Format("New Pref - Type: {0}, Name: {1}, Value: {2}",
                    type, name, value));
#endif
            }
        }

        public object this[PreferenceType typeId, string name, object defaultValue]
        {
            get
            {
                object value = this[typeId, name];
                if (value != null)
                {
                    return value;
                }
                return defaultValue;
            }
        }

        public void Save()
        {
            using (FileStream filestream = new FileStream(filename, FileMode.Create))
            {
                Document.Save(filestream);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Save();
                Document = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
