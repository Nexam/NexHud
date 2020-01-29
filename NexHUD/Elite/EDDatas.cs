using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NexHUD.EDDB;
using NexHUD.EDEngineer;
using NexHUD.EDSM;
using NexHUD.Spansh;
using NexHUD.UI;
using NexHUDCore;

namespace NexHUD.Elite
{
    public class EDDatas
    {
        #region singleton
        public static EDDatas Instance { get { return Nested.instance; } }


        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly EDDatas instance = new EDDatas();
        }
        #endregion


        /** Internal Datas Base **/
        private Dictionary<string, EDSystem> m_systems;
        private EDSystem _unknowSystem;
        private string _lastCurrentSystem;

        private List<string> m_systemAround = new List<string>();
        private int m_greatestSearchRadius = 0;

        /** Search thread & last results **/
        Thread m_lastSearchThread;
        Dictionary<uint, UserSearchResult> m_lastUSRs = new Dictionary<uint, UserSearchResult>();
        uint m_lastSearchId = 0;

        /** Current system thread **/
        Thread m_currentSystemThread;

        private EDDatas()
        {
            m_systems = new Dictionary<string, EDSystem>();

            //Unknows
            _unknowSystem = new EDSystem();
            _unknowSystem.name = "Unknow";
            _unknowSystem.security = EliteSecurity.Lawless;
            _unknowSystem.government = EliteGovernment.None;

            Program.EliteAPI.Events.LocationEvent += Events_LocationEvent;
        }

        private void Events_LocationEvent(object sender, EliteAPI.Events.LocationInfo e)
        {
            if (e.StarSystem != _lastCurrentSystem)
            {
                m_systemAround = new List<string>();
                m_greatestSearchRadius = 0;
                getCurrentSystem();
            }
        }

        public UserSearchResult getUserSearchResult(uint _id)
        {
            if (m_lastUSRs.ContainsKey(_id))
                return m_lastUSRs[_id];
            else
                return null;
        }
        public uint processUserSearch(NxSearchEntry _search)
        {
            if (_search.searchDisplay == null)
                return 0;

            if (m_lastSearchThread != null)
            {
                m_lastSearchThread.Abort();
            }

            m_lastSearchThread = null;
            m_lastSearchId++;
            m_lastUSRs.Add(m_lastSearchId, new UserSearchResult(_search, m_lastSearchId));

            SteamVR_NexHUD.Log("New search for {0} id {1}", _search.searchType, m_lastSearchId);
            m_lastSearchThread = new Thread(() => _startResearch(m_lastSearchId));
            m_lastSearchThread.Start();

            return m_lastSearchId;
        }

        private void _startResearch(uint _id)
        {
            Stopwatch _watch = new Stopwatch();
            _watch.Start();

            int _statAddedSystem = 0;
            int _statUpdatedSystem = 0;

            if (!m_lastUSRs.ContainsKey(_id))
                return;

            //Check if search is still valid
            if (m_lastUSRs[_id].searchID != m_lastSearchId)
            {
                m_lastUSRs[_id].Error = UserSearchResult.UserSearchError.Aborted;
                m_lastUSRs[_id].isDone = true;
                return;
            }
            try
            {
                if (!getCurrentSystem().receivedEdsmBasics)
                {
                    m_lastUSRs[_id].Error = UserSearchResult.UserSearchError.CurrentSystemNotCompleted;
                }
                else if (m_lastUSRs[_id].entry.searchType == NxSearchType.system)
                {
                    //If names are specified we will ignore other parameters
                    if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.name))
                    {
                        string[] _names = m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.name];
                        string[] _namesNotes = new string[0];
                        if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.nameNotes))
                            _namesNotes = m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.nameNotes];


                        for (int i = 0; i < _names.Length; i++)
                            _names[i] = _names[i].Trim();


                        for (int i = 0; i < _names.Length; i++)
                        {

                            //Can be optimized with https://www.edsm.net/api-v1/systems
                            EDSMSystemDatas _edsmDatas = ExternalDBConnection.EDSMSystemFullInfos(_names[i]);
                            if (_edsmDatas != null)
                            {
                                EDSystem _system = new EDSystem();
                                _system.updateEDSM(_edsmDatas);
                                SteamVR_NexHUD.Log("(EDSM) Found system: " + _system.name);
                                m_lastUSRs[_id].addSystem(_system);
                                if (i < _namesNotes.Length)
                                    _system.Notes = _namesNotes[i];
                            }
                        }

                    }
                    else
                    {
                        //Get systems in radius
                        int _radius = m_greatestSearchRadius;

                        int _startRardius = 40;
                        if (m_lastUSRs[_id].entry.searchMaxRadius > 0 && m_lastUSRs[_id].entry.searchMaxRadius < _startRardius)
                            _startRardius = m_lastUSRs[_id].entry.searchMaxRadius;


                        int _maxEdsmRadius = 100;
                        if (m_lastUSRs[_id].entry.searchMaxRadius > _startRardius && m_lastUSRs[_id].entry.searchMaxRadius < _maxEdsmRadius)
                            _maxEdsmRadius = m_lastUSRs[_id].entry.searchMaxRadius;

                        int[] _increments = new int[]
                    {
                            //Threshold,increment
                            60,20,
                            90,10,
                            100,5
                    };

                        int _pass = 1;
                        if (_radius < _startRardius)
                            _radius = _startRardius;

                        int _radiusMinimum = 0;
                        while (m_lastUSRs[_id].Count < NxMainPanelSearch.MAX_LINE_RESULT && _radius <= _maxEdsmRadius)
                        {
                            m_lastUSRs[_id].ResearchTime = _watch.ElapsedMilliseconds;
                            m_lastUSRs[_id].CurrentPass = _pass;
                            m_lastUSRs[_id].CurrentRadius = _radius;
                            //Building list
                            if (_radius > m_greatestSearchRadius)
                            {
                                EDSMSystemDatas[] _systemsInSphereRadius = ExternalDBConnection.EDSMSystemsInSphereRadius(getCurrentSystem().name, m_greatestSearchRadius, _radius);
                                if (_systemsInSphereRadius != null)
                                {
                                    m_greatestSearchRadius = _radius;
                                    for (int i = 0; i < _systemsInSphereRadius.Length; i++)
                                    {
                                        if (!m_systemAround.Contains(_systemsInSphereRadius[i].name))
                                        {
                                            m_systemAround.Add(_systemsInSphereRadius[i].name);
                                        }
                                        if (!m_systems.ContainsKey(_systemsInSphereRadius[i].name))
                                        {
                                            EDSystem _newSystem = new EDSystem();
                                            _newSystem.updateEDSM(_systemsInSphereRadius[i]);
                                            _statAddedSystem++;
                                            m_systems.Add(_systemsInSphereRadius[i].name, _newSystem);
                                        }
                                    }
                                }
                            }

                            List<EDSystem> _systemInRadius = new List<EDSystem>();
                            //Recalculate distance and updating basics...
                            foreach (string _sys in m_systemAround)
                            {
                                if (!m_systems.ContainsKey(_sys))
                                {
                                    _statAddedSystem++;
                                    m_systems.Add(_sys, new EDSystem());
                                }
                                if (!m_systems[_sys].receivedEdsmInfos)
                                {
                                    SteamVR_NexHUD.Log("//WARNING// system {0} has not received informations datas. Retrieving...", _sys);
                                    m_systems[_sys].updateEDSM(ExternalDBConnection.EDSMSystem(new ExternalDBConnection.EDSMSystemParameters() { name = _sys, showInformation = true }));
                                    m_systems[_sys].calculDistanceFromCurrent();
                                }
                                if (m_systems[_sys].distanceFromCurrentSystem < _radius && m_systems[_sys].distanceFromCurrentSystem >= _radiusMinimum)
                                    _systemInRadius.Add(m_systems[_sys]);
                            }


                            IEnumerable<EDSystem> _ordered = _systemInRadius.OrderBy(s => s.distanceFromCurrentSystem);

                            //Allegiance
                            if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.allegiance))
                                _ordered = _ordered.Where(x => m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.allegiance].Any(y => x.allegiance.ToStringFormated().Equals(y, StringComparison.InvariantCultureIgnoreCase)));

                            //Government
                            if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.government))
                                _ordered = _ordered.Where(x => m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.government].Any(y => x.government.ToStringFormated().Equals(y, StringComparison.InvariantCultureIgnoreCase)));

                            //Primary economy
                            if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.economy))
                                _ordered = _ordered.Where(x => m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.economy].Any(y => x.economy.ToStringFormated().Equals(y, StringComparison.InvariantCultureIgnoreCase)));

                            //Reserve
                            if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.reserve))
                                _ordered = _ordered.Where(x => m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.reserve].Any(y => x.reserve.ToStringFormated().Equals(y, StringComparison.InvariantCultureIgnoreCase)));

                            //Security
                            if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.security))
                                _ordered = _ordered.Where(x => m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.security].Any(y => x.security.ToStringFormated().Equals(y, StringComparison.InvariantCultureIgnoreCase)));

                            //State
                            if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.state))
                                _ordered = _ordered.Where(x => m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.state].Any(y => x.state.ToStringFormated().Equals(y, StringComparison.InvariantCultureIgnoreCase)));



                            SteamVR_NexHUD.Log("+ Search total result: {0} . Pass {1}", _ordered.Count(), _pass);
                            m_lastUSRs[_id].ResearchTime = _watch.ElapsedMilliseconds;
                            foreach (EDSystem x in _ordered)
                            {
                                m_lastUSRs[_id].addSystem(x);
                                if (m_lastUSRs[_id].Count >= NxMainPanelSearch.MAX_LINE_RESULT)
                                    break;
                            }
                            _radiusMinimum = _radius;

                            for (int _inc = 0; _inc < _increments.Length; _inc += 2)
                            {
                                if (_radius < _increments[_inc])
                                {
                                    _radius += _increments[_inc + 1];
                                    break;
                                }
                            }
                            if (_radius >= _maxEdsmRadius)
                                break;
                            _pass++;
                        }
                    }
                }
                else if (m_lastUSRs[_id].entry.searchType == NxSearchType.body)
                {
                    List<string> _listMaterials = new List<string>();
                    //Make sure the materials are valid
                    if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.rawMaterial))
                    {
                        foreach (string _material in m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.rawMaterial])
                        {
                            if (EngineerHelper.isRawMaterial(_material))
                                _listMaterials.Add(_material);
                        }
                    }
                    bool? isLandable = null;
                    if (m_lastUSRs[_id].entry.searchParamsFormated.ContainsKey(NxSearchParam.isLandable))
                    {
                        isLandable = m_lastUSRs[_id].entry.searchParamsFormated[NxSearchParam.isLandable][0] == "true";
                    }


                    if (_listMaterials.Count > 0)
                    {
                        int maxDistance = 20;

                        if (m_lastUSRs[_id].entry.searchMaxRadius > 0)
                            maxDistance = Math.Min(m_lastUSRs[_id].entry.searchMaxRadius, 100);

                        SpanshBodiesResult _spanshResult = ExternalDBConnection.SpanshBodies(getCurrentSystem().name, maxDistance, _listMaterials.ToArray(), isLandable);

                        //Get infos about the targeted systems
                        List<string> _systemWithBodies = new List<string>();
                        foreach (SpanshBody b in _spanshResult.results)
                        {
                            if (!_systemWithBodies.Contains(b.system_name))
                                _systemWithBodies.Add(b.system_name);

                        }

                        List<string> _systemToUpdate = new List<string>();
                        //Clean list for system we already received infos
                        foreach (string s in _systemWithBodies)
                        {
                            if (m_systems.ContainsKey(s))
                            {
                                if (!m_systems[s].receivedEdsmBasics && !m_systems[s].receivedEdsmInfos)
                                    _systemToUpdate.Add(s);
                            }
                            else
                            {
                                m_systemAround.Add(s);
                                m_systems.Add(s, new EDSystem());
                                _systemToUpdate.Add(s);
                            }
                        }
                        if( _systemToUpdate.Count > 0 )
                        {
                            EDSMSystemDatas[] _edsmDatas = ExternalDBConnection.EDSMSystemsList(_systemToUpdate.ToArray(), true);
                            foreach (EDSMSystemDatas _data in _edsmDatas)
                                m_systems[_data.name].updateEDSM(_data);
                        }

                        foreach(SpanshBody b in _spanshResult.results)
                        {
                            m_lastUSRs[_id].addBody(m_systems[b.system_name].addOrUpdateBody(b));
                        }
                        
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is ThreadAbortException)
                {
                    m_lastUSRs[_id].Error = UserSearchResult.UserSearchError.Aborted;
                    SteamVR_NexHUD.Log("Aborting last search thread {0}. Last Id {1}", Thread.CurrentThread.Name, _id);
                }
                else
                {
                    m_lastUSRs[_id].Error = UserSearchResult.UserSearchError.UnknowError;
                    SteamVR_NexHUD.Log("ERROR: Search {0} reported an error: {1}", _id, ex.Message);
                }

            }
            finally
            {
                _watch.Stop();
                SteamVR_NexHUD.Log("Search was done in {0}ms. Error:{1} Added:{2} Updated:{3}", _watch.ElapsedMilliseconds, m_lastUSRs[_id].Error, _statAddedSystem, _statUpdatedSystem);
                m_lastUSRs[_id].isDone = true;
                m_lastUSRs[_id].ResearchTime = _watch.ElapsedMilliseconds;
            }
        }


        public EDSystem getCurrentSystem()
        {
            string _systemName = Program.EliteAPI.Location.StarSystem;
            if (m_systems.ContainsKey(_systemName))
            {
                if (!m_systems[_systemName].isEDSMComplete() || !m_systems[_systemName].receivedEddbInfos)
                    updateCurrentSystemInfos();

                return m_systems[_systemName];
            }
            else
            {
                EDSystem _current = new EDSystem();
                _current.name = "Loading...";
                m_systems.Add(_systemName, _current);
                updateCurrentSystemInfos();
                return m_systems[_systemName];
            }
        }

        private void updateCurrentSystemInfos()
        {
            if (m_currentSystemThread != null && m_currentSystemThread.ThreadState == System.Threading.ThreadState.Running && _lastCurrentSystem == Program.EliteAPI.Location.StarSystem)
            {
                return; //Loading already in process
            }
            else
            {
                if (m_currentSystemThread != null) //We changed system before the loading was done for the previous.
                    m_currentSystemThread.Abort();

                _lastCurrentSystem = Program.EliteAPI.Location.StarSystem;
                m_currentSystemThread = new Thread(updateCurrentSystemThread);
                m_currentSystemThread.Start();
            }

        }

        private void updateCurrentSystemThread()
        {
            string _systemName = Program.EliteAPI.Location.StarSystem;
            if (string.IsNullOrEmpty(_systemName))
                return;
            Stopwatch _watch = new Stopwatch();
            _watch.Start();
            SteamVR_NexHUD.Log(">> Retrieving current system {0} from EDSM", _systemName);
            m_systems[_systemName].updateEDSM(ExternalDBConnection.EDSMSystemFullInfos(_systemName));
            SteamVR_NexHUD.Log(">> Retrieving value for system {0} from EDSM", _systemName);
            m_systems[_systemName].updateEDSM(ExternalDBConnection.EDSMSystemValue(_systemName));
            SteamVR_NexHUD.Log(">> Retrieving current system {0} from EDDB", _systemName);
            m_systems[_systemName].updateEDDB(ExternalDBConnection.EDDBSystemComplementaryInfos(_systemName));
            _watch.Stop();
            SteamVR_NexHUD.Log("--->> Update current system {0} took {1}ms", _systemName, _watch.ElapsedMilliseconds);
        }
    }
}
