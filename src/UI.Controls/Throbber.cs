using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class Throbber : GuiElement
    {
        public Throbber(string objectName) : base(objectName)
        {
            Graphic = Global.Textures["throbber-default"];
            OnUpdate = (element) =>
            {
                element.RotationOrigin = new Vector2(element.Graphic.Width / 2, element.Graphic.Height / 2);
                element.Location = new Vector2(element.Location.X + (element.Bounds.Width / 2), element.Location.Y + (element.Bounds.Height / 2));
                element.Rotation += .05f;
            };
        }

        public override string ID
        {
            get { return "GUI_THROBBER"; }
        }
    }
}
