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
            IsSpinning = true;
            Sprite = (Texture2D)ContentFactory.TryGetResource("throbber-default");
        }

        public bool IsSpinning { get; set; }

        public override Point Location
        {
            get { return base.Location; }
            set
            {
                base.Location = new Point(
                    value.X + (ActualSize.X / 2),
                    value.Y + (ActualSize.Y / 2));
            }
        }

        public override Point Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                DrawController.Origin = new Vector2(base.Size.X / 2, base.Size.Y / 2);
            }
        }

        public override void Update()
        {
            if (!IsSpinning)
            {
                return;
            }

            DrawController.Rotation += .05f;
        }
    }
}
