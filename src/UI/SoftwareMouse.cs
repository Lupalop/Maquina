using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

namespace Maquina.UI
{
    public static class SoftwareMouse
    {
        static SoftwareMouse()
        {
            MouseSprite = new Sprite();
            MouseSprite.SpriteType = SpriteType.Static;
            MouseSprite.Rows = 1;
            MouseSprite.Columns = 2;
        }

        public static Sprite MouseSprite;

        public static Texture2D MouseTexture
        {
            get { return MouseSprite.Texture; }
            set { MouseSprite.Texture = value; }
        }

        public static BlendState BlendState { get; set; }

        public static void Draw()
        {
            Application.SpriteBatch.Begin(default(SpriteSortMode), BlendState);
            MouseSprite.Draw();
            Application.SpriteBatch.End();
        }

        public static void Update()
        {
            MouseSprite.Update();
            MouseSprite.CurrentFrame = 0;
            MouseSprite.Location = Application.Input.MousePosition;

            if (Application.Input.MouseDown(MouseButton.Left) ||
                Application.Input.MouseDown(MouseButton.Right) ||
                Application.Input.MouseDown(MouseButton.Middle))
            {
                MouseSprite.CurrentFrame = 1;
            }
        }
    }
}
