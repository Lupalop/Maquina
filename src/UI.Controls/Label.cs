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
    public class Label : Control
    {
        public Label(string name) : base (name)
        {
            Id = "UI_LABEL";
            DrawController.Text = "";
        }

        public TextDrawController DrawController { get; set; }

        public override Point Location
        {
            get { return DrawController.Location; }
            set
            {
                DrawController.Location = value;
                base.Location = value;
            }
        }

        public override Point Size
        {
            get { return DrawController.Size; }
            set
            {
                DrawController.Size = value;
                base.Size = value;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                DrawController.Font,
                DrawController.Text,
                DrawController.Location.ToVector2(),
                DrawController.Tint * DrawController.Opacity,
                DrawController.Rotation,
                DrawController.Origin,
                ActualScale,
                DrawController.SpriteEffects,
                DrawController.LayerDepth);
        }
    }
}
