using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Apis.Spansh
{
    public class SpanshDatas
    {
        public static readonly string[] allegiance = new string[] { "Alliance", "Empire", "Federation", "Independent", "Pilots Federation" };
        public static readonly string[] economy = new string[] { "Agriculture", "Colony", "Extraction", "High Tech", "Industrial", "Military", "Refinery", "Service","Terraforming","Tourism" };

        public static readonly string[] government = new string[] {"Anarchy","Communism","Confederacy","Cooperative","Corporate","Democracy","Dictatorship","Feudal","Patronage","Prison","Prison colony","Theocracy"};


        public static readonly string[] security = new string[] { "Anarchy", "High", "Low", "Medium" };

        public static readonly string[] state = new string[] { "Blight", "Boom", "Bust", "Civil liberty", "Civil unrest", "Civil war", "Drought", "Election", "Expansion", "Famine", "Infrastructure Failure", "Investment", "Lockdown", "Natural Disaster", "None", "Outbreak", "Pirate attack", "Public Holiday", "Retreat", "Terrorist Attack", "War"};

        public static readonly string[] power = new string[] { "A. Lavigny-Duval", "Aisling Duval", "Archon Delaine", "Denton Patreus", "Edmund Mahon", "Felicia Winters", "Li Yong-Rui", "Pranav Antal", "Yuri Grom", "Zachary Hudson", "Zemina Torval" };

        public static readonly string[] power_state = new string[] { "Contested", "Controlled", "Exploited", "HomeSystem", "InPrepareRadius", "Prepared", "Turmoil" };
    }
}
