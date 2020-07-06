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
    public class SoftwareMouse : Entity
    {
        public SoftwareMouse() : base("SoftwareMouse")
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
            Application.SpriteBatch.Begin(default(SpriteSortMode), BlendState);
            Sprite.Draw(spriteBatch, DrawController, ActualBounds);
            Application.SpriteBatch.End();
        }

        public BlendState BlendState { get; set; }

        public override void Update()
        {
            if (Sprite is TextureAtlasSprite)
            {
                ((TextureAtlasSprite)Sprite).Frame = 0;
            }
            Location = Application.Input.MousePosition;

            if (Application.Input.MouseDown(MouseButton.Left) ||
                Application.Input.MouseDown(MouseButton.Right) ||
                Application.Input.MouseDown(MouseButton.Middle))
            {
                if (Sprite is TextureAtlasSprite)
                {
                    ((TextureAtlasSprite)Sprite).Frame = 1;
                }
            }
        }
    }
}
