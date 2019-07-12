using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class Throbber : Image
    {
        public Throbber(string objectName) : base(objectName)
        {
            IsSpinning = true;
            Graphic = Global.Textures["throbber-default"];
            ElementUpdated += (element) =>
            {
                if (!IsSpinning)
                    return;

                Image img = (Image)element;
                Sprite bg = img.Background;
                bg.RotationOrigin = new Vector2(
                    element.ActualSize.X / 2,
                    element.ActualSize.Y / 2);
                element.Location = new Point(
                    element.Location.X + (element.ActualSize.X / 2),
                    element.Location.Y + (element.ActualSize.Y / 2));
                bg.Rotation += .05f;
            };
        }

        // General
        public override string Id
        {
            get { return "GUI_THROBBER"; }
        }
        public bool IsSpinning { get; set; }
    }
}
