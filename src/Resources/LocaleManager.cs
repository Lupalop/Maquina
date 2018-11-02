using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Maquina.Resources
{
    public class LocaleManager
    {
        public LocaleManager()
        {
            LdefContentManager = new ContentManager<LocaleDefinition>();
            StrbContentManager = new ContentManager<StringBundle>();
            Strings = new Dictionary<string, string>();
            // TODO: Hack while we don't have a working preferences system
            CurrentLocale = new LocaleDefinition() { LanguageCode = "en-US" };
        }

        private ContentManager<LocaleDefinition> LdefContentManager;
        private ContentManager<StringBundle> StrbContentManager;

        public List<LocaleDefinition> GetAvailableLocales
        {
            get
            {
                List<LocaleDefinition> CreatedList = new List<LocaleDefinition>();
                IEnumerable<string> Directories = Directory.EnumerateDirectories(Platform.CreateRelativeLocation(
                    new string[] { Platform.ContentRootDirectory, Platform.LocalesDirectory }));
                foreach (var item in Directories)
                {
                    string LocaleDefLocation = Platform.CreateRelativeLocation(
                        new string[] { item, Platform.LocaleDefinitionXml });
                    // Check first if locale definition exists
                    if (File.Exists(LocaleDefLocation))
                    {
                        CreatedList.Add(LdefContentManager.LoadContent(LocaleDefLocation));
                    }
                }
                return CreatedList;
            }
        }

        public LocaleDefinition CurrentLocale { get; set; }

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
                StringBundle StringBundle = StrbContentManager.LoadContent(
                    Platform.CreateRelativeLocation(new string[] {
                        Platform.ContentRootDirectory, Platform.LocalesDirectory,
                        CurrentLocale.LanguageCode, value + ".xml"
                    }));
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
        public Dictionary<string, string> Strings { get; set; }

    }
}
