using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.Elite.Craft;
using NexHUD.Inputs;
using NexHUD.Ui.Common;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Improve
{
    public class UiCraftListItem : NxButton
    {
        public const int width = 250;
        public const int height = 85;

        private NxSimpleText m_bpType;
        private NxSimpleText m_bpName;
        private NxSimpleText m_bpExp;
        private NxSimpleText m_materials;

        private CraftlistItem m_item;

        public CraftlistItem item { get => m_item; }
        public UiCraftListItem(int _x, int _y, UiImproveCraftlist _parent) : base( _x,_y, width, height, "Pin", _parent.UiImprove.Menu )
        {
            x = _x;
            y = _y;
            // m_background = new NxRectangle(_x, _y, width, height, EDColors.getColor(EDColors.WHITE, 0.1f));
            //Add(m_background);

            this.labelST.isVisible = false;

            m_bpType = new NxSimpleText(x + width / 2, y + 2, "5 x BEAM LASER", EDColors.ORANGE, 18, NxFonts.EuroCapital) { centerHorizontal = true };
            m_bpName = new NxSimpleText(x + width / 2, y + 22, "> Long range G5 <", EDColors.LIGHTBLUE, 18, NxFonts.EuroStile) { centerHorizontal = true };
            m_bpExp = new NxSimpleText(x + width / 2, y + 42, "[ Thermal vent ]", EDColors.YELLOW, 16, NxFonts.EuroStile) { centerHorizontal = true };
            m_materials = new NxSimpleText(x + width / 2, y + 60, "All materials!", EDColors.GREEN, 16, NxFonts.EuroCapital) { centerHorizontal = true };

            Add(m_bpType);
            Add(m_bpName);
            Add(m_bpExp);
            Add(m_materials);

            ColorBack = EDColors.getColor(EDColors.WHITE, 0.05f);
            ColorBackSelected = EDColors.getColor(EDColors.ORANGE, 0.2f);
        }
        public void set(CraftlistItem _item)
        {
            m_item = _item;
            if (m_item != null && m_item.isValid)
            {
                m_bpType.text = string.Format("{0} x {1}", m_item.count, m_item.type);
                m_bpName.text = string.Format("> {0} G{1} <", m_item.name, m_item.grade);
                m_bpName.Color = EDColors.LIGHTBLUE;
                m_bpExp.text = string.Format("[ {0} ]", string.IsNullOrEmpty(m_item.experimental) ? "no experimental" : m_item.experimental);
                int craftNbr = m_item.canCraft();
                if (craftNbr == m_item.count)
                {
                    m_materials.text = "All materials!";
                    m_materials.Color = EDColors.GREEN;
                }
                else if( craftNbr > 0)
                {
                    m_materials.text = string.Format("Can craft {0}x", craftNbr);
                    m_materials.Color = EDColors.YELLOW;
                }
                else
                {
                    m_materials.text = string.Format("Missing materials!");
                    m_materials.Color = EDColors.RED;
                }
                
            }
            else
            {
                m_bpType.text = "";
                m_bpName.text = "Explore blueprints";
                m_bpName.Color = EDColors.GRAY;
                m_bpExp.text = "";
                m_materials.text = "";

            }
        }
    }
    public class UiImproveCraftlist : NxGroup
    {
        UiImprove m_uiImprove;

        UiCraftListItem[] m_Pins;

        NxButton m_blueprintsBtn;

        public Point cursor;
        public UiImproveCraftlist(UiImprove _uiImprove) : base(_uiImprove.Menu.frame.NxOverlay)
        {
            m_uiImprove = _uiImprove;

            int hMax = 4;
            int vMax = 5;
            m_Pins = new UiCraftListItem[hMax * vMax];
            int i = 0;

            for (int v = 0; v < vMax; v++)
            {
                for (int h = 0; h < hMax; h++)
                {
                    m_Pins[i] = new UiCraftListItem(5 + h * (UiCraftListItem.width + 8), 80 + v * (UiCraftListItem.height + 8), this);
                    m_Pins[i].Coords = new Point(h, v);
                    Add(m_Pins[i]);
                    i++;
                }
            }
            m_blueprintsBtn = new NxButton(NxMenu.Width / 2 - 100, NxMenu.Height - 50, 200, 40, "See blueprints", m_uiImprove.Menu);
            m_blueprintsBtn.Coords = new Point(2, vMax );
            Add(m_blueprintsBtn);
            cursor = m_blueprintsBtn.Coords;
        }
        public void refresh()
        {
            for (int i = 0; i < m_Pins.Length; i++)
            {
                if (i < Craftlist.list.Count)
                {
                    m_Pins[i].set(Craftlist.list[i]);
                }
                else
                    m_Pins[i].set(null);
            }
        }

        private bool _skipUpdate = true;

        public UiImprove UiImprove { get => m_uiImprove; set => m_uiImprove = value; }

        public override void Update()
        {
            base.Update();
            if (!isVisible)
            {
                _skipUpdate = true;
                return;
            }
            else if (_skipUpdate)
            {
                _skipUpdate = false;
                return;
            }

            bool up = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.up));
            bool down = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.down));
            bool left = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.left));
            bool right = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.right));
            bool select = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select));

            if (up && cursor.Y > 0)
                cursor.Y--;
            if (down && cursor.Y < 5)
                cursor.Y++;
            if (left && cursor.X > 0 && cursor.Y != m_blueprintsBtn.Coords.Y)
                cursor.X--;
            if (right && cursor.X < 3 && cursor.Y != m_blueprintsBtn.Coords.Y)
                cursor.X++;

            UiCraftListItem _pinSelected = null;
            foreach (UiCraftListItem i in m_Pins) {
                i.Selected = i.Coords == cursor;
                if (i.Selected)
                    _pinSelected = i;
            }

            m_blueprintsBtn.Selected = m_blueprintsBtn.Coords.Y == cursor.Y;

            if( select)
            {
                if (m_blueprintsBtn.Selected)
                    m_uiImprove.changeState(UiImprove.UiImproveState.Blueprints);
                else if (_pinSelected != null && _pinSelected.item != null)
                {
                    m_uiImprove.changeState(UiImprove.UiImproveState.BlueprintDetail);
                    m_uiImprove.BlueprintDetails.setBlueprint(_pinSelected.item);
                }
            }
           
        }
    }
}
