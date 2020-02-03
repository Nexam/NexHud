using Newtonsoft.Json;
using OpenTK.Input;
using System;

namespace NexHUDCore
{
    public class ShortcutEntry
    {
        public string id;       
        public string menuMode;
        public string key;
        public string[] modifiers;

        [JsonIgnore]
        public Key OpentTkKey;

        [JsonIgnore]
        public Key[] OpenTkModifiers;

        [JsonIgnore]
        public bool holdMode = false;

        public bool compile()
        {
            if (!Enum.TryParse(key, true, out OpentTkKey))
            {
                if (id != "menu" || (id == "menu" && !string.IsNullOrEmpty(key)))
                    return false;
            }

            OpenTkModifiers = modifiers != null ? new Key[modifiers.Length] : new Key[0];
            for (int i = 0; i < OpenTkModifiers.Length; i++)
            {
                Key _m;
                if (Enum.TryParse(modifiers[i], true, out _m))
                    OpenTkModifiers[i] = _m;
                else
                    return false;

            }

            if (menuMode == "hold" && id == "menu")
                holdMode = true;

            return true;
        }
    }
}
