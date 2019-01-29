﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Globalization;
using Maquina.Resources;

namespace Maquina.UI
{
    public class SceneManager : IDisposable
    {
        public SceneManager(Game game,
            SpriteBatch spriteBatch, Dictionary<string, SpriteFont> fonts,
            Dictionary<string, Song> songs, LocaleManager localeManager,
            InputManager inputManager)
        {
            this.Game = game;
            this.SpriteBatch = spriteBatch;
            this.Fonts = fonts;
            this.Songs = songs;
            this.Overlays = new SceneDictionary<string>();
            this.LocaleManager = localeManager;
            this.InputManager = inputManager;
        }

        public SceneBase CurrentScene { get; protected set; }
        private SceneBase _storedScene;
        public SceneBase StoredScene
        {
            get
            {
                return _storedScene;
            }
            set
            {
                // Preload content
                value.LoadContent();
                _storedScene = value;
            }
        }

        public void SwitchToScene(SceneBase scene, bool shouldLoadContent = true)
        {
#if DEBUG
            Console.WriteLine("Switching to scene: {0}", scene.SceneName);
#endif
            if (scene == null)
            {
#if DEBUG
                Console.WriteLine("Switching to given scene failed!");
#endif
                return;
            }
            // Unload previous scene
            if (CurrentScene != null)
                CurrentScene.Unload();
            // Check if load content should still be called
            if (shouldLoadContent)
                scene.LoadContent();
            // Load delayed content
            scene.DelayLoadContent();
            // Set current state to given scene
            CurrentScene = scene;
            // Show a fade effect to hide first frame misposition
            string overlayKey = String.Format("fade-{0}", scene);
            if (!Overlays.ContainsKey(overlayKey))
                Overlays.Add(overlayKey, new Scenes.FadeOverlay(this, overlayKey));
        }

        public bool SwitchToStoredScene()
        {
            if (StoredScene == null)
            {
                return false;
            }
            SwitchToScene(StoredScene, false);
            return true;
        }
        

        public Game Game { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        // Container of loaded Fonts
        public Dictionary<string, SpriteFont> Fonts { get; private set; }
        // Container of loaded Songs
        public Dictionary<string, Song> Songs { get; private set; }
        // Container of loaded overlay scenes
        public SceneDictionary<string> Overlays { get; private set; }
        public LocaleManager LocaleManager { get; private set; }
        // Input
        public InputManager InputManager { get; private set; }

        public void PlaySong(string songName, bool isRepeating = true)
        {
            if (Songs.ContainsKey(songName))
            {
                Song song = Songs[songName];
                if (MediaPlayer.Queue.ActiveSong != song)
                {
                    MediaPlayer.Play(song);
                    MediaPlayer.IsRepeating = isRepeating;
                }
            }
#if DEBUG
            else
            {
                Console.WriteLine(String.Format("SceneManager: Song '{0}' not found!", songName));
            }
#endif
        }
        public void PlaySong(string songName)
        {
            PlaySong(songName, true);
        }

        public void Draw(GameTime gameTime)
        {
            CurrentScene.Draw(gameTime);
            // If there are Overlays, call their draw method
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                Overlays[Overlays.Keys.ToList()[i]].Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime);
            InputManager.UpdateInput();
            InputManager.UpdateScene(CurrentScene);
            // If there are Overlays, call their update method
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                SceneBase scb = Overlays[Overlays.Keys.ToList()[i]];
                scb.Update(gameTime);
                InputManager.UpdateScene(scb);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CurrentScene.Unload();
                Overlays.Clear();
            }
        }
    }
}
