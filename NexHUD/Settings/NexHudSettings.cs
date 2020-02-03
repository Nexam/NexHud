using System;
using System.IO;
using Newtonsoft.Json;
using NexHUDCore;

namespace NexHUD.settings
{
    public class NexHudSettings
    {
        private static string m_path = "\\NexHudSettings.json";

        public bool launchWithElite = true;
        public NexHudEngineMode nexHudMode = NexHudEngineMode.Auto;
        public bool useCustomShortcutClassic = false;
        public bool stealFocus = true;

        #region singleton
        public static NexHudSettings GetInstance() { return m_instance; }

        private static NexHudSettings m_instance = new NexHudSettings();
        #endregion
        public static void load()
        {
            string _envPath = Environment.CurrentDirectory + m_path;
            //NexHudEngine.Log("Loadiing: "+_envPath);
            if (File.Exists(_envPath))
            {
                string _json = File.ReadAllText(_envPath);
                m_instance = JsonConvert.DeserializeObject<NexHudSettings>(_json);
            }
            if (m_instance == null)
                m_instance = new NexHudSettings();
        }
        public static void save()
        {
            string _envPath = Environment.CurrentDirectory + m_path;
            using (StreamWriter file = File.CreateText(_envPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, m_instance);
            }
        }
    }
}
