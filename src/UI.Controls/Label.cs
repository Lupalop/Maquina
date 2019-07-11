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
        public Label(string objectName) : base (objectName)
        {
            Child = new TextSprite();
            Child.SizeChanged += Child_SizeChanged;
            this.LocationChanged += Label_LocationChanged;
            IgnoreGlobalScaleChanged += Label_IgnoreGlobalScaleChanged;
            Text = "";
        }

        // General
        public override string Id
        {
            get { return "GUI_LABEL"; }
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
        private void Label_LocationChanged(Point value)
        {
            Child.Location = Location;
        }
        private void Child_SizeChanged(Point value)
        {
            Size = Child.Size;
        }
        private void Label_IgnoreGlobalScaleChanged(bool value)
        {
            Child.IgnoreGlobalScale = value;
        }
    }
}
