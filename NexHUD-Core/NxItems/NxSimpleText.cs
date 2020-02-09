using System.Drawing;

namespace NexHUDCore.NxItems
{

    public class NxSimpleText : NxItem
    {
        private string m_text = "blank";
        private int m_size;
        private SizeF m_sizeF = new SizeF();
        private bool m_centerHorizontal = false;
        private bool m_centerVertical = false;
        private NxFonts m_font = NxFonts.EuroStile;
        private bool m_vertical = false;
        private bool m_autoSize = true;

        public SizeF sizeF { get { return m_sizeF; } }

        public string text { get { return m_text; } set { if (m_text != value) { makeItDirty(); ResizeRectangle(); } m_text = value; } }
        public int size { get { return m_size; } set { if (m_size != value) makeItDirty(); m_size = value; } }
        public bool centerHorizontal { get { return m_centerHorizontal; } set { if (m_centerHorizontal != value) makeItDirty(); m_centerHorizontal = value; } }
        public bool centerVertical { get { return m_centerVertical; } set { if (m_centerVertical != value) makeItDirty(); m_centerVertical = value; } }
        public NxFonts font { get { return m_font; } set { if (m_font != value) makeItDirty(); m_font = value; } }

        public bool vertical { get { return m_vertical; } set { if (m_vertical != value) makeItDirty(); m_vertical = value; } }

        public bool AutoSize { get => m_autoSize; set => m_autoSize = value; }

        public NxSimpleText(int _x, int _y, string _text, Color _color, int _size = 16, NxFonts _font = NxFonts.EuroStile)
        {
            text = _text;
            Color = _color;
            font = _font;
            size = _size;
            x = _x; y = _y;
        }

        private Graphics m_lastGraphics = null;
        private void ResizeRectangle()
        {
            if (Overlay != null)
            {
                m_sizeF = Overlay.NxGraphics.MeasureString(text, NxFont.getFont(font, size));
                if (AutoSize)
                {
                    width = (int)m_sizeF.Width;
                    height = (int)m_sizeF.Height;
                }
            }
            else
                throw new System.Exception("Damn! Overlay == null");
        }
        public override void Render(Graphics _g)
        {
            m_lastGraphics = _g;
            m_sizeF = _g.MeasureString(text, NxFont.getFont(font, size));
            if (AutoSize)
            {
                width = (int)m_sizeF.Width;
                height = (int)m_sizeF.Height;
            }
            if (m_vertical)
            {
                _g.RotateTransform(-90);
                _g.DrawString(text, NxFont.getFont(font, size), SolidBrush,
                   -(m_centerVertical ? y - (sizeF.Height / 2) : y),
                   m_centerHorizontal ? x - (sizeF.Width / 2) : x);
                _g.RotateTransform(90);
            }
            else
            {
                _g.DrawString(text, NxFont.getFont(font, size), SolidBrush,
                 m_centerHorizontal ? x - (sizeF.Width / 2) : x,
                m_centerVertical ? y - (sizeF.Height / 2) : y);

            }

        }

        public override void Update()
        {
        }
    }
}
