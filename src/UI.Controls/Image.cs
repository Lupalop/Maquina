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
    public class Image : Control
    {
        public Image(string name) : base(name)
        {
            Id = "UI_IMAGE";
            DrawController = new DrawController();
        }

        private Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Bounds = _texture.Bounds;
            }
        }

        public DrawController DrawController { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                ActualBounds,
                DrawController.SourceRectangle,
                DrawController.Tint * DrawController.Opacity,
                DrawController.Rotation,
                DrawController.Origin,
                DrawController.SpriteEffects,
                DrawController.LayerDepth);
        }
    }
}
