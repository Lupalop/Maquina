using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    // TODO: should contain resolution management, etc
    // quick job to have the ResolutionChanged event
    // most stuff are stubbed out
    public class DisplayManager
    {
        public DisplayManager()
        {
        }

        public event Action<Rectangle> ResolutionChanged;
        public Rectangle PreviousWindowBounds;
        public Rectangle WindowBounds;

        public void Update()
        {
            if (PreviousWindowBounds != WindowBounds && ResolutionChanged != null)
            {
                ResolutionChanged(WindowBounds);
            }
            PreviousWindowBounds = WindowBounds;
            WindowBounds = Global.Game.GraphicsDevice.Viewport.Bounds;
        }
    }
}
