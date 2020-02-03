namespace NexHUD.apis.edsm
{

    public class EDSMSystemDatas
    {
        public string name;
        public EDSMPrimaryStar primaryStar;
        public uint? estimatedValue;
        public uint? estimatedValueMapped;
        public bool? requirePermit;
        public EDSMValuableBody[] valuableBodies;
        public EDSMTraffic traffic;
        public EDSMDeaths deaths;
        public EDSMCoords coords;
        public bool? coordsLocked;
        public EDSMInformation information;

        ///for sphere-radius result
        public double? distance;
        public int? bodyCount;

        //Bodies
        public EDSMBody[] bodies;
        public EDSMMaterials materials;
    }

    public class EDSMBody
    {
        public string name;
        public string type;
        public string subType;
        public int? distanceToArrival;
        public bool isMainStar;
        public bool isScoopable;
        public int? age;
        public string luminosity;
        public double? absoluteMagnitude;
        public double? solarMasses;
        public double? solarRadius;
        public int surfaceTemperature;
        public double? orbitalPeriod;
        public double? semiMajorAxis;
        public double? orbitalEccentricity;
        public double? orbitalInclination;
        public double? argOfPeriapsis;
        public double? rotationalPeriod;
        public bool? rotationalPeriodTidallyLocked;
        public double? axialTilt;
        public bool? isLandable;
        public double? gravity;
        public double? earthMasses;
        public double? radius;
        public string volcanismType;
        public string atmosphereType;
        public string terraformingState;
        public EDSMRing[] rings;
        public EDSMMaterials materials;
    }
    public class EDSMMaterials
    {
        public double? Carbon;
        public double? Niobium;
        public double? Vanadium;
        public double? Yttrium;
        public double? Chromium;
        public double? Molybdenum;
        public double? Phosphorus;
        public double? Technetium;
        public double? Cadmium;
        public double? Manganese;
        public double? Ruthenium;
        public double? Sulphur;
        public double? Iron;
        public double? Selenium;
        public double? Tin;
        public double? Zinc;
        public double? Germanium;
        public double? Nickel;
        public double? Tellurium;
        public double? Tungsten;
        public double? Arsenic;
        public double? Mercury;
        public double? Polonium;
        public double? Rhenium;
        public double? Antimony;
        public double? Boron;
        public double? Lead;
        public double? Zirconium;
    }
    public class EDSMRing
    {
        public string name;
        public string type;
        public long? mass;
        public double? innerRadius;
        public double? outerRadius;
    }
    public class EDSMCoords
    {
        public double x;
        public double y;
        public double z;
    }
    public class EDSMInformation
    {
        public string allegiance;
        public string government;
        public string faction;
        public string factionState;
        public long? population;
        public string economy;
        public string secondEconomy;
        public string reserve;
        public string security;
    }
    public class EDSMPrimaryStar
    {
        public string type;
        public string name;
        public bool? isScoopable;
    }
    public class EDSMValuableBody
    {
        public int? bodyID;
        public string bodyName;
        public uint? distance;
        public uint? valueMax;
    }
    public class EDSMTraffic
    {
        public int? total;
        public int? week;
        public int? day;
    }
    public class EDSMDeaths
    {
        public int? total;
        public int? week;
        public int? day;
    }
}
