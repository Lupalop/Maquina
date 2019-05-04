using System.Xml.Serialization;

namespace Maquina.Resources
{
    public class StringParameters
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Content { get; set; }
    }
}
