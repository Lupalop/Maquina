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
            SpriteBatch = Global.SpriteBatch;
            SourceRectangle = null;
            SpriteType = SpriteType.None;

            DelayInterval = 100;
            Table = new Point(1, 1);
        }

        // General
        private Texture2D graphic;
        public Texture2D Graphic
        {
            get { return graphic; }
            set
            {
                graphic = value;
                UpdateDestinationRectangle();
            }
        }

        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 RotationOrigin { get; set; }
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
                return Scale * Global.Scale;
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
                if (DestinationRectangleChanged != null)
                {
                    DestinationRectangleChanged(value);
                }
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
                if (LocationChanged != null)
                {
                    LocationChanged(value);
                }
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
                if (SizeChanged != null)
                {
                    SizeChanged(value);
                }
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

        public int Rows
        {
            get
            {
                return Table.Y;
            }
            set
            {
                TotalFrames = value * Table.Y;
                table.Y = value;
            }
        }

        public int Columns
        {
            get
            {
                return Table.X;
            }
            set
            {
                TotalFrames = Table.X * value;
                table.X = value;
            }
        }

        /// <summary>
        /// Represents the columns and rows contained by a texture atlas.
        /// X = columns, Y = rows
        /// </summary>
        private Point table;
        public Point Table
        {
            get
            {
                return table;
            }
            set
            {
                table = value;
                TotalFrames = value.X * value.Y;
            }
        }

        // Draw and update methods
        public virtual void Draw(GameTime gameTime)
        {
            if (Graphic != null)
            {
                SpriteBatch.Draw(Graphic, ActualDestinationRectangle,
                    SourceRectangle, Tint, Rotation, RotationOrigin,
                    SpriteEffects, LayerDepth);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (SpriteType != SpriteType.None && CurrentFrame == TotalFrames)
            {
                CurrentFrame = 0;
            }

            if (Graphic != null && SpriteType != SpriteType.None)
            {
                int width = Graphic.Width / Columns;
                int height = Graphic.Height / Rows;
                int row = (int)((float)CurrentFrame / (float)Columns);
                int column = CurrentFrame % Columns;

                SourceRectangle = new Rectangle(
                    width * column, height * row, width, height);
            }
        }

        public void UpdateDestinationRectangle()
        {
            if (Graphic == null)
            {
                return;
            }
            if (SpriteType != SpriteType.None)
            {
                Size = new Point(
                    Graphic.Width / Columns, Graphic.Height / Rows);
                return;
            }
            Size = Graphic.Bounds.Size;
        }

        // Child Events
        public event Action<Rectangle> DestinationRectangleChanged;
        public event Action<Point> LocationChanged;
        public event Action<Point> SizeChanged;

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
