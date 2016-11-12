using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace svglayerplay
{
    class svgFile
    {
        String m_strFilePath;
        public List<svgEntity> m_entities = new List<svgEntity>();
        int m_nLayerCount = 0;
        int m_nCurrentLayer = 0;
        float m_dWidth = 0f;
        float m_dHeight = 0f;
        public svgFile()
        {
        }
        public Boolean Load(String strFilePath)
        {
            try
            {
                m_strFilePath = strFilePath;
                m_entities.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(strFilePath);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("svg", "http://www.w3.org/2000/svg");
                XmlNode node = doc.DocumentElement.SelectSingleNode("//svg:svg", nsmgr);
                XmlAttributeCollection attributes = node.Attributes;
                //the document might contain a viewbox, if it does then we use that for the document size
                XmlNode viewBox = attributes.GetNamedItem("viewBox");
                if (viewBox != null)
                {
                    String[] slVieWbox = viewBox.Value.Split(' ');
                    if (slVieWbox.Length > 3)
                    {
                        m_dWidth = (float)Convert.ToDouble(slVieWbox[2]);
                        m_dHeight = (float)Convert.ToDouble(slVieWbox[3]);
                    }
                }
                else
                {
                    //otherwise look for a wide and height value
                    XmlNode width = attributes.GetNamedItem("width");
                    if (width != null) m_dWidth = (float)Convert.ToDouble(width.Value);
                    XmlNode height = attributes.GetNamedItem("height");
                    if (height != null) m_dHeight = (float)Convert.ToDouble(height.Value);
                }
                //parse the rest of the document
                for (int a = 0; a < node.ChildNodes.Count; a++)
                {
                    XmlNode child = node.ChildNodes[a];
                    svgEntity entity = new svgEntity();                    
                    if (entity.Parse(child))
                    {
                        m_entities.Add(entity);
                    }
                }
                m_nLayerCount = GetLayerCount();
                m_nCurrentLayer = 0;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            return false;
        }
        public float GetWidth()
        {
            return m_dWidth;
        }
        public float GetHeight()
        {
            return m_dHeight;
        }
        public int GetCurrentLayer()
        {
            return m_nCurrentLayer;
        }
        public void SelectLayer(int nLayer)
        {
            if (nLayer > m_nLayerCount) nLayer = m_nLayerCount - 1;
            if (nLayer < 0) nLayer = 0;
            m_nCurrentLayer = nLayer;
        }
        public int GetLayerCount()
        {
            //count all the group entites with the inkscape:groupmode attribute = layer
            int nLayerCount = 0;
            for (int a = 0; a < m_entities.Count; a++)
            {
                if (m_entities[a].m_strType.Equals("g"))
                {
                    if (m_entities[a].GetAttribute("inkscape:groupmode").Equals("layer"))
                    {
                        nLayerCount++;
                    }
                }
            }
            return nLayerCount;
        }
        public void Draw(float dScale, Graphics g,float dOffsetX,float dOffsetY)
        {
            svgTransformList transform = new svgTransformList();
            //if we want to draw the layers in the center of the canvas then we need to apply a translate transform using the offset values. The offset
            //values are calculated using the size of the SVG document
            if (dOffsetX != 0 || dOffsetY != 0)
            {
                transform.Add("translate(" + dOffsetX + "," + dOffsetY + ")");
            }
            //if we are zooming in or out, apply a scaling transform
            if (dScale != 1)
            {
                String strTransform = "scale(" + dScale + "," + dScale + ")";                
                transform.Add(strTransform);
            }
            //build the current transform matrix
            Matrix matrix = transform.GetMatrix();
            if (matrix != null)
            {
                //if it's a valid matrix then apply it to the graphics context
                g.Transform = matrix;
            }
            //if we only have one layer then draw the layer
            if (m_nLayerCount < 2)
            {
                for (int a = 0; a < m_entities.Count; a++)
                {
                    m_entities[a].Draw(g, transform);
                }
            }
            else
            {
                //otherwise skip to the correct layer and then draw it
                int nSkip = (m_nCurrentLayer == -1) ? 0 : m_nCurrentLayer;
                for (int a = 0; a < m_entities.Count; a++)
                {
                    if (m_entities[a].GetAttribute("inkscape:groupmode").Equals("layer"))
                    {
                        if (nSkip == 0)
                        {
                            m_entities[a].Draw(g, transform);
                            if (m_nCurrentLayer != -1) break;
                        }
                        else
                        {
                            nSkip--;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Creates a PNG sprite sheet containing each layer as a seperate cell
        /// </summary>
        /// <param name="strFilePath">The outptu PNG file name</param>
        /// <param name="nRows">Number of rows in the sheet</param>
        /// <param name="nCols">Number of columns in the sheet</param>
        /// <param name="nPadding">Number of pixels that will be applied as padding around each sprite</param>
        /// <returns>True if successful, False if error</returns>
        public Boolean WriteToSheet(String strFilePath,int nRows,int nCols,int nPadding)
        {
            int nCellWidth = (int)m_dWidth;
            int nCellHeight = (int)m_dHeight;
            double dSheetWidth = (nCols * nCellWidth) + (nPadding * (nCols + 1));
            double dSheetHeight = (nRows * nCellHeight) + (nPadding * (nRows + 1));
            Bitmap bmpSheet = new Bitmap(Convert.ToInt32(dSheetWidth), Convert.ToInt32(dSheetHeight), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gg = Graphics.FromImage(bmpSheet);            
            int nXpos = nPadding;
            int nYpos = nPadding;
            int nLayerCount = GetLayerCount();
            Rectangle rRect = new Rectangle(0, 0, nCellWidth, nCellHeight);
            for (int nLayer = 0; nLayer < nLayerCount; nLayer++)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(nCellWidth), Convert.ToInt32(nCellHeight), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bitmap);
                SelectLayer(nLayer);
                Draw(1, g,0,0);
                gg.DrawImage(bitmap, new System.Drawing.Point(nXpos,nYpos));
                bitmap.Dispose();
                nXpos += nCellWidth + nPadding;
                if (nXpos>=dSheetWidth)
                {
                    nXpos = nPadding;
                    nYpos += nCellHeight + nPadding;
                }
            }
            bmpSheet.Save(strFilePath, ImageFormat.Png);
            bmpSheet.Dispose();
            return true;
        }
        /// <summary>
        /// Writes each layer out as a seperate PNG file
        /// </summary>
        /// <param name="strPrefix">File name prefix, e.g. 'walk' will produce 'walk_1.png', 'walk_2.png' etc.</param>
        /// <param name="strPath">The output folder (must exist)</param>
        /// <returns>True if successful, False if error</returns>
        public Boolean WriteToPNGs(String strPrefix,String strPath)
        {
            List<Bitmap> images = new List<Bitmap>();
            int nLayerCount = GetLayerCount();
            for (int nLayer = 0; nLayer < nLayerCount; nLayer++)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(m_dWidth), Convert.ToInt32(m_dHeight), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                images.Add(bitmap);
                Graphics g = Graphics.FromImage(bitmap);
                Rectangle rRect = new Rectangle(0, 0, (int)m_dWidth, (int)m_dHeight);
                SelectLayer(nLayer);
                Draw(1, g,0,0);
            }
            FileInfo f = new FileInfo(m_strFilePath);
            String strOutputPath = new FileInfo(strPath).Directory.FullName + "\\";
            int nOutputIndex = 0;
            foreach (System.Drawing.Bitmap bmpImage in images)
            {
                String strFileName = strOutputPath + strPrefix + "_" + nOutputIndex + ".png";
                bmpImage.Save(strFileName, ImageFormat.Png);
                bmpImage.Dispose();
                nOutputIndex++;
            }
            return true;
        }
        /// <summary>
        /// Writes each layer out to an animated GIF file
        /// </summary>
        /// <param name="strFilePath">Output file name</param>
        /// <returns>True if successful, False if error</returns>
        public Boolean WriteToGif(String strFilePath)
        {
            List<Bitmap> images = new List<Bitmap>();
            int nLayerCount = GetLayerCount();
            for (int nLayer = 0; nLayer < nLayerCount; nLayer++)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(m_dWidth), Convert.ToInt32(m_dHeight), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                images.Add(bitmap);
                Graphics g = Graphics.FromImage(bitmap);
                Rectangle rRect = new Rectangle(0, 0, (int)m_dWidth, (int)m_dHeight);
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillRectangle(brush, rRect);
                SelectLayer(nLayer);
                Draw(1, g,0,0);
                brush.Dispose();
            }
            System.Windows.Media.Imaging.GifBitmapEncoder gEnc = new GifBitmapEncoder();
            foreach (System.Drawing.Bitmap bmpImage in images)
            {
                var bmp = bmpImage.GetHbitmap();
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                gEnc.Frames.Add(BitmapFrame.Create(src));
            }
            using (FileStream fs = new FileStream(strFilePath, FileMode.Create))
            {
                gEnc.Save(fs);
            }
            return true;
        }
    }
}
