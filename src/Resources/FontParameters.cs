using System.Xml.Serialization;

namespace Maquina.Resources
{
    public class FontParameters : ResourceParameters
    {
        [XmlAttribute]
        public int LineSpacing { get; set; }
        [XmlAttribute]
        public float Spacing { get; set; }
    }    
}
