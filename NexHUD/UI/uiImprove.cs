using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.UI
{
    public class UiImprove : NxGroup
    {
        public enum UiImproveState
        {
            CraftList,
            Blueprints,
            BlueprintDetail
        }
        private NxMenu m_menu;
        private UiImproveState m_state = UiImproveState.CraftList;
        private UiImproveState m_previousState = UiImproveState.CraftList;

        private UiImproveCraftlist m_craftlist;
        private UiImproveBlueprints m_blueprints;
        private UiImproveBlueprintDetails m_blueprintDetails;

        public NxMenu Menu { get => m_menu;  }
        internal UiImproveCraftlist Craftlist { get => m_craftlist; }
        public UiImproveBlueprints Blueprints { get => m_blueprints;  }
        public UiImproveBlueprintDetails BlueprintDetails { get => m_blueprintDetails;  }

        public UiImprove(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_menu = _menu;

            m_craftlist = new UiImproveCraftlist(this);
            m_blueprints = new UiImproveBlueprints(this);
            m_blueprintDetails = new UiImproveBlueprintDetails(this);

            Add(m_craftlist);
            Add(m_blueprints);
            Add(m_blueprintDetails);

            changeState(UiImproveState.CraftList);
        }
        public void changeState(UiImproveState _newState)
        {
            if (_newState == m_state)
                return;
            m_previousState = m_state;
            m_state = _newState;
            m_craftlist.isVisible = false;
            m_blueprints.isVisible = false;
            m_blueprintDetails.isVisible = false;
            switch (m_state)
            {
                case UiImproveState.CraftList:
                    m_craftlist.isVisible = true;
                    break;
                case UiImproveState.Blueprints:
                    m_blueprints.isVisible = true;
                    break;
                case UiImproveState.BlueprintDetail:
                    m_blueprintDetails.isVisible = true;
                    break;
            }
        }
        public override void Update()
        {
            base.Update();
            if (!isVisible)
                return;

            if( SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.back) ) )
            {
                if (m_state == UiImproveState.BlueprintDetail && m_previousState == UiImproveState.CraftList)
                    changeState(UiImproveState.CraftList);
                else if (m_state == UiImproveState.BlueprintDetail && m_previousState == UiImproveState.Blueprints)
                    changeState(UiImproveState.Blueprints);
                else if (m_state == UiImproveState.Blueprints)
                    changeState(UiImproveState.CraftList);
                else
                    m_menu.changeState(NxMenu.MenuState.Main);
            }
        }
    }
}
