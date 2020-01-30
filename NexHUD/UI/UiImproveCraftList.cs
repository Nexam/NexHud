using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.UI
{
    class UiImproveCraftlist : NxGroup
    {
        UiImprove m_uiImprove;
        bool m_firstUpdateSkipped = false;
        public UiImproveCraftlist(UiImprove _uiImprove) : base(_uiImprove.Menu.frame.NxOverlay)
        {
            m_uiImprove = _uiImprove;
            Add(new NxSimpleText(100, 100, "Place holder for blueprints...",EDColors.LIGHTBLUE,  24));
            Add(new NxSimpleText(100, 130, "hit 'select' to continue", EDColors.YELLOW, 18));
        }
        public override void Update()
        {
            base.Update();
            if (!isVisible)
            {
                m_firstUpdateSkipped = false;
                return;
            }

            if(!m_firstUpdateSkipped)
            {
                m_firstUpdateSkipped = true;
                return;
            }

            if (NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select)))
                m_uiImprove.changeState(UiImprove.UiImproveState.Blueprints);
        }
    }
}
