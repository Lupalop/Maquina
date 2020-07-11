using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public enum PropertyId
    {
        Custom                      = 0,
        // Base properties
        Location                    = 1,
        Size                        = 2,
        Scale                       = 3,
        // Control properties
        HorizontalAlignment         = 4,
        VerticalAlignment           = 5,
        Enabled                     = 6,
        Focused                     = 7,
        // Container properties
        Orientation                 = 8,
        Margin                      = 9,
    }

    public class PropertyChangedEventArgs : EventArgs
    {
        public PropertyChangedEventArgs(PropertyId property)
        {
            Id = property;
            Name = property.ToString();
        }

        public PropertyChangedEventArgs(string propertyName)
        {
            Id = PropertyId.Custom;
            Name = propertyName;
        }

        public PropertyId Id { get; protected set; }
        public string Name { get; protected set; }
    }
}
