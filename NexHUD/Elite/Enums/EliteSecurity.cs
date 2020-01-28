using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore.NxItems;

namespace NexHUD.Elite
{
    public enum EliteSecurity
    {
        Unknow,
        Low,
        Medium,
        High,

        Anarchy,
        Lawless
    }
    public class EliteSecurityHelper
    {
        public static Color getColor(EliteSecurity _security)
        {
            switch (_security)
            {
                case EliteSecurity.Anarchy:
                case EliteSecurity.Lawless:
                    return EDColors.RED;
                case EliteSecurity.Low:
                    return EDColors.YELLOW;
                case EliteSecurity.Medium:
                    return EDColors.GREEN;
                case EliteSecurity.High:
                    return EDColors.LIGHTBLUE;
            }
            return Color.Yellow;
        }
    }
}
