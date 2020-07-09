using Maquina.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class Canvas : Control, IContainer
    {
        // FIXME: Entities inside a canvas entity SHOULD be relative to the container's location.
        //        Sprite and Entity (and their children) have to be updated to handle this.
        public Canvas(string name) : base(name)
        {
            Children = new EntityCollection();
        }

        public EntityCollection Children { get; private set; }

        public override bool IgnoreDisplayScale
        {
            get { return true; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Children.Draw(spriteBatch);
        }

        public override void Update()
        {
            Children.Update();
        }

        protected override void OnEntityChanged(object sender, EntityChangedEventArgs e)
        {
            if (e.Property == EntityChangedProperty.Disabled)
            {
                foreach (var item in Children)
                {
                    if (item is Control)
                    {
                        ((Control)(item)).Disabled = Disabled;
                    }
                    if (Children.IsModified)
                    {
                        break;
                    }
                }
            }
            base.OnEntityChanged(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Children.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
