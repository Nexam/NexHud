using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUDCore.NxItems
{
    public class NxRectangle : NxItem
    {
        private int m_w;
        private int m_h;
        public int width
        {
            get => m_w;
            set
            {
                if (m_w != value)
                {
                    m_w = value; 
                    makeItDirty();
                }
            }
        }
        public int height
        {
            get => m_h;
            set
            {
                if( m_h != value)
                {
                    m_h = value; 
                    makeItDirty();
                }
            }
        }

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
