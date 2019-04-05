using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI
{
    public class MouseOverlay : OverlayBase
    {
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
                    OnUpdate = (element) =>
                    {
                        element.Location = InputManager.MousePosition.ToVector2();
                        element.Scale = Platform.GlobalScale;
                        element.CurrentFrame = 0;
                        // Change state when selected
                        if ((InputManager.MouseDown(MouseButton.Left) ||
                             InputManager.MouseDown(MouseButton.Right) ||
                             InputManager.MouseDown(MouseButton.Middle)) &&
                            InputManager.ShouldAcceptInput)
                        {
                            element.CurrentFrame = 1;
                        }
                    }
                }}
            };
        }

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
