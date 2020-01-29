using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace NexHUDCore.NxItems
{
    public class NxTextbox : NxItem
    {
        private string m_text = "blank";
        public NxFonts font = NxFonts.EuroStile;
        public int size;
        public int width;
        public int height;
        private bool m_reverseLineOrder = false;
        private bool m_reverseDrawing = false;

        private bool m_showBounds = false;
        private Color m_boundColors = Color.Red;
        private Pen m_boundPen = Pens.Red;

        private bool m_autowrap = true;
        public string text { get { return m_text; } set { m_text = value; makeItDirty(); } }
        public bool reverseLineOrder { get { return m_reverseLineOrder; } set { m_reverseLineOrder = value; makeItDirty(); } }
        public bool autoWrap { get { return m_autowrap; } set { m_autowrap = value; makeItDirty(); } }
        public bool reverseDrawing { get { return m_reverseDrawing; } set { m_reverseDrawing = value; makeItDirty(); } }

        public bool showBounds
        {
            get { return m_showBounds; }
            set
            {
                if (m_showBounds != value)
                {
                    m_showBounds = value;
                    makeItDirty();
                }

            }
        }
        public Color boundColors
        {
            get { return m_boundColors; }
            set
            {
                if (m_boundColors != value)
                {
                    m_boundColors = value;
                    m_boundPen = new Pen(boundColors);
                    makeItDirty();
                }

            }
        }

        public void clear()
        {
            m_text = "";
            makeItDirty();
        }
        public void AddLine(string _line)
        {
            m_text += "\r\n" + _line;
            makeItDirty();
        }

        public NxTextbox(int _x, int _y, int _width, int _height, string _text, Color _color, int _size = 16, NxFonts _font = NxFonts.EuroStile)
        {
            text = _text;
            Color = _color;
            font = _font;
            size = _size;
            width = _width;
            height = _height;
            x = _x; y = _y;
        }
        public override void Render(Graphics _g)
        {
            if (m_showBounds)
                _g.DrawRectangle(m_boundPen, x, y, width, height);

            Font trFont = NxFont.getFont(font, size);
            string[] _lines = Regex.Split(text, "\r\n|\r|\n");

            if (reverseLineOrder)
                Array.Reverse(_lines);

            int _posY = 0;

            if (reverseDrawing)
                _posY = height - trFont.Height;
            foreach (string _line in _lines)
            {
                if (_g.MeasureString(_line, trFont).Width < width)
                {
                    _g.DrawString(_line, trFont, SolidBrush, x, y + _posY);
                    _posY += trFont.Height * (reverseDrawing ? -1 : 1);
                }
                else
                {
                    string[] _words = _line.Split(' ');

                    int _ii = 0;

                    while (_ii < _words.Length)
                    {
                        string _interline = "";
                        while (_ii < _words.Length)
                        {
                            string _newWord = (_interline.Length != 0 ? " " : "") + _words[_ii];
                            if (_g.MeasureString(_interline + _newWord, trFont).Width < width)
                            {
                                _interline += _newWord;
                                _ii++;
                            }
                            else
                                break;
                        }
                        _g.DrawString(_interline, trFont, SolidBrush, x, y + _posY);
                        _posY += trFont.Height * (reverseDrawing ? -1 : 1);
                        if (!autoWrap)
                            break;
                    }
                }

                if ((_posY > height && reverseDrawing) || (_posY < 0 && !reverseDrawing))
                    break;
            }
        }

        public override void Update()
        {
        }
    }
}
