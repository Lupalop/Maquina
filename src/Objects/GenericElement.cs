﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Maquina.Objects
{
    /// <summary>
    /// Base object class where every drawable object inherits its properties
    /// </summary>
    public abstract class GenericElement : IElement, IDisposable
    {
        // Constructor
        protected GenericElement(string name)
        {
            Name = name;
            Tint = Color.White;
            Scale = 1f;
            MessageHolder = new Collection<object>();
            CurrentFrame = 0;
            TotalFrames = 0;
            SpriteType = SpriteType.None;
            GraphicEffects = SpriteEffects.None;
        }

        // Basic Properties
        public string Name { get; set; }
        public Collection<object> MessageHolder { get; set; }

        // Graphics
        public Texture2D Graphic { get; set; }
        public Vector2 Location { get; set; }
        public virtual Vector2 Dimensions { get; set; }
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 RotationOrigin { get; set; }
        public float Scale { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public SpriteEffects GraphicEffects { get; set; }

        // Update and draw events (essential to removing individual update commands from scenes)
        public Action OnUpdate { get; set; }
        public Action OnDraw { get; set; }

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
                if (SpriteType != SpriteType.None)
                {
                    int width = Graphic.Width / Columns;
                    int height = Graphic.Height / Rows;
                    int row = (int)((float)CurrentFrame / (float)Columns);
                    int column = CurrentFrame % Columns;

                    DestinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, width, height);
                    SourceRectangle = new Rectangle(width * column, height * row, width, height);
                }
                if (DestinationRectangle != Rectangle.Empty)
                {
                    if (SourceRectangle != Rectangle.Empty)
                        SpriteBatch.Draw(Graphic, DestinationRectangle, SourceRectangle, Tint, Rotation, RotationOrigin, GraphicEffects, 1f);
                    else
                        SpriteBatch.Draw(Graphic, DestinationRectangle, null, Tint, Rotation, RotationOrigin, GraphicEffects, 1f);
                    return;
                }
                else if (Rotation != 0 || Scale != 0)
                {
                    if (SourceRectangle != Rectangle.Empty)
                        SpriteBatch.Draw(Graphic, Location, SourceRectangle, Tint, Rotation, RotationOrigin, Scale, GraphicEffects, 1f);
                    else
                        SpriteBatch.Draw(Graphic, Location, null, Tint, Rotation, RotationOrigin, Scale, GraphicEffects, 1f);
                    return;
                }
                else
                {
                    SpriteBatch.Draw(Graphic, Bounds, Tint);
                }
            }
            if (OnDraw != null)
                OnDraw();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (SpriteType != SpriteType.None)
            {
                if (SpriteType == SpriteType.Animated)
                    CurrentFrame++;
                if (CurrentFrame == TotalFrames)
                    CurrentFrame = 0;
            }

            if (Graphic != null)
            {
                if (SourceRectangle != Rectangle.Empty)
                {
                    Dimensions = new Vector2(SourceRectangle.Width * Scale, SourceRectangle.Height * Scale);
                }
                else
                {
                    Dimensions = new Vector2(Graphic.Width * Scale, Graphic.Height * Scale);
                }
            }

            // Stripping the decimal parts of the vectors are intentional
            Bounds = new Rectangle(Location.ToPoint(), Dimensions.ToPoint());

            if (OnUpdate != null)
                OnUpdate();
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
            }
        }
    }
}
