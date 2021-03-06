﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUDCore.NxItems
{
    public class NxProgressBar : NxItem
    {
        private float m_value = 50;
        private float m_maxValue = 100;
        
        public int Value
        {
            get => (int)m_value;
            set
            {
                if (m_value != value)
                {
                    m_value = value;
                    makeItDirty();
                }
            }
        }
        public int MaxValue
        {
            get => (int)m_maxValue;
            set
            {
                if (m_maxValue != value)
                {
                    m_maxValue = value;
                    makeItDirty();
                }
            }
        }

        public NxProgressBar(int _x, int _y, int _w, int _h, Color _c)
        {
            x = _x;
            y = _y;
            width = _w;
            height = _h;
            Color = _c;
        }
        public override void Render(Graphics _g)
        {
            _g.FillRectangle( new SolidBrush(Color.FromArgb(124,0,0,0)) , x, y, width, height);
            float percent = m_value / m_maxValue;
            percent = Math.Min( Math.Max(percent, 0),100);
            _g.FillRectangle(SolidBrush, x+1, y+1, (float)(width-2) * percent, height-2);
        }

        public override void Update()
        {
        }
    }
}
