using DigitalFilm.Engine;
using DigitalFilm.Modes;
using DigitalFilm.Panels;
using DigitalFilm.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalFilm
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
            this.timer1.Interval = 1000;
            this.timer1.Tick += Timer1_Tick;
        }

        #region frmMain event callbacks

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
            engine.Clear();
            engine.Stop(); // Call engine notification to enable/disable controls

            cbPanels.Items.Add(new Panels.NoPanel(this.myPictureBox1));
            cbPanels.Items.Add(new Panels.PanelSimulator());

            // Find first external screen
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen.Primary == false)
                {
                    cbPanels.Items.Add(new Panels.ExternalPanel(screen));
                }
            }

            cbPanels.SelectedIndex = 0; // Select first panel in list

            listView1.OwnerDraw = true;
            listView1.DrawItem += listView1_DrawItem;

            this.btPreview.Enabled = false;

            toolStripStatusLabel1.Text = "Welcome to DigitalFilm!";

#if TEST
            if (false)
            {
                Test.FileFormat file = new Test.FileFormat();
                // Write the contents of the variable someClass to a file.
                Test.FileManagement.WriteToJsonFile<Test.FileFormat>(@"C:\someClass.txt", file);

                // Read the file contents back into a variable.
                Test.FileFormat object1 = Test.FileManagement.ReadFromJsonFile<Test.FileFormat>(@"C:\someClass.txt");
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            engine.Clear();
        }

#endregion

        #region Panels Management

        /// <summary>
        /// Display Form
        /// </summary>
        private frmDisplay display = new frmDisplay();

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

            this.display.Hide();
            this.btUnloadMode_Click(null, null);

            // Let update frmDisplay size following the selected panel in the list
            this.engine.Panel = selectedPanel;

            {
                string s = String.Format("New selected panel : {0}", selectedPanel.Name);
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }
        }

        #endregion

        #region ListView Tile Management

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImageList GetImageList()
        {
            ImageList il = new ImageList();

            il.ImageSize = ImageLayer.ThumbnailSize;

            ImageLayer[] arr = engine.Items;

            for (int y = 0; y < arr.Length; y++)
            {
                ImageLayer i = arr[y] as ImageLayer;
                string key = y.ToString();
                il.Images.Add(key, i.Thumbnail);
            }

            return il;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ListViewItem[] GetListViewItems()
        {
            List<ListViewItem> list = new List<ListViewItem>();

            ImageLayer[] arr = engine.Items;

            for (int y = 0; y < arr.Length; y++)
            {
                ImageLayer i = arr[y] as ImageLayer;
                string key = y.ToString();
                ListViewItem lvi = new ListViewItem(key);

                lvi.ImageIndex = i.Index;
                lvi.Tag = i;
                lvi.ImageKey = key;
                list.Add(lvi);
            }

            return list.ToArray();
        }

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

            e.Graphics.DrawImage(view.LargeImageList.Images[e.Item.Index], e.Bounds.X + border, e.Bounds.Y + border, e.Bounds.Width - 2 * border, e.Bounds.Height - 2 * border);

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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;

            if (lv.SelectedItems.Count == 0)
            {
                return;
            }

            if (engine.Status == EngineStatus.Running)
            {
                return;
            }

            ImageLayer il = lv.SelectedItems[0].Tag as ImageLayer;

            if (il != null)
            {
                this.myPictureBox1.Image = il.Bitmap;
                this.pgImageLayer.SelectedObject = lv.SelectedItems[0].Tag as ImageLayer;
            }
        }

        #endregion

        #region Timer management

        /// <summary>
        /// 
        /// </summary>
        private System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();

        /// <summary>
        /// 
        /// </summary>
        private Stopwatch timer2 = new Stopwatch();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            string s = String.Format("{0:00}:{1:00}.{2:00}", timer2.Elapsed.Minutes, timer2.Elapsed.Seconds, timer2.Elapsed.Milliseconds / 10);

            this.SafeUpdate(() => this.lbTimer.Text = s);
        }

        #endregion

        #region Display Engine management

        /// <summary>
        /// Display Engine
        /// </summary>
        private DisplayEngine engine;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageLayerIndex"></param>
        /// <param name="elapseTime"></param>
        private void Engine_OnNewProgress(int imageLayerIndex, TimeSpan elapseTime)
        {
            this.SafeUpdate(() => this.SuspendLayout());

            // Update Time
            string s = String.Format("{0:00}:{1:00}.{2:00}", elapseTime.Minutes, elapseTime.Seconds, elapseTime.Milliseconds / 10);
            this.SafeUpdate(() => this.lbTime.Text = s);

            // Update Tile
            try
            {
                this.SafeUpdate(() => { if (listView1.Items?[imageLayerIndex] != null) listView1.Items[imageLayerIndex].Selected = true; }); // Select
                this.SafeUpdate(() => listView1.Items?[imageLayerIndex].EnsureVisible()); // Scroll
            }
            catch { } // In case of a stop, Items could be empty so that trow an exception

            // Update ProgressBar

            // avec les lenteurs de la VM, il arrive que le temps d'éxécution soit plus long que le max théorique, donc on filtre pour ne pas avoir une exception...
            int val = (elapseTime.TotalSeconds > this.toolStripProgressBar1.Maximum) ? (int)this.toolStripProgressBar1.Maximum : (int)elapseTime.TotalSeconds;

            this.SafeUpdate(() => this.toolStripProgressBar1.Value = val);

            this.SafeUpdate(() => this.ResumeLayout());
        }

        /// <summary>
        /// DisplayEngine send us a new image to display
        /// </summary>
        /// <param name="bmp"></param>
        private void Engine_OnNewImage(Bitmap bmp)
        {
            this.SafeUpdate(() => this.myPictureBox1.Image = bmp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Engine_EngineStatusNotify(object sender, EngineStatus e)
        private void Engine_EngineStatusNotify(EngineStatus engineStatus, TimeSpan? totalExposureTime)
        {
            switch (engineStatus)
            {
                case EngineStatus.Started:
                    this.SafeUpdate(() => this.btPlay.Enabled = false);
                    this.SafeUpdate(() => this.btStop.Enabled = true);
                    this.SafeUpdate(() => this.gbModes.Enabled = false);
                    this.SafeUpdate(() => this.cbPanels.Enabled = false);
                    this.SafeUpdate(() => this.lbTotalExposureTime.Text = "00:00.00");
                    this.SafeUpdate(() => this.btPreview.Enabled = false);
                    break;

                case EngineStatus.Running:
                    // Init total exposure time
                    this.SafeUpdate(() => this.toolStripProgressBar1.Maximum = (int)totalExposureTime?.TotalSeconds);
                    string s = String.Format("{0:00}:{1:00}.{2:00}", totalExposureTime?.Minutes, totalExposureTime?.Seconds, totalExposureTime?.Milliseconds / 10);
                    this.SafeUpdate(() => this.lbTotalExposureTime.Text = s);

                    // Start timers
                    this.SafeUpdate(() => this.timer1.Start());
                    this.SafeUpdate(() => this.timer2.Restart());
                    break;

                case EngineStatus.Stopped:
                    this.SafeUpdate(() => this.btPlay.Enabled = false);
                    this.SafeUpdate(() => this.btStop.Enabled = false);
                    this.SafeUpdate(() => this.gbModes.Enabled = true);
                    this.SafeUpdate(() => this.cbPanels.Enabled = true);
                    this.SafeUpdate(() => this.btPreview.Enabled = true);
                    this.SafeUpdate(() => this.btUnloadMode_Click(null, null));

                    // Stop timers
                    this.SafeUpdate(() => this.timer1.Stop());
                    this.SafeUpdate(() => this.timer2.Stop());
                    Console.Beep(); // 200ms
                    break;
            }
        }

        #endregion

        #region Modes Management

        /// <summary>
        /// Load the list of integrated tests
        /// </summary>
        private void LoadModes()
        {
            this.cbMode.Items.Add(new Modes.Mode1());
            this.cbMode.Items.Add(new Modes.Mode2());
            this.cbMode.Items.Add(new Modes.Mode3());
            this.cbMode.Items.Add(new Modes.Mode4());
            this.cbMode.Items.Add(new Modes.Mode5());
            //this.cbMode.Items.Add(new Modes.Mode6());
            this.cbMode.Items.Add(new Modes.Mode7());
            this.cbMode.Items.Add(new Modes.Mode8());
            this.cbMode.Items.Add(new Modes.Mode9());

            this.btUnloadMode.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cbMode.SelectedItem is IMode mode))
            {
                return;
            }

            this.tbModeDescription.Text = mode.Description;
            this.pgMode.SelectedObject = mode;

            {
                string s = String.Format("New selected mode : {0}", mode.Name);
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }
        }

        /// <summary>
        /// Load the selected mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btLoadMode_Click(object sender, EventArgs e)
        {
            if (!(cbMode.SelectedItem is IMode mode))
            {
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();

            this.toolStripStatusLabel1.Text = "Loading mode " + mode.Name + "...";
            Log.WriteLine("Loading mode {0}", mode.Name);

            this.UseWaitCursor = true;

            this.toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            this.toolStripProgressBar1.MarqueeAnimationSpeed = 100;

            bool bLoaded = false;

            await Task.Run(() =>
            {
                bLoaded = mode.Load();
            });

            this.toolStripProgressBar1.MarqueeAnimationSpeed = 0;
            this.toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            this.toolStripProgressBar1.Value = 0;

            if (bLoaded == false)
            {
                {
                    string s = "Fail to load selected mode!";
                    this.toolStripStatusLabel1.Text = s;
                    Log.WriteLine(s);
                    MessageBox.Show(s);
                }
                this.btUnloadMode_Click(null, null);
                return;
            }

            this.SuspendLayout();

            this.listView1.Items.Clear();
            this.listView1.LargeImageList = GetImageList();
            this.listView1.Items.AddRange(GetListViewItems());

            this.pgMode.Enabled = false;
            this.cbMode.Enabled = false;
            this.btLoadMode.Enabled = false;
            this.btUnloadMode.Enabled = true;

            this.btPlay.Enabled = true;
            this.btPreview.Enabled = true;

            this.ResumeLayout(true);

            // If panel is an external panel, show on Load button
            if (engine.Panel is ExternalPanel)
            {
                display.Show();
            }

            {
                string s = "Mode loaded in " + sw.Elapsed.ToString("mm\\:ss\\.ff");
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }

            this.UseWaitCursor = false;
        }

        /// <summary>
        /// Unload current loaded mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUnloadMode_Click(object sender, EventArgs e)
        {
            if (!(cbMode.SelectedItem is IMode mode))
            {
                return;
            }

            {
                string s = "Unloading current mode";
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }

            this.UseWaitCursor = true;

            if (!(engine.Panel is ExternalPanel))
            {
                this.SafeUpdate(() => display.Hide());
            }

            this.SuspendLayout();

            mode.Unload();

            this.pgMode.Enabled = true;
            this.cbMode.Enabled = true;
            this.btLoadMode.Enabled = true;
            this.btUnloadMode.Enabled = false;
            this.btPreview.Enabled = false;
            this.toolStripProgressBar1.Value = 0;

            this.listView1.Items.Clear();

            this.ResumeLayout(true);

            {
                string s = "Mode unloaded";
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }

            this.UseWaitCursor = false;
        }

        #endregion

        #region Preview Management

        private Bitmap _savedImage = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.myPictureBox1.Image == null)
            {
                return;
            }

            this._savedImage = this.myPictureBox1.Image;

            Bitmap imageToDisplay = null;

            if (cbMode.SelectedItem is Mode5)
            {
                Mode5 mode = cbMode.SelectedItem as Mode5;

                imageToDisplay = BitmapTools.BitmapFromPaper(this._savedImage, mode.Paper);
            }
            else // Try to display with a default correction : Gamma = 1.4 (standard for paper)
            {
                // invert pixel
                Bitmap invertedImage = BitmapTools.GetInvertedBitmap(this._savedImage);

                // Apply paper gamma
                imageToDisplay = BitmapTools.GetBitmapWithGamma(invertedImage, 1.4);
            }

            this.myPictureBox1.Image = imageToDisplay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPreview_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.myPictureBox1.Image == null)
            {
                return;
            }

            this.myPictureBox1.Image = this._savedImage;
            this._savedImage = null;
        }

        #endregion

        #region Engine Play/Stop buttons

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPlay_Click(object sender, EventArgs e)
        {
            {
                string s = "Playing";
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }

            if (engine.Panel is PanelSimulator)
            {
                display.Show();
                Thread.Sleep(200); // Just to be sure that the display frame is loaded
            }

            engine.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStop_Click(object sender, EventArgs e)
        {
            engine.Stop();

            this.gbModes.Enabled = true;

            {
                string s = "Stop";
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }
        }

        #endregion

        #region Menu

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

        #endregion

        #region Invoke Management

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
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
    }
}
