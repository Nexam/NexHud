using EliteAPI;
using NexHUD.EDEngineer;
using NexHUD.Elite;
using NexHUD.Elite.Craftlist;
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
        public const string version = "0.0.1";

        private static NexHudOverlay m_vrConsoleOverlay;
        private static NxTextbox m_vrConsoleTb;
        private static EliteDangerousAPI m_eliteApi;
        public static EliteDangerousAPI eliteApi { get { return m_eliteApi; } }

        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            bool _debugMode = false;
            foreach(string a in args)
            {
                if (a == "debug")
                    _debugMode = true;
            }
            initLogs();
            //performTests();
            initElite();

            initEngine(_debugMode ? NexHudEngineMode.WindowDebug: NexHudEngineMode.Auto);

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
