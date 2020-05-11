using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

namespace Maquina.UI
{
    public partial class FadeOverlay : Overlay, IDisposable
    {
        public FadeOverlay(string overlayKey, Color fadeColor) : base("Fade Overlay")
        {
            OverlayKey = overlayKey;
            FadeColor = fadeColor;
            FadeSpeed = 0.1f;

            InitializeComponent();
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

        public override void Draw()
        {
            SpriteBatch.Begin();
            Entities.Draw();
            SpriteBatch.End();
        }

        public override void Update()
        {
            Entities.Update();
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
