using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Apis.Spansh
{
    public class SpanshSortValue
    {
        public const string ASC = "asc";
        public const string DESC = "desc";

        public string name;
        public string direction;

        public SpanshSortValue()
        {

        }
        public SpanshSortValue(bool _ascending)
        {
            direction = _ascending ? ASC : DESC;
        }
        public SpanshSortValue(string _name, bool _ascending)
        {
            name = _name;
            direction = _ascending ? ASC : DESC;
        }
    }
}
