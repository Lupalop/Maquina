using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class LogManager
    {
        /// <summary>
        /// A list containing objects of type <see cref="LogEntry"/>.
        /// </summary>
        public static ObservableCollection<LogEntry> Entries { get; private set; } = new ObservableCollection<LogEntry>();

        /// <summary>
        /// Gets or sets a value indicating whether messages of the log entries should be redirected to <see cref="Console"/>.
        /// </summary>
        public static bool RedirectOutputToConsole { get; set; }

        /// <summary>
        /// Inserts a new log entry to <see cref="Entries"/>.
        /// </summary>
        /// <param name="logLevel"> The severity of the log entry.</param>
        /// <param name="logGroup">The group of the log entry.</param>
        /// <param name="message">The message held by the log entry.</param>
        public static void Log(LogEntryLevel logLevel, int logGroup, string message)
        {
#if MGE_LOGGING
            LogEntry entry = new LogEntry(logLevel, logGroup, message);
            Entries.Add(entry);
#if MGE_CONSOLE
            if (RedirectOutputToConsole)
            {
                Console.WriteLine(entry.ToString());
            }
#endif
#endif
        }

        /// <summary>
        /// Inserts a new log entry to <see cref="Entries"/> with an Info severity.
        /// </summary>
        /// <param name="logGroup">The group of the log entry.</param>
        /// <param name="message">The message held by the log entry.</param>
        public static void Info(int logGroup, string message)
        {
            Log(LogEntryLevel.Info, logGroup, message);
        }

        /// <summary>
        /// Inserts a new log entry to <see cref="Entries"/> with a Warning severity.
        /// </summary>
        /// <param name="logGroup">The group of the log entry.</param>
        /// <param name="message">The message held by the log entry.</param>
        public static void Warn(int logGroup, string message)
        {
            Log(LogEntryLevel.Warning, logGroup, message);
        }

        /// <summary>
        /// Inserts a new log entry to <see cref="Entries"/> with an Error severity.
        /// </summary>
        /// <param name="logGroup">The group of the log entry.</param>
        /// <param name="message">The message held by the log entry.</param>
        public static void Error(int logGroup, string message)
        {
            Log(LogEntryLevel.Error, logGroup, message);
        }
    }
}
