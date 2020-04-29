using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("manifest")]
    public class ContentManifest : IManifest
    {
        [XmlAttribute("id")]
        public string Id
        {
            get { return "content"; }
            set { /* Ignore given ID */ }
        }
        private int _actualRevision;
        [XmlAttribute("revision")]
        public int Revision
        {
            get { return 0; }
            set { _actualRevision = value; }
        }

        [XmlElement("group")]
        public ContentGroup[] ContentGroups { get; set; }

        public Dictionary<string, object> Load(string groupId)
        {
            ContentGroup group = new ContentGroup();

            for (int i = 0; i < ContentGroups.Length; i++)
            {
                if (ContentGroups[i].Id == groupId)
                {
                    group = ContentGroups[i];
                }
            }

            Dictionary<string, object> resourceDictionary = new Dictionary<string, object>();

            if (group.FontPropertySet != null)
            {
                for (int i = 0; i < group.FontPropertySet.Length; i++)
                {
                    SpriteFontProperty item = group.FontPropertySet[i];
                    SpriteFont spriteFont = Application.Content.Load<SpriteFont>(item.Value);
                    spriteFont.Spacing = item.Spacing;
                    spriteFont.LineSpacing = item.LineSpacing;
                    resourceDictionary[item.Id] = spriteFont;
                }
            }
            if (group.MusicPropertySet != null)
            {
                for (int i = 0; i < group.MusicPropertySet.Length; i++)
                {
                    Property<string> item = group.MusicPropertySet[i];
                    resourceDictionary[item.Id] = Application.Content.Load<Song>(item.Value);
                }
            }
            if (group.SfxPropertySet != null)
            {
                for (int i = 0; i < group.SfxPropertySet.Length; i++)
                {
                    Property<string> item = group.SfxPropertySet[i];
                    resourceDictionary[item.Id] = Application.Content.Load<SoundEffect>(item.Value);
                }
            }
            if (group.TexturePropertySet != null)
            {
                for (int i = 0; i < group.TexturePropertySet.Length; i++)
                {
                    Property<string> item = group.TexturePropertySet[i];
                    resourceDictionary[item.Id] = Application.Content.Load<Texture2D>(item.Value);
                }
            }

            return resourceDictionary;
        }

        public Dictionary<string, object>[] LoadAll()
        {
            Dictionary<string, object>[] resourceDictionaries = new Dictionary<string, object>[ContentGroups.Length];

            for (int i = 0; i < ContentGroups.Length; i++)
            {
                resourceDictionaries[i] = Load(ContentGroups[i].Id);
            }

            return resourceDictionaries;
        }
    }
}
