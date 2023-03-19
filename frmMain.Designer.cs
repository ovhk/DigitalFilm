namespace DigitalDarkroom
{
    partial class frmMain
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonTest = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btPlay = new System.Windows.Forms.Button();
            this.btPause = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.btNext = new System.Windows.Forms.Button();
            this.btReset = new System.Windows.Forms.Button();
            this.pbDisplay = new System.Windows.Forms.PictureBox();
            this.lbTime = new System.Windows.Forms.Label();
            this.cbPanels = new System.Windows.Forms.ComboBox();
            this.gbModes = new System.Windows.Forms.GroupBox();
            this.tbModeDescription = new System.Windows.Forms.TextBox();
            this.btBrowseImgFiles = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rbMode6 = new System.Windows.Forms.RadioButton();
            this.rbMode5 = new System.Windows.Forms.RadioButton();
            this.tbDuration = new System.Windows.Forms.TextBox();
            this.rbMode4 = new System.Windows.Forms.RadioButton();
            this.rbMode3 = new System.Windows.Forms.RadioButton();
            this.rbMode2 = new System.Windows.Forms.RadioButton();
            this.rbMode1 = new System.Windows.Forms.RadioButton();
            this.btUnloadMode = new System.Windows.Forms.Button();
            this.btLoadMode = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisplay)).BeginInit();
            this.gbModes.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(0, 0);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 16;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(780, 30);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(200, 331);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.ControlDark;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 585);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(988, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(988, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // btPlay
            // 
            this.btPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btPlay.Location = new System.Drawing.Point(192, 372);
            this.btPlay.Name = "btPlay";
            this.btPlay.Size = new System.Drawing.Size(75, 34);
            this.btPlay.TabIndex = 4;
            this.btPlay.Text = "Play";
            this.btPlay.UseVisualStyleBackColor = true;
            this.btPlay.Click += new System.EventHandler(this.btPlay_Click);
            // 
            // btPause
            // 
            this.btPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btPause.Location = new System.Drawing.Point(354, 372);
            this.btPause.Name = "btPause";
            this.btPause.Size = new System.Drawing.Size(75, 34);
            this.btPause.TabIndex = 5;
            this.btPause.Text = "Pause";
            this.btPause.UseVisualStyleBackColor = true;
            this.btPause.Click += new System.EventHandler(this.btPause_Click);
            // 
            // btStop
            // 
            this.btStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStop.Location = new System.Drawing.Point(273, 372);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 34);
            this.btStop.TabIndex = 6;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btNext
            // 
            this.btNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btNext.Location = new System.Drawing.Point(435, 372);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(75, 34);
            this.btNext.TabIndex = 7;
            this.btNext.Text = "Next";
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // btReset
            // 
            this.btReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btReset.Location = new System.Drawing.Point(516, 372);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(75, 34);
            this.btReset.TabIndex = 8;
            this.btReset.Text = "Reset";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // pbDisplay
            // 
            this.pbDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDisplay.BackColor = System.Drawing.Color.Black;
            this.pbDisplay.Location = new System.Drawing.Point(192, 30);
            this.pbDisplay.Name = "pbDisplay";
            this.pbDisplay.Size = new System.Drawing.Size(582, 335);
            this.pbDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbDisplay.TabIndex = 9;
            this.pbDisplay.TabStop = false;
            // 
            // lbTime
            // 
            this.lbTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTime.AutoSize = true;
            this.lbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.Location = new System.Drawing.Point(837, 499);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(80, 24);
            this.lbTime.TabIndex = 10;
            this.lbTime.Text = "00:00.00";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbPanels
            // 
            this.cbPanels.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPanels.FormattingEnabled = true;
            this.cbPanels.Location = new System.Drawing.Point(12, 30);
            this.cbPanels.Name = "cbPanels";
            this.cbPanels.Size = new System.Drawing.Size(121, 28);
            this.cbPanels.TabIndex = 12;
            this.cbPanels.SelectedIndexChanged += new System.EventHandler(this.cbPanels_SelectedIndexChanged);
            // 
            // gbModes
            // 
            this.gbModes.Controls.Add(this.tbModeDescription);
            this.gbModes.Controls.Add(this.btBrowseImgFiles);
            this.gbModes.Controls.Add(this.label1);
            this.gbModes.Controls.Add(this.rbMode6);
            this.gbModes.Controls.Add(this.rbMode5);
            this.gbModes.Controls.Add(this.tbDuration);
            this.gbModes.Controls.Add(this.rbMode4);
            this.gbModes.Controls.Add(this.rbMode3);
            this.gbModes.Controls.Add(this.rbMode2);
            this.gbModes.Controls.Add(this.rbMode1);
            this.gbModes.Controls.Add(this.btUnloadMode);
            this.gbModes.Controls.Add(this.btLoadMode);
            this.gbModes.Location = new System.Drawing.Point(12, 64);
            this.gbModes.Name = "gbModes";
            this.gbModes.Size = new System.Drawing.Size(174, 342);
            this.gbModes.TabIndex = 14;
            this.gbModes.TabStop = false;
            this.gbModes.Text = "Modes";
            // 
            // tbModeDescription
            // 
            this.tbModeDescription.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tbModeDescription.Location = new System.Drawing.Point(19, 234);
            this.tbModeDescription.Multiline = true;
            this.tbModeDescription.Name = "tbModeDescription";
            this.tbModeDescription.ReadOnly = true;
            this.tbModeDescription.Size = new System.Drawing.Size(137, 63);
            this.tbModeDescription.TabIndex = 25;
            // 
            // btBrowseImgFiles
            // 
            this.btBrowseImgFiles.Location = new System.Drawing.Point(123, 19);
            this.btBrowseImgFiles.Name = "btBrowseImgFiles";
            this.btBrowseImgFiles.Size = new System.Drawing.Size(35, 23);
            this.btBrowseImgFiles.TabIndex = 24;
            this.btBrowseImgFiles.Text = "...";
            this.btBrowseImgFiles.UseVisualStyleBackColor = true;
            this.btBrowseImgFiles.Click += new System.EventHandler(this.btBrowseImgFiles_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Duration (ms)";
            // 
            // rbMode6
            // 
            this.rbMode6.AutoSize = true;
            this.rbMode6.Location = new System.Drawing.Point(19, 211);
            this.rbMode6.Name = "rbMode6";
            this.rbMode6.Size = new System.Drawing.Size(55, 17);
            this.rbMode6.TabIndex = 22;
            this.rbMode6.TabStop = true;
            this.rbMode6.Text = "Test 6";
            this.rbMode6.UseVisualStyleBackColor = true;
            this.rbMode6.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbMode5
            // 
            this.rbMode5.AutoSize = true;
            this.rbMode5.Location = new System.Drawing.Point(19, 188);
            this.rbMode5.Name = "rbMode5";
            this.rbMode5.Size = new System.Drawing.Size(55, 17);
            this.rbMode5.TabIndex = 21;
            this.rbMode5.TabStop = true;
            this.rbMode5.Text = "Test 5";
            this.rbMode5.UseVisualStyleBackColor = true;
            this.rbMode5.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // tbDuration
            // 
            this.tbDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDuration.Location = new System.Drawing.Point(94, 62);
            this.tbDuration.Name = "tbDuration";
            this.tbDuration.Size = new System.Drawing.Size(64, 26);
            this.tbDuration.TabIndex = 20;
            this.tbDuration.Text = "5000";
            this.tbDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbDuration.TextChanged += new System.EventHandler(this.tbDuration_TextChanged);
            // 
            // rbMode4
            // 
            this.rbMode4.AutoSize = true;
            this.rbMode4.Location = new System.Drawing.Point(19, 165);
            this.rbMode4.Name = "rbMode4";
            this.rbMode4.Size = new System.Drawing.Size(55, 17);
            this.rbMode4.TabIndex = 19;
            this.rbMode4.TabStop = true;
            this.rbMode4.Text = "Test 4";
            this.rbMode4.UseVisualStyleBackColor = true;
            this.rbMode4.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbMode3
            // 
            this.rbMode3.AutoSize = true;
            this.rbMode3.Location = new System.Drawing.Point(19, 142);
            this.rbMode3.Name = "rbMode3";
            this.rbMode3.Size = new System.Drawing.Size(55, 17);
            this.rbMode3.TabIndex = 18;
            this.rbMode3.TabStop = true;
            this.rbMode3.Text = "Test 3";
            this.rbMode3.UseVisualStyleBackColor = true;
            this.rbMode3.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbMode2
            // 
            this.rbMode2.AutoSize = true;
            this.rbMode2.Location = new System.Drawing.Point(19, 119);
            this.rbMode2.Name = "rbMode2";
            this.rbMode2.Size = new System.Drawing.Size(55, 17);
            this.rbMode2.TabIndex = 17;
            this.rbMode2.TabStop = true;
            this.rbMode2.Text = "Test 2";
            this.rbMode2.UseVisualStyleBackColor = true;
            this.rbMode2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbMode1
            // 
            this.rbMode1.AutoSize = true;
            this.rbMode1.Location = new System.Drawing.Point(19, 96);
            this.rbMode1.Name = "rbMode1";
            this.rbMode1.Size = new System.Drawing.Size(55, 17);
            this.rbMode1.TabIndex = 16;
            this.rbMode1.TabStop = true;
            this.rbMode1.Text = "Test 1";
            this.rbMode1.UseVisualStyleBackColor = true;
            this.rbMode1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // btUnloadTest
            // 
            this.btUnloadMode.Location = new System.Drawing.Point(95, 308);
            this.btUnloadMode.Name = "btUnloadTest";
            this.btUnloadMode.Size = new System.Drawing.Size(61, 23);
            this.btUnloadMode.TabIndex = 15;
            this.btUnloadMode.Text = "Unload";
            this.btUnloadMode.UseVisualStyleBackColor = true;
            this.btUnloadMode.Click += new System.EventHandler(this.btUnloadTest_Click);
            // 
            // btLoadTest
            // 
            this.btLoadMode.Location = new System.Drawing.Point(19, 308);
            this.btLoadMode.Name = "btLoadTest";
            this.btLoadMode.Size = new System.Drawing.Size(61, 23);
            this.btLoadMode.TabIndex = 14;
            this.btLoadMode.Text = "Load";
            this.btLoadMode.UseVisualStyleBackColor = true;
            this.btLoadMode.Click += new System.EventHandler(this.btLoadTest_Click);
            // 
            // listView1
            // 
            this.listView1.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 416);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(769, 153);
            this.listView1.TabIndex = 15;
            this.listView1.TileSize = new System.Drawing.Size(128, 128);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Tile;
            this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(988, 607);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.gbModes);
            this.Controls.Add(this.cbPanels);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.pbDisplay);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btPause);
            this.Controls.Add(this.btPlay);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonTest);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "DigitalDarkroom";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisplay)).EndInit();
            this.gbModes.ResumeLayout(false);
            this.gbModes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btPlay;
        private System.Windows.Forms.Button btPause;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.PictureBox pbDisplay;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.ComboBox cbPanels;
        private System.Windows.Forms.GroupBox gbModes;
        private System.Windows.Forms.RadioButton rbMode3;
        private System.Windows.Forms.RadioButton rbMode2;
        private System.Windows.Forms.RadioButton rbMode1;
        private System.Windows.Forms.Button btUnloadMode;
        private System.Windows.Forms.Button btLoadMode;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.RadioButton rbMode4;
        private System.Windows.Forms.TextBox tbDuration;
        private System.Windows.Forms.RadioButton rbMode5;
        private System.Windows.Forms.RadioButton rbMode6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbModeDescription;
        private System.Windows.Forms.Button btBrowseImgFiles;
    }
}

