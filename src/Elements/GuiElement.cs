using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public abstract class GuiElement : BaseElement
    {
        // Constructor
        protected GuiElement(string name) : base (name)
        {
            ControlAlignment = Alignment.Center;
            AutoPosition = false;
        }

        private Alignment controlAlignment;
        public Alignment ControlAlignment
        {
            get { return controlAlignment; }
            set
            {
                controlAlignment = value;
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Custom));
            }
        }
        public bool AutoPosition { get; set; }
        public override string Id
        {
            get { return "GENERIC_GUI"; }
        }
    }
}
