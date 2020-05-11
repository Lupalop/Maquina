using Microsoft.Xna.Framework;
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
            Id = "CONTAINER";
            Children = new EntityDictionary();
            IsScaleSupported = false;
        }

        public EntityDictionary Children { get; protected set; }

        public override void Draw()
        {
            Children.Draw();

            base.Draw();
        }

        public override void Update()
        {
            Children.Update();

            base.Update();
        }
    }
}
