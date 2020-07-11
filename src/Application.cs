using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Maquina.UI;
using System.Collections.Generic;
using Maquina.Entities;

namespace Maquina
{
    public static class Application
    {
        private static bool _isInitialized;
        public static readonly Version Version;
        public static readonly DateTime BuildDateTime;

        static Application()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version;
            BuildDateTime = new DateTime(2000, 1, 1)
                .AddDays(Version.Build)
                .AddSeconds(Version.Revision * 2);
            Timers = new List<Timer>();
            Animations = new List<Animation>();
#if DEBUG
            TimersEnabled = true;
            AnimationsEnabled = true;
#endif
        }

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

        // Timer/Animation lists
        public static List<Timer> Timers;
        public static List<Animation> Animations;
#if DEBUG
        public static bool AnimationsEnabled { get; set; }
        public static bool TimersEnabled { get; set; }
#endif

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
            for (int i = 0; i < Timers.Count; i++)
            {
#if DEBUG
                if (!TimersEnabled)
                {
                    break;
                }
#endif
                Timers[i].Update();
            }
            for (int i = 0; i < Animations.Count; i++)
            {
#if DEBUG
                if (!AnimationsEnabled)
                {
                    break;
                }
#endif
                Animations[i].Update();
            }
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
    }
}
