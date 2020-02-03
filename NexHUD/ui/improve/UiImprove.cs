using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.inputs;
using NexHUD.ui.search;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.ui.improve
{
    public class UiImprove : NxGroup
    {
        public enum UiImproveState
        {
            CraftList,
            Blueprints,
            BlueprintDetail,
            Search,
        }
        private NxMenu m_menu;
        private UiImproveState m_state = UiImproveState.Search;
        private UiImproveState m_previousState = UiImproveState.CraftList;

        private UiImproveCraftlist m_craftlist;
        private UiImproveBlueprints m_blueprints;
        private UiImproveBlueprintDetails m_blueprintDetails;
        private UiSearch m_search;

        public NxMenu Menu { get => m_menu;  }
        internal UiImproveCraftlist Craftlist { get => m_craftlist; }
        public UiImproveBlueprints Blueprints { get => m_blueprints;  }
        public UiImproveBlueprintDetails BlueprintDetails { get => m_blueprintDetails;  }
        public UiSearch search { get => m_search; }

        public UiImprove(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_menu = _menu;

            m_craftlist = new UiImproveCraftlist(this);
            m_blueprints = new UiImproveBlueprints(this);
            m_blueprintDetails = new UiImproveBlueprintDetails(this);
            m_search = new UiSearch(m_menu, true);

            Add(m_craftlist);
            Add(m_blueprints);
            Add(m_blueprintDetails);
            Add(m_search);

            changeState(UiImproveState.CraftList);
        }
        public void changeState(UiImproveState _newState)
        {
            if (_newState == m_state)
                return;
            if( m_state != UiImproveState.Search )
                m_previousState = m_state;
            m_state = _newState;
            m_craftlist.isVisible = false;
            m_blueprints.isVisible = false;
            m_blueprintDetails.isVisible = false;
            m_search.isVisible = false;
            switch (m_state)
            {
                case UiImproveState.CraftList:
                    m_craftlist.isVisible = true;
                    m_craftlist.refresh();
                    break;
                case UiImproveState.Blueprints:
                    m_blueprints.isVisible = true;
                    break;
                case UiImproveState.BlueprintDetail:
                    m_blueprintDetails.isVisible = true;
                    break;
                case UiImproveState.Search:
                    m_search.isVisible = true;
                    break;
            }
        }
        private bool _skipUpdate = true;
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

            bool _back = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.back));
            if( !Shortcuts.holdMode && !_back)
                _back = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.menu));

            if (_back)
            {
                if (m_state == UiImproveState.BlueprintDetail && m_previousState == UiImproveState.CraftList)
                    changeState(UiImproveState.CraftList);
                else if (m_state == UiImproveState.BlueprintDetail && m_previousState == UiImproveState.Blueprints)
                    changeState(UiImproveState.Blueprints);
                else if (m_state == UiImproveState.Blueprints)
                    changeState(UiImproveState.CraftList);
                else if (m_state == UiImproveState.Search)
                    changeState(UiImproveState.BlueprintDetail);
                else
                {
                    changeState(UiImproveState.CraftList);
                    m_menu.changeState(NxMenu.MenuState.Main);
                }
            }
        }
    }
}
