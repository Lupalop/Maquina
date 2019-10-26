using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Elements
{
    public interface IBaseElement : IDisposable
    {
        string Name { get; set; }
        string Id { get; }

        void Update();
        void Draw();
    }
}
