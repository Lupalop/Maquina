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
        public ElementChangedProperty Property { get; set; }
    }

    public delegate void ElementChangedEventHandler(object sender, ElementChangedEventArgs e);
}
