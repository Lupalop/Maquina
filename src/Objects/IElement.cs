using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Objects
{
    public interface IElement : IDisposable
    {
        // Basic properties
        string Name { get; set; }
        Collection<object> MessageHolder { get; set; }

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

        // Graphic (references)
        Rectangle Bounds { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        // Update and draw events (essential to removing individual update commands from scenes)
        Action OnDraw { get; set; }
        Action OnUpdate { get; set; }

        // For animated sprites
        SpriteType SpriteType { get; set; }
        int Rows { get; set; }
        int Columns { get; set; }

        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }

    public interface IUIElement : IElement, IDisposable
    {
        ControlAlignment ControlAlignment { get; set; }
    }

    public interface IGameElement : IElement, IDisposable
    {
        bool CampaignOnlyObject { get; set; }
        bool EditorVisibility { get; set; }
    }
}
