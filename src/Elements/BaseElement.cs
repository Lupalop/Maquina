using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Maquina.Elements
{
    public abstract class BaseElement : IBaseElement
    {
        // Constructor
        protected BaseElement(string name)
        {
            Name = name;
            Scale = 1;
        }

        // General properties
        public string Name { get; set; }
        public virtual string Id
        {
            get { return "GENERIC_BASE"; }
        }
        
        // Scale
        private float scale;
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                if (ScaleChanged != null)
                {
                    ScaleChanged(value);
                }
            }
        }
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
        //
        private bool ignoreGlobalScale;
        public bool IgnoreGlobalScale
        {
            get { return ignoreGlobalScale; }
            set
            {
                ignoreGlobalScale = value;
                if (IgnoreGlobalScaleChanged != null)
                {
                    IgnoreGlobalScaleChanged(value);
                }
            }
        }

        // Layout
        // Destination rectangle not adjusted for scale
        public virtual Rectangle Bounds
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
                if (LocationChanged != null)
                {
                    LocationChanged(value);
                }
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

        // Element events
        public event Action<Rectangle> DestinationRectangleChanged;
        public event Action<Point> LocationChanged;
        public event Action<Point> SizeChanged;
        public event Action<float> ScaleChanged;
        public event Action<bool> IgnoreGlobalScaleChanged;
        public event Action<BaseElement> OnUpdate;
        public event Action<BaseElement> OnDraw;

        // Update and draw methods
        public virtual void Update(GameTime gameTime)
        {
            if (OnUpdate != null)
            {
                OnUpdate(this);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (OnDraw != null)
            {
                OnDraw(this);
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
            if (disposing)
            {
                // Nothing to dispose.
                return;
            }
        }
    }
}
