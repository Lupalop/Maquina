using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("StringBundle")]
    public class StringBundle : IContent
    {
        public List<StringParameters> Strings { get; set; }
    }
}
