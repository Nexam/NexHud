using EliteAPI.Events;
using Newtonsoft.Json;
using NexHUD.Apis.Spansh;
using NexHUD.Elite;
using NexHUD.Elite.Searchs;
using NexHUDCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NexHUD.Elite.Engineers
{
    public class EngineerHelper
    {
        private static bool m_dataLoaded = false;

        private static BlueprintDatas[] m_bpDatas;
        private static MaterialDatas[] m_matDatas;
        private static Dictionary<string, List<BlueprintDatas>> m_experimentals = new Dictionary<string, List<BlueprintDatas>>();
        private static int[] m_rollPerGrade = new int[] { 0, 3, 4, 5, 6, 7 };

        public static BlueprintDatas[] blueprints { get { loadDatas(); return m_bpDatas; } }
        public static MaterialDatas[] materials { get { loadDatas(); return m_matDatas; } }
        public static int[] rollPerGrade { get => m_rollPerGrade; }

        public static BlueprintDatas[] getExperimentals(string _blueprintType)
        {
            if (m_experimentals.ContainsKey(_blueprintType))
                return m_experimentals[_blueprintType].ToArray();
            else
                return null;
        }


        public static MaterialDatas[] getAllCraftMaterials(string _blueprintType, string _blueprintName, string _experimental, int _grade)
        {
            Dictionary<string, MaterialDatas> _materials = new Dictionary<string, MaterialDatas>();
            for (int g = 1; g <= _grade + 1; g++)
            {
                BlueprintDatas d = null;
                if (g <= _grade)
                    d = blueprints.Where(x => x.Type == _blueprintType && x.Name == _blueprintName && x.Grade == g).FirstOrDefault();
                else if (!string.IsNullOrEmpty(_experimental))
                    d = blueprints.Where(x => x.Type == _blueprintType && x.Name == _experimental && x.IsExperimental).FirstOrDefault();

                if (d != null)
                {
                    foreach (BlueprintIngredient i in d.Ingredients)
                    {
                        if (!_materials.ContainsKey(i.Name))
                        {
                            MaterialDatas m = materials.Where(x => x.Name == i.Name).FirstOrDefault();
                            m.Quantity = i.Size * (d.IsExperimental ? 1 : rollPerGrade[g]);
                            _materials.Add(i.Name, m);
                        }
                        else
                            _materials[i.Name].Quantity += i.Size * (d.IsExperimental ? 1 : rollPerGrade[g]);
                    }
                }
            }
            return _materials.Values.ToArray();
        }

        public static void loadDatas()
        {
            if (m_dataLoaded)
                return;

            NexHudEngine.Log("Loading blueprints datas...");
            string _json = ResHelper.GetResourceText(Assembly.GetExecutingAssembly(), "Elite.Engineers.Datas.blueprints.json");
            m_bpDatas = JsonConvert.DeserializeObject<BlueprintDatas[]>(_json);
            NexHudEngine.Log(">>{0} blueprints loaded", m_bpDatas.Length);


            //Assigning categories
            foreach (BlueprintDatas d in m_bpDatas)
            {
                d.Categorie = getCategorie(d);
                if (d.IsExperimental)
                {
                    if (!m_experimentals.ContainsKey(d.Type))
                        m_experimentals.Add(d.Type, new List<BlueprintDatas>());
                    m_experimentals[d.Type].Add(d);
                }
                //max grade
                if (!d.IsExperimental && !d.IsUnlock)
                {
                    d.MaxGrade = m_bpDatas.Where(x => x.Type == d.Type && x.Name == d.Name && x.Grade > 0).Count();
                    if (d.MaxGrade > 5 || d.MaxGrade < 0)
                        throw new Exception("Something is wrong....");
                }
            }



            NexHudEngine.Log("Loading materials datas...");
            _json = ResHelper.GetResourceText(Assembly.GetExecutingAssembly(), "Elite.Engineers.Datas.entryData.json");
            m_matDatas = JsonConvert.DeserializeObject<MaterialDatas[]>(_json);
            NexHudEngine.Log(">>{0} materials loaded", m_matDatas.Length);

            fillAutosearchMaterials();


            m_dataLoaded = true;
        }

        private static void fillAutosearchMaterials()
        {
            NexHudEngine.Log("Loading search for materials...");
            string _path = Environment.CurrentDirectory + "\\Config\\MaterialsSearchs.json";
            string _json = "";
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
                    NexHudEngine.Log(ex.Message);
                }
            }

            if (_searchs != null)
            {
                foreach (NxSearchEntry s in _searchs)
                {
                    s.format();
                    MaterialDatas md = m_matDatas.Where(x => x.Name.ToLower() == s.searchName.ToLower()).FirstOrDefault();
                    if (md != null)
                        md.nxSearch = s;
                }
            }

            //Materials raw
            foreach (MaterialDatas md in m_matDatas.Where(x => x.Kind == "Material" && x.Subkind == "Raw" && x.Name != "Boron" && x.Name != "Lead" && x.Name != "Rhenium"))
            {
                
                if (md.cSearch == null)
                {
                    CustomSearch _cSearch = new CustomSearch()
                    {
                        SearchName = md.Name,
                        SearchBodies = new Apis.Spansh.SpanshSearchBodies()
                        {
                            filters = new Apis.Spansh.SpanshFilterBodies()
                            {
                                is_landable = new Apis.Spansh.SpanshValue<bool>(true),
                                distance_from_coords = new Apis.Spansh.SpanshValue<int>(0, 100),
                                materials = new Apis.Spansh.SpanshValue<double>[] { new Apis.Spansh.SpanshValue<double>(md.Name, ">", 0.1) }

                            },
                            sort = new Apis.Spansh.SpanshSort[]{
                                new Apis.Spansh.SpanshSort() { materials =  new Apis.Spansh.SpanshSortValue[]{new Apis.Spansh.SpanshSortValue(md.Name, false) } },
                                new Apis.Spansh.SpanshSort(){ distance_from_coords = new Apis.Spansh.SpanshSortValue(true)}
                            }
                        }
                    };
                    md.cSearch = _cSearch;
                }
            }

            //premade searchs
            int _radiusSystemSearch = 200;

            CustomSearch _sysBoomState = new CustomSearch()
            {
                SearchName = "Auto search. System in boom",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        state = new SpanshValue<string[]>(new string[] { "Boom" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            CustomSearch _sysBoomElectionState = new CustomSearch()
            {
                SearchName = "Auto search. System in boom & election",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        state = new SpanshValue<string[]>(new string[] { "Boom","Election" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            CustomSearch _sysElectionCivilwarState = new CustomSearch()
            {
                SearchName = "Auto search. System in election & civil war",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        state = new SpanshValue<string[]>(new string[] { "Election", "Civil war" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            CustomSearch _sysBoomRetreatState = new CustomSearch()
            {
                SearchName = "Auto search. System in boom & retreat ",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        state = new SpanshValue<string[]>(new string[] { "Boom", "Retreat" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            CustomSearch _sysElectionState = new CustomSearch()
            {
                SearchName = "Auto search. System in election",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        state = new SpanshValue<string[]>(new string[] { "Election" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            CustomSearch _sysBoomOutbreakWarsEmpFedState = new CustomSearch()
            {
                SearchName = "Auto search. Empire/Federation system in various states",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        allegiance = new SpanshValue<string[]>(new string[] { "Empire", "Federation" }),
                        state = new SpanshValue<string[]>(new string[] { "Boom","Outbreak","War","Civil war","Civil unrest" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            CustomSearch _sysBoomOutbreakWarsFedState = new CustomSearch()
            {
                SearchName = "Auto search. Federation system in boom, war states or outbreak",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        allegiance = new SpanshValue<string[]>(new string[] { "Federation" }),
                        state = new SpanshValue<string[]>(new string[] { "Boom", "Outbreak", "War", "Civil war", "Civil unrest" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            CustomSearch _sysOutbreakState = new CustomSearch()
            {
                SearchName = "Auto search. System in outbreak",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        state = new SpanshValue<string[]>(new string[] { "Outbreak" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            CustomSearch _sysAnarchyOutbreakState = new CustomSearch()
            {
                SearchName = "Auto search. Anarchy system in outbreak",
                SearchSystem = new SpanshSearchSystems()
                {
                    filters = new SpanshFilterSystems()
                    {
                        distance_from_coords = new SpanshValue<int?>(0,_radiusSystemSearch),
                        government = new SpanshValue<string[]>(new string[] { "Anarchy" }),
                        state = new SpanshValue<string[]>(new string[] { "Outbreak" }),
                    },
                    sort = new SpanshSort[]{ new SpanshSort(){
                            distance_from_coords = new SpanshSortValue(true)
                        }
                    }
                }
            };
            //Materials other
            foreach (MaterialDatas md in m_matDatas.Where(x => x.nxSearch == null))
            {
                switch (md.Name)
                {
                    case "Aberrant Shield Pattern Analysis":
                    case "Atypical Disrupted Wake Echoes":
                    case "Classified Scan Fragment":
                    case "Unidentified Scan Archives":
                    case "Irregular Emission Data":
                    case "Tagged Encryption Codes":
                    case "Unusual Encrypted Files":
                    case "Inconsistent Shield Soak Analysis":
                    case "Strange Wake Solutions":
                    case "Phase Alloys":
                        md.cSearch = _sysBoomState;
                        break;
                    case "Unexpected Emission Data":
                        md.cSearch = _sysBoomElectionState;
                        break;
                    case "Modified Consumer Firmware":
                        md.cSearch = _sysElectionCivilwarState;
                        break;
                    case "Security Firmware Patch":
                        md.cSearch = _sysBoomRetreatState;
                        break;
                    case "Galvanising Alloys":
                        md.cSearch = _sysElectionState;
                        break;
                    case "Proto Light Alloys":
                    case "Proto Radiolic Alloys":
                    case "Military Supercapacitors":
                        md.cSearch = _sysBoomOutbreakWarsEmpFedState;
                        break;
                    case "Polymer Capacitors":
                    case "Pharmaceutical Isolators":
                        md.cSearch = _sysOutbreakState;
                        break;
                    case "Chemical Manipulators":
                        md.cSearch = _sysAnarchyOutbreakState;
                        break;
                    case "Core Dynamics Composites":
                    case "Proprietary Composites": //last entry
                        md.cSearch = _sysBoomOutbreakWarsFedState;
                        break;

                }
            }

            int searchFoundAndCreated = m_matDatas.Where(x => x.cSearch != null).Count();

            NexHudEngine.Log("{0}/{1} materials with available searchs", searchFoundAndCreated, m_matDatas.Length);
        }

        public static int getCmdrMaterials(string name)
        {
            long _count = 0;

            Encoded e = NexHudMain.EliteApi.Materials.Encoded.Where(x => x.NameLocalised == name).FirstOrDefault();
            if (e != null)
                _count += e.Count;
            Encoded m = NexHudMain.EliteApi.Materials.Manufactured.Where(x => x.NameLocalised == name).FirstOrDefault();
            if (m != null)
                _count += m.Count;
            Raw r = NexHudMain.EliteApi.Materials.Raw.Where(x => x.Name == name.ToLower()).FirstOrDefault();
            if (r != null)
                _count += r.Count;

            return (int)_count;
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
