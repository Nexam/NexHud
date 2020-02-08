using Newtonsoft.Json;
using NexHUD.Elite;
using NexHUDCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public enum SearchError
        {
            None,
            Aborded,
            Unknow,
            SerializationFailed
        }
        /* END POINT */
        public const string URL_BODIES = @"https://spansh.co.uk/api/bodies/search?";
        public const string URL_SYSTEMS = @"https://spansh.co.uk/api/systems/search?";

        //Memory
        private Dictionary<string, UserSearchResult> m_memory = new Dictionary<string, UserSearchResult>();
        private HttpWebRequest m_lastRequest = null;

        public bool IsRequesting()
        {
            if( m_lastRequest != null )
            {
                return true;
            }
            return false;
        }

        public async void SearchInSystems(SpanshSearchSystems _search, Action<SpanshSystemsResult> _method, Action<SearchError> _onFailedMethod)
        {
           
            string _spanshJson = JsonConvert.SerializeObject(_search, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }); ;
            NxLog.log(NxLog.Type.Debug, "SearchInSystems. json={0}", _spanshJson);

            SearchError error = SearchError.None;
            Task<string> task = new Task<string>(() => requestPOSTFromURL(URL_SYSTEMS, _spanshJson, out error));
            task.Start();
            string json = await task;

            if (error != SearchError.None)
            {
                _onFailedMethod?.Invoke(error);
            }
            else
            {
                // string json = requestPOSTFromURL(URL_BODIES, _spanshJson);
                try
                {
                    SpanshSystemsResult result = JsonConvert.DeserializeObject<SpanshSystemsResult>(json, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    NxLog.log(NxLog.Type.Debug, "Search Successful! Search ID = " + result.search_reference);

                    _method?.Invoke(result);
                }
                catch (Exception ex)
                {
                    NexHudEngine.Log(NxLog.Type.Error, ex.Message);
                    _onFailedMethod?.Invoke(error = SearchError.SerializationFailed);
                }
            }
        }
        public async void SearchInBodies(SpanshSearchBodies _search, Action<SpanshBodiesResult> _method, Action<SearchError> _onFailedMethod)
        {
            //for test purpose:
          /*  _search = new SpanshSearchBodies()
            {
                filters = new SpanshFilterBodies()
                {
                    distance_from_coords = new SpanshValue<int>(0, 100),
                    is_landable = new SpanshValue<bool>(true),
                    estimated_mapping_value = new SpanshValue<int>(">", 100000)
                },
                reference_system = "Sol",
                size = 10,
                Page = 0,                
            };*/
 
            string _spanshJson = JsonConvert.SerializeObject(_search, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore }); ;
            NxLog.log(NxLog.Type.Debug, "SearchInBodies. json={0}", _spanshJson);

            SearchError error = SearchError.None;
            Task<string> task = new Task<string>( () => requestPOSTFromURL(URL_BODIES, _spanshJson, out error) );
            task.Start();
            string json = await task;

            if (error != SearchError.None)
            {
                _onFailedMethod?.Invoke(error);
            }
            else
            {
                // string json = requestPOSTFromURL(URL_BODIES, _spanshJson);
                try
                {
                    SpanshBodiesResult result = JsonConvert.DeserializeObject<SpanshBodiesResult>(json, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    NxLog.log( NxLog.Type.Debug, "Search Successful! Search ID = " + result.search_reference);

                    _method?.Invoke(result);
                }
                catch (Exception ex)
                {
                    NexHudEngine.Log(NxLog.Type.Error, ex.Message);
                    _onFailedMethod?.Invoke( error = SearchError.SerializationFailed);
                }
            }
        }

        private string requestPOSTFromURL(string _url, string _json, out SearchError error)
        {
            if (m_lastRequest != null)            
                m_lastRequest.Abort();
            

            m_lastRequest = (HttpWebRequest)WebRequest.Create(_url);
            m_lastRequest.ContentType = "application/json";
            m_lastRequest.Method = "POST";            

            using (var streamWriter = new StreamWriter(m_lastRequest.GetRequestStream()))
            {
                streamWriter.Write(_json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)m_lastRequest.GetResponse();
                m_lastRequest = null;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    error = SearchError.None;
                    return result;
                }
            }
            catch (Exception ex)
            {
               
                error = SearchError.Unknow;
                if ( ex is WebException )
                {
                    if (((WebException)ex).Status == WebExceptionStatus.RequestCanceled)
                    {
                        error = SearchError.Aborded; 
                        NexHudEngine.Log(NxLog.Type.Warning, ex.Message);
                    }
                }
                else
                    NexHudEngine.Log(NxLog.Type.Error, ex.Message);
            }
            return string.Empty;
        }
    }
}
