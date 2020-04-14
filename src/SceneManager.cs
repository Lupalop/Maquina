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
                    oldScene.Dispose();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    oldScene.Dispose();
                    newScene.LoadContent();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (Scene scene in Overlays.Values)
                    {
                        scene.Dispose();
                    }
                    break;
            }
        }

        public ObservableDictionary<string, Scene> Overlays { get; private set; }

        public Scene CurrentScene { get; protected set; }

        public void SwitchToScene(Scene scene, bool shouldLoadContent = true)
        {
            if (scene == null)
            {
#if LOG_ENABLED
                LogManager.Error(0, "Can't switch to a non-existent scene.");
#endif
                return;
            }
            if (CurrentScene == scene)
            {
#if LOG_ENABLED
                LogManager.Error(0, "Can't switch to the given scene because it is the current scene.");
#endif
                return;
            }
            CurrentScene._stopUpdating = true;
            // Show a fade effect when switching
            string overlayKey = string.Format("fade-{0}", scene);
            FadeOverlay overlay = new FadeOverlay(overlayKey);
            if (!Overlays.ContainsKey(overlayKey))
            {
                Overlays.Add(overlayKey, overlay);
            }
            // Unload previous scene
            if (CurrentScene != null)
            {
                CurrentScene.Dispose();
            }
            // Check if load content should still be called
            if (shouldLoadContent)
            {
                scene.LoadContent();
            }
            // Do a pre-current scene update pass
            scene.Update();
            // Set current state to given scene
            overlay.FadeInAnimation.AnimationFinished += (sender, e) =>
            {
#if LOG_ENABLED
                LogManager.Info(0, string.Format("Switched to scene: {0}", scene.Name));
#endif
                CurrentScene = scene;
            };
        }

        internal void Draw()
        {
            CurrentScene.Draw();
            // If there are Overlays, call their draw method
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                Overlays.Values.ElementAt(i).Draw();
            }
        }

        internal void Update()
        {
            if (!CurrentScene._stopUpdating)
            {
                CurrentScene.Update();
            }
            // If there are Overlays, call their update method
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                Overlays.Values.ElementAt(i).Update();
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
                CurrentScene.Dispose();
                Overlays.Clear();
            }
        }
    }
}
