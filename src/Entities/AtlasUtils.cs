using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public static class AtlasUtils
    {
        public static Rectangle CreateSourceFrameRectangle(
            Point textureSize,
            int columns,
            int rows,
            int targetFrame)
        {
            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }

            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException("rows");
            }

            int totalFrames = columns * rows;
            if (targetFrame < 0 || targetFrame > totalFrames)
            {
                throw new ArgumentOutOfRangeException("targetFrame");
            }

            int width = textureSize.X / columns;
            int height = textureSize.Y / rows;
            int row = targetFrame / columns;
            int column = targetFrame % columns;

            return new Rectangle(
                width * column,
                height * row,
                width,
                height);
        }

        public static Point GetFrameSize(
            Point textureSize,
            int columns,
            int rows)
        {
            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }

            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException("rows");
            }

            return new Point(
                    textureSize.X / columns,
                    textureSize.Y / rows);
        }
    }
}
