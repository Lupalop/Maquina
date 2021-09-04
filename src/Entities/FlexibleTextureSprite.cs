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

        private Point _sideDistance;
        private Rectangle[] _sourceRectangles;
        private Rectangle[] _destinationRectangles;
        private Rectangle _cachedBounds;

        public FlexibleTextureSprite(Texture2D texture, Point sideDistance)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            if (sideDistance == null)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            Texture = texture;
            Size = texture.Bounds.Size;
            _sideDistance = sideDistance;

            PrepareSourceRectangles();
        }

        private void PrepareSourceRectangles()
        {
            _sourceRectangles = new Rectangle[RectangleCount];
            Point textureSize = Texture.Bounds.Size;

            int horizontalEdgeWidth = _sideDistance.X * 2;
            int verticalEdgeWidth = _sideDistance.Y * 2;
            int middleWidth = textureSize.X - horizontalEdgeWidth;
            int middleHeight = textureSize.Y - verticalEdgeWidth;

            int distanceFromTop = textureSize.Y - _sideDistance.Y;
            int distanceFromLeft = textureSize.X - _sideDistance.X;

            // Section 1
            _sourceRectangles[0] = new Rectangle(Point.Zero, _sideDistance);
            _sourceRectangles[3] = new Rectangle(0, _sideDistance.Y, _sideDistance.X, middleHeight);
            _sourceRectangles[6] = new Rectangle(0, distanceFromTop, _sideDistance.X, _sideDistance.Y);
            // Section 2
            _sourceRectangles[1] = new Rectangle(_sideDistance.X, 0, middleWidth, _sideDistance.Y);
            _sourceRectangles[4] = new Rectangle(_sideDistance.X, _sideDistance.Y, middleWidth, middleHeight);
            _sourceRectangles[7] = new Rectangle(_sideDistance.X, distanceFromTop, middleWidth, _sideDistance.Y);
            // Section 3
            _sourceRectangles[2] = new Rectangle(distanceFromLeft, 0, _sideDistance.X, _sideDistance.Y);
            _sourceRectangles[5] = new Rectangle(distanceFromLeft, _sideDistance.Y, _sideDistance.X, middleHeight);
            _sourceRectangles[8] = new Rectangle(distanceFromLeft, distanceFromTop, _sideDistance.X, _sideDistance.Y);
        }

        private void PrepareDestinationRectangles(Rectangle bounds)
        {
            if (_cachedBounds == bounds)
            {
                return;
            }

            _destinationRectangles = new Rectangle[RectangleCount];

            // Section 1
            _destinationRectangles[0] = new Rectangle(
                bounds.Location,
                _sideDistance);
            _destinationRectangles[3] = new Rectangle(
                bounds.Location.X,
                bounds.Location.Y + _sideDistance.Y,
                _sideDistance.X,
                bounds.Height - _sideDistance.Y * 2);
            _destinationRectangles[6] = new Rectangle(
                bounds.Location.X,
                bounds.Location.Y + bounds.Height - _sideDistance.Y,
                _sideDistance.X,
                _sideDistance.Y);
            // Section 2
            _destinationRectangles[1] = new Rectangle(
                bounds.Location.X + _sideDistance.X,
                bounds.Location.Y,
                bounds.Width - _sideDistance.X * 2,
                _sideDistance.Y);
            _destinationRectangles[4] = new Rectangle(
                bounds.Location.X + _sideDistance.X,
                bounds.Location.Y + _sideDistance.Y,
                bounds.Width - _sideDistance.X * 2,
                bounds.Height - _sideDistance.Y * 2);
            _destinationRectangles[7] = new Rectangle(
                bounds.Location.X + _sideDistance.X,
                bounds.Location.Y + bounds.Height - _sideDistance.Y,
                bounds.Width - _sideDistance.X * 2,
                _sideDistance.Y);
            // Section 3
            _destinationRectangles[2] = new Rectangle(
                bounds.Location.X + bounds.Width - _sideDistance.X,
                bounds.Location.Y,
                _sideDistance.X,
                _sideDistance.Y);
            _destinationRectangles[5] = new Rectangle(
                bounds.Location.X + bounds.Width - _sideDistance.X,
                bounds.Location.Y + _sideDistance.Y,
                _sideDistance.X,
                bounds.Height - _sideDistance.Y * 2);

            _destinationRectangles[8] = new Rectangle(
                bounds.Location.X + bounds.Width - _sideDistance.X,
                bounds.Location.Y + bounds.Height - _sideDistance.Y,
                _sideDistance.X,
                _sideDistance.Y);

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
