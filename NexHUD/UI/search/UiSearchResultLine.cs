using NexHUD.Apis.Spansh;
using NexHUD.Inputs;
using NexHUD.Ui.Common;
using NexHUDCore.NxItems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Ui.Search
{
    public class UiSearchResultLine : NxGroup, ISelectable
    {
        private NxSimpleText[] m_props;
        private NxRectangle[] m_separators;
        private NxRectangle m_background;
        private int[] m_maxWidth;
        private bool m_Selected = false;
        private bool m_isSelectable = true;
        private bool m_isEnable = true;

        public int[] XPos;

        public EventHandler onClick;

        public SpanshSystem LastSystem;
        public UiSearchResultLine(UiSearchResult _parent) : base(_parent.Parent)
        {
            Color = EDColors.getColor(EDColors.ORANGE, 0.1f);
            RelativeChildPos = true;

            m_background = new NxRectangle(0, 0, width, height, Color);
            Add(m_background);

            XPos = new int[12];
            m_props = new NxSimpleText[12];
            m_separators = new NxRectangle[m_props.Length];
            m_maxWidth = new int[m_props.Length];

            for (int i = 0; i < m_separators.Length; i++)
            {
                m_separators[i] = new NxRectangle(i * 50, 0, 2, height, Color.Black);
                Add(m_separators[i]);
            }

            for (int i = 0; i < m_props.Length; i++)
            {
                m_props[i] = new NxSimpleText(i * 50, 5, "Property " + i.ToString(), Color.Orange, 18) { AutoSize = true };
                Add(m_props[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="index">if (index == -1) display title</param>
        public void SetDatas(SpanshSystemsResult result, int index = -1)
        {

            m_background.width = width;
            m_background.height = height;


            isSelectable = index != -1;
            isEnable = index != -1;
            Selected = false;

            LastSystem = null;

            List<string> fields = new List<string>();

            if (result.search.filters.government != null)
                fields.Add(nameof(result.search.filters.government));
            if (result.search.filters.population != null)
                fields.Add(nameof(result.search.filters.population));
            if (result.search.filters.power != null)
                fields.Add(nameof(result.search.filters.power));
            if (result.search.filters.power_state != null)
                fields.Add(nameof(result.search.filters.power_state));
            if (result.search.filters.primary_economy != null)
                fields.Add(nameof(result.search.filters.primary_economy));
            if (result.search.filters.secondary_economy != null)
                fields.Add(nameof(result.search.filters.secondary_economy));
            if (result.search.filters.state != null)
                fields.Add(nameof(result.search.filters.state));

            //Always display !
            fields.Add(nameof(result.search.filters.allegiance));
            fields.Add(nameof(result.search.filters.security));

            //Visibilitys
            for (int i = 2; i < m_props.Length - 2; i++)
            {
                if (i - 2 < fields.Count)
                {
                    m_props[i].isVisible = true;
                    m_separators[i].isVisible = true;
                    m_separators[i].height = height;
                }
                else
                {
                    m_props[i].isVisible = false;
                    m_separators[i].isVisible = false;
                }
            }

            if (index == -1)
            {

                m_props[0].text = "Dist.";
                m_props[1].text = "System name";

                for (int i = 2; i < m_props.Length - 2; i++)
                {
                    if (i - 2 < fields.Count)
                        m_props[i].text = fields[i - 2];
                }
            }
            else if (index < result.count)
            {
                LastSystem = result.results[index];
                m_props[0].text = Math.Round(result.results[index].distance, 1).ToString();
                m_props[1].text = result.results[index].name;

                for (int i = 2; i < m_props.Length - 2; i++)
                {
                    if (i - 2 < fields.Count)
                    {
                        switch (fields[i - 2])
                        {
                            case nameof(SpanshSearchSystems.filters.allegiance): m_props[i].text = result.results[index].allegiance; break;
                            case nameof(SpanshSearchSystems.filters.government): m_props[i].text = result.results[index].government; break;
                            case nameof(SpanshSearchSystems.filters.population): m_props[i].text = result.results[index].population.ToString(); break;
                            case nameof(SpanshSearchSystems.filters.power):
                                m_props[i].text = "";
                                foreach (string p in result.results[index].power)
                                    m_props[i].text += p + ", ";
                                m_props[i].text = m_props[i].text.Substring(0, m_props[i].text.Length - 2);
                                break;
                            case nameof(SpanshSearchSystems.filters.power_state): m_props[i].text = result.results[index].power_state; break;
                            case nameof(SpanshSearchSystems.filters.primary_economy): m_props[i].text = result.results[index].primary_economy; break;
                            case nameof(SpanshSearchSystems.filters.secondary_economy): m_props[i].text = result.results[index].secondary_economy; break;
                            case nameof(SpanshSearchSystems.filters.security): m_props[i].text = result.results[index].security; break;
                            case nameof(SpanshSearchSystems.filters.state): m_props[i].text = result.results[index].state; break;
                        }
                    }
                }
            }

            setPositions();

            m_background.Color = isEnable ? Color.Orange : Color.White;

        }
        private void setPositions()
        {
            //Positions
            int _totalWidth = 0;
            for (int i = 0; i < XPos.Length; i++)
                XPos[i] = 0;

            for (int i = 0; i < m_props.Length; i++)
            {
                // m_separators[i].x = _totalWidth;
                //  m_props[i].x = 5 + _totalWidth;
                XPos[i] = _totalWidth;

                _totalWidth += m_props[i].width + 15;
            }
        }
        public void setPositions(int[] _Xpos)
        {
            XPos = _Xpos;
            for (int i = 0; i < m_props.Length; i++)
            {
                m_separators[i].x = XPos[i] - 5;
                m_props[i].x = XPos[i];
            }
        }
        public override void Update()
        {
            base.Update();

            if (Shortcuts.SelectPressed && isVisible && Selected)
                onClick?.Invoke(this, new EventArgs());
        }
        public override void Render(Graphics _g)
        {
            base.Render(_g);

            m_background.Color = EDColors.getColor(m_background.Color, Selected ? 0.3f: 0.1f);
            m_props[1].Color = Selected ? EDColors.WHITE : EDColors.ORANGE;
        }
        public bool Selected { get => m_Selected; set { if (m_Selected != value) makeItDirty(); m_Selected = value; } }
        public bool isSelectable { get => m_isSelectable; set { if (m_isSelectable != value) makeItDirty(); m_isSelectable = value; } }
        public bool isEnable { get => m_isEnable; set { if (m_isEnable != value) makeItDirty(); m_isEnable = value; } }

    }
}
