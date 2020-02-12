using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Apis.Spansh
{
    public class SpanshFilterBodies
    {
        public SpanshValue<string[]> name;
        public SpanshValue<string[]> atmosphere;
        public SpanshValue<int> distance_from_coords;
        public SpanshValue<int> distance_to_arrival;
        public SpanshValue<int> estimated_scan_value;
        public SpanshValue<int> estimated_mapping_value;
        public SpanshValue<double> gravity;

        public SpanshValue<double>[] materials;
        public SpanshValue<bool> is_landable;
        // public intValue distance;
    }
    public class SpanshFilterSystems
    {
        public SpanshValue<string[]> name;
        public SpanshValue<int?> distance_from_coords;
        public SpanshValue<string[]> allegiance;
        public SpanshValue<string[]> primary_economy;
        public SpanshValue<string[]> government;
        public SpanshValue<string[]> power;
        public SpanshValue<string[]> power_state;
        public SpanshValue<string[]> secondary_economy;
        public SpanshValue<string> minor_faction_presence;
        public SpanshValue<string[]> security;
        public SpanshValue<string[]> state;


        public SpanshValue<long?> population;
        public SpanshValue<bool?> needs_permit;

    }
}
