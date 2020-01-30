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
        public const int Width = 280;
        public const int Height = 300;

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
            y = 105;
            m_isExperimental = _isExperimetal;
            x = IsExperimental ? 10 + Width : 5;

            Add(new NxRectangle(x, y, Width, Height, EDColors.getColor(EDColors.WHITE, .05f)));
            m_gradeText = new NxSimpleText(x + 5, y+2, "GRADE / EXP NAME.", EDColors.YELLOW, 20, NxFonts.EuroCapital);// { vertical = true };
            Add(m_gradeText);

            int _frameHeight = 110;
            //Frame Modifications
            NxRectangle _frameMod = new NxRectangle(x + 5, y + 50, Width - 10, _frameHeight, EDColors.getColor(EDColors.BLACK, 0.4f));
            Add(_frameMod);
            Add(new NxSimpleText(_frameMod.x, _frameMod.y-20, "MODIFIERS", EDColors.getColor(EDColors.YELLOW, 0.5f)));
            //Frame Cost
            NxRectangle _frameCost = new NxRectangle( x + 5, y+Height-5- _frameHeight, Width -10, _frameHeight, EDColors.getColor(EDColors.BLACK, 0.4f));
            Add(_frameCost);
            Add(new NxSimpleText(_frameCost.x, _frameCost.y-20, "CRAFTING COST", EDColors.getColor(EDColors.YELLOW, 0.5f)));


            for (int i = 0; i < m_modifiers.Length; i++)
            {
                m_modifiers[i] = new NxSimpleText(_frameMod.x + 5, _frameMod.y+ 5 + i * 20, "Modifier " + i.ToString(), EDColors.GRAY, 18);
                Add(m_modifiers[i]);
            }
            for (int i = 0; i < m_costs.Length; i++)
            {
                m_costs[i] = new NxSimpleText(_frameCost.x + 5, _frameCost.y + 5 + i * 20, "Cost " + i.ToString(), EDColors.GRAY, 18);
                Add(m_costs[i]);
            }
        }
        public void setDatas(BlueprintDatas _datas)
        {
            m_datas = _datas;

            if (m_isExperimental)
                m_gradeText.text = _datas.Name;
            else
                m_gradeText.text = string.Format("Grade {0}", _datas.Grade);

            for (int i = 0; i < m_modifiers.Length; i++)
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

    public class UiBlueprintMaterialList : NxGroup
    {
        UiImproveBlueprintDetails m_uiBlueprintsDetails;
        public UiBlueprintMaterialList(UiImproveBlueprintDetails _uiBlueprintsDetails) : base(_uiBlueprintsDetails.Parent)
        {
            m_uiBlueprintsDetails = _uiBlueprintsDetails;
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
        NxSimpleText[] m_blueprintEngineers = new NxSimpleText[5];
        //Descriptions
        UiBlueprintDetails m_BlueprintDetails;
        UiBlueprintDetails m_BlueprintDetailsExperimental;
        //List of all materials
        NxButton[] m_materials = new NxButton[15];
        public UiImproveBlueprintDetails(UiImprove _uiImprove) : base(_uiImprove.Menu.frame.NxOverlay)
        {
            m_uiImprove = _uiImprove;
            //Titre
            m_blueprintType = new NxSimpleText(5, 70, "TYPE: ", EDColors.YELLOW, 24, NxFonts.EuroCapital);
            m_blueprintName = new NxSimpleText(120, 70, "TYPE: ", EDColors.GREEN, 24, NxFonts.EuroCapital);
            Add(m_blueprintType);
            Add(m_blueprintName);

            for (int i = 0; i < m_blueprintEngineers.Length; i++)
            {
                m_blueprintEngineers[i] = new NxSimpleText(5, 75, "engineer " + i, EDColors.GRAY, 18, NxFonts.EuroStile);
                m_blueprintEngineers[i].isVisible = false;
                Add(m_blueprintEngineers[i]);
            }

            Add(new NxSimpleText(NxMenu.Width - 265, 105, "All materials (click to search)", EDColors.YELLOW, 20, NxFonts.EuroStile));
            //Material list
            for(int i = 0; i < m_materials.Length; i++)
            {
                m_materials[i] = new NxButton(NxMenu.Width - 265, 130+i*28, 255, 26, "Material " + i, m_uiImprove.Menu);
                
                m_materials[i].ColorBack = EDColors.BLACK;
                m_materials[i].ColorBackSelected = EDColors.WHITE;
                m_materials[i].ColorLines = EDColors.BLACK;
                m_materials[i].ColorLabel = EDColors.LIGHTBLUE;
                Add(m_materials[i]);
            }

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

            for (int i = 0; i < m_blueprintEngineers.Length; i++)
            {
                if (i < m_blueprint.Engineers.Length)
                {
                    m_blueprintEngineers[i].isVisible = true;
                    m_blueprintEngineers[i].text = string.Format("| {0}", m_blueprint.Engineers[i]);
                    //TODO Color lock/unlock
                }
                else
                    m_blueprintEngineers[i].isVisible = false;
            }

            int _maxGrade = 6;
            BlueprintDatas d = null;
            while (d == null && _maxGrade > 0)
            {
                _maxGrade--;
                d = EngineerHelper.blueprints.Where(x => x.Type == m_blueprint.Type && x.Name == m_blueprint.Name && x.Grade == _maxGrade).FirstOrDefault();
            }
            if (d != null)
                m_BlueprintDetails.setDatas(d);

            //first experimental
            BlueprintDatas e = null;
            e = EngineerHelper.blueprints.Where(x => x.Type == m_blueprint.Type && x.IsExperimental).FirstOrDefault();
            if (e != null)
                m_BlueprintDetailsExperimental.setDatas(e);


            //list of all materials needed with all rolls
            for(int i = 0; i < m_materials.Length; i++ )
            {
                MaterialDatas[] _needed = EngineerHelper.getAllCraftMaterials(m_blueprint.Type, m_blueprint.Name, e != null ? e.Name : string.Empty, _maxGrade);
                if (i < _needed.Length)
                {
                    m_materials[i].isVisible = true;
                    m_materials[i].Label = string.Format("{0} x {1}", _needed[i].Quantity, _needed[i].Name);
                    if (_needed[i].nxSearch != null)
                    {
                        m_materials[i].ColorBack = EDColors.getColor(Color.Orange, 0.1f);
                        m_materials[i].ColorBackSelected = EDColors.getColor(Color.Orange, 0.8f);
                    }
                    else
                    {
                        m_materials[i].ColorBack = EDColors.getColor(Color.White, 0.05f);
                        m_materials[i].ColorBackSelected = EDColors.getColor(Color.Orange, 0.2f);
                    }
                }
                else
                    m_materials[i].isVisible = false;
            }

            m_updatePositions = true;
        }
        private void updatePositions()
        {
            m_blueprintName.x = 5 + (int)m_blueprintType.sizeF.Width;

            for (int i = 0; i < m_blueprintEngineers.Length; i++)
            {
                if( i == 0)
                    m_blueprintEngineers[i].x = m_blueprintName.x + (int)m_blueprintName.sizeF.Width;
                else
                    m_blueprintEngineers[i].x = m_blueprintEngineers[i - 1].x + (int)m_blueprintEngineers[i - 1].sizeF.Width;
            }

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
