using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    public class ResourceParameters
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("path")]
        public string Path { get; set; }
    }
}
