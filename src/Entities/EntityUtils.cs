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
    }
}
