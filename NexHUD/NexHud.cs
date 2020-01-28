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
using NexHUD.Spansh;
using NexHUD.Elite.Enums;
using NexHUD.EDEngineer;

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

            //To test Spansh.co.uk for bodies search. I know, this is dirty :)
            /*BodyTester();
            return;*/

            //Engineer test
           /* EngineerTester();
            return;*/



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

        private static void EngineerTester()
        {
            string _bpJson = ResHelper.GetResourceText(Assembly.GetExecutingAssembly(), "Datas.blueprints.json");

            BlueprintDatas[] _datas = Newtonsoft.Json.JsonConvert.DeserializeObject<BlueprintDatas[]>(_bpJson);

            Console.WriteLine("datas:" + _datas.Length);

            //listing
            List<string> _type = new List<string>();
            foreach (BlueprintDatas d in _datas)
            {
                if (!_type.Contains(d.Name) && !d.Engineers.Contains("@Synthesis") && !d.Engineers.Contains("@Technology") && d.Type != "Unlock")
                    _type.Add(d.Name);
            }
            for (int i = 0; i < _type.Count; i++)
                Console.WriteLine(i + ":" + _type[i]);

            using (StreamWriter output = new StreamWriter("Blueprints.cs"))
            {
                output.WriteLine("public enum BlueprintName");
                output.WriteLine("{");
                foreach (string t in _type)
                    output.WriteLine(t.Replace(" ", "").Replace("-","_") +",");
                output.WriteLine("}");
            }

            Console.ReadKey();
        }
        private static void BodyTester()
        {
            string _currentSystem = "Nemehi";


            Stopwatch _watch = new Stopwatch();
            _watch.Start();


            //Random search
            EliteRawMaterial[] _mats = new EliteRawMaterial[] { EliteRawMaterial.Phosphorus };
            // Random r = new Random();
            // m = (EliteMaterial)r.Next(Enum.GetNames(typeof(EliteMaterial)).Length);

            Console.Write("Get best match around " + _currentSystem + " for: ");
            for (int j = 0; j < _mats.Length; j++)
            {
                Console.Write(_mats[j] + "(" + EliteMaterialHelper.getGrade(_mats[j]) + ")" + (j < _mats.Length - 1 ? "" : ","));
            }
            Console.WriteLine();

            int _distance = 100;
            int _step = 100;
            int _totalCount = 0;
            while (_distance <= 100)
            {
                Console.WriteLine("search from {0} to {1}", _distance - _step, _distance);
                SpanshBodiesResult _response = ExternalDBConnection.SpanshBodies(_currentSystem, _mats);

                Console.WriteLine(_response);

                if (_response != null)
                {

                    for (int i = 0; i < _response.results.Length; i++)
                    {
                        Console.Write("- {0} (Dist:{1}. DistTA:{2}", _response.results[i].name, Math.Round((double)_response.results[i].distance, 1), _response.results[i].distance_to_arrival);
                        for (int j = 0; j < _mats.Length; j++)
                        {
                            Console.Write("| {0}: {1}%",
                                _mats[j],
                                _response.results[i].materials.Where(x => x.name == _mats[j].ToString()).FirstOrDefault().share,
                                _response.results[i].distance,
                                _response.results[i].distance_to_arrival
                                );

                        }
                        Console.WriteLine();
                    }
                    _totalCount += _response.results.Length;
                }

                if (_totalCount < 11)
                {
                    _distance += _step;
                    Console.WriteLine("Extending the search...");
                }
                else
                    break;
            }

            _watch.Stop();
            Console.WriteLine("Search finisehd in {0}ms", _watch.ElapsedMilliseconds);
            Console.WriteLine("Type any key to exit");
            Console.ReadKey();
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
