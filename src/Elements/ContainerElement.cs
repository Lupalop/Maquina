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
            Children = new ObservableDictionary<string, BaseElement>();
            IsScaleSupported = false;
        }

        public ObservableDictionary<string, BaseElement> Children { get; set; }

        public override void Draw()
        {
            foreach (BaseElement element in Children.Values)
            {
                element.Draw();
            }

            base.Draw();
        }

        public override void Update()
        {
            foreach (BaseElement element in Children.Values)
            {
                element.Update();
            }

            base.Update();
        }
    }
}
