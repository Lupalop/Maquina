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
            Sprite = new TextSprite();
            Sprite.SpriteChanged += Child_SpriteChanged;
            ElementChanged += Label_ElementChanged;
            Sprite.Text = "";
        }

        // Child elements
        public TextSprite Sprite { get; private set; }

        // Draw and update methods
        public override void Draw()
        {
            Sprite.Draw();
            base.Draw();
        }
        public override void Update()
        {
            Sprite.Update();
            base.Update();
        }

        // Listeners
        private void Child_SpriteChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property == ElementChangedProperty.Size)
            {
                Size = Sprite.Size;
            }
        }
        private void Label_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            switch (e.Property)
            {
                case ElementChangedProperty.Location:
                    Sprite.Location = Location;
                    break;
                case ElementChangedProperty.IgnoreGlobalScale:
                    Sprite.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
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
                Sprite.SpriteChanged -= Child_SpriteChanged;
                ElementChanged -= Label_ElementChanged;
            }
            base.Dispose(disposing);
        }
    }
}
