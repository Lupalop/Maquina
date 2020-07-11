using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Maquina.Entities;

namespace Maquina.UI
{
    public abstract class Scene : IDisposable
    {
        private EntityCollection _entities;
        private bool _contentLoaded;

        public Scene(string name, SpriteBatch spriteBatch)
        {
            Name = name;
            Entities = new EntityCollection();
            Application.Display.ScaleChanged += OnLayoutDirty;
            Application.Display.ResolutionChanged += OnLayoutDirty;
        }

        public Scene(string name)
            : this(name, Application.SpriteBatch)
        { }

        public Scene()
            : this("Untitled Scene")
        { }

        protected SpriteBatch SpriteBatch { get; set; }

        public EntityCollection Entities
        {
            get { return _entities; }
            set
            {
                if (_contentLoaded)
                {
                    throw new InvalidOperationException("Changing the entity collection after the scene content has been loaded is not allowed.");
                }
                if (_entities != null)
                {
                    _entities.CollectionChanged -= OnLayoutDirty;
                    _entities.EntityChanged -= OnLayoutDirty;
                }
                _entities = value;
                if (_entities != null)
                {
                    _entities.CollectionChanged += OnLayoutDirty;
                    _entities.EntityChanged += OnLayoutDirty;
                }
            }
        }

        public string Name { get; private set; }
        public bool Enabled { get; set; }

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
            _contentLoaded = true;
            if (ContentLoaded != null)
            {
                ContentLoaded(this, EventArgs.Empty);
            }
        }

        public abstract void Draw();
        public abstract void Update();

        protected void OnLayoutDirty(object sender, EventArgs e)
        {
            if (e is PropertyChangedEventArgs &&
                ((PropertyChangedEventArgs)e).Id == PropertyId.Location)
            {
                return;
            }

            foreach (Entity entity in Entities)
            {
                if (entity is Control)
                {
                    Control control = (Control)entity;
                    EntityUtils.AutoPosition(control);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Entities.Dispose();
#if MGE_LOGGING
                LogManager.Info(0, string.Format("Scene disposed: {0}", Name));
#endif
                if (Disposed != null)
                {
                    Disposed(this, EventArgs.Empty);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
