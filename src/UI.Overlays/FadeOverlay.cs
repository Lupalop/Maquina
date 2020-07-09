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
        private Image _fadeImage;
        private string _overlayKey;
        private Texture2D _fadeBackground;
        private Color _fadeColor;

        public FadeOverlay(string overlayKey, Color fadeColor) : base("Fade Overlay")
        {
            _overlayKey = overlayKey;
            FadeColor = fadeColor;
            FadeSpeed = 0.1f;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _fadeImage = new Image("Background");
            _fadeImage.IgnoreDisplayScale = true;
            _fadeImage.Sprite = _fadeBackground;
            _fadeImage.Bounds = Bounds;

            Application.Display.ResolutionChanged += (sender, e) =>
            {
                _fadeImage.Bounds = ((DisplayManager)sender).WindowBounds;
            };

            FadeInAnimation = new FadeInAnimation(_fadeImage, FadeSpeed);
            FadeInAnimation.AnimationFinished += (sender, e) =>
            {
                FadeOutAnimation.Start();
            };

            FadeOutAnimation = new FadeOutAnimation(_fadeImage, FadeSpeed);
            FadeOutAnimation.AnimationFinished += (sender, e) =>
            {
                Application.Scenes.Overlays.Remove(this);
            };

            FadeInAnimation.Start();

            Entities.Add(_fadeImage);
        }

        public FadeInAnimation FadeInAnimation { get; protected set; }
        public FadeOutAnimation FadeOutAnimation { get; protected set; }

        public FadeOverlay(string overlayKey) : this(overlayKey, Color.Black) { }

        public float FadeSpeed { get; set; }

        public Color FadeColor
        {
            get { return _fadeColor; }
            private set
            {
                _fadeColor = value;
                _fadeBackground = new Texture2D(Game.GraphicsDevice, 1, 1);
                _fadeBackground.SetData(new Color[] { value });
            }
        }

        public override void Draw()
        {
            SpriteBatch.Begin();
            Entities.Draw(SpriteBatch);
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
                _fadeBackground.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
