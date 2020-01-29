using NexHUD.EDDB;
using NexHUD.EDSM;
using NexHUD.Spansh;
using System;
using System.Collections.Generic;

namespace NexHUD.Elite
{
    public class EDSystem
    {
        public string name = "unset";
        public double x = 0;
        public double y = 0;
        public double z = 0;
        public long population = 0;
        public EliteGovernment government = EliteGovernment.Unknow;
        public EliteAllegiance allegiance = EliteAllegiance.Unknow;
        public EliteState state = EliteState.Unknow;
        public EliteSecurity security = EliteSecurity.Unknow;
        public EliteEconomy economy = EliteEconomy.Unknow;
        public EliteEconomy secondEconomy = EliteEconomy.Unknow;
        public string power = string.Empty;
        public string power_state = string.Empty;
        public bool needsPermit = false;
        public string controlling_minor_faction = "unknow";
        public EliteReserve reserve = EliteReserve.Unknow;

        //Star infos
        public string star_type = "unknow";
        public string star_name = "unknow";
        public bool star_isScoopable = false;

        //Value system
        public EDSMValuableBody[] valuableBodies = new EDSMValuableBody[0];
        public uint value = 0;
        public uint valueMapped = 0;
        //traffic
        public int traffic_total = 0;
        public int traffic_week = 0;
        public int traffic_day = 0;
        //deaths
        public int deaths_total = 0;
        public int deaths_week = 0;
        public int deaths_day = 0;

        //For search result
        public string Notes = string.Empty;

        private double m_distanceFromCurrentSystem = -1;
        public double distanceFromCurrentSystem { get { return m_distanceFromCurrentSystem; } }


        private Dictionary<int, EDBody> m_Bodys = new Dictionary<int, EDBody>();


        public void calculDistanceFromCurrent()
        {
            EDSystem _oSys = EDDatas.Instance.getCurrentSystem();
            OpenTK.Vector3d o = new OpenTK.Vector3d(_oSys.x, _oSys.y, _oSys.z);
            OpenTK.Vector3d d = new OpenTK.Vector3d(x, y, z);
            m_distanceFromCurrentSystem = Math.Round(OpenTK.Vector3d.Distance(o, d), 2);

        }

        //Other


        private bool m_receivedEddbInfos = false;
        public bool receivedEddbInfos { get { return m_receivedEddbInfos; } }

        private bool m_receivedEdsmBasics = false;
        public bool receivedEdsmBasics { get { return m_receivedEdsmBasics; } }

        private bool m_receivedEdsmPermit = false;
        public bool receivedEdsmPermit { get { return m_receivedEdsmPermit; } }

        private bool m_receivedEdsmInfos = false;
        public bool receivedEdsmInfos { get { return m_receivedEdsmInfos; } }

        private bool m_receivedEdsmPrimaryStar = false;
        public bool receivedEdsmPrimaryStar { get { return m_receivedEdsmPrimaryStar; } }

        private bool m_receivedEdsmEstValue = false;
        public bool receivedEdsmEstValue { get { return m_receivedEdsmEstValue; } }


        public bool isEDSMComplete()
        {
            return m_receivedEdsmBasics && m_receivedEdsmInfos && m_receivedEdsmPermit && m_receivedEdsmPrimaryStar && m_receivedEdsmEstValue;
        }

        //public string populationString { get { return String.Format("{0:#,##0}", population); } }

        public EliteSystemThreat Threat
        {
            get
            {
                if (deaths_week > 500 || deaths_day > 50)
                    return EliteSystemThreat.High;
                else if (deaths_week > 100 || deaths_day > 10)
                    return EliteSystemThreat.Medium;
                else
                    return EliteSystemThreat.Low;

            }
        }

        public EDBody addOrUpdateBody(SpanshBody _spanshDatas)
        {
            if (_spanshDatas.edsm_id == null)
                throw new Exception("Missing edsm id for body " + _spanshDatas.name);
            if (!m_Bodys.ContainsKey((int)_spanshDatas.edsm_id))
                m_Bodys.Add((int)_spanshDatas.edsm_id, new EDBody(this));

            m_Bodys[(int)_spanshDatas.edsm_id].update(_spanshDatas);

            return m_Bodys[(int)_spanshDatas.edsm_id];
        }

        public void updateEDDB(EDDBSystemDatas _datas)
        {
            if (_datas == null)
                return;
            power = _datas.power;
            power_state = _datas.power_state;

            m_receivedEddbInfos = true;

            //TODO: Presence
        }

        public void updateEDSM(EDSMSystemDatas _datas)
        {
            if (_datas == null)
                return;
            name = _datas.name;
            if (_datas.coords != null)
            {
                x = _datas.coords.x;
                y = _datas.coords.y;
                z = _datas.coords.z;
            }

            if (_datas.distance != null)
                m_distanceFromCurrentSystem = (double)_datas.distance;

            m_receivedEdsmBasics = true;

            if (_datas.requirePermit != null)
            {
                needsPermit = (bool)_datas.requirePermit;
                m_receivedEdsmPermit = true;
            }

            if (_datas.information != null)
            {
                if (_datas.information.allegiance != null)
                    Enum.TryParse(_datas.information.allegiance.Replace(" ", string.Empty), true, out allegiance);
                if (_datas.information.government != null)
                    Enum.TryParse(_datas.information.government.Replace(" ", string.Empty), true, out government);
                controlling_minor_faction = _datas.information.faction;
                if (_datas.information.factionState != null)
                    Enum.TryParse(_datas.information.factionState.Replace(" ", string.Empty), true, out state);
                if (_datas.information.population != null)
                    population = (long)_datas.information.population;

                if (_datas.information.security != null)
                    Enum.TryParse(_datas.information.security.Replace(" ", string.Empty), out security);
                if (_datas.information.economy != null)
                    Enum.TryParse(_datas.information.economy.Replace(" ", string.Empty), true, out economy);
                if (_datas.information.secondEconomy != null)
                    Enum.TryParse(_datas.information.secondEconomy.Replace(" ", string.Empty), true, out secondEconomy);
                if (_datas.information.reserve != null)
                    Enum.TryParse(_datas.information.reserve.Replace(" ", string.Empty), true, out reserve);

                m_receivedEdsmInfos = true;
            }
            if (_datas.primaryStar != null)
            {
                if (_datas.primaryStar.name.Length > 0)
                    star_name = _datas.primaryStar.name;
                if (_datas.primaryStar.type.Length > 0)
                    star_type = _datas.primaryStar.type;
                if (_datas.primaryStar.isScoopable != null)
                    star_isScoopable = (bool)_datas.primaryStar.isScoopable;

                m_receivedEdsmPrimaryStar = true;
            }

            if (_datas.valuableBodies != null)
            {
                valuableBodies = _datas.valuableBodies;
                if (_datas.valuableBodies != null)
                    value = (uint)_datas.estimatedValue;
                if (_datas.estimatedValueMapped != null)
                    valueMapped = (uint)_datas.estimatedValueMapped;
                m_receivedEdsmEstValue = true;
            }
        }
    }
}
