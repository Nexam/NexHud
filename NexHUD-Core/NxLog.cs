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
        public enum Type
        {
            Info,
            Debug,
            Warning,
            Error,
            Fatal,
            Verbose
        }
        private static bool initialized = false;
        public static void log(Type t, string message, params object[] args)
        {
            if (!initialized)
                init();
            switch (t)
            {
                case Type.Info:
                    Log.Logger.Information(string.Format(message, args));
                    break;
                case Type.Debug:
                    Log.Logger.Debug(string.Format(message, args));
                    break;
                case Type.Warning:
                    Log.Logger.Warning(string.Format(message, args));
                    break;
                case Type.Error:
                    Log.Logger.Error(string.Format(message, args));
                    break;
                case Type.Fatal:
                    Log.Logger.Fatal(string.Format(message, args));
                    break;
                case Type.Verbose:
                    Log.Logger.Verbose(string.Format(message, args));
                    break;
            }
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
