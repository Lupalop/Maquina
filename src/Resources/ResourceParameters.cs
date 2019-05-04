using System.Xml.Serialization;

namespace Maquina.Resources
{
    public class ResourceParameters
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Location { get; set; }
    }
}
