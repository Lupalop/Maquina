using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class TextureAtlasSprite : TextureSprite
    {
        private int _columns;
        private int _rows;
        private Rectangle[] _sourceRectangles;

        public TextureAtlasSprite(Texture2D texture, int columns, int rows)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }
            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException("rows");
            }

            Texture = texture;
            TotalFrames = columns * rows;
            _columns = columns;
            _rows = rows;

            Size = new Point(
                    texture.Bounds.Size.X / columns,
                    texture.Bounds.Size.Y / rows);

            PrepareFrameSourceRectangles();
        }

        public bool OverrideSourceRectangle { get; set; }

        public int TotalFrames { get; protected set; }

        public int Frame { get; set; }

        private Rectangle CreateSourceFrameRectangle(int targetFrame)
        {
            if (targetFrame < 0 || targetFrame > TotalFrames)
            {
                throw new ArgumentOutOfRangeException("targetFrame");
            }

            int row = targetFrame / _columns;
            int column = targetFrame % _columns;
            Point positionInTexture = new Point(Size.X * column, Size.Y * row);

            return new Rectangle(positionInTexture, Size);
        }

        private void PrepareFrameSourceRectangles()
        {
            _sourceRectangles = new Rectangle[TotalFrames];
            for (int i = 0; i < TotalFrames; i++)
            {
                _sourceRectangles[i] = CreateSourceFrameRectangle(i);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, DrawController controller, Rectangle bounds)
        {
            spriteBatch.Draw(
                Texture,
                bounds,
                OverrideSourceRectangle ? controller.SourceRectangle : _sourceRectangles[Frame],
                controller.Tint * controller.Opacity,
                controller.Rotation,
                controller.Origin,
                controller.SpriteEffects,
                controller.LayerDepth);
        }

        public override void Draw(SpriteBatch spriteBatch, DrawController controller, Point location, float scale)
        {
            spriteBatch.Draw(
                Texture,
                location.ToVector2(),
                OverrideSourceRectangle ? controller.SourceRectangle : _sourceRectangles[Frame],
                controller.Tint * controller.Opacity,
                controller.Rotation,
                controller.Origin,
                scale,
                controller.SpriteEffects,
                controller.LayerDepth);
        }
    }
}
