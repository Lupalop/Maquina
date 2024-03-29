﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Maquina.Entities
{
    /// <summary>
    /// Defines the base class for entities.
    /// </summary>
    public abstract class Entity : IDisposable
    {
        private Rectangle _bounds;
        private float _scale;
        private bool _ignoreDisplayScale;
        private Entity _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class
        /// with a specified name, drawing bounds, scale, draw controller,
        /// and optionally ignores the display scale.
        /// </summary>
        /// <param name="name">A name that describes the entity.</param>
        /// <param name="bounds">The drawing bounds of the entity</param>
        /// <param name="scale">The scaling of the entity.</param>
        /// <param name="ignoreDisplayScale">The behavior of the entity when dealing with the display scale.</param>
        /// <param name="drawController">The draw controller of the entity.</param>
        protected Entity(
            string name,
            Rectangle bounds,
            float scale,
            bool ignoreDisplayScale,
            DrawController drawController)
        {
            if (scale < 0)
            {
                throw new ArgumentOutOfRangeException("scale");
            }
            Name = name;
            _bounds = bounds;
            _scale = scale;
            DrawController = drawController;
            _ignoreDisplayScale = ignoreDisplayScale;
            _parent = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class
        /// with a specified name.
        /// </summary>
        /// <param name="name">A name that describes the entity.</param>
        protected Entity(string name) : this(
            name,
            Rectangle.Empty,
            1,
            false,
            new DrawController())
        {
        }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the unique identifier of the entity.
        /// </summary>
        public string Id
        {
            get { return GetType().FullName; }
        }

        /// <summary>
        /// Gets or sets the draw controller used by the entity.
        /// </summary>
        public DrawController DrawController { get; set; }

        /// <summary>
        /// Gets or sets the drawing location of the entity on the screen.
        /// </summary>
        public virtual Point Location
        {
            get { return _bounds.Location; }
            set
            {
                if (value == _bounds.Location)
                {
                    return;
                }
                _bounds.Location = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.Location));
            }
        }

        /// <summary>
        /// Gets or sets the dimensions of the entity.
        /// </summary>
        public virtual Point Size
        {
            get { return _bounds.Size; }
            set
            {
                if (value == _bounds.Size)
                {
                    return;
                }
                _bounds.Size = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.Size));
            }
        }

        /// <summary>
        /// Gets or sets the scaling of the entity.
        /// </summary>
        public virtual float Scale
        {
            get { return _scale; }
            set
            {
                if (value == _scale || value < 0)
                {
                    return;
                }
                _scale = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.Scale));
            }
        }

        /// <summary>
        /// Gets or sets the behavior of the entity when dealing with the display scale.
        /// </summary>
        public virtual bool IgnoreDisplayScale
        {
            get { return _ignoreDisplayScale; }
            set { _ignoreDisplayScale = value; }
        }

        /// <summary>
        /// Gets or sets the parent entity of this entity.
        /// </summary>
        public Entity Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                {
                    return;
                }
                _parent = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PropertyId.Parent));
            }
        }

        /// <summary>
        /// Gets or sets the drawing bounds of the entity.
        /// </summary>
        public virtual Rectangle Bounds
        {
            get { return _bounds; }
            set
            {
                Location = value.Location;
                Size = value.Size;
            }
        }

        /// <summary>
        /// Gets the factor to multiply the size of the entity, taking into account the display scale.
        /// </summary>
        public float ActualScale
        {
            get
            {
                if (IgnoreDisplayScale)
                {
                    return Scale;
                }
                return Scale * Application.Display.Scale;
            }
        }

        /// <summary>
        /// Gets the dimensions of the entity, taking into account the display scale.
        /// </summary>
        public Point ActualSize
        {
            get
            {
                return new Point(
                    (int)(Size.X * ActualScale),
                    (int)(Size.Y * ActualScale));
            }
        }

        /// <summary>
        /// Gets the drawing bounds of the entity, taking into account the display scale.
        /// </summary>
        public Rectangle ActualBounds
        {
            get { return new Rectangle(Location, ActualSize); }
        }

        /// <summary>
        /// Occurs when a property of the entity has been modified.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// This is called when the entity should update itself.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// This is called when the entity should draw itself.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to be used in drawing text strings and sprites.</param>
        public abstract void Draw(SpriteBatch spriteBatch);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                PropertyChanged = null;
            }
        }

        /// <summary>
        /// Releases the resources used by this <see cref="Entity"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
