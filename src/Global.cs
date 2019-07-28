using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Maquina.Elements;

namespace Maquina
{
    public static class Global
    {
        static Global()
        {
            ResourceXml = "platformresources.xml";
            PreferencesXml = "preferences.xml";
            LocaleDefinitionXml = "locale.xml";
            ContentRootDirectory = "Content";
            LocalesDirectory = "locales";
            DefaultLocale = "en-US";

            Scale = 1f;
            Fonts = new Dictionary<string, SpriteFont>();
            BGM = new Dictionary<string, Song>();
        }

        private static Version _version = Assembly.GetExecutingAssembly().GetName().Version;
        public static DateTime BuildDateTime = new DateTime(2000, 1, 1)
            .AddDays(_version.Build)
            .AddSeconds(_version.Revision * 2);

        // General Information
        public static readonly string Name = "Maquina";
        public static readonly string BuildId = String.Format("{0} {1}",
            BuildDateTime.ToShortDateString(), BuildDateTime.ToShortTimeString());
        public static readonly int APIVersion = 0;

        // Resources
        public static string ResourceXml { get; set; }
        public static string PreferencesXml { get; set; }
        public static string LocaleDefinitionXml { get; set; }
        public static string ContentRootDirectory { get; set; }
        public static string LocalesDirectory { get; set; }
        public static string DefaultLocale { get; set; }

        // App-wide properties
        public static Dictionary<string, SpriteFont> Fonts { get; set; }
        public static Dictionary<string, Song> BGM { get; set; }
        public static Dictionary<string, SoundEffect> SFX { get; set; }
        public static Dictionary<string, Texture2D> Textures { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static Game Game { get; set; }

        // Scale
        private static float scale;
        public static float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                OnScaleChanged(value);
            }
        }
        public static event EventHandler<float> ScaleChanged;
        private static void OnScaleChanged(float newScale)
        {
            if (ScaleChanged != null)
            {
                ScaleChanged(null, newScale);
            }
        }

        // Managers
        public static SceneManager SceneManager { get; set; }
        public static AudioManager AudioManager { get; set; }
        public static InputManager InputManager { get; set; }
        public static LocaleManager LocaleManager { get; set; }
        public static DisplayManager DisplayManager { get; set; }
        public static PreferencesManager PreferencesManager { get; set; }
    }
}
