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
    public class DebugOverlay : Overlay
    {
        // FPS+O Counter
        bool[] isCounterVisible = new bool[4];
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

        // Mouse Coords
        string mouseCoordinates;

        public DebugOverlay() : base("Debug Overlay")
        {
            // Listen to window focus events
            Game.Activated += delegate
            {
                Console.WriteLine("Window Focus: Gained");
            };
            Game.Deactivated += delegate
            {
                Console.WriteLine("Window Focus: Lost");
            };
        }

        public override void Update(GameTime gameTime)
        {
            // FPS Counter
            if (InputManager.KeyPressed(Keys.F2))
                isCounterVisible[0] = !isCounterVisible[0];
            if (InputManager.KeyPressed(Keys.F10))
                isCounterVisible[1] = !isCounterVisible[1];
            if (InputManager.KeyPressed(Keys.F11))
                isCounterVisible[2] = !isCounterVisible[2];
            if (InputManager.KeyPressed(Keys.F12))
                isCounterVisible[3] = !isCounterVisible[3];

            // Scale controls
            if (InputManager.KeyPressed(Keys.F9))
                Global.Scale += 0.1f;
            if (InputManager.KeyPressed(Keys.F8))
                Global.Scale -= 0.1f;

            // Refresh Scene (similar to a browser)
            if (InputManager.KeyPressed(Keys.F5))
                SceneManager.SwitchToScene(SceneManager.CurrentScene, true);

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
                mouseCoordinates = SceneManager.Overlays["mouse"].Objects["Mouse"].Location.ToString();
            }

            // List Overlays currently loaded
            if (isCounterVisible[2])
            {
                sceneOverlayList = "";
                for (int i = 0; i < SceneManager.Overlays.Count; i++)
                {
                    List<string> keyList = SceneManager.Overlays.Keys.ToList();
                    sceneOverlayList += String.Format("Key {0}: {2}, Scene Name: {1} \n",
                        new object[] { i, SceneManager.Overlays[keyList[i]].SceneName, keyList[i] });
                }
            }
            // List objects loaded
            if (isCounterVisible[1])
            {
                sceneObjectList = ListElementsFromDictionary(SceneManager.CurrentScene.Objects);
            }
        }

        public string ListElementsFromDictionary(Dictionary<string, GenericElement> elements, bool isContainer = false)
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
                    elements[keyList[i]].ID,
                    elements[keyList[i]].Name,
                    elements[keyList[i]].Bounds.ToString());
                if (elements[keyList[i]].ID == "GUI_CONTAINER")
                {
                    ElementContainer container = (ElementContainer)elements[keyList[i]];
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
            if (isCounterVisible[2])
            {
                string sceneManagerInfo = sceneInfoHeader + 
                    string.Format(sceneCurrentHeader, SceneManager.CurrentScene.SceneName) + 
                    string.Format(sceneOverlayHeader, SceneManager.Overlays.Count) + 
                    sceneOverlayList;
                SpriteBatch.DrawString(Fonts["o-default"], sceneManagerInfo, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[1])
            {
                string objectInfo = string.Format(sceneObjectHeader, SceneManager.CurrentScene.Objects.Count) +
                                    sceneObjectList;
                SpriteBatch.DrawString(Fonts["o-default"], objectInfo, new Vector2(0, 0), Color.White);
            }

            if (isCounterVisible[0])
            {
                string dbCounter = string.Format("FPS: {0}, Memory: {1}, Overlay scenes: {2}", frameRate, GC.GetTotalMemory(false), SceneManager.Overlays.Count);
                SpriteBatch.DrawString(Fonts["o-default"], dbCounter, new Vector2(0, 0), Color.White);
            }
            if (isCounterVisible[3])
            {
                SpriteBatch.DrawString(Fonts["o-default"], mouseCoordinates, new Vector2(0, 0), Color.White);
            }
            SpriteBatch.End();
        }
    }
}
