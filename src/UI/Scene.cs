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
            SceneName = sceneName;
            // Create local references to global properties
            SceneManager = Global.Scenes;
            InputManager = Global.Input;
            Game = Global.Game;
            SpriteBatch = Global.SpriteBatch;
            Fonts = Global.Fonts;
            // 
            Elements = new Dictionary<string, BaseElement>();
            // Layout stuff
            IsFirstUpdateDone = false;
        }

        private void DisplayManager_ResolutionChanged(Rectangle obj)
        {
            UpdateLayout(Elements);
        }

        private void Global_ScaleChanged(object sender, float e)
        {
            UpdateLayout(Elements);
        }

        protected SceneManager SceneManager { get; private set; }
        protected InputManager InputManager { get; private set; }
        protected Game Game { get; private set; }
        protected SpriteBatch SpriteBatch { get; private set; }
        protected Dictionary<string, SpriteFont> Fonts { get; private set; }

        private IDictionary<string, BaseElement> elements;
        public IDictionary<string, BaseElement> Elements
        {
            get { return elements; }
            set
            {
                elements = value;
                UpdateLayout(Elements);
            }
        }
        public string SceneName { get; private set; }
        private bool IsFirstUpdateDone;

        public event EventHandler LoadContentFinished;
        public event EventHandler UnloadFinished;

        protected Rectangle WindowBounds
        {
            get
            {
                return Global.Display.WindowBounds;
            }
        }

        public virtual void LoadContent()
        {
#if LOG_ENABLED
            LogManager.Info(0, string.Format("Content loaded from: {0}", SceneName));
#endif
            if (LoadContentFinished != null)
            {
                LoadContentFinished(this, EventArgs.Empty);
            }
            // Listen to resolution change events in order to update layout when needed
            Global.Display.ResolutionChanged += DisplayManager_ResolutionChanged;
            Global.ScaleChanged += Global_ScaleChanged;
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
            Global.Display.ResolutionChanged -= DisplayManager_ResolutionChanged;
            Global.ScaleChanged -= Global_ScaleChanged;
#if LOG_ENABLED
            LogManager.Info(0, string.Format("Unloaded content from scene: {0}", SceneName));
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
            // Draw elements in the element array
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

        public virtual void UpdateElements(GameTime gameTime, IDictionary<string, BaseElement> elements)
        {
            UpdateElements(gameTime, elements.Values);
        }
        public virtual void UpdateElements(GameTime gameTime, IEnumerable<BaseElement> elements)
        {
            for (int i = 0; i < elements.Count(); i++)
            {
                BaseElement element = elements.ElementAt(i);
                element.Update(gameTime);
            }
        }

        public virtual void UpdateLayout(IDictionary<string, BaseElement> elements)
        {
            UpdateLayout(elements.Values);
        }
        public virtual void UpdateLayout(IEnumerable<BaseElement> elements)
        {
            for (int i = 0; i < elements.Count(); i++)
            {
                BaseElement element = elements.ElementAt(i);

                // Recurse if element is a container
                if (element is IContainerElement)
                {
                    IContainerElement container = (IContainerElement)element;
                    UpdateLayout(container.Children);
                }

                // Ignore non-UI elements
                if (element is GuiElement)
                {
                    GuiElement modifiedElement = (GuiElement)element;
                    
                    // Ignore elements that are not requesting auto position
                    if (!modifiedElement.AutoPosition)
                    {
                        return;
                    }

                    int modifiedElementX = modifiedElement.Location.X;
                    int modifiedElementY = modifiedElement.Location.Y;

                    switch (modifiedElement.HorizontalAlignment)
                    {
                        case Alignment.Left:
                            modifiedElementX = WindowBounds.Left;
                            break;
                        case Alignment.Center:
                            if (element.ActualBounds.Width != 0)
                                modifiedElementX = WindowBounds.Center.X - (element.ActualBounds.Width / 2);
                            break;
                        case Alignment.Right:
                            modifiedElementX = WindowBounds.Right - element.ActualBounds.Width;
                            break;
                        case Alignment.Fixed:
                            break;
                    }

                    switch (modifiedElement.VerticalAlignment)
                    {
                        case Alignment.Left:
                            modifiedElementY = WindowBounds.Center.Y;
                            break;
                        case Alignment.Center:
                            if (element.ActualBounds.Height != 0)
                                modifiedElementY = WindowBounds.Center.Y - (element.ActualBounds.Height / 2);
                            break;
                        case Alignment.Right:
                            modifiedElementY = WindowBounds.Center.Y;
                            break;
                        case Alignment.Fixed:
                            break;
                    }

                    modifiedElement.Location = new Point(modifiedElementX, modifiedElementY);
                }
            }
#if LOG_ENABLED
            LogManager.Info(0, string.Format("Updated scene layout: {0}", SceneName));
#endif
        }
    }
}
