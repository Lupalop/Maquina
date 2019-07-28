﻿using System;
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
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Scale, value));
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
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.IgnoreGlobalScale, value));
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
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.DestinationRectangle, value));
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
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Location, value, Point.Zero));
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
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Size, Point.Zero, value));
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
        public event ElementChangedEventHandler ElementChanged;
        public event Action<BaseElement> ElementUpdated;
        public event Action<BaseElement> ElementDrawn;

        protected void OnElementUpdated()
        {
            if (ElementUpdated != null)
            {
                ElementUpdated(this);
            }
        }

        protected void OnElementDrawn()
        {
            if (ElementDrawn != null)
            {
                ElementDrawn(this);
            }
        }

        protected void OnElementChanged(ElementChangedEventArgs e)
        {
            if (ElementChanged != null)
            {
#if HAS_CONSOLE && LOG_GENERAL
                if (Name != "mouse")
                {
                    Console.WriteLine("Element updated: " + this.Name + this.Id + e.Property.ToString());
                }
#endif
                ElementChanged(this, e);
            }
        }

        // Update and draw methods
        public virtual void Update(GameTime gameTime)
        {
            OnElementUpdated();
        }

        public virtual void Draw(GameTime gameTime)
        {
            OnElementDrawn();
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
                ElementChanged = null;
                ElementUpdated = null;
                ElementDrawn = null;
            }
        }
    }
}
