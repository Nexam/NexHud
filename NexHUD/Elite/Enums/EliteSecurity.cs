using NexHUDCore.NxItems;
using System.Drawing;

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
