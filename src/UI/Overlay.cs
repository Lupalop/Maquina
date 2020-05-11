using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maquina.Entities;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Maquina.UI
{
    public abstract class Overlay : Scene
    {
        protected Overlay(string sceneName) : base(sceneName) {}
        protected Overlay(string sceneName, Scene parentScene) : base(sceneName)
        {
            ParentScene = parentScene;
            DisableParentSceneUI = false;
        }
        protected Overlay(string sceneName, Scene parentScene, bool disableParentSceneUI) : base(sceneName)
        {
            ParentScene = parentScene;
            DisableParentSceneUI = disableParentSceneUI;
        }

        private Scene parentScene;
        public Scene ParentScene
        {
            get { return parentScene; }
            protected set
            {
                parentScene = value;
                CheckParentScene();
            }
        }
        private bool disableParentSceneUI;
        public bool DisableParentSceneUI
        {
            get { return disableParentSceneUI; }
            protected set
            {
                disableParentSceneUI = value;
                CheckParentScene();
            }
        }

        private void CheckParentScene()
        {
            if (ParentScene != null)
            {
                foreach (var item in ParentScene.Entities.Values)
                {
                    if (item is Control)
                    {
                        ((Control)(item)).Disabled = DisableParentSceneUI;
                    }
                    if (ParentScene.Entities.IsModified)
                    {
                        break;
                    }
                }
            }
        }
    }
}
