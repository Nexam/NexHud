using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.Inputs;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Common
{
    public class NxCheckbox : NxGroup, ISelectable
    {
        public object Obj;

        public bool renderContour = false;

        public float AlphaNormal = 0.3f;
        public float AlphaSelected = 0.8f;

        public Color colorSelected { get => m_colorSelected; set { if (value != m_colorSelected) makeItDirty(); m_colorSelected = value; } }
        public Color colorDisable { get => m_colorDisable; set { if (value != m_colorDisable) makeItDirty(); m_colorDisable = value; } }
        public Color colorUnselectable { get => m_colorUnselectable; set { if (value != m_colorUnselectable) makeItDirty(); m_colorUnselectable = value; } }
        public bool Selected { get => m_selected; set { if (value != m_selected) makeItDirty(); m_selected = value; } }
        public bool isSelectable { get => m_isSelectable; set { if (value != m_isSelectable) makeItDirty(); m_isSelectable = value; } }
        public bool isEnable { get => m_isEnable; set { if (value != m_isEnable) makeItDirty(); m_isEnable = value; } }

        public bool Checked { get => m_Checked; set { if (value != m_Checked) makeItDirty(); m_Checked = value; } }

        public int BoxSize { get => m_BoxSize; set { if (value != m_BoxSize) makeItDirty(); m_BoxSize = value; } }

        public bool CircleBox { get => m_CircleBox; set { if (value != m_CircleBox) makeItDirty(); m_CircleBox = value; } }

        private bool m_Checked = false;
        public string Label;
        private bool m_selected = false;
        private bool m_isSelectable = true;
        private bool m_isEnable = true;
        private Color m_colorSelected;
        private Color m_colorDisable;
        private Color m_colorUnselectable;
        private int m_BoxSize = 30;
        private bool m_CircleBox = false;

        public static Font DefaultFont = NxFont.getFont(NxFonts.EuroStile, 18);

        public EventHandler onClick;

        public NxCheckbox(int _x, int _y, int _width, int _height, string _label, NxMenu _parent) : base(_parent.frame.NxOverlay)
        {
            RelativeChildPos = true;
            width = _width;
            height = _height;
            x = _x;
            y = _y;
            Label = _label;
            configColors(EDColors.ORANGE, EDColors.YELLOW, EDColors.BLUE, EDColors.GRAY);
        }
        public void configColors(Color _color, Color _selected, Color _disable, Color _unselectable)
        {
            Color = _color;
            colorSelected = _selected;
            colorDisable = _disable;
            colorUnselectable = _unselectable;

        }
        public override void Render(Graphics _g)
        {
            float a = Selected ? AlphaSelected : AlphaNormal;
            Color c = isSelectable ? (isEnable ? (Selected ? colorSelected : Color) : colorSelected) : colorUnselectable;
            Color cIgnoreSelected = isSelectable ? (isEnable ? Color : colorSelected) : colorUnselectable;

            //Back
            _g.FillRectangle(new SolidBrush(EDColors.getColor(cIgnoreSelected, Selected ? 0.1f : 0.05f)), Rectangle);

            //Contour
            if (renderContour)
                _g.DrawRectangle(new Pen(EDColors.getColor(c, a)), Rectangle);

            //Checkbox
            if (CircleBox)
            {
                _g.DrawEllipse(new Pen(EDColors.getColor(c, a*0.5f), 2), new Rectangle(x + 2, y + (Rectangle.Height / 2) - (m_BoxSize / 2), m_BoxSize, m_BoxSize));
                if (Checked)
                    _g.FillEllipse(new SolidBrush(colorSelected), new Rectangle(x + 4, y + (Rectangle.Height / 2) - (m_BoxSize / 2) + 2, m_BoxSize - 4, m_BoxSize - 4));

            }
            else
            {
                _g.DrawRectangle(new Pen(EDColors.getColor(c, a * 0.5f), 2), new Rectangle(x + 2, y + (Rectangle.Height / 2) - (m_BoxSize / 2), m_BoxSize, m_BoxSize));
                if (Checked)
                    _g.FillRectangle(new SolidBrush(colorSelected), new Rectangle(x + 4, y + (Rectangle.Height / 2) - (m_BoxSize / 2) + 2, m_BoxSize - 4, m_BoxSize - 4));
            }
            Color ctext = Checked ? colorSelected : c;

            //text
            SizeF sizeLabel = _g.MeasureString(Label, DefaultFont);
            _g.DrawString(
                Label,
               DefaultFont,
                new SolidBrush(EDColors.getColor(ctext, Selected || Checked ? 1f: 0.4f)),
                x + m_BoxSize + 2,
                y + (Rectangle.Height / 2) - (sizeLabel.Height/2) + 4
                );
            base.Render(_g);
        }

        public override void Update()
        {
            base.Update();
            if( isVisible )
            {
                if( Selected )
                {
                    bool select = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select));
                    if (select)
                    {
                        Checked = !Checked;
                        onClick?.Invoke(this, new EventArgs());
                    }
                }
            }
        }
    }
}
