﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class TextureSprite
    {
        protected TextureSprite()
        {
        }

        public TextureSprite(Texture2D texture)
        {
            Texture = texture;
            Size = (texture == null) ? Point.Zero : Texture.Bounds.Size;
        }

        public static implicit operator TextureSprite(Texture2D texture)
        {
            return new TextureSprite(texture);
        }

        public virtual Texture2D Texture { get; protected set; }

        public virtual Point Size { get; protected set; }

        public virtual void Draw(
            SpriteBatch spriteBatch,
            DrawController controller,
            Rectangle bounds,
            Rectangle? sourceRectangle)
        {
            spriteBatch.Draw(
                Texture,
                bounds,
                sourceRectangle,
                controller.Tint * controller.Opacity,
                controller.Rotation,
                controller.Origin,
                controller.SpriteEffects,
                controller.LayerDepth);
        }

        public void Draw(
            SpriteBatch spriteBatch,
            DrawController controller,
            Rectangle bounds)
        {
            Draw(spriteBatch, controller, bounds, null);
        }

        public virtual void Draw(
            SpriteBatch spriteBatch,
            DrawController controller,
            Point location,
            float scale,
            Rectangle? sourceRectangle)
        {
            spriteBatch.Draw(
                Texture,
                location.ToVector2(),
                sourceRectangle,
                controller.Tint * controller.Opacity,
                controller.Rotation,
                controller.Origin,
                scale,
                controller.SpriteEffects,
                controller.LayerDepth);
        }

        public virtual void Draw(
            SpriteBatch spriteBatch,
            DrawController controller,
            Point location,
            float scale)
        {
            Draw(spriteBatch, controller, location, scale, null);
        }
    }
}
