using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI
{
    // TODO: Convert to use UI elements once API reaches level 1.
    //
    //       Elements (and associated properties) are constantly changed
    //       at this point and converting this now would break
    //       the overlay frequently.
    //
    //       The plan is to convert this into a window once that component lands.
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
        string sceneObjectHeader = "\nElements in Current Scene ({0}):\n";
        string sceneObjectList;
        string globalTimerHeader = "\nRegistered Timers:\n";
        string globalTimerList;

        // Mouse Coords
        string mouseCoordinates;

        public DebugOverlay() : base("Debug Overlay")
        {
#if LOG_ENABLED
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

        public override void Update(GameTime gameTime)
        {
            // FPS Counter
            if (Global.Input.KeyPressed(Keys.F2))
                isCounterVisible[0] = !isCounterVisible[0];
            if (Global.Input.KeyPressed(Keys.F10))
                isCounterVisible[1] = !isCounterVisible[1];
            if (Global.Input.KeyPressed(Keys.F11))
                isCounterVisible[2] = !isCounterVisible[2];
            if (Global.Input.KeyPressed(Keys.F12))
                isCounterVisible[3] = !isCounterVisible[3];
            if (Global.Input.KeyPressed(Keys.F7))
                isCounterVisible[4] = !isCounterVisible[4];

            // Scale controls
            if (Global.Input.KeyPressed(Keys.F9))
                Global.Display.Scale += 0.1f;
            if (Global.Input.KeyPressed(Keys.F8))
                Global.Display.Scale -= 0.1f;

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            // List mouse coordinates
            if (isCounterVisible[3])
            {
                mouseCoordinates = Global.Input.MousePosition.ToString();
            }

            // List Overlays currently loaded
            if (isCounterVisible[2])
            {
                sceneOverlayList = "";
                for (int i = 0; i < Global.Scenes.Overlays.Count; i++)
                {
                    List<string> keyList = Global.Scenes.Overlays.Keys.ToList();
                    sceneOverlayList += String.Format("Key {0}: {2}, Scene Name: {1} \n",
                        i, Global.Scenes.Overlays[keyList[i]].Name, keyList[i]);
                }
            }
            // List elements loaded
            if (isCounterVisible[1])
            {
                sceneObjectList = ListElementsFromDictionary(Global.Scenes.CurrentScene.Elements);
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

        public string ListElementsFromDictionary(IDictionary<string, BaseElement> elements, bool isContainer = false)
        {
            List<string> keyList = elements.Keys.ToList();
            string list = "";
            if (isContainer)
            {
                list += "== START CONTAINER ELEMENTS ==\n";
            }
            for (int i = 0; i < elements.Count; i++)
            {
                list += String.Format(
                    "Key {0}: {1}, ID: {2}, Name: {3}, Bounds: {4} \n",
                    i,
                    keyList[i],
                    elements[keyList[i]].Id,
                    elements[keyList[i]].Name,
                    elements[keyList[i]].ActualBounds.ToString());
                if (elements[keyList[i]] is IContainerElement)
                {
                    IContainerElement container = (IContainerElement)elements[keyList[i]];
                    list += ListElementsFromDictionary(container.Children, true);
                }
            }
            if (isContainer)
            {
                list += "== END CONTAINER ELEMENTS ==\n";
            }
            return list;
        }

        public override void Draw(GameTime gameTime)
        {
            // FPS Counter
            frameCounter++;

            SpriteBatch.Begin();
            if (isCounterVisible[0])
            {
                string dbCounter = string.Format("FPS: {0}, Memory: {1}, Overlay scenes: {2}", frameRate, GC.GetTotalMemory(false), Global.Scenes.Overlays.Count);
                SpriteBatch.DrawString(Global.Fonts["o-default"], dbCounter, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[1])
            {
                string objectInfo = string.Format(sceneObjectHeader, Global.Scenes.CurrentScene.Elements.Count) +
                                    sceneObjectList;
                SpriteBatch.DrawString(Global.Fonts["o-default"], objectInfo, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[2])
            {
                string sceneManagerInfo = sceneInfoHeader + 
                    string.Format(sceneCurrentHeader, Global.Scenes.CurrentScene.Name) + 
                    string.Format(sceneOverlayHeader, Global.Scenes.Overlays.Count) + 
                    sceneOverlayList;
                SpriteBatch.DrawString(Global.Fonts["o-default"], sceneManagerInfo, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[3])
            {
                SpriteBatch.DrawString(Global.Fonts["o-default"], mouseCoordinates, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[4])
            {
                string timerManagerInfo = globalTimerHeader + globalTimerList;
                SpriteBatch.DrawString(Global.Fonts["o-default"], timerManagerInfo, new Vector2(0, 0), Color.White);
            }
            SpriteBatch.End();
        }
    }
}
