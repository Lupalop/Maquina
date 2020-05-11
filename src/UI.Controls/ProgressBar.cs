using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

namespace Maquina.UI
{
    public class ProgressBar : Control
    {
        // FIXME: REWRITE PROGRESS BAR ENTITY, USE BARSPRITE
        public ProgressBar(string name) : base(name)
        {
            Id = "UI_PROGRESSBAR";
        }
    }
}
