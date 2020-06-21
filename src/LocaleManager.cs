using System;
using System.Collections.Generic;
using System.IO;
using Maquina.Content;

namespace Maquina
{
    public class LocaleManager
    {
        public string RootDirectory { get; set; }
        public string LocalizedPrefix { get; set; }
        public Dictionary<string, string> Strings { get; set; }
        public const string LocaleDefinitionXml = "locale.xml";

        public LocaleManager()
        {
            RootDirectory = "locales";
            LocalizedPrefix = Application.Preferences.GetString("app.locale.prefix", "&");
            Strings = new Dictionary<string, string>();
            LanguageCode = Application.Preferences.GetString("app.locale", "");
        }

        private string languageCode;
        public string LanguageCode
        {
            get { return languageCode; }
            set
            {
                languageCode = value.Trim();

                if (languageCode == string.Empty)
                {
                    return;
                }

                CurrentLocale = new LocaleManifest() { Code = value };

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
                        Property<string>[] strings = XmlHelper.Load<StringManifest>(fileName).StringPropertySet;
                        for (int i = 0; i < strings.Length; i++)
                        {
                            Strings.Add(strings[i].Id, strings[i].Value);
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

        public LocaleManifest CurrentLocale { get; private set; }

        public List<LocaleManifest> LocaleList
        {
            get
            {
                List<LocaleManifest> CreatedList = new List<LocaleManifest>();
                IEnumerable<string> Directories = Directory.EnumerateDirectories(
                        Path.Combine(Application.Content.RootDirectory, RootDirectory));
                foreach (var item in Directories)
                {
                    string LocaleDefLocation = Path.Combine(item, LocaleDefinitionXml);
                    // Check first if locale definition exists
                    if (File.Exists(LocaleDefLocation))
                    {
                        CreatedList.Add(XmlHelper.Load<LocaleManifest>(LocaleDefLocation));
                    }
                }
                return CreatedList;
            }
        }

        public string TryGetString(string name)
        {
            if (languageCode != string.Empty)
            {
                string keyName = name;

                if (name.StartsWith(LocalizedPrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    keyName = name.Substring(1);
                }

                if (Strings.ContainsKey(keyName))
                {
                    return Strings[keyName];
                }
            }

            // If string is not found, simply return the original string
            return name;
        }
    }
}
