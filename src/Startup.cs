using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class Startup
    {
        public static Action GameAction { get; set; }
        public static void Run(string[] args)
        {
#if HAS_CONSOLE
            if (args != null)
            {
                // Enumerate passed arguments
                foreach (string arg in args)
                {
                    switch (arg.ToLower())
                    {

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

            GameAction();

#if HAS_CONSOLE
            while (PromptForRestart)
            {
                Console.WriteLine("Restart? Y = Yes, Other keys = No");
                GC.Collect();
                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    WriteHeader();
                    GameAction();
                }
                else
                    return;
            }
        }

        private static void WriteHeader()
        {
            Console.Clear();
            Console.Title = String.Format("{0} Console", Global.Name);
            Console.WriteLine("/*");
            Console.WriteLine(" * {0} Developer Console", Global.Name);
            Console.WriteLine(" * API Version {0}, built on {1}", Global.APIVersion, Global.BuildDate);
            Console.WriteLine(" */");
            Console.WriteLine();
        }

        public static bool PromptForRestart { get; private set; }
#else
        }
#endif
    }
}
