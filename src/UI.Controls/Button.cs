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
        private Point _labelLocation;
        private Point _iconLocation;

        public Button(string name) : base (name)
        {
            BackgroundSprite = new TextureAtlasSprite(
                (Texture2D)ContentFactory.TryGetResource("button-default"), 3, 1);
            TextSprite = name;
            ClickSound = (SoundEffect)ContentFactory.TryGetResource("click_default");
        }

        public TextureSprite BackgroundSprite { get; set; }
        public TextureSprite IconSprite { get; set; }
        public TextSprite TextSprite { get; set; }

        public HorizontalAlignment IconAlignment { get; set; }

        public event EventHandler LeftMouseDown;
        public event EventHandler RightMouseDown;

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
                if (IconSprite != null && TextSprite != null)
                {
                    return IconSprite.Size + TextSprite.Size;
                }
                if (IconSprite != null && TextSprite == null)
                {
                    return IconSprite.Size;
                }
                if (IconSprite == null && TextSprite != null)
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
                    TextSprite = new TextSprite();
                }
                TextSprite.Text = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BackgroundSprite?.Draw(spriteBatch, DrawController, ActualBounds);
            EntityUtils.BeginScissor(ActualBounds);
            TextSprite?.Draw(spriteBatch, DrawController, _labelLocation, ActualScale);
            EntityUtils.EndScissor();
            IconSprite?.Draw(spriteBatch, DrawController, _iconLocation, ActualScale);
        }

        public override void Update()
        {
            if (BackgroundSprite is TextureAtlasSprite)
            {
                ((TextureAtlasSprite)BackgroundSprite).Frame = 0;
            }

            if (Enabled && Application.Input.Enabled)
            {
                // Update frame: Cursor is on the button
                if (ActualBounds.Contains(Application.Input.MousePosition) &&
                    BackgroundSprite is TextureAtlasSprite)
                {
                    ((TextureAtlasSprite)BackgroundSprite).Frame = 1;
                }

                // Update focus: Mouse button is pressed
                if (Application.Input.MouseDown(MouseButton.Left)  ||
                    Application.Input.MouseDown(MouseButton.Right) ||
                    Application.Input.MouseDown(MouseButton.Middle))
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
                if (LeftMouseDown != null)
                {
                    if (Application.Input.MousePressed(MouseButton.Left) &&
                        ActualBounds.Contains(Application.Input.MousePosition))
                    {
                        LeftMouseDown(this, EventArgs.Empty);
                        ClickSound.Play();
                    }
                }

                // Right Mouse Button Click Action
                if (RightMouseDown != null)
                {
                    if (Application.Input.MousePressed(MouseButton.Right) &&
                        ActualBounds.Contains(Application.Input.MousePosition))
                    {
                        RightMouseDown(this, EventArgs.Empty);
                        ClickSound.Play();
                    }
                }
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.Id == PropertyId.Enabled || e.Id == PropertyId.Focused)
            {
                return;
            }

            if (TextSprite != null)
            {
                _labelLocation = new Point(
                        ActualBounds.Center.X - (int)(TextSprite.Size.X * ActualScale / 2),
                        ActualBounds.Center.Y - (int)(TextSprite.Size.Y * ActualScale / 2));
            }

            if (IconSprite != null)
            {
                switch (IconAlignment)
                {
                    case HorizontalAlignment.Left:
                        _iconLocation.X = Location.X;
                        break;
                    case HorizontalAlignment.Center:
                        _iconLocation.X = Location.X + (ActualBounds.Width / 2) - (int)(IconSprite.Size.X * ActualScale / 2);
                        break;
                    case HorizontalAlignment.Right:
                        _iconLocation.X = Location.X + ActualBounds.Width - (int)(IconSprite.Size.X * ActualScale);
                        break;
                }

                _iconLocation.Y = Location.Y + (ActualBounds.Height / 2) - (int)(IconSprite.Size.Y * ActualScale / 2);
            }

            base.OnPropertyChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}
