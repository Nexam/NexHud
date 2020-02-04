using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Apis.Spansh
{
    public class SpanshDatas
    {
        public readonly string[] government = new string[] {"Anarchy","Communism","Confederacy","Cooperative","Corporate","Democracy","Dictatorship","Feudal","Patronage","Prison","Prison colony","Theocracy"};

        public readonly string[] allegiance = new string[] {"Alliance", "Empire", "Federation", "Independent", "Pilots Federation" };

        public readonly string[] security = new string[] { "Anarchy", "High", "Low", "Medium" };

        public readonly string[] state = new string[] { "Blight", "Boom", "Bust", "Civil liberty", "Civil unrest", "Civil war", "Drought", "Election", "Expansion", "Famine", "Infrastructure Failure", "Investment", "Lockdown", "Natural Disaster", "None", "Outbreak", "Pirate attack", "Public Holiday", "Retreat", "Terrorist Attack", "War"};
    }
}
