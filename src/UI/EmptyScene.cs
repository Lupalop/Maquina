using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class EmptyScene : Scene
    {
        public EmptyScene() : base() { }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
