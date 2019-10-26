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
            Children = new ObservableDictionary<string, BaseElement>();
            DisabledStateChanged += Canvas_DisabledStateChanged;
            IsScaleSupported = false;
        }

        private ObservableDictionary<string, BaseElement> children;
        public ObservableDictionary<string, BaseElement> Children
        {
            get { return children; }
            set
            {
                children = value;
            }
        }

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
