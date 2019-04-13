using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Maquina
{
    public static class TimerManager
    {
        public static List<Timer> Timers = new List<Timer>();

        public static void Add(Timer timer)
        {
            Timers.Add(timer);
        }

        public static void Remove(Timer timer)
        {
            Timers.Remove(timer);
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < Timers.Count; i++)
            {
                Timers[i].Update(gameTime);
            }
        }
    }
}
