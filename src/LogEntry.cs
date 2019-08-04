using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public class LogEntry
    {
        public LogEntry(LogEntryLevel entryType, int logGroup, string message)
        {
            EntryType = entryType;
            Message = message;
            Timestamp = DateTime.Now;
            LogGroup = logGroup;
        }

        public LogEntryLevel EntryType { get; private set; }
        public string Message { get; private set; }
        public int LogGroup { get; private set; }
        public DateTime Timestamp { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}-{1} - {2}: {3}",
                Timestamp.ToShortDateString(), Timestamp.ToLongTimeString(),
                EntryType.ToString(), Message);
        }
    }
}
