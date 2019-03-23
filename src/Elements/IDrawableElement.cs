using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Elements
{
    public interface IDrawableElement
    {
        // Graphics
        Texture2D Graphic { get; set; }
        Vector2 Location { get; set; }
        Vector2 Dimensions { get; set; }
        Color Tint { get; set; }
        float Rotation { get; set; }
        Vector2 RotationOrigin { get; set; }
        float Scale { get; set; }
        Rectangle DestinationRectangle { get; set; }
        Rectangle SourceRectangle { get; set; }
        SpriteEffects GraphicEffects { get; set; }
        float LayerDepth { get; set; }

        // Graphic (references)
        Rectangle Bounds { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        // For animated sprites
        SpriteType SpriteType { get; set; }
        int Rows { get; set; }
        int Columns { get; set; }

        // Draw action
        Action OnDraw { get; set; }
        void Draw(GameTime gameTime);
    }
}
