using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NexHUDCore
{
    public class NxTransparentGameWindow : GameWindow
    {
        private Margins marg;

        //this is used to specify the boundaries of the transparent area
        internal struct Margins
        {
            public int Left, Right, Top, Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        public const int GWL_EXSTYLE = -20;

        public const int WS_EX_LAYERED = 0x80000;

        public const int WS_EX_TRANSPARENT = 0x20;

        public const int LWA_ALPHA = 0x2;

        public const int LWA_COLORKEY = 0x1;

        [DllImport("dwmapi.dll")]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

        //Always on top 
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private void init()
        {
            this.WindowBorder = WindowBorder.Hidden;
            
            //Make the window's border completely transparant
            SetWindowLong(this.WindowInfo.Handle, GWL_EXSTYLE,
                    (IntPtr)(GetWindowLong(this.WindowInfo.Handle, GWL_EXSTYLE) ^ WS_EX_LAYERED ^ WS_EX_TRANSPARENT));

            //Set the Alpha on the Whole Window to 255 (solid)
            SetLayeredWindowAttributes(this.WindowInfo.Handle, 0, 255, LWA_ALPHA);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetWindowPos(this.WindowInfo.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            //Create a margin (the whole form)
            marg.Left = 0;
            marg.Top = 0;
            marg.Right = this.Width;
            marg.Bottom = this.Height;

            //Expand the Aero Glass Effect Border to the WHOLE form.
            // since we have already had the border invisible we now
            // have a completely invisible window - apart from the DirectX
            // renders NOT in black.
            DwmExtendFrameIntoClientArea(this.WindowInfo.Handle, ref marg);
        }

        #region constructors
        public NxTransparentGameWindow()
        {
            init();
        }

        public NxTransparentGameWindow(int width, int height) : base(width, height)
        {
            init();
        }

        public NxTransparentGameWindow(int width, int height, GraphicsMode mode) : base(width, height, mode)
        {
            init();
        }

        public NxTransparentGameWindow(int width, int height, GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            init();
        }

        public NxTransparentGameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options) : base(width, height, mode, title, options)
        {
            init();
        }

        public NxTransparentGameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device) : base(width, height, mode, title, options, device)
        {
            init();
        }

        public NxTransparentGameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device, int major, int minor, GraphicsContextFlags flags) : base(width, height, mode, title, options, device, major, minor, flags)
        {
            init();
        }

        public NxTransparentGameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device, int major, int minor, GraphicsContextFlags flags, IGraphicsContext sharedContext) : base(width, height, mode, title, options, device, major, minor, flags, sharedContext)
        {
            init();
        }

        public NxTransparentGameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device, int major, int minor, GraphicsContextFlags flags, IGraphicsContext sharedContext, bool isSingleThreaded) : base(width, height, mode, title, options, device, major, minor, flags, sharedContext, isSingleThreaded)
        {
            init();
        }
        #endregion
    }
}
