using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Objects
{
    public class GuiElement : GenericElement, IUIElement
    {
        // Constructor
        protected GuiElement(string name) : base (name)
        {
            ControlAlignment = ControlAlignment.Center;
        }

        public ControlAlignment ControlAlignment { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
