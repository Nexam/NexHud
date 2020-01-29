using System;
using System.Drawing;

namespace NexHUDCore.NxItems
{
    public class NxLoading : NxItem
    {
        private float _elapsed = 0;

        public int width = 1;
        public int height = 20;
        public int spacing = 6;

        private float[] scales;
        private int numbers = 9;
        public float speed = 5;

        public float freqSpeed = 2f;
        public float minFreq = 1f;
        public float maxFreq = 6;
        private float freq = 0.1f;
        public bool incFreq = true;
        private Brush m_brush;
        private Brush m_brush2;

        private float m_fondWide = 0;

        public NxLoading(int _x, int _y)
        {
            x = _x;
            y = _y;
            m_brush = new SolidBrush(EDColors.ORANGE);
            m_brush2 = new SolidBrush(EDColors.WHITE);
            scales = new float[numbers];
            scales.Initialize();

        }
        public override void Render(Graphics _g)
        {
            int _totalWidth = width * numbers + spacing * (numbers - 1);
            int _h = height;

            int _x = x - _totalWidth / 2;
            int _y = y - height / 2;

            for (float i = 0; i < scales.Length; i++)
                _g.FillRectangle(i == 0 || i == (scales.Length - 1) ? m_brush2 : m_brush, _x + width * i + spacing * i, _y - (_h * scales[(int)i]) / 2, width, _h * scales[(int)i]);

            if (m_fondWide == 0)
                m_fondWide = _g.MeasureString("Loading", NxFont.getFont(NxFonts.EuroCapital, 16)).Width;
            //if()
            _g.DrawString("Loading", NxFont.getFont(NxFonts.EuroCapital, 16), m_brush, x - m_fondWide / 2, _y + height / 2);

            // _g.FillRectangle(m_brush, _x + _w + spacing,           _y - (_h * s2) / 2,   _w,   _h * s2);
            // _g.FillRectangle(m_brush, _x + _w * 2 + spacing * 2,   _y - (_h * s3) / 2,   _w,   _h * s3);
        }

        public override void Update()
        {
            _elapsed += SteamVR_NexHUD.deltaTime * speed;

            if (_elapsed > 3.14f)
                _elapsed = -3.14f;

            if (incFreq)
                freq += SteamVR_NexHUD.deltaTime * freqSpeed;
            else
                freq -= SteamVR_NexHUD.deltaTime * freqSpeed;

            if (freq < minFreq)
                incFreq = true;
            else if (freq > maxFreq)
                incFreq = false;



            for (float i = 0; i < scales.Length; i++)
            {
                float n = scales.Length;
                scales[(int)i] = .1f + .9f * (1f + (float)Math.Cos(_elapsed + i / (n / freq))) / 2f;
            }

            makeItDirty();
        }
    }
}
