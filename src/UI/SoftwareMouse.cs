using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI
{
    public static class SoftwareMouse
    {
        static SoftwareMouse()
        {
            MouseElement = new Image("mouse")
            {
                SpriteType = SpriteType.Static,
                ControlAlignment = Alignment.Fixed,
                Rows = 1,
                Columns = 2,
                OnUpdate = (element) =>
                {
                    element.Location = Global.InputManager.MousePosition;
                    element.Scale = Global.Scale;
                    element.CurrentFrame = 0;
                    // Change state when selected
                    if ((Global.InputManager.MouseDown(MouseButton.Left) ||
                            Global.InputManager.MouseDown(MouseButton.Right) ||
                            Global.InputManager.MouseDown(MouseButton.Middle)) &&
                        Global.InputManager.ShouldAcceptInput)
                    {
                        element.CurrentFrame = 1;
                    }
                }
            };
        }

        public static Image MouseElement;
        
        public static Texture2D MouseSprite
        {
            get
            {
                return MouseElement.Graphic;
            }
            set
            {
#if HAS_CONSOLE && LOG_GENERAL
            Console.WriteLine("Mouse: sprite changed");
#endif
                MouseElement.Graphic = value;
            }
        }

        public static void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Begin();
            MouseElement.Draw(gameTime);
            Global.SpriteBatch.End();
        }

        public static void Update(GameTime gameTime)
        {
            MouseElement.Update(gameTime);
        }
    }
}
