using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    public class SceneDictionary<T> : Dictionary<T, SceneBase>
    {
        public override void Add(T key, SceneBase scene)
        {
            // Load content when scene is added
            scene.LoadContent();
            base.Add(key, scene);
        }
        public override bool Remove(T key)
        {
            // Unload content when scene is removed
            this[key].Unload();
            return base.Remove(key);
        }
        public override void Clear()
        {
            // Unload content of every scene
            foreach (SceneBase scene in this.Values)
            {
                scene.Unload();
            }
            base.Clear();
        }
    }
}
