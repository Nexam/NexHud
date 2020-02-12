using EliteAPI;
using NexHUD.Elite;
using NexHUD.Elite.Craft;
using NexHUD.Elite.Engineers;
using NexHUD.Elite.Searchs;
using NexHUD.Inputs;
using NexHUD.Settings;
using NexHUD.Ui;
using NexHUD.Utility;
using NexHUDCore;
using NexHUDCore.NxItems;
using OpenTK;
using System;
using System.Threading;

namespace NexHUD
{
    public class NexHudMain
    {
        private static Mutex m_mutex = null;
        public const string AppName = "NexHud";
        public const string Version = "v0.3-beta";
        public const string ArgAuto = "auto";
        public const string ArgDebug = "debug";
        public const string ArgVr = "vr";
        public const string ArgClassic = "classic";

        private static NexHudOverlay m_vrConsoleOverlay;
        private static NxTextbox m_vrConsoleTb;
        private static EliteDangerousAPI m_eliteApi;
        public static EliteDangerousAPI EliteApi { get { return m_eliteApi; } }

        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            m_mutex = new Mutex(true, AppName, out bool createdNew);

            if (!createdNew)
            {
                Console.WriteLine(AppName + " is already running! Exiting the application.");
                Console.ReadKey();
                return;
            }

            InitLogs();
            NexHudSettings.load();

            NexHudEngine.Log("NexHud Version: " + Version);

            NexHudEngineMode _requestedMode = NexHudSettings.GetInstance().nexHudMode;
            foreach (string a in args)
            {
                if (a == ArgAuto)
                    _requestedMode = NexHudEngineMode.Auto;
                else if (a == ArgDebug)
                    _requestedMode = NexHudEngineMode.WindowDebug;
                else if (a == ArgVr)
                    _requestedMode = NexHudEngineMode.Vr;
                else if (a == ArgClassic)
                    _requestedMode = NexHudEngineMode.WindowOverlay;
            }
            NexHudEngine.Log("Engine mode Requested: " + _requestedMode);
            //performTests();
            InitEliteApi();

            InitEngine(_requestedMode);
            NexHudEngine.Log("Engine mode: " + _requestedMode);

            LoadConfigs();
            

            if( NexHudEngine.engineMode == NexHudEngineMode.Vr)
                InitVrConsole();

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
        private static void LoadConfigs()
        {
            EngineerHelper.loadDatas();

            if (!Shortcuts.loadShortcuts(NexHudEngine.engineMode, NexHudSettings.GetInstance().useCustomShortcutClassic ))
                return;

            //User params
            UserSearchs.readConfigFile();

            //craft list
            Craftlist.load();

            //Searchs
            Bookmarks.Load();

        }

        private static void InitLogs()
        {
            //Core Log event
            NexHudEngine.LogEvent += OnNexHudEngineLog;
            //Create the vrConsole to register whatever happen before VR is initialized
            m_vrConsoleTb = new NxTextbox(0, 0, 700, 500, "", EDColors.ORANGE, 24);
            m_vrConsoleTb.reverseDrawing = true;
            m_vrConsoleTb.reverseLineOrder = true;
        }
        private static void InitEngine(NexHudEngineMode _mode)
        {
            NexHudEngine.Log("Engine Initilization...");
            NexHudEngine.Init(_mode);
            NexHudEngine.FPS = 30;
        }

        private static void InitVrConsole()
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

        private static void InitEliteApi()
        {
            NexHudEngine.Log(NxLog.Type.Debug, "Elite API Initilization...");
            m_eliteApi = new EliteDangerousAPI();
            m_eliteApi.Logger.AddHandler( new EliteApiLogger() );
            m_eliteApi.Start(false);

            NexHudEngine.Log(NxLog.Type.Info, "Welcome CMDR " + m_eliteApi.Commander.Commander);
            NexHudEngine.Log(NxLog.Type.Info, "Current Star system: " + m_eliteApi.Location.StarSystem);

        }

       
        private static void OnNexHudEngineLog(string line)
        {
          //  Console.WriteLine(line);

            if (m_vrConsoleTb != null)
                m_vrConsoleTb.AddLine(line);

        }
    }
}
//}
