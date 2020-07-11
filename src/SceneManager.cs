using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Globalization;
using Maquina.Content;
using Maquina.UI;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Maquina
{
    public class SceneManager : IDisposable
    {
        public SceneManager()
        {
            Overlays = new ObservableCollection<Scene>();
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
                    foreach (Scene scene in Overlays)
                    {
                        scene.Dispose();
                    }
                    break;
            }
        }

        public ObservableCollection<Scene> Overlays { get; private set; }

        public Scene CurrentScene { get; protected set; }

        public void SwitchToScene(Scene scene, bool shouldLoadContent = true)
        {
            List<string> x = new List<string>();
            if (scene == null)
            {
#if MGE_LOGGING
                LogManager.Error(0, "Can't switch to a non-existent scene.");
#endif
                return;
            }
            if (CurrentScene == scene)
            {
#if MGE_LOGGING
                LogManager.Error(0, "Can't switch to the given scene because it is the current scene.");
#endif
                return;
            }
            // Prevent current scene from updating
            CurrentScene.Enabled = false;

            FadeOverlay overlay = new FadeOverlay("fade-overlay");
            // Switch to the given scene once the fade transition is complete
            overlay.FadeInAnimation.AnimationFinished += (sender, e) =>
            {
#if MGE_LOGGING
                LogManager.Info(0, string.Format("Switched to scene: {0}", scene.Name));
#endif
                CurrentScene = scene;
            };
            Overlays.Add(overlay);

            // Unload previous scene
            CurrentScene.Dispose();

            // Check if load content should still be called
            if (shouldLoadContent)
            {
                scene.LoadContent();
            }
        }

        internal void Draw()
        {
            CurrentScene.Draw();
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                Overlays[i].Draw();
            }
        }

        internal void Update()
        {
            if (CurrentScene.Enabled)
            {
                CurrentScene.Update();
            }
            for (int i = Overlays.Count - 1; i >= 0; i--)
            {
                Overlays[i].Update();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CurrentScene.Dispose();
                Overlays.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
