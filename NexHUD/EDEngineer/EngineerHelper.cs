﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NexHUDCore;

namespace NexHUD.EDEngineer
{
    public class EngineerHelper
    {
        private static bool m_dataLoaded = false;

        private static BlueprintDatas[] m_bpDatas;
        private static MaterialDatas[] m_matDatas;
        public static void loadDatas()
        {
            if (m_dataLoaded)
                return;

            SteamVR_NexHUD.Log("Loading blueprints datas...");
            string _json = ResHelper.GetResourceText(Assembly.GetExecutingAssembly(), "EDEngineer.Datas.blueprints.json");
            m_bpDatas = JsonConvert.DeserializeObject<BlueprintDatas[]>(_json);
            SteamVR_NexHUD.Log(">>{0} blueprints loaded", m_bpDatas.Length);

            SteamVR_NexHUD.Log("Loading materials datas...");
            _json = ResHelper.GetResourceText(Assembly.GetExecutingAssembly(), "EDEngineer.Datas.entryData.json");
            m_matDatas = JsonConvert.DeserializeObject<MaterialDatas[]>(_json);
            SteamVR_NexHUD.Log(">>{0} materials loaded", m_bpDatas.Length);

            m_dataLoaded = true;
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
