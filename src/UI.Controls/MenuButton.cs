using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Maquina.Objects;
using Maquina.UI.Scenes;

namespace Maquina.UI.Controls
{
    public class MenuButton : GuiElement
    {
        public MenuButton(string objectName, SceneManager sceneManager)
            : base (objectName)
        {
            this.SceneManager = sceneManager;
            MouseState = sceneManager.MouseState;
            MouseOverlay = (MouseOverlay)sceneManager.Overlays["mouse"];
            // Default MB graphic
            Graphic = sceneManager.Game.Content.Load<Texture2D>("menuBG");
            Font = sceneManager.Fonts["default"];
            SpriteType = SpriteType.Static;
            Rows = 1;
            Columns = 3;
            ClickSound = sceneManager.Game.Content.Load<SoundEffect>("sfx/click");
        }

        private SceneManager SceneManager;
        public Vector2 GraphicCenter { get; set; }
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public MouseState MouseState { get; set; }
        public MouseOverlay MouseOverlay { get; set; }
        public Action LeftClickAction { get; set; }
        public Action RightClickAction { get; set; }
        public bool Disabled { get; set; }
        public SoundEffect ClickSound { get; set; }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (Text != null)
                SpriteBatch.DrawString(Font, Text, GraphicCenter, Tint, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
        }

        public override Vector2 Dimensions
        {
            get
            {
                if (Text != null && Graphic == null)
                {
                    return Font.MeasureString(Text);
                }
                return base.Dimensions;
            }
        }

        bool LeftClickFired;
        bool RightClickFired;
        public override void Update(GameTime gameTime)
        {
            // TODO: Support touch events (I don't have a real touch device unfortunately)
            if (Text != null)
            {
                Vector2 TextLength = Font.MeasureString(Text);
                GraphicCenter = new Vector2(Location.X + (Bounds.Width / 2) - TextLength.X / 2, Location.Y + Bounds.Height / 4);
            }
            MouseState = SceneManager.MouseState;
            CurrentFrame = 0;

            // Don't respond to any event if button is disabled
            if (!Disabled)
            {
                // If mouse is on top of the button
                if (Bounds.Contains(MouseOverlay.Bounds.Location) && SpriteType != SpriteType.None)
                {
                    CurrentFrame = 1;
                }

                // If the button was clicked
                if ((MouseState.LeftButton == ButtonState.Pressed ||
                     MouseState.RightButton == ButtonState.Pressed ||
                     MouseState.MiddleButton == ButtonState.Pressed) && Bounds.Contains(MouseOverlay.Bounds.Location) && SpriteType != SpriteType.None)
                {
                    CurrentFrame = 2;
                }

                // Left Mouse Button Click Action
                if (LeftClickAction != null)
                {
                    if (MouseState.LeftButton == ButtonState.Pressed && Bounds.Contains(MouseOverlay.Bounds.Location))
                        LeftClickFired = true;
                    if (MouseState.LeftButton == ButtonState.Pressed && !Bounds.Contains(MouseOverlay.Bounds.Location))
                        LeftClickFired = false;
                    if (MouseState.LeftButton == ButtonState.Released && LeftClickFired)
                    {
                        LeftClickAction.Invoke();
                        ClickSound.Play();
                        // In order to prevent the action from being fired again
                        LeftClickFired = false;
                    }
                }

                // Right Mouse Button Click Action
                if (RightClickAction != null)
                {
                    if (MouseState.RightButton == ButtonState.Pressed && Bounds.Contains(MouseOverlay.Bounds.Location))
                        RightClickFired = true;
                    if (MouseState.RightButton == ButtonState.Pressed && !Bounds.Contains(MouseOverlay.Bounds.Location))
                        RightClickFired = false;
                    if (MouseState.RightButton == ButtonState.Released && RightClickFired)
                    {
                        RightClickAction.Invoke();
                        ClickSound.Play();
                        // In order to prevent the action from being fired again
                        RightClickFired = false;
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}
