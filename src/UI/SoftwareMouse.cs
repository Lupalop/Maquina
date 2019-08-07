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
                Rows = 1,
                Columns = 2,
            };
            MouseElement.ElementUpdated += (elem) =>
            {
                Image element = (Image)elem;
                element.Location = Global.Input.MousePosition;
                element.Background.CurrentFrame = 0;

                // Change state when selected
                if (Global.Input.MouseDown(MouseButton.Left) ||
                    Global.Input.MouseDown(MouseButton.Right) ||
                    Global.Input.MouseDown(MouseButton.Middle))
                {
                    element.Background.CurrentFrame = 1;
                }
            };
        }

        public static Image MouseElement;
        public static Texture2D MouseSprite
        {
            get { return MouseElement.Graphic; }
            set { MouseElement.Graphic = value; }
        }
        public static BlendState BlendState { get; set; }

        public static void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Begin(default(SpriteSortMode), BlendState);
            MouseElement.Draw(gameTime);
            Global.SpriteBatch.End();
        }

        public static void Update(GameTime gameTime)
        {
            MouseElement.Update(gameTime);
        }
    }
}
