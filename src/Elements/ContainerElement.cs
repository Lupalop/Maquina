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
            Children = new ObservableDictionary<string, BaseElement>();
            IsScaleSupported = false;
        }

        public ObservableDictionary<string, BaseElement> Children { get; set; }
        public override string Id
        {
            get { return "GENERIC_CONTAINER"; }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (BaseElement element in Children.Values)
            {
                element.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (BaseElement element in Children.Values)
            {
                element.Update(gameTime);
            }

            base.Update(gameTime);
        }
    }
}
