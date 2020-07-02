using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Maquina.UI;

namespace Maquina
{
    public static class Application
    {
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
#if MGE_LOCALE
            Locale = new LocaleManager();
#endif
            SoftwareMouse = new SoftwareMouse();

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
            SoftwareMouse.Draw(SpriteBatch);
        }

        // Platform version information
        private static Version _version = Assembly.GetExecutingAssembly().GetName().Version;
        public static DateTime BuildDateTime = new DateTime(2000, 1, 1)
            .AddDays(_version.Build)
            .AddSeconds(_version.Revision * 2);
        public static readonly string BuildId = string.Format("{0} {1}",
            BuildDateTime.ToShortDateString(), BuildDateTime.ToShortTimeString());
        
        // Increment API version for breaking changes
        public static readonly int APIVersion = 0;

        // Game component managers
        public static SceneManager Scenes { get; private set; }
        public static AudioManager Audio { get; private set; }
        public static InputManager Input { get; private set; }
#if MGE_LOCALE
        public static LocaleManager Locale { get; private set; }
#endif
        public static DisplayManager Display { get; private set; }
        public static PreferencesManager Preferences { get; private set; }
        public static SoftwareMouse SoftwareMouse { get; private set; }

        // Game properties
        public static MaquinaGame Game { get; private set; }
        public static GameTime GameTime
        {
            get { return Game.GameTime; }
        }
        public static ContentManager Content
        {
            get { return Game.Content; }
        }
        public static GraphicsDeviceManager Graphics
        {
            get { return Game.Graphics; }
        }
        public static GraphicsDevice GraphicsDevice
        {
            get { return Game.GraphicsDevice; }
        }
        public static SpriteBatch SpriteBatch
        {
            get { return Game.SpriteBatch; }
        }
    }
}
