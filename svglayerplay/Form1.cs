using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace svglayerplay
{
    public partial class Form1 : Form
    {
        svgFile m_svgFile = null;
        float m_dScale = 1;
        static String m_strCurrentFilePath = "";
        public Form1()
        {
            InitializeComponent();           
            //Set Double Buffering
            panel1.GetType().GetMethod("SetStyle",
              System.Reflection.BindingFlags.Instance |
              System.Reflection.BindingFlags.NonPublic).Invoke(panel1,new object[]{ System.Windows.Forms.ControlStyles.UserPaint | 
              System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | System.Windows.Forms.ControlStyles.DoubleBuffer, true });

            speedBar.Value = 10;
            scaleBar.Value = 10;
            timer1.Interval = 500;
            timer1.Enabled = false;            
            UpdateDescription();
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (m_svgFile == null)
            {
                m_svgFile = new svgFile();
            }
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            DialogResult result = dlg.ShowDialog();            
            if (result == DialogResult.OK)
            {
                m_strCurrentFilePath = dlg.FileName;
                m_svgFile.Load(m_strCurrentFilePath);
                UpdateDescription();
                this.Refresh();
                timer1.Enabled = false;
                m_strCurrentFilePath = dlg.FileName;
                //we want to monitor the file for changes
                String strPathToWatch = new FileInfo(m_strCurrentFilePath).Directory.FullName;
                CreateFileWatcher(strPathToWatch);
            }
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            //close the application
            this.Close();
        }
        private void btnFirstFrame_Click(object sender, EventArgs e)
        {
            //move to the first frame
            m_svgFile.SelectLayer(0);
            UpdateDescription();
            this.Refresh();
        }

        private void btnPrevFrame_Click(object sender, EventArgs e)
        {
            //move back one frame (the svgFile class will position the frame index at 0 if the position specified is less than 0)
            int nCurrentLayer = m_svgFile.GetCurrentLayer();
            m_svgFile.SelectLayer(nCurrentLayer - 1);  
            this.Refresh();
            UpdateDescription();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //enable the timer
            timer1.Enabled = true;
            UpdateDescription();
        }

        private void btnNextFrame_Click(object sender, EventArgs e)
        {
            //move to the next layer (the svgFile class will position the frame index at the last frame if the position specified exceeds the number of frames)
            int nCurrentLayer = m_svgFile.GetCurrentLayer();
            m_svgFile.SelectLayer(nCurrentLayer + 1);
            this.Refresh();
            UpdateDescription();
        }

        private void btnLastFrame_Click(object sender, EventArgs e)
        {
            //go to the last frame (the svgFile class will position the frame index at the last frame if the position specified exceeds the number of frames)
            m_svgFile.SelectLayer(+99999);
            UpdateDescription();
            this.Refresh();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            //stop the timer
            timer1.Enabled = false;
            UpdateDescription();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //stop the timer
            timer1.Enabled = false;
            //reset to frame 0
            m_svgFile.SelectLayer(0);
            this.Refresh();
            UpdateDescription();
        }
        private void speedBar_Scroll(object sender, EventArgs e)
        {
            //set the delay between frames to a value between 500 ms and 25 ms using the speed bar position
            int s = (int)speedBar.Value;
            int interval = 500 - ((s - 1) * 25);
            timer1.Interval = interval;
        }
        private void scaleBar_Scroll(object sender, EventArgs e)
        {
            //set the scale value between 0.1 and 1 using the scale bar position
            int scale = (int)scaleBar.Value;
            m_dScale = (100 + ((scale - 10) * 10)) / 100f;
            this.Refresh();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int nCurrentLayer = m_svgFile.GetCurrentLayer();
            //we want to know if the current frame index has reached the end of the sequence
            if (nCurrentLayer < m_svgFile.GetLayerCount() - 1)
            {
                m_svgFile.SelectLayer(nCurrentLayer + 1);
            }
            else
            {
                //if so, position the frame index at the start
                m_svgFile.SelectLayer(0);
            }
            this.Refresh();
            UpdateDescription();
        }
        private void panel1_Paint(object sender, PaintEventArgs pe)
        {
            if (m_svgFile != null)
            {
                Rectangle rRect = new Rectangle(pe.ClipRectangle.X, pe.ClipRectangle.Y, pe.ClipRectangle.Width, pe.ClipRectangle.Height);
                Graphics g = pe.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                SolidBrush brush = new SolidBrush(System.Drawing.Color.White);
                Pen pen = new Pen(Color.Black);
                g.FillRectangle(brush, rRect);
                g.DrawRectangle(pen, rRect);
                //calculate the offset required to center the layer in the middle of the canvas
                float dOffsetX = 0;
                float dOffsetY = 0;
                float dSvgWidth = m_svgFile.GetWidth() * m_dScale;
                float dSvgHeight = m_svgFile.GetHeight() * m_dScale;
                if (dSvgWidth < pe.ClipRectangle.Width)
                {
                    dOffsetX = (float)((pe.ClipRectangle.Width - dSvgWidth)/2.0);
                }
                if (dSvgHeight < pe.ClipRectangle.Height)
                {
                    dOffsetY = (float)((pe.ClipRectangle.Height - dSvgHeight) / 2.0);
                }
                //are we drawing all the layers?
                if (chkDisplayAllLayers.Checked)
                {
                    for (int l = 0; l < m_svgFile.GetLayerCount(); l++)
                    {
                        m_svgFile.SelectLayer(l);
                        m_svgFile.Draw(m_dScale, g, dOffsetX, dOffsetY);
                    }
                }
                else
                {
                    //no just draw the current layer
                    m_svgFile.Draw(m_dScale, g, dOffsetX, dOffsetY);
                }
                brush.Dispose();
                pen.Dispose();
            }
        }
        public void CreateFileWatcher(string strPath)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.SynchronizingObject = this;
            watcher.Path = strPath;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.svg";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }
        /// <summary>
        /// Called when the SVG file has been updated by an external application. This method will reload the SVG file
        /// and redraw the canvas (if the animation is currently playing it will continue without interruption)
        /// </summary>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.FullPath.Equals(m_strCurrentFilePath,StringComparison.OrdinalIgnoreCase))
            {
                // Specify what is done when a file is changed, created, or deleted.
                m_svgFile.Load(m_strCurrentFilePath);
                this.Refresh();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportFrm frm = new ExportFrm();
            frm.m_strSVGFilePath = m_strCurrentFilePath;
            frm.m_nLayerCount = m_svgFile.GetLayerCount();
            frm.ShowDialog();
        }
        /// <summary>
        /// Updates the text fields below the player controls to show the current frame index, width and height and total frames, and also enables/disables controls based on the
        /// current frame index
        /// </summary>
        public void UpdateDescription()
        {
            int nCurrentLayer = 0;
            int nTotalLayers = 0;
            //if we have a valid svg file
            if (m_svgFile != null)
            {
                //get information on dimensions and frame count
                nCurrentLayer = m_svgFile.GetCurrentLayer();
                nTotalLayers = m_svgFile.GetLayerCount();
                txtWidth.Text = m_svgFile.GetWidth().ToString();
                txtHeight.Text = m_svgFile.GetHeight().ToString();
                txtTotalFrames.Text = nTotalLayers.ToString();
                txtCurrentFrame.Text = (nCurrentLayer + 1).ToString();
                btnExport.Enabled = true;
                scaleBar.Enabled = true;
            }
            else
            {
                //otherwise disable some controls
                btnExport.Enabled = false;
                scaleBar.Enabled = false;
            }
            //if we have more than one layer
            if (nTotalLayers > 1)
            {
                //enable animation controls
                btnFirstFrame.Enabled = (nCurrentLayer != 0);
                btnPrevFrame.Enabled = (nCurrentLayer != 0);
                btnPlay.Enabled = !timer1.Enabled;
                btnPause.Enabled = timer1.Enabled;
                btnStop.Enabled = timer1.Enabled;
                btnNextFrame.Enabled = nCurrentLayer < (nTotalLayers - 1);
                btnLastFrame.Enabled = nCurrentLayer < (nTotalLayers - 1);
                speedBar.Enabled = true;
            }
            else
            {
                //disable animation controls
                btnFirstFrame.Enabled = false;
                btnPlay.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
                btnPrevFrame.Enabled = false;
                btnNextFrame.Enabled = false;
                btnLastFrame.Enabled = false;
                speedBar.Enabled = false;
            }
        }
        //the user has clicked on the control to see/hide all the layers
        private void chkDisplayAllLayers_CheckedChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }
        //the window has been resized
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }              
    }
}
