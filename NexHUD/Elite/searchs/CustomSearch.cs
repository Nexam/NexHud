using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NexHUD.Apis.Spansh;

namespace NexHUD.Elite.Searchs
{
    public class CustomSearch
    {
        /// <summary>
        /// Name of the search
        /// </summary>
        public string SearchName;
        /// <summary>
        /// User list of systems with notes (e.g. Double painites hotpots)
        /// </summary>
        public Dictionary<string, string> SystemsNotes;
        /// <summary>
        /// (First) Search params for systems endpoint
        /// </summary>
        public SpanshSearchSystems SearchSystem;
        /// <summary>
        /// (Second) Search params for bodies endpoint
        /// </summary>
        public SpanshSearchBodies SearchBodies;

        [JsonIgnore]
        public bool Serializable = true;

    }
}
