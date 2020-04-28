using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class Canvas : GuiElement, IContainerElement
    {
        // FIXME: Elements inside a canvas element SHOULD be relative to the container's location.
        //        Sprite and BaseElement (and their children) have to be updated to handle this.
        public Canvas(string name) : base(name)
        {
            Id = "GUI_CANVAS";
            Children = new ElementDictionary();
            DisabledStateChanged += Canvas_DisabledStateChanged;
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

        private void Canvas_DisabledStateChanged(object sender, EventArgs e)
        {
            GuiUtils.SetElementDisabledState(Children, Disabled);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisabledStateChanged -= Canvas_DisabledStateChanged;
                foreach (var element in Children.Values)
                {
                    element.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
