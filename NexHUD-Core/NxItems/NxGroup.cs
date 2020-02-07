using System;
using System.Collections.Generic;
using System.Drawing;

namespace NexHUDCore.NxItems
{
    public class NxGroup : NxItem
    {
        private List<NxItem> m_nxItems = new List<NxItem>();
        private NxOverlay m_parent = null;
        private bool m_realativeChildPos = false;

        public NxOverlay Parent { get { return m_parent; } }


        public NxGroup(NxOverlay _parent)
        {
            m_parent = _parent;
        }
        public virtual void Add(NxItem _item)
        {
            m_nxItems.Add(_item);
            _item.group = this;
            makeItDirty();
        }
        public virtual void Remove(NxItem _item)
        {
            m_nxItems.Remove(_item);
            _item.group = null;
            makeItDirty();
        }

        public override bool isVisible 
        { 
            get => base.isVisible;
            set
            {
                base.isVisible = value;
            }
        }

        public bool RelativeChildPos { get => m_realativeChildPos; set => m_realativeChildPos = value; }
        public List<NxItem> Items { get => m_nxItems; }

        public override void Render(Graphics _g)
        {
            // SteamVR_NexHUD.Log("Render NxOverlay @ " + DateTime.Now.ToString());
            foreach (NxItem i in m_nxItems)
            {
                if (i.isVisible)
                {
                    if (m_parent.RenderDirtyBox && i.isDirty)
                        _g.DrawRectangle(Pens.Crimson, i.Rectangle);
                    i.Render(_g);
                }
                i.visIsUptodate = true;
                i.isDirty = false;
            }
            isDirty = false;
        }

        public override void Update()
        {
            bool _checkDirty = false;
            if (m_parent != null)
                _checkDirty = (m_parent.lastDirtyCheck + m_parent.dirtyCheckFreq < DateTime.Now);

            foreach (NxItem i in m_nxItems)
            {
                i.Update();
                if (_checkDirty)
                {
                    if ((i.isDirty && i.isVisible) || !i.visIsUptodate)
                        isDirty = true;
                }
            }
        }
    }
}
