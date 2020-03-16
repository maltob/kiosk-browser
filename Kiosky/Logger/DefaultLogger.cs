using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiosky.Logger
{
    /// <summary>
    /// Logger for the application
    /// </summary>
    public class DefaultLogger
    {
        private static ILogger logger;

        private enum LogType  {Info,Warning,Error};

        /// <summary>
        /// Saves to the default log
        /// </summary>
        private static void Log(string message, LogType type = LogType.Info)
        {
            if(logger == null)
            {
                logger = new LoggerConfiguration().WriteTo.File(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"KioskBrowser","logs","browser.log")).CreateLogger();
            }
            logger.Information(message);
        }

        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="message"></param>
        public static void LogInfo(string message)
        {
            Log(message, LogType.Info);
        }

        /// <summary>
        /// Log a warning message to the default logger
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {
            Log(message, LogType.Warning);
        }


        /// <summary>
        /// Log an error message to the default logger
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            Log(message, LogType.Error);
        }
    }
}
