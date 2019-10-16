using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    public class FontParameters : ResourceParameters
    {
        [XmlAttribute("linespacing")]
        public int LineSpacing { get; set; }
        [XmlAttribute("spacing")]
        public float Spacing { get; set; }
    }
}
