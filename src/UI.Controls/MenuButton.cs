using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Maquina.Elements;

namespace Maquina.UI
{
    public class MenuButton : GuiElement
    {
        public MenuButton(string objectName, SceneManager sceneManager)
            : base (objectName)
        {
            InputManager = sceneManager.InputManager;
            // Default MB graphic
            Graphic = sceneManager.Game.Content.Load<Texture2D>("menuBG");
            Font = sceneManager.Fonts["default"];
            SpriteType = SpriteType.Static;
            Rows = 1;
            Columns = 3;
            ClickSound = sceneManager.Game.Content.Load<SoundEffect>("sfx/click");
            IconAlignment = ControlAlignment.Center;
        }

        // Fields
        private InputManager InputManager;
        private Vector2 GraphicCenter;
        // Properties
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public Action LeftClickAction { get; set; }
        public Action RightClickAction { get; set; }
        public bool Disabled { get; set; }
        public SoundEffect ClickSound { get; set; }
        public Texture2D Icon { get; set; }
        public ControlAlignment IconAlignment { get; set; }
        public Vector2 IconLocation { get; set; }

        public override string ID
        {
            get { return "GUI_MENUBUTTON"; }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (Text != null)
            {
                SpriteBatch.DrawString(Font, Text, GraphicCenter, Tint, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
            }
            if (Icon != null)
            {
                SpriteBatch.Draw(Icon, IconLocation, null, Tint, 0f, Vector2.Zero, Scale, SpriteEffects.None, 1f);
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

        bool LeftClickFired;
        bool RightClickFired;
        public override void Update(GameTime gameTime)
        {
            CurrentFrame = 0;

            // Don't respond to any event if button is disabled
            if (!Disabled && InputManager.ShouldAcceptInput)
            {
                // If mouse is on top of the button
                if (Bounds.Contains(InputManager.MousePosition) && SpriteType != SpriteType.None)
                {
                    CurrentFrame = 1;
                }

                // If the button was clicked
                if ((InputManager.MouseDown(MouseButton.Left) ||
                     InputManager.MouseDown(MouseButton.Right) ||
                     InputManager.MouseDown(MouseButton.Middle)) &&
                     Bounds.Contains(InputManager.MousePosition) &&
                     SpriteType != SpriteType.None)
                {
                    CurrentFrame = 2;
                }

                // Left Mouse Button Click Action
                if (LeftClickAction != null)
                {
                    if (InputManager.MouseDown(MouseButton.Left) && Bounds.Contains(InputManager.MousePosition))
                        LeftClickFired = true;
                    if (InputManager.MouseDown(MouseButton.Left) && !Bounds.Contains(InputManager.MousePosition))
                        LeftClickFired = false;
                    if (InputManager.MouseUp(MouseButton.Left) && LeftClickFired)
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
                    if (InputManager.MouseDown(MouseButton.Right) && Bounds.Contains(InputManager.MousePosition))
                        RightClickFired = true;
                    if (InputManager.MouseDown(MouseButton.Right) && !Bounds.Contains(InputManager.MousePosition))
                        RightClickFired = false;
                    if (InputManager.MouseUp(MouseButton.Right) && RightClickFired)
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

        public override void UpdatePoints()
        {
            base.UpdatePoints();
            if (Text != null)
            {
                Vector2 TextLength = Font.MeasureString(Text);
                Vector2 NewTextLength = new Vector2(TextLength.X * Scale, TextLength.Y * Scale);
                GraphicCenter = new Vector2(Location.X + (Bounds.Width / 2) - NewTextLength.X / 2, Location.Y + Bounds.Height / 4);
            }

            if (Icon != null)
            {
                switch (IconAlignment)
                {
                    case ControlAlignment.Left:
                        IconLocation = new Vector2(Location.X,
                            Location.Y + (Bounds.Height / 2) - (Icon.Bounds.Height / 2));
                        break;
                    case ControlAlignment.Center:
                        IconLocation = new Vector2(Location.X + (Bounds.Width / 2) - (Icon.Bounds.Width / 2),
                            Location.Y + (Bounds.Height / 2) - (Icon.Bounds.Height / 2));
                        break;
                    case ControlAlignment.Right:
                        IconLocation = new Vector2(Location.X + Bounds.Width - Icon.Bounds.Width,
                            Location.Y + (Bounds.Height / 2) - (Icon.Bounds.Height / 2));
                        break;
                    case ControlAlignment.Fixed:
                        return;
                }
            }
        }
    }
}
