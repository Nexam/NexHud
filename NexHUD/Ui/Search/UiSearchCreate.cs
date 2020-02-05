
using NexHUD.Apis.Spansh;
using NexHUD.Ui.Common;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Search
{
    public class UiSearchCreate : NxGroupOrganized
    {
        public UiSearchCreate(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            InitGrid(20, 20);
            x = 5;
            y = 90;
            RelativeChildPos = true;

            Add(new NxSimpleText(0, 0, "Allegiance", EDColors.ORANGE, 18, NxFonts.EuroCapital));
            int _cx = 0;
            int _cy = 0;

            int bsize = 90;
            int bmargin = 2;
            //Allegiance
            foreach(string s in SpanshDatas.allegiance)
            {
                NxButton b = new NxButton(_cx*(bsize+bsize), _cy+20, bsize, 30, s, _menu);
                b.Coords = new System.Drawing.Point(_cx, _cy);
                Add(b);
                _cx++;
            }
            _cx = 0;
            _cy++;
            //Economy
            Add(new NxSimpleText(0, 55, "Economy", EDColors.ORANGE, 18, NxFonts.EuroCapital));
            foreach (string s in SpanshDatas.economy)
            {
                NxButton b = new NxButton(_cx * (bsize + bsize), _cy + 75, bsize, 30, s, _menu);
                b.Coords = new System.Drawing.Point(_cx, _cy);
                Add(b);
                _cx++;
            }
        }
    }
}
