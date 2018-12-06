using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    // <Content>
    // Used for loading primary content resources
    [XmlRoot("Content")]
    public class ResourceContent : IContent
    {
        // key, resource location
        public List<FontParameters> Fonts { get; set; }
        public List<ResourceParameters> BGM { get; set; }
        public List<ResourceParameters> SFX { get; set; }
        public List<ResourceParameters> Textures { get; set; }
    }

    public class FontParameters : ResourceParameters
    {
        [XmlAttribute]
        public int LineSpacing { get; set; }
        [XmlAttribute]
        public float Spacing { get; set; }
    }
    public class ResourceParameters
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Location { get; set; }
    }
    
    // <LocaleDefinition>
    [XmlRoot("LocaleDefinition")]
    public class LocaleDefinition : IContent
    {
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public List<string> Authors { get; set; }
    }

    // <StringBundle>
    // Used for loading localized string resources
    [XmlRoot("StringBundle")]
    public class StringBundle : IContent
    {
        public List<StringParameters> Strings { get; set; }
    }
    public class StringParameters
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Content { get; set; }
    }
}
