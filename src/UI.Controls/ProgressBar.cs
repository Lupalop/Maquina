using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI
{
    public class ProgressBar : GuiElement
    {
        // FIXME: REWRITE PROGRESS BAR ELEMENT, USE BARSPRITE SUBELEMENT
        public ProgressBar(string name) : base(name)
        {
            Id = "GUI_PROGRESSBAR";
        }
    }
}
