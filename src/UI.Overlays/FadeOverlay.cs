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
    public partial class FadeOverlay : Overlay, IDisposable
    {
        public FadeOverlay(string overlayKey, Color fadeColor) : base("Fade Overlay")
        {
            OverlayKey = overlayKey;
            FadeColor = fadeColor;
            FadeSpeed = 0.1f;
        }
        public FadeOverlay(string overlayKey) : this(overlayKey, Color.Black) { }

        private string OverlayKey;
        private Texture2D FadeBackground;
        public FadeInAnimation FadeInAnimation { get; private set; }
        public FadeOutAnimation FadeOutAnimation { get; private set; }
        public float FadeSpeed { get; set; }

        private Color fadeColor;
        public Color FadeColor
        {
            get { return fadeColor; }
            private set
            {
                fadeColor = value;
                FadeBackground = new Texture2D(Game.GraphicsDevice, 1, 1);
                FadeBackground.SetData(new Color[] { value });
            }
        }

        public override void LoadContent()
        {
            InitializeComponent();
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            GuiUtils.DrawElements(gameTime, Elements);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            GuiUtils.UpdateElements(gameTime, Elements);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FadeBackground.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
