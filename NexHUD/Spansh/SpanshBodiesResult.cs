namespace NexHUD.Spansh
{
    public class SpanshBodiesResult
    {
        public int count;
        public int from;
        public SpanshBody[] results;
        public string search_reference;
    }
    public class SpanshBody
    {
        public double? arg_of_periapsis;
        public string atmosphere;
        public double? axial_tilt;
        public int? body_id;
        public double? distance;
        public int? distance_to_arrival;
        public double? earth_masses;
        public int? edsm_id;
        public int? estimated_mapping_value;
        public int? estimated_scan_value;
        public double? gravity;
        public bool? is_landable;
        public SpanshMaterial[] materials;
        public string name;
        public SpanshMaterial[] solid_composition;
        public string subtype;
        public string system_name;
        public string terraforming_state;
        public string type;
        public string volcanism_type;
        public long? system_id64;

    }
    public class SpanshMaterial
    {
        public string name;
        public double? share;
    }
}
