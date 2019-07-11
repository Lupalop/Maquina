using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Maquina.Elements;
using System.Collections.ObjectModel;

namespace Maquina.UI
{
    public abstract class Scene
    {
        public Scene(string sceneName = "Untitled Scene")
        {
            // Scene name assignment
            this.SceneName = sceneName;
            // Create local references to global properties
            SceneManager = Global.SceneManager;
            InputManager = Global.InputManager;
            Game = Global.Game;
            SpriteBatch = Global.SpriteBatch;
            Fonts = Global.Fonts;
            // 
            Elements = new Dictionary<string, BaseElement>();
            // Layout stuff
            ElementSpacing = 5;
            IsFirstUpdateDone = false;
        }

        private void DisplayManager_ResolutionChanged(Rectangle obj)
        {
            UpdateLayout(Elements);
        }

        protected SceneManager SceneManager { get; private set; }
        protected InputManager InputManager { get; private set; }
        protected Game Game { get; private set; }
        protected SpriteBatch SpriteBatch { get; private set; }
        protected Dictionary<string, SpriteFont> Fonts { get; private set; }

        public IDictionary<string, BaseElement> Elements { get; set; }
        public string SceneName { get; private set; }
        private bool IsFirstUpdateDone;

        public event EventHandler LoadContentFinished;
        public event EventHandler UnloadFinished;

        public Point ScreenCenter
        {
            get
            {
                return Global.DisplayManager.WindowBounds.Center;
            }
        }

        public virtual void LoadContent()
        {
#if HAS_CONSOLE && LOG_VERBOSE
            Console.WriteLine("Finished loading content from: {0}", SceneName);
#endif
            if (LoadContentFinished != null)
            {
                LoadContentFinished(this, EventArgs.Empty);
            }
            // Listen to resolution change events in order to update layout when needed
            Global.DisplayManager.ResolutionChanged += DisplayManager_ResolutionChanged;
            // Update layout after all elements were loaded
            UpdateLayout(Elements);
        }

        public virtual void Draw(GameTime gameTime) { }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsFirstUpdateDone)
            {
                IsFirstUpdateDone = true;
            }
        }

        public virtual void Unload()
        {
            DisposeElements(Elements);
#if HAS_CONSOLE && LOG_VERBOSE
            Console.WriteLine("Unloaded content from scene: {0}", SceneName);
#endif
            if (UnloadFinished != null)
            {
                UnloadFinished(this, EventArgs.Empty);
            }
        }

        public virtual void DisposeElements(IDictionary<string, BaseElement> objects)
        {
            DisposeElements(objects.Values);
        }
        public virtual void DisposeElements(IEnumerable<BaseElement> objects)
        {
            for (int i = 0; i < objects.Count(); i++)
            {
                objects.ElementAt(i).Dispose();
            }
        }

        public virtual int TotalElementHeight(IDictionary<string, BaseElement> objects)
        {
            return TotalElementHeight(objects.Values);
        }
        public virtual int TotalElementHeight(IEnumerable<BaseElement> objects)
        {
            int totalElementHeight = 0;

            for (int i = 0; i < objects.Count(); i++)
            {
                var currentObject = objects.ElementAt(i);
                if (!(currentObject is GuiElement) || currentObject == null)
                {
                    continue;
                }

                GuiElement Object = (GuiElement)currentObject;
                if (Object.ControlAlignment == Alignment.Center)
                {
                    totalElementHeight += currentObject.ActualBounds.Height;
                }
            }
            return totalElementHeight;
        }

        public virtual void DrawElements(GameTime gameTime, IDictionary<string, BaseElement> objects)
        {
            DrawElements(gameTime, objects.Values);
        }
        public virtual void DrawElements(GameTime gameTime, IEnumerable<BaseElement> objects)
        {
            if (!IsFirstUpdateDone)
            {
                return;
            }
            // Draw objects in the element array
            for (int i = 0; i < objects.Count(); i++)
            {
                try
                {
                    objects.ElementAt(i).Draw(gameTime);
                }
                catch (NullReferenceException)
                { 
                    // Suppress errors
                }
            }
        }

        public virtual void UpdateObjects(GameTime gameTime, IDictionary<string, BaseElement> objects)
        {
            UpdateObjects(gameTime, objects.Values);
        }
        public int ElementSpacing { get; set; }
        public virtual void UpdateObjects(GameTime gameTime, IEnumerable<BaseElement> objects)
        {
            for (int i = 0; i < objects.Count(); i++)
            {
                BaseElement element = objects.ElementAt(i);
                element.Update(gameTime);
            }
        }

        public virtual void UpdateLayout(IDictionary<string, BaseElement> objects)
        {
            UpdateLayout(objects.Values);
        }
        public virtual void UpdateLayout(IEnumerable<BaseElement> objects)
        {
            int distanceFromTop = ScreenCenter.Y - (int)(TotalElementHeight(objects) / 2);
            for (int i = 0; i < objects.Count(); i++)
            {
                BaseElement element = objects.ElementAt(i);
                if (!(element is GuiElement))
                {
                    continue;
                }

                var modifiedElement = (GuiElement)element;
                if (modifiedElement.ControlAlignment == Alignment.Center)
                {
                    if (modifiedElement.Size != null)
                    {
                        modifiedElement.Location = new Point(
                            ScreenCenter.X - (element.ActualBounds.Width / 2),
                            distanceFromTop);
                        distanceFromTop += element.ActualBounds.Height;
                        distanceFromTop += ElementSpacing;
                    }
                    else
                    {
                        modifiedElement.Location = new Point(ScreenCenter.X, distanceFromTop);
                    }
                }

                if (modifiedElement.ControlAlignment == Alignment.Left ||
                    modifiedElement.ControlAlignment == Alignment.Right)
                {
                    // TODO: Implement left and right control alignments
                    throw new NotImplementedException();
                }
            }
#if HAS_CONSOLE && LOG_GENERAL
            Console.WriteLine(String.Format("Updated scene layout: {0}", SceneName));
#endif
        }
    }
}
