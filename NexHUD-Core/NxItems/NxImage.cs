using System.Drawing;

namespace NexHUDCore.NxItems
{
    public class NxImage : NxItem
    {
        private Bitmap m_image;

      


        public Bitmap Image
        {
            get { return m_image; }
            set
            {
                m_image = value;
                if (m_image != null)
                    m_image.MakeTransparent();
                makeItDirty();
            }
        }


        public void setOriginalSize()
        {
            if (m_image != null)
            {
                width = m_image.Width;
                height = m_image.Height;
            }
        }

        public NxImage(int _x, int _y, Bitmap _image) : this(_x, _y, 0, 0, _image, true)
        {

        }
        public NxImage(int _x, int _y, int _w, int _h, Bitmap _image) : this(_x, _y, _w, _h, _image, false)
        {

        }
        private NxImage(int _x, int _y, int _w, int _h, Bitmap _image, bool autoSize)
        {
            x = _x;
            y = _y;
            width = _w;
            height = _h;
            m_image = _image;

            if (m_image != null)
                m_image.MakeTransparent();
            if (autoSize)
                setOriginalSize();
        }
        public override void Render(Graphics _g)
        {
            if (m_image != null)
            {
                _g.DrawImage(m_image, x, y, width, height);
            }
        }

        public override void Update()
        {
        }
    }
}
