using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUDCore.NxItems;

namespace NexHUD.Ui.Common
{
    public class NxGroupOrganized : NxGroup
    {
        private List<NxButton>[,] m_grid;
        private int m_GridWidth = 1;
        private int m_GridHeight = 1;
        private Point m_Cursor = new Point();
        public NxGroupOrganized(NxOverlay _parent) : base(_parent)
        {
        }

        public int GridWidth { get => m_GridWidth;}
        public int GridHeight { get => m_GridHeight;}
        public Point Cursor { get => m_Cursor; set => m_Cursor = value; }

        public void InitGrid(int _GridWidth, int _GridHeight)
        {
            m_GridWidth = _GridWidth;
            m_GridHeight = _GridHeight;
            m_grid = new List<NxButton>[_GridWidth, _GridHeight];

            for(int gy = 0; gy < m_GridHeight; gy++ )
            {
                for (int gx = 0; gx < m_GridWidth; gx++)
                {
                    m_grid[gx, gy] = new List<NxButton>();
                }
            }
        }
        public override void Add(NxItem _item)
        {
            base.Add(_item);
            if (_item is NxButton)
                m_grid[((NxButton)_item).Coords.X, ((NxButton)_item).Coords.Y].Add(_item as NxButton);
        }
        public override void Remove(NxItem _item)
        {
            base.Remove(_item);
            if (_item is NxButton)
                m_grid[((NxButton)_item).Coords.X, ((NxButton)_item).Coords.Y].Remove(_item as NxButton);
        }
        public void moveRight()
        {
            MoveCursor(new Point(1, 0));
        }
        public void moveLeft()
        {
            MoveCursor(new Point(-1, 0));
        }
        public void moveUp()
        {
            MoveCursor(new Point(0, -1));
        }
        public void moveDown()
        {
            MoveCursor(new Point(1, 1));
        }
        private NxButton[] MoveCursor(Point move)
        {
            Point cursorMoved = new Point(Cursor.X+move.X, Cursor.Y+move.Y);

            cursorMoved.X = Math.Min(Math.Max(cursorMoved.X, 0), GridWidth);
            cursorMoved.Y = Math.Min(Math.Max(cursorMoved.Y, 0), GridHeight);

            bool limitReached = false;

            while(limitReached)
            {
                if(m_grid[cursorMoved.X, cursorMoved.Y].Count > 0)
                {
                    Cursor = cursorMoved;
                    return m_grid[cursorMoved.X, cursorMoved.Y].ToArray();
                }
                else
                {
                    limitReached = (cursorMoved.X == 0 && move.X < 0) ||
                                    (cursorMoved.Y == 0 && move.Y < 0) ||
                                    (cursorMoved.X >= GridWidth && move.X > 0) ||
                                    (cursorMoved.Y >= GridHeight && move.Y > 0);
                    cursorMoved = new Point(cursorMoved.X + move.X, cursorMoved.Y + move.Y);
                }
            }
            return new NxButton[0];
        }
    }
}
