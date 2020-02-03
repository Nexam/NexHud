using NexHUDCore;
using NexHUDCore.NxItems;
using System.Drawing;
using System.Reflection;

namespace NexHUD.ui
{
    public class UiMainMenuButton : NxGroup
    {
        public enum MenuButtonType
        {
            Search,
            Improve,
            Trade
        }
        public const int WIDTH = 140;
        public const int HEIGHT = 300;
        public const int ICON_SIZE = 128;
        public const int SELECTED_ADD = 2;

        private Rectangle m_originRec = new Rectangle();
        private MenuButtonType m_Type = MenuButtonType.Search;
        private NxRectangle m_background;
        private NxImage m_Icon;
        private NxSimpleText m_TopText;
        private NxTextbox m_BottomText;

        private bool m_isSelected = false;
        private bool m_isActive = true;

        public bool isSelected { get { return m_isSelected; } set { m_isSelected = value; } }
        public bool isActive { get { return m_isActive; } set { m_isActive = value; } }

        public MenuButtonType menuButtonType { get { return m_Type; } }

        private string getIconPath()
        {
            switch (m_Type)
            {
                case MenuButtonType.Improve: return "Resources.MenuImages.menu_improve.png";
                case MenuButtonType.Search: return "Resources.MenuImages.menu_search.png";
                case MenuButtonType.Trade: return "Resources.MenuImages.menu_trade.png";
            }
            return "Resources.MenuImages.menu_search.png";
        }
        private string getMenuTitle()
        {
            switch (m_Type)
            {
                case MenuButtonType.Improve: return "Improve";
                case MenuButtonType.Search: return "Search";
                case MenuButtonType.Trade: return "Trade";
            }
            return "UNKNOW";
        }
        private string getBottomText()
        {
            switch (m_Type)
            {
                case MenuButtonType.Improve: return "See your craft list from Inara and more";
                case MenuButtonType.Search: return "Search engine based on EDDB & EDSM Datas";
                case MenuButtonType.Trade: return "Find your next trade route and more";
            }
            return "UNKNOW";
        }
        /// <summary>
        /// The button is centered on the position;
        /// </summary>
        /// <param name="_parent"></param>
        public UiMainMenuButton(int _x, int _y, MenuButtonType _type, NxOverlay _parent) : base(_parent)
        {
            x = _x;
            y = _y;
            m_Type = _type;

            //background
            Color = EDColors.getColor(EDColors.ORANGE, 0.2f);
            m_originRec = new Rectangle(x - (WIDTH / 2), y - (HEIGHT / 2), WIDTH, HEIGHT);
            m_background = new NxRectangle(m_originRec.X, m_originRec.Y, m_originRec.Width, m_originRec.Height, Color);
            Add(m_background);

            //Title
            m_TopText = new NxSimpleText(x, m_background.y + 5, getMenuTitle(), EDColors.ORANGE, 34, NxFonts.EuroCapital);
            m_TopText.centerHorizontal = true;
            Add(m_TopText);

            //Icon
            m_Icon = new NxImage(m_background.x + (WIDTH - ICON_SIZE) / 2, m_background.y + 50, ResHelper.GetResourceImage(Assembly.GetExecutingAssembly(), getIconPath()));
            Add(m_Icon);

            //Bottom Text
            int _botHeight = 80;
            int _padding = 5;
            m_BottomText = new NxTextbox(m_background.x + _padding, m_background.y + (HEIGHT - _botHeight) - _padding, WIDTH - _padding * 2, _botHeight, getBottomText(), EDColors.ORANGE, 19);
            m_BottomText.showBounds = true;
            m_BottomText.boundColors = EDColors.getColor(EDColors.ORANGE, .5f);
            Add(m_BottomText);

        }

        public override void Render(Graphics _g)
        {
            base.Render(_g);
        }

        public override void Update()
        {
            base.Update();

            if (isSelected)
            {
                m_background.x = m_originRec.X - SELECTED_ADD;
                m_background.y = m_originRec.Y - SELECTED_ADD;
                m_background.width = m_originRec.Width + SELECTED_ADD * 2;
                m_background.height = m_originRec.Height + SELECTED_ADD * 2;
                m_background.Color = EDColors.getColor(isActive ? EDColors.ORANGE : Color.Gray, 0.6f);
                if (isActive)
                {
                    m_BottomText.Color = EDColors.YELLOW;
                    m_TopText.Color = EDColors.YELLOW;
                }
            }
            else
            {
                m_background.x = m_originRec.X;
                m_background.y = m_originRec.Y;
                m_background.width = m_originRec.Width;
                m_background.height = m_originRec.Height;
                m_background.Color = EDColors.getColor(isActive ? EDColors.ORANGE : Color.Gray, 0.2f);
                m_BottomText.Color = EDColors.ORANGE;
                m_TopText.Color = EDColors.ORANGE;
            }
        }
    }
}
