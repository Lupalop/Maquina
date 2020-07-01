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
            DrawController = new DrawController();
        }

        public static DrawController DrawController;

        private static Texture2D _texture;
        public static Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                DrawController.Size = AtlasUtils.GetFrameSize(
                    _texture.Bounds.Size,
                    2,
                    1);
                DrawController.SourceRectangle = AtlasUtils.CreateSourceFrameRectangle(
                    _texture.Bounds.Size,
                    2,
                    1,
                    0);
            }
        }

        public static BlendState BlendState { get; set; }

        public static void Draw()
        {
            Application.SpriteBatch.Begin(default(SpriteSortMode), BlendState);
            Application.SpriteBatch.Draw(
                Texture,
                DrawController.DestinationRectangle,
                DrawController.SourceRectangle,
                DrawController.Tint * DrawController.Opacity,
                DrawController.Rotation,
                DrawController.Origin,
                DrawController.SpriteEffects,
                DrawController.LayerDepth);
            Application.SpriteBatch.End();
        }

        public static void Update()
        {
            DrawController.SourceRectangle = AtlasUtils.CreateSourceFrameRectangle(
                    _texture.Bounds.Size,
                    2,
                    1,
                    0);
            DrawController.Location = Application.Input.MousePosition;

            if (Application.Input.MouseDown(MouseButton.Left) ||
                Application.Input.MouseDown(MouseButton.Right) ||
                Application.Input.MouseDown(MouseButton.Middle))
            {
                DrawController.SourceRectangle = AtlasUtils.CreateSourceFrameRectangle(
                    _texture.Bounds.Size,
                    2,
                    1,
                    1);
            }
        }
    }
}
