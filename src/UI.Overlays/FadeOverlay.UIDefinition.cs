using Maquina.Elements;
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
            FadeImage = new Image("Background")
            {
                IgnoreGlobalScale = true,
                Graphic = FadeBackground,
            };
            FadeImage.Background.DestinationRectangle = WindowBounds;

            Global.Display.ResolutionChanged += (sender, e) =>
            {
                FadeImage.Background.DestinationRectangle = ((DisplayManager)sender).WindowBounds;
            };

            FadeInAnimation = new FadeInAnimation(FadeImage.Background, FadeSpeed);
            FadeInAnimation.AnimationFinished += (sender, e) =>
            {
                FadeOutAnimation.Start();
            };

            FadeOutAnimation = new FadeOutAnimation(FadeImage.Background, FadeSpeed);
            FadeOutAnimation.AnimationFinished += (sender, e) =>
            {
                Global.Scenes.Overlays.Remove(OverlayKey);
            };

            FadeInAnimation.Start();

            Elements.Add(FadeImage.Name, FadeImage);
        }
    }
}
