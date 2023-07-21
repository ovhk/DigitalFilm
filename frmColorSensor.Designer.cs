namespace DigitalFilm
{
    partial class frmColorSensor
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
            this.components = new System.ComponentModel.Container();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btCalibrate = new System.Windows.Forms.Button();
            this.lbColor = new System.Windows.Forms.Label();
            this.lbGray = new System.Windows.Forms.Label();
            this.pnColor = new System.Windows.Forms.Panel();
            this.pnGray = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(95, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btCalibrate
            // 
            this.btCalibrate.Location = new System.Drawing.Point(113, 12);
            this.btCalibrate.Name = "btCalibrate";
            this.btCalibrate.Size = new System.Drawing.Size(511, 21);
            this.btCalibrate.TabIndex = 3;
            this.btCalibrate.Text = "Calibrate";
            this.btCalibrate.UseVisualStyleBackColor = true;
            this.btCalibrate.Click += new System.EventHandler(this.btCalibrate_Click);
            // 
            // lbColor
            // 
            this.lbColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbColor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbColor.Location = new System.Drawing.Point(10, 344);
            this.lbColor.Name = "lbColor";
            this.lbColor.Size = new System.Drawing.Size(300, 25);
            this.lbColor.TabIndex = 4;
            this.lbColor.Text = "---";
            this.lbColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbGray
            // 
            this.lbGray.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGray.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbGray.Location = new System.Drawing.Point(325, 344);
            this.lbGray.Name = "lbGray";
            this.lbGray.Size = new System.Drawing.Size(300, 25);
            this.lbGray.TabIndex = 5;
            this.lbGray.Text = "---";
            this.lbGray.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnColor
            // 
            this.pnColor.Location = new System.Drawing.Point(12, 39);
            this.pnColor.Name = "pnColor";
            this.pnColor.Size = new System.Drawing.Size(300, 300);
            this.pnColor.TabIndex = 6;
            // 
            // pnGray
            // 
            this.pnGray.Location = new System.Drawing.Point(324, 39);
            this.pnGray.Name = "pnGray";
            this.pnGray.Size = new System.Drawing.Size(300, 300);
            this.pnGray.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 380);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(638, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // serialPort1
            // 
            this.serialPort1.ReadTimeout = 500;
            this.serialPort1.WriteTimeout = 500;
            this.serialPort1.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.serialPort1_ErrorReceived);
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // frmColorSensor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(638, 402);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnGray);
            this.Controls.Add(this.pnColor);
            this.Controls.Add(this.lbGray);
            this.Controls.Add(this.lbColor);
            this.Controls.Add(this.btCalibrate);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmColorSensor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Color Sensor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmColorSensor_FormClosing);
            this.Load += new System.EventHandler(this.frmColorSensor_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btCalibrate;
        private System.Windows.Forms.Label lbColor;
        private System.Windows.Forms.Label lbGray;
        private System.Windows.Forms.Panel pnColor;
        private System.Windows.Forms.Panel pnGray;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.IO.Ports.SerialPort serialPort1;
    }
}