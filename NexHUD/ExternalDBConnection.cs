﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NexHUD.EDDB;
using NexHUD.EDSM;
using NexHUD.Elite.Enums;
using NexHUD.Spansh;
using NexHUDCore;

namespace NexHUD
{
    public class ExternalDBConnection
    {
        /* https://elitebgs.app/api/eddb */


        /*Endpoints EDDB*/
        // Factions
        public const string url_EDDBFactions = @"https://eddbapi.kodeblox.com/api/v4/factions?";
        // Populated Systems
        public const string url_EDDBpopulatedSystems = @"https://eddbapi.kodeblox.com/api/v4/populatedsystems?";
        // Stations
        public const string url_EDDBStations = @"https://eddbapi.kodeblox.com/api/v4/stations?";
        // Systems
        public const string url_EDDBSystems = @"https://eddbapi.kodeblox.com/api/v4/systems?";
        /*Please note that the endpoints have been changed to their own subdomain.*/

        /*ENDPOINTS EDSM*/
        public const string url_EDSMSystem = @"https://www.edsm.net/api-v1/system?";
        public const string url_EDSMSystems = @"https://www.edsm.net/api-v1/systems?";
        public const string url_SystemsInSphere = @"https://www.edsm.net/api-v1/sphere-systems?";
        public const string url_SystemsInCube = @"https://www.edsm.net/api-v1/cube-systems?";
        public const string url_EDSMSystemValue = @"https://www.edsm.net/api-system-v1/estimated-value?";
        public const string url_EDSMSystemBodies = @"https://www.edsm.net/api-system-v1/bodies?";

        /* END POINT Spansh */
        public const string url_SpanshBodies = @"https://spansh.co.uk/api/bodies/search?";

        public static bool isEDSMOnline()
        {
            if (isURLOnline(@"https://www.edsm.net/"))
                return true;
            return false;
        }
        public static bool isEDDBOnline()
        {
            if (isURLOnline(@"https://elitebgs.app/api/eddb"))
                return true;
            return false;
        }
        private static bool isURLOnline(string _url)
        {
            bool _alive = true;
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url);
                response = (HttpWebResponse)req.GetResponse();
            }
            catch (Exception ex)
            {
                SteamVR_NexHUD.Log(ex.Message);
                _alive = false;
            }

            if (response != null)
            {
                //SteamVR_NexHUD.Log("Returned status code: " + response.StatusDescription);
                _alive = response.StatusCode == HttpStatusCode.OK;
                response.Close();
            }

            return _alive;
        }

        public class spanshParam
        {
            public int? count;
            public string reference_system;
            public spanshFilter filters;
            public spanshSort[] sort;
        }
        public class spanshFilter
        {
            public spanshValue<long> system_id64;
            public spanshValue<int> distance_from_coords;
            public spanshValue<long> population;
            public spanshValue<double>[] materials;
            public spanshValue<bool> is_landable;
            // public intValue distance;
        }
        public class spanshValue<T>
        {
            public string name;
            public T value;
            public string comparison;
            public T min;
            public T max;

            public spanshValue(T _min, T _max)
            {
                min = _min;
                max = _max;
            }
            public spanshValue(T _v) : this(null, ( typeof(T) == typeof(bool) ? "": "==" ), _v)
            {
            }
            public spanshValue(string _comparison, T _v ) : this(null,  _comparison, _v)
            {
            }
            public spanshValue(string _name, string _comparison,  T _value )
            {
                name = _name;
                value = _value;
                comparison = _comparison;
            }
        }

        public class spanshSort
        {
            public spanshSortValue distance_from_coords;
            public spanshSortValue[] materials;
        }
        public class spanshSortValue
        {
            //public const string ASC = "asc";
            //public const string DESC = "desc";

            public string name;
            public string direction;
            public spanshSortValue(bool _ascending)
            {
                direction = _ascending ? "asc" : "desc";
            }
            public spanshSortValue( string _name, bool _ascending)
            {
                name = _name;
                direction = _ascending ? "asc" : "desc";
            }
        }

        public static SpanshBodiesResult SpanshBodies(string _systemOrigin, KeyValuePair<EliteMaterial,double>[] _materials, int maxDistance = 25, int minDistance = 0)
        {
            Dictionary<string, string> _sParams = new Dictionary<string, string>();

            spanshValue<double> [] _spanshMats = new spanshValue<double>[_materials.Length];
            for(int i = 0; i < _materials.Length; i++)
            {
                _spanshMats[i] = new spanshValue<double>(_materials[i].Key.ToString(), ">=", _materials[i].Value);
            }
            spanshSortValue[] _spanshMatsSort = new spanshSortValue[_materials.Length];
            for (int i = 0; i < _materials.Length; i++)
            {
                _spanshMatsSort[i] = new spanshSortValue(_materials[i].Key.ToString(), false);
            }

            spanshParam p = new spanshParam()
            {
                count = 10,
                reference_system = _systemOrigin,
                filters = new spanshFilter()
                {
                    distance_from_coords = new spanshValue<int>(minDistance, maxDistance),
                    materials = _spanshMats,
                    is_landable = new spanshValue<bool>(true)
                },
                sort = new spanshSort[] { new spanshSort() {
                    materials = _spanshMatsSort,
                    distance_from_coords = new spanshSortValue(true) } }
            };
            string _spanshJson = JsonConvert.SerializeObject(p);

            string json = requestPOSTFromURL(url_SpanshBodies, _spanshJson);
            try
            {
                SpanshBodiesResult result = JsonConvert.DeserializeObject<SpanshBodiesResult>(json);
                Console.WriteLine("Result: {0} RealCount: {1}", result.count, result.results.Length);
                Console.WriteLine("Search ID: "+result.search_reference);

                return result;
            }
            catch(Exception ex)
            {
                SteamVR_NexHUD.Log(ex.Message);
                SteamVR_NexHUD.Log(json);
            }

            return null;
        }
        public static EDSMSystemDatas EDSMSystemBodies(string _systemName)
        {
            string _edsmParams = string.Format("systemName={0}", _systemName);


            string _json = requestGETFromURL(url_EDSMSystemBodies, _edsmParams.ToString());

            try { return JsonConvert.DeserializeObject<EDSMSystemDatas>(_json); }
            catch (Exception ex)
            {
                SteamVR_NexHUD.Log(ex.Message);
            }
            return null;
        }

        public static EDSMSystemDatas[] EDSMSystemsInSphereRadius(string _originSystem, int _minRadius, int _radius, bool _showInformation = true)
        {
            string _edsmParams = string.Format("systemName={0}&minRadius={1}&radius={2}&showCoordinates=1", _originSystem, _minRadius, _radius);
            if (_showInformation)
                _edsmParams += "&showInformation=1";

            string _json = requestGETFromURL(url_SystemsInSphere, _edsmParams.ToString());

            try { return JsonConvert.DeserializeObject<EDSMSystemDatas[]>(_json); }
            catch (Exception ex) { SteamVR_NexHUD.Log(ex.Message); }
            return null;
        }

        public struct EDSMSystemParameters
        {
            public string name;
            public bool showPermit;
            public bool showInformation;
            public bool showPrimaryStar;
            //public bool includeHidden;
        }

        public static EDSMSystemDatas EDSMSystemFullInfos(string _systemName)
        {
            return EDSMSystem(new EDSMSystemParameters()
            {
                name = _systemName,
                showPermit = true,
                showInformation = true,
                showPrimaryStar = true,
            });
        }

        public static EDSMSystemDatas EDSMSystem(EDSMSystemParameters _parameters)
        {
            string _edsmParams = "systemName=" + _parameters.name + "&showCoordinates=1";

            if (_parameters.showPermit)
                _edsmParams += "&showPermit=1";
            if (_parameters.showInformation)
                _edsmParams += "&showInformation=1";
            if (_parameters.showPrimaryStar)
                _edsmParams += "&showPrimaryStar=1";

            string _json = requestGETFromURL(url_EDSMSystem, _edsmParams.ToString());

            try { return JsonConvert.DeserializeObject<EDSMSystemDatas>(_json); }
            catch (Exception ex) { SteamVR_NexHUD.Log(ex.Message); }
            return null;
        }


        public static EDSMSystemDatas EDSMSystemValue(string _systemName, int? _systemId = null)
        {
            string _edsmParams = "systemName=" + _systemName;
            if (_systemId != null)
                _edsmParams += "&systemId=" + _systemId;

            string _json = requestGETFromURL(url_EDSMSystemValue, _edsmParams.ToString());

            try { return JsonConvert.DeserializeObject<EDSMSystemDatas>(_json); }
            catch (Exception ex) { SteamVR_NexHUD.Log("ERROR. System name={0}. message:{1}", _systemName, ex.Message); }
            return null;
        }

        /// <summary>
        /// Get powerplay informations
        /// </summary>
        /// <param name="_systemName"></param>
        /// <returns></returns>
        public static EDDBSystemDatas EDDBSystemComplementaryInfos(string _systemName)
        {
            SteamVR_NexHUD.Log("Search complementary information EDDB for System: " + _systemName);

            string _json = requestGETFromURL(url_EDDBSystems, string.Format("name={0}", _systemName));

            try
            {
                EDDBSystemPage _systemPage = JsonConvert.DeserializeObject<EDDBSystemPage>(_json);
                if (_systemPage != null && _systemPage.docs != null && _systemPage.docs.Length > 0)
                {
                    return _systemPage.docs[0];
                }
            }
            catch (Exception ex)
            {
                SteamVR_NexHUD.Log(ex.Message);
            }
            return null;
        }


        private static string requestPOSTFromURL(string _url, string _json)
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
                SteamVR_NexHUD.Log(ex.Message);
            }
            return "Error in requestPOSTFromURL";
        }
        private static string requestGETFromURL(string _url, string _params)
        {
            string html = string.Empty;
            string url = _url + _params;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;



            string _json = "";
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    _json = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                SteamVR_NexHUD.Log(ex.Message);
            }
            return _json;
        }
    }
}
