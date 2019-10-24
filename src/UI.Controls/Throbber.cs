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
            ElementUpdated += (sender, e) =>
            {
                if (!IsSpinning)
                    return;

                Background.Rotation += .05f;
            };
            ElementChanged -= Image_ElementChanged;
            ElementChanged += Throbber_ElementChanged;
            Global.Display.ScaleChanged += Global_ScaleChanged;
        }

        private void Global_ScaleChanged(object sender, EventArgs e)
        {
            Throbber_ElementChanged(this, new ElementChangedEventArgs(ElementChangedProperty.Location));
        }

        private void Throbber_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            Background.RotationOrigin = new Vector2(Size.X / 2, Size.Y / 2);
            switch (e.Property)
            {
                case ElementChangedProperty.Location:
                    Point NewLocation = new Point(Location.X + (ActualSize.X / 2), Location.Y + (ActualSize.Y / 2));
                    Background.Location = NewLocation;
                    break;
                case ElementChangedProperty.IgnoreGlobalScale:
                    Background.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
                    break;
                default:
                    break;
            }
        }

        // General
        public override string Id
        {
            get { return "GUI_THROBBER"; }
        }
        public bool IsSpinning { get; set; }
    }
}
