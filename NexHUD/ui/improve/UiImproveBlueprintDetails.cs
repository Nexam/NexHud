using System.Drawing;
using System.Linq;
using NexHUD.Elite.Craft;
using NexHUD.Elite.Engineers;
using NexHUD.Inputs;
using NexHUD.Ui.Common;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Improve
{
    public class UiBlueprintDetails : NxGroup
    {
        const int ModY = 10;
        const int CostY = 350;
        const int EngineerY = 750;
        public const int Width = 290;
        public const int Height = 300;

        bool m_isExperimental;
        UiImproveBlueprintDetails m_uiBlueprintsDetails;

        NxSimpleText m_gradeText;
        NxSimpleText[] m_modifiers = new NxSimpleText[5];
        NxSimpleText[] m_costs = new NxSimpleText[5];

        NxButton m_prevButton;
        NxButton m_nextButton;

        BlueprintDatas m_datas;
        BlueprintDatas[] m_experimentals;
        public bool IsExperimental { get => m_isExperimental; }

        public UiBlueprintDetails(bool _isExperimetal, UiImproveBlueprintDetails _uiBlueprintsDetails) : base(_uiBlueprintsDetails.Parent)
        {
            m_uiBlueprintsDetails = _uiBlueprintsDetails;
            y = 105;
            m_isExperimental = _isExperimetal;
            x = IsExperimental ? (NxMenu.Width / 2) - (Width / 2) - 50 : 5;

            Add(new NxRectangle(x, y, Width, Height, EDColors.getColor(EDColors.WHITE, .05f)));
            m_gradeText = new NxSimpleText(x + 5, y + 2, "GRADE / EXP NAME.", EDColors.YELLOW, 20, NxFonts.EuroCapital);// { vertical = true };
            Add(m_gradeText);

            int _frameHeight = 110;
            //Frame Modifications
            NxRectangle _frameMod = new NxRectangle(x + 5, y + 50, Width - 10, _frameHeight, EDColors.getColor(EDColors.BLACK, 0.4f));
            Add(_frameMod);
            Add(new NxSimpleText(_frameMod.x, _frameMod.y - 20, "MODIFIERS", EDColors.getColor(EDColors.YELLOW, 0.5f)));
            //Frame Cost
            NxRectangle _frameCost = new NxRectangle(x + 5, y + Height - 5 - _frameHeight, Width - 10, _frameHeight, EDColors.getColor(EDColors.BLACK, 0.4f));
            Add(_frameCost);
            Add(new NxSimpleText(_frameCost.x, _frameCost.y - 20, "CRAFTING COST", EDColors.getColor(EDColors.YELLOW, 0.5f)));


            for (int i = 0; i < m_modifiers.Length; i++)
            {
                m_modifiers[i] = new NxSimpleText(_frameMod.x + 5, _frameMod.y + 5 + i * 20, "Modifier " + i.ToString(), EDColors.GRAY, 18);
                Add(m_modifiers[i]);
            }
            for (int i = 0; i < m_costs.Length; i++)
            {
                m_costs[i] = new NxSimpleText(_frameCost.x + 5, _frameCost.y + 5 + i * 20, "Cost " + i.ToString(), EDColors.GRAY, 18);
                Add(m_costs[i]);
            }

            m_prevButton = new NxButton(x, y + Height + 5, (Width / 2) - 2, 25, "<< Previous", _uiBlueprintsDetails.uiImprove.Menu);
            m_prevButton.Coords = new Point(IsExperimental ? 2 : 0, 0);
            m_nextButton = new NxButton(x + m_prevButton.width + 4, m_prevButton.y, m_prevButton.width, m_prevButton.height, "Next >>", _uiBlueprintsDetails.uiImprove.Menu);
            m_nextButton.Coords = new Point(IsExperimental ? 3 : 1, 0);
            Add(m_prevButton);
            Add(m_nextButton);
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



            m_prevButton.Selected = m_uiBlueprintsDetails.cursor == m_prevButton.Coords;
            m_nextButton.Selected = m_uiBlueprintsDetails.cursor == m_nextButton.Coords;

            bool select = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select));

            if (select)
            {
                if (!IsExperimental)
                {
                    if (m_prevButton.Selected && m_prevButton.isSelectable)
                        m_uiBlueprintsDetails.prevGrade();

                    if (m_nextButton.Selected && m_nextButton.isSelectable)
                        m_uiBlueprintsDetails.nexGrade();
                }
                else if (m_experimentals != null)
                {
                    if (m_prevButton.Selected && m_prevButton.isSelectable)
                        m_uiBlueprintsDetails.changeExperimental(m_prevButton.Obj as BlueprintDatas);

                    if (m_nextButton.Selected && m_nextButton.isSelectable)
                        m_uiBlueprintsDetails.changeExperimental(m_nextButton.Obj as BlueprintDatas);
                }
            }
        }
        public void setDatas(BlueprintDatas _datas, string _blueprintType)
        {
            m_datas = _datas;


            if (m_isExperimental)
            {
                if (_datas != null)
                {
                    m_gradeText.text = _datas.Name;
                }
                else
                    m_gradeText.text = "No Experimental";

                m_experimentals = EngineerHelper.getExperimentals(_blueprintType);
                m_prevButton.isSelectable = false;
                m_nextButton.isSelectable = false;

                m_prevButton.Obj = null;
                m_nextButton.Obj = null;

                if (m_experimentals != null)
                {
                    if (_datas != null)
                        m_prevButton.isSelectable = true;

                    for (int i = 0; m_experimentals != null && i < m_experimentals.Length; i++)
                    {
                        if (m_datas != null)
                        {

                            if (m_experimentals[i].Name == m_datas.Name && i < m_experimentals.Length - 1)
                            {
                                m_nextButton.isSelectable = true;
                                if (i > 0)
                                    m_prevButton.Obj = m_experimentals[i - 1];
                                m_nextButton.Obj = m_experimentals[i + 1];
                            }
                        }
                        else
                        {
                            m_nextButton.Obj = m_experimentals[0];
                            m_nextButton.isSelectable = true;
                            break;
                        }
                    }

                }
            }
            else
            {
                m_gradeText.text = string.Format("Grade {0}", m_datas.Grade);
                if (m_datas.Grade <= 1)
                    m_prevButton.isSelectable = false;
                else
                    m_prevButton.isSelectable = true;

                if (m_datas.Grade >= m_datas.MaxGrade)
                    m_nextButton.isSelectable = false;
                else
                    m_nextButton.isSelectable = true;
            }


            for (int i = 0; i < m_modifiers.Length; i++)
            {
                if (m_datas != null && i < m_datas.Effects.Length)
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
                if (m_datas != null && i < m_datas.Ingredients.Length)
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
        const string PIN_LABEL = "-  Pin this blueprint x {0}  +";
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
        NxSimpleText[] m_cargos = new NxSimpleText[15];

        NxSimpleText m_Tips;
        NxSimpleText m_NxSeachDescription;

        NxButton m_ButtonPin;
        NxButton m_ButtonDelete;

        public Point cursor = new Point();

        private int m_materialEntrys = 0;

        private int m_pinCount = 1;
        public UiImprove uiImprove { get => m_uiImprove; }

        private BlueprintDatas m_experimental = null;

        private bool m_fromCraftList = false;
        private CraftlistItem m_displayedCraft = null;

        public CraftlistItem displayedCraft { get => m_displayedCraft; set => m_displayedCraft = value; }
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

            Add(new NxSimpleText(NxMenu.Width - 375, 105, "[Cargo]", EDColors.GRAY, 20, NxFonts.EuroStile) { centerHorizontal = true });

            Add(new NxSimpleText(NxMenu.Width - 345, 105, "All materials (click to search)", EDColors.YELLOW, 20, NxFonts.EuroStile));
            //Material list
            for (int i = 0; i < m_materials.Length; i++)
            {
                m_materials[i] = new NxButton(NxMenu.Width - 345, 130 + i * 28, 325, 26, "Material " + i, m_uiImprove.Menu);

                m_materials[i].ColorBack = EDColors.BLACK;
                m_materials[i].ColorBackSelected = EDColors.WHITE;
                m_materials[i].ColorLabel = EDColors.LIGHTBLUE;
                Add(m_materials[i]);

                m_cargos[i] = new NxSimpleText(NxMenu.Width - 375, 15 + 130 + i * 28, "0", EDColors.GRAY) { centerHorizontal = true, centerVertical = true };
                Add(m_cargos[i]);
            }

            m_BlueprintDetails = new UiBlueprintDetails(false, this);
            Add(m_BlueprintDetails);
            m_BlueprintDetailsExperimental = new UiBlueprintDetails(true, this);
            Add(m_BlueprintDetailsExperimental);


            m_Tips = new NxSimpleText(5, 165 + UiBlueprintDetails.Height, "Place holder for tips", EDColors.BLUE, 20, NxFonts.EuroStile);
            Add(m_Tips);
            m_NxSeachDescription = new NxSimpleText(5, 190 + UiBlueprintDetails.Height, "Place holder for search desc", EDColors.YELLOW, 20, NxFonts.EuroStile);
            Add(m_NxSeachDescription);

            m_ButtonPin = new NxButton(5, NxMenu.Height - 50, NxMenu.Width - 10, 40, string.Format(PIN_LABEL, m_pinCount), m_uiImprove.Menu);
            m_ButtonPin.ColorBack = EDColors.getColor(EDColors.GREEN, 0.1f);
            m_ButtonPin.ColorBackSelected = EDColors.getColor(EDColors.GREEN, 0.8f);
            m_ButtonPin.LabelTextSize = 22;
            m_ButtonPin.Coords = new Point(0, 100);
            Add(m_ButtonPin);

            m_ButtonDelete = new NxButton(5, NxMenu.Height - 50, NxMenu.Width - 10, 40, "/!\\ DELETE /!\\", m_uiImprove.Menu);
            m_ButtonDelete.ColorBack = EDColors.getColor(EDColors.RED, 0.1f);
            m_ButtonDelete.ColorBackSelected = EDColors.getColor(EDColors.RED, 0.8f);
            m_ButtonDelete.LabelTextSize = 22;
            m_ButtonDelete.ColorLabel = EDColors.RED;
            m_ButtonDelete.ColorLabelSelected = EDColors.WHITE;
            m_ButtonDelete.Coords = new Point(0, 101);
            Add(m_ButtonDelete);
        }

        public void setBlueprint(CraftlistItem _item)
        {
            m_blueprint = _item.blueprint;
            m_experimental = _item.experimentBlueprint;
            m_fromCraftList = true;
            m_pinCount = _item.count;
            m_displayedCraft = _item;
            cursor = new Point();
            updateContent();
        }
        public void setBlueprint(BlueprintDatas _blueprint)
        {
            m_blueprint = _blueprint;
            if (m_experimental != null)
            {
                if (m_blueprint == null || (m_experimental.Type != m_blueprint.Type))
                    m_experimental = null;
            }
            m_fromCraftList = false;
            m_displayedCraft = null;
            m_pinCount = 1;
            cursor = new Point();
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

            /* BlueprintDatas d = null;
             while (d == null && _maxGrade > 0)
             {
                 _maxGrade--;
                 d = EngineerHelper.blueprints.Where(x => x.Type == m_blueprint.Type && x.Name == m_blueprint.Name && x.Grade == _maxGrade).FirstOrDefault();
             }
             if (d != null)
                 m_BlueprintDetails.setDatas(d, d.Type);*/
            m_BlueprintDetails.setDatas(m_blueprint, m_blueprint.Type);
            //experimental
            m_BlueprintDetailsExperimental.setDatas(m_experimental, m_blueprint.Type);


            m_materialEntrys = 0;
            MaterialDatas[] _needed = EngineerHelper.getAllCraftMaterials(m_blueprint.Type, m_blueprint.Name, m_experimental != null ? m_experimental.Name : string.Empty, m_blueprint.Grade);
            //list of all materials needed with all rolls
            for (int i = 0; i < m_materials.Length; i++)
            {
                m_materials[i].Coords = new Point(4, i);
                if (i < _needed.Length)
                {
                    m_materialEntrys++;
                    m_materials[i].Obj = _needed[i];
                    m_materials[i].isVisible = true;
                    m_cargos[i].isVisible = true;
                    m_materials[i].Label = string.Format("{0} x {1}", _needed[i].Quantity, _needed[i].Name);

                    int _cargo = EngineerHelper.getCmdrMaterials(_needed[i].Name);
                    m_materials[i].ColorLabel = _cargo >= _needed[i].Quantity ? EDColors.BLUE : EDColors.RED;
                    m_cargos[i].text = _cargo.ToString();

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
                {
                    m_materials[i].isVisible = false;
                    m_cargos[i].isVisible = false;
                }
            }

            if( m_fromCraftList )
            {
                m_ButtonDelete.isVisible = true;
                m_ButtonPin.y = NxMenu.Height - 105;
                m_ButtonPin.repos();

               
            }
            else
            {
                m_ButtonDelete.isVisible = false;
                m_ButtonPin.y = NxMenu.Height - 50;
                m_ButtonPin.repos();
            }

            m_updatePositions = true;
        }
        private void updatePositions()
        {
            m_blueprintName.x = 5 + (int)m_blueprintType.sizeF.Width;

            for (int i = 0; i < m_blueprintEngineers.Length; i++)
            {
                if (i == 0)
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

        public void prevGrade()
        {
            if (m_blueprint != null && m_blueprint.Grade > 1)
                changeGrade(m_blueprint.Grade - 1);
        }
        public void nexGrade()
        {
            if (m_blueprint != null && m_blueprint.Grade < m_blueprint.MaxGrade)
                changeGrade(m_blueprint.Grade + 1);
        }
        private void changeGrade(int _newGrade)
        {
            BlueprintDatas _newdatas = EngineerHelper.blueprints.Where(x => x.Type == m_blueprint.Type && x.Name == m_blueprint.Name && x.Grade == _newGrade).FirstOrDefault();
            if (_newdatas != null)
            {
                m_blueprint = _newdatas;
                if (m_experimental != null)
                {
                    if (m_blueprint == null || (m_experimental.Type != m_blueprint.Type))
                        m_experimental = null;
                }

                if( m_displayedCraft != null )
                {
                    m_displayedCraft.grade = m_blueprint.Grade;
                }
               

                updateContent();
            }
        }
        public void changeExperimental(BlueprintDatas _newExperimental)
        {
            m_experimental = _newExperimental;

            if (m_displayedCraft != null)
            {
                m_displayedCraft.grade = m_blueprint.Grade;
                m_displayedCraft.experimental = m_experimental != null ? m_experimental.Name : string.Empty;
                m_displayedCraft.compile();
            }

            updateContent();
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

            //Cursor on Prev/Next
            if (cursor.X <= 3 && cursor.Y == 0)
            {
                if (cursor.X > 0 && left)
                    cursor.X--;
                else if (right)
                {
                    if (cursor.X < 3)
                        cursor.X++;
                    else //Materials
                    {
                        cursor = new Point(4, 0);
                    }
                }
                else if (down)
                {                   
                    cursor.Y = m_ButtonPin.Coords.Y;
                }
            }
            //Cursor on materials
            else if (cursor.X >= 4 && cursor.Y <= m_materialEntrys)
            {
                if (left)
                    cursor = new Point(3, 0);
                else if (down)
                {
                    if (cursor.Y < m_materialEntrys - 1)
                        cursor.Y++;
                    else
                        cursor.Y = m_ButtonPin.Coords.Y;
                }
                else if (up && cursor.Y > 0)
                    cursor.Y--;

            }
            //Cursor on pin/delete button
            else
            {
                if (up)
                {
                    if (cursor.Y < 100)
                        cursor.Y++;
                    else
                    {
                        if (cursor.X <= 3)
                            cursor.Y = 0;
                        else
                            cursor.Y = m_materialEntrys - 1;
                    }
                }
                else if (right && m_pinCount < 10 && cursor.Y == m_ButtonPin.Coords.Y)
                {                 
                    m_pinCount++; 
                    if (m_displayedCraft != null) m_displayedCraft.count = m_pinCount;
                }
                else if (left && m_pinCount > 1 && cursor.Y == m_ButtonPin.Coords.Y)
                {
                    m_pinCount--;
                    if (m_displayedCraft != null) m_displayedCraft.count = m_pinCount;
                }
                else if (down && m_fromCraftList)
                {
                    cursor.Y = m_ButtonDelete.Coords.Y;
                }

            }

            m_ButtonPin.Label = string.Format(PIN_LABEL, m_pinCount);

            NxButton _materialBtnSelected = null;
            ////SELECTEDS
            foreach (NxButton b in m_materials)
            {
                b.Selected = b.Coords == cursor;
                if (b.Selected)
                    _materialBtnSelected = b;
            }

            m_ButtonPin.Selected = cursor.Y == m_ButtonPin.Coords.Y;

            m_ButtonDelete.Selected = cursor.Y == m_ButtonDelete.Coords.Y;

            //Mat tools tips
            if (_materialBtnSelected != null && _materialBtnSelected.Obj != null)
            {
                string _tips = "";
                foreach (string tip in ((MaterialDatas)_materialBtnSelected.Obj).OriginDetails)
                    _tips += tip + " |";
                _tips = _tips.Substring(0, _tips.Length - 1).Trim();
                m_Tips.text = _tips;

                if (((MaterialDatas)_materialBtnSelected.Obj).nxSearch != null)
                {
                    m_NxSeachDescription.text = ((MaterialDatas)_materialBtnSelected.Obj).nxSearch.Description;
                    if (select)
                    {
                        m_uiImprove.changeState(UiImprove.UiImproveState.Search);
                        m_uiImprove.search.processSearch(((MaterialDatas)_materialBtnSelected.Obj).nxSearch, _tips, ((MaterialDatas)_materialBtnSelected.Obj).Name);
                    }
                }
                else
                    m_NxSeachDescription.text = string.Empty;
            }
            else
            {
                m_Tips.text = string.Empty;
                m_NxSeachDescription.text = string.Empty;
            }

            //Pin that!
            if( m_ButtonPin.Selected && select && !m_fromCraftList)
            {
                CraftlistItem craftItem = CraftlistItem.create(m_blueprint, m_experimental, m_pinCount);
                Craftlist.Add(craftItem);
                m_uiImprove.changeState(UiImprove.UiImproveState.CraftList);

            }
            //Delete that !
            if( m_ButtonDelete.Selected && select && m_fromCraftList )
            {
                Craftlist.Delete(m_displayedCraft);
                m_uiImprove.changeState(UiImprove.UiImproveState.CraftList);
            }
        }
    }
}
