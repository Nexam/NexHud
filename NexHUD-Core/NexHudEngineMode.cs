using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUDCore
{
    /// <summary>
    /// The mode of the NexHudEngine: Vr, WindowOverlay or WindowDebug
    /// </summary>
    public enum NexHudEngineMode
    {
        /// <summary>
        /// SteamVR mode
        /// </summary>
        Vr = 0,
        /// <summary>
        /// Overlay without VR
        /// </summary>
        WindowOverlay = 1,
        /// <summary>
        /// OpenGL Window (debug)
        /// </summary>
        WindowDebug = 2,

        /// <summary>
        /// Detect if Vr is running and set the proper Engine mode
        /// </summary>
        Auto = 3,
    }
}
