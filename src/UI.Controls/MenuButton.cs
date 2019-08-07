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
        public MenuButton(string objectName) : base (objectName)
        {
            InputManager = Global.Input;

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
            Background.SizeChanged += Background_SizeChanged;
            Label.SizeChanged += Label_SizeChanged;
            Icon.SizeChanged += Icon_SizeChanged;
            // Parent
            ElementChanged += MenuButton_ElementChanged;

            TooltipFont = Global.Fonts["o-default_m"];
            MenuBackground = Global.Textures["button-default"];
            ClickSound = Global.SFX["click_default"];
            IconAlignment = Alignment.Center;
        }

        // General
        protected InputManager InputManager;
        public override string Id
        {
            get { return "GUI_MENUBUTTON"; }
        }
        
        // Child elements
        public Sprite Background { get; set; }
        public Sprite Icon { get; set; }
        public TextSprite Label { get; set; }
        public TextSprite Tooltip { get; set; }

        // Element events
        public event Action OnLeftClick;
        public event Action OnRightClick;

        // Properties
        public bool Disabled { get; set; }
        public SoundEffect ClickSound { get; set; }
        public Alignment IconAlignment { get; set; }
        
        // Aliases
        // Child 1: Background
        public Texture2D MenuBackground
        {
            get { return Background.Graphic; }
            set { Background.Graphic = value; }
        }
        public Color MenuBackgroundTint
        {
            get { return Background.Tint; }
            set { Background.Tint = value; }
        }
        public Point MenuBackgroundTable
        {
            get { return Background.Table; }
            set { Background.Table = value; }
        }
        public SpriteType MenuBackgroundSpriteType
        {
            get { return Background.SpriteType; }
            set { Background.SpriteType = value; }
        }
        // Child 2: Label
        public SpriteFont MenuFont
        {
            get { return Label.Font; }
            set { Label.Font = value; }
        }
        public string MenuLabel
        {
            get { return Label.Label; }
            set { Label.Label = value; }
        }
        public Color MenuLabelTint
        {
            get { return Label.Tint; }
            set { Label.Tint = value; }
        }
        // Child 3: Icon
        public Texture2D MenuIcon
        {
            get { return Icon.Graphic; }
            set { Icon.Graphic = value; }
        }
        public Color MenuIconTint
        {
            get { return Icon.Tint; }
            set { Icon.Tint = value; }
        }
        public Point MenuIconTable
        {
            get { return Icon.Table; }
            set { Icon.Table = value; }
        }
        public SpriteType MenuIconSpriteType
        {
            get { return Icon.SpriteType; }
            set { Icon.SpriteType = value; }
        }
        // Child 4: Tooltip
        public string TooltipText
        {
            get { return Tooltip.Label; }
            set { Tooltip.Label = value; }
        }
        public SpriteFont TooltipFont
        {
            get { return Tooltip.Font; }
            set { Tooltip.Font = value; }
        }
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
        private Vector2 rotationOrigin;
        public Vector2 RotationOrigin
        {
            get { return rotationOrigin; }
            set
            {
                rotationOrigin = value;
                Background.RotationOrigin = value;
                Label.RotationOrigin = value;
                Icon.RotationOrigin = value;
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

        // Draw and update methods
        public override void Draw(GameTime gameTime)
        {
            if (Background != null && MenuBackground != null)
            {
                Background.Draw(gameTime);
            }
            if (Label != null)
            {
                Label.Draw(gameTime);
            }
            if (Icon != null && MenuIcon != null)
            {
                Icon.Draw(gameTime);
            }
            if (Tooltip != null)
            {
                Tooltip.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        bool LeftClickFired;
        bool RightClickFired;
        public override void Update(GameTime gameTime)
        {
            Background.CurrentFrame = 0;

            // Don't respond to any event if button is disabled
            if (!Disabled && InputManager.ShouldAcceptInput)
            {
                // If mouse is on top of the button
                if (ActualBounds.Contains(InputManager.MousePosition))
                {
                    if (Background.SpriteType != SpriteType.None)
                    {
                        Background.CurrentFrame = 1;
                    }
                    if (Tooltip != null)
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
                     InputManager.MouseDown(MouseButton.Middle)) &&
                     ActualBounds.Contains(InputManager.MousePosition) &&
                     Background.SpriteType != SpriteType.None)
                {
                    Background.CurrentFrame = 2;
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
                        OnLeftClick.Invoke();
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
                        OnRightClick.Invoke();
                        ClickSound.Play();
                        // In order to prevent the action from being fired again
                        RightClickFired = false;
                    }
                }
            }

            if (Background != null)
            {
                Background.Update(gameTime);
            }
            if (Label != null)
            {
                Label.Update(gameTime);
            }
            if (Icon != null)
            {
                Icon.Update(gameTime);
            }
            if (Tooltip != null)
            {
                Tooltip.Update(gameTime);
            }

            base.Update(gameTime);
        }
        
        // Listeners
        private void MenuButton_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            switch (e.Property)
            {
                case ElementChangedProperty.Location:
                    if (Background != null)
                    {
                        Background.Location = Location;
                    }
                    RecalculateLabelLocation();
                    RecalculateIconLocation();
                    break;
                case ElementChangedProperty.IgnoreGlobalScale:
                    Background.IgnoreGlobalScale = e.IgnoreGlobalScale;
                    Icon.IgnoreGlobalScale = e.IgnoreGlobalScale;
                    Label.IgnoreGlobalScale = e.IgnoreGlobalScale;
                    break;
                default:
                    break;
            }
        }
        private void Icon_SizeChanged(Point value)
        {
            RecalculateIconLocation();
        }
        private void Label_SizeChanged(Point value)
        {
            if (Background == null || Background.Graphic == null)
            {
                Size = value;
            }
            RecalculateLabelLocation();
        }
        private void Background_SizeChanged(Point value)
        {
            Size = Background.Size;
        }

        // Misc
        private void RecalculateLabelLocation()
        {
            if (Label != null)
            {
                Label.Location = new Point(Location.X + (ActualBounds.Width / 2) - (Label.ActualSize.X / 2),
                    Location.Y + (ActualBounds.Height / 2) - (Label.ActualSize.Y / 2));
            }
        }
        private void RecalculateIconLocation()
        {
            if (Icon != null)
            {
                switch (IconAlignment)
                {
                    case Alignment.Left:
                        Icon.Location = new Point(Location.X,
                            Location.Y + (ActualBounds.Height / 2) - (Icon.ActualSize.Y / 2));
                        break;
                    case Alignment.Center:
                        Icon.Location = new Point(Location.X + (ActualBounds.Width / 2) - (Icon.ActualSize.X / 2),
                            Location.Y + (ActualBounds.Height / 2) - (Icon.ActualSize.Y / 2));
                        break;
                    case Alignment.Right:
                        Icon.Location = new Point(Location.X + ActualBounds.Width - Icon.ActualSize.X,
                            Location.Y + (ActualBounds.Height / 2) - (Icon.ActualSize.Y / 2));
                        break;
                    case Alignment.Fixed:
                        return;
                }
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
