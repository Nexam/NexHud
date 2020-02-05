using NexHUD.Elite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NexHUD.Apis.Spansh
{
    public class SearchEngine
    {
        #region singleton
        public static SearchEngine Instance { get { return Nested.instance; } }
        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly SearchEngine instance = new SearchEngine();
        }
        #endregion

        //Memory
        private Dictionary<string, UserSearchResult> m_memory = new Dictionary<string, UserSearchResult>();

        public async Task SearchInBodies(SpanshSearch _search, Action<SpanshBodiesResult> _method)
        {
            Task<SpanshBodiesResult> t = new Task<SpanshBodiesResult>( () => ApiConnection.SpanshBodies("Sol",50,new string[] {"Arsenic" }, true));
            t.Start();
            //t.ContinueWith(() => _method(t.Result));
           // t.Wait();
           
        }
    }
}
