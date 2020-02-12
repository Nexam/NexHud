using NexHUDCore.NxItems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Ui.Common
{
    public class NxGroupAutoCursor : NxGroup
    {
        private Rectangle m_VirtualCursor;
        private bool m_renderCursor = true;
        public NxGroupAutoCursor(NxOverlay _parent) : base(_parent)
        {
            m_VirtualCursor = new Rectangle(0, 0, 30, 30);
        }

        public void setCursorSize(int _width, int _height)
        {
            m_VirtualCursor.Width = _width;
            m_VirtualCursor.Height = _height;
        }

        public void MoveCursorToFirst()
        {
            ISelectable first = (ISelectable)Items.Where(x => x is ISelectable ).FirstOrDefault();
            if (first != null)
            {
                first.Selected = true;
                moveCursorTo(first);
            }
            else
                m_VirtualCursor = new Rectangle(Rectangle.Width / 2, Rectangle.Y, m_VirtualCursor.Width, m_VirtualCursor.Height);
            makeItDirty();

        }
        public void moveCursorTo(ISelectable item)
        {
            Rectangle r = item.Rectangle;
            m_VirtualCursor = new Rectangle(
                    r.X + (r.Width / 2) - (m_VirtualCursor.Width / 2),
                    r.Y + (r.Height / 2) - (m_VirtualCursor.Height / 2),
                    m_VirtualCursor.Width,
                    m_VirtualCursor.Height
                );
            makeItDirty();
        }

        public ISelectable GetSelectableUnderCursor()
        {
            foreach (NxItem item in Items)
            {
                if (item is ISelectable)
                {
                    if (item.isVisible && ((ISelectable)item).isSelectable && item.Rectangle.IntersectsWith(m_VirtualCursor))
                    {
                        return (ISelectable)item;
                    }
                }
            }
            return null;
        }

        public void moveRight()
        {
            MoveToSelect(new Point(1, 0));
        }
        public void moveLeft()
        {
            MoveToSelect(new Point(-1, 0));
        }
        public void moveUp()
        {
            MoveToSelect(new Point(0, -1));
        }
        public void moveDown()
        {
            MoveToSelect(new Point(0, 1));
        }
        public void MoveToSelect(Point vector)
        {
            ISelectable under = GetSelectableUnderCursor();
            ISelectable lastValidUnder = under;
            Rectangle _lastValidCursor = new Rectangle(Rectangle.Width / 2, Rectangle.Y, m_VirtualCursor.Width, m_VirtualCursor.Height);//m_VirtualCursor;
            if (under != null)
            {
                moveCursorTo(under);
                _lastValidCursor = m_VirtualCursor;
                m_VirtualCursor.X += vector.X * ((under.Rectangle.Width / 2) + (m_VirtualCursor.Width / 2) + 2);
                m_VirtualCursor.Y += vector.Y * ((under.Rectangle.Height / 2) + (m_VirtualCursor.Height / 2) + 2);

                under = GetSelectableUnderCursor();
                if (under != null)
                {
                    moveCursorTo(under);
                    _lastValidCursor = m_VirtualCursor;
                }
            }

            while (under == null && Rectangle.IntersectsWith(m_VirtualCursor))
            {
                m_VirtualCursor.X += vector.X * m_VirtualCursor.Width;
                m_VirtualCursor.Y += vector.Y * m_VirtualCursor.Height;
                under = GetSelectableUnderCursor();
                if (under != null)
                {
                    moveCursorTo(under);
                    _lastValidCursor = m_VirtualCursor;
                }
            }
            m_VirtualCursor = _lastValidCursor;

            if (under == null)
                under = lastValidUnder;

            foreach (NxItem item in Items)
            {
                if (item is ISelectable)
                {
                    if (!((ISelectable)item).isSelectable)
                        ((ISelectable)item).Selected = false;
                    else
                        ((ISelectable)item).Selected = item == under;
                }
            }

            if (m_renderCursor)
                makeItDirty();
        }
        public override void Render(Graphics _g)
        {
            base.Render(_g);
            if (m_renderCursor)
            {
                Pen p = new Pen(Color.Aqua, 2);
                _g.DrawRectangle(p, m_VirtualCursor);
            }
        }

    }
}
