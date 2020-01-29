using System.Drawing;

namespace NexHUDCore.NxItems
{
    public class EDColors
    {
        public static Color BLACK = Color.Black;
        public static Color WHITE = Color.White;

        public static Color BACKGROUND = Color.FromArgb(150, 0, 0, 0);
        public static Color TRANSPARENT = Color.Transparent;

        public static Color ORANGE = Color.FromArgb(255, 113, 0);
        public static Color YELLOW = Color.FromArgb(255, 176, 0);

        public static Color LIGHTBLUE = Color.FromArgb(0, 179, 247);
        public static Color BLUE = Color.FromArgb(10, 139, 214);

        public static Color GREEN = Color.FromArgb(2, 158, 76);

        public static Color RED = Color.FromArgb(255, 0, 0);

        public static Color GRAY = Color.FromArgb(126, 126, 126);

        public static Color getColor(Color _c, float _Alpha)
        {
            return Color.FromArgb((int)(_Alpha * 255.0f), _c);
        }

    }
}
