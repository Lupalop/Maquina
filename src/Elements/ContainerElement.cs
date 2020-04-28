using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public class ContainerElement : BaseElement, IContainerElement
    {
        public ContainerElement(string name) : base(name)
        {
            Id = "GENERIC_CONTAINER";
            Children = new ElementDictionary();
            IsScaleSupported = false;
        }

        public ElementDictionary Children { get; protected set; }

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
