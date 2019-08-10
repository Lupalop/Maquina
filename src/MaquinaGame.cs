using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Maquina.UI;
using Maquina.Resources;
using System.IO;

namespace Maquina
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public abstract class MaquinaGame : Game
    {
        protected GraphicsDeviceManager Graphics;
        protected SpriteBatch SpriteBatch;
        protected SceneManager SceneManager;
        protected LocaleManager LocaleManager;
        protected InputManager InputManager;
        protected PreferencesManager PreferencesManager;
        protected AudioManager AudioManager;
        protected DisplayManager DisplayManager;

        // TODO: move this to display manager
        protected int LastWindowWidth;
        protected int LastWindowHeight;

        public MaquinaGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Global.Game = this;
            Global.Graphics = Graphics;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Global.Content = Content;
            // Create instance
            PreferencesManager = new PreferencesManager();
            LocaleManager = new LocaleManager(PreferencesManager.GetStringPreference("app.locale", Global.DefaultLocale));
            InputManager = new InputManager();
            AudioManager = new AudioManager();
            SceneManager = new SceneManager();
            DisplayManager = new DisplayManager();

            Global.Preferences = PreferencesManager;
            Global.Locale = LocaleManager;
            Global.Input = InputManager;
            Global.Audio = AudioManager;
            Global.Scenes = SceneManager;
            Global.Display = DisplayManager;

            // Window
            IsMouseVisible = PreferencesManager.GetBoolPreference("app.window.useNativeCursor", false);
            LastWindowWidth = PreferencesManager.GetIntPreference("app.window.width", 800);
            LastWindowHeight = PreferencesManager.GetIntPreference("app.window.height", 600);
            Window.AllowUserResizing = PreferencesManager.GetBoolPreference("app.window.allowUserResizing", false);

            // Audio
            float soundVolume;
            float.TryParse(PreferencesManager.GetStringPreference("app.audio.sound", "1f"), out soundVolume);
            AudioManager.SoundVolume = soundVolume;
            AudioManager.MusicVolume = PreferencesManager.GetIntPreference("app.audio.music", 255);
            AudioManager.IsMuted = PreferencesManager.GetBoolPreference("app.audio.mastermuted", false);

#if DEBUG
            Graphics.HardwareModeSwitch = !PreferencesManager.GetBoolPreference("app.window.fullscreen.borderless", true);
#else
            Graphics.HardwareModeSwitch = !PreferencesManager.GetBoolPref("app.window.fullscreen.borderless", false);
#endif

            // TODO: move to display manager
            // Identify if we should go fullscreen
            if (PreferencesManager.GetBoolPreference("app.window.fullscreen", false))
            {
                Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;

                Graphics.ToggleFullScreen();
            }
            else
            {
                Graphics.PreferredBackBufferWidth = LastWindowWidth;
                Graphics.PreferredBackBufferHeight = LastWindowHeight;
            }
            Graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create instance of SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Global.SpriteBatch = SpriteBatch;
            // Create instance of the content loader
            ContentLoader<ResourceContent> resources = new ContentLoader<ResourceContent>();
            // Load platform resources synchronously
            resources.Content = resources.Initialize(
                    Path.Combine(Content.RootDirectory, Global.ResourceXml));
            Global.Fonts =
                resources.Content.Load(ResourceType.Fonts) as Dictionary<string, SpriteFont>;
            Global.BGM =
                resources.Content.Load(ResourceType.BGM) as Dictionary<string, Song>;
            Global.SFX =
                resources.Content.Load(ResourceType.SFX) as Dictionary<string, SoundEffect>;
            Global.Textures =
                resources.Content.Load(ResourceType.Textures) as Dictionary<string, Texture2D>;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: move to display manager
            PreferencesManager.SetBoolPreference("app.window.fullscreen", Graphics.IsFullScreen);
            // Save window dimensions if not in fullscreen
            if (!Graphics.IsFullScreen)
            {
                PreferencesManager.SetIntPreference("app.window.width", Graphics.PreferredBackBufferWidth);
                PreferencesManager.SetIntPreference("app.window.height", Graphics.PreferredBackBufferHeight);
            }
            PreferencesManager.SetBoolPreference("app.audio.mastermuted", AudioManager.IsMuted);
            PreferencesManager.SetStringPreference("app.audio.sound", AudioManager.SoundVolume.ToString());
            PreferencesManager.SetIntPreference("app.audio.music", AudioManager.MusicVolume);

            // Dispose content
            PreferencesManager.Dispose();
            SceneManager.Dispose();
            SpriteBatch.Dispose();
            Graphics.Dispose();

#if LOG_ENABLED
            LogManager.Info(0, "Game content unloaded.");
#endif
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            DisplayManager.Update();
            InputManager.Update();
            SceneManager.Update(gameTime);
            TimerManager.Update(gameTime);
            SoftwareMouse.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SceneManager.Draw(gameTime);
            SoftwareMouse.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
