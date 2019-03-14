using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI
{
    [Serializable]
    public class SceneDictionary<T> : Dictionary<T, SceneBase>
    {
        public SceneDictionary() : base(0, null) { }
        protected SceneDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public new void Add(T key, SceneBase scene)
        {
            // Load content when scene is added
            scene.LoadContent();
            base.Add(key, scene);
        }
        public new bool Remove(T key)
        {
            // Unload content when scene is removed
            this[key].Unload();
            return base.Remove(key);
        }
        public new void Clear()
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
