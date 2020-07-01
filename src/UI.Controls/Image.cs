using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

namespace Maquina.UI
{
    public class Image : Control
    {
        public Image(string name) : base(name)
        {
            Id = "UI_IMAGE";
            Sprite = new Sprite();
            Sprite.SpriteChanged += Background_SpriteChanged;
            Changed += Image_EntityChanged;
        }

        // Child sprite
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
        private void Background_SpriteChanged(object sender, EntityChangedEventArgs e)
        {
            if (e.Property == EntityChangedProperty.Size)
            {
                Size = Sprite.Size;
            }
        }

        protected void Image_EntityChanged(object sender, EntityChangedEventArgs e)
        {
            switch (e.Property)
            {
                case EntityChangedProperty.Location:
                    Sprite.Location = Location;
                    break;
                case EntityChangedProperty.IgnoreGlobalScale:
                    Sprite.IgnoreGlobalScale = ((Entity)sender).IgnoreGlobalScale;
                    break;
                case EntityChangedProperty.Scale:
                    Sprite.Scale = Scale;
                    break;
                default:
                    break;
            }
        }
    }
}
