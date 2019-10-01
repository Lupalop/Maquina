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
            get { return Global.Graphics; }
        }
        private GraphicsDevice GraphicsDevice
        {
            get { return Global.Game.GraphicsDevice; }
        }

        public DisplayManager()
        {
#if DEBUG
            Graphics.HardwareModeSwitch = !Global.Preferences.GetBoolPreference("app.window.fullscreen.borderless", true);
#else
            Graphics.HardwareModeSwitch = !Global.Preferences.GetBoolPreference("app.window.fullscreen.borderless", false);
#endif
            // Window
            Global.Game.IsMouseVisible = Global.Preferences.GetBoolPreference("app.window.useNativeCursor", false);
            UnmaximizedWindowBounds = new Point(
                Global.Preferences.GetIntPreference("app.window.width", 800),
                Global.Preferences.GetIntPreference("app.window.height", 600));
            Global.Game.Window.AllowUserResizing =
                Global.Preferences.GetBoolPreference("app.window.allowUserResizing", false);


            // Identify if we should go fullscreen
            if (Global.Preferences.GetBoolPreference("app.window.fullscreen", false))
            {
                Graphics.PreferredBackBufferHeight = Global.Game.GraphicsDevice.DisplayMode.Height;
                Graphics.PreferredBackBufferWidth = Global.Game.GraphicsDevice.DisplayMode.Width;

                Graphics.ToggleFullScreen();
            }
            else
            {
                Graphics.PreferredBackBufferWidth = UnmaximizedWindowBounds.X;
                Graphics.PreferredBackBufferHeight = UnmaximizedWindowBounds.Y;
            }

            Graphics.ApplyChanges();
        }

        public event Action<Rectangle> ResolutionChanged;
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

        public void Update()
        {
            if (PreviousWindowBounds != WindowBounds && ResolutionChanged != null)
            {
                ResolutionChanged(WindowBounds);
            }
            PreviousWindowBounds = WindowBounds;
            WindowBounds = Global.Game.GraphicsDevice.Viewport.Bounds;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Global.Preferences.SetBoolPreference("app.window.fullscreen", Global.Graphics.IsFullScreen);
                // Save window dimensions if not in fullscreen
                if (!Graphics.IsFullScreen)
                {
                    Global.Preferences.SetIntPreference("app.window.width", Global.Graphics.PreferredBackBufferWidth);
                    Global.Preferences.SetIntPreference("app.window.height", Global.Graphics.PreferredBackBufferHeight);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
