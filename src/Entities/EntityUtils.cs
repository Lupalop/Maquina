using Maquina.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Maquina.Entities
{
    public static class EntityUtils
    {
        private static List<Rectangle> _scissorRectangles;
        private static int _scissorDepth;

        static EntityUtils()
        {
            _scissorRectangles = new List<Rectangle>();
            _scissorDepth = -1;

            ScissorTestRasterizerState = new RasterizerState()
            {
                ScissorTestEnable = true
            };
        }

        public static readonly RasterizerState ScissorTestRasterizerState;

        public static void BeginScissor(Rectangle scissorRectangle)
        {
            _scissorDepth++;
            _scissorRectangles.Add(Application.GraphicsDevice.ScissorRectangle);
            Application.GraphicsDevice.ScissorRectangle = scissorRectangle;
        }

        public static void EndScissor()
        {
            if (_scissorDepth < 0 || _scissorDepth > _scissorRectangles.Count)
            {
                throw new InvalidOperationException();
            }
            Application.GraphicsDevice.ScissorRectangle = _scissorRectangles[_scissorDepth];
            _scissorRectangles.RemoveAt(_scissorDepth);
            _scissorDepth--;
        }

        public static void AutoPosition(
            Entity entity,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment)
        {
            Point newLocation = entity.Location;

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    newLocation.X = Application.Display.WindowBounds.Left;
                    break;
                case HorizontalAlignment.Center:
                    if (entity.ActualBounds.Width > 0)
                    {
                        newLocation.X = Application.Display.WindowBounds.Center.X - (entity.ActualBounds.Width / 2);
                    }
                    break;
                case HorizontalAlignment.Right:
                    newLocation.X = Application.Display.WindowBounds.Right - entity.ActualBounds.Width;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    newLocation.Y = Application.Display.WindowBounds.Top;
                    break;
                case VerticalAlignment.Center:
                    if (entity.ActualBounds.Height > 0)
                    {
                        newLocation.Y = Application.Display.WindowBounds.Center.Y - (entity.ActualBounds.Height / 2);
                    }
                    break;
                case VerticalAlignment.Bottom:
                    newLocation.Y = Application.Display.WindowBounds.Bottom - entity.ActualBounds.Height;
                    break;
            }

            entity.Location = newLocation;
        }

        public static void AutoPosition(Control control)
        {
            if (control.AutoPosition)
            {
                AutoPosition(control, control.HorizontalAlignment, control.VerticalAlignment);
            }
        }
    }
}
