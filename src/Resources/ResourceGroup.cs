using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    public class ResourceGroup
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlElement("font")]
        public FontParameters[] Fonts { get; set; }
        [XmlElement("music")]
        public ResourceParameters[] BGM { get; set; }
        [XmlElement("sfx")]
        public ResourceParameters[] SFX { get; set; }
        [XmlElement("texture")]
        public ResourceParameters[] Textures { get; set; }

        [XmlIgnore]
        public Dictionary<string, SpriteFont> FontDictionary { get; set; }
        [XmlIgnore]
        public Dictionary<string, Song> BGMDictionary { get; set; }
        [XmlIgnore]
        public Dictionary<string, SoundEffect> SFXDictionary { get; set; }
        [XmlIgnore]
        public Dictionary<string, Texture2D> TextureDictionary { get; set; }
    }
}
