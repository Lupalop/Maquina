using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public class FadeOutAnimation : Animation
    {
        public FadeOutAnimation(DrawController drawParameters, float speed) : base(drawParameters, speed) { }

        private float Opacity = 1;

        public override void Update()
        {
            Opacity -= Speed;
            Target.Opacity = Opacity;
            if (Opacity <= 0)
            {
                OnAnimationFinished();
            }
        }
    }

    public class FadeInAnimation : Animation
    {
        public FadeInAnimation(DrawController drawParameters, float speed) : base(drawParameters, speed) { }

        private float Opacity = 0;

        public override void Update()
        {
            Opacity += Speed;
            Target.Opacity = Opacity;
            if (Opacity >= 1)
            {
                OnAnimationFinished();
            }
        }
    }
}
