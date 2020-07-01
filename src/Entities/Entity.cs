using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Maquina.Entities
{
    public abstract class Entity : IEntity
    {
        private Point _location;
        private Point _size;
        private float _scale;
        private bool _ignoreGlobalScale;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class
        /// with a specified name.
        /// </summary>
        /// <param name="name">A name that describes the entity.</param>
        protected Entity(string name)
        {
            Id = "GENERIC_BASE";
            Name = name;
            Scale = 1;
        }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the unique identifier of the entity.
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets or sets the drawing location of the entity on the screen.
        /// </summary>
        public virtual Point Location
        {
            get { return _location; }
            set
            {
                if (value == _location)
                {
                    return;
                }
                _location = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Location));
            }
        }

        /// <summary>
        /// Gets or sets the dimensions of the entity.
        /// </summary>
        public virtual Point Size
        {
            get { return _size; }
            set
            {
                if (value == _size)
                {
                    return;
                }
                _size = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Size));
            }
        }

        /// <summary>
        /// Gets or sets the scaling of the entity.
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set
            {
                if (value == _scale || value < 0)
                {
                    return;
                }
                _scale = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Scale));
            }
        }

        /// <summary>
        /// Gets or sets the behavior of the entity when dealing with the global scale.
        /// </summary>
        public bool IgnoreGlobalScale
        {
            get { return _ignoreGlobalScale; }
            set { _ignoreGlobalScale = value; }
        }

        /// <summary>
        /// Gets or sets the drawing bounds of the entity.
        /// </summary>
        public virtual Rectangle Bounds
        {
            get { return new Rectangle(Location, Size); }
            set
            {
                Location = value.Location;
                Size = value.Size;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.DestinationRectangle));
            }
        }

        /// <summary>
        /// Gets the factor to multiply the size of the entity, taking into account the global scale.
        /// </summary>
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

        /// <summary>
        /// Gets the dimensions of the entity, taking into account the global scale.
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
        /// Gets the drawing bounds of the entity, taking into account the global scale.
        /// </summary>
        public Rectangle ActualBounds
        {
            get { return new Rectangle(Location, ActualSize); }
        }

        /// <summary>
        /// Occurs when a property of the entity has been modified.
        /// </summary>
        public event EventHandler<EntityChangedEventArgs> Changed;

        protected virtual void OnEntityChanged(EntityChangedEventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        /// <summary>
        /// This is called when the entity should update itself.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// This is called when the entity should draw itself using the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to be used in drawing text strings and sprites.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// This is called when the entity should draw itself using the default <see cref="SpriteBatch"/>.
        /// </summary>
        public void Draw()
        {
            Draw(Application.SpriteBatch);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Changed = null;
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
