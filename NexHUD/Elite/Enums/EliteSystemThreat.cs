using NexHUDCore.NxItems;
using System.Drawing;

namespace NexHUD.Elite
{
    public enum EliteSystemThreat
    {
        Low,
        Medium,
        High
    }
    public class EliteSystemThreatHelper
    {
        public static Color getColor(EliteSystemThreat _threat)
        {
            switch (_threat)
            {
                case EliteSystemThreat.High: return EDColors.RED;
                case EliteSystemThreat.Medium: return EDColors.YELLOW;
            }
            return EDColors.GREEN;
        }
    }
}
