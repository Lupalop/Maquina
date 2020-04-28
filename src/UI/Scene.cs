using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Maquina.Elements;

namespace Maquina.UI
{
    public abstract class Scene : IDisposable
    {
        public Scene(string sceneName)
        {
            Name = sceneName;
            Elements = new ElementDictionary();
        }
        public Scene() : this("Untitled Scene") { }

        protected Game Game { get { return Application.Game; } }
        protected SpriteBatch SpriteBatch { get { return Application.SpriteBatch; } }

        public ElementDictionary Elements { get; private set; }
        public string Name { get; private set; }

        internal bool _stopUpdating = false;

        public event EventHandler ContentLoaded;
        public event EventHandler Disposed;

        protected Rectangle WindowBounds
        {
            get { return Application.Display.WindowBounds; }
        }

        public virtual void LoadContent()
        {
#if LOG_ENABLED
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
                Elements.Clear(true);
#if LOG_ENABLED
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
