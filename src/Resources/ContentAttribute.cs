using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Maquina.Elements;
using Microsoft.Xna.Framework;

namespace Maquina.Resources
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    public sealed class ContentAttribute : Attribute
    {
        public ContentAttribute(string id)
        {
            contentId = id;
        }

        readonly string contentId;
        public string Id
        {
            get { return contentId; }
        }
    }
}
