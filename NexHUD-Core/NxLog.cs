using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace NexHUDCore
{
    public class NxLog
    {
        private static bool initialized = false;
        public static void log(string message, params object[] args)
        {
            if (!initialized)
                init();
            Log.Logger.Debug(string.Format(message, args));
        }
        public static void init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\NexHud.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            initialized = true;
        }
        public static void flush()
        {
            Log.CloseAndFlush();
            initialized = false;
        }
    }
}
