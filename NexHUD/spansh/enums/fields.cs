using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.spansh
{
    public class Field
    {
        public readonly string Name;
        public readonly object[] Values;

        public Field(string _name, object[] _values)
        {
            Name = _name;
            Values = _values;
        }

        public bool IsValidValue(object _value)
        {
            return Values.Contains(_value);
        }
    }
    public class SpanshFields
    {
        public const string Atmosphere = "atmosphere";

        private Dictionary<string, Field> m_SystemFields;

        public Field Get(string _name)
        {
            if (m_SystemFields == null)
                Init();

            return m_SystemFields.ContainsKey(_name) ? m_SystemFields[_name] : null;
        }
        private void Init()
        {
            m_SystemFields.Add(Atmosphere, new Field(Atmosphere, new string[] { }));
        }
    }
}
