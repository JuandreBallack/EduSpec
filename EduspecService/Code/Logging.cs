using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EduSpecService
{
    class Logging
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void LogEntry(LogLevel loglevel, string Message)
        {
            try
            {
                logger.Log(loglevel, Message);
                //var logEntry = new LogEntry() { Severity = EventType, Priority = Priority, Message = Message };
                
            }
            catch { }
        }
    }
}
