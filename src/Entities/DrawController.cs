using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maquina.Entities
{
    public class DrawController
    {
        public DrawController()
        {
            Tint = Color.White;
            Rotation = 0;
            Origin = Vector2.Zero;
            SpriteEffects = SpriteEffects.None;
            LayerDepth = 1;
            Opacity = 1;
        }

        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public float LayerDepth { get; set; }
        public float Opacity { get; set; }
    }
}
