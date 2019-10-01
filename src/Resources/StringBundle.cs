using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("StringBundle")]
    [Content("StringBundle")]
    public class StringBundle
    {
        public List<StringParameters> Strings { get; set; }
    }
}
