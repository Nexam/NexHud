using System.Drawing;

namespace NexHUDCore.NxItems
{
    public class NxRectangle : NxItem
    {
        public NxRectangle(int _x, int _y, int _w, int _h, Color _c)
        {
            x = _x;
            y = _y;
            width = _w;
            height = _h;
            Color = _c;
        }
        public override void Render(Graphics _g)
        {
            _g.FillRectangle(SolidBrush, x, y, width, height);
        }

        public override void Update()
        {
        }
    }
}
