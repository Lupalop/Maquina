using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Maquina.Entities;

namespace Maquina.UI
{
    public abstract class Scene : IDisposable
    {
        public Scene(string sceneName)
        {
            Name = sceneName;
            Entities = new EntityDictionary();
        }
        public Scene() : this("Untitled Scene") { }

        protected Game Game { get { return Application.Game; } }
        protected SpriteBatch SpriteBatch { get { return Application.SpriteBatch; } }

        public EntityDictionary Entities { get; private set; }
        public string Name { get; private set; }

        public bool IsFrozen { get; internal set; }

        public event EventHandler ContentLoaded;
        public event EventHandler Disposed;

        protected Rectangle Bounds
        {
            get { return Application.Display.WindowBounds; }
        }

        public virtual void LoadContent()
        {
#if MGE_LOGGING
            LogManager.Info(0, string.Format("Content loaded from: {0}", Name));
#endif
            if (ContentLoaded != null)
            {
                ContentLoaded(this, EventArgs.Empty);
            }
        }

        public abstract void Draw();
        public abstract void Update();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Entities.Clear(true);
#if MGE_LOGGING
                LogManager.Info(0, string.Format("Scene disposed: {0}", Name));
#endif
                if (Disposed != null)
                {
                    Disposed(this, EventArgs.Empty);
                }
            }
        }
    }
}
