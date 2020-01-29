using NAudio.Wave;
using NexHUD.Spansh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NexHUD
{
    /// <summary>
    /// Class to test differents packages, api, etc...
    /// </summary>
    public class NxTester
    {
        public static void AudioTester()
        {
            var url = "http://radiosidewinder.out.airtime.pro:8000/radiosidewinder_b";
            url = "http://bluford.torontocast.com:8421/stream";// lave radio
            url = "http://bluford.torontocast.com:8447/hq"; //hutton orbital
            using (var mf = new MediaFoundationReader(url))
            using (var wo = new WaveOutEvent())
            {
                wo.Init(mf);
                wo.Play();
                wo.Volume = 0.5f;

                //while (wo.PlaybackState == PlaybackState.Playing)
                {
                    Console.WriteLine("Playing " + url);
                    Console.WriteLine("Press any Escape to exit");
                    while (true)
                        if (Console.ReadKey().Key == ConsoleKey.Escape)
                            break;
                    //break;
                }
            }
        }

        public static void BodyTester()
        {
            string _currentSystem = "Nemehi";


            Stopwatch _watch = new Stopwatch();
            _watch.Start();


            //Random search
            string[] _mats = new string[] { "Phosphorus" };
            // Random r = new Random();
            // m = (EliteMaterial)r.Next(Enum.GetNames(typeof(EliteMaterial)).Length);

            Console.Write("Get best match around " + _currentSystem + " for: ");
            for (int j = 0; j < _mats.Length; j++)
            {
                Console.Write(_mats[j] + (j < _mats.Length - 1 ? "" : ","));
            }
            Console.WriteLine();

            int _distance = 100;
            int _step = 100;
            int _totalCount = 0;
            while (_distance <= 100)
            {
                Console.WriteLine("search from {0} to {1}", _distance - _step, _distance);
                SpanshBodiesResult _response = ExternalDBConnection.SpanshBodies(_currentSystem, 100, _mats, true);

                Console.WriteLine(_response);

                if (_response != null)
                {

                    for (int i = 0; i < _response.results.Length; i++)
                    {
                        Console.Write("- {0} (Dist:{1}. DistTA:{2}", _response.results[i].name, Math.Round((double)_response.results[i].distance, 1), _response.results[i].distance_to_arrival);
                        for (int j = 0; j < _mats.Length; j++)
                        {
                            Console.Write("| {0}: {1}%",
                                _mats[j],
                                _response.results[i].materials.Where(x => x.name == _mats[j].ToString()).FirstOrDefault().share,
                                _response.results[i].distance,
                                _response.results[i].distance_to_arrival
                                );

                        }
                        Console.WriteLine();
                    }
                    _totalCount += _response.results.Length;
                }

                if (_totalCount < 11)
                {
                    _distance += _step;
                    Console.WriteLine("Extending the search...");
                }
                else
                    break;
            }

            _watch.Stop();
            Console.WriteLine("Search finisehd in {0}ms", _watch.ElapsedMilliseconds);
            Console.WriteLine("Type any key to exit");
            Console.ReadKey();
        }
    }
}
