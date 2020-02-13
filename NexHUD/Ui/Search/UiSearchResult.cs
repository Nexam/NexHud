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
using System.Threading.Tasks;

namespace NexHUD.Ui.Search
{
    public class UiSearchResult : NxGroupAutoCursor
    {
        public const int MAX_LINE_RESULT = 13;

        private UiSearch2 m_UiSearch;

        private NxSimpleText m_title;
        private NxSimpleText m_messageInfo;
        private NxLoading m_loading;
        private UiSearchResultLine[] m_results;

        private float m_messageLifeTime;

        public int[] ResultPosX;

        private UiSearch2.State m_PreviousState = UiSearch2.State.Create;

        private NxButton m_BtnDelete;
        private NxButton m_BtnSave;
        private CustomSearch m_LastSearch;
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

            m_messageInfo = new NxSimpleText(10, height - 160, "", Color.CadetBlue, 20);

            Add(m_messageInfo);

            //results

            ResultPosX = new int[UiSearchResultLine.PropertiesCount];
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

                m_results[i].onClick += OnClickResult;
                Add(m_results[i]);
            }

            MoveCursorToFirst();

            m_BtnSave = new NxButton(width / 2 + 5, height - 135, width / 2 - 10, 35, "Save search", m_UiSearch.Menu);
            m_BtnSave.ColorBack = EDColors.getColor(EDColors.GREEN, 0.1f);
            m_BtnSave.ColorBackSelected = EDColors.getColor(EDColors.GREEN, 0.8f);
            m_BtnSave.onClick += onSaveClicked;
            Add(m_BtnSave);

            m_BtnDelete = new NxButton(5, height - 135, width / 2 - 10, 35, "Delete search", m_UiSearch.Menu);
            m_BtnDelete.ColorBack = EDColors.getColor(EDColors.RED, 0.1f);
            m_BtnDelete.ColorBackSelected = EDColors.getColor(EDColors.RED, 0.8f);
            m_BtnDelete.onClick += onDeleteClicked;
            Add(m_BtnDelete);
        }

        private void onDeleteClicked(object sender, EventArgs e)
        {
           if( m_LastSearch != null && m_PreviousState == UiSearch2.State.Bookmarks)
            {
                Bookmarks.Delete(m_LastSearch);
                m_UiSearch.changeState(UiSearch2.State.Bookmarks);
                m_UiSearch.UiBookmarks.refreshCards();
            }
        }

        private void onSaveClicked(object sender, EventArgs e)
        {
            if( m_LastSearch != null && m_PreviousState == UiSearch2.State.Create)
            {
                Bookmarks.Save(m_LastSearch);
                m_UiSearch.changeState(UiSearch2.State.Bookmarks);
                m_UiSearch.UiBookmarks.refreshCards();
            }
        }

        private void OnClickResult(object sender, EventArgs e)
        {
            if (sender is UiSearchResultLine)
            {
                SpanshSystem system = ((UiSearchResultLine)sender).LastSystem;
                if (system != null)
                {
                    displayMessage(string.Format("{0} has been copied to your clipboard!", system.name.ToString()), EDColors.BLUE);
                    Clipboard.SetText(system.name.ToString());
                }
            }
        }

        public void displayMessage(string _text, Color _c)
        {
            m_messageInfo.Color = _c;
            m_messageInfo.text = _text;
            m_messageLifeTime = 0;
        }


        public void processSearch(CustomSearch _search, UiSearch2.State _previous)
        {
            m_PreviousState = _previous;
            m_LastSearch = _search;
            m_title.text = "Search : " + _search.SearchName;

            m_BtnDelete.isVisible = m_PreviousState == UiSearch2.State.Bookmarks;
            m_BtnSave.isVisible = m_PreviousState == UiSearch2.State.Create;

            m_loading.isVisible = true;
            displayMessage(string.Format("Process search: '{0}'", _search.SearchName), EDColors.YELLOW);
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
            displayMessage("Search Succeeded!", EDColors.GREEN);
        }


        private void _onSystemsReceived(SpanshSystemsResult obj)
        {
            m_loading.isVisible = false;
            displayMessage("Search Succedded!", EDColors.GREEN);

            for (int i = 0; i < ResultPosX.Length; i++)
                ResultPosX[i] = 0;

            for (int i = 0; i < m_results.Length; i++)
            {
                if (i < obj.results.Length)
                {
                    m_results[i].isVisible = true;
                    m_results[i].SetDatas(obj, i - 1);
                    for (int j = 0; j < ResultPosX.Length; j++)
                        ResultPosX[j] = Math.Max(ResultPosX[j], m_results[i].XPos[j]);
                }
                else
                    m_results[i].isVisible = false;
            }

            for (int i = 0; i < m_results.Length; i++)
                m_results[i].setPositions(ResultPosX);

            MoveCursorToFirst();
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

            if (m_messageLifeTime < 10)
                m_messageLifeTime += NexHudEngine.deltaTime;

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


            if (Shortcuts.BackPressed)
            {
                m_UiSearch.changeState(m_PreviousState);
                return;
            }

            if (Shortcuts.UpPressed) moveUp();
            if (Shortcuts.DownPressed) moveDown();
            if (Shortcuts.LeftPressed) moveLeft();
            if (Shortcuts.RightPressed) moveRight();
        }
    }
}
