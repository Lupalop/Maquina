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
        public Button(string name) : base (name)
        {
            BackgroundSprite = new TextureAtlasSprite(
                (Texture2D)ContentFactory.TryGetResource("button-default"), 3, 1);
            TextSprite = name;
            ClickSound = (SoundEffect)ContentFactory.TryGetResource("click_default");
        }

        public TextureSprite BackgroundSprite { get; set; }
        public TextSprite TextSprite { get; set; }

        public event EventHandler OnLeftClick;
        public event EventHandler OnRightClick;

        public SoundEffect ClickSound { get; set; }

        public override Point Size
        {
            get
            {
                if (base.Size != Point.Zero)
                {
                    return base.Size;
                }
                if (BackgroundSprite != null)
                {
                    return BackgroundSprite.Size;
                }
                if (TextSprite != null)
                {
                    return TextSprite.Size;
                }
                return base.Size;
            }
        }

        public string Text
        {
            get { return TextSprite?.Text; }
            set
            {
                if (TextSprite == null)
                {
                    TextSprite = value;
                    return;
                }
                TextSprite.Text = value;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BackgroundSprite?.Draw(spriteBatch, DrawController, ActualBounds);
            TextSprite?.Draw(spriteBatch, DrawController, LabelLocation, ActualScale);
        }

        bool LeftClickFired;
        bool RightClickFired;

        public override void Update()
        {
            if (BackgroundSprite is TextureAtlasSprite)
            {
                ((TextureAtlasSprite)BackgroundSprite).Frame = 0;
            }

            // Don't respond to any event if button is disabled
            if (!Disabled && Application.Input.ShouldAcceptInput)
            {
                // If mouse is on top of the button
                if (ActualBounds.Contains(Application.Input.MousePosition))
                {
                    if (BackgroundSprite is TextureAtlasSprite)
                    {
                        ((TextureAtlasSprite)BackgroundSprite).Frame = 1;
                    }
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
                        if (BackgroundSprite is TextureAtlasSprite)
                        {
                            ((TextureAtlasSprite)BackgroundSprite).Frame = 2;
                        }
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

        private Point LabelLocation
        {
            get
            {
                return new Point(
                    ActualBounds.Center.X - (int)(TextSprite.Size.X * ActualScale / 2),
                    ActualBounds.Center.Y - (int)(TextSprite.Size.Y * ActualScale / 2));
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
