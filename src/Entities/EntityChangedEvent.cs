using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public enum EntityChangedProperty
    {
        Custom                      = 0,
        // Base properties
        Location                    = 1,
        Size                        = 2,
        Scale                       = 3,
        // Control properties
        HorizontalAlignment         = 4,
        VerticalAlignment           = 5,
        Disabled                    = 6,
        Focused                     = 7,
        // Container properties
        Orientation                 = 8,
        Margin                      = 9,
    }

    public class EntityChangedEventArgs : EventArgs
    {
        public EntityChangedEventArgs(EntityChangedProperty property)
        {
            Property = property;
            PropertyName = property.ToString();
        }

        public EntityChangedEventArgs(string propertyName)
        {
            Property = EntityChangedProperty.Custom;
            PropertyName = propertyName;
        }

        public EntityChangedProperty Property { get; protected set; }
        public string PropertyName { get; protected set; }
    }
}
