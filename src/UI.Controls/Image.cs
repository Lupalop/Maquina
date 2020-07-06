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
    public class Image : Control
    {
        public Image(string name) : base(name)
        {
        }

        public TextureSprite Sprite { get; set; }

        public override Point Size
        {
            get
            {
                if (base.Size != Point.Zero || Sprite == null)
                {
                    return base.Size;
                }
                return Sprite.Size;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite?.Draw(spriteBatch, DrawController, ActualBounds);
        }

        public override void Update()
        {
        }
    }
}
