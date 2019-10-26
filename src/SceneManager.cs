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
using System.Collections.Specialized;

namespace Maquina
{
    public class SceneManager : IDisposable
    {
        public SceneManager()
        {
            Overlays = new ObservableDictionary<string, Scene>();
            Overlays.CollectionChanged += Overlays_CollectionChanged;
            CurrentScene = new EmptyScene();
        }

        void Overlays_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Scene newScene = null;
            Scene oldScene = null;
            
            if (e.NewItems != null)
            {
                newScene = (Scene)e.NewItems[0];
            }
            if (e.OldItems != null)
            {
                oldScene = (Scene)e.OldItems[0];
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    newScene.LoadContent();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    oldScene.Unload();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    oldScene.Unload();
                    newScene.LoadContent();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (Scene scene in Overlays.Values)
                    {
                        scene.Unload();
                    }
                    break;
            }
        }

        public ObservableDictionary<string, Scene> Overlays { get; private set; }

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
#if LOG_ENABLED
            LogManager.Info(0, string.Format("Switched to scene: {0}", scene.SceneName));
#endif
            if (scene == null)
            {
#if LOG_ENABLED
                LogManager.Error(0, "Unable to switch to a `null` scene.");
#endif
                return;
            }
            // Show a fade effect when switching
            string overlayKey = String.Format("fade-{0}", scene);
            FadeOverlay overlay = new FadeOverlay(overlayKey);
            if (!Overlays.ContainsKey(overlayKey))
            {
                Overlays.Add(overlayKey, overlay);
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
            overlay.FadeInAnimation.AnimationFinished += (sender, e) => CurrentScene = scene;
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
