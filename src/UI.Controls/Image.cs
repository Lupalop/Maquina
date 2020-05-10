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
    public class Image : Control
    {
        public Image(string name) : base(name)
        {
            Id = "UI_IMAGE";
            Sprite = new Sprite();
            Sprite.SpriteChanged += Background_SpriteChanged;
            ElementChanged += Image_ElementChanged;
        }

        // Child elements
        public Sprite Sprite { get; private set; }
        
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
        private void Background_SpriteChanged(object sender, ElementChangedEventArgs e)
        {
            if (e.Property == ElementChangedProperty.Size)
            {
                Size = Sprite.Size;
            }
        }

        protected void Image_ElementChanged(object sender, ElementChangedEventArgs e)
        {
            switch (e.Property)
            {
                case ElementChangedProperty.Location:
                    Sprite.Location = Location;
                    break;
                case ElementChangedProperty.IgnoreGlobalScale:
                    Sprite.IgnoreGlobalScale = ((BaseElement)sender).IgnoreGlobalScale;
                    break;
                case ElementChangedProperty.Scale:
                    Sprite.Scale = Scale;
                    break;
                default:
                    break;
            }
        }
    }
}
