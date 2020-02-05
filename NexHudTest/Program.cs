using NexHUD.Apis.Spansh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            SearchEngine.Instance.SearchInBodies(new SpanshSearch(), _onBodiesFounded);
            Console.WriteLine("_____________");

            Console.WriteLine("==> Type escape to exit");
            while (true)
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
        }

        private static int _onBodiesFounded(SpanshBodiesResult obj)
        {
            Console.WriteLine("_onBodiesFounded: {0}", obj);
            return 1;
        }
    }
}
