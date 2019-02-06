using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Maquina.Objects;
using System.Collections.ObjectModel;

namespace Maquina.UI
{
    // TODO: Implement basic layout engine (allowing object groups inside object groups and more...)
    public abstract class SceneBase
    {
        public SceneBase(SceneManager sceneManager, string sceneName = "Untitled Scene")
        {
            if (sceneManager == null)
            {
                throw new ArgumentNullException();
            }
            // Scene name assignment
            this.SceneName = sceneName;
            // Assign values to important variables
            this.SceneManager = sceneManager;
            InputManager = sceneManager.InputManager;
            Game = sceneManager.Game;
            SpriteBatch = sceneManager.SpriteBatch;
            Fonts = sceneManager.Fonts;
            Objects = new Dictionary<string, GenericElement>();
            ScreenCenter = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            // Layout stuff
            ObjectSpacing = 5;
        }

        public SceneManager SceneManager { get; private set; }
        public InputManager InputManager { get; set; }
        public Game Game { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public Dictionary<string, SpriteFont> Fonts { get; set; }
        public Dictionary<string, GenericElement> Objects { get; set; }
        public string SceneName { get; private set; }

        public Vector2 ScreenCenter { get; private set; }

        public virtual void LoadContent()
        {
#if DEBUG
            Console.WriteLine("Loading content in: {0}", SceneName);
#endif
        }

        public virtual void DelayLoadContent()
        {
#if DEBUG
            Console.WriteLine("Loading delayed content in: {0}", SceneName);
#endif
        }

        public virtual void Draw(GameTime gameTime)
        {
#if DEBUG
            if (Platform.VerboseOutput)
                Console.WriteLine("Drawing from scene: {0}", SceneName);
#endif
        }

        public virtual void Update(GameTime gameTime)
        {
#if DEBUG
            if (Platform.VerboseOutput)
                Console.WriteLine("Updating from scene: {0}", SceneName);
#endif
            ScreenCenter = Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            if (!IsFirstUpdateDone)
                IsFirstUpdateDone = true;
        }

        public virtual void Unload()
        {
#if DEBUG
            Console.WriteLine("Unloading from scene: {0}", SceneName);
#endif
        }

        public virtual int GetAllObjectsHeight(Dictionary<string, GenericElement> objects)
        {
            return GetAllObjectsHeightFromArray(objects.Values.ToArray<GenericElement>());
        }
        public virtual int GetAllObjectsHeight(Collection<GenericElement> objects)
        {
            return GetAllObjectsHeightFromArray(objects.ToArray<GenericElement>());
        }
        public virtual int GetAllObjectsHeight(GenericElement[] objects)
        {
            return GetAllObjectsHeightFromArray(objects);
        }
        private int GetAllObjectsHeightFromArray(GenericElement[] objects)
        {
            int ObjectsHeight = 0;

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].UpdatePoints();
                objects[i].OnUpdate();

                if (!(objects[i] is GuiElement))
                {
                    continue;
                }

                GuiElement Object = (GuiElement)objects[i];
                if (Object.ControlAlignment == ControlAlignment.Center)
                {
                    ObjectsHeight += objects[i].Bounds.Height;
                }
            }
            return ObjectsHeight;
        }

        public virtual void DrawObjects(GameTime gameTime, Dictionary<string, GenericElement> objects)
        {
            DrawObjectsFromArray(gameTime, objects.Values.ToArray<GenericElement>());
        }
        public virtual void DrawObjects(GameTime gameTime, Collection<GenericElement> objects)
        {
            DrawObjectsFromArray(gameTime, objects.ToArray<GenericElement>());
        }
        public virtual void DrawObjects(GameTime gameTime, GenericElement[] objects)
        {
            DrawObjectsFromArray(gameTime, objects);
        }
        private void DrawObjectsFromArray(GameTime gameTime, GenericElement[] objs)
        {
            if (IsFirstUpdateDone)
            {
                // Draw objects in the Object array
                for (int i = 0; i < objs.Length; i++)
                {
                    try
                    {
                        objs[i].Draw(gameTime);
                    }
                    catch (NullReferenceException)
                    { 
                        // Suppress errors
                    }
                }
            }
        }

        public virtual void UpdateObjects(GameTime gameTime, Dictionary<string, GenericElement> objects)
        {
            UpdateObjectsFromArray(gameTime, objects.Values.ToArray<GenericElement>());
        }
        public virtual void UpdateObjects(GameTime gameTime, Collection<GenericElement> objects)
        {
            UpdateObjectsFromArray(gameTime, objects.ToArray<GenericElement>());
        }
        public virtual void UpdateObjects(GameTime gameTime, GenericElement[] objects)
        {
            UpdateObjectsFromArray(gameTime, objects);
        }

        public int ObjectSpacing { get; set; }
        private void UpdateObjectsFromArray(GameTime gameTime, GenericElement[] objects)
        {
            int distanceFromTop = (int)(ScreenCenter.Y - (GetAllObjectsHeight(objects) / 2));
            // Dynamically compute for spacing between *centered* objects
            for (int i = 0; i < objects.Length; i++)
            {
                GenericElement Object = objects[i];
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
                ModifiedObject.Update(gameTime);
            }
        }
        private bool IsFirstUpdateDone = false;
    }
}
