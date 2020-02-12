using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.Inputs;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Search
{
    public class UiSearchBookmarks : NxGroup
    {
        UiSearch2 m_UiSearch;
        public UiSearchBookmarks(UiSearch2 _UiSearch) : base(_UiSearch.Menu.frame.NxOverlay)
        {
            m_UiSearch = _UiSearch;
            Add(new NxSimpleText(50,200,"Search bookmarks placeholder", EDColors.ORANGE, 24));
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

            if (Shortcuts.SelectPressed)
                m_UiSearch.changeState(UiSearch2.State.Create);

        }
    }
}
