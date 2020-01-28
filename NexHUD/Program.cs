using System;
using OpenTK;
using NexHUDCore;
using EliteAPI;

using Somfic.Logging;
using Somfic.Logging.Handlers;
using NexHUDCore.NxItems;
using System.Net;
using System.IO;
using NexHUD.EDDB;
using NexHUD.UI;
using NexHUD.Elite;
using NexHUD.EDSM;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;

namespace NexHUD
{
    class Program
    {
        public const string version = "0.0.1";
        private static NexHUDOverlay m_consoleOverlay;
        private static EliteDangerousAPI m_EliteAPI;

        private static NxTextbox m_IngameConsoleTb;


        public static EliteDangerousAPI EliteAPI { get { return m_EliteAPI; } }

        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            SteamVR_NexHUD.LogEvent += SteamVR_NexHUD_Log;
            //BodyTester();
            //return;



            if (!Shortcuts.loadShortcuts())
                return;

            m_IngameConsoleTb = new NxTextbox(0, 0, 700, 500, "", EDColors.ORANGE, 24);
            m_IngameConsoleTb.reverseDrawing = true;
            m_IngameConsoleTb.reverseLineOrder = true;
            initElite();

            //User params
            UserSearchs.readConfigFile();


            //test
            SteamVR_NexHUD.Log("EDDB Status: " + (ExternalDBConnection.isEDDBOnline() ? "online" : "offline"));
            SteamVR_NexHUD.Log("EDSM Status: " + (ExternalDBConnection.isEDSMOnline() ? "online" : "offline"));

            //Console.ReadKey();



            initVR();

            SteamVR_NexHUD.PostUpdateCallback += SteamVR_NexHUD_PostUpdateCallback;

            m_consoleOverlay = new NexHUDOverlay(m_IngameConsoleTb.width, m_IngameConsoleTb.height, "Debug Console", "NexHUD Debug Console");
            m_consoleOverlay.InGameOverlay.SetAttachment(AttachmentType.Absolute, new Vector3(-1.1f, -1.1f, -.3f), new Vector3(45, -90, 0));// new Vector3(-5.5f,3f, -3.8f), new Vector3(-35, -85, 0));
            m_consoleOverlay.InGameOverlay.Alpha = 1f;
            m_consoleOverlay.InGameOverlay.Width = .9f;
            m_consoleOverlay.UpdateEveryFrame = true;
            m_consoleOverlay.NxOverlay.dirtyCheckFreq = TimeSpan.FromSeconds(0.1);
            m_consoleOverlay.NxOverlay.LogRenderTime = false;
            m_consoleOverlay.NxOverlay.Add(m_IngameConsoleTb);


            NxMenu menu = new NxMenu();
            SteamVR_NexHUD.RunOverlays(); // Runs update/draw calls for all active overlays. And yes, it's blocking.

        }

        private static void BodyTester()
        {
            string _currentSystem = "Nemehi";

            string _material = "Antimony";

            Stopwatch _watch = new Stopwatch();
            int _count = 0;
            _watch.Start();

            Console.WriteLine("Get systems arround " + _currentSystem + "...");
            //get system around;
            EDSMSystemDatas[] _edsmSystems = ExternalDBConnection.EDSMSystemsInSphereRadius(_currentSystem, 0, 40, false);
            Console.WriteLine("System list received in {0}ms | {1} systems!", _watch.ElapsedMilliseconds, _edsmSystems.Length);


            List<string> _fields = new List<string>();
            foreach (var f in typeof(EDSMMaterials).GetFields().Where(f => f.IsPublic))
            {
                _fields.Add(f.Name);
            }

            string _bestMatch = "";
            double _bestPercent = 0.0;
            double _threshold = 2.0;

            foreach (EDSMSystemDatas _edsmSys in _edsmSystems)
            {
                _count++;
                EDSMSystemDatas _systemFound = ExternalDBConnection.EDSMSystemBodies(_edsmSys.name);
                if (_systemFound == null)
                    break;
                if (_systemFound.bodies != null)
                {
                    foreach (EDSMBody _body in _systemFound.bodies)
                    {
                        if (_body.materials == null)
                            continue;
                        string _match = "";

                        bool _ThresholdOk = false;
                        foreach(string f in _fields)
                        {
                            if ( !_material.Equals(f) )
                                continue;
                            var _value = _body.materials.GetType().GetField(f).GetValue(_body.materials);
                            if (_value != null)
                            {
                                _match += string.Format("{0}:{1}%", f, _value);
                                _ThresholdOk = (double)_value > _threshold;
                                if (_bestPercent < (double)_value )
                                {
                                    _bestMatch = string.Format("Best Match = System {0}, bodie {1}. || {2}", _systemFound.name, _body.name, _match);
                                    _bestPercent = (double)_value;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(_match) && _ThresholdOk)
                            Console.WriteLine("Match for system {0} bodie {1}. {2}", _systemFound.name, _body.name, _match);
                    }
                }

                Thread.Sleep(100);
                if (_count % 10 == 0)
                {
                    Console.WriteLine("system {0}/{1}. {2}ms elapsed", _count, _edsmSystems.Length, _watch.ElapsedMilliseconds);
                    Thread.Sleep(1000);
                }
                if (_count >= 100)
                    break;
            }
            _watch.Stop();
            Console.WriteLine("Search finisehd in {0}ms", _watch.ElapsedMilliseconds);
            Console.WriteLine(_bestMatch);
            Console.WriteLine("Type any key to exit");
            Console.ReadKey();
            throw new ApplicationException("END");
        }


        private static void SteamVR_NexHUD_PostUpdateCallback(object sender, EventArgs e)
        {

        }

        private static void initVR()
        {
            SteamVR_NexHUD.Log("VR Initilization...");
            SteamVR_NexHUD.Init();
            SteamVR_NexHUD.FPS = 30;
        }

        private static void initElite()
        {
            SteamVR_NexHUD.Log("Elite API Initilization...");
            m_EliteAPI = new EliteDangerousAPI();
            m_EliteAPI.Logger.AddHandler(new ConsoleHandler());
            m_EliteAPI.Start(false);

            m_EliteAPI.Events.AllEvent += Events_AllEvent;
            SteamVR_NexHUD.Log("Welcome CMDR " + m_EliteAPI.Commander.Commander);
            SteamVR_NexHUD.Log("Current Star system: " + m_EliteAPI.Location.StarSystem);
            SteamVR_NexHUD.Log("Current Station: " + m_EliteAPI.Location.Station);
            SteamVR_NexHUD.Log("Current Body: " + m_EliteAPI.Location.Body);
            SteamVR_NexHUD.Log("Current Body type: " + m_EliteAPI.Location.BodyType);

        }

        private static void Events_AllEvent(object sender, dynamic e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Elite Event triggered: " + e);
            Console.ResetColor();

            if (m_IngameConsoleTb != null)
            {
                try
                {
                    m_IngameConsoleTb.AddLine(e.ToString());
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

            if (m_IngameConsoleTb != null)
                m_IngameConsoleTb.AddLine(line);

        }
    }
}
//}