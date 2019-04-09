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
    public abstract class SceneBase
    {
        public SceneBase(string sceneName = "Untitled Scene")
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
            Objects = new Dictionary<string, GenericElement>();
            ScreenCenter = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            // Layout stuff
            ObjectSpacing = 5;
        }

        protected SceneManager SceneManager { get; private set; }
        protected InputManager InputManager { get; private set; }
        protected Game Game { get; private set; }
        protected SpriteBatch SpriteBatch { get; private set; }
        protected Dictionary<string, SpriteFont> Fonts { get; private set; }

        public Dictionary<string, GenericElement> Objects { get; set; }
        public string SceneName { get; private set; }

        public Vector2 ScreenCenter { get; private set; }

        public virtual void LoadContent()
        {
#if HAS_CONSOLE && LOG_VERBOSE
            Console.WriteLine("Loading content in: {0}", SceneName);
#endif
        }

        public virtual void DelayLoadContent()
        {
#if HAS_CONSOLE && LOG_VERBOSE
            Console.WriteLine("Loading delayed content in: {0}", SceneName);
#endif
        }

        public virtual void Draw(GameTime gameTime) { }

        public virtual void Update(GameTime gameTime)
        {
            ScreenCenter = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            if (!IsFirstUpdateDone)
                IsFirstUpdateDone = true;
        }

        public virtual void Unload()
        {
#if HAS_CONSOLE && LOG_VERBOSE
            Console.WriteLine("Unloading from scene: {0}", SceneName);
#endif
        }

        public virtual int GetAllObjectsHeight(IDictionary<string, GenericElement> objects)
        {
            return GetAllObjectsHeight(objects.Values);
        }
        public virtual int GetAllObjectsHeight(IEnumerable<GenericElement> objects)
        {
            int ObjectsHeight = 0;

            for (int i = 0; i < objects.Count(); i++)
            {
                var currentObject = objects.ElementAt(i);
                if (!(currentObject is GuiElement) || currentObject == null)
                {
                    continue;
                }

                currentObject.UpdatePoints();
                if (currentObject.OnUpdate != null)
                {
                    currentObject.OnUpdate(currentObject);
                }

                GuiElement Object = (GuiElement)currentObject;
                if (Object.ControlAlignment == ControlAlignment.Center)
                {
                    ObjectsHeight += currentObject.Bounds.Height;
                }
            }
            return ObjectsHeight;
        }

        public virtual void DrawObjects(GameTime gameTime, IDictionary<string, GenericElement> objects)
        {
            DrawObjects(gameTime, objects.Values);
        }
        public virtual void DrawObjects(GameTime gameTime, IEnumerable<GenericElement> objects)
        {
            if (IsFirstUpdateDone)
            {
                // Draw objects in the Object array
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
        }

        public virtual void UpdateObjects(GameTime gameTime, IDictionary<string, GenericElement> objects)
        {
            UpdateObjects(gameTime, objects.Values);
        }
        public int ObjectSpacing { get; set; }
        public virtual void UpdateObjects(GameTime gameTime, IEnumerable<GenericElement> objects)
        {
            int distanceFromTop = (int)(ScreenCenter.Y - (GetAllObjectsHeight(objects) / 2));

            for (int i = 0; i < objects.Count(); i++)
            {
                GenericElement Object = objects.ElementAt(i);
                if (!(Object is GuiElement))
                {
                    Object.Update(gameTime);
                    continue;
                }

                var ModifiedObject = (GuiElement)Object;
                if (ModifiedObject.ControlAlignment == ControlAlignment.Center)
                {
                    if (ModifiedObject.Graphic != null || ModifiedObject.Dimensions != null)
                    {
                        ModifiedObject.Location = new Vector2(ScreenCenter.X - (Object.Bounds.Width / 2), distanceFromTop);
                        distanceFromTop += Object.Bounds.Height;
                        distanceFromTop += ObjectSpacing;
                    }
                    else
                    {
                        ModifiedObject.Location = new Vector2(ScreenCenter.X, distanceFromTop);
                    }
                }

                if (ModifiedObject.ControlAlignment == ControlAlignment.Left ||
                    ModifiedObject.ControlAlignment == ControlAlignment.Right)
                {
                    throw new NotImplementedException();
                }

                ModifiedObject.Update(gameTime);
            }
        }
        private bool IsFirstUpdateDone = false;
    }
}
