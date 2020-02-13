using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.Elite;
using NexHUD.Elite.Searchs;
using NexHUD.Inputs;
using NexHUD.Ui.Common;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Search
{
    public class UiSearchBookmarkCard : NxGroup, ISelectable
    {
        private bool m_Selected = false;
        private bool m_isSelectable = true;
        private bool m_isEnable = true;
        private NxSimpleText m_type;
        private NxSimpleText[] m_content;

        public CustomSearch Search;
        public EventHandler onClick;

        public UiSearchBookmarkCard(NxOverlay _parent) : base(_parent)
        {
            RelativeChildPos = true;
            width = 250;
            height = 120;

            m_type = new NxSimpleText(2, 2, "", EDColors.YELLOW, 18, NxFonts.EuroCapital);
            m_content = new NxSimpleText[10];
            for (int i = 0; i < m_content.Length; i++)
            {
                m_content[i] = new NxSimpleText(2, 20 + i * 20, "content", EDColors.ORANGE);
                m_content[i].Color = EDColors.LIGHTBLUE;
                Add(m_content[i]);
                m_content[i].isVisible = false;
            }

            Add(m_type);

        }

        public void setDatas(CustomSearch search)
        {
            Search = search;
            foreach (NxSimpleText text in m_content)
                text.isVisible = false;

            if (search == null)
            {
                m_type.text = "Empty";
                m_type.Color = EDColors.GRAY;
                return;
            }
            m_type.Color = EDColors.YELLOW;

            if (search.SystemsNotes != null)
            {
                m_type.text = search.SearchName;
                string[] names = search.SystemsNotes.Keys.ToArray();
                for (int i = 0; i < names.Length && i < m_content.Length; i++)
                {
                    m_content[i].text = getSubstring(names[i]);
                    m_content[i].isVisible = true;
                }
            }
            else if (search.SearchSystem != null)
            {
                m_type.text = "System search";

                int i = 0;

                if (search.SearchSystem.filters.allegiance != null)
                    foreach (string v in search.SearchSystem.filters.allegiance?.value)
                    {
                        m_content[i].text = getSubstring(v);
                        m_content[i].isVisible = true;
                        i++;
                        if (i >= m_content.Length) break;
                    }
                if (search.SearchSystem.filters.government != null)
                    foreach (string v in search.SearchSystem.filters.government?.value)
                    {
                        m_content[i].text = getSubstring(v);
                        m_content[i].isVisible = true;
                        i++;
                        if (i >= m_content.Length) break;
                    }
                if (search.SearchSystem.filters.power != null)
                    foreach (string v in search.SearchSystem.filters.power?.value)
                    {
                        m_content[i].text = getSubstring(v);
                        m_content[i].isVisible = true;
                        i++;
                        if (i >= m_content.Length) break;
                    }
                if (search.SearchSystem.filters.power_state != null)
                    foreach (string v in search.SearchSystem.filters.power_state?.value)
                    {
                        m_content[i].text = getSubstring(v);
                        m_content[i].isVisible = true;
                        i++;
                        if (i >= m_content.Length) break;
                    }
                if (search.SearchSystem.filters.primary_economy != null)
                    foreach (string v in search.SearchSystem.filters.primary_economy?.value)
                    {
                        m_content[i].text = getSubstring(v);
                        m_content[i].isVisible = true;
                        i++;
                        if (i >= m_content.Length) break;
                    }
                if (search.SearchSystem.filters.security != null)
                    foreach (string v in search.SearchSystem.filters.security?.value)
                    {
                        m_content[i].text = getSubstring(v);
                        m_content[i].isVisible = true;
                        i++;
                        if (i >= m_content.Length) break;
                    }
                if (search.SearchSystem.filters.state != null)
                    foreach (string v in search.SearchSystem.filters.state?.value)
                    {
                        m_content[i].text = getSubstring(v);
                        m_content[i].isVisible = true;
                        i++;
                        if (i >= m_content.Length) break;
                    }
            }
            else if (search.SearchBodies != null)
                m_type.text = "Body search";

            setPositions();
        }

        private void setPositions()
        {
            int _x = 5;
            int _y = 30;
            foreach (NxSimpleText text in m_content)
            {
                if (_x + text.width + 3 > width)
                {
                    _x = 5;
                    _y += 20;
                }
                if (_y + 20 > height)
                {
                    text.isVisible = false;
                    continue;
                }
                text.x = _x;
                text.y = _y;
                _x += text.width + 3;

            }
        }
        private string getSubstring(string s)
        {
            if (s.Length > 13)
                s = s.Substring(0, 10) + "...";
            return s;
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

            if (isVisible && Selected && isEnable && Shortcuts.SelectPressed)
            {
                onClick?.Invoke(this, new EventArgs());
            }
        }
        public override void Render(Graphics _g)
        {

            if (Selected)
            {
                _g.FillRectangle(new SolidBrush(EDColors.getColor(EDColors.ORANGE, 0.2f)), Rectangle);
                _g.DrawRectangle(new Pen(EDColors.ORANGE, 1), Rectangle);
            }
            else
                _g.FillRectangle(new SolidBrush(EDColors.getColor(EDColors.WHITE, 0.05f)), Rectangle);
            foreach (NxSimpleText text in m_content)
            {
                if (text.isVisible)
                    _g.FillRectangle(new SolidBrush(EDColors.getColor(EDColors.BLUE, 0.4f)), text.Rectangle);
            }
            base.Render(_g);
        }

        public bool Selected { get => m_Selected; set { if (value != Selected) { m_Selected = value; makeItDirty(); } } }
        public bool isSelectable { get => m_isSelectable; set => m_isSelectable = value; }
        public bool isEnable { get => m_isEnable; set => m_isEnable = value; }
    }
    public class UiSearchBookmarks : NxGroupAutoCursor
    {
        UiSearch2 m_UiSearch;
        List<UiSearchBookmarkCard> m_Cards = new List<UiSearchBookmarkCard>();
        public UiSearchBookmarks(UiSearch2 _UiSearch) : base(_UiSearch.Menu.frame.NxOverlay)
        {
            m_UiSearch = _UiSearch;
            RelativeChildPos = true;
            y = 90;
            width = NxMenu.Width;
            height = NxMenu.Height - 90;

            for (int i = 0; i < 4; i++)
            {
                for (int u = 0; u < 4; u++)
                {
                    UiSearchBookmarkCard card = new UiSearchBookmarkCard(_UiSearch.Menu.frame.NxOverlay);
                    card.x = 5 + u * 253;
                    card.y = i * 123;
                    m_Cards.Add(card);
                    Add(card);
                }
            }

            NxButton btnAdd = new NxButton(5, height - 50, width - 10, 40, "Create search // Quick search", m_UiSearch.Menu);
            btnAdd.onClick += OnClickAdd;
            Add(btnAdd);
            MoveCursorToFirst();


            refreshCards();
        }
        public void refreshCards()
        {
            foreach (UiSearchBookmarkCard card in m_Cards)
                card.setDatas(null);
            for (int i = 0; i < Bookmarks.Searchs.Count && i < m_Cards.Count; i++)
            {

                m_Cards[i].setDatas(Bookmarks.Searchs[i]);
                m_Cards[i].onClick += OnClickBookmark;
            }
        }
        private void OnClickAdd(object sender, EventArgs e)
        {
            m_UiSearch.changeState(UiSearch2.State.Create);
        }

        private void OnClickBookmark(object sender, EventArgs e)
        {
            if (sender is UiSearchBookmarkCard)
            {
                UiSearchBookmarkCard card = (UiSearchBookmarkCard)sender;
                if (card.Search != null)
                {
                    m_UiSearch.changeState(UiSearch2.State.SearchResult);
                    m_UiSearch.UiSearchResult.processSearch(card.Search, UiSearch2.State.Bookmarks);
                }
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

            if (Shortcuts.BackPressed)
            {
                m_UiSearch.Menu.changeState(NxMenu.MenuState.Main);
                return;
            }

            if (Shortcuts.UpPressed) moveUp();
            if (Shortcuts.DownPressed) moveDown();
            if (Shortcuts.LeftPressed) moveLeft();
            if (Shortcuts.RightPressed) moveRight();
        }
    }
}
