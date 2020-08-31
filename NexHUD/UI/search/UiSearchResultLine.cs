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
        public const int PropertiesCount = 12;
        private NxSimpleText[] m_props;
        private NxRectangle[] m_separators;
        private NxRectangle m_background;
        private bool m_Selected = false;
        private bool m_isSelectable = true;
        private bool m_isEnable = true;

        public int[] Widths;

        public EventHandler onClick;

        public SpanshSystem LastSystem;
        public SpanshBody LastBody;

        private bool m_registerWidth = false;
        public bool WidthMustBeRefreshed = false;
        public UiSearchResultLine(UiSearchResult _parent) : base(_parent.Parent)
        {
            Color = EDColors.getColor(EDColors.ORANGE, 0.1f);
            RelativeChildPos = true;

            m_background = new NxRectangle(0, 0, width, height, Color);
            Add(m_background);

            Widths = new int[PropertiesCount];
            m_props = new NxSimpleText[PropertiesCount];
            m_separators = new NxRectangle[PropertiesCount];

            for (int i = 0; i < m_separators.Length; i++)
            {
                m_separators[i] = new NxRectangle(i * 50, 0, 1, height, Color.Black);
                Add(m_separators[i]);
            }

            for (int i = 0; i < m_props.Length; i++)
            {
                m_props[i] = new NxSimpleText(i * 50, 5, "Property " + i.ToString(), Color.Orange, 18) { AutoSize = false };
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
            LastBody = null;

            List<string> fields = new List<string>();

            if (result.search.filters.primary_economy != null)
                fields.Add(nameof(result.search.filters.primary_economy));
            if (result.search.filters.state != null)
                fields.Add(nameof(result.search.filters.state));
            if (result.search.filters.government != null)
                fields.Add(nameof(result.search.filters.government));
            if (result.search.filters.population != null)
                fields.Add(nameof(result.search.filters.population));
            if (result.search.filters.power != null)
                fields.Add(nameof(result.search.filters.power));
            if (result.search.filters.power_state != null)
                fields.Add(nameof(result.search.filters.power_state));
            if (result.search.filters.secondary_economy != null)
                fields.Add(nameof(result.search.filters.secondary_economy));

            //Always display !
            fields.Add(nameof(result.search.filters.allegiance));
            fields.Add(nameof(result.search.filters.security));

            //Additionnal fields
            if (fields.Count < 10 && !fields.Contains(nameof(result.search.filters.primary_economy)))
                fields.Add(nameof(result.search.filters.primary_economy));
            if (fields.Count < 10 && !fields.Contains(nameof(result.search.filters.state)))
                fields.Add(nameof(result.search.filters.state));
            if (fields.Count < 10 && !fields.Contains(nameof(result.search.filters.government)))
                fields.Add(nameof(result.search.filters.government));
            if (fields.Count < 10 && !fields.Contains(nameof(result.search.filters.population)))
                fields.Add(nameof(result.search.filters.population));
            if (fields.Count < 10 && !fields.Contains(nameof(result.search.filters.power)))
                fields.Add(nameof(result.search.filters.power));
            if (fields.Count < 10 && !fields.Contains(nameof(result.search.filters.power_state)))
                fields.Add(nameof(result.search.filters.power_state));

            //Visibilitys
            for (int i = 2; i < m_props.Length - 2; i++)
            {
                if (i - 2 < fields.Count)
                {
                    m_props[i].isVisible = true;
                    m_separators[i].isVisible = true;
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
                        m_props[i].text = SpanshFilterSystems.getNameOf(fields[i - 2]);
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
                                if (result.results[index].power != null)
                                {
                                    foreach (string p in result.results[index].power)
                                        m_props[i].text += p + ", ";
                                    m_props[i].text = m_props[i].text.Substring(0, m_props[i].text.Length - 2);
                                }
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

            m_registerWidth = true;

            m_background.Color = isEnable ? Color.Orange : Color.White;

        }

        public void SetDatas(SpanshBodiesResult result, string material_name, int index = -1)
        {
            m_background.width = width;
            m_background.height = height;


            isSelectable = index != -1;
            isEnable = index != -1;
            Selected = false;

            LastSystem = null;
            LastBody = null;
            //datas

         

            //Visibilitys
            for (int i = 0; i < m_props.Length; i++)
            {
                if (i < 6)
                {
                    m_props[i].isVisible = true;
                    m_separators[i].isVisible = true;
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

                m_props[2].text = "Body";
                m_props[3].text = "Dist. to arrival";
                m_props[4].text = "Gravity";
                m_props[5].text = material_name; //??? Nom material
            }
            else if (index < result.count)
            {
                LastBody = result.results[index];
                //distance from system to system
                m_props[0].text = Math.Round( (double)result.results[index].distance, 1).ToString();
                //System name
                m_props[1].text = result.results[index].system_name;
                //Body
                m_props[2].text = result.results[index].name;
                //Distance to arrival
                m_props[3].text = Math.Round((double)result.results[index].distance_to_arrival, 0).ToString();
                //Gravity
                m_props[4].text = Math.Round((double)result.results[index].gravity, 2).ToString();

                //Material
                foreach(SpanshMaterial mat in result.results[index].materials )
                {
                    if( mat.name.ToLower()== material_name.ToLower() )
                    {
                        m_props[5].text = Math.Round( (double)mat.share, 2).ToString() + " %";
                    }
                }
            }

            ///

            m_registerWidth = true;

            m_background.Color = isEnable ? Color.Orange : Color.White;

        }
            public void setPositions(int[] _Widths)
        {
            int _totalWidth = 0;
            for (int i = 0; i < m_props.Length; i++)
            {
                if (_totalWidth + _Widths[i] < width)
                {
                    m_props[i].isVisible = true;
                    m_separators[i].isVisible = true;
                    m_separators[i].x = _totalWidth-3;
                    m_props[i].x = _totalWidth;
                    _totalWidth += _Widths[i]+3;
                }
                else
                {
                    m_props[i].isVisible = false;
                    m_separators[i].isVisible = false;
                }
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

            m_background.Color = EDColors.getColor(m_background.Color, Selected ? 0.3f : 0.1f);
            m_props[1].Color = Selected ? EDColors.WHITE : EDColors.ORANGE;

            if (m_registerWidth)
            {
                for (int i = 0; i < m_props.Length; i++)
                {
                    Widths[i] = (int)m_props[i].sizeF.Width;
                }
                m_registerWidth = false;
                WidthMustBeRefreshed = true;
            }
        }
        public bool Selected { get => m_Selected; set { if (m_Selected != value) makeItDirty(); m_Selected = value; } }
        public bool isSelectable { get => m_isSelectable; set { if (m_isSelectable != value) makeItDirty(); m_isSelectable = value; } }
        public bool isEnable { get => m_isEnable; set { if (m_isEnable != value) makeItDirty(); m_isEnable = value; } }

    }
}
