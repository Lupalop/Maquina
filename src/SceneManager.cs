using System;
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
using Maquina.UI;

namespace Maquina
{
    public class SceneManager : IDisposable
    {
        public SceneManager()
        {
            Overlays = new EventDictionary<string, Scene>();
            Overlays.ItemAdded += Overlays_ItemAdded;
            Overlays.ItemRemoved += Overlays_ItemRemoved;
            Overlays.DictionaryCleared += Overlays_DictionaryCleared;
            CurrentScene = new EmptyScene();
        }

        private void Overlays_ItemAdded(string key, Scene scene)
        {
            // Load content when scene is added
            scene.LoadContent();
        }

        private void Overlays_ItemRemoved(string key, Scene scene)
        {
            // Unload content when scene is removed
            scene.Unload();
        }

        private void Overlays_DictionaryCleared()
        {
            // Unload content of every scene
            foreach (Scene scene in Overlays.Values)
            {
                scene.Unload();
            }
        }

        public EventDictionary<string, Scene> Overlays { get; private set; }

        public Scene CurrentScene { get; protected set; }
        private Scene _storedScene;
        public Scene StoredScene
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

        public void SwitchToScene(Scene scene, bool shouldLoadContent = true)
        {
#if HAS_CONSOLE && LOG_GENERAL
            Console.WriteLine("Switching to scene: {0}", scene.SceneName);
#endif
            if (scene == null)
            {
#if HAS_CONSOLE && LOG_GENERAL
                Console.WriteLine("Switching to given scene failed!");
#endif
                return;
            }
            // Unload previous scene
            if (CurrentScene != null)
            {
                CurrentScene.Unload();
            }
            // Check if load content should still be called
            if (shouldLoadContent)
            {
                scene.LoadContent();
            }
            // Set current state to given scene
            CurrentScene = scene;
            // Show a fade effect when switching
            string overlayKey = String.Format("fade-{0}", scene);
            if (!Overlays.ContainsKey(overlayKey))
            {
                Overlays.Add(overlayKey, new FadeOverlay(overlayKey));
            }
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

        public void TryRemoveOverlays(string name)
        {
            for (int i = 0; i < Overlays.Keys.Count; i++)
            {
                if (Overlays.Keys.ElementAt(i).Contains(name))
                {
                    Overlays.Remove(Overlays.Keys.ElementAt(i));
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            CurrentScene.Draw(gameTime);
            // If there are Overlays, call their draw method
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                Overlays[Overlays.Keys.ElementAt(i)].Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime);
            // If there are Overlays, call their update method
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                Overlays[Overlays.Keys.ElementAt(i)].Update(gameTime);
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
