using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NexHUD.Apis.ApiConnection;

namespace NexHUD.Apis.Spansh
{
    public class SpanshSearch
    {
        public SpanshFilderBodies filters;
        public int Page;
        //public SpanshCoords ReferenceCoords;
        public string reference_system;
        public int size;
        public SpanshSort[] sort;
    }
}
