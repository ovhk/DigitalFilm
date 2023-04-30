using DigitalDarkroom.Controls;

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
            this.pgImageLayer = new System.Windows.Forms.PropertyGrid();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btPlay = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.lbTime = new System.Windows.Forms.Label();
            this.cbPanels = new System.Windows.Forms.ComboBox();
            this.gbModes = new System.Windows.Forms.GroupBox();
            this.pgMode = new System.Windows.Forms.PropertyGrid();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.tbModeDescription = new System.Windows.Forms.TextBox();
            this.btUnloadMode = new System.Windows.Forms.Button();
            this.btLoadMode = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.lbTotalBuration = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbTimer = new System.Windows.Forms.Label();
            this.btPreview = new System.Windows.Forms.Button();
            this.myPictureBox1 = new DigitalDarkroom.Controls.MyPictureBox();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            // pgImageLayer
            // 
            this.pgImageLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pgImageLayer.Location = new System.Drawing.Point(787, 115);
            this.pgImageLayer.Name = "pgImageLayer";
            this.pgImageLayer.Size = new System.Drawing.Size(193, 307);
            this.pgImageLayer.TabIndex = 1;
            this.pgImageLayer.ViewBackColor = System.Drawing.SystemColors.ControlDark;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 597);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(988, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(988, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
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
            this.btPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPlay.Location = new System.Drawing.Point(12, 326);
            this.btPlay.Name = "btPlay";
            this.btPlay.Size = new System.Drawing.Size(198, 46);
            this.btPlay.TabIndex = 4;
            this.btPlay.Text = "Play";
            this.btPlay.UseVisualStyleBackColor = true;
            this.btPlay.Click += new System.EventHandler(this.btPlay_Click);
            // 
            // btStop
            // 
            this.btStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btStop.Location = new System.Drawing.Point(12, 378);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(198, 44);
            this.btStop.TabIndex = 6;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // lbTime
            // 
            this.lbTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTime.AutoSize = true;
            this.lbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.Location = new System.Drawing.Point(880, 447);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(96, 25);
            this.lbTime.TabIndex = 10;
            this.lbTime.Text = "00:00.00";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbPanels
            // 
            this.cbPanels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPanels.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPanels.FormattingEnabled = true;
            this.cbPanels.Location = new System.Drawing.Point(64, 30);
            this.cbPanels.Name = "cbPanels";
            this.cbPanels.Size = new System.Drawing.Size(146, 28);
            this.cbPanels.TabIndex = 12;
            this.cbPanels.SelectedIndexChanged += new System.EventHandler(this.cbPanels_SelectedIndexChanged);
            // 
            // gbModes
            // 
            this.gbModes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbModes.Controls.Add(this.pgMode);
            this.gbModes.Controls.Add(this.cbMode);
            this.gbModes.Controls.Add(this.tbModeDescription);
            this.gbModes.Controls.Add(this.btUnloadMode);
            this.gbModes.Controls.Add(this.btLoadMode);
            this.gbModes.Location = new System.Drawing.Point(12, 64);
            this.gbModes.Name = "gbModes";
            this.gbModes.Size = new System.Drawing.Size(198, 256);
            this.gbModes.TabIndex = 14;
            this.gbModes.TabStop = false;
            this.gbModes.Text = "Modes";
            // 
            // pgMode
            // 
            this.pgMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pgMode.Location = new System.Drawing.Point(12, 51);
            this.pgMode.Name = "pgMode";
            this.pgMode.Size = new System.Drawing.Size(177, 79);
            this.pgMode.TabIndex = 25;
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(12, 21);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(177, 24);
            this.cbMode.Sorted = true;
            this.cbMode.TabIndex = 25;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // tbModeDescription
            // 
            this.tbModeDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbModeDescription.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tbModeDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbModeDescription.Location = new System.Drawing.Point(12, 137);
            this.tbModeDescription.Margin = new System.Windows.Forms.Padding(4);
            this.tbModeDescription.Multiline = true;
            this.tbModeDescription.Name = "tbModeDescription";
            this.tbModeDescription.ReadOnly = true;
            this.tbModeDescription.Size = new System.Drawing.Size(177, 74);
            this.tbModeDescription.TabIndex = 25;
            // 
            // btUnloadMode
            // 
            this.btUnloadMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btUnloadMode.Location = new System.Drawing.Point(109, 217);
            this.btUnloadMode.Name = "btUnloadMode";
            this.btUnloadMode.Size = new System.Drawing.Size(80, 32);
            this.btUnloadMode.TabIndex = 15;
            this.btUnloadMode.Text = "Unload";
            this.btUnloadMode.UseVisualStyleBackColor = true;
            this.btUnloadMode.Click += new System.EventHandler(this.btUnloadMode_Click);
            // 
            // btLoadMode
            // 
            this.btLoadMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btLoadMode.Location = new System.Drawing.Point(13, 217);
            this.btLoadMode.Name = "btLoadMode";
            this.btLoadMode.Size = new System.Drawing.Size(80, 32);
            this.btLoadMode.TabIndex = 14;
            this.btLoadMode.Text = "Load";
            this.btLoadMode.UseVisualStyleBackColor = true;
            this.btLoadMode.Click += new System.EventHandler(this.btLoadMode_Click);
            // 
            // listView1
            // 
            this.listView1.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 428);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Screen :";
            // 
            // lbTotalBuration
            // 
            this.lbTotalBuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTotalBuration.AutoSize = true;
            this.lbTotalBuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalBuration.Location = new System.Drawing.Point(880, 504);
            this.lbTotalBuration.Name = "lbTotalBuration";
            this.lbTotalBuration.Size = new System.Drawing.Size(96, 25);
            this.lbTotalBuration.TabIndex = 25;
            this.lbTotalBuration.Text = "00:00.00";
            this.lbTotalBuration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(787, 504);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 25);
            this.label1.TabIndex = 26;
            this.label1.Text = "Total:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(787, 447);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 25);
            this.label2.TabIndex = 27;
            this.label2.Text = "Current:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(787, 475);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 25);
            this.label4.TabIndex = 28;
            this.label4.Text = "Timer:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTimer
            // 
            this.lbTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTimer.AutoSize = true;
            this.lbTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimer.Location = new System.Drawing.Point(880, 475);
            this.lbTimer.Name = "lbTimer";
            this.lbTimer.Size = new System.Drawing.Size(96, 25);
            this.lbTimer.TabIndex = 29;
            this.lbTimer.Text = "00:00.00";
            this.lbTimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btPreview
            // 
            this.btPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPreview.Location = new System.Drawing.Point(787, 63);
            this.btPreview.Name = "btPreview";
            this.btPreview.Size = new System.Drawing.Size(193, 46);
            this.btPreview.TabIndex = 30;
            this.btPreview.Text = "Preview";
            this.btPreview.UseVisualStyleBackColor = true;
            this.btPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btPreview_MouseDown);
            this.btPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btPreview_MouseUp);
            // 
            // myPictureBox1
            // 
            this.myPictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myPictureBox1.BackColor = System.Drawing.Color.Black;
            this.myPictureBox1.Image = null;
            this.myPictureBox1.Location = new System.Drawing.Point(216, 30);
            this.myPictureBox1.Name = "myPictureBox1";
            this.myPictureBox1.Size = new System.Drawing.Size(565, 392);
            this.myPictureBox1.TabIndex = 17;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(988, 619);
            this.Controls.Add(this.btPreview);
            this.Controls.Add(this.lbTimer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbTotalBuration);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.myPictureBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.gbModes);
            this.Controls.Add(this.cbPanels);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btPlay);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pgImageLayer);
            this.Controls.Add(this.buttonTest);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "DigitalDarkroom";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbModes.ResumeLayout(false);
            this.gbModes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.PropertyGrid pgImageLayer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btPlay;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.ComboBox cbPanels;
        private System.Windows.Forms.GroupBox gbModes;
        private System.Windows.Forms.Button btUnloadMode;
        private System.Windows.Forms.Button btLoadMode;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox tbModeDescription;
        private DigitalDarkroom.Controls.MyPictureBox myPictureBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.PropertyGrid pgMode;
        private System.Windows.Forms.Label lbTotalBuration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbTimer;
        private System.Windows.Forms.Button btPreview;
    }
}

