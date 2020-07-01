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
            Id = "UI_CANVAS";
            Children = new EntityDictionary();
        }

        public EntityDictionary Children { get; private set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Children.Draw();

            base.Draw();
        }

        public override void Update()
        {
            Children.Update();

            base.Update();
        }

        protected override void OnDisabledStateChanged()
        {
            foreach (var item in Children.Values)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in Children.Values)
                {
                    item.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
