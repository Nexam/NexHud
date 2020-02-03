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

namespace NexHUD
{
    public class NexHudMain
    {
        public const string version = "v0.2-beta";
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
            initLogs();
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
            NexHudEngine.Log("Engine mode: " + _requestedMode);
            //performTests();
            initElite();

            initEngine(_requestedMode);

            loadConfigs();
            

            if( NexHudEngine.engineMode == NexHudEngineMode.Vr)
                initVRConsole();

            new NxMenu();

            NexHudEngine.Run(); // Runs update/draw calls for all active overlays. And yes, it's blocking.
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

            if (!Shortcuts.loadShortcuts())
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
            NexHudEngine.Log("Elite API Initilization...");
            m_eliteApi = new EliteDangerousAPI();
            m_eliteApi.Logger.AddHandler(new ConsoleHandler());
            m_eliteApi.Start(false);

            m_eliteApi.Events.AllEvent += Events_AllEvent;
            NexHudEngine.Log("Welcome CMDR " + m_eliteApi.Commander.Commander);
            NexHudEngine.Log("Current Star system: " + m_eliteApi.Location.StarSystem);
            NexHudEngine.Log("Current Station: " + m_eliteApi.Location.Station);
            NexHudEngine.Log("Current Body: " + m_eliteApi.Location.Body);
            NexHudEngine.Log("Current Body type: " + m_eliteApi.Location.BodyType);

        }

        private static void Events_AllEvent(object sender, dynamic e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Elite Event triggered: " + e);
            Console.ResetColor();

            if (m_vrConsoleTb != null)
            {
                try
                {
                    m_vrConsoleTb.AddLine(e.ToString());
                }
                catch (Exception ex)
                {
                    NexHudEngine.Log("ERROR Events_AllEvent: " + ex.Message.ToString());
                }
            }
        }

        private static void SteamVR_NexHUD_Log(string line)
        {
            Console.WriteLine(line);

            if (m_vrConsoleTb != null)
                m_vrConsoleTb.AddLine(line);

        }
    }
}
//}
