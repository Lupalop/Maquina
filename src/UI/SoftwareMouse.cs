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
    public class SoftwareMouse : Entity
    {
        public SoftwareMouse() : base("SoftwareMouse")
        {
            DrawController = new DrawController();
        }

        public DrawController DrawController;

        private Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Size = AtlasUtils.GetFrameSize(
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

        public BlendState BlendState { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Application.SpriteBatch.Begin(default(SpriteSortMode), BlendState);
            Application.SpriteBatch.Draw(
                Texture,
                ActualBounds,
                DrawController.SourceRectangle,
                DrawController.Tint * DrawController.Opacity,
                DrawController.Rotation,
                DrawController.Origin,
                DrawController.SpriteEffects,
                DrawController.LayerDepth);
            Application.SpriteBatch.End();
        }

        public override void Update()
        {
            DrawController.SourceRectangle = AtlasUtils.CreateSourceFrameRectangle(
                    _texture.Bounds.Size,
                    2,
                    1,
                    0);
            Location = Application.Input.MousePosition;

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
