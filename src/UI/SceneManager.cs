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
            Dictionary<string, Song> songs, LocaleManager localeManager)
        {
            this.Game = game;
            this.SpriteBatch = spriteBatch;
            this.Fonts = fonts;
            this.Songs = songs;
            this.Overlays = new Dictionary<string, SceneBase>();
            this.LocaleManager = localeManager;
        }

        private SceneBase _currentScene;
        public SceneBase CurrentScene
        {
            get
            {
                return _currentScene;
            }
            set
            {
#if DEBUG
                Console.WriteLine("Switching to scene: {0}", value.SceneName);
#endif
                // Unload previous scene
                if (_currentScene != null)
                    _currentScene.Unload();
                // TODO: ALLOW LOAD CONTENT TO BE ON DEMAND
                value.LoadContent();
                // Load delayed content
                value.DelayLoadContent();
                // Set current state to given scene
                _currentScene = value;
                // Show a fade effect to hide first frame misposition
                string overlayKey = String.Format("fade-{0}", value);
                if (!Overlays.ContainsKey(overlayKey))
                    Overlays.Add(overlayKey, new Scenes.FadeOverlay(this, overlayKey));
            }
        }

        public Game Game { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        // List of loaded Fonts
        public Dictionary<string, SpriteFont> Fonts { get; private set; }
        // List of loaded Songs
        public Dictionary<string, Song> Songs { get; private set; }
        // List of loaded overlay scenes
        public Dictionary<string, SceneBase> Overlays { get; private set; }
        // Locale Manager
        public LocaleManager LocaleManager { get; private set; }

        public KeyboardState KeyboardState { get; set; }
        public GamePadState GamepadState { get; set; }
        public MouseState MouseState { get; set; }
        public TouchPanelState TouchState { get; set; }

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
            UpdateKeys(CurrentScene);
            // If there are Overlays, call their update method
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                SceneBase scb = Overlays[Overlays.Keys.ToList()[i]];
                scb.Update(gameTime);
                UpdateKeys(scb);
            }
        }

        public void UpdateKeys(SceneBase scene)
        {
            if (scene != null)
            {
                scene.KeyboardState = KeyboardState;
                scene.GamepadState = GamepadState;
                scene.MouseState = MouseState;
                scene.TouchState = TouchState;
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
                _currentScene.Unload();
                foreach (SceneBase item in Overlays.Values)
                {
                    item.Unload();
                }
            }
        }
    }
}
