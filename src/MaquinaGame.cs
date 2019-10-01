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
            Global.Preferences = PreferencesManager;

            LocaleManager = new LocaleManager(PreferencesManager.GetStringPreference("app.locale", Global.DefaultLocale));
            InputManager = new InputManager();
            AudioManager = new AudioManager();
            SceneManager = new SceneManager();
            DisplayManager = new DisplayManager();

            Global.Locale = LocaleManager;
            Global.Input = InputManager;
            Global.Audio = AudioManager;
            Global.Scenes = SceneManager;
            Global.Display = DisplayManager;

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
            // Dispose content
            AudioManager.Dispose();
            DisplayManager.Dispose();

            SceneManager.Dispose();
            SpriteBatch.Dispose();
            Graphics.Dispose();
            PreferencesManager.Dispose();
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
