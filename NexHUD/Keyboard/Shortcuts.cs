using Newtonsoft.Json;
using NexHUDCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace NexHUD
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

        private static Dictionary<string, ShortcutEntry> m_entrys = new Dictionary<string, ShortcutEntry>();

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

        public static bool loadShortcuts()
        {
            string _path = Environment.CurrentDirectory + "\\Config\\Shortcuts.json";
            if( NexHudEngine.engineMode == NexHudEngineMode.WindowDebug )
                _path = Environment.CurrentDirectory + "\\Config\\Shortcuts-debug.json";
           

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
                            m_entrys.Add(_array[i].id, _array[i]);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    NexHudEngine.Log("ERROR while reading Shortcuts.json");
                    NexHudEngine.Log(ex.Message);
                    Console.WriteLine("Type any key to exist...");
                    Console.ReadKey();
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
    }
}
