using Newtonsoft.Json;
using NexHUD.Settings;
using NexHUDCore;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace NexHUD.Inputs
{
    public enum ShortcutId
    {
        left,
        right,
        up,
        down,
        select,
        menu,
        back
    }
    public class Shortcuts
    {
        public const string autoPath = "\\Config\\Shortcuts.json";
        public const string classicPath = "\\Config\\Shortcuts-classic.json";
        public const string debugPath = "\\Config\\Shortcuts-debug.json";

        private static Dictionary<string, ShortcutEntry> m_entrys = new Dictionary<string, ShortcutEntry>();

        public static ShortcutEntry[] getShortcuts()
        {
            ShortcutEntry[] _array = new ShortcutEntry[m_entrys.Values.Count];
            m_entrys.Values.CopyTo(_array, 0);
            return _array;
        }
        public static bool holdMode
        {
            get
            {
                ShortcutEntry _menu = get(ShortcutId.menu);
                if (_menu != null)
                    return _menu.holdMode;
                return false;
            }
        }
        public static ShortcutEntry get(ShortcutId _id)
        {
            ShortcutEntry e = null;
            m_entrys.TryGetValue(_id.ToString(), out e);
            return e;
        }

        public static bool loadShortcuts(NexHudEngineMode _mode, bool _useClassic)
        {
            string _path = Environment.CurrentDirectory + autoPath;
            if (_mode == NexHudEngineMode.WindowDebug)
                _path = Environment.CurrentDirectory + debugPath;
            else if (_mode == NexHudEngineMode.WindowOverlay && _useClassic)
                _path = Environment.CurrentDirectory + classicPath;


            if (File.Exists(_path))
            {
                try
                {
                    ShortcutEntry[] _array;
                    string _json = File.ReadAllText(_path);
                    _array = JsonConvert.DeserializeObject<ShortcutEntry[]>(_json);
                    for (int i = 0; i < _array.Length; i++)
                    {
                        if (!_array[i].compile())
                            throw new Exception(string.Format("ERROR: shortcut {0} is invalid", _array[i].id));
                        else
                        {
                            if (m_entrys.ContainsKey(_array[i].id))
                                m_entrys[_array[i].id] = _array[i];
                            else
                                m_entrys.Add(_array[i].id, _array[i]);
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    NexHudEngine.Log("ERROR while reading Shortcuts.json");
                    NexHudEngine.Log(ex.Message);
                    //Console.WriteLine("Type any key to exist...");
                    // Console.ReadKey();
                    return false;
                }
            }
            else
            {
                NexHudEngine.Log("ERROR: {0} is missing !", _path);
                Console.WriteLine("Type any key to exist...");
                Console.ReadKey();
                return false;
            }
        }

        public static void setShortcut(ShortcutId _id, Key[] _modifiers, Key? _key)
        {
            if (m_entrys.ContainsKey(_id.ToString()))
            {
                m_entrys[_id.ToString()].key = _key?.ToString();
                m_entrys[_id.ToString()].OpentTkKey = _key != null ? (Key)_key : Key.Unknown;

                string[] _mods = new string[_modifiers.Length];
                for (int i = 0; i < _modifiers.Length; i++)
                    _mods[i] = _modifiers[i].ToString();

                m_entrys[_id.ToString()].modifiers = _mods;
                m_entrys[_id.ToString()].OpenTkModifiers = _modifiers;
            }
        }

        public static void setMenuMode(bool hold)
        {
            if (m_entrys.ContainsKey(ShortcutId.menu.ToString()))
            {
                m_entrys[ShortcutId.menu.ToString()].holdMode = hold;
                m_entrys[ShortcutId.menu.ToString()].menuMode = hold ? "hold" : "press";
            }
        }

        public static void saveShortcuts(bool _classicMode = false)
        {
            ShortcutEntry[] _entrys = new ShortcutEntry[m_entrys.Count];

            m_entrys.Values.CopyTo(_entrys, 0);

            string _path = Environment.CurrentDirectory + autoPath;
            if (_classicMode)
                _path = Environment.CurrentDirectory + classicPath;

            using (StreamWriter file = File.CreateText(_path))
            {
                JsonSerializer serializer = new JsonSerializer() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore };
                serializer.Serialize(file, _entrys);
            }
        }

        /*  bool up = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.up));
            bool down = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.down));
            bool left = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.left));
            bool right = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.right));
            bool select = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select));
*/
        public static bool UpPressed { get { return NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.up)); } }
        public static bool DownPressed { get { return NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.down)); } }
        public static bool LeftPressed { get { return NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.left)); } }
        public static bool RightPressed { get { return NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.right)); } }
        public static bool SelectPressed { get { return NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select)); } }
        public static bool BackPressed
        {
            get
            {
                if (holdMode)
                    return NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.back));
                else
                    return NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.back)) || NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.menu));
            }
        }
        public static bool MenuPressed { get { return NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.menu)); } }

    }
}
