using Maquina.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class AnimationManager
    {
#if DEBUG
        public static bool IsFrozen { get; set; }
#endif
        public static List<Animation> Animations = new List<Animation>();

        public static void Add(Animation animation)
        {
            Animations.Add(animation);
        }

        public static void Remove(Animation timer)
        {
            Animations.Remove(timer);
        }

        public static void Update()
        {
#if DEBUG
            if (IsFrozen)
            {
                return;
            }
#endif
            for (int i = 0; i < Animations.Count; i++)
            {
                Animations[i].Update();
            }
        }
    }
}
