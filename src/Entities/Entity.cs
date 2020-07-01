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

        protected Entity(string name)
        {
            Id = "GENERIC_BASE";
            Name = name;
            Scale = 1;
        }

        public string Name { get; set; }
        public string Id { get; protected set; }

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

        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Scale));
            }
        }

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

        public event EventHandler<EntityChangedEventArgs> Changed;

        protected virtual void OnEntityChanged(EntityChangedEventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
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
                Changed = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
