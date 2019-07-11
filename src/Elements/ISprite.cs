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
        Vector2 RotationOrigin { get; set; }
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
        bool IgnoreGlobalScale { get; set; }

        // Events
        event Action<Rectangle> DestinationRectangleChanged;
        event Action<Point> LocationChanged;
        event Action<Point> SizeChanged;

        // Draw and update methods
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }
}
