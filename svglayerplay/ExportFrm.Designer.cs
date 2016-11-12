namespace svglayerplay
{
    partial class ExportFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.exportAsPNG = new System.Windows.Forms.RadioButton();
            this.exportAsGif = new System.Windows.Forms.RadioButton();
            this.gifFilePath = new System.Windows.Forms.TextBox();
            this.fileNamePrefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pngOutputPath = new System.Windows.Forms.TextBox();
            this.browseGif = new System.Windows.Forms.Button();
            this.browsePng = new System.Windows.Forms.Button();
            this.exportSheet = new System.Windows.Forms.RadioButton();
            this.sheetFilePath = new System.Windows.Forms.TextBox();
            this.browseSheet = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.sheetCols = new System.Windows.Forms.TextBox();
            this.sheetRows = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sheetPadding = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // exportAsPNG
            // 
            this.exportAsPNG.AutoSize = true;
            this.exportAsPNG.Location = new System.Drawing.Point(17, 22);
            this.exportAsPNG.Name = "exportAsPNG";
            this.exportAsPNG.Size = new System.Drawing.Size(149, 17);
            this.exportAsPNG.TabIndex = 0;
            this.exportAsPNG.TabStop = true;
            this.exportAsPNG.Text = "Export layers as PNG files:";
            this.exportAsPNG.UseVisualStyleBackColor = true;
            // 
            // exportAsGif
            // 
            this.exportAsGif.AutoSize = true;
            this.exportAsGif.Location = new System.Drawing.Point(16, 270);
            this.exportAsGif.Name = "exportAsGif";
            this.exportAsGif.Size = new System.Drawing.Size(168, 17);
            this.exportAsGif.TabIndex = 2;
            this.exportAsGif.TabStop = true;
            this.exportAsGif.Text = "Export layers as animated GIF:";
            this.exportAsGif.UseVisualStyleBackColor = true;
            // 
            // gifFilePath
            // 
            this.gifFilePath.Location = new System.Drawing.Point(188, 270);
            this.gifFilePath.Name = "gifFilePath";
            this.gifFilePath.Size = new System.Drawing.Size(369, 20);
            this.gifFilePath.TabIndex = 8;
            // 
            // fileNamePrefix
            // 
            this.fileNamePrefix.Location = new System.Drawing.Point(138, 54);
            this.fileNamePrefix.Name = "fileNamePrefix";
            this.fileNamePrefix.Size = new System.Drawing.Size(186, 20);
            this.fileNamePrefix.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "File name prefix:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Output folder:";
            // 
            // pngOutputPath
            // 
            this.pngOutputPath.Location = new System.Drawing.Point(138, 86);
            this.pngOutputPath.Name = "pngOutputPath";
            this.pngOutputPath.Size = new System.Drawing.Size(354, 20);
            this.pngOutputPath.TabIndex = 4;
            // 
            // browseGif
            // 
            this.browseGif.Location = new System.Drawing.Point(563, 267);
            this.browseGif.Name = "browseGif";
            this.browseGif.Size = new System.Drawing.Size(75, 23);
            this.browseGif.TabIndex = 9;
            this.browseGif.Text = "Browse ...";
            this.browseGif.UseVisualStyleBackColor = true;
            this.browseGif.Click += new System.EventHandler(this.browseGif_Click);
            // 
            // browsePng
            // 
            this.browsePng.Location = new System.Drawing.Point(498, 84);
            this.browsePng.Name = "browsePng";
            this.browsePng.Size = new System.Drawing.Size(75, 23);
            this.browsePng.TabIndex = 5;
            this.browsePng.Text = "Browse ...";
            this.browsePng.UseVisualStyleBackColor = true;
            this.browsePng.Click += new System.EventHandler(this.browsePng_Click);
            // 
            // exportSheet
            // 
            this.exportSheet.AutoSize = true;
            this.exportSheet.Location = new System.Drawing.Point(17, 139);
            this.exportSheet.Name = "exportSheet";
            this.exportSheet.Size = new System.Drawing.Size(171, 17);
            this.exportSheet.TabIndex = 1;
            this.exportSheet.TabStop = true;
            this.exportSheet.Text = "Export single PNG sprite sheet:";
            this.exportSheet.UseVisualStyleBackColor = true;
            // 
            // sheetFilePath
            // 
            this.sheetFilePath.Location = new System.Drawing.Point(188, 139);
            this.sheetFilePath.Name = "sheetFilePath";
            this.sheetFilePath.Size = new System.Drawing.Size(369, 20);
            this.sheetFilePath.TabIndex = 6;
            // 
            // browseSheet
            // 
            this.browseSheet.Location = new System.Drawing.Point(563, 139);
            this.browseSheet.Name = "browseSheet";
            this.browseSheet.Size = new System.Drawing.Size(75, 23);
            this.browseSheet.TabIndex = 7;
            this.browseSheet.Text = "Browse ...";
            this.browseSheet.UseVisualStyleBackColor = true;
            this.browseSheet.Click += new System.EventHandler(this.browseSheet_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(563, 22);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 10;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Columns:";
            // 
            // sheetCols
            // 
            this.sheetCols.Location = new System.Drawing.Point(115, 174);
            this.sheetCols.Name = "sheetCols";
            this.sheetCols.Size = new System.Drawing.Size(42, 20);
            this.sheetCols.TabIndex = 12;
            // 
            // sheetRows
            // 
            this.sheetRows.Location = new System.Drawing.Point(115, 200);
            this.sheetRows.Name = "sheetRows";
            this.sheetRows.Size = new System.Drawing.Size(42, 20);
            this.sheetRows.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Rows:";
            // 
            // sheetPadding
            // 
            this.sheetPadding.Location = new System.Drawing.Point(115, 228);
            this.sheetPadding.Name = "sheetPadding";
            this.sheetPadding.Size = new System.Drawing.Size(42, 20);
            this.sheetPadding.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 231);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Padding:";
            // 
            // ExportFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 407);
            this.Controls.Add(this.sheetPadding);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sheetRows);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.sheetCols);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.browseSheet);
            this.Controls.Add(this.sheetFilePath);
            this.Controls.Add(this.exportSheet);
            this.Controls.Add(this.browsePng);
            this.Controls.Add(this.browseGif);
            this.Controls.Add(this.pngOutputPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileNamePrefix);
            this.Controls.Add(this.gifFilePath);
            this.Controls.Add(this.exportAsGif);
            this.Controls.Add(this.exportAsPNG);
            this.Name = "ExportFrm";
            this.Text = "Export SVG";
            this.Load += new System.EventHandler(this.ExportFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton exportAsPNG;
        private System.Windows.Forms.RadioButton exportAsGif;
        private System.Windows.Forms.TextBox gifFilePath;
        private System.Windows.Forms.TextBox fileNamePrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox pngOutputPath;
        private System.Windows.Forms.Button browseGif;
        private System.Windows.Forms.Button browsePng;
        private System.Windows.Forms.RadioButton exportSheet;
        private System.Windows.Forms.TextBox sheetFilePath;
        private System.Windows.Forms.Button browseSheet;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sheetCols;
        private System.Windows.Forms.TextBox sheetRows;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox sheetPadding;
        private System.Windows.Forms.Label label5;
    }
}