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
        public override void Draw()
        {
            Application.GraphicsDevice.Clear(Color.Black);
        }
        public override void Update() { }
    }
}
