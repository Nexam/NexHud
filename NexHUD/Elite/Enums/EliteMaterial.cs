using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Elite.Enums
{
    public enum EliteMaterial
    {
        Carbon,
        Niobium,
        Vanadium,
        Yttrium,
        Chromium,
        Molybdenum,
        Phosphorus,
        Technetium,
        Cadmium,
        Manganese,
        Ruthenium,
        Sulphur,
        Iron,
        Selenium,
        Tin,
        Zinc,
        Germanium,
        Nickel,
        Tellurium,
        Tungsten,
        Arsenic,
        Mercury,
        Polonium,
        Rhenium,
        Antimony,
        Boron,
        Lead,
        Zirconium
    }

    public class EliteMaterialHelper
    {
        public static double getDefaultThreshold(EliteMaterial _material)
        {
            switch (_material)
            {
                case EliteMaterial.Carbon: return 18.8;
                case EliteMaterial.Niobium: return 1.8;
                case EliteMaterial.Vanadium: return 9.5;
                case EliteMaterial.Yttrium: return 1.6;
                case EliteMaterial.Chromium: return 10.6;
                case EliteMaterial.Molybdenum: return 1.8;
                case EliteMaterial.Phosphorus: return 12.1;
                case EliteMaterial.Technetium: return 1.0;
                case EliteMaterial.Cadmium: return 2.1;
                case EliteMaterial.Manganese: return 9.7;
                case EliteMaterial.Ruthenium: return 1.6;
                case EliteMaterial.Sulphur: return 22.3;
                case EliteMaterial.Iron: return 27.5;
                case EliteMaterial.Selenium: return 3.3;
                case EliteMaterial.Tin: return 1.8;
                case EliteMaterial.Zinc: return 6.9;
                case EliteMaterial.Germanium: return 3.7;
                case EliteMaterial.Nickel: return 20.8;
                case EliteMaterial.Tellurium: return 1.0;
                case EliteMaterial.Tungsten: return 1.5;
                case EliteMaterial.Arsenic: return 1.7;
                case EliteMaterial.Mercury: return 1.2;
                case EliteMaterial.Polonium: return 1.17;
                case EliteMaterial.Rhenium: return 0.0; //Mining
                case EliteMaterial.Antimony: return 1.0;
                case EliteMaterial.Boron: return 0.0; // mining
                case EliteMaterial.Lead:  return 0.0; //mining
                case EliteMaterial.Zirconium: return 3.1;
            }
            return 0.0;

        }
    }
}
