using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maquina.Resources
{
    [XmlRoot("LocaleDefinition")]
    public class LocaleDefinition : IContent
    {
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public List<string> Authors { get; set; }
    }
}
