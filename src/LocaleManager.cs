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
        public LocaleManager(string prefLanguageCode)
        {
            LocaleDefinitionContent = new ContentLoader<LocaleDefinition>();
            StringBundleContent = new ContentLoader<StringBundle>();
            Strings = new Dictionary<string, string>();

            CurrentLocale = new LocaleDefinition() { LanguageCode = prefLanguageCode };
        }

        // Content Managers
        private ContentLoader<LocaleDefinition> LocaleDefinitionContent;
        private ContentLoader<StringBundle> StringBundleContent;

        // Locale Definitions
        public LocaleDefinition CurrentLocale { get; set; }
        public List<LocaleDefinition> GetAvailableLocales
        {
            get
            {
                List<LocaleDefinition> CreatedList = new List<LocaleDefinition>();
                IEnumerable<string> Directories = Directory.EnumerateDirectories(
                        Path.Combine(Platform.ContentRootDirectory, Platform.LocalesDirectory));
                foreach (var item in Directories)
                {
                    string LocaleDefLocation = Path.Combine(item, Platform.LocaleDefinitionXml);
                    // Check first if locale definition exists
                    if (File.Exists(LocaleDefLocation))
                    {
                        CreatedList.Add(LocaleDefinitionContent.Initialize(LocaleDefLocation));
                    }
                }
                return CreatedList;
            }
        }

        // String Bundles
        private string _stringBundleName;
        public string StringBundleName
        {
            get
            {
                return _stringBundleName;
            }
            set
            {
                // Load the string bundle
                StringBundle StringBundle = StringBundleContent.Initialize(
                    Path.Combine(Platform.ContentRootDirectory, Platform.LocalesDirectory,
                        CurrentLocale.LanguageCode, value + ".xml"
                    ));
                // Clear dictionary content (in cases where we're reused)
                Strings.Clear();
                // Place all items into strings dictionary
                foreach (var item in StringBundle.Strings)
                {
                    Strings[item.Name] = item.Content;
                }
                // Set value
                _stringBundleName = value;
            }
        }

        // Strings
        public Dictionary<string, string> Strings { get; set; }

    }
}
