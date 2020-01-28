﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using NexHUD.Elite;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.UI
{

   
    public class NxMainPanelSearchButton : NxGroup
    {
        public const int Height = 30;
        private int m_width = 10;

        private NxRectangle m_background;
        internal NxSimpleText searchName;

        public bool Selected = false;

        public NxMainPanelSearchButton(int _x, int _y, int _width, NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_width = _width;
            x = _x;
            y = _y;

            m_background = new NxRectangle(x, y, m_width, Height, EDColors.getColor(EDColors.ORANGE, 0.1f));
            Add(m_background);
            Add(new NxRectangle(x, y, m_width, 1, EDColors.YELLOW));
            Add(new NxRectangle(x, y+ Height-1, m_width, 1, EDColors.getColor( EDColors.YELLOW,0.5f) ));

            searchName = new NxSimpleText(x + (m_width / 2), y + (Height / 2), "empty", EDColors.getColor( EDColors.WHITE, 0.2f) );
            searchName.centerHorizontal = true;
            searchName.centerVertical = true;
            Add(searchName);
        }
        public override void Update()
        {
            base.Update();
            if( searchName.text != "empty" )
            {
                if (Selected)
                    searchName.Color = EDColors.WHITE;
                else
                    searchName.Color = EDColors.YELLOW;
            }
            m_background.Color = Selected ? EDColors.getColor(EDColors.ORANGE, 0.8f): EDColors.getColor(EDColors.ORANGE,0.1f) ;
        }
    }

    public class NxMainPanelSearchResult : NxGroup
    {
        int _ty = 0;
        int _textSize = 21;

        private bool m_isTitle = false;
        private NxRectangle m_background;

        private NxSimpleText m_Distance;
        private NxSimpleText m_SystemName;

        private NxSimpleText[] m_Properties;
        private NxRectangle[] m_PropertiesSeparators;


        public NxSimpleText Distance { get => m_Distance; set => m_Distance = value; }
        public NxSimpleText SystemName { get => m_SystemName; set => m_SystemName = value; }
        public NxRectangle Background { get => m_background; set => m_background = value; }


        public bool isSelected { get; set; }

        public NxMainPanelSearchResult( int _y, bool _isTitle, NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_isTitle = _isTitle;
            y = _y;
            int _height = 30;
            m_background = new NxRectangle(x, y, NxMenu.Width, _height,  m_isTitle ? EDColors.getColor(EDColors.WHITE, 0.1f) : EDColors.getColor(EDColors.ORANGE, 0.1f));
            Add(m_background);
            //Texts
            int _ty = y+5;
            int _textSize = 21;
            Color _textColor = m_isTitle ? EDColors.getColor(EDColors.WHITE, 0.4f) : EDColors.YELLOW;
            //m_Distance
            m_Distance = new NxSimpleText(x+5, _ty, "Dist.", _textColor, _textSize);
            Add(m_Distance);
            //m_SystemName
            m_SystemName = new NxSimpleText(x+100, _ty, "System Name", _textColor, _textSize);
            Add(m_SystemName);
            Add(new NxRectangle(m_SystemName.x-5, y, 1, _height, Color.Black));


            m_Properties = new NxSimpleText[NxSearchEntry.MAX_PARAMS_DISPLAY];
            m_PropertiesSeparators = new NxRectangle[NxSearchEntry.MAX_PARAMS_DISPLAY];
            for (int i = 0; i < NxSearchEntry.MAX_PARAMS_DISPLAY; i++)
            {
                m_Properties[i] = new NxSimpleText(x + 300*i, _ty, "Propertie "+(i+1).ToString(), _textColor, _textSize);
                m_PropertiesSeparators[i] = new NxRectangle(x + 300 * i - 5, y, 1, _height, Color.Black);
                Add(m_Properties[i]);
                Add(m_PropertiesSeparators[i]);
                m_Properties[i].isVisible = false;
                m_PropertiesSeparators[i].isVisible = false;
            }

        }

        public void onSearchResult(NxSearchDisplay[] _displays, EDSystem _system)
        {
            int _x = x + 370;
            for(int i = 0; i < m_Properties.Length; i++)
            {
                if( i < _displays.Length)
                {
                    m_Properties[i].isVisible = true;
                    m_PropertiesSeparators[i].isVisible = true;
                    if(m_isTitle)
                    {
                        m_Properties[i].text = _displays[i].ToStringFormated();
                    }
                    else
                    {
                        m_Properties[i].Color = EDColors.BLUE;
                        switch (_displays[i])
                        {
                            case NxSearchDisplay.allegiance: m_Properties[i].text = _system.allegiance.ToStringFormated(); break;
                            case NxSearchDisplay.nameNotes: m_Properties[i].text = _system.Notes; break;
                            case NxSearchDisplay.government: m_Properties[i].text = _system.government.ToStringFormated(); break;
                            case NxSearchDisplay.state: m_Properties[i].text = _system.state.ToStringFormated(); break;
                            case NxSearchDisplay.economy: m_Properties[i].text = _system.economy.ToStringFormated(); break;
                            case NxSearchDisplay.reserve: m_Properties[i].text = _system.reserve.ToStringFormated(); break;
                            case NxSearchDisplay.security:
                                m_Properties[i].text = _system.security.ToStringFormated();
                                m_Properties[i].Color = EliteSecurityHelper.getColor(_system.security);
                                break;
                            case NxSearchDisplay.Unexpected:
                                m_Properties[i].text = _displays[i].ToStringFormated();
                                m_Properties[i].Color = EDColors.RED;
                                break;
                            case NxSearchDisplay.secondEconomy:
                                m_Properties[i].text = _system.secondEconomy.ToStringFormated(); 
                                break;
                            case NxSearchDisplay.population:
                                m_Properties[i].text = String.Format("{0:#,##0}", _system.population);
                                break;
                        }
                    }
                    m_Properties[i].x = _x;
                    m_PropertiesSeparators[i].x = _x - 5;
                    _x += getWidthFor(_displays[i]);

                }
                else
                {
                    m_Properties[i].isVisible = false;
                    m_PropertiesSeparators[i].isVisible = false;
                }
            }
        }

        public override void Update()
        {
            base.Update();
            if(!m_isTitle)
            {
                m_background.Color = isSelected ? EDColors.getColor(EDColors.ORANGE, 0.3f) : EDColors.getColor(EDColors.ORANGE, 0.1f);
                m_SystemName.Color = isSelected ? EDColors.WHITE : EDColors.YELLOW;
            }
        }
        private int getWidthFor(NxSearchDisplay _sp)
        {
            switch (_sp)
            {
                case NxSearchDisplay.Unexpected:
                    return 60;
                case NxSearchDisplay.nameNotes:
                    return 300;
                case NxSearchDisplay.allegiance:
                    return 125;
                case NxSearchDisplay.government:
                    return 120;
                case NxSearchDisplay.state:
                    return 120;
                case NxSearchDisplay.economy:
                case NxSearchDisplay.secondEconomy:
                    return 100;
                case NxSearchDisplay.reserve:
                    return 90;
                case NxSearchDisplay.security:
                    return 95;
                case NxSearchDisplay.population:
                    return 90;
            }
            return 100;
        }
    }
    public class NxMainPanelSearch : NxGroup
    {
        public const int MAX_LINE_RESULT = 11;
        NxMenu m_menu;
        /* Cursor */
        private int m_CursorX = 0;
        private int m_CursorY = 0;
        private int m_CursorMaxX = 0;
        private int m_CursorMaxY = 0;

        /* Top Section */
        NxMainPanelSearchButton[] m_buttons;
        private int m_buttonPerRow = 6;
        private int m_buttonRow = 2;

        /* Search Result */
        private NxMainPanelSearchResult[] m_searchResults;
        private NxLoading m_loading;

        private NxMainPanelSearchResult m_searchTitles;


        private float m_messageLifeTime;
        private NxSimpleText m_messageInfo;

        private uint _currentSearchId = 0;

        public NxMainPanelSearch(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_menu = _menu;
            //Title
            Add(new NxSimpleText(0, NxMainPanelTopInfos.HEIGHT, "Search...", EDColors.ORANGE, 24, NxFonts.EuroCapital));

            //Initialize buttons           
            int _buttonId = 0;
            int _buttonSpacing = 5;
            int _by = 0;
            m_buttons = new NxMainPanelSearchButton[m_buttonPerRow * m_buttonRow];

            m_CursorMaxX = m_buttonPerRow - 1;
            m_CursorMaxY = m_buttonRow - 1;

            int _buttonWidth = (NxMenu.Width - (m_buttonPerRow + 1) * _buttonSpacing) / m_buttonPerRow;
            for (int a = 0; a < m_buttonRow; a++)
            {
                for (int b = 0; b < m_buttonPerRow; b++)
                {
                    int _bx = _buttonSpacing + b * _buttonSpacing + b * _buttonWidth;
                    _by = (a * _buttonSpacing) + (a * NxMainPanelSearchButton.Height) + NxMainPanelTopInfos.HEIGHT + 30;
                    m_buttons[_buttonId] = new NxMainPanelSearchButton(_bx, _by, _buttonWidth, m_menu);
                    Add(m_buttons[_buttonId]);

                    if (_buttonId < UserSearchs.userSearchEntrys.Length)
                        m_buttons[_buttonId].searchName.text = UserSearchs.userSearchEntrys[_buttonId].searchName;

                    _buttonId++;

                }
            }
            //Title
            _by += 30;
            Add(new NxSimpleText(0, _by, "Search Result...", EDColors.ORANGE, 24, NxFonts.EuroCapital));
            _by += 30;
            m_searchTitles = new NxMainPanelSearchResult(_by, true, m_menu);
            Add(m_searchTitles);//Categorie description
            _by += 34;
            //Initialize Search result
            m_searchResults = new NxMainPanelSearchResult[MAX_LINE_RESULT];
            for (int i = 0; i < m_searchResults.Length; i++)
            {
                m_searchResults[i] = new NxMainPanelSearchResult(_by, false, m_menu);
                Add(m_searchResults[i]);
                _by += 32;
                m_searchResults[i].isVisible = false;
            }

            //Loading

            m_loading = new NxLoading(NxMenu.Width / 2, 500);
            Add(m_loading);

            //Message

            m_messageInfo = new NxSimpleText(10, NxMenu.Height - 35, "", Color.CadetBlue, 22);
            Add(m_messageInfo);
        }

        private bool m_firstUpdateSkipped = false; //To avoid insta click from the previous panel

        public void displayMessage(string _text, Color _c)
        {
            m_messageInfo.Color = _c;
            m_messageInfo.text = _text;
            m_messageLifeTime = 0;
        }

        private float _updateMessageResearchDelay = 0;
        public override void Update()
        {
            base.Update();
            _updateMessageResearchDelay += SteamVR_NexHUD.deltaTime;
            if (m_messageLifeTime < 10)
                m_messageLifeTime += SteamVR_NexHUD.deltaTime;
            if (isVisible)
            {
                //Message display
                if (m_messageLifeTime < 5)
                {
                    m_messageInfo.isVisible = true;
                    if (m_messageLifeTime > 4)
                        m_messageInfo.Color = EDColors.getColor(m_messageInfo.Color, 5 - m_messageLifeTime);
                }
                else
                {
                    m_messageInfo.isVisible = false;
                }
                if (!m_firstUpdateSkipped)
                {
                    m_firstUpdateSkipped = true;
                    return;
                }
                if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.right)) && m_CursorX < m_CursorMaxX)
                    m_CursorX++;
                if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.left)) && m_CursorX > 0)
                    m_CursorX--;
                if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.down)) && m_CursorY < m_CursorMaxY)
                    m_CursorY++;
                if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.up) ) && m_CursorY > 0)
                    m_CursorY--;

                int _buttonSelectedId = m_CursorX + (m_CursorY * m_buttonPerRow);
                NxSearchEntry _searchSelected = null;
                for (int i = 0; i < m_buttons.Length; i++)
                {
                    if (i == _buttonSelectedId)
                    {
                        m_buttons[i].Selected = true;
                        if (i < UserSearchs.userSearchEntrys.Length)
                            _searchSelected = UserSearchs.userSearchEntrys[i];
                    }
                    else
                        m_buttons[i].Selected = false;
                }


                UserSearchResult _lastUSR = EDDatas.Instance.getUserSearchResult(_currentSearchId);

                if (_lastUSR != null && !_lastUSR.isDone)
                {

                    m_loading.isVisible = true;
                }
                else
                {
                    m_loading.isVisible = false;
                }

                EDSystem _systemSelected = null;
                if (_lastUSR != null)
                {
                    if (_updateMessageResearchDelay > 1)
                    {
                        if(_lastUSR.CurrentPass > 1 )
                        {
                            if (!_lastUSR.isDone)
                            {
                                displayMessage(string.Format("Research pass {0}. Extending radius to {1}. {2}ms elapsed",
                                    _lastUSR.CurrentPass,
                                    _lastUSR.CurrentRadius,
                                    _lastUSR.ResearchTime
                                    ), EDColors.ORANGE);
                            }
                            else if(!_lastUSR.messageDisplayed)
                            {
                                displayMessage(string.Format("Research done in {0}ms!", _lastUSR.ResearchTime), EDColors.BLUE);
                                _lastUSR.messageDisplayed = true;
                            }
                        }
                    }
                    m_searchTitles.onSearchResult(_lastUSR.displays, null);
                    List<EDSystem> _systems = _lastUSR.getSystemByDist();
                  
                    m_CursorMaxY = m_buttonRow - 1 + Math.Min( _systems.Count, MAX_LINE_RESULT );

                    int _lineSelected = m_CursorY - (m_buttonRow );

                    m_loading.y = m_searchResults[0].y + 60;
                    for (int i = 0; i < m_searchResults.Length; i++)
                    {
                        if (_systems != null && i < _systems.Count)
                        {
                            m_searchResults[i].isVisible = true;
                            m_searchResults[i].Distance.text = _systems[i].distanceFromCurrentSystem.ToString("#0.0") + "Ly";
                            m_searchResults[i].SystemName.text = _systems[i].name;

                            m_searchResults[i].onSearchResult(_lastUSR.displays, _systems[i]);
                            m_searchResults[i].isSelected = _lineSelected == i;
                            if (m_searchResults[i].isSelected)
                                _systemSelected = _systems[i];

                            m_loading.y = m_searchResults[i].y + 60;
                        }
                        else
                        {
                            m_searchResults[i].isVisible = false;
                        }
                    }
                }
                else
                    m_CursorMaxY = m_buttonRow - 1;


                if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.select) ))
                {
                    if (_searchSelected != null)
                    {
                        _currentSearchId = EDDatas.Instance.processUserSearch(_searchSelected);
                    }
                    else if (_systemSelected != null)
                    {
                        displayMessage(string.Format("{0} has been copied to your clipboard!", _systemSelected.name.ToString()), EDColors.YELLOW);
                        Clipboard.SetText(_systemSelected.name.ToString());
                    }
                }


                if (m_CursorY > m_CursorMaxY)
                    m_CursorY = m_CursorMaxY;

            }
            else
            {
                m_firstUpdateSkipped = false;
            }
            if (_updateMessageResearchDelay > 1)
                _updateMessageResearchDelay = 0;

        }
    
       

       
    }
}