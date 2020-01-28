using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace NexHUDCore
{
    public class ShortcutEntry
    {
        public string id;
        public string menuMode;
        public string key;
        public string[] modifiers;

        public Key OpentTkKey;
        public Key[] OpenTkModifiers;

        public bool holdMode = false;

        public bool compile()
        {
            if (!Enum.TryParse(key, true, out OpentTkKey))
            {
                if( id != "menu" ||( id == "menu" && !string.IsNullOrEmpty(key) ))
                    return false;
            }

            OpenTkModifiers = new Key[modifiers.Length];
            for (int i = 0; i < modifiers.Length; i++)
            {
                Key _m;
                if (Enum.TryParse(modifiers[i], true, out _m))
                    OpenTkModifiers[i] = _m;
                else
                    return false;

            }

            if (menuMode == "hold" && id == "menu" )
                holdMode = true;

            return true;
        }
    }
}
