using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Maquina.Resources;
using System.IO;

namespace Maquina
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public abstract class MaquinaGame : Game
    {
        public GraphicsDeviceManager Graphics { get; protected set; }
        public SpriteBatch SpriteBatch { get; protected set; }
        public GameTime GameTime { get; protected set; }
        public const string ResourceXml = "resources.xml";

        public MaquinaGame()
        {
            Graphics = new GraphicsDeviceManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Application.Initialize(this);
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
            // Load platform resources synchronously
            string resourcePath = Path.Combine(Content.RootDirectory, ResourceXml);
            ContentManifest resources = XmlHelper.Load<ContentManifest>(resourcePath);
            ContentFactory.Source.Add("platform", resources.Load("platform"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Application.Unload();
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
            GameTime = gameTime;
            Application.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Application.Draw();
            base.Draw(gameTime);
        }
    }
}
