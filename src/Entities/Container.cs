using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class Container : Entity, IContainer
    {
        public Container(string name) : base(name)
        {
            Children = new EntityDictionary();
        }

        public EntityDictionary Children { get; protected set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Children.Draw(spriteBatch);
        }

        public override void Update()
        {
            Children.Update();
        }
    }
}
