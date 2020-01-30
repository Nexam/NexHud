using NexHUDCore.NxItems;
using System.Drawing;

namespace NexHUD.UI
{
    public class NxButton : NxGroup
    {
        public object Obj;

        private const float HeightInc = 1.1f;

        public Point Coords = new Point(0, 0);

        private int m_height = 10;
        private int m_width = 10;

        private NxRectangle m_background;
        private NxSimpleText m_buttonName;

        public bool Selected = false;
        public bool isSelectable = true;

        public Color ColorLabel;
        public Color ColorLabelSelected;
        public Color ColorLabelDisable;

        public Color ColorBack;
        public Color ColorBackSelected;
        public Color ColorBackDisable;

        public Color ColorLines;


        private NxRectangle[] Lines = new NxRectangle[2];

        public string Label
        {
            get { return m_buttonName.text; }
            set { m_buttonName.text = value; }
        }
        public int Height { get { return Selected ? (int)(m_height * HeightInc) : m_height; } set { m_height = value; repos(); } }
        public int Width { get { return m_width; } set { m_width = value; repos(); } }
        public NxSimpleText labelST { get => m_buttonName; }
        public void resetColors()
        {
            ColorLabel = EDColors.YELLOW;
            ColorLabelSelected = EDColors.WHITE;
            ColorLabelDisable = EDColors.GRAY;

            ColorBack = EDColors.getColor(EDColors.ORANGE, 0.1f);
            ColorBackSelected = EDColors.getColor(EDColors.ORANGE, 0.8f);
            ColorBackDisable = EDColors.getColor(EDColors.WHITE, 0.1f);
            ColorLines = EDColors.YELLOW;
        }

        private void repos()
        {
            m_background.x = x;
            m_background.y = y;
            m_background.width = m_width;
            m_background.height = m_height;

            Lines[0].x = x;
            Lines[0].y = y;
            Lines[0].width = m_width;
            Lines[0].height = 1;

            Lines[1].x = x;
            Lines[1].y = y + m_height - 1;
            Lines[1].width = m_width;
            Lines[1].height = 1;

            m_buttonName.x = x + (m_width / 2);
            m_buttonName.y = y + (m_height / 2);
        }
        public NxButton(int _x, int _y, int _width, int _height, string _label, NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_width = _width;
            m_height = _height;
            x = _x;
            y = _y;

            resetColors();

            m_background = new NxRectangle(x, y, m_width, m_height, ColorBack);
            Add(m_background);
            Lines[0] = new NxRectangle(x, y, m_width, 1, EDColors.YELLOW);
            Lines[1] = new NxRectangle(x, y + m_height - 1, m_width, 1, EDColors.getColor(EDColors.YELLOW, 0.5f));
            Add(Lines[0]);
            Add(Lines[1]);

            m_buttonName = new NxSimpleText(x + (m_width / 2), y + (m_height / 2), _label, EDColors.getColor(EDColors.WHITE, 0.2f));
            m_buttonName.centerHorizontal = true;
            m_buttonName.centerVertical = true;
            Add(m_buttonName);
        }
        public override void Update()
        {
            base.Update();
            if (Selected && isSelectable)
                m_buttonName.Color = ColorLabelSelected;
            else if (!isSelectable)
                m_buttonName.Color = ColorLabelDisable;
            else
                m_buttonName.Color = ColorLabel;

            if (isSelectable)
                m_background.Color = Selected ? ColorBackSelected : ColorBack;
            else
                m_background.Color = ColorBackDisable;

            if (Selected)
            {
                m_background.height = (int)(m_height * HeightInc);
                m_background.y = y - (m_background.height - m_height) / 2;
                m_buttonName.size = 20;
            }
            else
            {
                m_background.height = (m_height);
                m_background.y = y;
                m_buttonName.size = 16;
            }



            Lines[0].Color = ColorLines;
            Lines[1].Color = EDColors.getColor(ColorLines, 0.5f);
        }
    }
}
