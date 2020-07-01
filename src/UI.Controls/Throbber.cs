using Maquina.Entities;
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
            Id = "UI_THROBBER";
            IsSpinning = true;
            Sprite.Texture = (Texture2D)ContentFactory.TryGetResource("throbber-default");
            EntityUpdated += (sender, e) =>
            {
                if (!IsSpinning)
                    return;

                Sprite.Rotation += .05f;
            };
            Changed -= Image_EntityChanged;
            Changed += Throbber_EntityChanged;
            Application.Display.ScaleChanged += Global_ScaleChanged;
        }

        private void Global_ScaleChanged(object sender, EventArgs e)
        {
            Throbber_EntityChanged(this, new EntityChangedEventArgs(EntityChangedProperty.Location));
        }

        private void Throbber_EntityChanged(object sender, EntityChangedEventArgs e)
        {
            Sprite.Origin = new Vector2(Size.X / 2, Size.Y / 2);
            switch (e.Property)
            {
                case EntityChangedProperty.Location:
                    Point NewLocation = new Point(Location.X + (ActualSize.X / 2), Location.Y + (ActualSize.Y / 2));
                    Sprite.Location = NewLocation;
                    break;
                case EntityChangedProperty.IgnoreGlobalScale:
                    Sprite.IgnoreGlobalScale = ((Entity)sender).IgnoreGlobalScale;
                    break;
                default:
                    break;
            }
        }

        public bool IsSpinning { get; set; }
    }
}
