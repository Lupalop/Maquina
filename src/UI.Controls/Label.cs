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
    public class Label : GuiElement
    {
        public Label(string name) : base (name)
        {
            Id = "GUI_LABEL";
            Child = new TextSprite();
            Child.SpriteChanged += Child_SpriteChanged;
            ElementChanged += Label_ElementChanged;
            Text = "";
        }

        // Child elements
        public TextSprite Child { get; set; }

        // Convenient aliases
        public SpriteFont Font
        {
            get { return Child.Font; }
            set { Child.Font = value; }
        }
        public string Text
        {
            get { return Child.Label; }
            set { Child.Label = value; }
        }
        public Color Tint
        {
            get { return Child.Tint; }
            set { Child.Tint = value; }
        }
        public float Rotation
        {
            get { return Child.Rotation; }
            set { Child.Rotation = value; }
        }
        public Vector2 RotationOrigin
        {
            get { return Child.RotationOrigin; }
            set { Child.RotationOrigin = value; }
        }
        public SpriteEffects SpriteEffects
        {
            get { return Child.SpriteEffects; }
            set { Child.SpriteEffects = value; }
        }
        public float LayerDepth
        {
            get { return Child.LayerDepth; }
            set { Child.LayerDepth = value; }
        }
        public float Opacity
        {
            get { return Child.Opacity; }
            set { Child.Opacity = value; }
        }

        // Draw and update methods
        public override void Draw(GameTime gameTime)
        {
            if (Child != null)
            {
                Child.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            if (Child != null)
            {
                Child.Update(gameTime);
            }
            base.Update(gameTime);
        }

        // Listeners
        private void Child_SpriteChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property == ElementChangedProperty.Size)
            {
                Size = Child.Size;
            }
        }
        private void Label_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            switch (e.Property)
            {
                case ElementChangedProperty.Location:
                    Child.Location = Location;
                    break;
                case ElementChangedProperty.IgnoreGlobalScale:
                    Child.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
                    break;
                default:
                    break;
            }
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Child.SpriteChanged -= Child_SpriteChanged;
                ElementChanged -= Label_ElementChanged;
            }
            base.Dispose(disposing);
        }
    }
}
