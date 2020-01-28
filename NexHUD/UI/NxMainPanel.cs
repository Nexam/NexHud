using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore;
using NexHUDCore.NxItems;
using OpenTK;
using OpenTK.Input;

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

        private NexHUDOverlay m_frame;

        private MenuState m_state = MenuState.Initialize;

        public NexHUDOverlay frame { get { return m_frame; } }
        //Top Infos panel
        public NxMainPanelTopInfos m_topPanel;
        public NxMainPanelMenu m_menuPanel;
        public NxMainPanelPlayerInfos m_playerInfosPanel;
        public NxMainPanelSearch m_searchPanel;

        public NxMenu()
        {

            initMainFrame();
            initContent();

            SteamVR_NexHUD.PostUpdateCallback += SteamVR_NexHUD_PostUpdateCallback;

            changeState(MenuState.Main);


            frame.InGameOverlay.Hide();
        }
        private void initMainFrame()
        {
            m_frame = new NexHUDOverlay(Width, Height, "NxMenu", "NexHUD Menu");
            m_frame.InGameOverlay.SetAttachment(AttachmentType.Absolute, new Vector3(0, 0, -3), new Vector3(0, 0, 0));
            m_frame.InGameOverlay.Alpha = 1f;
            m_frame.InGameOverlay.Width = OverlayWidth;
            m_frame.UpdateEveryFrame = true;
            m_frame.NxOverlay.dirtyCheckFreq = TimeSpan.FromSeconds(0.1);
        }

        private void initContent()
        {
            m_topPanel = new NxMainPanelTopInfos(this);
            m_frame.NxOverlay.Add(m_topPanel);

            m_menuPanel = new NxMainPanelMenu(this);
            m_frame.NxOverlay.Add(m_menuPanel);
            //Desactivate unavailable buttons
            m_menuPanel.setActive(NxMainPanelMenuButton.MenuButtonType.Improve, false);
            m_menuPanel.setActive(NxMainPanelMenuButton.MenuButtonType.Trade, false);

            m_playerInfosPanel = new NxMainPanelPlayerInfos(this);
            m_frame.NxOverlay.Add(m_playerInfosPanel);

            m_searchPanel = new NxMainPanelSearch(this);
            m_frame.NxOverlay.Add(m_searchPanel);

        }

        public void changeState(MenuState _newState)
        {
            if (_newState == m_state)
                return;
            m_state = _newState;
            switch(m_state)
            {
                case MenuState.Main:
                    m_menuPanel.isVisible = true;
                    m_playerInfosPanel.isVisible = true;
                    m_searchPanel.isVisible = false;
                    break;
                case MenuState.Search:
                    m_menuPanel.isVisible = false;
                    m_playerInfosPanel.isVisible = false;
                    m_searchPanel.isVisible = true;
                    break;
            }
        }

        private void SteamVR_NexHUD_PostUpdateCallback(object sender, EventArgs e)
        {
            if (m_state == MenuState.Initialize)
                return;


            if (Shortcuts.holdMode)
            {
                if( SteamVR_NexHUD.isShortcutIsHold(Shortcuts.get(ShortcutId.menu ) ))
                {
                    if(!m_frame.RenderInGameOverlay)
                    {
                        m_frame.RenderInGameOverlay = true;
                    }

                    if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.back)))
                    {
                        if (m_state != MenuState.Main)
                            changeState(MenuState.Main);
                    }
                }
                else
                {
                    if(m_frame.RenderInGameOverlay)
                    {
                        m_frame.RenderInGameOverlay = false;                        
                    }
                    return;
                }
            }
            else
            {
                if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.menu)))
                {
                    if (m_frame.RenderInGameOverlay)
                    {
                        if (m_state != MenuState.Main)
                            changeState(MenuState.Main);
                        else
                            m_frame.RenderInGameOverlay = false;
                    }
                    else
                    {
                        changeState(MenuState.Main);
                        m_frame.RenderInGameOverlay = true;
                    }
                }
            }

            if (m_frame.RenderInGameOverlay)
            {                
                //MAIN MENU NAVIGATION
                if (m_menuPanel.isVisible)
                {
                    if (SteamVR_NexHUD.isShortcutPressed( Shortcuts.get(ShortcutId.right) ))
                    {
                        m_menuPanel.selectNext();
                    }
                    else if (SteamVR_NexHUD.isShortcutPressed( Shortcuts.get(ShortcutId.left) ))
                    {
                        m_menuPanel.selectPrev();
                    }
                    else if( SteamVR_NexHUD.isShortcutPressed( Shortcuts.get(ShortcutId.select) ))
                    {
                        if( m_menuPanel.isSelectedMenuActive() )
                        {
                            switch (m_menuPanel.SelectedMenu)
                            {
                                case NxMainPanelMenuButton.MenuButtonType.Search:
                                    changeState(MenuState.Search);
                                    break;
                                case NxMainPanelMenuButton.MenuButtonType.Improve:
                                    changeState(MenuState.Improve);
                                    break;
                                case NxMainPanelMenuButton.MenuButtonType.Trade:
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
