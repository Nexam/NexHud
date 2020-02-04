using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore;
using Somfic.Logging;

namespace NexHUD.Utility
{
    public class EliteApiLogger : ILoggerHandler
    {
        public void WriteLog(LogMessage message)
        {
            switch (message.Severity)
            {
                case Severity.Success:
                    NexHudEngine.Log(NxLog.Type.Info, message.Content);
                    break;
                case Severity.Info:
                    NexHudEngine.Log(NxLog.Type.Info, message.Content);
                    break;
                case Severity.Warning:
                    NexHudEngine.Log(NxLog.Type.Warning, message.Content);
                    break;
                case Severity.Error:
                    NexHudEngine.Log(NxLog.Type.Error, message.Content);
                    break;
                case Severity.Debug:
                    NexHudEngine.Log(NxLog.Type.Debug, message.Content);
                    break;
                case Severity.Special:
                    NexHudEngine.Log(NxLog.Type.Info, message.Content);
                    break;
            }
        }
    }
}
