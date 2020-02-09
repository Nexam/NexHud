using NexHUD.Elite.Engineers;
using NexHUD.Elite;
using NexHUDCore;
using NexHUDCore.NxItems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NexHUD.Inputs;
using NexHUD.Apis.Spansh;
using NexHUD.Elite.Searchs;

namespace NexHUD.Ui.Search
{
    public class UiSearchNew : NxGroup
    {
        public const int MAX_LINE_RESULT = 11;
        NxMenu m_menu;

        private bool m_firstUpdateSkipped = false; //To avoid insta click from the previous panel
        private NxSimpleText m_title;
        private NxSimpleText m_messageInfo;
        private NxLoading m_loading;

        private float m_messageLifeTime;
        private float m_updateMessageFreq = 0;

        private SearchPanelType m_searchPanelType;
        public enum SearchPanelType
        {
            Bookmarks,
            QuickSearch,
            Materials
        }

        public UiSearchNew(NxMenu _menu, SearchPanelType _searchPanelType = SearchPanelType.Bookmarks) : base(_menu.frame.NxOverlay)
        {
            m_menu = _menu;
            m_searchPanelType = _searchPanelType;
            //Title
            m_title = new NxSimpleText(0, UiMainTopInfos.HEIGHT, m_searchPanelType.ToString() + "...", EDColors.ORANGE, 24, NxFonts.EuroCapital);
            Add(m_title);

            int _by = 0;
            //Bookmark buttons

            //Tips for materials

            //Title
            _by += 30;
            Add(new NxSimpleText(0, _by, "Search Result...", EDColors.ORANGE, 24, NxFonts.EuroCapital));
            _by += 30;

            //Loading

            m_loading = new NxLoading(NxMenu.Width / 2, 500);
            Add(m_loading);

            //Message

            m_messageInfo = new NxSimpleText(10, NxMenu.Height - 35, "", Color.CadetBlue, 22);
            Add(m_messageInfo);
        }


        public void displayMessage(string _text, Color _c)
        {
            m_messageInfo.Color = _c;
            m_messageInfo.text = _text;
            m_messageLifeTime = 0;
        }


        public void processSearch(CustomSearch _search)
        {
            m_title.text = "Search : " + _search.SearchName;
            m_loading.isVisible = true;
            if (_search.SearchSystem != null)
            {
                SearchEngine.Instance.SearchInSystems(_search.SearchSystem, _onSystemsReceived, _onSearchFailed);
            }
            else if (_search.SearchBodies != null)
            {
                SearchEngine.Instance.SearchInBodies(_search.SearchBodies, _onBodiesReceived, _onSearchFailed);
            }
        }

        private void _onBodiesReceived(SpanshBodiesResult obj)
        {
            m_loading.isVisible = false;
            displayMessage("Search Succeeded!", EDColors.RED);
        }


        private void _onSystemsReceived(SpanshSystemsResult obj)
        {
            m_loading.isVisible = false;
            displayMessage("Search Succedded!", EDColors.RED);
        }

        private void _onSearchFailed(SearchEngine.SearchError obj)
        {
            m_loading.isVisible = false;
            displayMessage("Search failed: " + obj.ToString(), EDColors.RED);
        }

        public override void Update()
        {
            base.Update();
            m_updateMessageFreq += NexHudEngine.deltaTime;
            if (m_messageLifeTime < 10)
                m_messageLifeTime += NexHudEngine.deltaTime;
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

            }
            else
            {
                m_firstUpdateSkipped = false;
            }
            if (m_updateMessageFreq > 1)
                m_updateMessageFreq = 0;

        }




    }
}
