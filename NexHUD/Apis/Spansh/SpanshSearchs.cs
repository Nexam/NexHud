using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NexHUD.Apis.ApiConnection;
using Newtonsoft.Json;

namespace NexHUD.Apis.Spansh
{
    public class SpanshSearchBodies
    {
        public SpanshFilterBodies filters;
        public int Page;
        //public SpanshCoords ReferenceCoords;
        public string reference_system;

        [JsonIgnore]
        public int size = 20;

        public SpanshSort[] sort;
    }
    public class SpanshSearchSystems
    {
        public int Page;
        public SpanshFilterSystems filters;
        //public SpanshCoords ReferenceCoords;
        public string reference_system;

        [JsonIgnore]
        public int size = 20;

        public SpanshSort[] sort;
    }
}
