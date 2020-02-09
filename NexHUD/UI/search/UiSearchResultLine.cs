using NexHUD.Ui.Common;
using NexHUDCore.NxItems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Ui.Search
{
    public class UiSearchResultLine : NxGroup, ISelectable
    {
        private NxSimpleText[] m_props;
        private NxRectangle m_background;
        private bool m_Selected = false;
        private bool m_isSelectable = true;
        private bool m_isEnable = true;

        public Color ColorBackSelected;
        public Color ColorBackDisable;
        public Color ColorText;
        public Color ColorTextSelected;
        public Color ColorTextDisable;
        public UiSearchResultLine(NxOverlay _parent) : base(_parent)
        {
            Color = EDColors.getColor(EDColors.ORANGE, 0.1f);
            ColorBackSelected = EDColors.getColor(EDColors.ORANGE, 0.8f);
            ColorBackDisable = EDColors.getColor(EDColors.WHITE, 0.1f);

            ColorText = EDColors.YELLOW;
            ColorTextSelected = EDColors.WHITE;
            ColorTextDisable = EDColors.BLACK;
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Render(Graphics _g)
        {
            base.Render(_g);
        }
        public bool Selected { get => m_Selected; set { if (m_Selected != value) makeItDirty(); m_Selected = value; } }
        public bool isSelectable { get => m_isSelectable; set { if (m_isSelectable != value) makeItDirty(); m_isSelectable = value; } }
        public bool isEnable { get => m_isEnable; set { if (m_isEnable != value) makeItDirty(); m_isEnable = value; } }

    }
}
