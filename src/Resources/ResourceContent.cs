using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("Content")]
    [Content("ResourceContent")]
    public class ResourceContent
    {
        // key, resource location
        public List<FontParameters> Fonts { get; set; }
        public List<ResourceParameters> BGM { get; set; }
        public List<ResourceParameters> SFX { get; set; }
        public List<ResourceParameters> Textures { get; set; }

        public object Load(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Fonts:
                    Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
                    for (int i = 0; i < Fonts.Count; i++)
                    {
                        FontParameters item = Fonts[i];
                        fonts[item.Name] = Global.Game.Content.Load<SpriteFont>(item.Location);
                        fonts[item.Name].Spacing = item.Spacing;
                        fonts[item.Name].LineSpacing = item.LineSpacing;
                    }
                    return fonts;
                case ResourceType.BGM:
                    Dictionary<string, Song> songs = new Dictionary<string, Song>();
                    for (int i = 0; i < BGM.Count; i++)
                    {
                        ResourceParameters item = BGM[i];
                        songs[item.Name] = Global.Game.Content.Load<Song>(item.Location);
                    }
                    return songs;
                case ResourceType.SFX:
                    Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
                    for (int i = 0; i < SFX.Count; i++)
                    {
                        ResourceParameters item = SFX[i];
                        sounds[item.Name] = Global.Game.Content.Load<SoundEffect>(item.Location);
                    }
                    return sounds;
                case ResourceType.Textures:
                    Dictionary<string, Texture2D> tex = new Dictionary<string, Texture2D>();
                    for (int i = 0; i < Textures.Count; i++)
                    {
                        ResourceParameters item = Textures[i];
                        tex[item.Name] = Global.Game.Content.Load<Texture2D>(item.Location);
                    }
                    return tex;
            }
            return null;
        }
    }
}
