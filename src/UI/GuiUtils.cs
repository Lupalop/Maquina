using Maquina.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public static class GuiUtils
    {
        public static void SetElementDisabledState(IDictionary<string, BaseElement> elements, bool disabled)
        {
            if (elements != null)
            {
                SetElementDisabledState(elements.Values, disabled);
            }
        }
        public static void SetElementDisabledState(IEnumerable<BaseElement> elements, bool disabled)
        {
            for (int i = 0; i < elements.Count(); i++)
            {
                BaseElement element = elements.ElementAt(i);
                if (element is GuiElement)
                {
                    ((GuiElement)(element)).Disabled = disabled;
                }
            }
        }

    }
}
