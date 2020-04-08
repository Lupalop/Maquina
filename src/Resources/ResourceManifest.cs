using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("resourcemanifest")]
    [Content("ResourceManifest")]
    public class ResourceManifest
    {
        [XmlElement("resources")]
        public ResourceGroup[] ResourceGroups { get; set; }

        public Dictionary<string, object> Load(string groupId)
        {
            ResourceGroup group = new ResourceGroup();

            for (int i = 0; i < ResourceGroups.Length; i++)
            {
                if (ResourceGroups[i].Id == groupId)
                {
                    group = ResourceGroups[i];
                }
            }

            Dictionary<string, object> resourceDictionary = new Dictionary<string, object>();

            if (group.Fonts != null)
            {
                for (int i = 0; i < group.Fonts.Length; i++)
                {
                    FontParameters item = group.Fonts[i];
                    SpriteFont spriteFont = Application.Content.Load<SpriteFont>(item.Path);
                    spriteFont.Spacing = item.Spacing;
                    spriteFont.LineSpacing = item.LineSpacing;
                    resourceDictionary[item.Id] = spriteFont;
                }
            }
            if (group.BGM != null)
            {
                for (int i = 0; i < group.BGM.Length; i++)
                {
                    ResourceParameters item = group.BGM[i];
                    resourceDictionary[item.Id] = Application.Content.Load<Song>(item.Path);
                }
            }
            if (group.SFX != null)
            {
                for (int i = 0; i < group.SFX.Length; i++)
                {
                    ResourceParameters item = group.SFX[i];
                    resourceDictionary[item.Id] = Application.Content.Load<SoundEffect>(item.Path);
                }
            }
            if (group.Textures != null)
            {
                for (int i = 0; i < group.Textures.Length; i++)
                {
                    ResourceParameters item = group.Textures[i];
                    resourceDictionary[item.Id] = Application.Content.Load<Texture2D>(item.Path);
                }
            }

            return resourceDictionary;
        }

        public Dictionary<string, object>[] LoadAll()
        {
            Dictionary<string, object>[] resourceDictionaries = new Dictionary<string, object>[ResourceGroups.Length];

            for (int i = 0; i < ResourceGroups.Length; i++)
            {
                resourceDictionaries[i] = Load(ResourceGroups[i].Id);
            }

            return resourceDictionaries;
        }
    }
}
