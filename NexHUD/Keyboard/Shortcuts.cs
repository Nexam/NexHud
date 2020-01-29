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

            //write keys & modifiers
            /*
            StreamWriter writer = File.CreateText("\\Config\\keys.txt");
            writer.WriteLine("===== KEY LIST =====");
            string[] _keynames = Enum.GetNames(typeof(Key));
            foreach (string k in _keynames)
                writer.WriteLine(k + ",");

            writer.WriteLine("");
            writer.WriteLine("===== MODIFIERS LIST =====");
            string[] _modifiersNames = Enum.GetNames(typeof(KeyModifiers));
            foreach (string m in _modifiersNames)
                writer.WriteLine(m + ",");
            writer.Close();
            writer.Dispose();
            */

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
                    SteamVR_NexHUD.Log("ERROR while reading Shortcuts.json");
                    SteamVR_NexHUD.Log(ex.Message);
                    Console.WriteLine("Type any key to exist...");
                    Console.ReadKey();
                    return false;
                }
            }
            else
            {
                SteamVR_NexHUD.Log("ERROR: {0} is missing !", _path);
                Console.WriteLine("Type any key to exist...");
                Console.ReadKey();
                return false;
            }
        }
    }
}
