using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Maquina.Elements;

namespace Maquina.Resources
{
    // Wrapper around for XmlHelper
    public class ContentLoader<T>
    {
        // Accessor methods
        public T Content { get; set; }

        // Content loader(s)
        public T Initialize(string file)
        {
            T Content = XmlHelper.Load<T>(file);
            return Content;
        }
    }

    // Provides methods for dealing with XML files
    public static class XmlHelper
    {
        public static T Load<T>(string file)
        {
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    var deserializer = new XmlSerializer(typeof(T));
                    return (T)deserializer.Deserialize(reader);
                }
            }
            catch (IOException)
            {
                return default(T);
            }
        }

        public static void Save<T>(T toSerialize, string file)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using (TextWriter textWriter = new StreamWriter(file))
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
            }
        }
    }
}
