using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Maquina.UI;

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
        }

        private static bool _isInitialized;
        internal static void Initialize(MaquinaGame gameClass)
        {
            if (_isInitialized)
            {
                return;
            }

            Game = gameClass;

            Preferences = new PreferencesManager();
            Audio = new AudioManager();
            Display = new DisplayManager();
            Input = new InputManager();
            Scenes = new SceneManager();
            Locale = new LocaleManager();

            _isInitialized = true;
        }

        internal static void Unload()
        {
            if (!_isInitialized)
            {
                return;
            }

            Audio.Dispose();
            Display.Dispose();
            Scenes.Dispose();
            Preferences.Dispose();

            _isInitialized = false;
        }

        internal static void Update()
        {
            if (!_isInitialized)
            {
                return;
            }

            Display.Update();
            Input.Update();
            Scenes.Update();
            AnimationManager.Update();
            TimerManager.Update();
            SoftwareMouse.Update();
        }

        internal static void Draw()
        {
            if (!_isInitialized)
            {
                return;
            }

            Scenes.Draw();
            SoftwareMouse.Draw();
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

        // Game managers
        public static SceneManager Scenes { get; private set; }
        public static AudioManager Audio { get; private set; }
        public static InputManager Input { get; private set; }
        public static LocaleManager Locale { get; private set; }
        public static DisplayManager Display { get; private set; }
        public static PreferencesManager Preferences { get; private set; }

        // MG Framework
        public static SpriteBatch SpriteBatch { get; set; }
        public static MaquinaGame Game { get; set; }
        public static GameTime GameTime { get; set; }
        public static ContentManager Content
        {
            get { return Game.Content; }
        }
        public static GraphicsDeviceManager Graphics
        {
            get { return Game.Graphics; }
        }
    }
}
