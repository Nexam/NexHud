using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Apis.Spansh.Datas
{
    class SpanshStation
    {
        public string allegiance;
        public long body_id64;
        public string body_name;
        public string controlling_minor_faction;

        public double distance;
        public double distance_to_arrival;

        public string government;
        public bool has_large_pad;
        public bool has_market;
        public string id;
        public bool is_planetary;
        public int market_id;
        public string market_updated_at;
        public string power_state;
        public string primary_economy;
        public long system_id64;
        public string system_name;
        public string[] system_power;
        public double system_x;
        public double system_y;
        public double system_z;
        public string type;
        public string updated_at;

        public SpanshStationMarket[] market;
        public SPanshStationModule[] modules;
        
    }
    public class SpanshStationMarket
    {
        public int buy_price;
        public string category;
        public string commodity;
        public int demand;
        public int sell_price;
        public int supply;
    }
    public class SPanshStationModule
    {
        public string category;
        public int module_class; //class
        public string ed_symbol;
        public string name;
        public string rating;
        public string ship;
    }
}
