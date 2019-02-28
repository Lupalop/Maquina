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
        private static Version _version = Assembly.GetExecutingAssembly().GetName().Version;
        private static DateTime _buildDate = new DateTime(2000, 1, 1)
            .AddDays(_version.Build)
            .AddSeconds(_version.Revision * 2);
        // General Information
        public const string Name = "Maquina";
        public static readonly string BuildDate = String.Format("{0} {1}",
            new object[] { _buildDate.ToShortDateString(), _buildDate.ToShortTimeString() });
        public const string Version = "0.0.1";
        // XML Files
        public const string ResourceXml = "resources.xml";
        public const string PreferencesXml = "preferences.xml";
        public const string LocaleDefinitionXml = "locale.xml";
        // Resources
        public const string ContentRootDirectory = "Content";
        public const string LocalesDirectory = "locales";
        public const string DefaultLocale = "en-US";
        public static float GlobalScale = 1f;
        // Engine Startup
        public static Action RunGame { get; set; }
        public static void StartEngine(string[] args)
        {
#if DEBUG
            if (args != null)
            {
                // Enumerate passed arguments
                foreach (string arg in args)
                {
                    switch (arg.ToLower())
                    {
                        case "--v":
                            VerboseOutput = true;
                            break;
                        case "--pfr":
                            PromptForRestart = true;
                            break;
                        default:
                            // Ignore other arguments passed
                            break;
                    }
                }
            }

            WriteHeader();
#endif

            RunGame();

#if DEBUG
            while (PromptForRestart)
            {
                Console.WriteLine("Restart game? Y = Yes, Other keys = No");
                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    WriteHeader();
                    RunGame();
                }
                else
                    return;
            }
#endif
        }
#if DEBUG
        private static void WriteHeader()
        {
            Console.Clear();
            Console.Title = String.Format("{0} Debug Console", Platform.Name);
            Console.WriteLine("/*");
            Console.WriteLine(" * {0} Game Engine", Platform.Name);
            Console.WriteLine(" * Version {0}, built on {1}", new object[] { Platform.Version, Platform.BuildDate });
            Console.WriteLine(" * Developer Console");
            Console.WriteLine(" */");
            Console.WriteLine();
        }

        public static bool VerboseOutput { get; private set; }
        public static bool PromptForRestart { get; private set; }
#endif
    }
}
