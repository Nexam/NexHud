using System;
using System.IO;
using System.Net;
using NexHUDCore;

namespace NexHUD.Apis.Spansh
{
    public class SpanshApi
    {
        public const string Url_SpanshBodies = @"https://spansh.co.uk/api/bodies/search?";
        public const string Url_SpanshSystems = @"https://spansh.co.uk/api/systems/search?";

        public static object SearchInSystems(SpanshSearchBodies _search)
        {
            return null;
           /* _maxDistance = Math.Min(_maxDistance, 100);

            Dictionary<string, string> _sParams = new Dictionary<string, string>();

            spanshValue<double?>[] _spanshMats = new spanshValue<double?>[_materials.Length];
            for (int i = 0; i < _materials.Length; i++)
            {
                _spanshMats[i] = new spanshValue<double?>(_materials[i], ">", 0);
            }
            spanshSortValue[] _spanshMatsSort = new spanshSortValue[_materials.Length];
            for (int i = 0; i < _materials.Length; i++)
            {
                _spanshMatsSort[i] = new spanshSortValue(_materials[i].ToString(), false);
            }

            spanshParam p = new spanshParam()
            {
                size = 11,
                reference_system = _systemOrigin,
                filters = new spanshFilter()
                {
                    distance_from_coords = new spanshValue<int?>() { max = _maxDistance },
                    materials = _spanshMats,
                    is_landable = new spanshValue<bool?>(_isLandable)
                },
                sort = new spanshSort[] { new spanshSort() {
                    materials = _spanshMatsSort,
                },new spanshSort() {
                    distance_from_coords = new spanshSortValue(true)
                } }
            };
            string _spanshJson = JsonConvert.SerializeObject(p, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }); ;

            Console.WriteLine(_spanshJson);
            string json = requestPOSTFromURL(Url_SpanshBodies, _spanshJson);
            try
            {
                SpanshBodiesResult result = JsonConvert.DeserializeObject<SpanshBodiesResult>(json);
                Console.WriteLine("Result: {0} RealCount: {1}", result.count, result.results.Length);
                Console.WriteLine("Search ID: " + result.search_reference);

                return result;
            }
            catch (Exception ex)
            {
                NexHudEngine.Log(ex.Message);
                NexHudEngine.Log(json);
            }

            return null;*/
        }
        private static string RequestPOSTFromURL(string _url, string _json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(_json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
            catch (Exception ex)
            {
                NexHudEngine.Log(NxLog.Type.Error, ex.Message);
                NexHudEngine.Log(NxLog.Type.Error, ex.StackTrace);
                NexHudEngine.Log(NxLog.Type.Error, ex.Source);
            }
            return "Error in SpanshApi.RequestPOSTFromURL";
        }

    }
}
