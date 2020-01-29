using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Valve.VR;

namespace NexHUDCore
{
    public class SteamVR_NexHUD
    {
        public static float deltaTime;
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

        public static bool TraceLevel = false;
        public static bool PrefixOverlayType = true;
        internal const string DefaultFragmentShaderPath = "Resources.fragShader.frag";

        public static NexHUDOverlay ActiveKeyboardOverlay = null;

        public static bool Initialised { get { return _initialised; } }

        public static List<NexHUDOverlay> Overlays;

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

        public static GameWindow gw;

        public static void Init()
        {
            Overlays = new List<NexHUDOverlay>();

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


            gw = new GameWindow();
            GL.Enable(EnableCap.Texture2D);

            SteamVR_NexHUD.Log("NexHUDCore Initialised");

            _initialised = true;

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

        private static NexHUDOverlay _introOverlay;

        private static float _introWidth = 1;
        private static float _introDelay = -1;
        private static float _introTotalTime = 4f;
        private static DateTime _introStart;

        public static bool isIntroFinished()
        {
            return _introDelay >= _introTotalTime;
        }
        private static void createIntroOverlay()
        {
            int size = 512;
            _introOverlay = new NexHUDOverlay(size, size, "introOverlay", "NxHUD VR");

            _introOverlay.InGameOverlay.SetAttachment(AttachmentType.Absolute, new Vector3(0.0f, .2f, -1.1f), new Vector3(0.0f, 0.0f, 0));
            _introOverlay.InGameOverlay.Alpha = 0f;
            _introOverlay.InGameOverlay.Width = _introWidth;

            _introOverlay.UpdateEveryFrame = true;
            _introOverlay.GradientIntro = false;

            _introOverlay.BMPTexture = ResHelper.GetResourceImage("Resources.Logo.png");

            // _introOverlay.BMPTexture = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\Resources\\Logo.png");

            //_introOverlay.BMPTexture.MakeTransparent();
        }


        private static Stopwatch internalWatch;
        public static void RunOverlays()
        {
            Stopwatch fpsWatch = new Stopwatch();
            internalWatch = new Stopwatch();
            VREvent_t eventData = new VREvent_t();
            uint vrEventSize = (uint)Marshal.SizeOf<VREvent_t>();

            createIntroOverlay();



            while (!_doStop)
            {
                fpsWatch.Restart();
                internalWatch.Restart();

                UpdatePoses();

                //Working: 
                /*if( Keyboard.GetState().IsKeyDown(Key.Space) )
                {
                    Console.WriteLine("Space DOWN !!!");
                }*/

                if (!isIntroFinished())
                {
                    if (_introDelay == -1)
                    {
                        _introDelay = 0;
                        _introStart = DateTime.Now;
                    }
                    else
                        _introDelay = (float)(DateTime.Now - _introStart).TotalSeconds;



                    _introOverlay.InGameOverlay.Width = (0.5f + (_introDelay / _introTotalTime) * 0.05f) * _introWidth;

                    if (_introDelay < 1)
                        _introOverlay.InGameOverlay.Alpha = MathHelper.Clamp(_introDelay, 0, 1f);
                    else if (_introDelay > _introTotalTime - 1)
                        _introOverlay.InGameOverlay.Alpha = MathHelper.Clamp(_introTotalTime - _introDelay, 0, 1f);
                    else
                        _introOverlay.InGameOverlay.Alpha = 1;

                    //Console.WriteLine(_introDelay);
                    _introOverlay.Update();
                    _introOverlay.Draw();
                    if (_introDelay > _introTotalTime)
                    {

                        _introOverlay.Destroy();
                        Console.WriteLine("intro finished");
                    }
                }
                else
                {

                    PreUpdateCallback?.Invoke(null, null);
                    foreach (NexHUDOverlay overlay in Overlays)
                    {
                        overlay.Update();
                    }
                    updateKeyboardStates();

                    PostUpdateCallback?.Invoke(null, null);

                    PreDrawCallback?.Invoke(null, null);
                    foreach (NexHUDOverlay overlay in Overlays)
                    {
                        overlay.Draw();
                    }

                    PostDrawCallback?.Invoke(null, null);
                }


                fpsWatch.Stop();
                Thread.Sleep(fpsWatch.ElapsedMilliseconds >= _frameSleep ? 0 : (int)(_frameSleep - fpsWatch.ElapsedMilliseconds));

                deltaTime = (float)internalWatch.Elapsed.TotalMilliseconds;
                deltaTime /= 1000.0f;

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

        public static void Log(string message)
        {
            LogEvent?.Invoke(message);
        }
        public static void Log(string message, params object[] _params)
        {
            message = string.Format(message, _params);
            LogEvent?.Invoke(message);
        }

    }
}
