using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUDCore.NxItems
{
    public class NxGroup : NxItem
    {
        private List<NxItem> m_nxItems = new List<NxItem>();
        private NxOverlay m_parent = null;

        public NxOverlay Parent { get { return m_parent; } }

        public NxGroup(NxOverlay _parent)
        {
            m_parent = _parent;
        }
        public void Add(NxItem _item)
        {
            m_nxItems.Add(_item);
            makeItDirty();
        }
        public void Remove(NxItem _item)
        {
            m_nxItems.Remove(_item);
            makeItDirty();
        }

        public override void Render(Graphics _g)
        {
            // SteamVR_NexHUD.Log("Render NxOverlay @ " + DateTime.Now.ToString());
            foreach (NxItem i in m_nxItems)
            {
                if (i.isVisible)
                    i.Render(_g);
                i.visIsUptodate = true;
                i.isDirty = false;
            }
            isDirty = false;
        }
        
        public override void Update()
        {
            bool _checkDirty = false;
            if( m_parent != null )
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
