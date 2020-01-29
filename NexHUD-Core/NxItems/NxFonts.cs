using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace NexHUDCore.NxItems
{
    public enum NxFonts
    {
        EuroCapital,
        EuroStile
    }
    public class NxFont
    {
        private static string m_euroCapName = "Euro Caps";
        private static string m_euroStileName = "Eurostile-Roman";

        private static bool m_initialized = false;
        private static PrivateFontCollection m_pCollection;
        private static Dictionary<NxFonts, string> m_fontNames = new Dictionary<NxFonts, string>();
        private static Dictionary<NxFonts, Dictionary<int, Font>> m_fonts = new Dictionary<NxFonts, Dictionary<int, Font>>();

        public static Font getFont(NxFonts _font, int size)
        {
            if (!m_initialized)
                initialize();
            bool missing = true;

            if (m_fonts.ContainsKey(_font))
            {
                if (m_fonts[_font].ContainsKey(size))
                {
                    missing = false;
                }
            }

            if (missing)
            {
                Font f = new Font(m_fontNames[_font], size, GraphicsUnit.Pixel);

                if (!m_fonts.ContainsKey(_font))
                    m_fonts.Add(_font, new Dictionary<int, Font>());

                m_fonts[_font].Add(size, f);

            }
            return m_fonts[_font][size];
        }
        private static void initialize()
        {
            m_pCollection = new PrivateFontCollection();

            // Add three font files to the private collection.
            m_pCollection.AddFontFile(Environment.CurrentDirectory + "\\Resources\\EUROCAPS.TTF");
            m_pCollection.AddFontFile(Environment.CurrentDirectory + "\\Resources\\eurstl24.TTF");

            m_fontNames.Add(NxFonts.EuroCapital, "Euro Caps");
            m_fontNames.Add(NxFonts.EuroStile, "Eurostile-Roman");

            m_initialized = true;
        }
    }
}
