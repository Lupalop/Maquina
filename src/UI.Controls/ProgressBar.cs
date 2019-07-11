using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI
{
    public class ProgressBar : Image
    {
        // FIXME: REWRITE PROGRESS BAR ELEMENT, USE BARSPRITE SUBELEMENT
        public ProgressBar(string name)
            : base(name)
        {
        }

        // General
        public override string Id
        {
            get { return "GUI_PROGRESSBAR"; }
        }
    }
}
