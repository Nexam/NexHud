using System.Drawing;

namespace NexHUDCore.NxItems
{
    public abstract class NxItem
    {
        private Color m_Color;
        private Brush m_Brush;

        public NxGroup group;

        public Color Color
        {
            get { return m_Color; }
            set
            {
                if (m_Color != value)
                {
                    makeItDirty();
                    m_Color = value;
                    m_Brush = new SolidBrush(m_Color);
                }
            }
        }

        private int m_x;
        private int m_y;
        public int x { get { return m_x; } set { if (m_x != value) makeItDirty(); m_x = value; } }
        public int y { get { return m_y; } set { if (m_y != value) makeItDirty(); m_y = value; } }

        public Brush SolidBrush { get { return m_Brush; } }

        public bool isDirty = true;
        public void makeItDirty() { isDirty = true; }

        private bool m_isVisible = true;
        internal bool visIsUptodate = true;
        public virtual bool isVisible
        {
            get
            {
                if (group != null && !group.isVisible)
                        return false;
                return m_isVisible;
            }
            set
            {
                if (m_isVisible != value)
                {
                    visIsUptodate = false;
                    makeItDirty();
                }
                m_isVisible = value;
            }
        }

        public abstract void Update();
        public abstract void Render(Graphics _g);

    }
}
