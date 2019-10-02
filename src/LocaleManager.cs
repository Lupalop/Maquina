using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Maquina.Resources;

namespace Maquina
{
    public class LocaleManager
    {
        public LocaleManager()
        {
            Strings = new Dictionary<string, string>();
            LanguageCode = Global.Preferences.GetStringPreference("app.locale", Global.DefaultLocale);
        }

        private string languageCode;
        public string LanguageCode
        {
            get { return languageCode; }
            set
            {
                languageCode = value;
                CurrentLocale = new LocaleDefinition() { LanguageCode = value };
                IEnumerable<string> fileList = Directory.EnumerateFiles(
                    Path.Combine(Global.Content.RootDirectory, Global.LocaleDirectory, value));
                // Load associated string bundles
                foreach (string fileName in fileList)
                {
                    if (fileName.Contains(Global.LocaleDefinitionXml))
                    {
                        continue;
                    }
                    List<StringParameters> strings = XmlHelper.Load<StringBundle>(fileName).Strings;
                    for (int i = 0; i < strings.Count; i++)
                    {
                        Strings.Add(strings[i].Name, strings[i].Content);
                    }
                }
            }
        }

        public LocaleDefinition CurrentLocale { get; private set; }
        public List<LocaleDefinition> LocaleList
        {
            get
            {
                List<LocaleDefinition> CreatedList = new List<LocaleDefinition>();
                IEnumerable<string> Directories = Directory.EnumerateDirectories(
                        Path.Combine(Global.Content.RootDirectory, Global.LocaleDirectory));
                foreach (var item in Directories)
                {
                    string LocaleDefLocation = Path.Combine(item, Global.LocaleDefinitionXml);
                    // Check first if locale definition exists
                    if (File.Exists(LocaleDefLocation))
                    {
                        CreatedList.Add(XmlHelper.Load<LocaleDefinition>(LocaleDefLocation));
                    }
                }
                return CreatedList;
            }
        }

        public Dictionary<string, string> Strings { get; set; }
    }
}
