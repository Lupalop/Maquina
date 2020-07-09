using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class EntityContainer : Entity, IEntityContainer
    {
        public EntityContainer(string name) : base(name)
        {
            Children = new EntityCollection();
        }

        public EntityCollection Children { get; protected set; }

        public override bool IgnoreDisplayScale
        {
            get { return true; }
        }

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
