using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Apis.Spansh
{
    public class SpanshSystem
    {
        public string allegiance;
        public SpanshBody[] bodies;
        public string controlling_minor_faction;
        public double distance;
        public int edsm_id;
        public string government;
        public string id;
        public long id64;
        public SpanshMinorFaction[] minor_faction_presences;
        public string name;
        public bool needs_permit;
        public long population;
        public string[] power;
        public string power_state;
        public string primary_economy;
        public string secondary_economy;
        public string security;
        public double x;
        public double y;
        public double z;

        [Newtonsoft.Json.JsonIgnore]
        public string state
        {
            get
            {
                if (minor_faction_presences == null)
                    return string.Empty;
                minor_faction_presences = minor_faction_presences.OrderByDescending(x => x.influence).ToArray();
                List<string> states = new List<string>();
                for(int i = 0; i < minor_faction_presences.Length; i++)
                {
                    SpanshMinorFaction faction = minor_faction_presences[i];
                    if (faction.state != "None" && !states.Contains(faction.state))
                    {
                        states.Add(faction.state);
                    }
                }

                string stateReport = "None";
                foreach(string s in states)
                {
                    if (s == states.First())
                        stateReport = s + (states.Count > 1 ? ", ": "");
                    else if (s != states.Last())
                        stateReport += s + ", ";
                    else
                        stateReport += s;
                }

                return stateReport;
            }
        }
    }
    public class SpanshMinorFaction
    {
        public double influence;
        public string name;
        public string state;
    }
}
