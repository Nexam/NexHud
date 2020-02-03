using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore;

namespace NexHUD
{
    public class FocusHelper
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private static void BringWindowToFront(Process pTemp)
        {
            SetForegroundWindow(pTemp.MainWindowHandle);
        }

        
        public static void focusOnGame()
        {
            Process[] process = Process.GetProcessesByName("EliteDangerous64");
            if( process.Length > 0 )
            {
                BringWindowToFront(process[0]);
            }
        }
        public static void focusonNexHud()
        {
            SetForegroundWindow(NexHudEngine.gameWindow.WindowInfo.Handle);
        }
    }
}
