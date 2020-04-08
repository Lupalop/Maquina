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
#if LOG_ENABLED
            LogManager.Warn(0, $"Missing resource: {containerName}, {resourceName}");
#endif
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
#if LOG_ENABLED
            LogManager.Warn(0, $"Missing resource: {resourceName}");
#endif
            return null;
        }
    }
}
