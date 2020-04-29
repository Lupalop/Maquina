using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    // This is essentially the same as KeyValuePair<TKey, TValue>
    // except that it's a class and makes use of custom XML attributes
    public class Property<T>
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlText]
        public T Value { get; set; }
    }
}
