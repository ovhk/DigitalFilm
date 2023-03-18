using DigitalDarkroom.Panels;
using DigitalDarkroom.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalDarkroom
{
    /// <summary>
    /// Main Form
    /// </summary>
    public partial class frmMain : Form
    {
        /// <summary>
        /// Main form
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Display Form
        /// </summary>
        private frmDisplay display = new frmDisplay();
        
        /// <summary>
        /// Display Engine
        /// </summary>
        private DisplayEngine engine;

        /// <summary>
        /// Main form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadTests();

            this.btStop.Enabled = false;

            engine = DisplayEngine.GetInstance();
            engine.EngineStatusNotify += Engine_EngineStatusNotify;
            engine.OnNewImage += Engine_OnNewImage;
            engine.OnNewProgress += Engine_OnNewProgress;

            cbPanels.Items.Add(new Panels.PanelSimulator());
            cbPanels.Items.Add(new Panels.WisecocoTOP103MONO8K01A());
            cbPanels.SelectedIndex = 0; // Select first panel in list
        }

        private void Engine_OnNewProgress(int imageLayerIndex, TimeSpan elapseTime, TimeSpan totalDuration)
        {
            SafeUpdate(() => this.lbTime.Text = String.Format("{0:00}:{1:00}.{2:00}",
                elapseTime.Minutes, elapseTime.Seconds, elapseTime.Milliseconds / 10));
            SafeUpdate(() => this.lbTime.ForeColor = Color.White);
            SafeUpdate(() => this.lbTime.Refresh());

            // TODO : avec les lenteurs de la VM, il arrive que le temps d'éxécution soit plus long que le max théorique, donc on filtre pour ne pas avoir une exception...
            int val = (elapseTime.TotalSeconds > totalDuration.TotalSeconds) ? (int)totalDuration.TotalSeconds : (int)elapseTime.TotalSeconds;

            SafeUpdate(() => this.toolStripProgressBar1.Value = val);
            SafeUpdate(() => this.toolStripProgressBar1.Maximum = (int) totalDuration.TotalSeconds);

            if (totalDuration.Subtract(elapseTime).TotalSeconds <= 3)
            {
                SafeUpdate(() => this.lbTime.ForeColor = Color.Yellow);
                Console.Beep();
            }
        }

        /// <summary>
        /// DisplayEngine send us a new image to display
        /// </summary>
        /// <param name="bmp"></param>
        private void Engine_OnNewImage(Bitmap bmp)
        {
            SafeUpdate(() => this.pbDisplay.Image = bmp);
            SafeUpdate(() => this.Refresh());
        }

        #region TODO REMOVE THIS

        private void buttonTest_Click(object sender, EventArgs e)
        {
            IPanel selectedPanel = (IPanel)this.cbPanels.SelectedItem;

            if (selectedPanel == null || !(selectedPanel is IPanel))
            {
                return;
            }

            int Width = selectedPanel.Width;
            int Height = selectedPanel.Height;

            engine.setSize(Width, Height);
            display.Show();

            Image img = Image.FromFile(@"C:\Users\sectronic\Desktop\Digital-Picture-to-Analog-Darkroom-print-master\test.png");

            Bitmap a = new Bitmap(img);

            engine.PushImage(a, 2000);

            Bitmap b = new Bitmap(Width, Height);

            for (int Xcount = 0; Xcount < b.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < b.Height; Ycount++)
                {
                    b.SetPixel(Xcount, Ycount, Color.Red);
                }
            }

            engine.PushImage(new Bitmap(b), 2000);

            //engine.PushImage(GenerateGreylevelBands8bit(Width, Height), 5000);
            //engine.PushImage(GenerateGreylevelBands(Width, Height), 5000);

           
            //display.Run();
        }

        private Bitmap GenerateGreylevelBands8bit(int width, int height)
        {
            Bitmap b = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            for (int i = 0; i < 256; i++)
            {
                Tools.FillIndexedRectangle(b, new Rectangle(i * (width / 256), 0, width / 256, height), Color.FromArgb(i, i, i));
            }

            return b;
        }

        private Bitmap GenerateGreylevelBands(int width, int height)
        {
            Bitmap b = new Bitmap(width, height);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                for (int i = 0; i < 256; i++)
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(i, i, i)))
                    {
                        gfx.FillRectangle(brush, i * (width / 256), 0, width / 256, height);
                    }
                }
            }

            return b;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_EngineStatusNotify(object sender, EngineStatus e)
        {
            switch (e)
            {
                case EngineStatus.Started:
                    this.SafeUpdate(() => this.btPlay.Enabled = false);
                    this.SafeUpdate(() => this.btStop.Enabled = true);
                    break;
                case EngineStatus.Running:
                    // TODO récupérer le temps écoulé pour l'afficher
                    Console.WriteLine("Running");
                    break;
                case EngineStatus.Stopped:
                case EngineStatus.Ended:
                    this.SafeUpdate(() => display.Hide());
                    this.SafeUpdate(() => this.btPlay.Enabled = false);
                    this.SafeUpdate(() => this.btStop.Enabled = false);
                    this.SafeUpdate(() => this.btUnloadTest_Click(null, null));
                    break;
            }
        }

        #region Invoke Management

        private void SafeUpdate(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        #endregion

        #region Integrated tests

        private RadioButton selectedrbTest;

        /// <summary>
        /// Load the list of integrated tests
        /// </summary>
        private void LoadTests()
        {
            this.rbTest1.Tag = new Tests.Test1() as object;
            this.rbTest1.Text = ((ITest)this.rbTest1.Tag).Name;

            this.rbTest2.Tag = new Tests.Test2() as object;
            this.rbTest2.Text = ((ITest)this.rbTest2.Tag).Name;

            btUnloadTest.Enabled = false;
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                return;
            }

            if (rb.Checked)
            {
                selectedrbTest = rb;
            }
        }
        /// <summary>
        /// Load the selected test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLoadTest_Click(object sender, EventArgs e)
        {
            if (selectedrbTest == null)
            {
                return;
            }

            // TODO : status bar ?

            this.SuspendLayout(); // TODO : usefull ?

            ITest test = selectedrbTest.Tag as ITest;
            test.Load(5000);

            listView1.Items.Clear();
            ImageList imageList = engine.GetImageList();
            listView1.LargeImageList = imageList;

            //foreach (Image i in imageList.Images)
            for (int i = 0; i < imageList.Images.Count; i++)
            {
                string key = i.ToString();

                var listViewItem = listView1.Items.Add(new ListViewItem(key));

                listViewItem.Tag = key; // TODO : idealement il faut mettre l'objet ImageLayer
                listViewItem.ImageKey = key;
            }

            btLoadTest.Enabled = false;
            btUnloadTest.Enabled = true;

            this.rbTest1.Enabled = false;
            this.rbTest2.Enabled = false;
            this.rbTest3.Enabled = false;

            this.toolStripProgressBar1.Value = 0;
            this.btPlay.Enabled = true;

            this.ResumeLayout(true); // TODO : usefull ?
        }

        /// <summary>
        /// Unload current loaded test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUnloadTest_Click(object sender, EventArgs e)
        {
            if (selectedrbTest == null)
            {
                return;
            }

            this.SuspendLayout(); // TODO : usefull ?

            ITest test = selectedrbTest.Tag as ITest;
            test.Unload();

            // TODO : status bar ?

            btLoadTest.Enabled = true;
            btUnloadTest.Enabled = false;

            this.rbTest1.Enabled = true;
            this.rbTest2.Enabled = true;
            this.rbTest3.Enabled = true;

            listView1.Items.Clear();

            this.ResumeLayout(true); // TODO : usefull ?
        }

        #endregion

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            
            this.propertyGrid1.SelectedObject = lv.SelectedItems[0].Tag as string;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPanels_SelectedIndexChanged(object sender, EventArgs e)
        {
            IPanel selectedPanel = (IPanel)this.cbPanels.SelectedItem;

            if (selectedPanel == null || !(selectedPanel is IPanel))
            {
                return;
            }

            // Let update frmDisplat size following the selected panel in the list
            engine.setSize(selectedPanel.Width, selectedPanel.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPlay_Click(object sender, EventArgs e)
        {
            display.Show();
            Thread.Sleep(100); // Just to be sure that the display frame is loaded
            engine.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPause_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStop_Click(object sender, EventArgs e)
        {
            engine.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNext_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReset_Click(object sender, EventArgs e)
        {
            // TODO : this is for test

            File.FileFormat file = new File.FileFormat();
            // Write the contents of the variable someClass to a file.
            File.FileManagement.WriteToJsonFile<File.FileFormat>(@"C:\someClass.txt", file);

            // Read the file contents back into a variable.
            File.FileFormat object1 = File.FileManagement.ReadFromJsonFile<File.FileFormat>(@"C:\someClass.txt");
        }

        #region Menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.openFileDialog1
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

        #endregion

    }
}
