using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public class Sprite : ISprite, IDisposable
    {
        public Sprite()
        {
            Tint = Color.White;
            Scale = 1;
            SpriteEffects = SpriteEffects.None;
            LayerDepth = 1f;
            SpriteBatch = Application.SpriteBatch;
            SourceRectangle = null;
            SpriteType = SpriteType.None;
            Opacity = 1;

            DelayInterval = 100;
            Rows = 1;
            Columns = 1;
        }

        // General
        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                UpdateDestinationRectangle();
            }
        }

        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public bool IgnoreGlobalScale { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        
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

        // Layout
        // Destination rectangle not adjusted for scale
        public Rectangle DestinationRectangle
        {
            get { return new Rectangle(Location, Size); }
            set
            {
                Location = value.Location;
                Size = value.Size;
                OnSpriteChanged(new ElementChangedEventArgs(ElementChangedProperty.DestinationRectangle));
            }
        }
        // Destination rectangle adjusted for scale
        public Rectangle ActualDestinationRectangle
        {
            get { return new Rectangle(Location, ActualSize); }
        }
        // Sprite source rectangle (used for texture atlases)
        public Rectangle? SourceRectangle { get; set; }
        // Sprite location
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
                OnSpriteChanged(new ElementChangedEventArgs(ElementChangedProperty.Location));
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
                    //return;
                }
                size = value;
                OnSpriteChanged(new ElementChangedEventArgs(ElementChangedProperty.Size));
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

        // For animated sprites
        private Timer DelayTimer;
        private void DelayTimer_Elapsed(object sender, EventArgs e)
        {
            currentFrame++;
        }

        private int delayInterval;
        public int DelayInterval
        {
            get { return delayInterval; }
            set
            {
                if (DelayTimer != null)
                {
                    DelayTimer.Interval = value;
                }
                delayInterval = value;
            }
        }

        private SpriteType spriteType;
        public SpriteType SpriteType
        {
            get
            {
                return spriteType;
            }
            set
            {
                spriteType = value;
                UpdateDestinationRectangle();
                //
                if (value == SpriteType.Animated)
                {
                    DelayTimer = new Timer
                    {
                        AutoReset = true,
                        Enabled = true,
                        Interval = DelayInterval
                    };
                    DelayTimer.Elapsed += DelayTimer_Elapsed;
                    return;
                }
                //
                if (DelayTimer != null)
                {
                    DelayTimer.Close();
                    DelayTimer = null;
                }
            }
        }

        private int currentFrame;
        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }
        public int TotalFrames { get; private set; }

        private int rows;
        public int Rows
        {
            get { return rows; }
            set
            {
                TotalFrames = value * Columns;
                rows = value;
                UpdateDestinationRectangle();
            }
        }

        private int columns;
        public int Columns
        {
            get { return columns; }
            set
            {
                TotalFrames = Rows * value;
                columns = value;
                UpdateDestinationRectangle();
            }
        }

        // Draw and update methods
        public virtual void Draw()
        {
            if (Texture != null)
            {
                SpriteBatch.Draw(Texture, ActualDestinationRectangle,
                    SourceRectangle, Tint * Opacity, Rotation, Origin,
                    SpriteEffects, LayerDepth);
            }
        }

        public virtual void Update()
        {
            if (SpriteType != SpriteType.None && CurrentFrame == TotalFrames)
            {
                CurrentFrame = 0;
            }

            if (Texture != null && SpriteType != SpriteType.None)
            {
                int width = Texture.Width / Columns;
                int height = Texture.Height / Rows;
                int row = CurrentFrame / Columns;
                int column = CurrentFrame % Columns;

                SourceRectangle = new Rectangle(
                    width * column, height * row, width, height);
            }
        }

        public void UpdateDestinationRectangle()
        {
            if (Texture == null)
            {
                return;
            }
            if (SpriteType != SpriteType.None)
            {
                Size = new Point(
                    Texture.Width / Columns, Texture.Height / Rows);
                return;
            }
            Size = Texture.Bounds.Size;
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


        // IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && DelayTimer != null)
            {
                DelayTimer.Close();
            }
        }
    }
}
