using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class Platform
    {
        // General Information
        public const string Name = "Maquina";
        public const double Version = 0.01;
        // XML Files
        public const string ResourceXml = "resources.xml";
        public const string PreferencesXml = "preferences.xml";
        public const string LocaleDefinitionXml = "locale.xml";
        // Resources
        public const string ContentRootDirectory = "Content";
        public const string LocalesDirectory = "locales";
        public const string DefaultLocale = "en-US";
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
            Console.WriteLine(" * {0} Game Engine, Platform: v{1}",
                new object[] { Platform.Name, Platform.Version });
            Console.WriteLine(" * Developer Console");
            Console.WriteLine(" */");
            Console.WriteLine();
        }

        public static bool VerboseOutput { get; private set; }
        public static bool PromptForRestart { get; private set; }
#endif
    }
}
