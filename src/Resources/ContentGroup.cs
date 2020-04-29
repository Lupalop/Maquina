using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    public class ContentGroup
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlElement("font")]
        public SpriteFontProperty[] FontPropertySet { get; set; }
        [XmlElement("music")]
        public Property<string>[] MusicPropertySet { get; set; }
        [XmlElement("sfx")]
        public Property<string>[] SfxPropertySet { get; set; }
        [XmlElement("texture")]
        public Property<string>[] TexturePropertySet { get; set; }
    }
}
