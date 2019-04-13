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
    public class TextBox : GuiElement
    {
        public TextBox(string objectName) : base (objectName)
        {
            InputManager = Global.InputManager;
            // Default TB graphic
            Graphic = Global.Textures["textbox-default"];
            Font = Global.Fonts["o-default_m"];
            TooltipFont = Global.Fonts["o-default_m"];
            IconAlignment = ControlAlignment.Center;
            TooltipOpacity = Color.Transparent;
            Global.Game.Window.TextInput += Window_TextInput;
            Text = "";
            MaxInput = 30;
            OnInput = delegate { };
        }

        // Fields
        private InputManager InputManager;
        private Vector2 TextLocation;
        private Vector2 TooltipLocation;
        private Color TooltipOpacity;
        // Properties
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public bool Disabled { get; set; }
        public Texture2D Icon { get; set; }
        public ControlAlignment IconAlignment { get; set; }
        public Vector2 IconLocation { get; set; }
        public string Tooltip { get; set; }
        public SpriteFont TooltipFont { get; set; }
        public int MaxInput { get; set; }

        public Action OnInput { get; set; }

        public override string ID
        {
            get { return "GUI_TEXTBOX"; }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (Text != null)
            {
                SpriteBatch.DrawString(Font, Text, TextLocation, Tint, Rotation, RotationOrigin, Scale, GraphicEffects, LayerDepth - 0.1f);
            }
            if (Icon != null)
            {
                SpriteBatch.Draw(Icon, IconLocation, null, Tint, Rotation, RotationOrigin, Scale, SpriteEffects.None, LayerDepth - 0.1f);
            }
            if (Tooltip != null)
            {
                // Tooltips 'probably' don't need rotation
                SpriteBatch.DrawString(TooltipFont, Tooltip, TooltipLocation, TooltipOpacity, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
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

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            OnInput();
            if (InputManager.ReservedKeys.Contains(e.Key) || Text.Length > MaxInput)
            {
                return;
            }
            Text += e.Character;
        }

        public override void Update(GameTime gameTime)
        {
            CurrentFrame = 0;

            // Don't respond to any event if button is disabled
            if (!Disabled && InputManager.ShouldAcceptInput)
            {
                // Tooltips
                if (Bounds.Contains(InputManager.MousePosition) && SpriteType != SpriteType.None)
                {
                    if (Tooltip != null)
                    {
                        Vector2 MousePosition = InputManager.MousePosition.ToVector2();
                        TooltipLocation = new Vector2(MousePosition.X + 20, MousePosition.Y + 5);
                        TooltipOpacity = Color.White;
                    }
                }
                else
                {
                    TooltipOpacity = Color.Transparent;
                }

                // TODO: Concept of focus
                if (InputManager.KeyPressed(Keys.Back) && Text.Length > 0)
                {
                    Text = Text.Remove(MathHelper.Clamp(Text.Length - 1, 0, Int32.MaxValue), 1);
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
                TextLocation = new Vector2(Location.X + (Bounds.Width / 2) - NewTextLength.X / 2, Location.Y + Bounds.Height / 4);
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
