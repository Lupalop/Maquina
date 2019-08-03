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
        }
        protected Overlay(string sceneName, Scene parentScene, bool disableParentSceneGui) : base(sceneName)
        {
            ParentScene = parentScene;
            DisableParentSceneGui = disableParentSceneGui;
        }

        public Scene ParentScene { get; set; }
        public bool DisableParentSceneGui { get; private set; }

        public override void LoadContent()
        {
            if (ParentScene != null && DisableParentSceneGui)
            {
                GuiUtils.DisableAllMenuButtons(ParentScene.Elements);
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
