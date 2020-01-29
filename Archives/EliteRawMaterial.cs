using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Elite.Enums
{
    public enum EliteMaterialGrade
    {
        VeryCommon = 1,
        Common = 2,
        Standard = 3,
        Rare = 4,
        VeryRare = 5,
    }
    public enum EliteRawMaterial
    {
        //Very Common
        Carbon,
        Phosphorus,
        Sulphur,
        Iron,
        Nickel,
        Rhenium,
        Lead,
        //Common
        Vanadium,
        Chromium,
        Manganese,
        Zinc,
        Germanium,
        Arsenic,
        Zirconium,
        //Standard,
        Niobium,
        Molybdenum,
        Cadmium,
        Tin,
        Tungsten,
        Mercury,
        Boron,
        //Rare
        Yttrium,
        Technetium,
        Ruthenium,
        Selenium,
        Tellurium,
        Polonium,
        Antimony,
    }

    public enum EliteManufacturedMaterial
    {
        //Very common
        SalvagedAlloys,
        GridResistors,
        ChemicalStorageUnits,
        CompactComposites,
        BasicConductors,
        CrystalShards,
        GuardianPowerCell,
        GuardianWreckageComponents,
        HeatConductionWiring,
        MechanicalScrap,
        WornShieldEmitters,
        TemperedAlloys,
        //Common
        GalvanisingAlloys,
        HybridCapacitors,
        ChemicalProcessors,
        FilamentComposites,
        ConductiveComponents,
        FlawedFocusCrystals,
        GuardianPowerConduit,
        HeatDispersionPlate,
        MechanicalEquipment,
        ShieldEmitters,
        ThargoidCarapace,
        HeatResistantCeramics,
        //Standard
        PhaseAlloys,
        ElectrochemicalArrays,
        ChemicalDistillery,
        HighDensityComposites,
        ConductiveCeramics,
        FocusCrystals,
        GuardianSentinelWeaponParts,
        GuardianTechnologyComponent,
        HeatExchangers,
        MechanicalComponents,
        ShieldingSensors,
        BioMechanicalConduits,
        PropulsionElements,
        WeaponParts,
        WreckageComponents,
        ThargoidEnergyCell,
        PrecipitatedAlloys,
        //Rare
        ProtoLightAlloys,
        PolymerCapacitors,
        ChemicalManipulators,
        ProprietaryComposites,
        ConductivePolymers,
        RefinedFocusCrystals,
        HeatVanes,
        ConfigurableComponents,
        CompoundShielding,
        ThargoidTechnologicalComponents,
        ThermicAlloys,
        //VeryRare
        ProtoRadiolicAlloys,
        MilitarySupercapacitors,
        PharmaceuticalIsolators,
        CoreDynamicsComposites,
        BiotechConductors,
        ExquisiteFocusCrystals,
        ProtoHeatRadiators,
        ImprovisedComponents,
        ImperialShielding,
        SensorFragment,
        ThargoidOrganicCircuitry,
        MilitaryGradeAlloys,
    }
    public class EliteMaterialHelper
    {
        public static EliteMaterialGrade getGrade(EliteManufacturedMaterial _material)
        {
            if (_material >= EliteManufacturedMaterial.ProtoRadiolicAlloys)
                return EliteMaterialGrade.VeryCommon;
            else if (_material >= EliteManufacturedMaterial.ProtoLightAlloys)
                return EliteMaterialGrade.Rare;
            else if (_material >= EliteManufacturedMaterial.PhaseAlloys)
                return EliteMaterialGrade.Standard;
            else if (_material >= EliteManufacturedMaterial.GalvanisingAlloys)
                return EliteMaterialGrade.Common;
            else
                return EliteMaterialGrade.VeryCommon;
        }
        public static EliteMaterialGrade getGrade(EliteRawMaterial _material)
        {
            if (_material >= EliteRawMaterial.Yttrium)
                return EliteMaterialGrade.Rare;
            else if (_material >= EliteRawMaterial.Niobium)
                return EliteMaterialGrade.Standard;
            else if (_material >= EliteRawMaterial.Vanadium)
                return EliteMaterialGrade.Common;
            else
                return EliteMaterialGrade.VeryCommon;
        }
        public static double getDefaultThreshold(EliteRawMaterial _material)
        {
            switch (_material)
            {
                case EliteRawMaterial.Carbon: return 18.8;
                case EliteRawMaterial.Niobium: return 1.8;
                case EliteRawMaterial.Vanadium: return 9.5;
                case EliteRawMaterial.Yttrium: return 1.6;
                case EliteRawMaterial.Chromium: return 10.6;
                case EliteRawMaterial.Molybdenum: return 1.8;
                case EliteRawMaterial.Phosphorus: return 12.1;
                case EliteRawMaterial.Technetium: return 1.0;
                case EliteRawMaterial.Cadmium: return 2.1;
                case EliteRawMaterial.Manganese: return 9.7;
                case EliteRawMaterial.Ruthenium: return 1.6;
                case EliteRawMaterial.Sulphur: return 22.3;
                case EliteRawMaterial.Iron: return 27.5;
                case EliteRawMaterial.Selenium: return 3.3;
                case EliteRawMaterial.Tin: return 1.8;
                case EliteRawMaterial.Zinc: return 6.9;
                case EliteRawMaterial.Germanium: return 3.7;
                case EliteRawMaterial.Nickel: return 20.8;
                case EliteRawMaterial.Tellurium: return 1.0;
                case EliteRawMaterial.Tungsten: return 1.5;
                case EliteRawMaterial.Arsenic: return 1.7;
                case EliteRawMaterial.Mercury: return 1.2;
                case EliteRawMaterial.Polonium: return 1.17;
                case EliteRawMaterial.Rhenium: return 0.0; //Mining
                case EliteRawMaterial.Antimony: return 1.0;
                case EliteRawMaterial.Boron: return 0.0; // mining
                case EliteRawMaterial.Lead: return 0.0; //mining
                case EliteRawMaterial.Zirconium: return 3.1;
            }
            return 0.0;

        }
    }
}
