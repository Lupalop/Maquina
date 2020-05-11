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
        // Constructor
        protected Entity(string name)
        {
            Id = "GENERIC_BASE";
            Name = name;
            Scale = 1;
        }

        // General properties
        public string Name { get; set; }
        public string Id { get; protected set; }
        
        // Scale
        private float scale;
        public float Scale
        {
            get
            {
                if (!IsScaleSupported)
                {
                    return 1;
                }
                return scale;
            }
            set
            {
#if LOG_ENABLED
                if (!IsScaleSupported)
                {
                    LogManager.Warn(0,
                        string.Format("Entity {0} with ID {1} does not support the scale property.", Name, Id));
                }
#endif
                scale = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Scale));
            }
        }
        // Scale adjusted for global scale
        public float ActualScale
        {
            get
            {
                if (IgnoreGlobalScale || !IsScaleSupported)
                {
                    return Scale;
                }
                return Scale * Application.Display.Scale;
            }
        }
        //
        private bool ignoreGlobalScale;
        public bool IgnoreGlobalScale
        {
            get { return ignoreGlobalScale; }
            set
            {
                ignoreGlobalScale = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.IgnoreGlobalScale));
            }
        }
        protected bool IsScaleSupported = true;

        // Layout
        // Destination rectangle not adjusted for scale
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
        // Destination rectangle adjusted for scale
        public Rectangle ActualBounds
        {
            get
            {
                return new Rectangle(Location, ActualSize);
            }
        }

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

        // Size not adjusted for scale
        private Point size;
        public virtual Point Size
        {
            get { return size; }
            set
            {
                if (value == size)
                {
                    return;
                }
                size = value;
                OnEntityChanged(new EntityChangedEventArgs(EntityChangedProperty.Size));
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

        // Entity events
        public event EventHandler<EntityChangedEventArgs> EntityChanged;
        public event EventHandler EntityUpdated;
        public event EventHandler EntityDrawn;

        protected virtual void OnEntityUpdated()
        {
            if (EntityUpdated != null)
            {
                EntityUpdated(this, EventArgs.Empty);
            }
        }

        protected virtual void OnEntityDrawn()
        {
            if (EntityDrawn != null)
            {
                EntityDrawn(this, EventArgs.Empty);
            }
        }

        protected virtual void OnEntityChanged(EntityChangedEventArgs e)
        {
            if (EntityChanged != null)
            {
                EntityChanged(this, e);
            }
        }

        // Update and draw methods
        public virtual void Update()
        {
            OnEntityUpdated();
        }

        public virtual void Draw()
        {
            OnEntityDrawn();
        }

        // IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                EntityChanged = null;
                EntityUpdated = null;
                EntityDrawn = null;
            }
        }
    }
}
