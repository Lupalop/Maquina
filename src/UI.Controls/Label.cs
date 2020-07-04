using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

namespace Maquina.UI
{
    public class Label : Control
    {
        public Label(string name) : base (name)
        {
            Sprite = new TextSprite();
        }

        public TextSprite Sprite { get; set; }

        public override Point Size
        {
            get
            {
                if (base.Size != Point.Zero)
                {
                    return base.Size;
                }
                return Sprite == null ? base.Size : Sprite.Size;
            }
        }

        public string Text
        {
            get { return Sprite?.Text; }
            set
            {
                if (Sprite == null)
                {
                    Sprite = value;
                    return;
                }
                Sprite.Text = value;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite?.Draw(spriteBatch, DrawController, Location, ActualScale);
        }

        public override void Update()
        {
        }
    }
}
