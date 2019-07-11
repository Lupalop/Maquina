using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public interface IContainer
    {
        ObservableDictionary<string, BaseElement> Children { get; set; }
        Orientation Orientation { get; set; }
        Region ElementMargin { get; set; }
    }
}
