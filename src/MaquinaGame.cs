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
        public GraphicsDeviceManager Graphics;
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
            // Initialize graphics manager
            Graphics = new GraphicsDeviceManager(this);
            // Set root directory where content files will be loaded
            Content.RootDirectory = Global.ContentRootDirectory;
            Global.Game = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create instance
            PreferencesManager = new PreferencesManager();
            LocaleManager = new LocaleManager(PreferencesManager.GetCharPref("app.locale", Global.DefaultLocale));
            InputManager = new InputManager();
            AudioManager = new AudioManager();
            SceneManager = new SceneManager();
            DisplayManager = new DisplayManager();

            Global.PreferencesManager = PreferencesManager;
            Global.LocaleManager = LocaleManager;
            Global.InputManager = InputManager;
            Global.AudioManager = AudioManager;
            Global.SceneManager = SceneManager;
            Global.DisplayManager = DisplayManager;

            // Window
            IsMouseVisible = PreferencesManager.GetBoolPref("app.window.useNativeCursor", false);
            LastWindowWidth = PreferencesManager.GetIntPref("app.window.width", 800);
            LastWindowHeight = PreferencesManager.GetIntPref("app.window.height", 600);
            Window.AllowUserResizing = PreferencesManager.GetBoolPref("app.window.allowUserResizing", false);

            // Audio
            float soundVolume;
            float.TryParse(PreferencesManager.GetCharPref("app.audio.sound", "1f"), out soundVolume);
            AudioManager.SoundVolume = soundVolume;
            AudioManager.MusicVolume = PreferencesManager.GetIntPref("app.audio.music", 255);
            AudioManager.IsMuted = PreferencesManager.GetBoolPref("app.audio.mastermuted", false);

#if DEBUG
            Graphics.HardwareModeSwitch = !PreferencesManager.GetBoolPref("app.window.fullscreen.borderless", true);
#else
            Graphics.HardwareModeSwitch = !PreferencesManager.GetBoolPref("app.window.fullscreen.borderless", false);
#endif

            // TODO: move to display manager
            // Identify if we should go fullscreen
            if (PreferencesManager.GetBoolPref("app.window.fullscreen", false))
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
                    Path.Combine(Global.ContentRootDirectory, Global.ResourceXml));
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
#if HAS_CONSOLE && LOG_GENERAL
            Console.WriteLine("Unloading game content");
#endif
            // TODO: move to display manager
            PreferencesManager.SetBoolPref("app.window.fullscreen", Graphics.IsFullScreen);
            // Save window dimensions if not in fullscreen
            if (!Graphics.IsFullScreen)
            {
                PreferencesManager.SetIntPref("app.window.width", Graphics.PreferredBackBufferWidth);
                PreferencesManager.SetIntPref("app.window.height", Graphics.PreferredBackBufferHeight);
            }
            PreferencesManager.SetBoolPref("app.audio.mastermuted", AudioManager.IsMuted);
            PreferencesManager.SetCharPref("app.audio.sound", AudioManager.SoundVolume.ToString());
            PreferencesManager.SetIntPref("app.audio.music", AudioManager.MusicVolume);

            // Dispose content
            SceneManager.Dispose();
            SpriteBatch.Dispose();
            Graphics.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            DisplayManager.Update();
            InputManager.UpdateInput();
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
