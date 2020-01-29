﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.EDEngineer;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.UI
{
    public class NxMainPanelBlueprints : NxGroup
    {
        private const int HEIGHT_CAT = 35;
        private const int HEIGHT_TYPE = 30;
        private const int HEIGHT_NAME = 25;
        private const int BUTTON_WIDTH = 250;

        private bool m_firstUpdateSkipped = false;

        public List<NxButton> m_Buttons = new List<NxButton>();

        private Point m_CursorCoords = new Point();

        private int m_CursorMaxX = 0;
        private int[] m_CursorMaxY = new int[10];

        private string m_TypeSelected = "";
        private BlueprintDatas m_NameSelected = null;
        public NxMainPanelBlueprints(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            //create Buttons
            for (int i = 0; i < 54; i++)
            {
                m_Buttons.Add(new NxButton(200, 200 + i * 20, BUTTON_WIDTH, HEIGHT_CAT, "Blueprint " + i, _menu));
                m_Buttons[i].isVisible = false;
                Add(m_Buttons[i]);
            }
            refresh();
        }

        public override void Update()
        {
            base.Update();

            if (!isVisible)
                return;

            if (!m_firstUpdateSkipped)
            {
                m_firstUpdateSkipped = true;
                return;
            }

            if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.right)) && m_CursorCoords.X < m_CursorMaxX)
                m_CursorCoords.X++;
            if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.left)) && m_CursorCoords.X > 0)
                m_CursorCoords.X--;
            if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.down)) ) 
            {
                if( m_CursorCoords.Y < m_CursorMaxY[m_CursorCoords.X] )
                    m_CursorCoords.Y++;
                else if( m_CursorCoords.X < m_CursorMaxX)
                {
                    m_CursorCoords.X++;
                    m_CursorCoords.Y = 0;
                }
            }
            if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.up)) )
            {
                if (m_CursorCoords.Y > (m_CursorCoords.X == 0 ? 1 : 0) )
                    m_CursorCoords.Y--;
                else if (m_CursorCoords.X > 0)
                {
                    m_CursorCoords.X--;
                    m_CursorCoords.Y = m_CursorMaxY[m_CursorCoords.X];
                }
            }

            //Pick the one
            NxButton _selected = null;
            while (_selected == null)
            {
                _selected = m_Buttons.Where(x => x.Coords == m_CursorCoords).FirstOrDefault();
                if( _selected != null && !_selected.isSelectable )
                {
                    if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.up)))
                        m_CursorCoords.Y--;
                    else
                        m_CursorCoords.Y++;
                    _selected = null;
                    continue;
                }
                if (_selected == null)
                {
                    if (m_CursorCoords.Y > 1)
                        m_CursorCoords.Y--;
                    else if (m_CursorCoords.X > 0)
                        m_CursorCoords.X--;
                    else
                        break;
                }
            }
            foreach (NxButton b in m_Buttons)
                b.Selected = b == _selected;

            if (SteamVR_NexHUD.isShortcutPressed(Shortcuts.get(ShortcutId.select)) && _selected != null)
            {
                if (_selected.Obj is string)
                    m_TypeSelected = (string)_selected.Obj;
                else if (_selected.Obj is BlueprintDatas)
                    SteamVR_NexHUD.Log("TODO: Display blueprint: {0} / {1}", ((BlueprintDatas)_selected.Obj).Type, ((BlueprintDatas)_selected.Obj).Name);
                refresh();
            }
        }

        private void refresh()
        {
            int _startX = 5;
            int _startY = 100;
            int _btnId = 0;
            int _y = _startY;
            int _x = _startX;
            int _vSpace = 5;
            int _hSpace = 5;

            Point _cCoords = new Point();
            m_CursorMaxX = 0;
            m_CursorMaxY.Initialize();

            for (int i = 0; i <= (int)BlueprintCategorie.Armour; i++)
            {
                if (_y > _startY)
                    _y += _vSpace;
                BlueprintCategorie _categorie = (BlueprintCategorie)i;
                m_Buttons[_btnId].Coords = _cCoords;
                m_Buttons[_btnId].resetColors();
                m_Buttons[_btnId].Label = EDEnumHelper.ToStringFormated(_categorie).ToUpper();
                m_Buttons[_btnId].isSelectable = false;
                m_Buttons[_btnId].x = _x;
                m_Buttons[_btnId].y = _y;
                m_Buttons[_btnId].Height = HEIGHT_CAT;
                _y += m_Buttons[_btnId].Height + _vSpace;
                _btnId++;
                _cCoords.Y++;


                List<string> _typeAdded = new List<string>();
                List<string> _nameAdded = new List<string>();
                foreach (BlueprintDatas _data in EngineerHelper.blueprints.Where(x => x.Categorie == _categorie))
                {
                    if (_typeAdded.Contains(_data.Type))
                        continue;
                    m_CursorMaxX = Math.Max(m_CursorMaxX, _cCoords.X);
                    m_CursorMaxY[_cCoords.X] = Math.Max(m_CursorMaxY[_cCoords.X], _cCoords.Y);

                    m_Buttons[_btnId].Obj = _data.Type;
                    m_Buttons[_btnId].Coords = _cCoords;
                    m_Buttons[_btnId].resetColors();
                    m_Buttons[_btnId].ColorLines = EDColors.BLACK;
                    m_Buttons[_btnId].ColorLabel = EDColors.getColor( EDColors.YELLOW,.8f );
                    m_Buttons[_btnId].Label = _data.Type;
                    m_Buttons[_btnId].isSelectable = true;
                    m_Buttons[_btnId].Selected = false;
                    m_Buttons[_btnId].x = _x;
                    m_Buttons[_btnId].y = _y;
                    m_Buttons[_btnId].Height = HEIGHT_TYPE;
                    _y += m_Buttons[_btnId].Height + _vSpace;

                  

                    _btnId++;
                    _cCoords.Y++;

                    _typeAdded.Add(_data.Type);

                    if (_cCoords.Y > 12)
                    {
                        _y = _startY;
                        _x += BUTTON_WIDTH + _hSpace;
                        _cCoords.Y = 0;
                        _cCoords.X++;
                    }


                    if (!string.IsNullOrEmpty(m_TypeSelected) && m_TypeSelected == _data.Type)
                    {
                        m_CursorCoords = m_Buttons[_btnId-1].Coords;
                        m_Buttons[_btnId - 1].ColorLines = EDColors.YELLOW;
                        m_Buttons[_btnId - 1].ColorBack = EDColors.getColor(EDColors.ORANGE, 0.3f);
                        m_Buttons[_btnId - 1].ColorLabel = EDColors.YELLOW;

                        foreach (BlueprintDatas _data2 in EngineerHelper.blueprints.Where(x => x.Type == m_TypeSelected && !x.IsExperimental ))
                        {
                            if (_nameAdded.Contains(_data2.Name))
                                continue;
                            m_CursorMaxX = Math.Max(m_CursorMaxX, _cCoords.X);
                            m_CursorMaxY[_cCoords.X] = Math.Max(m_CursorMaxY[_cCoords.X], _cCoords.Y);

                            m_Buttons[_btnId].Obj = _data2;
                            m_Buttons[_btnId].Coords = _cCoords;
                            m_Buttons[_btnId].resetColors();
                            m_Buttons[_btnId].ColorBack = EDColors.getColor( EDColors.GREEN,0.4f);
                            m_Buttons[_btnId].ColorBackSelected = EDColors.getColor(EDColors.GREEN, 0.8f);
                            m_Buttons[_btnId].ColorLines = EDColors.BLACK;
                            m_Buttons[_btnId].Label = _data2.Name;
                            m_Buttons[_btnId].isSelectable = true;
                            m_Buttons[_btnId].Selected = false;
                            m_Buttons[_btnId].x = _x;
                            m_Buttons[_btnId].y = _y;
                            m_Buttons[_btnId].Height = HEIGHT_TYPE;
                            _y += m_Buttons[_btnId].Height + _vSpace;
                            _btnId++;
                            _cCoords.Y++;

                            _nameAdded.Add(_data2.Name);

                            if (_cCoords.Y > 12)
                            {
                                _y = _startY;
                                _x += BUTTON_WIDTH + _hSpace;
                                _cCoords.Y = 0;
                                _cCoords.X++;
                            }
                        }
                    }

                  
                }

            }

            for (int i = 0; i < m_Buttons.Count; i++)
            {
                m_Buttons[i].isVisible = i < _btnId;
                m_Buttons[i].makeItDirty();
            }
        }
    }
}
