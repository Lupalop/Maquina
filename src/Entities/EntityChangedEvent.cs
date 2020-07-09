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
        Name                        = 1,
        Location                    = 2,
        Size                        = 3,
        Scale                       = 4,
        // Control properties
        HorizontalAlignment         = 5,
        VerticalAlignment           = 6,
        Disabled                    = 7,
        Focused                     = 8,
        // Container properties
        Orientation                 = 9,
        Margin                      = 10,
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
