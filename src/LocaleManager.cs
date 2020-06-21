using System;
using System.Collections.Generic;
using System.IO;
using Maquina.Content;

namespace Maquina
{
#if MGE_LOCALE
    public class LocaleManager
    {
        public string RootDirectory { get; set; }
        public string BindingPrefix { get; set; }

        public Dictionary<string, string> Strings { get; set; }
        public Dictionary<string, LocaleManifest> Languages { get; private set; }

        public const string LocaleDefinitionFileName = "locale.xml";
        public const string LanguageCodePreference = "app.locale";

        public LocaleManager()
        {
            Strings = new Dictionary<string, string>(
                StringComparer.InvariantCultureIgnoreCase);
            Languages = new Dictionary<string, LocaleManifest>(
                StringComparer.InvariantCultureIgnoreCase);
            RootDirectory = "locales";
            BindingPrefix = "&";
            LanguageCode = Application.Preferences.GetString(LanguageCodePreference, "");
            ReloadLanguages();
        }

        private string languageCode;
        public string LanguageCode
        {
            get { return languageCode; }
            set
            {
                languageCode = value.Trim();
                Application.Preferences.SetString(LanguageCodePreference, languageCode);

                if (languageCode == string.Empty || !Languages.ContainsKey(languageCode))
                {
                    Strings.Clear();
                    return;
                }

                string languageDirectory =
                    Path.Combine(Application.Content.RootDirectory, RootDirectory, value);

                LoadStringManifests(languageDirectory);
            }
        }

        public LocaleManifest CurrentLanguage
        {
            get
            {
                if (Languages.ContainsKey(languageCode))
                {
                    return Languages[languageCode];
                }

                return null;
            }
        }

        private bool LoadStringManifests(string location)
        {
            try
            {
                IEnumerable<string> fileList = Directory.EnumerateFiles(location);

                foreach (string fileName in fileList)
                {
                    if (fileName.Contains(LocaleDefinitionFileName))
                    {
                        continue;
                    }

                    Property<string>[] strings =
                        XmlHelper.Load<StringManifest>(fileName).StringPropertySet;

                    for (int i = 0; i < strings.Length; i++)
                    {
                        Strings.Add(strings[i].Id, strings[i].Value);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
#if MGE_LOGGING
                LogManager.Warn(0, ex.Message);
#endif
                return false;
            }
        }

        public void ReloadLanguages()
        {
            Languages.Clear();

            IEnumerable<string> Directories = Directory.EnumerateDirectories(
                    Path.Combine(Application.Content.RootDirectory, RootDirectory));

            foreach (var item in Directories)
            {
                string manifestFileName = Path.Combine(item, LocaleDefinitionFileName);

                if (File.Exists(manifestFileName))
                {
                    LocaleManifest manifest = XmlHelper.Load<LocaleManifest>(manifestFileName);
                    Languages.Add(manifest.Code, manifest);
                }
            }
        }

        public string TryGetString(string name)
        {
            if (languageCode != string.Empty)
            {
                string keyName = name;

                if (name.StartsWith(BindingPrefix, StringComparison.InvariantCultureIgnoreCase))
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
#endif
}
