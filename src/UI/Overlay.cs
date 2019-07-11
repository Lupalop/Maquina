using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maquina.Elements;
using System.Collections.ObjectModel;

namespace Maquina.UI
{
    public abstract class Overlay : Scene
    {
        protected Overlay(string sceneName, Scene parentScene = null)
            : base(sceneName)
        {
            ParentScene = parentScene;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public Scene ParentScene { get; set; }

        public virtual void DisableAllMenuButtons(IDictionary<string, BaseElement> objects)
        {
            if (objects != null)
                DisableAllMenuButtonsFromArray(objects.Values.ToArray<BaseElement>());
        }
        public virtual void DisableAllMenuButtons(Collection<BaseElement> objects)
        {
            DisableAllMenuButtonsFromArray(objects.ToArray<BaseElement>());
        }
        public virtual void DisableAllMenuButtons(BaseElement[] objects)
        {
            DisableAllMenuButtonsFromArray(objects);
        }
        private static void DisableAllMenuButtonsFromArray(BaseElement[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is MenuButton)
                {
                    MenuButton mb = (MenuButton)objects[i];
                    mb.Disabled = true;
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (ParentScene != null) DisableAllMenuButtons(ParentScene.Elements);
        }
    }
}
