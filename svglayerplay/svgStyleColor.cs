using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace svglayerplay
{
    /// <summary>
    /// This class converts a SVG color attribute into a c# Color
    /// </summary>
    class svgStyleColor
    {
        public Color m_color;
        public svgStyleColor()
        {
            m_color = Color.Black;
        }
        /// <summary>
        /// Convert SVG color attribute to Color
        /// </summary>
        /// <param name="strColor">SVG color attribute</param>
        /// <param name="fOpacity">SVG color opacity</param>        
        public svgStyleColor(String strColor,float fOpacity)
        {
            if (strColor.Contains("url("))
            {
                m_color = Color.Black;
                return;
            }
            if (strColor.Length > 5)
            {
                byte red = (byte)int.Parse(strColor.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                byte green = (byte)int.Parse(strColor.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                byte blue = (byte)int.Parse(strColor.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                m_color = Color.FromArgb((byte)(fOpacity * 255), red, green, blue);
            }
        }
    }
}
