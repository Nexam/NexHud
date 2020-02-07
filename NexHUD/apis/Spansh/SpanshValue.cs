using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Apis.Spansh
{
    public class SpanshValue<T>
    {
        public string name;
        public T value;
        public string comparison;
        public T min;
        public T max;
        public SpanshValue()
        {
        }
        public SpanshValue(T _min, T _max)
        {
            min = _min;
            max = _max;
        }
        public SpanshValue(T _v) : this(null, null, _v)
        {
        }
        public SpanshValue(string _comparison, T _v) : this(null, _comparison, _v)
        {
        }
        public SpanshValue(string _name, string _comparison, T _value)
        {
            name = _name;
            value = _value;
            comparison = _comparison;
        }
    }
}
