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
            Entities = new EntityCollection();
            Application.Display.ScaleChanged += OnLayoutDirty;
            Application.Display.ResolutionChanged += OnLayoutDirty;
            Entities.CollectionChanged += OnLayoutDirty;
            Entities.EntityChanged += OnLayoutDirty;
        }

        public Scene() : this("Untitled Scene") { }

        protected Game Game { get { return Application.Game; } }
        protected SpriteBatch SpriteBatch { get { return Application.SpriteBatch; } }

        public EntityCollection Entities { get; private set; }
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
                    AutoPosition(control);
                }
            }
        }

        public void AutoPosition(
            Entity entity,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment)
        {
            Point newLocation = entity.Location;

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    newLocation.X = Application.Display.WindowBounds.Left;
                    break;
                case HorizontalAlignment.Center:
                    if (entity.ActualBounds.Width > 0)
                    {
                        newLocation.X = Application.Display.WindowBounds.Center.X - (entity.ActualBounds.Width / 2);
                    }
                    break;
                case HorizontalAlignment.Right:
                    newLocation.X = Application.Display.WindowBounds.Right - entity.ActualBounds.Width;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    newLocation.Y = Application.Display.WindowBounds.Top;
                    break;
                case VerticalAlignment.Center:
                    if (entity.ActualBounds.Height > 0)
                    {
                        newLocation.Y = Application.Display.WindowBounds.Center.Y - (entity.ActualBounds.Height / 2);
                    }
                    break;
                case VerticalAlignment.Bottom:
                    newLocation.Y = Application.Display.WindowBounds.Bottom - entity.ActualBounds.Height;
                    break;
            }

            entity.Location = newLocation;
        }

        public void AutoPosition(Control control)
        {
            if (control.AutoPosition)
            {
                AutoPosition(control, control.HorizontalAlignment, control.VerticalAlignment);
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
