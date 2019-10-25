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
    public class Image : GuiElement
    {
        public Image(string name) : base(name)
        {
            Id = "GUI_IMAGE";
            Background = new Sprite();
            Background.SpriteChanged += Background_SpriteChanged;
            ElementChanged += Image_ElementChanged;
        }

        // Child elements
        public Sprite Background { get; set; }
        
        // Aliases
        public Texture2D Graphic
        {
            get { return Background.Graphic; }
            set { Background.Graphic = value; }
        }
        public Color Tint
        {
            get { return Background.Tint; }
            set { Background.Tint = value; }
        }
        public float Rotation
        {
            get { return Background.Rotation; }
            set { Background.Rotation = value; }
        }
        public Vector2 RotationOrigin
        {
            get { return Background.RotationOrigin; }
            set { Background.RotationOrigin = value; }
        }
        public SpriteType SpriteType
        {
            get { return Background.SpriteType; }
            set { Background.SpriteType = value; }
        }
        public SpriteEffects SpriteEffects
        {
            get { return Background.SpriteEffects; }
            set { Background.SpriteEffects = value; }
        }
        public int Rows
        {
            get { return Background.Rows; }
            set { Background.Rows = value; }
        }
        public int Columns
        {
            get { return Background.Columns; }
            set { Background.Columns = value; }
        }
        public float LayerDepth
        {
            get { return Background.LayerDepth; }
            set { Background.LayerDepth = value; }
        }
        public float Opacity
        {
            get { return Background.Opacity; }
            set { Background.Opacity = value; }
        }

        // Draw and update methods
        public override void Draw(GameTime gameTime)
        {
            if (Background != null && Graphic != null)
            {
                Background.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            if (Background != null)
            {
                Background.Update(gameTime);
            }
            base.Update(gameTime);
        }

        // Listeners
        private void Background_SpriteChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property == ElementChangedProperty.Size)
            {
                Size = Background.Size;
            }
        }

        protected void Image_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            switch (e.Property)
            {
                case ElementChangedProperty.Location:
                    Background.Location = Location;
                    break;
                case ElementChangedProperty.IgnoreGlobalScale:
                    Background.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
                    break;
                case ElementChangedProperty.Scale:
                    Background.Scale = Scale;
                    break;
                default:
                    break;
            }
        }
    }
}
