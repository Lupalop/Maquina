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
            MouseElement = new Image("mouse");
            MouseElement.Sprite.SpriteType = SpriteType.Static;
            MouseElement.Sprite.Rows = 1;
            MouseElement.Sprite.Columns = 2;
            MouseElement.ElementUpdated += (sender, e) =>
            {
                MouseElement.Location = Application.Input.MousePosition;
                MouseElement.Sprite.CurrentFrame = 0;

                // Change state when selected
                if (Application.Input.MouseDown(MouseButton.Left) ||
                    Application.Input.MouseDown(MouseButton.Right) ||
                    Application.Input.MouseDown(MouseButton.Middle))
                {
                    MouseElement.Sprite.CurrentFrame = 1;
                }
            };
        }

        public static Image MouseElement;
        public static Texture2D MouseSprite
        {
            get { return MouseElement.Sprite.Graphic; }
            set { MouseElement.Sprite.Graphic = value; }
        }
        public static BlendState BlendState { get; set; }

        public static void Draw()
        {
            Application.SpriteBatch.Begin(default(SpriteSortMode), BlendState);
            MouseElement.Draw();
            Application.SpriteBatch.End();
        }

        public static void Update()
        {
            MouseElement.Update();
        }
    }
}
