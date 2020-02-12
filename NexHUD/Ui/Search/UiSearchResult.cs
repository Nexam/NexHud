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
using NexHUD.Ui.Common;

namespace NexHUD.Ui.Search
{
    public class UiSearchResult : NxGroupAutoCursor
    {
        public const int MAX_LINE_RESULT = 11;

        private UiSearch2 m_UiSearch;

        private bool m_firstUpdateSkipped = false; //To avoid insta click from the previous panel
        private NxSimpleText m_title;
        private NxSimpleText m_messageInfo;
        private NxLoading m_loading;
        private UiSearchResultLine[] m_results;

        private float m_messageLifeTime;
        private float m_updateMessageFreq = 0;

        public int[] ResultPosX;

        public UiSearch2.State PreviousState = UiSearch2.State.Create;

        public UiSearchResult(UiSearch2 _search) : base(_search.Menu.frame.NxOverlay)
        {
            m_UiSearch = _search;
            RelativeChildPos = true;
            //Title
            m_title = new NxSimpleText(0, 0, "Search result...", EDColors.ORANGE, 24, NxFonts.EuroCapital);
            Add(m_title);

            int _by = 0;

            width = NxMenu.Width;
            height = NxMenu.Height - y;
            //Bookmark buttons

            //Tips for materials

            //Title
            _by += 40;

            //Loading

            m_loading = new NxLoading(NxMenu.Width / 2, 500);
            Add(m_loading);

            //Message

            m_messageInfo = new NxSimpleText(10, NxMenu.Height - 35, "", Color.CadetBlue, 22);

            Add(m_messageInfo);

            //results

            ResultPosX = new int[12];
            m_results = new UiSearchResultLine[MAX_LINE_RESULT];
            for (int i = 0; i < m_results.Length; i++)
            {
                m_results[i] = new UiSearchResultLine(this)
                {
                    x = 0,
                    y = _by,
                    width = NxMenu.Width,
                    height = 30
                };
                _by += m_results[i].height + 2;
                Add(m_results[i]);
            }

            MoveCursorToFirst();
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

            for (int i = 0; i < ResultPosX.Length; i++)
                ResultPosX[i] = 0;

            for (int i = 0; i < m_results.Length; i++)
            {
                if (i < obj.results.Length)
                {
                    m_results[i].SetDatas(obj, i - 1);
                    for (int j = 0; j < ResultPosX.Length; j++)
                        ResultPosX[j] = Math.Max(ResultPosX[j], m_results[i].XPos[j]);
                }
                else
                    m_results[i].isVisible = false;
            }

            for (int i = 0; i < m_results.Length; i++)
                m_results[i].setPositions(ResultPosX);

        }

        private void _onSearchFailed(SearchEngine.SearchError obj)
        {
            m_loading.isVisible = false;
            displayMessage("Search failed: " + obj.ToString(), EDColors.RED);
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

            if (Shortcuts.BackPressed) m_UiSearch.changeState(PreviousState);

            if (Shortcuts.UpPressed) moveUp();
            if (Shortcuts.DownPressed) moveDown();
            if (Shortcuts.LeftPressed) moveLeft();
            if (Shortcuts.RightPressed) moveRight();
        }
    }
}
