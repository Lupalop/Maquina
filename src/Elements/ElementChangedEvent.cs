using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public enum ElementChangedProperty
    {
        Custom,
        DestinationRectangle,
        Location,
        Size,
        Scale,
        IgnoreGlobalScale
    }

    public class ElementChangedEventArgs : EventArgs
    {
        public ElementChangedEventArgs(ElementChangedProperty property)
        {
            Property = property;
        }
        public ElementChangedEventArgs(ElementChangedProperty property, Rectangle bounds)
        {
            Property = property;
            Location = bounds.Location;
            Size = bounds.Size;
        }
        public ElementChangedEventArgs(ElementChangedProperty property, Point location, Point size)
        {
            Property = property;
            Location = location;
            Size = size;
        }
        public ElementChangedEventArgs(ElementChangedProperty property, float scale)
        {
            Property = property;
            Scale = scale;
        }
        public ElementChangedEventArgs(ElementChangedProperty property, bool ignoreGlobalScale)
        {
            Property = property;
            IgnoreGlobalScale = ignoreGlobalScale;
        }

        public ElementChangedProperty Property { get; set; }
        public Rectangle Bounds
        {
            get { return new Rectangle(Location, Size); }
        }
        public Point Location { get; set; }
        public Point Size { get; set; }
        public float Scale { get; set; }
        public bool IgnoreGlobalScale { get; set; }
    }

    public delegate void ElementChangedEventHandler(object sender, ElementChangedEventArgs e);
}
