using NexHUDCore;
using OpenTK;
using System;

namespace NexHUD.UI
{
    public class NxMenu
    {
        public enum MenuState
        {
            Initialize,
            Main,
            Search,
            Improve,
            Trade,
        }
        public const int Width = 1024, Height = 630;
        public const float OverlayWidth = 2f;

        private NexHudOverlay m_frame;

        private MenuState m_state = MenuState.Initialize;

        public NexHudOverlay frame { get { return m_frame; } }
        //Top Infos panel
        public UiMainTopInfos m_uiTopInfos;
        //Player infos & main menu
        public UiMainPlayerInfos m_uiPlayerInfos;
        public UiMainMenu m_uiMainMenu;
        //Search
        public UiSearch m_searchPanel;
        //Improve
        public UiImprove m_uiImprove;
        public NxMenu()
        {

            initMainFrame();
            initContent();

            NexHudEngine.PostUpdateCallback += SteamVR_NexHUD_PostUpdateCallback;

            changeState(MenuState.Main);


            frame.renderOverlay = false;
        }
        private void initMainFrame()
        {
            m_frame = new NexHudOverlay(Width, Height, "NxMenu", "NexHUD Menu");
            m_frame.setVRPosition(new Vector3(0, 0, -3), new Vector3(0, 0, 0));
            m_frame.setWMPosition(new Vector2(0, 0.2f), 0.4f);
            m_frame.Alpha = 1f;
            m_frame.setVRWidth(OverlayWidth);
            m_frame.NxOverlay.dirtyCheckFreq = TimeSpan.FromSeconds(0.1);
        }

        private void initContent()
        {
            m_uiTopInfos = new UiMainTopInfos(this);
            m_frame.NxOverlay.Add(m_uiTopInfos);

            m_uiMainMenu = new UiMainMenu(this);
            m_frame.NxOverlay.Add(m_uiMainMenu);
            //Desactivate unavailable buttons
            //m_menuPanel.setActive(NxMainPanelMenuButton.MenuButtonType.Improve, false);
            m_uiMainMenu.setActive(UiMainMenuButton.MenuButtonType.Trade, false);

            m_uiPlayerInfos = new UiMainPlayerInfos(this);
            m_frame.NxOverlay.Add(m_uiPlayerInfos);

            m_searchPanel = new UiSearch(this);
            m_frame.NxOverlay.Add(m_searchPanel);

            m_uiImprove = new UiImprove(this);
            m_frame.NxOverlay.Add(m_uiImprove);

        }

        public void changeState(MenuState _newState)
        {
            if (_newState == m_state)
                return;
            m_state = _newState;

            m_searchPanel.isVisible = false;
            m_uiImprove.isVisible = false;
            m_uiMainMenu.isVisible = false;
            m_uiPlayerInfos.isVisible = false;

            switch (m_state)
            {
                case MenuState.Main:
                    m_uiMainMenu.isVisible = true;
                    m_uiPlayerInfos.isVisible = true;
                    break;
                case MenuState.Search:
                    m_searchPanel.isVisible = true;
                    break;
                case MenuState.Improve:
                    m_uiImprove.isVisible = true;
                    break;
            }
        }

        private void SteamVR_NexHUD_PostUpdateCallback(object sender, EventArgs e)
        {
            if (m_state == MenuState.Initialize)
                return;


            if (Shortcuts.holdMode)
            {
                if (NexHudEngine.isShortcutIsHold(Shortcuts.get(ShortcutId.menu)))
                {
                    if (!m_frame.renderOverlay)
                    {
                        m_frame.renderOverlay = true;
                    }

                    if (NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.back)))
                    {
                        if (m_state != MenuState.Main && m_state != MenuState.Improve)
                            changeState(MenuState.Main);
                    }
                }
                else
                {
                    if (m_frame.renderOverlay)
                    {
                        m_frame.renderOverlay = false;
                    }
                    return;
                }
            }
            else
            {
                if (NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.menu)))
                {
                    if (m_frame.renderOverlay)
                    {
                        if (m_state != MenuState.Main)
                        {
                            if( m_state != MenuState.Improve)
                                changeState(MenuState.Main);
                        }
                        else
                            m_frame.renderOverlay = false;
                    }
                    else
                    {
                        changeState(MenuState.Main);
                        m_frame.renderOverlay = true;
                    }
                }
            }

            if (m_frame.renderOverlay)
            {
                //MAIN MENU NAVIGATION
                if (m_uiMainMenu.isVisible)
                {
                    if (NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.right)))
                    {
                        m_uiMainMenu.selectNext();
                    }
                    else if (NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.left)))
                    {
                        m_uiMainMenu.selectPrev();
                    }
                    else if (NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select)))
                    {
                        if (m_uiMainMenu.isSelectedMenuActive())
                        {
                            switch (m_uiMainMenu.SelectedMenu)
                            {
                                case UiMainMenuButton.MenuButtonType.Search:
                                    changeState(MenuState.Search);
                                    break;
                                case UiMainMenuButton.MenuButtonType.Improve:
                                    changeState(MenuState.Improve);
                                    break;
                                case UiMainMenuButton.MenuButtonType.Trade:
                                    changeState(MenuState.Trade);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
