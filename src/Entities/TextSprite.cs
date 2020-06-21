using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Entities
{
    public class TextSprite : ISprite
    {
        public TextSprite()
        {
            Tint = Color.White;
            Font = (SpriteFont)ContentFactory.TryGetResource("default");
            Text = "";
            Scale = 1;
            LayerDepth = 1f;
            Opacity = 1;
            SpriteBatch = Application.SpriteBatch;
        }

        // General
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
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
                return Scale * Application.Display.Scale;
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
#if LOCALE_ENABLED
                text = Application.Locale.TryGetString(value);
#else
                text = value;
#endif
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
                OnSpriteChanged(new EntityChangedEventArgs(EntityChangedProperty.DestinationRectangle));
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
                OnSpriteChanged(new EntityChangedEventArgs(EntityChangedProperty.Location));
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
                OnSpriteChanged(new EntityChangedEventArgs(EntityChangedProperty.Size));
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
        public float Opacity { get; set; }

        // Draw and update methods
        public virtual void Draw()
        {
            SpriteBatch.DrawString(Font, Text, Location.ToVector2(), Tint * Opacity,
                Rotation, Origin, ActualScale, SpriteEffects, LayerDepth);
        }

        public virtual void Update()
        {
            // Do nothing
        }

        protected virtual void UpdateTextMeasurement()
        {
            if (font != null && text != null)
            {
                Size = font.MeasureString(Text).ToPoint();
            }
        }

        // Child Events
        public event EventHandler<EntityChangedEventArgs> SpriteChanged;
        protected virtual void OnSpriteChanged(EntityChangedEventArgs e)
        {
            if (SpriteChanged != null)
            {
                SpriteChanged(this, e);
            }
        }
    }
}
