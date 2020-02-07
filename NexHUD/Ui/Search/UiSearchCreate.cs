
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NexHUD.Apis.Spansh;
using NexHUD.Elite;
using NexHUD.Inputs;
using NexHUD.Ui.Common;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Search
{
    public class UiSearchCreate : NxGroupAutoCursor
    {
        NxMenu m_menu;
        public UiSearchCreate(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_menu = _menu;
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

            int boxsize = 15;
            int boxPlusPadding = boxsize + 4;

            int spaceBetweenCheckbox = 8;

            int spaceBetweenCategories = 35;

            Color breakLineColor = EDColors.LIGHTBLUE;


            //Allegiance
            Add(new NxSimpleText(0, 0, "Allegiance", EDColors.BLUE, 18, NxFonts.EuroCapital));
            Add(new NxRectangle(5, _cy + 20, m_menu.frame.WindowWidth - 10, 1, breakLineColor));
            _cy += 24;
            foreach (string s in SpanshDatas.allegiance)
            {
                int _width = (int)m_menu.frame.NxGraphics.MeasureString(s, NxCheckbox.DefaultFont).Width + boxPlusPadding;
                _cx += spaceBetweenCheckbox;
                NxCheckbox b = new NxCheckbox(_cx, _cy, _width, 30, s, _menu) { BoxSize = boxsize };
                b.Obj = nameof(SpanshDatas.allegiance);
                Add(b);
                _cx += _width;
            }
            

            _cx += 25;
            Add(new NxSimpleText(_cx, 0, "Need a permit", EDColors.BLUE, 18, NxFonts.EuroCapital));
            for (int i = 0; i < 3; i++)
            {
                string _cbText = "???";
                switch (i)
                {
                    case 0: _cbText = "Yes"; break;
                    case 1: _cbText = "No"; break;
                    case 2: _cbText = "Mine only"; break;
                }
                int _width = (int)m_menu.frame.NxGraphics.MeasureString(_cbText, NxCheckbox.DefaultFont).Width + boxPlusPadding;

                NxCheckbox b = new NxCheckbox(_cx, _cy, _width, 30, _cbText, _menu) { BoxSize = boxsize, CircleBox = true };
                b.Obj = "permitOptions";
                if (i == 2)
                    b.Checked = true;
                Add(b);
                _cx += _width;
                _cx += 15;
            }


            _cx = 0;
            _cy += spaceBetweenCategories;
            //Economy
            Add(new NxSimpleText(_cx, _cy, "Economy", EDColors.BLUE, 18, NxFonts.EuroCapital));
            Add(new NxRectangle(5, _cy + 20, m_menu.frame.WindowWidth - 10, 1, breakLineColor));
            _cy += 24;
            foreach (string s in SpanshDatas.economy)
            {
                int _width = (int)m_menu.frame.NxGraphics.MeasureString(s, NxCheckbox.DefaultFont).Width + boxPlusPadding;
                if (_cx + _width + 5 > m_menu.frame.WindowWidth)
                {
                    _cx = 0;
                    _cy += 32;
                }
                _cx += spaceBetweenCheckbox;
                NxCheckbox b = new NxCheckbox(_cx, _cy, _width, 30, s, _menu) { BoxSize = boxsize };
                b.Obj = nameof(SpanshDatas.economy);
                Add(b);
                _cx += _width;

            }

            _cx = 0;
            _cy += spaceBetweenCategories;
            //Governement
            Add(new NxSimpleText(_cx, _cy, "Government", EDColors.BLUE, 18, NxFonts.EuroCapital));
            Add(new NxRectangle(5, _cy + 20, m_menu.frame.WindowWidth - 10, 1, breakLineColor));
            _cy += 24;
            foreach (string s in SpanshDatas.government)
            {
                int _width = (int)m_menu.frame.NxGraphics.MeasureString(s, NxCheckbox.DefaultFont).Width + boxPlusPadding;
                if (_cx + _width + 5 > m_menu.frame.WindowWidth)
                {
                    _cx = 0;
                    _cy += 32;
                }
                _cx += spaceBetweenCheckbox;
                NxCheckbox b = new NxCheckbox(_cx, _cy , _width, 30, s, _menu) { BoxSize = boxsize };
                b.Obj = nameof(SpanshDatas.government);
                Add(b);
                _cx += _width;

            }


            _cx = 0;
            _cy += spaceBetweenCategories;
            //State
            Add(new NxSimpleText(_cx, _cy, "State", EDColors.BLUE, 18, NxFonts.EuroCapital));
            Add(new NxRectangle(5, _cy + 20, m_menu.frame.WindowWidth - 10, 1, breakLineColor));
            _cy += 24;
            foreach (string s in SpanshDatas.state)
            {
                int _width = (int)m_menu.frame.NxGraphics.MeasureString(s, NxCheckbox.DefaultFont).Width + boxPlusPadding;
                if (_cx + _width + 5 > m_menu.frame.WindowWidth)
                {
                    _cx = 0;
                    _cy += 32;
                }
                _cx += spaceBetweenCheckbox;
                NxCheckbox b = new NxCheckbox(_cx, _cy , _width, 30, s, _menu) { BoxSize = boxsize };
                b.Obj = nameof(SpanshDatas.state);
                Add(b);
                _cx += _width;
            }

            _cx = 0;
            _cy += spaceBetweenCategories;
            //Power
            Add(new NxSimpleText(_cx, _cy, "Power", EDColors.BLUE, 18, NxFonts.EuroCapital));
            Add(new NxRectangle(5, _cy + 20, m_menu.frame.WindowWidth - 10, 1, breakLineColor));
            _cy += 24;
            foreach (string s in SpanshDatas.power)
            {
                int _width = (int)m_menu.frame.NxGraphics.MeasureString(s, NxCheckbox.DefaultFont).Width + boxPlusPadding;
                if (_cx + _width + 5 > m_menu.frame.WindowWidth)
                {
                    _cx = 0;
                    _cy += 32;
                }
                _cx += spaceBetweenCheckbox;
                NxCheckbox b = new NxCheckbox(_cx, _cy, _width, 30, s, _menu) { BoxSize = boxsize };
                b.Obj = nameof(SpanshDatas.power);
                Add(b);
                _cx += _width;
            }

            _cx = 0;
            _cy += spaceBetweenCategories;
            //State
            Add(new NxSimpleText(_cx, _cy, "Power state", EDColors.BLUE, 18, NxFonts.EuroCapital));
            Add(new NxRectangle(5, _cy + 20, m_menu.frame.WindowWidth - 10, 1, breakLineColor));
            _cy += 24;
            foreach (string s in SpanshDatas.power_state)
            {
                int _width = (int)m_menu.frame.NxGraphics.MeasureString(s, NxCheckbox.DefaultFont).Width + boxPlusPadding;
                if (_cx + _width + 5 > m_menu.frame.WindowWidth)
                {
                    _cx = 0;
                    _cy += 32;
                }
                _cx += spaceBetweenCheckbox;
                NxCheckbox b = new NxCheckbox(_cx, _cy, _width, 30, s, _menu) { BoxSize = boxsize };
                b.Obj = nameof(SpanshDatas.power_state);
                Add(b);
                _cx += _width;
            }

            _cx = 0;
            _cy += spaceBetweenCategories;

            NxButton btnCreate = new NxButton(0, _cy, m_menu.frame.WindowWidth, 20, "Create search (console)", m_menu);
            btnCreate.onClick += onCreate;
            Add(btnCreate);


            MoveCursorToFirst();
        }

        private void onCreate(object sender, EventArgs e)
        {
            SpanshSearchSystems _search = compileSearch();
            Console.WriteLine(_search);

            SearchEngine.Instance.SearchInSystems(_search, _onSearchSuccess, _onSearchFailed);
        }

        private void _onSearchFailed(SearchEngine.SearchError obj)
        {
            Console.WriteLine("search failed: "+obj.ToString());
        }

        private void _onSearchSuccess(SpanshSystemsResult obj)
        {

            Console.WriteLine("search success:");
            foreach (SpanshSystem system in obj.results)
                Console.WriteLine("- {0}Ly _ {1} ({2})", system.distance, system.name, system.allegiance);
        }

        private SpanshSearchSystems compileSearch()
        {
            SpanshSearchSystems _search = new SpanshSearchSystems();
            _search.filters = new SpanshFilterSystems();
            _search.reference_system = EDDatas.Instance.getCurrentSystem().name;
            _search.filters.distance_from_coords = new SpanshValue<int?>(0, 100);

            List<string> allegiances = new List<string>();
            List<string> economies = new List<string>();
            List<string> governments = new List<string>();
            List<string> states = new List<string>();
            List<string> powers = new List<string>();
            List<string> powers_states = new List<string>();

            foreach (NxItem item in Items)
            {
                if(item is NxCheckbox )
                {
                    NxCheckbox checkBox = item as NxCheckbox;
                    if( checkBox.Checked )
                    {
                        switch(checkBox.Obj)
                        {
                            case "permitOptions": break;
                            case nameof(SpanshDatas.allegiance):
                                allegiances.Add(checkBox.Label);
                                break;
                            case nameof(SpanshDatas.economy):
                                economies.Add(checkBox.Label);
                                break;
                            case nameof(SpanshDatas.government):
                                governments.Add(checkBox.Label);
                                break;
                            case nameof(SpanshDatas.power):
                                powers.Add(checkBox.Label);
                                break;
                            case nameof(SpanshDatas.power_state):
                                powers_states.Add(checkBox.Label);
                                break;
                            case nameof(SpanshDatas.state):
                                states.Add(checkBox.Label);
                                break;
                        }
                    }
                }
            }

            if( allegiances.Count > 0 )
                _search.filters.allegiance = new SpanshValue<string[]>(allegiances.ToArray());
            if (economies.Count > 0)
                _search.filters.primary_economy = new SpanshValue<string[]>(economies.ToArray());
            if (governments.Count > 0)
                _search.filters.government = new SpanshValue<string[]>(governments.ToArray());
            if (powers.Count > 0)
                _search.filters.power = new SpanshValue<string[]>(powers.ToArray());
            if (powers_states.Count > 0)
                _search.filters.power_state = new SpanshValue<string[]>(powers_states.ToArray());
            if (states.Count > 0)
                _search.filters.state = new SpanshValue<string[]>(states.ToArray());

            return _search;
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
