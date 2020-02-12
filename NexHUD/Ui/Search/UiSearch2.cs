using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Search
{
    public class UiSearch2 : NxGroup
    {
        public enum State
        {
            Bookmarks,
            Create,
            SearchResult,
        }

        NxMenu m_menu;
        State m_state;
        UiSearchBookmarks m_UiBookmarks;
        UiSearchCreate m_UiSearchCreate;
        UiSearchResult m_UiSearchResult;

        public NxMenu Menu { get => m_menu; set => m_menu = value; }
        public UiSearchBookmarks UiBookmarks { get => m_UiBookmarks; set => m_UiBookmarks = value; }
        public UiSearchCreate UiSearchCreate { get => m_UiSearchCreate; set => m_UiSearchCreate = value; }
        public UiSearchResult UiSearchResult { get => m_UiSearchResult; set => m_UiSearchResult = value; }

        public UiSearch2( NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_menu = _menu;
            int overlayY = 80;

            RelativeChildPos = true;

            Add(m_UiBookmarks = new UiSearchBookmarks(this) { y = overlayY });
            Add(m_UiSearchCreate = new UiSearchCreate(this) { y = overlayY });
            Add(m_UiSearchResult = new UiSearchResult(this) { y = overlayY });

            changeState(State.Bookmarks);
        }

        public void changeState(State _newState)
        {
            m_state = _newState;

            m_UiBookmarks.isVisible = false;
            m_UiSearchCreate.isVisible = false;
            m_UiSearchResult.isVisible = false;
           
            switch (m_state)
            {
                case State.Bookmarks:
                    m_UiBookmarks.isVisible = true;
                    break;
                case State.Create:
                    m_UiSearchCreate.isVisible = true;
                    break;
                case State.SearchResult:
                    m_UiSearchResult.isVisible = true;
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

        }
    }
}
