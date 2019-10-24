using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI
{
    public class FadeOverlay : Overlay, IDisposable
    {
        public FadeOverlay(string overlayKey)
            : base("Fade Overlay")
        {
            this.OverlayKey = overlayKey;
            this.FadeColor = Color.Black;
            this.FadeSpeed = 0.1f;
        }

        public FadeOverlay(string overlayKey, Color fadeColor)
            : base("Fade Overlay")
        {
            this.OverlayKey = overlayKey;
            this.FadeColor = fadeColor;
            this.FadeSpeed = 0.1f;
        }

        private float Opacity = 1f;
        private string OverlayKey;
        private Texture2D FadeBackground;
        public float FadeSpeed { get; set; }

        private Color _fadeColor;
        public Color FadeColor
        {
            get
            {
                return _fadeColor;
            }
            set
            {
                _fadeColor = value;
                FadeBackground = new Texture2D(Game.GraphicsDevice, 1, 1);
                FadeBackground.SetData(new Color[] { value });
            }
        }

        public override void LoadContent()
        {
            Image Background = new Image("Background")
            {
                IgnoreGlobalScale = true,
            };
            Background.ElementUpdated += (sender, e) =>
            {
                Background.Graphic = FadeBackground;
                Background.Background.DestinationRectangle = Game.GraphicsDevice.Viewport.Bounds;
                Background.Background.Tint = FadeColor * Opacity;
            };
            Elements.Add(Background.Name, Background);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawElements(gameTime, Elements);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            Opacity -= FadeSpeed;

            base.Update(gameTime);
            base.UpdateElements(gameTime, Elements);

            // Remove overlay when opacity below 0
            if (Opacity <= 0f)
            {
                SceneManager.Overlays.Remove(OverlayKey);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                FadeBackground.Dispose();
            }
        }
    }
}
