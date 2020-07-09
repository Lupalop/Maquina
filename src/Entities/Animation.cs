using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Entities
{
    public abstract class Animation : IAnimation, IDisposable
    {
        public Animation(Entity target, float speed)
        {
            Target = target;
            Speed = speed;
        }

        public float Speed { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsRepeating { get; set; }
        public Entity Target { get; private set; }

        public event EventHandler AnimationFinished;
        public event EventHandler AnimationStarted;

        protected virtual void OnAnimationStarted()
        {
            if (AnimationStarted != null)
            {
                AnimationStarted(this, EventArgs.Empty);
            }
        }
        protected virtual void OnAnimationFinished()
        {
            if (AnimationFinished != null)
            {
                AnimationFinished(this, EventArgs.Empty);
            }
            Dispose();
        }

        public void Start()
        {
            if (IsRunning)
            {
                IsRunning = false;
                return;
            }
            IsRunning = true;
            OnAnimationStarted();

            AnimationManager.Add(this);
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public abstract void Update();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsRunning = false;
                IsRepeating = false;
                AnimationManager.Remove(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
