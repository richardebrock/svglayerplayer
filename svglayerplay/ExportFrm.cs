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
    public partial class ExportFrm : Form
    {
        public String m_strSVGFilePath = "";
        public int m_nLayerCount = 0;
        public ExportFrm()
        {
            InitializeComponent();
        }
        private void ExportFrm_Load(object sender, EventArgs e)
        {
            FileInfo f = new FileInfo(m_strSVGFilePath);
            String strPrefix = f.Name.Substring(0, f.Name.IndexOf("."));
            fileNamePrefix.Text = strPrefix;            
            sheetPadding.Text = "10";
            double dCols = Math.Floor(Math.Sqrt(m_nLayerCount));
            double dRows = Math.Ceiling(m_nLayerCount / dCols);
            sheetCols.Text = dCols.ToString();
            sheetRows.Text = dRows.ToString();
        }
        private void browseSheet_Click(object sender, EventArgs e)
        {
            exportSheet.Checked = true;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                sheetFilePath.Text = dlg.FileName;                
            }
        }
        private void browsePng_Click(object sender, EventArgs e)
        {
            exportAsPNG.Checked = true;
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                pngOutputPath.Text = dlg.SelectedPath;
            }
        }
        private void browseGif_Click(object sender, EventArgs e)
        {
            exportAsGif.Checked = true;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "GIF files (*.gif)|*.gif|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                gifFilePath.Text = dlg.FileName;
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            svgFile svgFile = new svgFile();
            svgFile.Load(m_strSVGFilePath);
            if (exportSheet.Checked)
            {
                if (svgFile.WriteToSheet(sheetFilePath.Text,Convert.ToInt32(sheetRows.Text),Convert.ToInt32(sheetCols.Text),Convert.ToInt32(sheetPadding.Text)))
                {
                    string message = "Sprite sheet exported successfully";
                    string caption = "Export SVG";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, caption, buttons);
                    this.Close();
                }
            }
            else if (exportAsPNG.Checked)
            {
                if (pngOutputPath.Text.Substring(pngOutputPath.Text.Length-1) != "\\")
                {
                    pngOutputPath.Text += "\\";
                }
                if (svgFile.WriteToPNGs(fileNamePrefix.Text, pngOutputPath.Text))
                {
                    string message = "PNG files exported successfully";
                    string caption = "Export SVG";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, caption, buttons);
                    this.Close();
                }
            }
            else if (exportAsGif.Checked)
            {
                if (svgFile.WriteToGif(gifFilePath.Text))
                {
                    string message = "Animated GIF exported successfully";
                    string caption = "Export SVG";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, caption, buttons);
                    this.Close();
                }
            }
        }        
    }
}
