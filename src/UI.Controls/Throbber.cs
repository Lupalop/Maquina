using Maquina.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class Throbber : Image
    {
        public Throbber(string name) : base(name)
        {
            Id = "GUI_THROBBER";
            IsSpinning = true;
            Sprite.Graphic = (Texture2D)ContentFactory.TryGetResource("throbber-default");
            ElementUpdated += (sender, e) =>
            {
                if (!IsSpinning)
                    return;

                Sprite.Rotation += .05f;
            };
            ElementChanged -= Image_ElementChanged;
            ElementChanged += Throbber_ElementChanged;
            Application.Display.ScaleChanged += Global_ScaleChanged;
        }

        private void Global_ScaleChanged(object sender, EventArgs e)
        {
            Throbber_ElementChanged(this, new ElementChangedEventArgs(ElementChangedProperty.Location));
        }

        private void Throbber_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            Sprite.RotationOrigin = new Vector2(Size.X / 2, Size.Y / 2);
            switch (e.Property)
            {
                case ElementChangedProperty.Location:
                    Point NewLocation = new Point(Location.X + (ActualSize.X / 2), Location.Y + (ActualSize.Y / 2));
                    Sprite.Location = NewLocation;
                    break;
                case ElementChangedProperty.IgnoreGlobalScale:
                    Sprite.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
                    break;
                default:
                    break;
            }
        }

        public bool IsSpinning { get; set; }
    }
}
