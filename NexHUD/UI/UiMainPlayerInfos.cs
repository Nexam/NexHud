using EliteAPI.Events;
using NexHUDCore;
using NexHUDCore.NxItems;
using System;
using System.Drawing;
using System.Reflection;

namespace NexHUD.Ui
{
    public class UiMainPlayerInfos : NxGroup
    {
        private NxMenu m_menu;
        private NxImage m_PlayerRank;
        private NxSimpleText m_PlayerName;
        private NxSimpleText m_ShipName;
        private NxSimpleText m_Balance;
        private NxSimpleText m_Rebuy;

        private long m_lastRebuy = 0;

        private string getImageForRank(int _conmbatRank, int _tradeRank, int _exploRank)
        {
            int _rank = 0;
            int _eliteCount = 0;
            if (_conmbatRank >= 8)
                _eliteCount++;
            if (_tradeRank >= 8)
                _eliteCount++;
            if (_exploRank >= 8)
                _eliteCount++;

            _rank = Math.Max(_conmbatRank, Math.Max(_tradeRank, _exploRank));

            if (_eliteCount >= 3)
                _rank = 10;
            else if (_eliteCount >= 2)
                _rank = 9;


            switch (_rank)
            {
                case 0: return "Resources.ranks.rank-1.png";
                case 1: return "Resources.ranks.rank-2.png";
                case 2: return "Resources.ranks.rank-3.png";
                case 3: return "Resources.ranks.rank-4.png";
                case 4: return "Resources.ranks.rank-5.png";
                case 5: return "Resources.ranks.rank-6.png";
                case 6: return "Resources.ranks.rank-7.png";
                case 7: return "Resources.ranks.rank-8.png";
                case 8: return "Resources.ranks.rank-9.png";
                case 9: return "Resources.ranks.double-elite.png";
                case 10: return "Resources.ranks.triple-elite.png";
            }
            return "Resources.ranks.rank-1.png";
        }
        public UiMainPlayerInfos(NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            x = 0;
            y = UiMainTopInfos.HEIGHT + 5;
            m_menu = _menu;

            NexHudMain.EliteApi.Events.RankEvent += onRankEvent;

            m_PlayerRank = new NxImage(x, y, ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), getImageForRank((int)NexHudMain.EliteApi.Commander.CombatRank, (int)NexHudMain.EliteApi.Commander.TradeRank, (int)NexHudMain.EliteApi.Commander.ExplorationRank)));
            Add(m_PlayerRank);

            m_PlayerName = new NxSimpleText(x + 64, y, "CMDR " + NexHudMain.EliteApi.Commander.Commander, EDColors.WHITE, 22);
            Add(m_PlayerName);

            m_ShipName = new NxSimpleText(x + 64, y + 24, "Unknow ship", EDColors.ORANGE, 22, NxFonts.EuroCapital);
            Add(m_ShipName);
            NexHudMain.EliteApi.Events.SetUserShipNameEvent += Events_SetUserShipNameEvent;

            m_Balance = new NxSimpleText(m_menu.frame.WindowWidth, y, string.Empty, EDColors.YELLOW, 20);
            Add(m_Balance);
            m_Rebuy = new NxSimpleText(m_menu.frame.WindowWidth, y + 24, string.Empty, EDColors.RED, 18);
            Add(m_Rebuy);
            NexHudMain.EliteApi.Events.LoadoutEvent += Events_LoadoutEvent;
        }

        private void Events_LoadoutEvent(object sender, LoadoutInfo e)
        {
            m_lastRebuy = e.Rebuy;
        }

        private void Events_SetUserShipNameEvent(object sender, SetUserShipNameInfo e)
        {
            m_ShipName.text = e.Ship;

        }

        private void onRankEvent(object sender, RankInfo e)
        {
            m_PlayerRank.Image = ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), getImageForRank((int)NexHudMain.EliteApi.Commander.CombatRank, (int)NexHudMain.EliteApi.Commander.TradeRank, (int)NexHudMain.EliteApi.Commander.ExplorationRank));
        }

        public override void Render(Graphics _g)
        {
            base.Render(_g);
            m_Balance.x = m_menu.frame.WindowWidth - (int)m_Balance.sizeF.Width;
            m_Rebuy.x = m_menu.frame.WindowWidth - (int)m_Rebuy.sizeF.Width;
        }
        public override void Update()
        {
            base.Update();

            m_Balance.text = string.Format("{0:#,0}", NexHudMain.EliteApi.Commander.Credits) + " cr";

            m_Rebuy.text = "Rebuy: " + string.Format("{0:#,0}", m_lastRebuy);

            if (m_lastRebuy > NexHudMain.EliteApi.Commander.Credits / 5)
                m_Rebuy.Color = EDColors.YELLOW;
            else if (m_lastRebuy > NexHudMain.EliteApi.Commander.Credits)
                m_Rebuy.Color = EDColors.RED;
            else
                m_Rebuy.Color = EDColors.getColor(EDColors.WHITE, 0.5f);

        }
    }
}
