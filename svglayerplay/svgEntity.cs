using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Xml;
using System.Drawing.Drawing2D;

namespace svglayerplay
{
    class svgEntity
    {
        public svgEntity m_parent = null;
        public string m_strType = "";
        public List<svgEntity> m_entities = new List<svgEntity>();
        public XmlAttributeCollection m_attributes = null;        
        public svgEntityStyle m_style = new svgEntityStyle();
        public Pen m_strokePen = null;
        public SolidBrush m_fillBrush = null;
        public String m_strTransform = "";
        public svgEntity()
        {
        }
        public void setParent(svgEntity parent)
        {
            m_parent = parent;
        }
        public virtual Boolean Parse(XmlNode node)
        {
            try
            {
                m_strType = node.Name;
                if (node.Attributes != null)
                {
                    m_attributes = node.Attributes;
                }
                m_strTransform = GetAttribute("transform");
                String strStyle = GetAttribute("style");
                m_style.Parse(strStyle);
                if (m_style.m_bVisible == false)
                {
                    return false;
                }
                if (node.ChildNodes != null)
                {
                    for (int a = 0; a < node.ChildNodes.Count; a++)
                    {
                        XmlNode child = node.ChildNodes[a];
                        if (child.Name == "path")
                        {
                            svgPath path = new svgPath();
                            path.setParent(this);
                            m_entities.Add(path);
                            path.Parse(child);
                        }
                        else if (child.Name == "rect")
                        {
                            svgRect rect = new svgRect();
                            rect.setParent(this);
                            m_entities.Add(rect);
                            rect.Parse(child);
                        }
                        else if (child.Name == "circle")
                        {
                            svgCircle circle = new svgCircle();
                            circle.setParent(this);
                            m_entities.Add(circle);
                            circle.Parse(child);
                        }
                        else if (child.Name == "ellipse")
                        {
                            svgEllipse ellipse = new svgEllipse();
                            ellipse.setParent(this);
                            m_entities.Add(ellipse);
                            ellipse.Parse(child);
                        }
                        else
                        {
                            //groups get processed here
                            svgEntity entity = new svgEntity();
                            entity.setParent(this);
                            m_entities.Add(entity);
                            entity.Parse(child);
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            return false;
        }        
        public String GetAttribute(String strAttribName)
        {
            String strAttribValue = "";
            if (m_attributes != null)
            {
                XmlNode attr = m_attributes.GetNamedItem(strAttribName);
                if (attr != null)
                {
                    strAttribValue = attr.Value;
                }
            }
            return strAttribValue;
        }        
        public void InitializeStyle()
        {
            try
            {
                if (m_strokePen == null)
                {
                    svgStyleColor strokeColor = m_style.GetStrokeColor();
                    if (strokeColor == null && m_parent != null)
                    {
                        strokeColor = m_parent.m_style.GetStrokeColor();
                    }
                    if (strokeColor != null)
                    {
                        int strokeWidth = (int)m_style.m_dStrokeWidth;
                        m_strokePen = new Pen(strokeColor.m_color, strokeWidth);
                    }
                }
                if (m_fillBrush == null)
                {
                    svgStyleColor fillColor = m_style.GetFillColor();
                    if (fillColor == null & m_parent != null)
                    {
                        fillColor = m_parent.m_style.GetFillColor();
                    }
                    if (fillColor != null)
                    {
                        m_fillBrush = new SolidBrush(fillColor.m_color);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }
        public Boolean IsStyleFillDefined()
        {
            return m_style.m_fillStyle == svgFillStyle.enabled;
        }
        public Boolean IsStyleStrokeDefined()
        {
            return m_style.m_strokeStyle == svgStrokeStyle.enabled;
        }
        public virtual void Draw(Graphics g, svgTransformList transform)
        {
            try
            {
                InitializeStyle();
                if (m_strTransform.Length > 0)
                {
                    transform.Add(m_strTransform);
                    Matrix matrix = transform.GetMatrix();
                    if (matrix != null)
                    {
                        g.Transform = matrix;
                    }
                }
                DrawInternal(g);
                for (int a = 0; a < m_entities.Count; a++)
                {
                    m_entities[a].Draw(g, transform);
                }
                if (m_strTransform.Length > 0)
                {
                    transform.Remove();
                }
                Matrix matrix2 = transform.GetMatrix();
                if (matrix2 != null)
                {
                    g.Transform = matrix2;
                }
                else
                {
                    g.Transform = null;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }
        public virtual void DrawInternal(Graphics g)
        {
        }
        public void Debug()
        {
            Trace.WriteLine("type=" + m_strType);
            for (int a = 0; a < m_entities.Count; a++)
            {
                m_entities[a].Debug();
            }
        }
    }
}
