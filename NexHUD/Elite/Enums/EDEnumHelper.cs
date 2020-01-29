using System.Linq;
using System.Text;

namespace System
{
    public static class EDEnumHelper
    {
        public static string stringToEnumName(string _string)
        {
            for (int i = 1; i < _string.Length; i++)
            {
                if (_string[i] == ' ' && i < _string.Length - 1)
                {
                    _string = _string.Substring(0, i) + Char.ToUpper(_string[i + 1]) + _string.Substring(i + 2, (_string.Length - 1) - (i + 1));
                    i--;
                }
            }
            _string = _string.Replace(" ", "");
            _string = _string.Replace("-", "__");
            _string = _string.Replace("(", "_ob_");
            _string = _string.Replace(")", "_cb_");
            _string = _string.Replace("+", "_p_");
            _string = _string.Replace("%", "_pc_");
            _string = _string.Replace(",", "_c_");

            if (new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }.Contains(_string[0]))
                _string = _string.Insert(0, "___");

            return _string;
        }
        /// <summary>
        /// Return a spaced version of the enum
        /// ex PrisonColony will return Prison colony
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="_enum">the enum value</param>
        /// <returns></returns>
        public static string toStringOperation(string _enum)
        {
            string _enumStr = _enum.ToString();
            _enumStr = _enumStr.Replace("___", ""); //When the string start with a number
            _enumStr = _enumStr.Replace("_ob_", "("); //"ob = Open bracket"
            _enumStr = _enumStr.Replace("_cb_", ")"); //"cb = Close bracket"
            _enumStr = _enumStr.Replace("_p_", "+"); // p = plus
            _enumStr = _enumStr.Replace("_pc_", "%"); // pc = percent
            _enumStr = _enumStr.Replace("_c_", ","); //c = comma
            _enumStr = _enumStr.Replace("__", "-");


            char[] _IgnoreSpace = new char[] { '-', '_', 'è', ')', 'é', '(' };


            StringBuilder newText = new StringBuilder(_enumStr.Length * 2);
            newText.Append(_enumStr[0]);
            for (int i = 1; i < _enumStr.Length; i++)
            {
                if (_enumStr[i] == '(')
                {
                    newText.Append(' ');
                    continue;
                }
                if (i > 0 && _enumStr[i] == '+' && _enumStr[i - 1] == ',')
                {
                    newText.Append(' ');
                    continue;
                }
                if (char.IsUpper(_enumStr[i]))
                    if (_enumStr[i - 1] != ' ' && !_IgnoreSpace.Contains(_enumStr[i - 1]))
                    {
                        bool _prevIsUpper = char.IsUpper(_enumStr[i - 1]);
                        bool _nextIsUpper = i < _enumStr.Length - 1 ? char.IsUpper(_enumStr[i + 1]) : false;

                        bool _insertSpace = false;
                        if (!_prevIsUpper)
                            _insertSpace = true;
                        if (_prevIsUpper && _nextIsUpper)
                            _insertSpace = false;
                        if (_prevIsUpper && !_nextIsUpper && i < _enumStr.Length - 1)
                            _insertSpace = true;


                        if (_insertSpace)
                            newText.Append(' ');
                    }
                newText.Append(_enumStr[i]);
            }
            //string _result = newText.ToString().ToLower();
            return newText.ToString().FirstCharToUpper();
        }

        public static string toString<Enum>(Enum _enum)
        {
            return toStringOperation(_enum.ToString());
        }

        /// <summary>
        /// Return a spaced version of the enum
        /// ex PrisonColony will return Prison colony
        /// </summary>
        public static string ToStringFormated(this Enum input)
        {
            return toString(input);
        }
    }
}
