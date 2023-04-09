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
using System.CodeDom;
using Newtonsoft.Json.Linq;

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
        /// Display Engine
        /// </summary>
        private DisplayEngine engine;

        #region frmMain event callbacks

        /// <summary>
        /// Main form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
#if TEST_BUFFERED_FILE
            ImageLayerFile.ClearFiles();
#endif
            LoadModes();
            engine = DisplayEngine.GetInstance();
            engine.EngineStatusNotify += Engine_EngineStatusNotify;
            engine.OnNewImage += Engine_OnNewImage;
            engine.OnNewProgress += Engine_OnNewProgress;
            engine.Stop(); // Call engine notification to enable/disable controls

            cbPanels.Items.Add(new Panels.NoPanel(this.myPictureBox1));
            cbPanels.Items.Add(new Panels.PanelSimulator());

            // Find first external screen
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen.Primary == false)
                {
                    cbPanels.Items.Add(new Panels.ExternalPanel(screen));
                    break;
                }
            }

            cbPanels.SelectedIndex = 0; // Select first panel in list

            listView1.OwnerDraw = true;
            listView1.DrawItem += listView1_DrawItem;

            toolStripStatusLabel1.Text = "Welcome to DigitalDarkroom!";

            // TODO : this is for test
            if (false)
            {
                File.FileFormat file = new File.FileFormat();
                // Write the contents of the variable someClass to a file.
                File.FileManagement.WriteToJsonFile<File.FileFormat>(@"C:\someClass.txt", file);

                // Read the file contents back into a variable.
                File.FileFormat object1 = File.FileManagement.ReadFromJsonFile<File.FileFormat>(@"C:\someClass.txt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
#if TEST_BUFFERED_FILE
            ImageLayerFile.ClearFiles();
#endif
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

            ImageLayer il = lv.SelectedItems[0].Tag as ImageLayer;
            
            if (il != null)
            {
                this.myPictureBox1.Image = il.Bitmap;
                this.propertyGrid1.SelectedObject = lv.SelectedItems[0].Tag as ImageLayer;
            }
        }

        #endregion

        #region Display Engine notifications

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageLayerIndex"></param>
        /// <param name="elapseTime"></param>
        /// <param name="totalDuration"></param>
        private void Engine_OnNewProgress(int imageLayerIndex, TimeSpan elapseTime, TimeSpan totalDuration)
        {
            SafeUpdate(() => this.lbTime.Text = String.Format("{0:00}:{1:00}.{2:00}",
                elapseTime.Minutes, elapseTime.Seconds, elapseTime.Milliseconds / 10));
            SafeUpdate(() => this.lbTime.ForeColor = Color.White);
            SafeUpdate(() => this.lbTime.Refresh());

            try
            {
                
                SafeUpdate(() => { if (listView1.Items?[imageLayerIndex] != null) listView1.Items[imageLayerIndex].Selected = true; }); // Select
                SafeUpdate(() => listView1.Items?[imageLayerIndex].EnsureVisible()); // Scroll
            }
            catch { } // In case of a stop, Items could be empty so that trow an exception

            // avec les lenteurs de la VM, il arrive que le temps d'éxécution soit plus long que le max théorique, donc on filtre pour ne pas avoir une exception...
            int val = (elapseTime.TotalSeconds > totalDuration.TotalSeconds) ? (int)totalDuration.TotalSeconds : (int)elapseTime.TotalSeconds;

            SafeUpdate(() => this.toolStripProgressBar1.Value = val);
            SafeUpdate(() => this.toolStripProgressBar1.Maximum = (int)totalDuration.TotalSeconds);

            if (totalDuration.Subtract(elapseTime).TotalSeconds <= 3)
            {
                SafeUpdate(() => this.lbTime.ForeColor = Color.Yellow);
                //Console.Beep(); // ça prend 200 ms -- NE PAS UTILISER
            }
        }

        /// <summary>
        /// DisplayEngine send us a new image to display
        /// </summary>
        /// <param name="bmp"></param>
        private void Engine_OnNewImage(Bitmap bmp)
        {
            SafeUpdate(() => this.myPictureBox1.Image = bmp);
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
                    break;
                case EngineStatus.Stopped:
                case EngineStatus.Ended:
                    this.SafeUpdate(() => this.btPlay.Enabled = false);
                    this.SafeUpdate(() => this.btStop.Enabled = false);
                    this.SafeUpdate(() => this.gbModes.Enabled = true);
                    this.SafeUpdate(() => this.btUnloadMode_Click(null, null));
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
            this.cbMode.Items.Add(new Modes.Mode6());
            this.cbMode.Items.Add(new Modes.Mode7());
            this.cbMode.Items.Add(new Modes.Mode8());

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
        private void btLoadMode_Click(object sender, EventArgs e)
        {
            if (!(cbMode.SelectedItem is IMode mode))
            {
                return;
            }

            toolStripStatusLabel1.Text = "Loading mode " + mode.Name + "...";
            Log.WriteLine("Loading mode {0}", mode.Name);

            Cursor.Current = Cursors.WaitCursor;

            this.SuspendLayout();

            if (mode.Load() == false)
            {
                {
                    string s = "Fail to load selected mode!";
                    toolStripStatusLabel1.Text = s;
                    Log.WriteLine(s);
                    MessageBox.Show(s);
                }
                this.btUnloadMode_Click(null, null);
                return;
            }

            this.listView1.Items.Clear();
            this.listView1.LargeImageList = engine.GetImageList();
            this.listView1.Items.AddRange(engine.GetListViewItems().ToArray());

            this.btLoadMode.Enabled = false;
            this.btUnloadMode.Enabled = true;

            this.toolStripProgressBar1.Value = 0;
            this.btPlay.Enabled = true;

            this.ResumeLayout(true);

            // If panel is an external panel, show on Load button
            if (engine.Panel is ExternalPanel)
            {
                display.Show();
            }

            {
                string s = "Mode loaded";
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }

            Cursor.Current = Cursors.Default;
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
                string s = "Unloaded current mode";
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }

            Cursor.Current = Cursors.WaitCursor;

            if (!(engine.Panel is ExternalPanel))
            {
                this.SafeUpdate(() => display.Hide());
            }

            this.SuspendLayout();

            mode.Unload();

            this.btLoadMode.Enabled = true;
            this.btUnloadMode.Enabled = false;

            this.listView1.Items.Clear();

            this.ResumeLayout(true);

            {
                string s = "Mode unloaded";
                toolStripStatusLabel1.Text = s;
                Log.WriteLine(s);
            }

            Cursor.Current = Cursors.Default;
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

            this.gbModes.Enabled = false;

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
