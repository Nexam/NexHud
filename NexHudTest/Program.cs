using NexHUD.Apis.Spansh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NexHudTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Nex Hud Test...");

            Console.WriteLine("Search in bodies...");
            Console.WriteLine("_____________");
            SearchEngine.Instance.SearchInBodies(new SpanshSearchBodies(), _onBodiesFounded, _onFailedSearch);
            Thread.Sleep(500);
            SearchEngine.Instance.SearchInBodies(new SpanshSearchBodies(), _onBodiesFounded, _onFailedSearch);
            Console.WriteLine("-------------");

            Console.WriteLine("==> Type escape to exit");
            while (true)
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
        }

        private static void _onFailedSearch(SearchEngine.SearchError error)
        {
            Console.WriteLine("-> failed: " + error);
        }

        private static void _onBodiesFounded(SpanshBodiesResult obj)
        {
            Console.WriteLine("-> _onBodiesFounded: {0}", obj?.search_reference);
        }
    }
}
