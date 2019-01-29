using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Objects;
using Maquina.UI.Controls;

namespace Maquina.UI.Scenes
{
    public class MouseOverlay : OverlayBase {
    
        public MouseOverlay(SceneManager sceneManager, Texture2D mouseSprite)
            : base(sceneManager, "Mouse Overlay")
        {
            Objects = new Dictionary<string, GenericElement> {
                { "Mouse", new Image("mouse")
                {
                    SpriteBatch = this.SpriteBatch, 
                    Graphic = mouseSprite,
                    SpriteType = SpriteType.Static,
                    ControlAlignment = ControlAlignment.Fixed,
                    Location = InputManager.MouseState.Position.ToVector2(),
                    Rows = 1,
                    Columns = 2,
                    OnUpdate = () =>
                    {
                        Vector2 MousePosition = InputManager.MouseState.Position.ToVector2();
                        GenericElement Mouse = Objects["Mouse"];

                        Mouse.Location = new Vector2(
                            MathHelper.Clamp(MousePosition.X, 0, Game.GraphicsDevice.Viewport.Bounds.Right - Mouse.Dimensions.X), 
                            MathHelper.Clamp(MousePosition.Y, 0, Game.GraphicsDevice.Viewport.Bounds.Bottom - Mouse.Dimensions.Y));
                        Bounds = Mouse.Bounds;

                        Mouse.CurrentFrame = 0;
                        // Change state when selected
                        if ((InputManager.MouseState.LeftButton == ButtonState.Pressed ||
                            InputManager.MouseState.RightButton == ButtonState.Pressed) &&
                            InputManager.ShouldAcceptInput)
                        {
                            Mouse.CurrentFrame = 1;
                        }
                    }
                }}
            };
        }

        public Rectangle Bounds { get; set; }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {            
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }
    }
}
