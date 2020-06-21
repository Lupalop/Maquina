using Maquina.Entities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public partial class FadeOverlay
    {
        Image FadeImage;

        private void InitializeComponent()
        {
            FadeImage = new Image("Background");
            FadeImage.IgnoreGlobalScale = true;
            FadeImage.Sprite.Texture = FadeBackground;
            FadeImage.Sprite.DestinationRectangle = Bounds;

            Application.Display.ResolutionChanged += (sender, e) =>
            {
                FadeImage.Sprite.DestinationRectangle = ((DisplayManager)sender).WindowBounds;
            };

            FadeInAnimation = new FadeInAnimation(FadeImage.Sprite, FadeSpeed);
            FadeInAnimation.AnimationFinished += (sender, e) =>
            {
                FadeOutAnimation.Start();
            };

            FadeOutAnimation = new FadeOutAnimation(FadeImage.Sprite, FadeSpeed);
            FadeOutAnimation.AnimationFinished += (sender, e) =>
            {
                Application.Scenes.Overlays.Remove(this);
            };

            FadeInAnimation.Start();

            Entities.Add(FadeImage);
        }
    }
}
