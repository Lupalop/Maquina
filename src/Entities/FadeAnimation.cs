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
        public FadeOutAnimation(Entity target, float speed) : base(target, speed) { }

        private float Opacity = 1;

        public override void Update()
        {
            Opacity -= Speed;
            Target.DrawController.Opacity = Opacity;
            if (Opacity <= 0)
            {
                OnAnimationFinished();
            }
        }
    }

    public class FadeInAnimation : Animation
    {
        public FadeInAnimation(Entity target, float speed) : base(target, speed) { }

        private float Opacity = 0;

        public override void Update()
        {
            Opacity += Speed;
            Target.DrawController.Opacity = Opacity;
            if (Opacity >= 1)
            {
                OnAnimationFinished();
            }
        }
    }
}
