using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Elements
{
    public class TextSprite : ISprite
    {
        public TextSprite()
        {
            Tint = Color.White;
            Font = Global.Fonts["default"];
            Label = "";
            Scale = 1;
            LayerDepth = 1f;
            SpriteBatch = Global.SpriteBatch;
        }

        // General
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 RotationOrigin { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public bool IgnoreGlobalScale { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        private SpriteFont font;
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                font = value;
                UpdateTextMeasurement();
            }
        }
        
        // Scale
        public float Scale { get; set; }
        // Scale adjusted for global scale
        public float ActualScale
        {
            get
            {
                if (IgnoreGlobalScale)
                {
                    return Scale;
                }
                return Scale * Global.Display.Scale;
            }
        }

        private string label;
        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                UpdateTextMeasurement();
            }
        }

        // Layout
        // Destination rectangle not adjusted for scale
        public Rectangle DestinationRectangle
        {
            get { return new Rectangle(Location, Size); }
            set
            {
                Location = value.Location;
                Size = value.Size;
                OnSpriteChanged(new ElementChangedEventArgs(ElementChangedProperty.DestinationRectangle, value));
            }
        }
        // Destination rectangle adjusted for scale
        public Rectangle ActualDestinationRectangle
        {
            get { return new Rectangle(Location, ActualSize); }
        }
        // Source rectangle
        public Rectangle? SourceRectangle
        {
            get { return null; }
            set { return; }
        }
        // Location
        private Point location;
        public Point Location
        {
            get { return location; }
            set
            {
                if (location == value)
                {
                    return;
                }
                location = value;
                OnSpriteChanged(new ElementChangedEventArgs(ElementChangedProperty.Location, value, Point.Zero));
            }
        }
        // Size not adjusted for scale
        private Point size;
        public Point Size
        {
            get { return size; }
            set
            {
                if (size == value)
                {
                    return;
                }
                size = value;
                OnSpriteChanged(new ElementChangedEventArgs(ElementChangedProperty.Size, Point.Zero, value));
            }
        }
        // Size adjusted for scale
        public Point ActualSize
        {
            get
            {
                return new Point(
                    (int)(size.X * ActualScale),
                    (int)(size.Y * ActualScale));
            }
        }
        public float LayerDepth { get; set; }

        // Draw and update methods
        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.DrawString(Font, Label, Location.ToVector2(), Tint,
                Rotation, RotationOrigin, ActualScale, SpriteEffects, LayerDepth);
        }

        public virtual void Update(GameTime gameTime)
        {
            // Do nothing
        }

        public void UpdateTextMeasurement()
        {
            if (font != null && label != null)
            {
                Size = font.MeasureString(Label).ToPoint();
            }
        }

        // Child Events
        public event ElementChangedEventHandler SpriteChanged;
        protected void OnSpriteChanged(ElementChangedEventArgs e)
        {
            if (SpriteChanged != null)
            {
                SpriteChanged(this, e);
            }
        }
    }
}
