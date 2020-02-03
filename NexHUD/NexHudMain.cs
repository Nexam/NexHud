using EliteAPI;
using NexHUD.EDEngineer;
using NexHUD.Elite;
using NexHUD.Elite.Craftlist;
using NexHUD.Settings;
using NexHUD.UI;
using NexHUDCore;
using NexHUDCore.NxItems;
using OpenTK;
using Somfic.Logging.Handlers;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace NexHUD
{
    public class NexHudMain
    {
        private static Mutex mutex = null;
        public const string appName = "NexHud";
        public const string version = "v0.3-beta";
        public const string ARG_AUTO = "auto";
        public const string ARG_DEBUG = "debug";
        public const string ARG_VR = "vr";
        public const string ARG_CLASSIC = "classic";

        private static NexHudOverlay m_vrConsoleOverlay;
        private static NxTextbox m_vrConsoleTb;
        private static EliteDangerousAPI m_eliteApi;
        public static EliteDangerousAPI eliteApi { get { return m_eliteApi; } }

        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                Console.WriteLine(appName + " is already running! Exiting the application.");
                Console.ReadKey();
                return;
            }

            initLogs();
            NexHudSettings.load();

            NexHudEngine.Log("NexHud Version: " + version);

            NexHudEngineMode _requestedMode = NexHudSettings.GetInstance().nexHudMode;
            foreach (string a in args)
            {
                if (a == ARG_AUTO)
                    _requestedMode = NexHudEngineMode.Auto;
                else if (a == ARG_DEBUG)
                    _requestedMode = NexHudEngineMode.WindowDebug;
                else if (a == ARG_VR)
                    _requestedMode = NexHudEngineMode.Vr;
                else if (a == ARG_CLASSIC)
                    _requestedMode = NexHudEngineMode.WindowOverlay;

            }
            NexHudEngine.Log("Engine mode Requested: " + _requestedMode);
            //performTests();
            initElite();

            initEngine(_requestedMode);
            NexHudEngine.Log("Engine mode: " + _requestedMode);

            loadConfigs();
            

            if( NexHudEngine.engineMode == NexHudEngineMode.Vr)
                initVRConsole();

            new NxMenu();

            try
            {
                NexHudEngine.Run(); // Runs update/draw calls for all active overlays. And yes, it's blocking.
            }
            catch(Exception ex)
            {
                NexHudEngine.Log(NxLog.Type.Fatal, ex.Message);
                NexHudEngine.Log(NxLog.Type.Fatal, ex.Source);
                NexHudEngine.Log(NxLog.Type.Fatal, ex.StackTrace);
            }
        }


     

        private static void performTests()
        {
            //To test Spansh.co.uk for bodies search. I know, this is dirty :)
            //NxTester.BodyTester();

            //Test NAudio for playing radios. EQ and Reverb later
            // NxTester.AudioTester();
            NxTester.RadioTester();
            Environment.Exit(0);
        }
        private static void loadConfigs()
        {
            EngineerHelper.loadDatas();

            if (!Shortcuts.loadShortcuts(NexHudEngine.engineMode, NexHudSettings.GetInstance().useCustomShortcutClassic ))
                return;

            //User params
            UserSearchs.readConfigFile();

            //craft list
            Craftlist.load();

        }

        private static void initLogs()
        {
            //Core Log event
            NexHudEngine.LogEvent += SteamVR_NexHUD_Log;
            //Create the vrConsole to register whatever happen before VR is initialized
            m_vrConsoleTb = new NxTextbox(0, 0, 700, 500, "", EDColors.ORANGE, 24);
            m_vrConsoleTb.reverseDrawing = true;
            m_vrConsoleTb.reverseLineOrder = true;
        }
        private static void initEngine(NexHudEngineMode _mode)
        {
            NexHudEngine.Log("Engine Initilization...");
            NexHudEngine.Init(_mode);
            NexHudEngine.FPS = 30;
        }

        private static void initVRConsole()
        {
            m_vrConsoleOverlay = new NexHudOverlay(m_vrConsoleTb.width, m_vrConsoleTb.height, "Debug Console", "NexHUD Debug Console");
            m_vrConsoleOverlay.setVRPosition(new Vector3(-1.1f, -1.1f, -.3f), new Vector3(45, -90, 0));
            m_vrConsoleOverlay.setVRWidth(.9f);

            m_vrConsoleOverlay.setWMPosition(new Vector2(-.75f, 0.1f), 0.2f);
            //m_vrConsoleOverlay.Alpha = 1f;
            m_vrConsoleOverlay.NxOverlay.dirtyCheckFreq = TimeSpan.FromSeconds(0.1);
            m_vrConsoleOverlay.NxOverlay.LogRenderTime = false;
            m_vrConsoleOverlay.NxOverlay.Add(m_vrConsoleTb);
        }

        private static void initElite()
        {
            NexHudEngine.Log(NxLog.Type.Debug, "Elite API Initilization...");
            m_eliteApi = new EliteDangerousAPI();
            m_eliteApi.Logger.AddHandler( new EliteApiLogger() );
            m_eliteApi.Start(false);

            NexHudEngine.Log(NxLog.Type.Info, "Welcome CMDR " + m_eliteApi.Commander.Commander);
            NexHudEngine.Log(NxLog.Type.Info, "Current Star system: " + m_eliteApi.Location.StarSystem);

        }

       
        private static void SteamVR_NexHUD_Log(string line)
        {
          //  Console.WriteLine(line);

            if (m_vrConsoleTb != null)
                m_vrConsoleTb.AddLine(line);

        }
    }
}
//}
