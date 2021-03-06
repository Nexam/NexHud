﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Valve.VR;

namespace NexHUDCore
{
    public class NexHudEngine
    {
        private static float m_deltaTime;
        public static float deltaTime { get => m_deltaTime; }

        private static NexHudEngineMode m_engineMode;
        public static NexHudEngineMode engineMode { get => m_engineMode; }

        //Open VR :
        static CVRSystem _system;
        static CVRCompositor _compositor;
        static CVROverlay _overlay;
        static CVRApplications _applications;
        static int _frameSleep;
        static int _fps;
        static bool _doStop = false;

        static bool _initialised = false;

        public delegate void LogEventDelegate(string line);

        public static event LogEventDelegate LogEvent;

        public static event EventHandler PreUpdateCallback;
        public static event EventHandler PostUpdateCallback;

        public static event EventHandler PreDrawCallback;
        public static event EventHandler PostDrawCallback;

        public static bool TraceLevel = true;
        //internal const string DefaultFragmentShaderPath = "Resources.fragShader.frag";
        private static int m_cockpitTextureId = 0;


        public static bool Initialised { get { return _initialised; } }

        public static List<NexHudOverlay> Overlays;

        public static void Stop()
        {
            _doStop = true;
        }

        public static int FPS
        {
            get { return _fps; }
            set { _frameSleep = (int)((1f / (float)value) * 1000); _fps = value; }
        }

        public static CVRSystem OVRSystem
        {
            get { return _system; }
        }

        public static CVRCompositor OVRCompositor
        {
            get { return _compositor; }
        }

        public static CVROverlay OverlayManager
        {
            get { return _overlay; }
        }

        public static CVRApplications Applications
        {
            get { return _applications; }
        }

        private static GameWindow m_gameWindow;

        public static GameWindow gameWindow { get => m_gameWindow; }

        public static void Init(NexHudEngineMode _engineMode)
        {
            if (_engineMode == NexHudEngineMode.Auto)
            {
                if (isVrEnvironmentRunning())
                {
                    Log("Auto mode: Steam VR Detected !");
                    _engineMode = NexHudEngineMode.Vr;
                }
                else
                {
                    Log("Auto mode: No Steam VR detected, running classic overlay");
                    _engineMode = NexHudEngineMode.WindowOverlay;
                }
            }

            m_engineMode = _engineMode;
            Overlays = new List<NexHudOverlay>();
            switch (m_engineMode)
            {
                case NexHudEngineMode.Vr:
                    initEngineVR();
                    break;
                case NexHudEngineMode.WindowOverlay:
                case NexHudEngineMode.WindowDebug:
                    initEngineWindow();
                    break;
            }


            NexHudEngine.Log("NexHUDCore Initialised ({0})", m_engineMode);
            _initialised = true;
        }

        private static bool isVrEnvironmentRunning()
        {
            bool _vrServerFound = false;
            string[] processNames = new string[] { "vrserver" };

            foreach( string n in processNames)
            {
                Process[] _process = Process.GetProcessesByName(n);
                Console.WriteLine("list of process for \"{0}\": ", n);
                foreach (Process p in _process)
                {
                    _vrServerFound = true;
                    Console.WriteLine("- Process: {0} ({1})", p.ProcessName, p.Id);
                }
            }

            return _vrServerFound;
        }
        private static void initEngineWindow()
        {
            if (m_engineMode == NexHudEngineMode.WindowOverlay)
            {
                Rectangle resolution = Screen.PrimaryScreen.Bounds;
                m_gameWindow = new NxTransparentGameWindow(resolution.Width, resolution.Height); //TODO: auto size
                
            }
            else
                m_gameWindow = new GameWindow(1440, 900);

            
            m_gameWindow.Title = "NexHud Engine - " + m_engineMode;
            m_gameWindow.X = 50;
            m_gameWindow.Y = 50;
            //gw.WindowInfo.Handle; IntPtr
            GL.Enable(EnableCap.Texture2D);
        }
        private static void initEngineVR()
        {
            bool tryAgain = true;

            while (tryAgain && !_doStop)
            {
                try
                {
                    InitOpenVR();
                    tryAgain = false;
                }
                catch (Exception e)
                {
                    Log(e.Message);
                    Log("Trying again in 3 seconds");
                    Thread.Sleep(3000);
                }
            }

            if (_doStop)
            {
                return;
            }

            _system = OpenVR.System;
            _compositor = OpenVR.Compositor;
            _overlay = OpenVR.Overlay;
            _applications = OpenVR.Applications;


            m_gameWindow = new GameWindow();
            //gw.WindowInfo.Handle; IntPtr
            GL.Enable(EnableCap.Texture2D);

        }

        static void InitOpenVR()
        {
            EVRInitError ovrError = EVRInitError.None;

            OpenVR.Init(ref ovrError, EVRApplicationType.VRApplication_Overlay);

            if (ovrError != EVRInitError.None)
            {
                throw new Exception("Failed to init OpenVR! " + ovrError.ToString());
            }

            OpenVR.GetGenericInterface(OpenVR.IVRCompositor_Version, ref ovrError);

            if (ovrError != EVRInitError.None)
            {
                throw new Exception("Failed to init Compositor! " + ovrError.ToString());
            }

            OpenVR.GetGenericInterface(OpenVR.IVROverlay_Version, ref ovrError);

            if (ovrError != EVRInitError.None)
            {
                throw new Exception("Failed to init Overlay!");
            }
        }

        private static readonly TrackedDevicePose_t[] _poses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        private static readonly TrackedDevicePose_t[] _gamePoses = new TrackedDevicePose_t[0];

        private static void UpdatePoses()
        {
            if (_compositor == null) return;
            _compositor.GetLastPoses(_poses, _gamePoses);
        }

        private static NexHudOverlay _introOverlay;

        private static float _introWidth = 1;
        private static float _introDelay = -1;
        private static float _introTotalTime = 4f;
        private static float _introWmWidth = 0.5f;

        private static DateTime _introStart;

        public static bool isIntroFinished()
        {
            return _introDelay >= _introTotalTime;
        }
        private static void createIntroOverlay()
        {
            int size = 512;
            _introOverlay = new NexHudOverlay(size, size, "introOverlay", "NxHUD VR");

            _introOverlay.setVRPosition(new Vector3(0.0f, .2f, -1.1f), new Vector3(0.0f, 0.0f, 0));
            _introOverlay.Alpha = 0f;
            _introOverlay.setVRWidth(_introWidth);
            _introOverlay.setWMPosition(new Vector2(), _introWmWidth);

            _introOverlay.GradientIntro = false;

            _introOverlay.BMPTexture = ResHelper.GetResourceImage("Resources.Logo.png");

            _introOverlay.rebindTexture();
            // _introOverlay.BMPTexture = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\Resources\\Logo.png");

            //_introOverlay.BMPTexture.MakeTransparent();
        }


        private static Stopwatch internalWatch;
        public static void Run()
        {
            Stopwatch fpsWatch = new Stopwatch();
            internalWatch = new Stopwatch();

            createIntroOverlay();

            if (m_engineMode == NexHudEngineMode.Vr)
            {
                while (!_doStop)
                {
                    fpsWatch.Restart();
                    UpdatePoses();

                    updateEngine(); //+draw inside

                    fpsWatch.Stop();
                    Thread.Sleep(fpsWatch.ElapsedMilliseconds >= _frameSleep ? 0 : (int)(_frameSleep - fpsWatch.ElapsedMilliseconds));

                    m_deltaTime = (float)internalWatch.Elapsed.TotalMilliseconds;
                    m_deltaTime /= 1000.0f;
                }
            }
            else
            {
                m_gameWindow.UpdateFrame += onGameWindowUpdate;
                m_gameWindow.RenderFrame += onGameWindowRender;
                m_gameWindow.Load += onGameWindowLoad;
                m_gameWindow.Resize += onGameWindowResize;
                m_gameWindow.Run(FPS);
            }

        }

        private static void onGameWindowLoad(object sender, EventArgs e)
        {
            if( m_engineMode == NexHudEngineMode.WindowDebug )
            {
                m_cockpitTextureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, m_cockpitTextureId);

                Bitmap _cockpitTex = ResHelper.GetResourceImage("Resources.cockpit.png");

                BitmapData data = _cockpitTex.LockBits(new Rectangle(0, 0, _cockpitTex.Width, _cockpitTex.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _cockpitTex.Width, _cockpitTex.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                _cockpitTex.UnlockBits(data);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            }
           
        }

        private static void onGameWindowResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, m_gameWindow.Width, m_gameWindow.Height);
        }

        private static void onGameWindowRender(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(0, 0, 0, 0);
            GL.Enable(EnableCap.Texture2D);
            if (m_engineMode == NexHudEngineMode.WindowDebug)
            {
                GL.BindTexture(TextureTarget.Texture2D, m_cockpitTextureId);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 0); GL.Vertex2(-1, 1); //top left
                GL.TexCoord2(1, 0); GL.Vertex2(1, 1); //top right
                GL.TexCoord2(1, 1); GL.Vertex2(1, -1); //bottom right
                GL.TexCoord2(0, 1); GL.Vertex2(-1, -1); //bottom left
                GL.End();
            }


            drawEngine();

            GL.Disable(EnableCap.Texture2D);
            //
            m_gameWindow.SwapBuffers();
        }

        private static void onGameWindowUpdate(object sender, FrameEventArgs e)
        {
            m_deltaTime = (float)e.Time;
            updateEngine();

        }

        private static void drawEngine()
        {
            if (!isIntroFinished())
            {
                 _introOverlay.Draw();
            }
            else
            {
                PreDrawCallback?.Invoke(null, null);
                foreach (NexHudOverlay overlay in Overlays)
                {
                    overlay.Draw();
                }
                PostDrawCallback?.Invoke(null, null);
            }
        }
        private static void updateEngine()
        {
            internalWatch.Restart();

            if (!isIntroFinished())
            {
                if (_introDelay == -1)
                {
                    _introDelay = 0;
                    _introStart = DateTime.Now;
                }
                else
                    _introDelay = (float)(DateTime.Now - _introStart).TotalSeconds;



                _introOverlay.setVRWidth((0.5f + (_introDelay / _introTotalTime) * 0.05f) * _introWidth);
                _introOverlay.WmSize = (0.5f + (_introDelay / _introTotalTime) * 0.05f) * _introWmWidth;

                if (_introDelay < 1)
                    _introOverlay.Alpha = MathHelper.Clamp(_introDelay, 0, 1f);
                else if (_introDelay > _introTotalTime - 1)
                    _introOverlay.Alpha = MathHelper.Clamp(_introTotalTime - _introDelay, 0, 1f);
                else
                    _introOverlay.Alpha = 1;

                _introOverlay.Update();

                if (m_engineMode == NexHudEngineMode.Vr)
                    drawEngine();


                if (_introDelay > _introTotalTime)
                {

                    _introOverlay.Destroy();
                    Console.WriteLine("intro finished");
                }
            }
            else
            {

                PreUpdateCallback?.Invoke(null, null);
                foreach (NexHudOverlay overlay in Overlays)
                {
                    overlay.Update();
                }
                updateKeyboardStates();

                PostUpdateCallback?.Invoke(null, null);

                if (m_engineMode == NexHudEngineMode.Vr)
                {
                    drawEngine();
                }

            }



        }

        private static List<Key> m_KeyPressedBefore = new List<Key>();
        private static List<Key> m_KeyPressedThisFrame = new List<Key>();
        private static int _totalKeys = 0;
        private static List<Key> m_lastKeysReleased = new List<Key>();
        private static void updateKeyboardStates()
        {
            //clean up
            foreach (Key k in m_KeyPressedThisFrame)
            {
                if (!m_KeyPressedBefore.Contains(k))
                    m_KeyPressedBefore.Add(k);
            }
            m_KeyPressedThisFrame = new List<Key>();
            m_lastKeysReleased = new List<Key>();


            if (_totalKeys == 0)
                _totalKeys = Enum.GetNames(typeof(Key)).Length;

            m_lastKeysReleased = new List<Key>();

            for (int i = 0; i < _totalKeys; i++)
            {
                Key k = (Key)i;
                if (Keyboard.GetState().IsKeyDown(k))
                {
                    if (!m_KeyPressedBefore.Contains(k))
                    {
                        m_KeyPressedThisFrame.Add(k);
                    }
                }
                else
                {
                    if (m_KeyPressedBefore.Contains(k))
                    {
                        m_KeyPressedBefore.Remove(k);
                        m_lastKeysReleased.Add(k);
                    }
                }
            }
        }


        public static bool isKeyPressed(Key k)
        {
            return m_KeyPressedThisFrame.Contains(k);
        }
        public static bool isShortcutPressed(ShortcutEntry e)
        {
            if (e == null)
                return false;
                       
            bool _modifiersOk = true;

            foreach (Key m in e.OpenTkModifiers)
                if (!Keyboard.GetState().IsKeyDown(m))
                    _modifiersOk = false;


            if (_modifiersOk)
            {
                return m_KeyPressedThisFrame.Contains(e.OpentTkKey);
            }
            else
                return false;
            //return m_KeyPressedThisFrame.Contains(k);
        }
        public static bool isShortcutIsHold(ShortcutEntry e)
        {
            if (e == null)
                return false;

            if (!Keyboard.GetState().IsAnyKeyDown)
                return false;

            bool _everythingIsHold = true;
            if (!string.IsNullOrEmpty(e.key))
                _everythingIsHold = Keyboard.GetState().IsKeyDown(e.OpentTkKey);

            foreach (Key m in e.OpenTkModifiers)
                if (!Keyboard.GetState().IsKeyDown(m))
                    _everythingIsHold = false;

            return _everythingIsHold;
        }
        public static bool isKeyHasBeenReleased(Key k)
        {
            return m_lastKeysReleased.Contains(k);
        }

       
        public static void Log(string message, params object[] _params)
        {
            NxLog.log(NxLog.Type.Debug, message, _params);
            message = string.Format(message, _params);
            LogEvent?.Invoke(message);
        }
        public static void Log(NxLog.Type type, string message, params object[] _params)
        {
            NxLog.log(type, message, _params);
            message = string.Format(message, _params);
            LogEvent?.Invoke(message);
        }

    }
}
