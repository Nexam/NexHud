using NexHUD.elite;
using NexHUDCore;
using NexHUDCore.NxItems;
using System;
using System.Drawing;
using System.Reflection;

namespace NexHUD.ui
{
    public class UiMainTopInfos : NxGroup
    {
        public const int HEIGHT = 66;
        NxMenu m_menu;

        NxGroup m_Content;
        NxLoading m_Loading;

        NxImage m_AllegianceLogo;
        EliteAllegiance m_lastAllegiance;

        NxSimpleText m_SystemName;
        NxSimpleText m_ControllingFaction;
        NxSimpleText m_GovAndPop;
        NxSimpleText m_Economy;
        NxSimpleText m_SecurityLabel;
        NxSimpleText m_Security;
        NxSimpleText m_ThreatLabel;
        NxSimpleText m_Threat;
        NxSimpleText m_Reserve;
        NxSimpleText m_SystemValue;
        NxSimpleText m_SystemValueMapped;
        NxSimpleText m_traffic;
        int Column1 = 0;
        int decal_x = 50;
        private float infoCheck = 0;
        public UiMainTopInfos(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            m_menu = _menu;
            //Loading & Content;
            m_Content = new NxGroup(Parent);
            Add(m_Content);
            m_Content.isVisible = false;
            m_Loading = new NxLoading(NxMenu.Width / 2, 30);
            Add(m_Loading);

            //Decoration
            Add(new NxImage(0, 0, NxMenu.Width, HEIGHT * 2, ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), "Resources.GradientOrange15p.png")));
            Add(new NxRectangle(0, HEIGHT, NxMenu.Width, 1, EDColors.YELLOW));

            //* Faction icon *//
            m_AllegianceLogo = new NxImage(0, 0, ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), "Resources.factions.inde64.png"));
            m_Content.Add(m_AllegianceLogo);
            //* -- Titles **//
            //System Name
            m_SystemName = new NxSimpleText(decal_x, 0, string.Empty, EDColors.ORANGE, 40, NxFonts.EuroCapital);
            m_Content.Add(m_SystemName);
            //Controlling Factions
            m_ControllingFaction = new NxSimpleText(decal_x + 5, 35, string.Empty, EDColors.YELLOW, 20, NxFonts.EuroCapital);
            m_Content.Add(m_ControllingFaction);

            //* -- Descriptions **//
            //Government & Population
            Column1 = 0;

            int infoSize = 18;

            m_GovAndPop = new NxSimpleText(Column1, 2, string.Empty, EDColors.getColor(EDColors.WHITE, 0.5f), infoSize, NxFonts.EuroStile);
            m_Content.Add(m_GovAndPop);
            //Economy
            m_Economy = new NxSimpleText(Column1, 20, string.Empty, EDColors.BLUE, infoSize, NxFonts.EuroStile);
            m_Content.Add(m_Economy);
            //Security
            m_SecurityLabel = new NxSimpleText(Column1, 40, "Security:", EDColors.getColor(EDColors.WHITE, 0.5f), infoSize, NxFonts.EuroStile);
            m_Content.Add(m_SecurityLabel);
            m_Security = new NxSimpleText(Column1, 40, string.Empty, EDColors.BLUE, infoSize, NxFonts.EuroStile);
            m_Content.Add(m_Security);
            //Threat
            m_ThreatLabel = new NxSimpleText(Column1, 40, "Threat:", EDColors.getColor(EDColors.WHITE, 0.5f), infoSize, NxFonts.EuroStile);
            m_Content.Add(m_ThreatLabel);
            m_Threat = new NxSimpleText(Column1, 40, string.Empty, EDColors.BLUE, infoSize, NxFonts.EuroStile);
            m_Content.Add(m_Threat);
            //Traffic
            m_traffic = new NxSimpleText(0, 40, string.Empty, EDColors.getColor(EDColors.WHITE, 0.5f), infoSize, NxFonts.EuroStile);
            m_Content.Add(m_traffic);

            //* -- Column 2 -- *//
            //reserve
            m_Reserve = new NxSimpleText(0, 2, string.Empty, EDColors.getColor(EDColors.WHITE, 0.5f), infoSize, NxFonts.EuroStile);
            m_Content.Add(m_Reserve);
            //System Value
            m_SystemValue = new NxSimpleText(0, 20, string.Empty, EDColors.LIGHTBLUE, infoSize, NxFonts.EuroStile);
            m_Content.Add(m_SystemValue);
            //System Value Mapped
            m_SystemValueMapped = new NxSimpleText(0, 40, string.Empty, EDColors.BLUE, infoSize, NxFonts.EuroStile);
            m_Content.Add(m_SystemValueMapped);


        }

        public override void Render(Graphics _g)
        {
            base.Render(_g);
            if (EDDatas.Instance.getCurrentSystem().receivedEddbInfos)
            {
                Column1 = decal_x + (int)Math.Max(m_SystemName.sizeF.Width, m_ControllingFaction.sizeF.Width) + 5;
                m_GovAndPop.x = Column1;
                m_Economy.x = Column1;
                m_SecurityLabel.x = Column1;
                m_Security.x = Column1 + (int)m_SecurityLabel.sizeF.Width;
                m_ThreatLabel.x = Column1 + 5 + (int)m_Security.sizeF.Width + (int)m_SecurityLabel.sizeF.Width;
                m_Threat.x = m_ThreatLabel.x + (int)m_ThreatLabel.sizeF.Width;
                m_traffic.x = m_Threat.x + 5 + (int)m_Threat.sizeF.Width;

                m_Reserve.x = NxMenu.Width - (int)m_Reserve.sizeF.Width;
                m_SystemValue.x = NxMenu.Width - (int)m_SystemValue.sizeF.Width;
                m_SystemValueMapped.x = NxMenu.Width - (int)m_SystemValueMapped.sizeF.Width;
            }


        }
        public override void Update()
        {
            base.Update();
            if (EDDatas.Instance.getCurrentSystem().receivedEddbInfos)
            {
                infoCheck += NexHudEngine.deltaTime;
                if (infoCheck > 1)
                {
                    infoCheck = 0;
                    m_Content.isVisible = true;
                    m_Loading.isVisible = false;

                    EliteAllegiance _allegiance = EDDatas.Instance.getCurrentSystem().allegiance;
                    if (_allegiance != m_lastAllegiance)
                    {
                        m_lastAllegiance = _allegiance;
                        switch (_allegiance)
                        {
                            case EliteAllegiance.Alliance: m_AllegianceLogo.Image = ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), "Resources.factions.alliance64.png"); break;
                            case EliteAllegiance.Empire: m_AllegianceLogo.Image = ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), "Resources.factions.empire64.png"); break;
                            case EliteAllegiance.Federation: m_AllegianceLogo.Image = ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), "Resources.factions.federation64.png"); break;
                            case EliteAllegiance.Independent: m_AllegianceLogo.Image = ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), "Resources.factions.inde64.png"); break;
                        }

                    }

                    m_SystemName.text = EDDatas.Instance.getCurrentSystem().name.FirstCharToUpper();
                    m_ControllingFaction.text = EDDatas.Instance.getCurrentSystem().controlling_minor_faction.FirstCharToUpper();
                    m_GovAndPop.text = EDDatas.Instance.getCurrentSystem().government + ". Population: " + String.Format("{0:#,##0}", EDDatas.Instance.getCurrentSystem().population);
                    m_Economy.text = EDDatas.Instance.getCurrentSystem().economy.ToStringFormated();
                    if (EDDatas.Instance.getCurrentSystem().secondEconomy != EliteEconomy.Unknow && EDDatas.Instance.getCurrentSystem().secondEconomy != EDDatas.Instance.getCurrentSystem().economy)
                        m_Economy.text += " (" + EDDatas.Instance.getCurrentSystem().secondEconomy.ToStringFormated() + ")";
                    m_Security.Color = EliteSecurityHelper.getColor(EDDatas.Instance.getCurrentSystem().security);
                    m_Security.text = EDDatas.Instance.getCurrentSystem().security.ToString();
                    m_Threat.Color = EliteSystemThreatHelper.getColor(EDDatas.Instance.getCurrentSystem().Threat);
                    m_Threat.text = EDDatas.Instance.getCurrentSystem().Threat.ToString();
                    m_Reserve.text = "Reserve: " + EDDatas.Instance.getCurrentSystem().reserve;
                    m_SystemValue.text = "Est. Value: " + string.Format("{0:#,0}", EDDatas.Instance.getCurrentSystem().value) + " cr";
                    m_SystemValueMapped.text = "Est. Mapped Value: " + string.Format("{0:#,0}", EDDatas.Instance.getCurrentSystem().valueMapped) + " cr";
                    m_traffic.text = "Traffic (24h): " + EDDatas.Instance.getCurrentSystem().traffic_day;

                }
            }
            else
            {
                m_Content.isVisible = false;
                m_Loading.isVisible = true;
            }
        }
    }
}
