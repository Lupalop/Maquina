using Maquina.Elements;
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

                // XXX: We have 5 commands, 6 subs, and 2 aliases
                switch (action[0].ToLower())
                {
                    case "quit":
                    case "exit":
                        Application.Game.Exit();
                        Environment.Exit(0);
                        break;
                    case "restart":
                        Application.Game.Exit();
                        Process.Start(Assembly.GetEntryAssembly().Location);
                        Environment.Exit(0);
                        break;
                    case "cls":
                    case "clear":
                        WriteHeader();
                        break;
                    case "query":
                        if (action.Length < 3)
                        {
                            CommandRequiresArgument("query", "[required] where (scene, overlay), [required] elementName (string)");
                            break;
                        }
                        // Limitation: can only tap into main element dictionary
                        switch (action[1])
                        {
                            case "scene":
                                GetContainerElementsState(Application.Scenes.CurrentScene.Elements.Values, action[2]);
                                break;
                            case "overlay":
                                for (int i = 0; i < Application.Scenes.Overlays.Count; i++)
                                {
                                    GetContainerElementsState(Application.Scenes.Overlays.Values.ElementAt(i).Elements.Values, action[2]);
                                }
                                break;
                            default:
                                CommandRequiresArgument("query", "[required] where (scene, overlay), [required] elementName (string)");
                                break;
                        }
                        break;
                    case "log":
                        if (action.Length < 2)
                        {
                            CommandRequiresArgument("log", "[required] action (clear, show, showall, redirect)");
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
                            case "show":
                                if (action.Length < 3)
                                {
                                    CommandRequiresArgument("log show", "[required] logGroup (int), [optional] logLevel (int)");
                                    break;
                                }
                                int logGroup = 0;
                                int logLevel = -1;
                                if (!int.TryParse(action[2], out logGroup))
                                {
                                    CommandRequiresArgument("log show", "[required] logGroup (int), [optional] logLevel (int)");
                                }
                                if (action.Length == 4)
                                {
                                    int.TryParse(action[3], out logLevel);
                                }
                                for (int i = 0; i < LogManager.Entries.Count; i++)
                                {
                                    LogEntry entry = LogManager.Entries[i];
                                    if (entry.LogGroup == logGroup)
                                    {
                                        if (logLevel != -1 && entry.EntryType == (LogEntryLevel)logLevel)
                                        {
                                            Console.WriteLine(entry.ToString());
                                        }
                                        else if (logLevel == -1)
                                        {
                                            Console.WriteLine(entry.ToString());
                                        }
                                    }
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
                                CommandRequiresArgument("log", "[required] action (clear, show, showall, redirect)");
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }

        private static void GetContainerElementsState(ICollection<BaseElement> elements, string elementName)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                BaseElement element = elements.ElementAt(i);
                if (element.Name.ToLower() == elementName.ToLower())
                {
                    //Console.WriteLine(element.ToString());
                    GetElementState(element);
                }
                if (element is IContainerElement)
                {
                    GetContainerElementsState(((IContainerElement)element).Children.Values, elementName);
                }
            }
        }

        private static void GetElementState(BaseElement element)
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine(string.Format("Element `{0}` information:", element.Name));
            info.AppendLine();
            info.AppendLine(string.Format("ID: {0}", element.Id));
            info.AppendLine(string.Format("Bounds: {0}", element.Bounds));
            info.AppendLine(string.Format("Actual Bounds: {0}", element.ActualBounds));
            info.AppendLine(string.Format("Scale: {0}", element.Scale));
            info.AppendLine(string.Format("Ignore Global Scale?: {0}", element.IgnoreGlobalScale));
            if (element is GuiElement)
            {
                GuiElement guiElement = (GuiElement)element;
                info.AppendLine();
                info.AppendLine("GUI Element information:");
                info.AppendLine(string.Format("Auto Position?: {0}", guiElement.AutoPosition));
                info.AppendLine(string.Format("Is Disabled?: {0}", guiElement.Disabled));
                info.AppendLine(string.Format("Is Focused?: {0}", guiElement.Focused));
                info.AppendLine(string.Format("Horizontal Alignment: {0}", guiElement.HorizontalAlignment));
                info.AppendLine(string.Format("Vertical Alignment: {0}", guiElement.VerticalAlignment));
            }
            Console.WriteLine(info.ToString());
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
            Console.WriteLine("API Version: {0} | Build identifier: {1}", Application.APIVersion, Application.BuildId);
            Console.WriteLine();
        }
#else
        }
#endif
    }
}
    