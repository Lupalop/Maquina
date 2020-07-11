using Maquina.Entities;
using Maquina.UI;
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
#if MGE_CONSOLE
            // Run the command parser on a separate thread to avoid blocking
            // the main thread. MonoGame doesn't support running the game
            // asynchronously and using Task will cause problems.
            Thread parserThread = new Thread(ParseCommands);
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

                // XXX: We have 10 commands, 14 subs, and 4 aliases
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
                            CommandRequiresArgument("query", "[required] where (scene, overlay), [required] entityName (string)");
                            break;
                        }
                        // Limitation: can only tap into main entity dictionary
                        switch (action[1])
                        {
                            case "scene":
                                GetContainerEntitiesState(Application.Scenes.CurrentScene.Entities, action[2]);
                                break;
                            case "overlay":
                                for (int i = 0; i < Application.Scenes.Overlays.Count; i++)
                                {
                                    GetContainerEntitiesState(Application.Scenes.Overlays[i].Entities, action[2]);
                                }
                                break;
                            default:
                                CommandRequiresArgument("query", "[required] where (scene, overlay), [required] entityName (string)");
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
                    case "sp":
                    case "setpref":
                        if (action.Length < 4)
                        {
                            CommandRequiresArgument("setpref", "[required] type (bool, int, float, string), [required] name (string), [required] value (depends on type)");
                            break;
                        }
                        switch (action[1])
                        {
                            case "bool":
                                bool boolValue;
                                bool.TryParse(action[3], out boolValue);
                                Application.Preferences.SetBoolean(action[2], boolValue);
                                break;
                            case "int":
                                int intValue;
                                int.TryParse(action[3], out intValue);
                                Application.Preferences.SetInt32(action[2], intValue);
                                break;
                            case "float":
                                float floatValue;
                                float.TryParse(action[3], out floatValue);
                                Application.Preferences.SetFloat(action[2], floatValue);
                                break;
                            case "string":
                                Application.Preferences.SetString(action[2], action[3]);
                                break;
                            default:
                                CommandRequiresArgument("setpref", "[required] prefType (bool, int, float, string), [required] prefName (string)");
                                break;
                        }
                        Console.WriteLine("Preference set.");
                        break;
                    case "gp":
                    case "getpref":
                        if (action.Length < 3)
                        {
                            CommandRequiresArgument("setpref", "[required] prefType (bool, int, float, string), [required] prefName (string)");
                            break;
                        }
                        switch (action[1])
                        {
                            case "bool":
                                Console.WriteLine("{0}: {1}", action[2], Application.Preferences.GetBoolean(action[2]));
                                break;
                            case "int":
                                Console.WriteLine("{0}: {1}", action[2], Application.Preferences.GetInt32(action[2]));
                                break;
                            case "float":
                                Console.WriteLine("{0}: {1}", action[2], Application.Preferences.GetFloat(action[2]));
                                break;
                            case "string":
                                Console.WriteLine("{0}: {1}", action[2], Application.Preferences.GetString(action[2]));
                                break;
                            default:
                                CommandRequiresArgument("setpref", "[required] prefType (bool, int, float, string), [required] prefName (string)");
                                break;
                        }
                        break;
                    case "resetprefs":
                        Application.Preferences.Reset();
                        break;
                    case "freeze":
                        Application.Scenes.CurrentScene.Enabled = false;
#if DEBUG
                        Application.AnimationsEnabled = false;
                        Application.TimersEnabled = false;
#endif
                        break;
                    case "unfreeze":
                        Application.Scenes.CurrentScene.Enabled = true;
#if DEBUG
                        Application.AnimationsEnabled = true;
                        Application.TimersEnabled = true;
#endif
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }

        private static void GetContainerEntitiesState(ICollection<Entity> entities, string entityName)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities.ElementAt(i);
                if (entity.Name.ToLower() == entityName.ToLower())
                {
                    //Console.WriteLine(entity.ToString());
                    GetEntityState(entity);
                }
                if (entity is IEntityContainer)
                {
                    GetContainerEntitiesState(((IEntityContainer)entity).Children, entityName);
                }
            }
        }

        private static void GetEntityState(Entity entity)
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine(string.Format("Entity `{0}` information:", entity.Name));
            info.AppendLine();
            info.AppendLine(string.Format("ID: {0}", entity.Id));
            info.AppendLine(string.Format("Bounds: {0}", entity.Bounds));
            info.AppendLine(string.Format("Actual Bounds: {0}", entity.ActualBounds));
            info.AppendLine(string.Format("Scale: {0}", entity.Scale));
            info.AppendLine(string.Format("Ignore Display Scale?: {0}", entity.IgnoreDisplayScale));
            if (entity is Control)
            {
                Control control = (Control)entity;
                info.AppendLine();
                info.AppendLine("UI Control information:");
                info.AppendLine(string.Format("Auto Position?: {0}", control.AutoPosition));
                info.AppendLine(string.Format("Is Enabled?: {0}", control.Enabled));
                info.AppendLine(string.Format("Is Focused?: {0}", control.Focused));
                info.AppendLine(string.Format("Horizontal Alignment: {0}", control.HorizontalAlignment));
                info.AppendLine(string.Format("Vertical Alignment: {0}", control.VerticalAlignment));
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
            string engineName = "Maquina Game Engine";
            Console.Title = $"{engineName} Console";
            Console.WriteLine("{0} [Version {1}]", engineName, Application.Version);
            Console.WriteLine("Build timestamp: {0}", Application.BuildDateTime);
            Console.WriteLine();
        }
#else
        }
#endif
    }
}
    