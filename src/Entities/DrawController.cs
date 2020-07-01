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
            SourceRectangle = null;
            Location = Point.Zero;
            Size = Point.Zero;
            Scale = 1;
            LayerDepth = 1;
            Opacity = 1;
        }

        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects SpriteEffects { get; set; }

        public virtual Rectangle DestinationRectangle
        {
            get { return new Rectangle(Location, Size); }
            set
            {
                Location = value.Location;
                Size = value.Size;
            }
        }

        public virtual Rectangle? SourceRectangle { get; set; }
        public Point Location { get; set; }
        public virtual Point Size { get; set; }
        public float Scale { get; set; }
        public float LayerDepth { get; set; }
        public float Opacity { get; set; }
    }
}
