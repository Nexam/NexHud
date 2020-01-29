using Newtonsoft.Json;
using NexHUD.Elite;
using NexHUDCore;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NexHUD.EDEngineer
{
    public class EngineerHelper
    {
        private static bool m_dataLoaded = false;

        private static BlueprintDatas[] m_bpDatas;
        private static MaterialDatas[] m_matDatas;

        public static BlueprintDatas[] blueprints { get { loadDatas(); return m_bpDatas; } }
        public static MaterialDatas[] materials { get { loadDatas(); return m_matDatas; } }

        public static void loadDatas()
        {
            if (m_dataLoaded)
                return;

            SteamVR_NexHUD.Log("Loading blueprints datas...");
            string _json = ResHelper.GetResourceText(Assembly.GetExecutingAssembly(), "EDEngineer.Datas.blueprints.json");
            m_bpDatas = JsonConvert.DeserializeObject<BlueprintDatas[]>(_json);
            SteamVR_NexHUD.Log(">>{0} blueprints loaded", m_bpDatas.Length);

            //Assigning categories
            foreach (BlueprintDatas d in m_bpDatas)
                d.Categorie = getCategorie(d);


            SteamVR_NexHUD.Log("Loading materials datas...");
            _json = ResHelper.GetResourceText(Assembly.GetExecutingAssembly(), "EDEngineer.Datas.entryData.json");
            m_matDatas = JsonConvert.DeserializeObject<MaterialDatas[]>(_json);
            SteamVR_NexHUD.Log(">>{0} materials loaded", m_matDatas.Length);

            SteamVR_NexHUD.Log("Loading search for materials...");
            string _path = Environment.CurrentDirectory + "\\Config\\MaterialsSearchs.json";
            NxSearchEntry[] _searchs = null;
            if (File.Exists(_path))
            {
                try
                {
                    _json = File.ReadAllText(_path);
                    _searchs = JsonConvert.DeserializeObject<NxSearchEntry[]>(_json);
                }
                catch (Exception ex)
                {
                    SteamVR_NexHUD.Log(ex.Message);
                }
            }

            if( _searchs != null )
            {
                foreach(NxSearchEntry s in _searchs)
                {
                    s.format();
                    MaterialDatas md = m_matDatas.Where(x => x.Name.ToLower() == s.searchName.ToLower()).FirstOrDefault();
                    if (md != null)
                        md.nxSearch = s;
                }
            }

            foreach(MaterialDatas md in m_matDatas.Where(x => x.Kind == "Material" && x.Subkind == "Raw" && x.Name != "Boron" && x.Name != "Lead" && x.Name != "Rhenium") )
            {
                if( md.nxSearch == null)
                {
                    NxSearchEntry _newSearch = new NxSearchEntry()
                    {
                        searchName = md.Name,
                        searchType = NxSearchType.body,
                        searchDisplay = new string[] {"materials","threat", "distanceToArrival" },
                        searchMaxRadius = 0, //default
                        searchParams = new string[][] { new string[] { "isLandable", "true" }, new string[] { "rawMaterial", md.Name } }
                    };
                    _newSearch.format();
                    md.nxSearch = _newSearch;
                }
            }

            int searchFoundAndCreated = m_matDatas.Where(x => x.nxSearch != null).Count();

            SteamVR_NexHUD.Log("{0}/{1} materials with available searchs", searchFoundAndCreated, m_matDatas.Length);
           
            m_dataLoaded = true;
        }

        public static BlueprintCategorie getCategorie(BlueprintDatas _datas)
        {
            if (_datas.Engineers.Contains("@Synthesis"))
                return BlueprintCategorie.Synthesis;

            switch (_datas.Type)
            {
                case "Beam Laser":
                case "Burst Laser":
                case "Cannon":
                case "Fragment Cannon":
                case "Mine Launcher":
                case "Missile Rack":
                case "Multi-Cannon":
                case "Plasma Accelerator":
                case "Pulse Laser":
                case "Rail Gun":
                case "Seeker Missile Rack":
                case "Torpedo Pylon":
                    return BlueprintCategorie.Hardpoint;
                case "Chaff Launcher":
                case "Electronic Countermeasure":
                case "Frame Shift Wake Scanner":
                case "Heat Sink Launcher":
                case "Kill Warrent Scanner":
                case "Manifest Scanner":
                case "Point Defence":
                case "Shield Booster":
                    return BlueprintCategorie.Utility;
                case "Auto Field-Maintenance Unit":
                case "Collector Limpet Controller":
                case "Detailed Surface Scanner":
                case "Frame Shift Interdictor":
                case "Fuel Scoop":
                case "Fuel Transfer Limpet Controller":
                case "Hatch Breaker Limpet Controller":
                case "Hull Reinforcement Package":
                case "Prospector Limpet Controller":
                case "Refinery":
                case "Shield Cell Bank":
                case "Shield Generator":
                    return BlueprintCategorie.OptionalInternal;
                case "Frame Shift Drive":
                case "Life Support":
                case "Power Distributor":
                case "Power Plant":
                case "Sensors":
                case "Thrusters":
                    return BlueprintCategorie.CoreInternal;
                case "Armour":
                    return BlueprintCategorie.Armour;

            }

            return BlueprintCategorie.Other;
        }

        public static bool isRawMaterial(string _materialName)
        {
            loadDatas();
            MaterialDatas m = m_matDatas.Where(x => x.Name == _materialName).FirstOrDefault();
            return m != null;
        }

        public static string getChemicalSymbol(string _rawMaterial)
        {
            for (int i = 0; i < ChemicalNames.Length; i++)
                if (ChemicalNames[i] == _rawMaterial)
                    return ChemicalSymboles[i];
            return "Unk";
        }

        private static string[] ChemicalNames = new string[]
        {
             "Hydrogen","Helium","Lithium","Beryllium","Boron","Carbon","Nitrogen",
            "Oxygen","Fluorine","Neon","Sodium","Magnesium","Aluminium","Silicon","Phosphorus","Sulfur","Chlorine","Argon",
            "Potassium","Calcium","Scandium","Titanium","Vanadium","Chromium","Manganese","Iron","Cobalt","Nickel","Copper","Zinc",
            "Gallium","Germanium","Arsenic","Selenium","Bromine","Krypton","Rubidium","Strontium","Yttrium","Zirconium","Niobium",
            "Molybdenum","Technetium","Ruthenium","Rhodium","Palladium","Silver","Cadmium","Indium","Tin","Antimony","Tellurium","Iodine",
            "Xenon","Caesium","Barium","Lanthanum","Cerium","Praseodymium","Neodymium","Promethium","Samarium","Europium","Gadolinium",
            "Terbium","Dysprosium","Holmium","Erbium","Thulium","Ytterbium","Lutetium","Hafnium","Tantalum","Tungsten","Rhenium","Osmium",
            "Iridium","Platinum","Gold","Mercury","Thallium","Lead","Bismuth","Polonium","Astatine","Radon","Francium","Radium",
            "Actinium","Thorium","Protactinium","Uranium","Neptunium","Plutonium","Americium","Curium","Berkelium","Californium",
            "Einsteinium","Fermium","Mendelevium","Nobelium","Lawrencium","Rutherfordium","Dubnium","Seaborgium","Bohrium","Hassium",
            "Meitnerium","Darmstadtium","Roentgenium","Copernicium","Nihonium","Flerovium","Moscovium","Livermorium","Tennessine","Oganesson"
        };
        private static string[] ChemicalSymboles = new string[]
        {
            "H","He","Li","Be","B","C","N","O","F","Ne","Na","Mg","Al","Si","P","S","Cl","Ar","K","Ca","Sc","Ti","V","Cr","Mn",
            "Fe","Co","Ni","Cu","Zn","Ga","Ge","As","Se","Br","Kr","Rb","Sr","Y","Zr","Nb","Mo","Tc","Ru","Rh","Pd","Ag","Cd","In",
            "Sn","Sb","Te","I","Xe","Cs","Ba","La","Ce","Pr","Nd","Pm","Sm","Eu","Gd","Tb","Dy","Ho","Er","Tm","Yb","Lu","Hf","Ta",
            "W","Re","Os","Ir","Pt","Au","Hg","Tl","Pb","Bi","Po","At","Rn","Fr","Ra","Ac","Th","Pa","U","Np","Pu","Am","Cm","Bk","Cf",
            "Es","Fm","Md","No","Lr","Rf","Db","Sg","Bh","Hs","Mt","Ds","Rg","Cn","Nh","Fl","Mc","Lv","Ts","Og"
        };
    }
}
