﻿using Maquina.Elements;
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
            FadeImage.Sprite.Graphic = FadeBackground;
            FadeImage.Sprite.DestinationRectangle = WindowBounds;

            Global.Display.ResolutionChanged += (sender, e) =>
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
                Global.Scenes.Overlays.Remove(OverlayKey);
            };

            FadeInAnimation.Start();

            Elements.Add(FadeImage.Name, FadeImage);
        }
    }
}
