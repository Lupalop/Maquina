using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("LocaleDefinition")]
    [Content("LocaleDefinition")]
    public class LocaleDefinition
    {
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public List<string> Authors { get; set; }
    }
}
