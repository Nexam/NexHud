using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore.NxItems;

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
            switch(_threat)
            {
                case EliteSystemThreat.High: return EDColors.RED;
                case EliteSystemThreat.Medium: return EDColors.YELLOW;
            }
            return EDColors.GREEN;
        }
    }
}
