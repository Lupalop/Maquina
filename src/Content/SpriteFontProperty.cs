using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace Maquina.Content
{
    public class SpriteFontProperty : Property<string>
    {
        [XmlAttribute("linespacing")]
        public int LineSpacing { get; set; }
        [XmlAttribute("spacing")]
        public float Spacing { get; set; }
    }
}
