
using NexHUD.Apis.Spansh;
using NexHUD.Inputs;
using NexHUD.Ui.Common;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Search
{
    public class UiSearchCreate : NxGroupAutoCursor
    {
        public UiSearchCreate(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            //10 params max
            /*Allegiance
            Economy
            Government
            Needs Permit
            Population
            100000000000
            Power
            Power State
            Security
            State
            Secondary Economy*/
           // InitGrid(8, 20);
            x = 5;
            y = 90;
            width = NxMenu.Width - 10;
            height = NxMenu.Height - 90 - 5;
            RelativeChildPos = true;

           
            int _cx = 0;
            int _cy = 0;


            int bsize = 150;
            int bmargin = 2;
            //Allegiance
            Add(new NxSimpleText(0, 0, "Allegiance", EDColors.ORANGE, 18, NxFonts.EuroCapital));
            foreach (string s in SpanshDatas.allegiance)
            {
                NxButton b = new NxButton(_cx * (bsize + bmargin), _cy + 20, bsize, 30, s, _menu);
                b.Coords = new System.Drawing.Point(_cx, _cy);
                Add(b);
                if (_cx == 0)
                    moveCursorTo(b);
                _cx++;
            }
            Add(new NxSimpleText(_cx * (bsize + bmargin)+5, 0, "Need a permit", EDColors.ORANGE, 18, NxFonts.EuroCapital));
            for(int i = 0; i < 2; i++)
            {
                NxButton b = new NxButton(_cx * (bsize + bmargin)+5, _cy + 20, bsize /2, 30, i == 0 ? "Yes": "No", _menu);
                b.Coords = new System.Drawing.Point(_cx, _cy);
                Add(b);
                _cx++;
            }

            bsize = 120;
            _cx = 0;
            _cy++;
            //Economy
            Add(new NxSimpleText(0, 55, "Economy", EDColors.ORANGE, 18, NxFonts.EuroCapital));
            foreach (string s in SpanshDatas.economy)
            {
                NxButton b = new NxButton(_cx * (bsize + bmargin), _cy * 32 + 50, bsize, 30, s, _menu);
                b.Coords = new System.Drawing.Point(_cx, _cy);
                Add(b);
                _cx++;
                if (_cx >= 7)
                {
                    _cx = 0;
                    _cy++;
                }
            }
            //Governement
            _cx = 0;
            _cy++;
            Add(new NxSimpleText(0, 165, "Government", EDColors.ORANGE, 18, NxFonts.EuroCapital));
            foreach (string s in SpanshDatas.government)
            {
                NxButton b = new NxButton(_cx * (bsize + bmargin), _cy * 32 + 160, bsize, 30, s, _menu);
                b.Coords = new System.Drawing.Point(_cx, _cy);
                Add(b);
                _cx++;
                if (_cx >= 7)
                {
                    _cx = 0;
                    _cy++;
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

            bool up = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.up));
            bool down = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.down));
            bool left = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.left));
            bool right = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.right));
            bool select = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select));

            if (up) moveUp();
            if (down) moveDown();
            if (left) moveLeft();
            if (right) moveRight();
        }
    }
}
