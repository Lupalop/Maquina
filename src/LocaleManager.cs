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
        public string RootDirectory { get; set; }
        public string DefaultLocale { get; set; }
        public const string LocaleDefinitionXml = "locale.xml";

        public LocaleManager()
        {
            RootDirectory = "locales";
            DefaultLocale = "en-US";
            Strings = new Dictionary<string, string>();
            LanguageCode = (string)Application.Preferences["app.locale", DefaultLocale];
        }

        private string languageCode;
        public string LanguageCode
        {
            get { return languageCode; }
            set
            {
                languageCode = value;
                CurrentLocale = new LocaleDefinition() { LanguageCode = value };
                try
                {
                    IEnumerable<string> fileList = Directory.EnumerateFiles(
                        Path.Combine(Application.Content.RootDirectory, RootDirectory, value));
                    // Load associated string bundles
                    foreach (string fileName in fileList)
                    {
                        if (fileName.Contains(LocaleDefinitionXml))
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
                catch (Exception ex)
                {
#if LOG_ENABLED
                    LogManager.Warn(0, ex.Message);
#endif
                    return;
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
                        Path.Combine(Application.Content.RootDirectory, RootDirectory));
                foreach (var item in Directories)
                {
                    string LocaleDefLocation = Path.Combine(item, LocaleDefinitionXml);
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
