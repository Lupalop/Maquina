using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Maquina.Entities;

namespace Maquina.UI
{
    public class Button : Control
    {
        private Texture2D _texture;
        private SpriteFont _font;
        private string _text;

        public Button(string name) : base (name)
        {
            Texture = (Texture2D)ContentFactory.TryGetResource("button-default");
            Size = AtlasUtils.GetFrameSize(
                _texture.Bounds.Size,
                3,
                1);
            DrawController.SourceRectangle = AtlasUtils.CreateSourceFrameRectangle(
                _texture.Bounds.Size,
                3,
                1,
                0);

            ClickSound = (SoundEffect)ContentFactory.TryGetResource("click_default");
            Font = (SpriteFont)ContentFactory.TryGetResource("default");
            Text = "";
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Bounds = _texture.Bounds;
                DrawController.SourceRectangle = null;
            }
        }

        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
#if MGE_LOCALE
                _text = Application.Locale.TryGetString(value);
#else
                text = value;
#endif
            }
        }

        private void UpdateSize()
        {

        }

        public event EventHandler OnLeftClick;
        public event EventHandler OnRightClick;

        public SoundEffect ClickSound { get; set; }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                ActualBounds,
                DrawController.SourceRectangle,
                DrawController.Tint * DrawController.Opacity,
                DrawController.Rotation,
                DrawController.Origin,
                DrawController.SpriteEffects,
                DrawController.LayerDepth);
            spriteBatch.DrawString(
                Font,
                Text,
                LabelLocation.ToVector2(),
                DrawController.Tint * DrawController.Opacity,
                DrawController.Rotation,
                DrawController.Origin,
                ActualScale,
                DrawController.SpriteEffects,
                DrawController.LayerDepth);
        }

        bool LeftClickFired;
        bool RightClickFired;
        public override void Update()
        {
            DrawController.SourceRectangle = AtlasUtils.CreateSourceFrameRectangle(
                _texture.Bounds.Size,
                3,
                1,
                0);

            // Don't respond to any event if button is disabled
            if (!Disabled && Application.Input.ShouldAcceptInput)
            {
                // If mouse is on top of the button
                if (ActualBounds.Contains(Application.Input.MousePosition))
                {
                    DrawController.SourceRectangle = AtlasUtils.CreateSourceFrameRectangle(
                        _texture.Bounds.Size,
                        3,
                        1,
                        1);
                }

                // If the button was clicked
                if ((Application.Input.MouseDown(MouseButton.Left) ||
                     Application.Input.MouseDown(MouseButton.Right) ||
                     Application.Input.MouseDown(MouseButton.Middle)))
                {
                    Focused = false;
                    if (ActualBounds.Contains(Application.Input.MousePosition))
                    {
                        Focused = true;
                        DrawController.SourceRectangle = AtlasUtils.CreateSourceFrameRectangle(
                            _texture.Bounds.Size,
                            3,
                            1,
                            2);
                    }
                }

                // Left Mouse Button Click Action
                if (OnLeftClick != null)
                {
                    if (Application.Input.MouseDown(MouseButton.Left) && ActualBounds.Contains(Application.Input.MousePosition))
                        LeftClickFired = true;
                    if (Application.Input.MouseDown(MouseButton.Left) && !ActualBounds.Contains(Application.Input.MousePosition))
                        LeftClickFired = false;
                    if (Application.Input.MouseUp(MouseButton.Left) && LeftClickFired)
                    {
                        OnLeftClick(this, EventArgs.Empty);
                        ClickSound.Play();
                        // In order to prevent the action from being fired again
                        LeftClickFired = false;
                    }
                }

                // Right Mouse Button Click Action
                if (OnRightClick != null)
                {
                    if (Application.Input.MouseDown(MouseButton.Right) && ActualBounds.Contains(Application.Input.MousePosition))
                        RightClickFired = true;
                    if (Application.Input.MouseDown(MouseButton.Right) && !ActualBounds.Contains(Application.Input.MousePosition))
                        RightClickFired = false;
                    if (Application.Input.MouseUp(MouseButton.Right) && RightClickFired)
                    {
                        OnRightClick(this, EventArgs.Empty);
                        ClickSound.Play();
                        // In order to prevent the action from being fired again
                        RightClickFired = false;
                    }
                }
            }
        }

        private Point LabelSize
        {
            get
            {
                if (Font != null && Text.Trim() != null)
                {
                    return Font.MeasureString(Text).ToPoint();
                }
                return Point.Zero;
            }
        }

        private Point LabelLocation
        {
            get
            {
                return new Point(
                    ActualBounds.Center.X - (int)(LabelSize.X * ActualScale / 2),
                    ActualBounds.Center.Y - (int)(LabelSize.Y * ActualScale / 2));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}
