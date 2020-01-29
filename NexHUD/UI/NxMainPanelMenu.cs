﻿using NexHUDCore.NxItems;
using System;
using System.Drawing;

namespace NexHUD.UI
{
    public class NxMainPanelMenu : NxGroup
    {
        /// <summary>
        /// Spacing between two buttons
        /// </summary>
        public const int Spacing = 60;

        /// <summary>
        /// The array of the Buttons generated from the enum NxMainPanelMenuButton.MenuButtonType
        /// </summary>
        NxMainPanelMenuButton[] m_buttons;

        private int m_selectedId = 0;
        public int SelectedId
        {
            get { return m_selectedId; }
            set
            {
                if (m_selectedId != value)
                {
                    m_selectedId = value;
                    onSelectChange();
                }
            }

        }

        public NxMainPanelMenuButton.MenuButtonType SelectedMenu
        {
            get
            {
                return m_buttons[SelectedId].menuButtonType;
            }
        }
        public bool isSelectedMenuActive()
        {
            return m_buttons[SelectedId].isActive;
        }
        public int ButtonNumbers { get { return m_buttons.Length; } }

        /// <summary>
        /// The main menu group (with the button Search, Improve, Trade....)
        /// </summary>
        /// <param name="_menu"></param>
        public NxMainPanelMenu(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            /**
             * There is no need to change this code to add a Menu Button
             * Simply add an entry in the enum NxMainPanelMenuButton.MenuButtonType
             * 
             * */
            int ButtonY = (_menu.frame.WindowHeight / 2) + (NxMainPanelTopInfos.HEIGHT / 2);
            int _number = Enum.GetNames(typeof(NxMainPanelMenuButton.MenuButtonType)).Length;
            m_buttons = new NxMainPanelMenuButton[_number];
            bool _pair = _number % 2 == 0;
            for (int i = 0; i < _number; i++)
            {
                m_buttons[i] = new NxMainPanelMenuButton(
                    _pair ?
                    (
                        (i - (_number / 2)) * (NxMainPanelMenuButton.WIDTH + Spacing) + _menu.frame.WindowWidth / 2 + (NxMainPanelMenuButton.WIDTH / 2) + (Spacing / 2)
                    )
                    :
                    (
                        (i - (_number / 2)) * (NxMainPanelMenuButton.WIDTH + Spacing) + _menu.frame.WindowWidth / 2
                    )
                    ,
                    ButtonY,
                    (NxMainPanelMenuButton.MenuButtonType)i, Parent
                    );


                Add(m_buttons[i]);
            }

            onSelectChange();
        }

        private void onSelectChange()
        {
            for (int i = 0; i < m_buttons.Length; i++)
            {
                m_buttons[i].isSelected = i == m_selectedId;
            }
        }

        public void selectNext()
        {
            if (SelectedId < ButtonNumbers)
                SelectedId++;
        }
        public void selectPrev()
        {
            if (SelectedId > 0)
                SelectedId--;
        }

        public void setActive(NxMainPanelMenuButton.MenuButtonType _type, bool _isActive)
        {
            for (int i = 0; i < m_buttons.Length; i++)
            {
                if (m_buttons[i].menuButtonType == _type)
                    m_buttons[i].isActive = _isActive;
            }
        }


        public override void Render(Graphics _g)
        {
            base.Render(_g);
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
