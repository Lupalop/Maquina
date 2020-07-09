using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

namespace Maquina.UI
{
    // TODO: Convert to use UI controls once API reaches level 1.
    //
    //       Entities (and its subclasses) are constantly changed
    //       at this point and converting this now would break
    //       the overlay frequently.
    //
    //       The plan is wait until the UI code stabilizes.
    public class DebugOverlay : Overlay
    {
        // FPS+O Counter
        bool[] isCounterVisible = new bool[5];
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        
        // Scene Manager Info
        string sceneInfoHeader = "\nScene Manager Information";
        string sceneCurrentHeader = "\nCurrent Scene: {0}";
        string sceneOverlayHeader = "\nOverlay Scenes ({0}):\n";
        string sceneOverlayList = "";
        string sceneObjectHeader = "\nEntities in Current Scene ({0}):\n";
        string sceneObjectList;
        string globalTimerHeader = "\nRegistered Timers:\n";
        string globalTimerList;

        // Mouse Coords
        string mouseCoordinates;

        public DebugOverlay() : base("Debug Overlay")
        {
#if MGE_LOGGING
            // Listen to window focus events
            Game.Activated += delegate
            {
                LogManager.Info(0, "Window Focus: Gained");
            };
            Game.Deactivated += delegate
            {
                LogManager.Info(0, "Window Focus: Lost");
            };
#endif
        }

        public override void Update()
        {
            // FPS Counter
            if (Application.Input.KeyPressed(Keys.F2))
                isCounterVisible[0] = !isCounterVisible[0];
            if (Application.Input.KeyPressed(Keys.F10))
                isCounterVisible[1] = !isCounterVisible[1];
            if (Application.Input.KeyPressed(Keys.F11))
                isCounterVisible[2] = !isCounterVisible[2];
            if (Application.Input.KeyPressed(Keys.F12))
                isCounterVisible[3] = !isCounterVisible[3];
            if (Application.Input.KeyPressed(Keys.F7))
                isCounterVisible[4] = !isCounterVisible[4];

            // Scale controls
            if (Application.Input.KeyPressed(Keys.F9))
                Application.Display.Scale += 0.1f;
            if (Application.Input.KeyPressed(Keys.F8))
                Application.Display.Scale -= 0.1f;

            elapsedTime += Application.GameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            // List mouse coordinates
            if (isCounterVisible[3])
            {
                mouseCoordinates = Application.Input.MousePosition.ToString();
            }

            // List Overlays currently loaded
            if (isCounterVisible[2])
            {
                sceneOverlayList = "";
                for (int i = 0; i < Application.Scenes.Overlays.Count; i++)
                {
                    sceneOverlayList += string.Format("Index {0}: Scene Name: {1} \n",
                        i, Application.Scenes.Overlays[i].Name);
                }
            }
            // List loaded entities
            if (isCounterVisible[1])
            {
                sceneObjectList = ListEntitiesFromCollection(Application.Scenes.CurrentScene.Entities);
            }
            // List timers
            if (isCounterVisible[4])
            {
                globalTimerList = "";
                for (int i = 0; i < TimerManager.Timers.Count; i++)
                {
                    globalTimerList += String.Format("Timer {0}: AutoReset-{1}, Enabled-{2}, Interval-{3} \n",
                        i, TimerManager.Timers[i].AutoReset, TimerManager.Timers[i].Enabled,
                        TimerManager.Timers[i].Interval);
                }
            }
        }

        public string ListEntitiesFromCollection(EntityCollection entities, bool isContainer = false)
        {
            string list = "";
            if (isContainer)
            {
                list += "== START CONTAINER ENTITIES ==\n";
            }
            for (int i = 0; i < entities.Count; i++)
            {
                list += string.Format(
                    "Index {0}: {1}, ID: {2}, Name: {3}, Bounds: {4} \n",
                    i,
                    entities[i],
                    entities[i].Id,
                    entities[i].Name,
                    entities[i].ActualBounds.ToString());
                if (entities[i] is IContainer)
                {
                    IContainer container = (IContainer)entities[i];
                    list += ListEntitiesFromCollection(container.Children, true);
                }
            }
            if (isContainer)
            {
                list += "== END CONTAINER ENTITIES ==\n";
            }
            return list;
        }

        public override void Draw()
        {
            // FPS Counter
            frameCounter++;

            SpriteBatch.Begin();
            if (isCounterVisible[0])
            {
                string dbCounter = string.Format("FPS: {0}, Memory: {1}, Overlay scenes: {2}", frameRate, GC.GetTotalMemory(false), Application.Scenes.Overlays.Count);
                SpriteBatch.DrawString((SpriteFont)ContentFactory.TryGetResource("o-default"), dbCounter, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[1])
            {
                string objectInfo = string.Format(sceneObjectHeader, Application.Scenes.CurrentScene.Entities.Count) +
                                    sceneObjectList;
                SpriteBatch.DrawString((SpriteFont)ContentFactory.TryGetResource("o-default"), objectInfo, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[2])
            {
                string sceneManagerInfo = sceneInfoHeader + 
                    string.Format(sceneCurrentHeader, Application.Scenes.CurrentScene.Name) + 
                    string.Format(sceneOverlayHeader, Application.Scenes.Overlays.Count) + 
                    sceneOverlayList;
                SpriteBatch.DrawString((SpriteFont)ContentFactory.TryGetResource("o-default"), sceneManagerInfo, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[3])
            {
                SpriteBatch.DrawString((SpriteFont)ContentFactory.TryGetResource("o-default"), mouseCoordinates, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[4])
            {
                string timerManagerInfo = globalTimerHeader + globalTimerList;
                SpriteBatch.DrawString((SpriteFont)ContentFactory.TryGetResource("o-default"), timerManagerInfo, new Vector2(0, 0), Color.White);
            }
            SpriteBatch.End();
        }
    }
}
