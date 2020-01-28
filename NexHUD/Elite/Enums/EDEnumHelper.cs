using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class EDEnumHelper
    {
        /// <summary>
        /// Return a spaced version of the enum
        /// ex PrisonColony will return Prison colony
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="_enum">the enum value</param>
        /// <returns></returns>
        public static string toString<Enum>(Enum _enum)
        {
            StringBuilder newText = new StringBuilder(_enum.ToString().Length * 2);
            newText.Append(_enum.ToString()[0]);
            for (int i = 1; i < _enum.ToString().Length; i++)
            {
                if (char.IsUpper(_enum.ToString()[i]))
                    if ((_enum.ToString()[i - 1] != ' ' && !char.IsUpper(_enum.ToString()[i - 1])))
                        newText.Append(' ');
                newText.Append(_enum.ToString()[i]);
            }
            string _result = newText.ToString().ToLower();
            return _result.FirstCharToUpper();
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
