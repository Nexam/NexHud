using NexHUD.elite;

namespace NexHUD.elite.engineers
{
    /* "Name": "Boron",
    "Rarity": "Standard",
    "FormattedName": "boron",
    "Kind": "Material",
    "Subkind": "Raw",
    "OriginDetails": [
      "Surface prospecting",
      "Mining"
    ],
    "Group": "Category7"*/
    public class MaterialDatas
    {
        public string Name;
        public string Rarity;
        public string FormattedName;
        public string Kind;
        public string Subkind;
        public string[] OriginDetails;
        public string Group;

        public NxSearchEntry nxSearch;

        public int Quantity = 1; //for crafting
    }
}
