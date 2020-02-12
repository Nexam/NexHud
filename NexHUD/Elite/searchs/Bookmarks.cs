using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NexHUD.Apis.Spansh;
using NexHUDCore;

namespace NexHUD.Elite.Searchs
{
    public class Bookmarks
    {
        public class SearchSystemList
        {
            public string searchName;
            public string[] systemNames;
        }
        public static List<CustomSearch> Searchs = new List<CustomSearch>();
        public static string BookmarkPath = Environment.CurrentDirectory + "\\Config\\Bookmarks.json";
        public static void Load()
        {
            Searchs = new List<CustomSearch>();
            LoadLists();

            //Load bookmarks
            NexHudEngine.Log("load Bookmarks.json");
            if (File.Exists(BookmarkPath))
            {
                string _json = File.ReadAllText(BookmarkPath);
                CustomSearch[] _list = JsonConvert.DeserializeObject<CustomSearch[]>(_json);
                Searchs.AddRange(_list);
            }
        }
        private static void LoadLists()
        {
            NxLog.log(NxLog.Type.Info, "load SearchSystemList.json");
            string path = Environment.CurrentDirectory + "\\Config\\SearchSystemList.json";
            if (File.Exists(path))
            {
                string _json = File.ReadAllText(path);
                SearchSystemList[] _list = JsonConvert.DeserializeObject<SearchSystemList[]>(_json);
                foreach (SearchSystemList s in _list)
                {
                    CustomSearch custom = new CustomSearch()
                    {
                        SearchName = s.searchName,
                        SystemsNotes = new Dictionary<string, string>(),
                        SearchSystem = new SpanshSearchSystems()
                        {
                            filters = new SpanshFilterSystems()                            
                        }
                    };
                    List<string> names = new List<string>();
                    for (int i = 0; i < s.systemNames.Length; i += 2)
                    {
                        names.Add(s.systemNames[i]);
                        custom.SystemsNotes.Add(s.systemNames[i], s.systemNames[i + 1]);
                    }
                    custom.SearchSystem.filters.name = new SpanshValue<string[]>(names.ToArray());
                    custom.Serializable = false;
                    Searchs.Add(custom);
                };
                
            }
        }
        public static void Save(CustomSearch search)
        {
            Searchs.Add(search);
            SaveAll();
        }
        public static void SaveAll()
        {
            using (StreamWriter file = File.CreateText(BookmarkPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Searchs.Where(x => x.Serializable ).ToArray() );
            }
        }
    }
}

