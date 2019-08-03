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
        public static void DisableAllMenuButtons(IDictionary<string, BaseElement> objects)
        {
            if (objects != null)
            {
                DisableAllMenuButtons(objects.Values.ToArray());
            }
        }
        public static void DisableAllMenuButtons(IEnumerable<BaseElement> objects)
        {
            for (int i = 0; i < objects.Count(); i++)
            {
                MenuButton mb = objects.ElementAt(i) as MenuButton;
                if (mb != null)
                {
                    mb.Disabled = true;
                }
            }
        }

    }
}
