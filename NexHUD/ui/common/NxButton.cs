using NexHUDCore.NxItems;
using System.Drawing;

namespace NexHUD.Ui.Common
{
    public class NxButton : NxGroup, ISelectable
    {
        public object Obj;

        private const float HeightInc = 1.1f;

        public Point Coords = new Point(0, 0);


        private NxRectangle m_background;
        private NxSimpleText m_buttonName;

        public bool Selected { get; set; }
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
        public NxSimpleText labelST { get => m_buttonName; }
        public bool isSelectable { get => m_isSelectable; set { if (m_isSelectable != value) makeItDirty();  m_isSelectable = value; } }

        public bool isEnable { get; set; }

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
            m_background.x = 0;
            m_background.y = 0;
            m_background.width = width;
            m_background.height = height;


            m_buttonName.x = (height / 2);
            m_buttonName.y = (width / 2);
        }
        public NxButton(int _x, int _y, int _width, int _height, string _label, NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            RelativeChildPos = true;
            width = _width;
            height = _height;
            x = _x;
            y = _y;

            resetColors();

            m_background = new NxRectangle(0, 0, width, height, ColorBack);
            Add(m_background);
        
            m_buttonName = new NxSimpleText( (width / 2), (height / 2), _label, EDColors.getColor(EDColors.WHITE, 0.2f));
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
                //m_background.height = (int)(height * HeightInc);
                //m_background.y = y - (m_background.height - height) / 2;
                m_buttonName.size = m_labelTextSize+4;
            }
            else
            {
                //m_background.height = (m_height);
                //m_background.y = y;
                m_buttonName.size = m_labelTextSize;
            }
        }
    }
}
