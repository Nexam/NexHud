using Newtonsoft.Json;
using NexHUDCore;
using System;
using System.IO;

namespace NexHUD.Elite
{
    public class UserSearchs
    {
        private static NxSearchEntry[] m_userSearchEntrys = new NxSearchEntry[0];
        public static NxSearchEntry[] userSearchEntrys { get { return m_userSearchEntrys; } }

        public static void readConfigFile()
        {
            string _path = Environment.CurrentDirectory + "\\Config\\Searchs.json";
            if (File.Exists(_path))
            {
                try
                {
                    string _json = File.ReadAllText(_path);
                    m_userSearchEntrys = JsonConvert.DeserializeObject<NxSearchEntry[]>(_json);
                    for (int i = 0; i < m_userSearchEntrys.Length; i++)
                        m_userSearchEntrys[i].format();
                }
                catch (Exception ex)
                {
                    SteamVR_NexHUD.Log("Error while reading Searchs.json");
                    SteamVR_NexHUD.Log(ex.Message);

                    m_userSearchEntrys = new NxSearchEntry[]
                    {
                        new NxSearchEntry(){searchName = "ERROR"},
                        new NxSearchEntry(){searchName = "in your"},
                        new NxSearchEntry(){searchName = "Searchs.json"}
                    };
                }
            }
        }
    }
}
