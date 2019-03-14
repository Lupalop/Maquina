using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Elements
{
    public interface IBaseElement : IDisposable
    {
        // Basic properties
        string Name { get; set; }
        string ID { get; }
        // TODO: Remove in the future. Must be moved to app code.
        Collection<object> MessageHolder { get; set; }

        // Update action
        Action OnUpdate { get; set; }
        void Update(GameTime gameTime);
    }
}
