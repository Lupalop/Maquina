using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    // TODO: Remove this class
    public static class GuiUtils
    {
        // TODO: Replace with state flags and move handling to element collection

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
                if (element is Control)
                {
                    ((Control)(element)).Disabled = disabled;
                }
            }
        }
    }
}
