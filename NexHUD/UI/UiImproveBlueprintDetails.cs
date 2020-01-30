using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.EDEngineer;
using NexHUDCore.NxItems;

namespace NexHUD.UI
{
    public class UiBlueprintDetails : NxGroup
    {
        const int ModY = 10;
        const int CostY = 350;
        const int EngineerY = 750;
        public const int Width = 250;
        public const int Height = 250;

        bool m_isExperimental;
        UiImproveBlueprintDetails m_uiBlueprintsDetails;

        NxSimpleText m_gradeText;
        NxSimpleText[] m_modifiers = new NxSimpleText[5];
        NxSimpleText[] m_costs = new NxSimpleText[5];

        BlueprintDatas m_datas;

        public bool IsExperimental { get => m_isExperimental; }

        public UiBlueprintDetails(bool _isExperimetal, UiImproveBlueprintDetails _uiBlueprintsDetails) : base(_uiBlueprintsDetails.Parent)
        {
            m_uiBlueprintsDetails = _uiBlueprintsDetails;
            y = 95;
            m_isExperimental = _isExperimetal;
            x = IsExperimental ? 10+ Width : 5;

            Add(new NxRectangle(x, y, Width, Height, EDColors.getColor(EDColors.WHITE, .1f)));
            m_gradeText = new NxSimpleText(x + 10, y, "GRADE ?", EDColors.YELLOW, 18, NxFonts.EuroCapital);// { vertical = true };
            Add(m_gradeText);

            //Frame Modifications
            Add(new NxRectangle(x+5, y+20, Width-10, Height/2 - 10-20, EDColors.getColor( EDColors.BLACK, 0.5f) ));
            //Frame Cost
            int _frameCostY = y + Height / 2 + 5 + 20;
            Add(new NxRectangle(x+5, _frameCostY, Width-10, Height / 2 - 10 - 20, EDColors.getColor( EDColors.BLACK, 0.5f) ));
          

            for(int i = 0; i < m_modifiers.Length; i++)
            {
                m_modifiers[i] = new NxSimpleText(x+5, y+25+i*20, "Modifier " + i.ToString(), EDColors.GRAY,18);
                Add(m_modifiers[i]);
            }
            for (int i = 0; i < m_costs.Length; i++)
            {
                m_costs[i] = new NxSimpleText(x + 5, _frameCostY + i * 20, "Cost " + i.ToString(), EDColors.GRAY,18);
                Add(m_costs[i]);
            }
        }
        public void setDatas(BlueprintDatas _datas)
        {
            m_datas = _datas;

            if (m_isExperimental)
                m_gradeText.text = "Experimental";
            else
                m_gradeText.text = string.Format("Grade {0}", _datas.Grade);

            for(int i = 0; i < m_modifiers.Length; i++)
            {
                if (i < m_datas.Effects.Length)
                {
                    m_modifiers[i].isVisible = true;
                    m_modifiers[i].text = string.Format("{0} : {1}", m_datas.Effects[i].Property, m_datas.Effects[i].Effect);
                    m_modifiers[i].Color = m_datas.Effects[i].IsGood ? EDColors.LIGHTBLUE : EDColors.RED;
                }
                else
                    m_modifiers[i].isVisible = false;
            }
            for (int i = 0; i < m_costs.Length; i++)
            {
                if (i < m_datas.Ingredients.Length)
                {
                    m_costs[i].isVisible = true;
                    m_costs[i].text = string.Format("{1} x {0}", m_datas.Ingredients[i].Name, m_datas.Ingredients[i].Size);
                    bool gotThem = EngineerHelper.getCmdrMaterials(m_datas.Ingredients[i].Name) >= m_datas.Ingredients[i].Size;
                    m_costs[i].Color = gotThem ? EDColors.BLUE : EDColors.RED;
                }
                else
                    m_costs[i].isVisible = false;
            }
        }
    }
    public class UiImproveBlueprintDetails : NxGroup
    {
        UiImprove m_uiImprove;
        BlueprintDatas m_blueprint;
        bool m_updatePositions = true;
        //Titre
        NxSimpleText m_blueprintType;
        NxSimpleText m_blueprintName;
        //Grades
        UiBlueprintDetails m_BlueprintDetails;
        UiBlueprintDetails m_BlueprintDetailsExperimental;
        public UiImproveBlueprintDetails(UiImprove _uiImprove) : base(_uiImprove.Menu.frame.NxOverlay)
        {
            m_uiImprove = _uiImprove;
            //Titre
            m_blueprintType = new NxSimpleText(5, 70, "TYPE: ", EDColors.YELLOW, 24, NxFonts.EuroCapital);
            m_blueprintName = new NxSimpleText(120, 70, "TYPE: ", EDColors.GREEN, 24, NxFonts.EuroCapital);
            Add(m_blueprintType);
            Add(m_blueprintName);

            m_BlueprintDetails = new UiBlueprintDetails(false, this);
            Add(m_BlueprintDetails);
            m_BlueprintDetailsExperimental = new UiBlueprintDetails(true, this);
            Add(m_BlueprintDetailsExperimental);
        }

        public void setBlueprint(BlueprintDatas _blueprint)
        {
            m_blueprint = _blueprint;
            updateContent();
        }
        private void updateContent()
        {
            if (m_blueprint == null)
                return;
            m_blueprintType.text = string.Format("{0} : ", m_blueprint.Type.ToUpper());
            m_blueprintName.text = string.Format("{0}", m_blueprint.Name);

            int _maxGrade = 6;
            BlueprintDatas d = null;
            while (d == null && _maxGrade > 0) {
                _maxGrade--;
                d = EngineerHelper.blueprints.Where(x => x.Type == m_blueprint.Type && x.Name == m_blueprint.Name && x.Grade == _maxGrade).FirstOrDefault();
            }
            if( d != null)
                m_BlueprintDetails.setDatas(d);

            //first experimental
            BlueprintDatas e = null;
            e = EngineerHelper.blueprints.Where(x => x.Type == m_blueprint.Type && x.IsExperimental ).FirstOrDefault();
            if( e != null)
                m_BlueprintDetailsExperimental.setDatas(e);

            m_updatePositions = true;
        }
        private void updatePositions()
        {
            m_blueprintName.x = 5 + (int)m_blueprintType.sizeF.Width;

            m_updatePositions = false;
        }

        public override void Render(Graphics _g)
        {
            base.Render(_g);
            if (m_updatePositions)
                updatePositions();
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
