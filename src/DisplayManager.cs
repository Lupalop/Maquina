using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    // TODO: should contain resolution management, etc
    // quick job to have the ResolutionChanged event
    // most stuff are stubbed out
    public class DisplayManager : IDisposable
    {
        private GraphicsDeviceManager Graphics
        {
            get { return Application.Graphics; }
        }
        private GraphicsDevice GraphicsDevice
        {
            get { return Application.Game.GraphicsDevice; }
        }

        public DisplayManager()
        {
#if DEBUG
            Graphics.HardwareModeSwitch = !(bool)Application.Preferences[
                "app.window.fullscreen.borderless", true];
#else
            Graphics.HardwareModeSwitch = !(bool)Application.Preferences[
                "app.window.fullscreen.borderless", false];
#endif
            // Window
            Application.Game.IsMouseVisible = 
                (bool)Application.Preferences["app.window.useNativeCursor", false];
            UnmaximizedWindowBounds = new Point(
                (int)Application.Preferences["app.window.width", 800],
                (int)Application.Preferences["app.window.height", 600]);
            Application.Game.Window.AllowUserResizing =
                (bool)Application.Preferences["app.window.allowUserResizing", false];

            // Global Scale
            Scale = 1f;

            // Identify if we should go fullscreen
            if ((bool)Application.Preferences["app.window.fullscreen", false])
            {
                Graphics.PreferredBackBufferHeight = Application.Game.GraphicsDevice.DisplayMode.Height;
                Graphics.PreferredBackBufferWidth = Application.Game.GraphicsDevice.DisplayMode.Width;

                Graphics.ToggleFullScreen();
            }
            else
            {
                Graphics.PreferredBackBufferWidth = UnmaximizedWindowBounds.X;
                Graphics.PreferredBackBufferHeight = UnmaximizedWindowBounds.Y;
            }

            Graphics.ApplyChanges();
        }

        public Rectangle PreviousWindowBounds { get; private set; }
        public Rectangle WindowBounds { get; private set; }
        //
        public Point UnmaximizedWindowBounds { get; private set; }

        public void ToggleFullScreen()
        {
            if (Graphics.IsFullScreen)
            {
                Graphics.PreferredBackBufferHeight = UnmaximizedWindowBounds.Y;
                Graphics.PreferredBackBufferWidth = UnmaximizedWindowBounds.X;
            }
            else
            {
                UnmaximizedWindowBounds = new Point(
                    Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
                Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            }
            Graphics.ToggleFullScreen();
        }

        // Event handlers
        public event EventHandler ScaleChanged;
        private void OnScaleChanged()
        {
            if (ScaleChanged != null)
            {
                ScaleChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler ResolutionChanged;
        private void OnResolutionChanged()
        {
            if (ResolutionChanged != null)
            {
                ResolutionChanged(this, EventArgs.Empty);
            }
        }

        private float scale;
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                OnScaleChanged();
            }
        }

        public void Update()
        {
            if (PreviousWindowBounds != WindowBounds)
            {
                OnResolutionChanged();
            }
            PreviousWindowBounds = WindowBounds;
            WindowBounds = Application.Game.GraphicsDevice.Viewport.Bounds;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.Preferences["app.window.fullscreen"] =
                    Application.Graphics.IsFullScreen;
                // Save window dimensions if not in fullscreen
                if (!Graphics.IsFullScreen)
                {
                    Application.Preferences["app.window.width"] =
                        Application.Graphics.PreferredBackBufferWidth;
                    Application.Preferences["app.window.height"] =
                        Application.Graphics.PreferredBackBufferHeight;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
