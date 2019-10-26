using Maquina.Elements;
using Microsoft.Xna.Framework;
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
        public static void DisposeElements(IDictionary<string, BaseElement> objects)
        {
            DisposeElements(objects.Values);
        }
        public static void DisposeElements(IEnumerable<BaseElement> objects)
        {
            for (int i = 0; i < objects.Count(); i++)
            {
                objects.ElementAt(i).Dispose();
            }
        }

        public static void DrawElements(IDictionary<string, BaseElement> objects)
        {
            DrawElements(objects.Values);
        }
        public static void DrawElements(IEnumerable<BaseElement> objects)
        {
            // Draw elements in the element array
            for (int i = 0; i < objects.Count(); i++)
            {
                try
                {
                    objects.ElementAt(i).Draw();
                }
                catch (NullReferenceException)
                {
                    // Suppress errors
                }
            }
        }

        public static void UpdateElements(IDictionary<string, BaseElement> elements)
        {
            UpdateElements(elements.Values);
        }
        public static void UpdateElements(IEnumerable<BaseElement> elements)
        {
            for (int i = 0; i < elements.Count(); i++)
            {
                BaseElement element = elements.ElementAt(i);
                element.Update();
            }
        }
    }
}
