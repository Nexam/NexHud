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
        public spanshFilter Filter;
        public int Page;
        //public SpanshCoords ReferenceCoords;
        public int Size;
        public spanshSort Sort;
    }
}
