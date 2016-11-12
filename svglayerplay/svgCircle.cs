using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Drawing.Drawing2D;

namespace svglayerplay
{
    class svgCircle : svgEntity
    {
        Rectangle m_rRectangle;
        public svgCircle()
        {
        }
        /// <summary>
        /// Parse the incoming XML node
        /// </summary>
        /// <param name="node">SVG entity</param>
        /// <returns>True if successful, False if error</returns>
        public override Boolean Parse(XmlNode node)
        {
            if (base.Parse(node))
            {
                double cx = Convert.ToDouble(GetAttribute("cx"));
                double cy = Convert.ToDouble(GetAttribute("cy"));
                double radius = Convert.ToDouble(GetAttribute("r"));
                m_rRectangle.X = (int)(cx - radius);
                m_rRectangle.Y = (int)(cy - radius);
                m_rRectangle.Width = (int)(radius*2);
                m_rRectangle.Height = (int)(radius*2);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Draw the SVG entity
        /// </summary>
        /// <param name="g">Graphics context</param>
        public override void DrawInternal(Graphics g)
        {
            base.DrawInternal(g);
            GraphicsPath path = new GraphicsPath();
            RectangleF r = new RectangleF(m_rRectangle.X, m_rRectangle.Y, m_rRectangle.Width, m_rRectangle.Height);
            path.AddEllipse(r);
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
