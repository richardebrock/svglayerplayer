using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Drawing.Drawing2D;

namespace svglayerplay
{
    class svgRect : svgEntity
    {
        /// <summary>
        /// Provides parsing of a SVG Rect entity and also draws the rectangle 
        /// </summary>
        Rectangle m_rRectangle;
        public svgRect()
        {
        }
        /// <summary>
        /// Parse the XML node containing the SVG rect
        /// </summary>
        /// <param name="node">XML node from SVG file</param>
        /// <returns>True if successful, False if an error occurred</returns>
        public override Boolean Parse(XmlNode node)
        {
            if (base.Parse(node))
            {
                double x = Convert.ToDouble(GetAttribute("x"));
                double y = Convert.ToDouble(GetAttribute("y"));
                double width = Convert.ToDouble(GetAttribute("width"));
                double height = Convert.ToDouble(GetAttribute("height"));
                m_rRectangle.X = (int)x;
                m_rRectangle.Y = (int)y;
                m_rRectangle.Width = (int)width;
                m_rRectangle.Height = (int)height;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Draw the SVG entity
        /// </summary>        
        /// <param name="g"></param>
        public override void DrawInternal(Graphics g)
        {
            base.DrawInternal(g);
            GraphicsPath path = new GraphicsPath();
            RectangleF r = new RectangleF(m_rRectangle.X, m_rRectangle.Y, m_rRectangle.Width, m_rRectangle.Height);
            path.AddRectangle(r);
            if (IsStyleFillDefined())
            {
                g.FillPath(m_fillBrush, path);
            }
            if (IsStyleStrokeDefined())
            {
                g.DrawPath(m_strokePen, path);
            }
        }
    }
}
