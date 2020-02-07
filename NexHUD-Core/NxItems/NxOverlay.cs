using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace NexHUDCore.NxItems
{
    public class NxOverlay : NxItem
    {
        //debug
        public bool RenderDirtyBox = false;
        private ulong m_RenderCount = 0;
        public bool RenderCount = false;

        private bool m_drawDefaultBackground;
        private NexHudOverlay m_parent;

        private List<NxItem> m_nxItems = new List<NxItem>();

        private DateTime m_lastDirtyCheck = DateTime.Now;
        public TimeSpan dirtyCheckFreq = TimeSpan.FromSeconds(1f);

        public DateTime lastDirtyCheck { get { return m_lastDirtyCheck; } }

        public bool LogRenderTime = true;

        public override bool isVisible
        {
            get => base.isVisible;
            set
            {
                base.isVisible = value;
            }
        }

        public NxOverlay(NexHudOverlay _parent, bool _drawDefaultBackground = true)
        {
            m_parent = _parent;
            m_drawDefaultBackground = _drawDefaultBackground;
        }

        public void Add(NxItem _item)
        {
            m_nxItems.Add(_item);
            if (_item.isVisible)
                makeItDirty();
        }
        public void Remove(NxItem _item)
        {
            m_nxItems.Remove(_item);
            makeItDirty();
        }
        public override void Render(Graphics _g)
        {
            if (isDirty)
            {
                m_RenderCount++;
                Stopwatch _watch = new Stopwatch();
                _watch.Start();

                if (m_drawDefaultBackground)
                {
                    _g.FillRegion(new SolidBrush(EDColors.BACKGROUND), _g.Clip);
                    int _u = 2;
                    _g.FillRectangle(new SolidBrush(EDColors.ORANGE), new Rectangle(0, 0, m_parent.WindowWidth, _u));
                    _g.FillRectangle(new SolidBrush(EDColors.ORANGE), new Rectangle(0, m_parent.WindowHeight - _u, m_parent.WindowWidth, _u));
                }
                foreach (NxItem i in m_nxItems)
                {
                    if (i.isVisible)
                    {
                        if (RenderDirtyBox && i.isDirty)
                            _g.DrawRectangle(Pens.Crimson, i.Rectangle);
                        i.Render(_g);
                    }
                    i.isDirty = false;
                    i.visIsUptodate = true;
                }
                isDirty = false;

                _watch.Stop();

                if (LogRenderTime && _watch.ElapsedMilliseconds > 100)
                    NexHudEngine.Log("NxOverlay " + this + " rendered in " + _watch.ElapsedMilliseconds + "ms");

                if (RenderCount)
                {
                    _g.FillRectangle(Brushes.White, new Rectangle(0, m_parent.WindowHeight - 20, 300, 20));
                    _g.DrawString(m_RenderCount.ToString(), NxFont.getFont(NxFonts.EuroStile, 17), Brushes.Crimson, 0, m_parent.WindowHeight - 20);
                    SizeF s = _g.MeasureString(m_RenderCount.ToString(), NxFont.getFont(NxFonts.EuroStile, 17) );
                    _g.DrawString(string.Format("// {0}ms" , _watch.ElapsedMilliseconds), NxFont.getFont(NxFonts.EuroStile, 17), Brushes.Crimson, s.Width + 5, m_parent.WindowHeight - 20);
                }
            }
        }

        public override void Update()
        {
            bool _checkDirty = (m_lastDirtyCheck + dirtyCheckFreq < DateTime.Now);

            foreach (NxItem i in m_nxItems)
            {
                i.Update();
                if (_checkDirty)
                {
                    if ((i.isDirty && i.isVisible) || !i.visIsUptodate)
                        isDirty = true;
                }
            }
            if (_checkDirty)
                m_lastDirtyCheck = DateTime.Now;

        }

    }
}
