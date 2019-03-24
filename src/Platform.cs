using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class Platform
    {
        static Platform()
        {
            GlobalScale = 1f;
        }
        private static Version _version = Assembly.GetExecutingAssembly().GetName().Version;
        private static DateTime _buildDate = new DateTime(2000, 1, 1)
            .AddDays(_version.Build)
            .AddSeconds(_version.Revision * 2);
        // General Information
        public static readonly string Name = "Maquina";
        public static readonly string BuildDate = String.Format("{0} {1}",
            _buildDate.ToShortDateString(), _buildDate.ToShortTimeString());
        public static readonly int  APIVersion = 0;
        // XML Files
        public static readonly string ResourceXml = "resources.xml";
        public static readonly string PreferencesXml = "preferences.xml";
        public static readonly string LocaleDefinitionXml = "locale.xml";
        // Resources
        public static readonly string ContentRootDirectory = "Content";
        public static readonly string LocalesDirectory = "locales";
        public static readonly string DefaultLocale = "en-US";
        // Properties
        public static float GlobalScale { get; set; }
    }
}
