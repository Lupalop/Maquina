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

        public static Rectangle[] SourceFromTextureAtlas(int columns, int rows, Point cellSize)
        {
            int totalCells = columns * rows;
            Rectangle[] cells = new Rectangle[totalCells];

            for (int currentCell = 0; currentCell < totalCells; currentCell++)
            {
                int row = currentCell / columns;
                int column = currentCell % columns;
                Point positionInTexture = new Point(
                    cellSize.X * column, cellSize.Y * row);

                cells[currentCell] = new Rectangle(positionInTexture, cellSize);
            }

            return cells;
        }

        public static Rectangle[] CreateFromSlicedTexture(Margin offset, Rectangle bounds)
        {
            Rectangle[] cells = new Rectangle[SliceCount];

            int middleWidth = bounds.Width - offset.Width;
            int middleHeight = bounds.Height - offset.Height;
            int distanceFromTop = bounds.Location.Y + offset.Top;
            int distanceFromLeft = bounds.Location.X + offset.Left;
            int distanceToBottom = bounds.Location.Y + bounds.Height - offset.Bottom;
            int distanceToRight = bounds.Location.X + bounds.Width - offset.Right;

            // Column 1
            cells[0] = new Rectangle(
                bounds.Location.X, bounds.Location.Y, offset.Left, offset.Top);
            cells[3] = new Rectangle(
                bounds.Location.X, distanceFromTop, offset.Left, middleHeight);
            cells[6] = new Rectangle(
                bounds.Location.X, distanceToBottom, offset.Left, offset.Bottom);
            // Column 2
            cells[1] = new Rectangle(
                distanceFromLeft, bounds.Location.Y, middleWidth, offset.Top);
            cells[4] = new Rectangle(
                distanceFromLeft, distanceFromTop, middleWidth, middleHeight);
            cells[7] = new Rectangle(
                distanceFromLeft, distanceToBottom, middleWidth, offset.Bottom);
            // Column 3
            cells[2] = new Rectangle(
                distanceToRight, bounds.Location.Y, offset.Right, offset.Top);
            cells[5] = new Rectangle(
                distanceToRight, distanceFromTop, offset.Right, middleHeight);
            cells[8] = new Rectangle(
                distanceToRight, distanceToBottom, offset.Right, offset.Bottom);

            return cells;
        }
    }
}
