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
            InputManager = sceneManager.InputManager;
            // Default MB graphic
            Graphic = sceneManager.Game.Content.Load<Texture2D>("menuBG");
            Font = sceneManager.Fonts["default"];
            SpriteType = SpriteType.Static;
            Rows = 1;
            Columns = 3;
            ClickSound = sceneManager.Game.Content.Load<SoundEffect>("sfx/click");
        }

        // Fields
        private SceneManager SceneManager;
        private InputManager InputManager;
        private Vector2 GraphicCenter;
        // Properties
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public Action LeftClickAction { get; set; }
        public Action RightClickAction { get; set; }
        public bool Disabled { get; set; }
        public SoundEffect ClickSound { get; set; }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (Text != null)
            {
                SpriteBatch.DrawString(Font, Text, GraphicCenter, Tint, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
            }
        }

        private Vector2 dimensions;
        public override Vector2 Dimensions
        {
            get
            {
                if (Text != null && Graphic == null)
                {
                    Vector2 CurrentDimensions = Font.MeasureString(Text);
                    return new Vector2(CurrentDimensions.X * Scale, CurrentDimensions.Y * Scale);
                }
                return dimensions;
            }
            set
            {
                dimensions = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            CurrentFrame = 0;

            // Don't respond to any event if button is disabled
            if (!Disabled && InputManager.ShouldAcceptInput && Bounds.Contains(InputManager.MousePosition))
            {
                // If mouse is on top of the button
                if (SpriteType != SpriteType.None)
                {
                    CurrentFrame = 1;
                }

                // If the button was clicked
                if ((InputManager.MousePressed(MouseButton.Left) ||
                     InputManager.MousePressed(MouseButton.Right) ||
                     InputManager.MousePressed(MouseButton.Middle)) && SpriteType != SpriteType.None)
                {
                    CurrentFrame = 2;
                }
                
                // Left Mouse Button Click Action
                if (LeftClickAction != null && InputManager.MousePressed(MouseButton.Left))
                {
                    LeftClickAction.Invoke();
                    ClickSound.Play();
                }

                // Right Mouse Button Click Action
                if (RightClickAction != null && InputManager.MousePressed(MouseButton.Right))
                {
                    RightClickAction.Invoke();
                    ClickSound.Play();
                }
            }

            base.Update(gameTime);
        }

        public override void UpdatePoints()
        {
            base.UpdatePoints();
            if (Text != null)
            {
                Vector2 TextLength = Font.MeasureString(Text);
                Vector2 NewTextLength = new Vector2(TextLength.X * Scale, TextLength.Y * Scale);
                GraphicCenter = new Vector2(Location.X + (Bounds.Width / 2) - NewTextLength.X / 2, Location.Y + Bounds.Height / 4);
            }
        }
    }
}
