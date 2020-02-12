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
        public UiSearchBookmarkCard(NxOverlay _parent) : base(_parent)
        {
            RelativeChildPos = true;
            width = 250;
            height = 120;

            m_type = new NxSimpleText(2, 2, "", EDColors.YELLOW, 18, NxFonts.EuroCapital);
            m_content = new NxSimpleText[10];
            for(int i = 0; i < m_content.Length; i++)
            {
                m_content[i] = new NxSimpleText(2, 20+i*20, "content", EDColors.ORANGE);
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
                m_type.text = "System search";
            else if (search.SearchBodies != null)
                m_type.text = "Body search";

            setPositions();
        }

        private void setPositions()
        {
            int _x = 5;
            int _y = 30;
            foreach( NxSimpleText text in m_content)
            {
                if( _x + text.width+3 > width)
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
                _x += text.width+3;
                
            }
        }
        private string getSubstring( string s)
        {
            if (s.Length > 13)
                s = s.Substring(0, 10) + "...";
            return s;
        }
        public override void Update()
        {
            base.Update();
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
                if( text.isVisible )
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
            Add(new NxSimpleText(50,200,"Search bookmarks placeholder", EDColors.ORANGE, 24));

            for(int i = 0; i < 4; i++ )
            {
                for(int u = 0; u < 4; u++)
                {
                    UiSearchBookmarkCard card = new UiSearchBookmarkCard(_UiSearch.Menu.frame.NxOverlay);
                    card.x = 5+u * 253;
                    card.y = i * 123;
                    m_Cards.Add(card);
                    Add(card);
                }
            }

            for(int i = 0; i < Bookmarks.Searchs.Count && i < m_Cards.Count; i++)
            {
                m_Cards[i].setDatas(Bookmarks.Searchs[i]);
            }
            MoveCursorToFirst();
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
            if (Shortcuts.SelectPressed)
                m_UiSearch.changeState(UiSearch2.State.Create);

            if (Shortcuts.UpPressed) moveUp();
            if (Shortcuts.DownPressed) moveDown();
            if (Shortcuts.LeftPressed) moveLeft();
            if (Shortcuts.RightPressed) moveRight();
        }
    }
}
