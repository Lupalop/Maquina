using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class FlexibleTextureSprite : TextureSprite
    {
        private const int RectangleCount = 9;

        private Margin _margin;
        private Rectangle[] _sourceRectangles;
        private Rectangle[] _destinationRectangles;
        private Rectangle _cachedBounds;

        public FlexibleTextureSprite(Texture2D texture, Margin margin)
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

            PrepareSourceRectangles();
        }

        private void PrepareSourceRectangles()
        {
            _sourceRectangles = new Rectangle[RectangleCount];
            Point textureSize = Texture.Bounds.Size;

            int middleWidth = textureSize.X - _margin.Width;
            int middleHeight = textureSize.Y - _margin.Height;
            int distanceToBottom = textureSize.Y - _margin.Bottom;
            int distanceToRight = textureSize.X - _margin.Right;

            // Section 1
            _sourceRectangles[0] = new Rectangle(
                0, 0, _margin.Left, _margin.Top);
            _sourceRectangles[3] = new Rectangle(
                0, _margin.Top, _margin.Left, middleHeight);
            _sourceRectangles[6] = new Rectangle(
                0, distanceToBottom, _margin.Left, _margin.Bottom);
            // Section 2
            _sourceRectangles[1] = new Rectangle(
                _margin.Left, 0, middleWidth, _margin.Top);
            _sourceRectangles[4] = new Rectangle(
                _margin.Left, _margin.Top, middleWidth, middleHeight);
            _sourceRectangles[7] = new Rectangle(
                _margin.Left, distanceToBottom, middleWidth, _margin.Bottom);
            // Section 3
            _sourceRectangles[2] = new Rectangle(
                distanceToRight, 0, _margin.Right, _margin.Top);
            _sourceRectangles[5] = new Rectangle(
                distanceToRight, _margin.Top, _margin.Right, middleHeight);
            _sourceRectangles[8] = new Rectangle(
                distanceToRight, distanceToBottom, _margin.Right, _margin.Bottom);
        }

        private void PrepareDestinationRectangles(Rectangle bounds)
        {
            if (_cachedBounds == bounds)
            {
                return;
            }

            _destinationRectangles = new Rectangle[RectangleCount];

            int middleWidth = bounds.Width - _margin.Width;
            int middleHeight = bounds.Height - _margin.Height;
            int distanceFromTop = bounds.Location.Y + _margin.Top;
            int distanceFromLeft = bounds.Location.X + _margin.Left;
            int distanceToBottom = bounds.Location.Y + bounds.Height - _margin.Bottom;
            int distanceToRight = bounds.Location.X + bounds.Width - _margin.Right;

            // Section 1
            _destinationRectangles[0] = new Rectangle(
                bounds.Location.X, bounds.Location.Y, _margin.Left, _margin.Top);
            _destinationRectangles[3] = new Rectangle(
                bounds.Location.X, distanceFromTop, _margin.Left, middleHeight);
            _destinationRectangles[6] = new Rectangle(
                bounds.Location.X, distanceToBottom, _margin.Left, _margin.Bottom);
            // Section 2
            _destinationRectangles[1] = new Rectangle(
                distanceFromLeft, bounds.Location.Y, middleWidth, _margin.Top);
            _destinationRectangles[4] = new Rectangle(
                distanceFromLeft, distanceFromTop, middleWidth, middleHeight);
            _destinationRectangles[7] = new Rectangle(
                distanceFromLeft, distanceToBottom, middleWidth, _margin.Bottom);
            // Section 3
            _destinationRectangles[2] = new Rectangle(
                distanceToRight, bounds.Location.Y, _margin.Right, _margin.Top);
            _destinationRectangles[5] = new Rectangle(
                distanceToRight, distanceFromTop, _margin.Right, middleHeight);
            _destinationRectangles[8] = new Rectangle(
                distanceToRight, distanceToBottom, _margin.Right, _margin.Bottom);

            _cachedBounds = bounds;
        }

        public override void Draw(SpriteBatch spriteBatch, DrawController controller, Rectangle bounds)
        {
            PrepareDestinationRectangles(bounds);

            for (int i = 0; i < RectangleCount; i++)
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

        public override void Draw(SpriteBatch spriteBatch, DrawController controller, Point location, float scale)
        {
            throw new NotSupportedException();
        }
    }
}
