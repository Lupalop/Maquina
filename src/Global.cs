using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Content;

namespace Maquina
{
    public static class Global
    {
        static Global()
        {
            ResourceXml = "resources.xml";
            PreferencesXml = "preferences.xml";
            LocaleDefinitionXml = "locale.xml";
            LocaleDirectory = "locales";
            DefaultLocale = "en-US";

            Fonts = new Dictionary<string, SpriteFont>();
            BGM = new Dictionary<string, Song>();
        }

        // Platform
        private static Version _version = Assembly.GetExecutingAssembly().GetName().Version;
        public static DateTime BuildDateTime = new DateTime(2000, 1, 1)
            .AddDays(_version.Build)
            .AddSeconds(_version.Revision * 2);
        public static readonly string BuildId = string.Format("{0} {1}",
            BuildDateTime.ToShortDateString(), BuildDateTime.ToShortTimeString());
        public static readonly int APIVersion = 0;

        // Resource locations
        public static string ResourceXml { get; set; }
        public static string PreferencesXml { get; set; }
        public static string LocaleDefinitionXml { get; set; }
        public static string LocaleDirectory { get; set; }
        public static string DefaultLocale { get; set; }

        // Content collections
        public static Dictionary<string, SpriteFont> Fonts { get; set; }
        public static Dictionary<string, Song> BGM { get; set; }
        public static Dictionary<string, SoundEffect> SFX { get; set; }
        public static Dictionary<string, Texture2D> Textures { get; set; }

        // Engine properties
        public static SpriteBatch SpriteBatch { get; set; }
        public static Game Game { get; set; }
        public static GameTime GameTime { get; set; }

        // Engine managers
        public static SceneManager Scenes { get; set; }
        public static AudioManager Audio { get; set; }
        public static InputManager Input { get; set; }
        public static LocaleManager Locale { get; set; }
        public static DisplayManager Display { get; set; }
        public static PreferencesManager Preferences { get; set; }
        public static ContentManager Content { get; set; }
        public static GraphicsDeviceManager Graphics { get; set; }
    }
}
