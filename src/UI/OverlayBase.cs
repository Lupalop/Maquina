using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maquina.Objects;
using Maquina.UI.Controls;
using System.Collections.ObjectModel;

namespace Maquina.UI
{
    public abstract class OverlayBase : SceneBase
    {
        protected OverlayBase(SceneManager sceneManager, string sceneName, SceneBase parentScene)
            : base(sceneManager, sceneName)
        {
            ParentScene = parentScene;
        }

        protected OverlayBase(SceneManager sceneManager, string sceneName)
            : base(sceneManager, sceneName)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            DelayLoadContent();
        }

        public SceneBase ParentScene { get; set; }

        public virtual void DisableAllMenuButtons(Dictionary<string, GenericElement> objects)
        {
            if (objects != null)
                DisableAllMenuButtonsFromArray(objects.Values.ToArray<GenericElement>());
        }
        public virtual void DisableAllMenuButtons(Collection<GenericElement> objects)
        {
            DisableAllMenuButtonsFromArray(objects.ToArray<GenericElement>());
        }
        public virtual void DisableAllMenuButtons(GenericElement[] objects)
        {
            DisableAllMenuButtonsFromArray(objects);
        }
        private static void DisableAllMenuButtonsFromArray(GenericElement[] objects)
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
            if (ParentScene != null) DisableAllMenuButtons(ParentScene.Objects);
        }
    }
}
