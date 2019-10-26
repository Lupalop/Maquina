using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Elements
{
    public interface IAnimation
    {
        float Speed { get; }
        bool IsRunning { get; }
        bool IsRepeating { get; }
        ISprite Target { get; }

        void Start();
        void Stop();

        event EventHandler AnimationStarted;
        event EventHandler AnimationFinished;
    }
}
