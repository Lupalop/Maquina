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
            HorizontalAlignment = Alignment.Center;
            VerticalAlignment = Alignment.Center;
            AutoPosition = false;
        }

        [Obsolete("Use the HorizontalAlignment and/or VerticalAlignment property instead.", true)]
        public Alignment ControlAlignment;

        private Alignment horizontalAlignment;
        public Alignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set
            {
                horizontalAlignment = value;
                OnElementChanged(new ElementChangedEventArgs(ElementChangedProperty.Custom));
            }
        }
        private Alignment verticalAlignment;
        public Alignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                verticalAlignment = value;
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
