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

            _sourceRectangles = SpriteUtils.SourceFromTextureAtlas(_columns, _rows, Size);
        }

        public int TotalFrames { get; protected set; }

        public int Frame { get; set; }

        public override void Draw(SpriteBatch spriteBatch, DrawController controller, Rectangle bounds, Rectangle? sourceRectangle)
        {
            spriteBatch.Draw(
                Texture,
                bounds,
                (sourceRectangle != null) ? sourceRectangle : _sourceRectangles[Frame],
                controller.Tint * controller.Opacity,
                controller.Rotation,
                controller.Origin,
                controller.SpriteEffects,
                controller.LayerDepth);
        }

        public override void Draw(SpriteBatch spriteBatch, DrawController controller, Point location, float scale, Rectangle? sourceRectangle)
        {
            spriteBatch.Draw(
                Texture,
                location.ToVector2(),
                (sourceRectangle != null) ? sourceRectangle : _sourceRectangles[Frame],
                controller.Tint * controller.Opacity,
                controller.Rotation,
                controller.Origin,
                scale,
                controller.SpriteEffects,
                controller.LayerDepth);
        }
    }
}
