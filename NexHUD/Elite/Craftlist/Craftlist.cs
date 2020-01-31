using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NexHUDCore;

namespace NexHUD.Elite.Craftlist
{
    public class Craftlist
    {
        private static List<CraftlistItem> m_list = new List<CraftlistItem>();
        private static string path = Environment.CurrentDirectory + "\\craftlist.json";

        public static List<CraftlistItem> list { get => m_list; }
        public static void load()
        {
            NexHudEngine.Log("load craftlist.json");
            if( File.Exists(path) )
            {
                string _json = File.ReadAllText(path);
                m_list = new List<CraftlistItem>();
                CraftlistItem[] _list = JsonConvert.DeserializeObject<CraftlistItem[]>(_json);
                m_list.AddRange(_list);
                foreach (CraftlistItem i in m_list)
                    i.compile();
            }
        }

        private static void save()
        {
            CraftlistItem[] _listArray = m_list.ToArray();
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, _listArray);
            }
        }

        public static void Add(CraftlistItem item)
        {
            m_list.Add(item);
            save();
        }
        public static void Delete(CraftlistItem item)
        {
            m_list.Remove(item);
            save();
        }
    }
}
