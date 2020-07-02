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
        Location,
        Size,
        Scale,
    }

    public class EntityChangedEventArgs : EventArgs
    {
        public EntityChangedEventArgs(EntityChangedProperty property)
        {
            Property = property;
        }
        public EntityChangedProperty Property { get; set; }
    }
}
