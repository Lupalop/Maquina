using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI
{
    public class Image : GuiElement
    {
        // GenericElement provides everything needed for an image, except that it is abstract...
        public Image(string objectName) : base(objectName) { }
    }
}
