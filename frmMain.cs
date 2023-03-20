using DigitalDarkroom.Panels;
using DigitalDarkroom.Modes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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
            this.DoubleBuffered = true; // Needed to eliminate display flickering
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
            LoadModes();

            engine = DisplayEngine.GetInstance();
            engine.EngineStatusNotify += Engine_EngineStatusNotify;
            engine.OnNewImage += Engine_OnNewImage;
            engine.OnNewProgress += Engine_OnNewProgress;
            engine.Stop(); // Call engine notification to enable/disable controls

            cbPanels.Items.Add(new Panels.NoPanel(this.propertyGrid1));
            cbPanels.Items.Add(new Panels.PanelSimulator());
            cbPanels.Items.Add(new Panels.WisecocoTOP103MONO8K01A());
            cbPanels.SelectedIndex = 0; // Select first panel in list

            listView1.OwnerDraw = true;
            listView1.DrawItem += listView1_DrawItem;
        }

        #region ListView Tile Management

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            ListView view = (ListView)sender;
            int border = 10;

            Color textColor = SystemColors.WindowText;
            if (e.Item.Selected)
            {
                if (view.Focused)
                {
                    textColor = SystemColors.HighlightText;
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                }
                else if (!view.HideSelection)
                {
                    textColor = SystemColors.ControlText;
                    e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
                }
            }
            else
            {
                using (SolidBrush br = new SolidBrush(view.BackColor))
                {
                    e.Graphics.FillRectangle(br, e.Bounds);
                }
            }

            e.Graphics.DrawImage(view.LargeImageList.Images[e.Item.Index], e.Bounds.X + border, e.Bounds.Y + border, e.Bounds.Width-2*border, e.Bounds.Height-2*border);

            e.Graphics.DrawRectangle(Pens.Red, e.Bounds);
            TextRenderer.DrawText(e.Graphics, e.Item.Text, view.Font, e.Bounds,
                                  textColor, Color.Empty,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListView lv = sender as ListView;

            //if (lv.SelectedItems.Count == 0)
            //{
            //    return;
            //}

            //this.propertyGrid1.SelectedObject = lv.SelectedItems[0].Tag as string;
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;

            if (lv.SelectedItems.Count == 0)
            {
                return;
            }

            ImageLayer il = lv.SelectedItems[0].Tag as ImageLayer;
            
            if (il != null)
            {
                this.pbDisplay.Image = il.GetBitmap();
                this.propertyGrid1.SelectedObject = lv.SelectedItems[0].Tag as ImageLayer;
            }
        }

        #endregion

        #region Display Engine notifications

        private void Engine_OnNewProgress(int imageLayerIndex, TimeSpan elapseTime, TimeSpan totalDuration)
        {
            SafeUpdate(() => this.lbTime.Text = String.Format("{0:00}:{1:00}.{2:00}",
                elapseTime.Minutes, elapseTime.Seconds, elapseTime.Milliseconds / 10));
            SafeUpdate(() => this.lbTime.ForeColor = Color.White);
            SafeUpdate(() => this.lbTime.Refresh());

            // TODO : use imageLayerIndex to update selected tile
            //SafeUpdate(() => listView1.Items[imageLayerIndex].Selected = true); //marche pas

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
            //Image old = this.pbDisplay.Image; // this cause Exception
            SafeUpdate(() => this.pbDisplay.Image = bmp);
            //SafeUpdate(() => this.pbDisplay.Refresh());
            //if (old != null) old.Dispose(); // this cause Exception
        }

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

        #endregion

        #region TODO REMOVE THIS

        private Bitmap GenerateGreylevelBands8bit(int width, int height)
        {
            Bitmap b = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            for (int i = 0; i < 256; i++)
            {
                ToolsTODO.FillIndexedRectangle(b, new Rectangle(i * (width / 256), 0, width / 256, height), Color.FromArgb(i, i, i));
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

        #region Modes Management

        private RadioButton selectedrbMode;

        /// <summary>
        /// Load the list of integrated tests
        /// </summary>
        private void LoadModes()
        {
            this.rbMode1.Tag = new Modes.Mode1() as object;
            this.rbMode1.Text = ((IMode)this.rbMode1.Tag).Name;

            this.rbMode2.Tag = new Modes.Mode2() as object;
            this.rbMode2.Text = ((IMode)this.rbMode2.Tag).Name;

            this.rbMode3.Tag = new Modes.Mode3() as object;
            this.rbMode3.Text = ((IMode)this.rbMode3.Tag).Name;

            this.rbMode4.Tag = new Modes.Mode4() as object;
            this.rbMode4.Text = ((IMode)this.rbMode4.Tag).Name;

            this.rbMode5.Tag = new Modes.Mode5() as object;
            this.rbMode5.Text = ((IMode)this.rbMode5.Tag).Name;

            this.rbMode6.Tag = new Modes.Mode6() as object;
            this.rbMode6.Text = ((IMode)this.rbMode6.Tag).Name;

            btUnloadMode.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBrowseImgFiles_Click(object sender, EventArgs e)
        {
            string[] filesPath;

            //openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            openFileDialog1.Filter = GetImageFilter();
            openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filesPath =  openFileDialog1.FileNames;
            }
        }

        private static string GetImageFilter()
        {
            return
                "All Files (*.*)|*.*" +
                "|All Pictures (*.emf;*.wmf;*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.bmp;*.dib;*.rle;*.gif;*.emz;*.wmz;*.tif;*.tiff;*.svg;*.ico)" +
                    "|*.emf;*.wmf;*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.bmp;*.dib;*.rle;*.gif;*.emz;*.wmz;*.tif;*.tiff;*.svg;*.ico" +
                "|Windows Enhanced Metafile (*.emf)|*.emf" +
                "|Windows Metafile (*.wmf)|*.wmf" +
                "|JPEG File Interchange Format (*.jpg;*.jpeg;*.jfif;*.jpe)|*.jpg;*.jpeg;*.jfif;*.jpe" +
                "|Portable Network Graphics (*.png)|*.png" +
                "|Bitmap Image File (*.bmp;*.dib;*.rle)|*.bmp;*.dib;*.rle" +
                "|Compressed Windows Enhanced Metafile (*.emz)|*.emz" +
                "|Compressed Windows MetaFile (*.wmz)|*.wmz" +
                "|Tag Image File Format (*.tif;*.tiff)|*.tif;*.tiff" +
                "|Scalable Vector Graphics (*.svg)|*.svg" +
                "|Icon (*.ico)|*.ico";
        }

        /// <summary>
        /// Called when selected test changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                return;
            }

            if (rb.Checked)
            {
                selectedrbMode = rb;
                IMode mode = rb.Tag as IMode;
                this.tbModeDescription.Text = mode.Description;
            }
        }
        /// <summary>
        /// Load the selected test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLoadTest_Click(object sender, EventArgs e)
        {
            if (selectedrbMode == null)
            {
                return;
            }

            // TODO : status bar ?

            this.SuspendLayout(); // TODO : usefull ?

            IMode mode = selectedrbMode.Tag as IMode;

            int duration = int.Parse(tbDuration.Text);

            mode.Load(null, duration);

            this.listView1.Items.Clear();
            this.listView1.LargeImageList = engine.GetImageList();
            this.listView1.Items.AddRange(engine.GetListViewItems().ToArray());

            this.btLoadMode.Enabled = false;
            this.btUnloadMode.Enabled = true;

            this.tbDuration.Enabled = false;
            this.rbMode1.Enabled = false;
            this.rbMode2.Enabled = false;
            this.rbMode3.Enabled = false;
            this.rbMode4.Enabled = false;
            this.rbMode5.Enabled = false;
            this.rbMode6.Enabled = false;

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
            if (selectedrbMode == null)
            {
                return;
            }

            this.SuspendLayout(); // TODO : usefull ?

            IMode test = selectedrbMode.Tag as IMode;
            test.Unload();

            // TODO : status bar ?

            this.btLoadMode.Enabled = true;
            this.btUnloadMode.Enabled = false;

            this.tbDuration.Enabled = true;
            this.rbMode1.Enabled = true;
            this.rbMode2.Enabled = true;
            this.rbMode3.Enabled = true;
            this.rbMode4.Enabled = true;
            this.rbMode5.Enabled = true;
            this.rbMode6.Enabled = true;

            this.listView1.Items.Clear();

            this.ResumeLayout(true); // TODO : usefull ?
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDuration_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            int val = 0;

            if (int.TryParse(tb.Text, out val) == false)
            {
                tb.Text = "5000";
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPanels_SelectedIndexChanged(object sender, EventArgs e)
        {
            IPanel selectedPanel = this.cbPanels.SelectedItem as IPanel;

            if (selectedPanel == null || !(selectedPanel is IPanel))
            {
                return;
            }

            if (selectedPanel.ToString() == "No Panel")
            {
                engine.Panel = selectedPanel;
                return;
            }

            // Let update frmDisplay size following the selected panel in the list
            engine.Panel = selectedPanel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPlay_Click(object sender, EventArgs e)
        {
            IPanel selectedPanel = this.cbPanels.SelectedItem as IPanel;

            if (selectedPanel.ToString() != "No Panel")
            {
                display.Show();
                Thread.Sleep(100); // Just to be sure that the display frame is loaded
            }
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
