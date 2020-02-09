using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.Apis.Spansh;

namespace NexHUD.Elite.Searchs
{
    public class CustomSystemsList
    {
        public string name;
        public string notes;
    }
    public class CustomSearch
    {
        /// <summary>
        /// Name of the search
        /// </summary>
        public string SearchName;
        /// <summary>
        /// User list of systems with notes (e.g. Double painites hotpots)
        /// </summary>
        public CustomSystemsList[] CustomSystemsList;
        /// <summary>
        /// (First) Search params for systems endpoint
        /// </summary>
        public SpanshSearchSystems SearchSystem;
        /// <summary>
        /// (Second) Search params for bodies endpoint
        /// </summary>
        public SpanshSearchBodies SearchBodies;

        public void Compile()
        {
            if (CustomSystemsList != null)
            {
                string[] systems = new string[CustomSystemsList.Length];
                for (int i = 0; i < CustomSystemsList.Length; i++)
                    systems[i] = CustomSystemsList[i].name;
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        name = new SpanshValue<string[]>(systems)
                    },
                    reference_system = EDDatas.Instance.getCurrentSystem().name,
                    sort = new SpanshSort[]{
                        new SpanshSort()
                        {
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                };
            }
        }

    }
}
