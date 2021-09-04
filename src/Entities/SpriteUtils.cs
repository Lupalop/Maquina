using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class SpriteUtils
    {
        public const int SliceCount = 9;

        public static Rectangle[] SourceFromTextureAtlas(int totalFrames, int columnCount, Point destinationSize)
        {
            Rectangle[] sourceRectangles = new Rectangle[totalFrames];
            for (int targetFrame = 0; targetFrame < totalFrames; targetFrame++)
            {
                if (targetFrame > totalFrames)
                {
                    throw new InvalidOperationException();
                }

                int row = targetFrame / columnCount;
                int column = targetFrame % columnCount;
                Point positionInTexture = new Point(
                    destinationSize.X * column, destinationSize.Y * row);

                sourceRectangles[targetFrame] = new Rectangle(positionInTexture, destinationSize);
            }
            return sourceRectangles;
        }

        public static Rectangle[] SourceFromSlicedTexture(Margin offset, Point textureSize)
        {
            Rectangle[] sourceRectangles = new Rectangle[SliceCount];

            int middleWidth = textureSize.X - offset.Width;
            int middleHeight = textureSize.Y - offset.Height;
            int distanceToBottom = textureSize.Y - offset.Bottom;
            int distanceToRight = textureSize.X - offset.Right;

            // Column 1
            sourceRectangles[0] = new Rectangle(
                0, 0, offset.Left, offset.Top);
            sourceRectangles[3] = new Rectangle(
                0, offset.Top, offset.Left, middleHeight);
            sourceRectangles[6] = new Rectangle(
                0, distanceToBottom, offset.Left, offset.Bottom);
            // Column 2
            sourceRectangles[1] = new Rectangle(
                offset.Left, 0, middleWidth, offset.Top);
            sourceRectangles[4] = new Rectangle(
                offset.Left, offset.Top, middleWidth, middleHeight);
            sourceRectangles[7] = new Rectangle(
                offset.Left, distanceToBottom, middleWidth, offset.Bottom);
            // Column 3
            sourceRectangles[2] = new Rectangle(
                distanceToRight, 0, offset.Right, offset.Top);
            sourceRectangles[5] = new Rectangle(
                distanceToRight, offset.Top, offset.Right, middleHeight);
            sourceRectangles[8] = new Rectangle(
                distanceToRight, distanceToBottom, offset.Right, offset.Bottom);

            return sourceRectangles;
        }

        public static Rectangle[] DestinationFromSlicedTexture(Margin offset, Rectangle bounds)
        {
            Rectangle[] destinationRectangles = new Rectangle[SliceCount];

            int middleWidth = bounds.Width - offset.Width;
            int middleHeight = bounds.Height - offset.Height;
            int distanceFromTop = bounds.Location.Y + offset.Top;
            int distanceFromLeft = bounds.Location.X + offset.Left;
            int distanceToBottom = bounds.Location.Y + bounds.Height - offset.Bottom;
            int distanceToRight = bounds.Location.X + bounds.Width - offset.Right;

            // Column 1
            destinationRectangles[0] = new Rectangle(
                bounds.Location.X, bounds.Location.Y, offset.Left, offset.Top);
            destinationRectangles[3] = new Rectangle(
                bounds.Location.X, distanceFromTop, offset.Left, middleHeight);
            destinationRectangles[6] = new Rectangle(
                bounds.Location.X, distanceToBottom, offset.Left, offset.Bottom);
            // Column 2
            destinationRectangles[1] = new Rectangle(
                distanceFromLeft, bounds.Location.Y, middleWidth, offset.Top);
            destinationRectangles[4] = new Rectangle(
                distanceFromLeft, distanceFromTop, middleWidth, middleHeight);
            destinationRectangles[7] = new Rectangle(
                distanceFromLeft, distanceToBottom, middleWidth, offset.Bottom);
            // Column 3
            destinationRectangles[2] = new Rectangle(
                distanceToRight, bounds.Location.Y, offset.Right, offset.Top);
            destinationRectangles[5] = new Rectangle(
                distanceToRight, distanceFromTop, offset.Right, middleHeight);
            destinationRectangles[8] = new Rectangle(
                distanceToRight, distanceToBottom, offset.Right, offset.Bottom);

            return destinationRectangles;
        }
    }
}
