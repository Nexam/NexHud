using NexHUD.Apis.Spansh;
using System.Collections.Generic;

namespace NexHUD.Elite
{
    public class EDBody
    {
        private EDSystem m_system;
        public double arg_of_periapsis;
        public string atmosphere;
        public double axial_tilt;
        public int bodyId;
        public int distance_to_arrival;
        public double earth_masses;
        public int edsmId;
        public int estimated_mapping_value;
        public int estimated_scan_value;
        public double gravity;
        public bool isLandable;
        public KeyValuePair<string, double>[] materials;
        public string name;
        public KeyValuePair<string, double>[] solid_composition;
        public string subtype;
        public string terraformingState;
        public string type;
        public string volcanismType;

        public EDSystem system { get { return m_system; } }

        public EDBody(EDSystem _system)
        {
            m_system = _system;
        }

        public void update(SpanshBody _datas)
        {
            if (_datas.arg_of_periapsis != null)
                arg_of_periapsis = (double)_datas.arg_of_periapsis;
            atmosphere = _datas.atmosphere;
            if (_datas.axial_tilt != null)
                axial_tilt = (double)_datas.axial_tilt;
            if (_datas.body_id != null)
                bodyId = (int)_datas.body_id;
            if (_datas.distance_to_arrival != null)
                distance_to_arrival = (int)_datas.distance_to_arrival;
            if (_datas.earth_masses != null)
                earth_masses = (double)_datas.earth_masses;
            if (_datas.edsm_id != null)
                edsmId = (int)_datas.edsm_id;
            if (_datas.estimated_mapping_value != null)
                estimated_mapping_value = (int)_datas.estimated_mapping_value;
            if (_datas.estimated_scan_value != null)
                estimated_scan_value = (int)_datas.estimated_scan_value;
            if (_datas.gravity != null)
                gravity = (double)_datas.gravity;
            if (_datas.is_landable != null)
                isLandable = (bool)_datas.is_landable;
            name = _datas.name.Replace(m_system.name, "").Trim();
            subtype = _datas.subtype;
            type = _datas.type;
            terraformingState = _datas.terraforming_state;
            volcanismType = _datas.volcanism_type;

            materials = new KeyValuePair<string, double>[_datas.materials.Length];
            int i = 0;
            foreach (SpanshMaterial _md in _datas.materials)
                materials[i++] = new KeyValuePair<string, double>(_md.name, (double)_md.share);

            solid_composition = new KeyValuePair<string, double>[_datas.materials.Length];
            i = 0;
            foreach (SpanshMaterial _md in _datas.solid_composition)
                solid_composition[i++] = new KeyValuePair<string, double>(_md.name, (double)_md.share);
        }
    }
}
