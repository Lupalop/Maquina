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
    public class Button : Control
    {
        public Button(string name) : base (name)
        {
            Id = "UI_BUTTON";
            InputManager = Application.Input;

            Background = new Sprite()
            {
                SpriteType = SpriteType.Static,
                Rows = 1,
                Columns = 3
            };
            Icon = new Sprite();
            Label = new TextSprite();
            Tooltip = new TextSprite()
            {
                Tint = Color.Transparent,
                IgnoreGlobalScale = true
            };

            // Child
            Background.SpriteChanged += Background_SpriteChanged;
            Label.SpriteChanged += Label_SpriteChanged;
            Icon.SpriteChanged += Icon_SpriteChanged;
            // Parent
            ElementChanged += Button_ElementChanged;

            LayerDepth = 1f;
            Tooltip.Font = (SpriteFont)ContentFactory.TryGetResource("o-default_m");
            Background.Texture = (Texture2D)ContentFactory.TryGetResource("button-default");
            ClickSound = (SoundEffect)ContentFactory.TryGetResource("click_default");
            IconAlignment = HorizontalAlignment.Center;
        }

        // General
        protected InputManager InputManager;
        
        // Child elements
        public Sprite Background { get; private set; }
        public Sprite Icon { get; private set; }
        public TextSprite Label { get; private set; }
        public TextSprite Tooltip { get; private set; }

        // Element events
        public event EventHandler OnLeftClick;
        public event EventHandler OnRightClick;

        // Properties
        public SoundEffect ClickSound { get; set; }
        public HorizontalAlignment IconAlignment { get; set; }
        
        // Common properties
        private Color tint;
        public Color Tint
        {
            get { return tint; }
            set
            {
                tint = value;
                Background.Tint = value;
                Label.Tint = value;
                Icon.Tint = value;
            }
        }
        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                Background.Rotation = value;
                Label.Rotation = value;
                Icon.Rotation = value;
            }
        }
        private Vector2 origin;
        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                Background.Origin = value;
                Label.Origin = value;
                Icon.Origin = value;
            }
        }
        private SpriteEffects spriteEffects;
        public SpriteEffects SpriteEffects
        {
            get { return spriteEffects; }
            set
            {
                spriteEffects = value;
                Background.SpriteEffects = value;
                Label.SpriteEffects = value;
                Icon.SpriteEffects = value;
            }
        }
        private float layerDepth;
        public float LayerDepth
        {
            get { return layerDepth; }
            set
            {
                layerDepth = value;
                Background.LayerDepth = value;
                Icon.LayerDepth = value - 0.1f;
                Label.LayerDepth = value - 0.1f;
                Tooltip.LayerDepth = value - 0.15f;
            }
        }
        private float opacity;
        public float Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                Background.Opacity = value;
                Icon.Opacity = value;
                Label.Opacity = value;
            }
        }

        // Draw and update methods
        public override void Draw()
        {
            Background.Draw();
            Label.Draw();
            Icon.Draw();
            Tooltip.Draw();

            base.Draw();
        }

        bool LeftClickFired;
        bool RightClickFired;
        public override void Update()
        {
            Background.CurrentFrame = 0;

            // Don't respond to any event if button is disabled
            if (!Disabled && InputManager.ShouldAcceptInput)
            {
                // If mouse is on top of the button
                if (ActualBounds.Contains(InputManager.MousePosition))
                {
                    Background.CurrentFrame = 1;
                    if (Tooltip.Text != string.Empty)
                    {
                        Tooltip.Location = new Point(
                            InputManager.MousePosition.X + 20,
                            InputManager.MousePosition.Y + 5);
                        Tooltip.Tint = Color.White;
                    }
                }
                else
                {
                    Tooltip.Tint = Color.Transparent;
                }

                // If the button was clicked
                if ((InputManager.MouseDown(MouseButton.Left) ||
                     InputManager.MouseDown(MouseButton.Right) ||
                     InputManager.MouseDown(MouseButton.Middle)))
                {
                    Focused = false;
                    if (ActualBounds.Contains(InputManager.MousePosition))
                    {
                        Focused = true;
                        Background.CurrentFrame = 2;
                    }
                }

                // Left Mouse Button Click Action
                if (OnLeftClick != null)
                {
                    if (InputManager.MouseDown(MouseButton.Left) && ActualBounds.Contains(InputManager.MousePosition))
                        LeftClickFired = true;
                    if (InputManager.MouseDown(MouseButton.Left) && !ActualBounds.Contains(InputManager.MousePosition))
                        LeftClickFired = false;
                    if (InputManager.MouseUp(MouseButton.Left) && LeftClickFired)
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
                    if (InputManager.MouseDown(MouseButton.Right) && ActualBounds.Contains(InputManager.MousePosition))
                        RightClickFired = true;
                    if (InputManager.MouseDown(MouseButton.Right) && !ActualBounds.Contains(InputManager.MousePosition))
                        RightClickFired = false;
                    if (InputManager.MouseUp(MouseButton.Right) && RightClickFired)
                    {
                        OnRightClick(this, EventArgs.Empty);
                        ClickSound.Play();
                        // In order to prevent the action from being fired again
                        RightClickFired = false;
                    }
                }
            }

            Background.Update();
            Label.Update();
            Icon.Update();
            Tooltip.Update();

            base.Update();
        }
        
        // Listeners
        private void Button_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            switch (e.Property)
            {
                case ElementChangedProperty.Location:
                    Background.Location = Location;
                    RecalculateLabelLocation();
                    RecalculateIconLocation();
                    break;
                case ElementChangedProperty.IgnoreGlobalScale:
                    Background.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
                    Icon.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
                    Label.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
                    break;
                case ElementChangedProperty.Scale:
                    Background.Scale = Scale;
                    Icon.Scale = Scale;
                    Label.Scale = Scale;
                    break;
                default:
                    break;
            }
        }
        private void Icon_SpriteChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property != ElementChangedProperty.Size)
                return;

            RecalculateIconLocation();
        }
        private void Label_SpriteChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property != ElementChangedProperty.Size)
                return;

            if (Background.Texture == null)
            {
                Size = Label.Size;
            }
            RecalculateLabelLocation();
        }
        private void Background_SpriteChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property != ElementChangedProperty.Size)
                return;

            Size = Background.Size;
        }

        // Misc
        private void RecalculateLabelLocation()
        {
            Label.Location = new Point(Location.X + (ActualBounds.Width / 2) - (Label.ActualSize.X / 2),
                Location.Y + (ActualBounds.Height / 2) - (Label.ActualSize.Y / 2));
        }
        private void RecalculateIconLocation()
        {
            switch (IconAlignment)
            {
                case HorizontalAlignment.Left:
                    Icon.Location = new Point(Location.X,
                        Location.Y + (ActualBounds.Height / 2) - (Icon.ActualSize.Y / 2));
                    break;
                case HorizontalAlignment.Center:
                    Icon.Location = new Point(Location.X + (ActualBounds.Width / 2) - (Icon.ActualSize.X / 2),
                        Location.Y + (ActualBounds.Height / 2) - (Icon.ActualSize.Y / 2));
                    break;
                case HorizontalAlignment.Right:
                    Icon.Location = new Point(Location.X + ActualBounds.Width - Icon.ActualSize.X,
                        Location.Y + (ActualBounds.Height / 2) - (Icon.ActualSize.Y / 2));
                    break;
                case HorizontalAlignment.Stretch:
                    // TODO: stub
                    return;
            }
        }

        // IDisposable implementation
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Background.Dispose();
                Icon.Dispose();
            }
        }
    }
}
