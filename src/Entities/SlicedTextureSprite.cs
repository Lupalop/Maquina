﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class SlicedTextureSprite : TextureSprite
    {
        private Margin _margin;
        private Rectangle[] _sourceRectangles;
        private Rectangle[] _destinationRectangles;
        private Rectangle _cachedBounds;

        public SlicedTextureSprite(Texture2D texture, Margin margin)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            if (margin == null)
            {
                throw new ArgumentOutOfRangeException("margin");
            }

            Texture = texture;
            Size = texture.Bounds.Size;
            _margin = margin;
            _sourceRectangles = SpriteUtils.CreateFromSlicedTexture(_margin, Texture.Bounds);
        }

        private void PrepareDestinationRectangles(Rectangle bounds)
        {
            if (_cachedBounds == bounds)
            {
                return;
            }
            _destinationRectangles = SpriteUtils.CreateFromSlicedTexture(_margin, bounds);
            _cachedBounds = bounds;
        }

        public override void Draw(SpriteBatch spriteBatch, DrawController controller, Rectangle bounds, Rectangle? sourceRectangle)
        {
            PrepareDestinationRectangles(bounds);

            for (int i = 0; i < _sourceRectangles.Length; i++)
            {
                spriteBatch.Draw(
                    Texture,
                    _destinationRectangles[i],
                    _sourceRectangles[i],
                    controller.Tint * controller.Opacity,
                    controller.Rotation,
                    controller.Origin,
                    controller.SpriteEffects,
                    controller.LayerDepth);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, DrawController controller, Point location, float scale, Rectangle? sourceRectangle)
        {
            throw new NotSupportedException();
        }
    }
}
