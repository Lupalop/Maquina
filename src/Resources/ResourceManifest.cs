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

        public ResourceGroup Load(string groupId)
        {
            ResourceGroup group = new ResourceGroup();

            for (int i = 0; i < ResourceGroups.Length; i++)
            {
                if (ResourceGroups[i].Id == groupId)
                {
                    group = ResourceGroups[i];
                }
            }

            Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
            if (group.Fonts != null)
            {
                for (int i = 0; i < group.Fonts.Length; i++)
                {
                    FontParameters item = group.Fonts[i];
                    fonts[item.Id] = Global.Content.Load<SpriteFont>(item.Path);
                    fonts[item.Id].Spacing = item.Spacing;
                    fonts[item.Id].LineSpacing = item.LineSpacing;
                }
            }
            group.FontDictionary = fonts;

            Dictionary<string, Song> songs = new Dictionary<string, Song>();
            if (group.BGM != null)
            {
                for (int i = 0; i < group.BGM.Length; i++)
                {
                    ResourceParameters item = group.BGM[i];
                    songs[item.Id] = Global.Content.Load<Song>(item.Path);
                }
            }
            group.BGMDictionary = songs;

            Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
            if (group.SFX != null)
            {
                for (int i = 0; i < group.SFX.Length; i++)
                {
                    ResourceParameters item = group.SFX[i];
                    sounds[item.Id] = Global.Content.Load<SoundEffect>(item.Path);
                }
            }
            group.SFXDictionary = sounds;

            Dictionary<string, Texture2D> tex = new Dictionary<string, Texture2D>();
            if (group.Textures != null)
            {
                for (int i = 0; i < group.Textures.Length; i++)
                {
                    ResourceParameters item = group.Textures[i];
                    tex[item.Id] = Global.Content.Load<Texture2D>(item.Path);
                }
            }
            group.TextureDictionary = tex;

            return group;
        }

        public ResourceGroup LoadAll()
        {
            ResourceGroup masterGroup = new ResourceGroup();

            for (int i = 0; i < ResourceGroups.Length; i++)
            {
                ResourceGroup group = Load(ResourceGroups[i].Id);
                if (i >= 1)
                {
                    masterGroup.FontDictionary =
                        masterGroup.FontDictionary.MergeWith(group.FontDictionary);
                    masterGroup.BGMDictionary =
                        masterGroup.BGMDictionary.MergeWith(group.BGMDictionary);
                    masterGroup.SFXDictionary =
                        masterGroup.SFXDictionary.MergeWith(group.SFXDictionary);
                    masterGroup.TextureDictionary =
                        masterGroup.TextureDictionary.MergeWith(group.TextureDictionary);
                }
                else
                {
                    masterGroup.FontDictionary = group.FontDictionary;
                    masterGroup.BGMDictionary = group.BGMDictionary;
                    masterGroup.SFXDictionary = group.SFXDictionary;
                    masterGroup.TextureDictionary = group.TextureDictionary;
                }
            }

            return masterGroup;
        }
    }
}
