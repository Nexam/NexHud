using EliteAPI;
using NexHUD.EDEngineer;
using NexHUD.Elite;
using NexHUD.UI;
using NexHUDCore;
using NexHUDCore.NxItems;
using OpenTK;
using Somfic.Logging.Handlers;
using System;

namespace NexHUD
{
    public class NexHudCore
    {
        public const string version = "0.0.1";

        private static NexHUDOverlay m_vrConsoleOverlay;
        private static NxTextbox m_vrConsoleTb;
        private static EliteDangerousAPI m_eliteApi;
        public static EliteDangerousAPI eliteApi { get { return m_eliteApi; } }

        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            initLogs();
            //performTests();
            loadConfigs();
            initElite();

            initVR();
            initVRConsole();

            new NxMenu();

            SteamVR_NexHUD.RunOverlays(); // Runs update/draw calls for all active overlays. And yes, it's blocking.
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

        }

        private static void initLogs()
        {
            //Core Log event
            SteamVR_NexHUD.LogEvent += SteamVR_NexHUD_Log;
            //Create the vrConsole to register whatever happen before VR is initialized
            m_vrConsoleTb = new NxTextbox(0, 0, 700, 500, "", EDColors.ORANGE, 24);
            m_vrConsoleTb.reverseDrawing = true;
            m_vrConsoleTb.reverseLineOrder = true;
        }
        private static void initVR()
        {
            SteamVR_NexHUD.Log("VR Initilization...");
            SteamVR_NexHUD.Init();
            SteamVR_NexHUD.FPS = 30;
        }

        private static void initVRConsole()
        {


            m_vrConsoleOverlay = new NexHUDOverlay(m_vrConsoleTb.width, m_vrConsoleTb.height, "Debug Console", "NexHUD Debug Console");
            m_vrConsoleOverlay.InGameOverlay.SetAttachment(AttachmentType.Absolute, new Vector3(-1.1f, -1.1f, -.3f), new Vector3(45, -90, 0));// new Vector3(-5.5f,3f, -3.8f), new Vector3(-35, -85, 0));
            m_vrConsoleOverlay.InGameOverlay.Alpha = 1f;
            m_vrConsoleOverlay.InGameOverlay.Width = .9f;
            m_vrConsoleOverlay.UpdateEveryFrame = true;
            m_vrConsoleOverlay.NxOverlay.dirtyCheckFreq = TimeSpan.FromSeconds(0.1);
            m_vrConsoleOverlay.NxOverlay.LogRenderTime = false;
            m_vrConsoleOverlay.NxOverlay.Add(m_vrConsoleTb);
        }

        private static void initElite()
        {
            SteamVR_NexHUD.Log("Elite API Initilization...");
            m_eliteApi = new EliteDangerousAPI();
            m_eliteApi.Logger.AddHandler(new ConsoleHandler());
            m_eliteApi.Start(false);

            m_eliteApi.Events.AllEvent += Events_AllEvent;
            SteamVR_NexHUD.Log("Welcome CMDR " + m_eliteApi.Commander.Commander);
            SteamVR_NexHUD.Log("Current Star system: " + m_eliteApi.Location.StarSystem);
            SteamVR_NexHUD.Log("Current Station: " + m_eliteApi.Location.Station);
            SteamVR_NexHUD.Log("Current Body: " + m_eliteApi.Location.Body);
            SteamVR_NexHUD.Log("Current Body type: " + m_eliteApi.Location.BodyType);

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
                    SteamVR_NexHUD.Log("ERROR Events_AllEvent: " + ex.Message.ToString());
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
