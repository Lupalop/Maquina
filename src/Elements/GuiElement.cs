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
        }

        public Alignment ControlAlignment { get; set; }
        public override string Id
        {
            get { return "GENERIC_GUI"; }
        }
    }
}
