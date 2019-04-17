using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Maquina.Elements
{
    public abstract class GenericElement : IBaseElement, IDrawableElement, IDisposable 
    {
        // Constructor
        protected GenericElement(string name)
        {
            Name = name;
            Tint = Color.White;
            Scale = Global.Scale;
            CurrentFrame = 0;
            TotalFrames = 0;
            SpriteType = SpriteType.None;
            GraphicEffects = SpriteEffects.None;
            LayerDepth = 1f;
            SpriteBatch = Global.SpriteBatch;
            FrameSwitchTimer = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = 100
            };
            FrameSwitchTimer.Elapsed += FrameSwitchTimer_Elapsed;
        }

        void FrameSwitchTimer_Elapsed(object sender, EventArgs e)
        {
            if (SpriteType == SpriteType.Animated)
            {
                CurrentFrame++;
            }
        }

        // Basic Properties
        public string Name { get; set; }
        public virtual string ID
        {
            get { return "GENERIC_BASE"; }
        }

        // Graphics
        public Texture2D Graphic { get; set; }
        public Vector2 Location { get; set; }
        public virtual Vector2 Dimensions { get; set; }
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 RotationOrigin { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public SpriteEffects GraphicEffects { get; set; }
        public float LayerDepth { get; set; }

        private Timer FrameSwitchTimer;
        private int frameSwitchInterval;
        public int FrameSwitchInterval
        {
            get
            {
                return frameSwitchInterval;
            }
            set
            {
                FrameSwitchTimer.Interval = value;
                frameSwitchInterval = value;
            }
        }

        private float scale;
        public float Scale
        {
            get
            {
                return scale * Global.Scale;
            }
            set
            {
                scale = value;
            }
        }

        // Update and draw events (essential to removing individual update commands from scenes)
        public Action<GenericElement> OnUpdate { get; set; }
        public Action<GenericElement> OnDraw { get; set; }

        // For Animated Sprites
        public SpriteType SpriteType { get; set; }

        private int rows;
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                TotalFrames = value * Columns;
                rows = value;
            }
        }
        private int columns;
        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                TotalFrames = Rows * value;
                columns = value;
            }
        }

        public int CurrentFrame;
        private int TotalFrames;

        public Rectangle Bounds { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        // O
        public virtual void Draw(GameTime gameTime)
        {
            if (Graphic != null)
            {
                if (DestinationRectangle != Rectangle.Empty)
                {
                    if (SourceRectangle != Rectangle.Empty)
                    {
                        SpriteBatch.Draw(Graphic, DestinationRectangle,
                            SourceRectangle, Tint,
                            Rotation, RotationOrigin,
                            GraphicEffects, LayerDepth);
                    }
                    else
                    {
                        SpriteBatch.Draw(Graphic, DestinationRectangle,
                            null, Tint,
                            Rotation, RotationOrigin,
                            GraphicEffects, LayerDepth);
                    }
                }
                else if (Rotation != 0 || Scale != 0)
                {
                    if (SourceRectangle != Rectangle.Empty)
                    {
                        SpriteBatch.Draw(Graphic, Location,
                            SourceRectangle, Tint,
                            Rotation, RotationOrigin,
                            Scale, GraphicEffects, LayerDepth);
                    }
                    else
                    {
                        SpriteBatch.Draw(Graphic, Location,
                            null, Tint,
                            Rotation, RotationOrigin,
                            Scale, GraphicEffects, LayerDepth);
                    }
                }
                else
                {
                    SpriteBatch.Draw(Graphic, Bounds, Tint);
                }
            }

            if (OnDraw != null)
            {
                OnDraw(this);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (SpriteType != SpriteType.None && CurrentFrame == TotalFrames)
            {
                CurrentFrame = 0;
            }

            UpdatePoints();

            if (OnUpdate != null)
            {
                OnUpdate(this);
            }
        }

        public virtual void UpdatePoints()
        {
            if (Graphic != null)
            {
                if (SpriteType != SpriteType.None)
                {
                    int width = Graphic.Width / Columns;
                    int height = Graphic.Height / Rows;
                    int row = (int)((float)CurrentFrame / (float)Columns);
                    int column = CurrentFrame % Columns;

                    DestinationRectangle = new Rectangle(
                        (int)Location.X, (int)Location.Y,
                        (int)(width * Scale), (int)(height * Scale));
                    SourceRectangle = new Rectangle(
                        width * column,
                        height * row,
                        width, height);
                }

                if (SourceRectangle != Rectangle.Empty)
                {
                    Dimensions = new Vector2(
                        SourceRectangle.Width * Scale,
                        SourceRectangle.Height * Scale);
                }
                else
                {
                    Dimensions = new Vector2(
                        Graphic.Width * Scale,
                        Graphic.Height * Scale);
                }
            }

            // Stripping the decimal parts of the vectors are intentional
            Bounds = new Rectangle(Location.ToPoint(), Dimensions.ToPoint());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Graphic.Dispose();
                FrameSwitchTimer.Close();
            }
        }
    }
}
