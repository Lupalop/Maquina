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
        public LocaleManager(PreferencesManager preferencesManager)
        {
            LdefContentManager = new ContentManager<LocaleDefinition>();
            StrbContentManager = new ContentManager<StringBundle>();
            Strings = new Dictionary<string, string>();

            // Attempt to get language code from preferences
            string PrefLanguageCode = preferencesManager.GetCharPref("app.locale");
            // If empty, use platform default
            if (PrefLanguageCode == "")
                PrefLanguageCode = Platform.DefaultLocale;

            CurrentLocale = new LocaleDefinition() { LanguageCode = PrefLanguageCode };
        }

        // Content Managers
        private ContentManager<LocaleDefinition> LdefContentManager;
        private ContentManager<StringBundle> StrbContentManager;

        // Locale Definitions
        public LocaleDefinition CurrentLocale { get; set; }
        public List<LocaleDefinition> GetAvailableLocales
        {
            get
            {
                List<LocaleDefinition> CreatedList = new List<LocaleDefinition>();
                IEnumerable<string> Directories = Directory.EnumerateDirectories(Utils.CreateLocation(
                    new string[] { Platform.ContentRootDirectory, Platform.LocalesDirectory }));
                foreach (var item in Directories)
                {
                    string LocaleDefLocation = Utils.CreateLocation(
                        new string[] { item, Platform.LocaleDefinitionXml });
                    // Check first if locale definition exists
                    if (File.Exists(LocaleDefLocation))
                    {
                        CreatedList.Add(LdefContentManager.Initialize(LocaleDefLocation));
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
                StringBundle StringBundle = StrbContentManager.Initialize(
                    Utils.CreateLocation(new string[] {
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

        // Strings
        public Dictionary<string, string> Strings { get; set; }

    }
}
