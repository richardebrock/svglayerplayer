using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace svglayerplay
{
    enum svgFillStyle { notset, disabled, enabled };
    enum svgStrokeStyle { notset, disabled, enabled } ;
    class svgEntityStyle
    {
        public float m_dOpacity = 255f;
        public svgStrokeStyle m_strokeStyle = svgStrokeStyle.notset;
        public float m_dStrokeWidth = 1f;
        public String m_strStrokeColor = "";
        public float m_dStrokeOpacity = 1f;
        public svgFillStyle m_fillStyle = svgFillStyle.notset;
        public String m_strFillColor = "";
        public float m_fFillOpacity = 1f;
        public bool m_bVisible = true;
        public svgStyleColor GetFillColor()
        {
            if (m_strFillColor.Length > 5)
            {
                svgStyleColor color = new svgStyleColor(m_strFillColor, m_fFillOpacity);
                return color;
            }
            return null;
        }
        public svgStyleColor GetStrokeColor()
        {
            if (m_strStrokeColor.Length>5)
            {
                svgStyleColor color = new svgStyleColor(m_strStrokeColor, m_dStrokeOpacity);
                return color;
            }
            return null;
        }
        /// <summary>
        /// Parse the incoming style string into the various style elements
        /// </summary>
        /// <param name="strStyleData">A style attribute string</param>
        public void Parse(String strStyleData)
        {
            String[] slStyles = strStyleData.Split(';');
            for (int s = 0; s < slStyles.Length; s++)
            {
                String[] slStyle = slStyles[s].Split(':');
                if (slStyle.Length > 1)
                {
                    String strName = slStyle[0];
                    String strValue = slStyle[1];
                    if (strName.Equals("opacity")) m_dOpacity = (float)Convert.ToDouble(strValue);
                    if (strName.Equals("fill"))
                    {
                        if (strValue.Equals("none"))
                        {
                            m_fillStyle = svgFillStyle.disabled;
                        }
                        else
                        {
                            m_fillStyle = svgFillStyle.enabled;
                            m_strFillColor = strValue;
                        }
                    }
                    if (strName.Equals("fill-opacity"))
                    {
                        m_fFillOpacity = (float)Convert.ToDouble(strValue);
                    }
                    if (strName.Equals("stroke"))
                    {
                        if (strValue.Equals("none"))
                        {
                            m_strokeStyle = svgStrokeStyle.disabled;
                        }
                        else
                        {
                            m_strokeStyle = svgStrokeStyle.enabled;
                            m_strStrokeColor = strValue;
                        }
                    }
                    if (strName.Equals("stroke-opacity"))
                    {
                        m_dStrokeOpacity = (float)Convert.ToDouble(strValue);
                    }
                    if (strName.Equals("stroke-width"))
                    {
                        if (strValue.Contains("px"))
                        {
                            m_dStrokeWidth = (float)Convert.ToDouble(strValue.Substring(0, strValue.Length - 2));
                        }
                        else
                        {
                            m_dStrokeWidth = (float)Convert.ToDouble(strValue);
                        }
                    }
                    /*
                    if (strName.Equals("display"))
                    {
                        if(strValue.Equals("none")) m_bVisible = false;
                    }*/
                }
            }
        }
    }
}
