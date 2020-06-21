using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Content
{
    [XmlRoot("manifest")]
    public class PreferencesManifest : IManifest
    {
        public PreferencesManifest()
        {
            BooleanPropertySet = new List<Property<bool>>();
            Int32PropertySet = new List<Property<int>>();
            FloatPropertySet = new List<Property<float>>();
            StringPropertySet = new List<Property<string>>();
        }

        [XmlAttribute("id")]
        public string Id
        {
            get { return "preferences"; }
            set { /* Ignore given ID */ }
        }
        private int _actualRevision;
        [XmlAttribute("revision")]
        public int Revision
        {
            get { return 0; }
            set { _actualRevision = value; }
        }

        [XmlElement("bool")]
        public List<Property<bool>> BooleanPropertySet { get; set; }
        [XmlElement("int")]
        public List<Property<int>> Int32PropertySet { get; set; }
        [XmlElement("float")]
        public List<Property<float>> FloatPropertySet { get; set; }
        [XmlElement("string")]
        public List<Property<string>> StringPropertySet { get; set; }

        public void Reset()
        {
            BooleanPropertySet.Clear();
            Int32PropertySet.Clear();
            FloatPropertySet.Clear();
            StringPropertySet.Clear();
        }

        public T GetPreference<T>(List<Property<T>> collection, string name, T defaultValue)
        {
            T result = defaultValue;
            foreach (var item in collection)
            {
                if (item.Id == name)
                {
                    result = item.Value;
                }
            }
#if MGE_LOGGING
            LogManager.Info(0, string.Format("Get Pref - Name: {0}, Value: {1}",
                name, result));
#endif
            return result;
        }
        public void SetPreference<T>(List<Property<T>> collection, string name, T value)
        {
            foreach (var item in collection)
            {
                if (item.Id == name)
                {
#if MGE_LOGGING
                    LogManager.Info(0, string.Format("Set Pref - Name: {0}, Old value: {1}, New value: {2}",
                        name, item.Value, value));
#endif
                    item.Value = value;
                    return;
                }
            }

            // Create preference if it doesn't exist
            collection.Add(new Property<T>() { Id = name, Value = value });
#if MGE_LOGGING
            LogManager.Info(0, string.Format("New Pref - Name: {0}, Value: {1}", name, value));
#endif
        }
    }
}
