using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Elements
{
    public interface ISprite
    {
        // General
        Color Tint { get; set; }
        float Rotation { get; set; }
        Vector2 Origin { get; set; }
        SpriteEffects SpriteEffects { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        // Layout
        Rectangle DestinationRectangle { get; set; }
        Rectangle ActualDestinationRectangle { get; }
        Rectangle? SourceRectangle { get; set; }
        Point Location { get; set; }
        Point Size { get; set; }
        Point ActualSize { get; }
        float Scale { get; set; }
        float ActualScale { get; }
        float LayerDepth { get; set; }
        float Opacity { get; set; }
        bool IgnoreGlobalScale { get; set; }

        // Events
        event ElementChangedEventHandler SpriteChanged;

        // Draw and update methods
        void Draw();
        void Update();
    }
}
