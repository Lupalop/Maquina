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
            MouseEntity = new Image("mouse");
            MouseEntity.Sprite.SpriteType = SpriteType.Static;
            MouseEntity.Sprite.Rows = 1;
            MouseEntity.Sprite.Columns = 2;
            MouseEntity.EntityUpdated += (sender, e) =>
            {
                MouseEntity.Location = Application.Input.MousePosition;
                MouseEntity.Sprite.CurrentFrame = 0;

                // Change state when selected
                if (Application.Input.MouseDown(MouseButton.Left) ||
                    Application.Input.MouseDown(MouseButton.Right) ||
                    Application.Input.MouseDown(MouseButton.Middle))
                {
                    MouseEntity.Sprite.CurrentFrame = 1;
                }
            };
        }

        public static Image MouseEntity;
        public static Texture2D MouseSprite
        {
            get { return MouseEntity.Sprite.Texture; }
            set { MouseEntity.Sprite.Texture = value; }
        }
        public static BlendState BlendState { get; set; }

        public static void Draw()
        {
            Application.SpriteBatch.Begin(default(SpriteSortMode), BlendState);
            MouseEntity.Draw();
            Application.SpriteBatch.End();
        }

        public static void Update()
        {
            MouseEntity.Update();
        }
    }
}
