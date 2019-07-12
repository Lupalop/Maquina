﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public interface IContainerElement
    {
        ObservableDictionary<string, BaseElement> Children { get; set; }
    }
}
