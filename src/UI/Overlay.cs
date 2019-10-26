using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maquina.Elements;
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
            DisableParentSceneGui = false;
        }
        protected Overlay(string sceneName, Scene parentScene, bool disableParentSceneGui) : base(sceneName)
        {
            ParentScene = parentScene;
            DisableParentSceneGui = disableParentSceneGui;
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
        private bool disableParentSceneGui;
        public bool DisableParentSceneGui
        {
            get { return disableParentSceneGui; }
            protected set
            {
                disableParentSceneGui = value;
                CheckParentScene();
            }
        }

        private void CheckParentScene()
        {
            if (ParentScene != null)
            {
                GuiUtils.SetElementDisabledState(ParentScene.Elements, DisableParentSceneGui);
            }
        }
    }
}
