using NexHUDCore.NxItems;
using System.Drawing;

namespace NexHUD.Ui.Common
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
        private bool m_isSelectable = true;

        public Color ColorLabel;
        public Color ColorLabelSelected;
        public Color ColorLabelDisable;
        public Color ColorLabelDisableSelected;

        public Color ColorBack;
        public Color ColorBackSelected;
        public Color ColorBackDisable;
        public Color ColorBackDisableSelected;
        
        private int m_labelTextSize = 16;

        public int LabelTextSize
        {
            get => m_labelTextSize;
            set => m_labelTextSize = value;
        }
        
        public string Label
        {
            get { return m_buttonName.text; }
            set { m_buttonName.text = value; }
        }
        public int Height { get { return Selected ? (int)(m_height * HeightInc) : m_height; } set { m_height = value; repos(); } }
        public int Width { get { return m_width; } set { m_width = value; repos(); } }
        public NxSimpleText labelST { get => m_buttonName; }
        public bool isSelectable { get => m_isSelectable; set { if (m_isSelectable != value) makeItDirty();  m_isSelectable = value; } }

        public void resetColors()
        {
            ColorLabel = EDColors.YELLOW;
            ColorLabelSelected = EDColors.WHITE;
            ColorLabelDisable = EDColors.getColor(EDColors.WHITE, 0.3f);
            ColorLabelDisableSelected = EDColors.getColor(EDColors.WHITE, 0.5f);

            ColorBack = EDColors.getColor(EDColors.ORANGE, 0.1f);
            ColorBackSelected = EDColors.getColor(EDColors.ORANGE, 0.8f);
            ColorBackDisable = EDColors.getColor(EDColors.WHITE, 0.1f);
            ColorBackDisableSelected = EDColors.getColor(EDColors.WHITE, 0.2f);
        }

        public void repos()
        {
            m_background.x = x;
            m_background.y = y;
            m_background.width = m_width;
            m_background.height = m_height;


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
                m_buttonName.Color = Selected ? ColorLabelDisableSelected : ColorLabelDisable;
            else
                m_buttonName.Color = ColorLabel;

            if (isSelectable)
                m_background.Color = Selected ? ColorBackSelected : ColorBack;
            else
                m_background.Color = Selected ? ColorBackDisableSelected : ColorBackDisable;

            if (Selected)
            {
                m_background.height = (int)(m_height * HeightInc);
                m_background.y = y - (m_background.height - m_height) / 2;
                m_buttonName.size = m_labelTextSize+4;
            }
            else
            {
                m_background.height = (m_height);
                m_background.y = y;
                m_buttonName.size = m_labelTextSize;
            }
        }
    }
}
