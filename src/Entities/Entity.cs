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
        protected Entity(string name)
        {
            Id = "GENERIC_BASE";
            Name = name;
            Scale = 1;
        }

        public string Name { get; set; }
        public string Id { get; protected set; }

        private Point location;
        public virtual Point Location
        {
            get { return location; }
            set
            {
                if (value == location)
                {
                    return;
                }
                location = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Location));
            }
        }

        private Point _size;
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

        private float _scale;
        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Scale));
            }
        }

        private bool _ignoreGlobalScale;
        public bool IgnoreGlobalScale
        {
            get { return _ignoreGlobalScale; }
            set { _ignoreGlobalScale = value; }
        }

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

        public Point ActualSize
        {
            get
            {
                return new Point(
                    (int)(_size.X * ActualScale),
                    (int)(_size.Y * ActualScale));
            }
        }

        public Rectangle ActualBounds
        {
            get { return new Rectangle(Location, ActualSize); }
        }

        public event EventHandler<EntityChangedEventArgs> EntityChanged;

        protected virtual void OnEntityChanged(EntityChangedEventArgs e)
        {
            if (EntityChanged != null)
            {
                EntityChanged(this, e);
            }
        }

        public virtual void Update()
        {
        }

        public virtual void Draw()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                EntityChanged = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
