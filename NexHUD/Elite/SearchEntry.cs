using System;
using System.Collections.Generic;


namespace NexHUD.Elite
{
    public class NxSearchEntry
    {
        public const int MAX_PARAMS_DISPLAY = 6;

        public string searchName;
        public NxSearchType searchType;
        public string[] searchDisplay;
        public string[][] searchParams;
        public int searchMaxRadius = 0;

        private Dictionary<NxSearchParam, string[]> m_searchParamFormated;
        private bool m_formated = false;

        public Dictionary<NxSearchParam, string[]> searchParamsFormated { get { return m_searchParamFormated; } }

        private NxSearchDisplay[] m_searchDisplayFormated;
        internal string Description;

        public NxSearchDisplay[] searchDisplayFormated { get { return m_searchDisplayFormated; } }

        public void format()
        {
            if (m_formated)
                return;

            m_searchParamFormated = new Dictionary<NxSearchParam, string[]>();

            if (searchParams != null)
            {
                for (int p = 0; p < searchParams.Length; p++)
                {
                    NxSearchParam _param = NxSearchParam.Unexpected;
                    Enum.TryParse<NxSearchParam>(searchParams[p][0], true, out _param);
                    if (_param != NxSearchParam.Unexpected && !m_searchParamFormated.ContainsKey(_param))
                    {
                        string _value = searchParams[p][1];
                        _value = _value.Replace("\r", "");
                        _value = _value.Replace("\n", "");
                        _value = string.Join(" ", _value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

                        if (_value.Length > 0)
                            m_searchParamFormated.Add(_param, _value.Split(new char[] { ';' }));
                    }
                }
            }
            List<NxSearchDisplay> _sdf = new List<NxSearchDisplay>();
            if (searchType == NxSearchType.body)
                _sdf.Add(NxSearchDisplay.bodyName);
            foreach (string _sd in searchDisplay)
            {
                NxSearchDisplay _ssp = NxSearchDisplay.Unexpected;
                Enum.TryParse(_sd, true, out _ssp);
                if (_ssp != NxSearchDisplay.Unexpected && !_sdf.Contains(_ssp))
                    _sdf.Add(_ssp);
                if (_sdf.Count >= MAX_PARAMS_DISPLAY)
                    break;
            }
            m_searchDisplayFormated = _sdf.ToArray();
            generateDescription();
            m_formated = true;
        }
        private void generateDescription()
        {
            Description = string.Format("Search '{0}': {1}", searchName, searchType);
            foreach(NxSearchParam p in m_searchParamFormated.Keys)
            {
                switch (p)
                {
                    case NxSearchParam.name:
                        Description += " is called ";
                        break;
                    case NxSearchParam.allegiance:
                        Description += " from ";
                        break;
                    case NxSearchParam.government:
                        Description += " ruled by ";
                        break;
                    case NxSearchParam.state:
                        Description += " in ";
                        break;
                    case NxSearchParam.economy:
                        Description += " living for ";
                        break;
                    case NxSearchParam.rawMaterial:
                        Description += " with ";
                        break;
                }
                for(int i = 0; i < m_searchParamFormated[p].Length; i++)
                {
                    if( p == NxSearchParam.isLandable && m_searchParamFormated[p][i] == "true" )
                    {
                        Description += " landable";
                        continue;
                    }
                    bool last = i == m_searchParamFormated[p].Length - 1;
                    Description += m_searchParamFormated[p][i];
                    if (!last)
                    {
                        if (p != NxSearchParam.rawMaterial)
                            Description += " or ";
                        else
                            Description += " and ";
                    }
                }
                switch (p)
                {
                    case NxSearchParam.reserve:
                        Description += " reserve";
                        break;
                    case NxSearchParam.security:
                        Description += " security";
                        break;
                    case NxSearchParam.threat:
                        Description += " threat";
                        break;
                }
            }
        }
    }


    public enum NxSearchType
    {
        none,
        system,
        body,
        station
    }
    public enum NxSearchParam
    {
        //ignored/always visible for display config
        Unexpected,
        name,
        //Configurable
        nameNotes,
        allegiance,
        government,
        state,
        economy,
        reserve,
        security,
        threat,

        //BODYS
        rawMaterial,
        isLandable
    }
    public enum NxSearchDisplay
    {
        Unexpected = 0,
        nameNotes,
        allegiance,
        government,
        state,
        economy,
        secondEconomy,
        reserve,
        security,
        population,
        //Bodys
        bodyName, //auto
        materials,
        distanceToArrival
    }
}