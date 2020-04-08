using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class ContentFactory
    {
        public static Dictionary<string, IDictionary<string, object>> Source { get; private set; }

        static ContentFactory()
        {
            Source = new Dictionary<string, IDictionary<string, object>>();
        }

        public static object TryGetResource(string containerName, string resourceName)
        {
            IDictionary<string, object> targetContainer;
            Source.TryGetValue(containerName, out targetContainer);
            if (targetContainer != null)
            {
                object targetResource;
                targetContainer.TryGetValue(resourceName, out targetResource);
                return targetResource;
            }
            return null;
        }

        public static object TryGetResource(string resourceName)
        {
            foreach (var item in Source)
            {
                if (item.Value.ContainsKey(resourceName))
                {
                    return item.Value[resourceName];
                }
            }
            return null;
        }

        public static bool ContainsResource(string containerName, string resourceName)
        {
            IDictionary<string, object> targetContainer;
            Source.TryGetValue(containerName, out targetContainer);
            if (targetContainer != null)
            {
                return targetContainer.ContainsKey(resourceName);
            }
            return false;
        }

        public static bool ContainsResource(string resourceName)
        {
            foreach (var item in Source)
            {
                if (item.Value.ContainsKey(resourceName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
