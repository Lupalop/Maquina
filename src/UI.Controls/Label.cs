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
    public class Label : Control
    {
        public Label(string name) : base (name)
        {
            Id = "UI_LABEL";
            Sprite = new TextSprite();
            Sprite.SpriteChanged += Child_SpriteChanged;
            EntityChanged += Label_EntityChanged;
            Sprite.Text = "";
        }

        // Child sprite
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
        private void Child_SpriteChanged(object sender, EntityChangedEventArgs e)
        {
            if (e.Property == EntityChangedProperty.Size)
            {
                Size = Sprite.Size;
            }
        }
        private void Label_EntityChanged(object sender, EntityChangedEventArgs e)
        {
            switch (e.Property)
            {
                case EntityChangedProperty.Location:
                    Sprite.Location = Location;
                    break;
                case EntityChangedProperty.IgnoreGlobalScale:
                    Sprite.IgnoreGlobalScale = ((Entity)sender).IgnoreGlobalScale;
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
                EntityChanged -= Label_EntityChanged;
            }
            base.Dispose(disposing);
        }
    }
}
