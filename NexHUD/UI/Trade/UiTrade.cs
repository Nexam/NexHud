using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.Inputs;
using NexHUD.Ui.Search;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Trade
{
    public class UiTrade : NxGroup
    {
        public enum UiTradeState
        {
            TradeMain,
            SellCargo,
            FindRoute,
            Search,
        }
        private NxMenu m_menu;
        private UiTradeState m_state = UiTradeState.Search;
        private UiTradeState m_previousState = UiTradeState.TradeMain;

        public NxMenu Menu { get => m_menu; }
        
        public UiTrade(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_menu = _menu;

                   changeState(UiTradeState.SellCargo);
        }
        public void changeState(UiTradeState _newState)
        {
            if (_newState == m_state)
                return;
            if (m_state != UiTradeState.Search)
                m_previousState = m_state;
            m_state = _newState;
            //All invisible


            //Visibility
            switch (m_state)
            {
                case UiTradeState.TradeMain:
                    break;
                case UiTradeState.SellCargo:
                    break;
                case UiTradeState.FindRoute:
                    break;
                case UiTradeState.Search:
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
            if (!Shortcuts.holdMode && !_back)
                _back = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.menu));

            if (_back)
            {
                /*if (m_state == UiImproveState.BlueprintDetail && m_previousState == UiImproveState.CraftList)
                    changeState(UiImproveState.CraftList);
                else if (m_state == UiImproveState.BlueprintDetail && m_previousState == UiImproveState.Blueprints)
                    changeState(UiImproveState.Blueprints);
                else if (m_state == UiImproveState.Blueprints)
                    changeState(UiImproveState.CraftList);
                else if (m_state == UiImproveState.Search)
                    changeState(UiImproveState.BlueprintDetail);
                else*/
                {
                    changeState(UiTradeState.TradeMain);
                    m_menu.changeState(NxMenu.MenuState.Main);
                }
            }
        }
    }
}
