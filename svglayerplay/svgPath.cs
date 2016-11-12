using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace svglayerplay
{
    class svgPath : svgEntity
    {
        private List<svgPathCommand> m_commands = null;
        public svgPath()
        {
        }
        /// <summary>
        /// Parses the incoming Path XML node into a list of path commands
        /// </summary>
        /// <param name="node">SVG entity</param>
        /// <returns>True if successful, False if error</returns>
        public override Boolean Parse(XmlNode node)
        {
            if (base.Parse(node))
            {
                svgPathData pathData = new svgPathData();
                pathData.SetData(GetAttribute("d"));
                m_commands = new List<svgPathCommand>();
                float dStartX = 0;
                float dStartY = 0;
                float dPenX = 0;
                float dPenY = 0;
                String strCurrentOp = "";
                String strItem = "";
                while(pathData.GetNextItem(ref strItem))
                {
                    if (strItem.All(char.IsLetter))
                    {
                        strCurrentOp = strItem;
                        if (strCurrentOp.Equals("z") || strCurrentOp.Equals("Z"))
                        {
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = strCurrentOp;
                            cmd.AddCoords(dStartX, dStartY);
                            m_commands.Add(cmd);
                        }
                    }
                    else
                    {
                        bool bRelative = true;
                        //if the letter is uppercase then the coordinates are absolute, if lowercase then the coordinates are relative the current pen position
                        if (strCurrentOp.Equals(strCurrentOp.ToUpper())) bRelative = false;
                        if (strCurrentOp.ToLower().Equals("m"))
                        {
                            float dX = (float)Convert.ToDouble(strItem);
                            float dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            dStartX = dX;
                            dStartY = dY;
                            dPenX = dX;
                            dPenY = dY;
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = strCurrentOp.ToLower();
                            cmd.AddCoords(dX, dY);
                            m_commands.Add(cmd);
                            //Inkscape seems to leave out the L or l command after the M command
                            strCurrentOp = (strCurrentOp.Equals("M") ? "L" : "l");
                        }
                        else if (strCurrentOp.ToLower().Equals("l")) //draw line
                        {                            
                            float dX = (float)Convert.ToDouble(strItem);
                            float dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dPenX += dX;
                                dPenY += dY;
                            }
                            else
                            {
                                dPenX = dX;
                                dPenY = dY;
                            }
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = strCurrentOp.ToLower();
                            cmd.AddCoords(dPenX, dPenY);
                            m_commands.Add(cmd);
                        }
                        else if (strCurrentOp.ToLower().Equals("h")) //horizontal line
                        {
                            float dX = (float)Convert.ToDouble(strItem);
                            if (bRelative)
                            {
                                dPenX += dX;
                            }
                            else
                            {
                                dPenX = dX;
                            }
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = strCurrentOp.ToLower();
                            cmd.AddCoords(dPenX, dPenY);
                            m_commands.Add(cmd);
                        }
                        else if (strCurrentOp.ToLower().Equals("v")) //vertical line
                        {
                            float dY = (float)Convert.ToDouble(strItem);
                            if (bRelative)
                            {
                                dPenY += dY;
                            }
                            else
                            {
                                dPenY = dY;
                            }
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = strCurrentOp.ToLower();
                            cmd.AddCoords(dPenX, dPenY);
                            m_commands.Add(cmd);
                        }
                        else if (strCurrentOp.ToLower().Equals("c")) //curve
                        {
                            float dX = (float)Convert.ToDouble(strItem);
                            float dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = strCurrentOp.ToLower();
                            m_commands.Add(cmd);
                            cmd.AddCoords(dX, dY); //control point 1
                            dX = (float)pathData.GetNextDouble();
                            dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            cmd.AddCoords(dX, dY); //control point 2
                            dX = (float)pathData.GetNextDouble();
                            dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            cmd.AddCoords(dX, dY); //end point
                            dPenX = dX;
                            dPenY = dY;
                        }
                        else if (strCurrentOp.ToLower().Equals("q")) //quadratic curve
                        {
                            float dX = (float)Convert.ToDouble(strItem);
                            float dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = strCurrentOp.ToLower();
                            m_commands.Add(cmd);
                            cmd.AddCoords(dX, dY);
                            //control point
                            dX = (float)pathData.GetNextDouble();
                            dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            cmd.AddCoords(dX, dY);
                            dPenX = dX;
                            dPenY = dY;
                        }
                        else if (strCurrentOp.ToLower().Equals("s"))
                        {
                            float dX = (float)Convert.ToDouble(strItem);
                            float dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = (strCurrentOp.Equals("S") ? "C" : "c");
                            m_commands.Add(cmd);
                            cmd.AddCoords(dX, dY); //control point 1
                            cmd.AddCoords(dX, dY); //control point 2
                            dX = (float)pathData.GetNextDouble();
                            dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            cmd.AddCoords(dX, dY); //end point
                            dPenX = dX;
                            dPenY = dY;
                        }
                        else if (strCurrentOp.ToLower().Equals("a")) //arc
                        {
                            float radiusX = (float)Convert.ToDouble(strItem);
                            float radiusY = (float)pathData.GetNextDouble();
                            float rotation = (float)pathData.GetNextDouble();
                            float arcflag = (float)pathData.GetNextDouble();
                            float sweepflag = (float)pathData.GetNextDouble();
                            float dX = (float)pathData.GetNextDouble();
                            float dY = (float)pathData.GetNextDouble();
                            if (bRelative)
                            {
                                dX += dPenX;
                                dY += dPenY;
                            }
                            svgPathCommand cmd = new svgPathCommand();
                            cmd.m_strCommand = strCurrentOp.ToLower();
                            m_commands.Add(cmd);
                            cmd.AddCoords(radiusX, radiusY);
                            cmd.AddData(rotation);
                            cmd.AddData(arcflag);
                            cmd.AddData(sweepflag);
                            cmd.AddCoords(dX, dY);
                            dPenX = dX;
                            dPenY = dY;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Draws the path by going through the list of path commands
        /// </summary>
        /// <param name="g">Graphics context</param>
        public override void DrawInternal(Graphics g)
        {
            base.DrawInternal(g);
            try
            {
                //if the path has commands
                if (m_commands != null)
                {
                    //initialize the style object
                    InitializeStyle();
                    //start a path
                    GraphicsPath path = new GraphicsPath();
                    float dPenX = 0;
                    float dPenY = 0;
                    for (int a = 0; a < m_commands.Count; a++)
                    {
                        if (m_commands[a].m_strCommand.Equals("m"))
                        {
                            //move the pen position to the coordinates
                            m_commands[a].GetCoord(0, ref dPenX, ref dPenY);
                        }
                        else if (m_commands[a].m_strCommand.Equals("l") || m_commands[a].m_strCommand.Equals("h") || m_commands[a].m_strCommand.Equals("v"))
                        {
                            //line draw commands
                            float dLineToX = 0;
                            float dLineToY = 0;
                            m_commands[a].GetCoord(0, ref dLineToX, ref dLineToY);
                            path.AddLine((float)dPenX, (float)dPenY, (float)dLineToX, (float)dLineToY);
                            dPenX = dLineToX;
                            dPenY = dLineToY;
                        }
                        else if (m_commands[a].m_strCommand.Equals("z"))
                        {
                            //return to the original start position, i.e. close the path
                            float dLineToX = 0;
                            float dLineToY = 0;
                            m_commands[a].GetCoord(0, ref dLineToX, ref dLineToY);
                            if (dPenX != dLineToX && dPenY != dLineToY)
                            {
                                path.AddLine((float)dPenX, (float)dPenY, (float)dLineToX, (float)dLineToY);
                            }
                            dPenX = dLineToX;
                            dPenY = dLineToY;
                        }                          
                        else if (m_commands[a].m_strCommand.Equals("c"))
                        {
                            //draw a curve
                            float[] dLineToX = new float[4];
                            float[] dLineToY = new float[4];
                            for (int b = 0; b < 3; b++)
                            {
                                float dX = 0;
                                float dY = 0;
                                m_commands[a].GetCoord(b, ref dX, ref dY);
                                dLineToX[b + 1] = dX;
                                dLineToY[b + 1] = dY;
                            }
                            dLineToX[0] = dPenX;
                            dLineToY[0] = dPenY;
                            PointF pt1 = new PointF(dLineToX[0], dLineToY[0]);
                            PointF pt2 = new PointF(dLineToX[1], dLineToY[1]);
                            PointF pt3 = new PointF(dLineToX[2], dLineToY[2]);
                            PointF pt4 = new PointF(dLineToX[3], dLineToY[3]);
                            path.AddBezier(pt1, pt2, pt3, pt4);
                            dPenX = dLineToX[3];
                            dPenY = dLineToY[3];
                        }
                        else if (m_commands[a].m_strCommand.Equals("q"))
                        {
                            //code from Daniel Pryden http://stackoverflow.com/questions/1447659/draw-quadratic-curve
                            float dX = 0;
                            float dY = 0;
                            m_commands[a].GetCoord(0, ref dX, ref dY);
                            float dX2 = 0;
                            float dY2 = 0;
                            m_commands[a].GetCoord(1, ref dX2, ref dY2);
                            float dX3 = 0;
                            float dY3 = 0;
                            m_commands[a].GetCoord(2, ref dX3, ref dY3);
                            PointF[] myArray = {new PointF(dX, dY),new PointF(dX2, dY2),new PointF(dX3, dY3)};
                            GraphicsPath myPath = new GraphicsPath();
                            path.AddBeziers(myArray);
                            dPenX = dX3;
                            dPenY = dY3;
                        }
                        else if (m_commands[a].m_strCommand.Equals("a"))
                        {
                            //draw an arc
                            float rX = 0f;
                            float rY = 0f;
                            m_commands[a].GetCoord(0, ref rX, ref rY);                            
                            float angle = m_commands[a].GetData(0);
                            float largeFlag = m_commands[a].GetData(1);
                            float sweepFlag = m_commands[a].GetData(2);
                            float dX = 0f;
                            float dY = 0f;
                            m_commands[a].GetCoord(1, ref dX, ref dY);
                            svgDrawArc arc = new svgDrawArc(new PointF(dPenX, dPenY), rX, rY, angle, (largeFlag == 1) ? SvgArcSize.Large : SvgArcSize.Small, (sweepFlag == 1) ? SvgArcSweep.Positive : SvgArcSweep.Negative, new PointF(dX,dY));
                            arc.Draw(path);
                            dPenX = dX;
                            dPenY = dY;
                        }
                    }
                    //if the path has a fill color
                    if (IsStyleFillDefined())
                    {
                        //we must close the path
                        path.CloseFigure();
                        //and then fill it
                        g.FillPath(m_fillBrush, path);
                    }
                    //if the path has a stroke
                    if (IsStyleStrokeDefined())
                    {
                        //draw the path
                        g.DrawPath(m_strokePen, path);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }
    }
}
