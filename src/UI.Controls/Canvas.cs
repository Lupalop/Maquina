using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class Canvas : Control, IContainerElement
    {
        // FIXME: Elements inside a canvas element SHOULD be relative to the container's location.
        //        Sprite and BaseElement (and their children) have to be updated to handle this.
        public Canvas(string name) : base(name)
        {
            Id = "UI_CANVAS";
            Children = new ElementDictionary();
            IsScaleSupported = false;
        }

        public ElementDictionary Children { get; private set; }

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

        protected override void OnDisabledStateChanged()
        {
            GuiUtils.SetElementDisabledState(Children, Disabled);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var element in Children.Values)
                {
                    element.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
