using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("manifest")]
    public class LocaleManifest : IManifest
    {
        [XmlAttribute("id")]
        public string Id
        {
            get { return "locale"; }
            set { /* Ignore given ID */ }
        }
        private int _actualRevision;
        [XmlAttribute("revision")]
        public int Revision
        {
            get { return 0; }
            set { _actualRevision = value; }
        }

        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("code")]
        public string Code { get; set; }
        [XmlElement("author")]
        public string[] Authors { get; set; }
    }
}
