using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maquina
{
    public static class CommandParser
    {
        public static void Start()
        {
#if HAS_CONSOLE
            // Run the command parser on a separate thread to avoid blocking
            // the main thread. MonoGame doesn't support running the game
            // asynchronously and using Task will cause problems.
            Thread parserThread = new Thread(() => ParseCommands());
            parserThread.Start();
        }

        // TODO: Create a console command interface and use it here
        //       The current setup (hardcoded) works but doesn't provide a way
        //       for the game-side project to add their own console commands
        private static void ParseCommands()
        {
            WriteHeader();
            while (true)
            {
                Console.Write("> ");
                string[] action = Console.ReadLine().Split(' ');

                // XXX: We have 4 commands, 3 subs, and 2 aliases
                switch (action[0].ToLower())
                {
                    case "quit":
                    case "exit":
                        Global.Game.Exit();
                        Environment.Exit(0);
                        break;
                    case "restart":
                        Global.Game.Exit();
                        Process.Start(Assembly.GetEntryAssembly().Location);
                        Environment.Exit(0);
                        break;
                    case "cls":
                    case "clear":
                        WriteHeader();
                        break;
                    case "log":
                        if (action.Length < 2)
                        {
                            CommandRequiresArgument("log", "clear, showall, redirect");
                            break;
                        }
                        switch (action[1])
                        {
                            case "clear":
                                LogManager.Entries.Clear();
                                break;
                            case "showall":
                                for (int i = 0; i < LogManager.Entries.Count; i++)
                                {
                                    Console.WriteLine(LogManager.Entries[i].ToString());
                                }
                                break;
                            case "redirect":
                                WriteHeader();
                                Console.WriteLine("Press Esc to stop.");
                                LogManager.RedirectOutputToConsole = true;
                                while (Console.ReadKey().Key != ConsoleKey.Escape)
                                {
                                    // Stop redirecting output when escape key is pressed
                                }
                                LogManager.RedirectOutputToConsole = false;
                                break;
                            default:
                                CommandRequiresArgument("log", "clear, showall, redirect");
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }

        private static void CommandRequiresArgument(string command, string arguments)
        {
            Console.WriteLine("Command `{0}` requires any of the following arguments: {1}",
                command, arguments);
        }

        private static void WriteHeader()
        {
            Console.Clear();
            string consoleName = "Maquina Developer Console";
            Console.Title = consoleName;
            Console.WriteLine(consoleName);
            Console.WriteLine("API Version: {0} | Build identifier: {1}", Global.APIVersion, Global.BuildId);
            Console.WriteLine();
        }
#else
        }
#endif
    }
}
    