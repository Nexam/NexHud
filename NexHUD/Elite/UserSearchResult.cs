using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Elite
{
    public class UserSearchResult
    {
        public enum UserSearchError
        {
            None = 0,
            CurrentSystemNotCompleted,
            Aborted,
            UnknowError
        }
        private bool m_isDone = false;
        private uint m_searchId = 0;
        private NxSearchEntry m_entry;
        private List<EDSystem> m_systems = new List<EDSystem>();
        public bool isDone { get { return m_isDone; } set { m_isDone = value; } }
        public NxSearchType searchType { 
            get {
                if (m_entry != null)
                    return m_entry.searchType;
                else
                    return NxSearchType.none;
            } 
        }
        public NxSearchDisplay[] displays { get {
                if (m_entry != null)
                    return m_entry.searchDisplayFormated;
                else
                    return new NxSearchDisplay[0];
            } }

        public uint searchID { get { return m_searchId; } }

        public NxSearchEntry entry { get { return m_entry; } }

        public UserSearchError Error = UserSearchError.None;

        public int Count { get { return m_systems.Count; } }

        public long ResearchTime = 0;
        public int CurrentPass = 1;
        public int CurrentRadius = 0;
        public bool messageDisplayed = false;

        public UserSearchResult(NxSearchEntry _entry, uint _searchId)
        {
            m_entry = _entry;
            m_searchId = _searchId;
        }

        public List<EDSystem> getSystemByDist()
        {
            IEnumerable<EDSystem> _result = m_systems.OrderBy(s => s.distanceFromCurrentSystem);

            return new List<EDSystem>(_result);
        }

        public void addSystem(EDSystem _system)
        {
            _system.calculDistanceFromCurrent();
            m_systems.Add(_system);
        }

    }
}
