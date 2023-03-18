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
            this.gbTests = new System.Windows.Forms.GroupBox();
            this.rbTest3 = new System.Windows.Forms.RadioButton();
            this.rbTest2 = new System.Windows.Forms.RadioButton();
            this.rbTest1 = new System.Windows.Forms.RadioButton();
            this.btUnloadTest = new System.Windows.Forms.Button();
            this.btLoadTest = new System.Windows.Forms.Button();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.listView1 = new System.Windows.Forms.ListView();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisplay)).BeginInit();
            this.gbTests.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(166, 27);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 0;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(588, 48);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(200, 302);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 465);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(887, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(887, 24);
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
            this.btPlay.Location = new System.Drawing.Point(174, 327);
            this.btPlay.Name = "btPlay";
            this.btPlay.Size = new System.Drawing.Size(75, 23);
            this.btPlay.TabIndex = 4;
            this.btPlay.Text = "Play";
            this.btPlay.UseVisualStyleBackColor = true;
            this.btPlay.Click += new System.EventHandler(this.btPlay_Click);
            // 
            // btPause
            // 
            this.btPause.Location = new System.Drawing.Point(336, 327);
            this.btPause.Name = "btPause";
            this.btPause.Size = new System.Drawing.Size(75, 23);
            this.btPause.TabIndex = 5;
            this.btPause.Text = "Pause";
            this.btPause.UseVisualStyleBackColor = true;
            this.btPause.Click += new System.EventHandler(this.btPause_Click);
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(255, 327);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 6;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btNext
            // 
            this.btNext.Location = new System.Drawing.Point(417, 327);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(75, 23);
            this.btNext.TabIndex = 7;
            this.btNext.Text = "Next";
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // btReset
            // 
            this.btReset.Location = new System.Drawing.Point(498, 327);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(75, 23);
            this.btReset.TabIndex = 8;
            this.btReset.Text = "Reset";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // pbDisplay
            // 
            this.pbDisplay.BackColor = System.Drawing.Color.Black;
            this.pbDisplay.Location = new System.Drawing.Point(158, 87);
            this.pbDisplay.Name = "pbDisplay";
            this.pbDisplay.Size = new System.Drawing.Size(424, 233);
            this.pbDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbDisplay.TabIndex = 9;
            this.pbDisplay.TabStop = false;
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.Location = new System.Drawing.Point(659, 394);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(80, 24);
            this.lbTime.TabIndex = 10;
            this.lbTime.Text = "00:00.00";
            // 
            // cbPanels
            // 
            this.cbPanels.FormattingEnabled = true;
            this.cbPanels.Location = new System.Drawing.Point(12, 48);
            this.cbPanels.Name = "cbPanels";
            this.cbPanels.Size = new System.Drawing.Size(121, 21);
            this.cbPanels.TabIndex = 12;
            this.cbPanels.SelectedIndexChanged += new System.EventHandler(this.cbPanels_SelectedIndexChanged);
            // 
            // gbTests
            // 
            this.gbTests.Controls.Add(this.rbTest3);
            this.gbTests.Controls.Add(this.rbTest2);
            this.gbTests.Controls.Add(this.rbTest1);
            this.gbTests.Controls.Add(this.btUnloadTest);
            this.gbTests.Controls.Add(this.btLoadTest);
            this.gbTests.Location = new System.Drawing.Point(12, 96);
            this.gbTests.Name = "gbTests";
            this.gbTests.Size = new System.Drawing.Size(121, 265);
            this.gbTests.TabIndex = 14;
            this.gbTests.TabStop = false;
            this.gbTests.Text = "Tests";
            // 
            // rbTest3
            // 
            this.rbTest3.AutoSize = true;
            this.rbTest3.Location = new System.Drawing.Point(19, 80);
            this.rbTest3.Name = "rbTest3";
            this.rbTest3.Size = new System.Drawing.Size(55, 17);
            this.rbTest3.TabIndex = 18;
            this.rbTest3.TabStop = true;
            this.rbTest3.Text = "Test 3";
            this.rbTest3.UseVisualStyleBackColor = true;
            this.rbTest3.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbTest2
            // 
            this.rbTest2.AutoSize = true;
            this.rbTest2.Location = new System.Drawing.Point(19, 57);
            this.rbTest2.Name = "rbTest2";
            this.rbTest2.Size = new System.Drawing.Size(55, 17);
            this.rbTest2.TabIndex = 17;
            this.rbTest2.TabStop = true;
            this.rbTest2.Text = "Test 2";
            this.rbTest2.UseVisualStyleBackColor = true;
            this.rbTest2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbTest1
            // 
            this.rbTest1.AutoSize = true;
            this.rbTest1.Location = new System.Drawing.Point(19, 34);
            this.rbTest1.Name = "rbTest1";
            this.rbTest1.Size = new System.Drawing.Size(55, 17);
            this.rbTest1.TabIndex = 16;
            this.rbTest1.TabStop = true;
            this.rbTest1.Text = "Test 1";
            this.rbTest1.UseVisualStyleBackColor = true;
            this.rbTest1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // btUnloadTest
            // 
            this.btUnloadTest.Location = new System.Drawing.Point(19, 231);
            this.btUnloadTest.Name = "btUnloadTest";
            this.btUnloadTest.Size = new System.Drawing.Size(75, 23);
            this.btUnloadTest.TabIndex = 15;
            this.btUnloadTest.Text = "Unload";
            this.btUnloadTest.UseVisualStyleBackColor = true;
            this.btUnloadTest.Click += new System.EventHandler(this.btUnloadTest_Click);
            // 
            // btLoadTest
            // 
            this.btLoadTest.Location = new System.Drawing.Point(19, 202);
            this.btLoadTest.Name = "btLoadTest";
            this.btLoadTest.Size = new System.Drawing.Size(75, 23);
            this.btLoadTest.TabIndex = 14;
            this.btLoadTest.Text = "Load";
            this.btLoadTest.UseVisualStyleBackColor = true;
            this.btLoadTest.Click += new System.EventHandler(this.btLoadTest_Click);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // listView1
            // 
            this.listView1.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 371);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(614, 78);
            this.listView1.TabIndex = 15;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(887, 487);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.gbTests);
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
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisplay)).EndInit();
            this.gbTests.ResumeLayout(false);
            this.gbTests.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbTests;
        private System.Windows.Forms.RadioButton rbTest3;
        private System.Windows.Forms.RadioButton rbTest2;
        private System.Windows.Forms.RadioButton rbTest1;
        private System.Windows.Forms.Button btUnloadTest;
        private System.Windows.Forms.Button btLoadTest;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ListView listView1;
    }
}

