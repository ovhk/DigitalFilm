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

        #region Panels Management

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
                this.myPictureBox1.Image = il.GetBitmap();
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
                SafeUpdate(() => listView1.Items[imageLayerIndex].Selected = true); // Select
                SafeUpdate(() => listView1.Items[imageLayerIndex].EnsureVisible()); // Scroll
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
                    this.SafeUpdate(() => this.btUnloadMode_Click(null, null));
                    break;
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

            this.btBrowseImgFiles.Enabled = false;

            this.btUnloadMode.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private string[] selectedFilesPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBrowseImgFiles_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            this.openFileDialog1.Filter = ImageFileFilter.GetImageFilter();
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.Multiselect = true;

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                this.selectedFilesPath = this.openFileDialog1.FileNames;
            }
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

                Type t = mode.GetType();
                if (t.Equals(typeof(Mode5)))
                {
                    this.btBrowseImgFiles.Enabled = true;
                } 
                else if (t.Equals(typeof(Mode6)))
                {
                    this.btBrowseImgFiles.Enabled = true;
                }
                else
                {
                    this.btBrowseImgFiles.Enabled = false;
                }
            }
        }
        /// <summary>
        /// Load the selected mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLoadMode_Click(object sender, EventArgs e)
        {
            if (selectedrbMode == null)
            {
                return;
            }

            if (this.btBrowseImgFiles.Enabled) // this mean that this mode need a file or files
            {
                if (this.selectedFilesPath == null)
                {
                    MessageBox.Show("No file selected");
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            // TODO : status bar ?

            this.SuspendLayout();

            IMode mode = selectedrbMode.Tag as IMode;

            int duration = int.Parse(tbDuration.Text);

            if (mode.Load(this.selectedFilesPath, duration) == false)
            {
                MessageBox.Show("Fail to load selected mode!");
                this.btUnloadMode_Click(null, null);
                return;
            }

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

            this.ResumeLayout(true);

            // If panel is an external panel, show on Load button
            if (engine.Panel is ExternalPanel)
            {
                display.Show();
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
            if (selectedrbMode == null)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            if (!(engine.Panel is ExternalPanel))
            {
                this.SafeUpdate(() => display.Hide());
            }

            this.SuspendLayout();

            IMode mode = selectedrbMode.Tag as IMode;
            mode.Unload();

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

            this.ResumeLayout(true);

            Cursor.Current = Cursors.Default;
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

        #region Engine Play/Stop buttons

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPlay_Click(object sender, EventArgs e)
        {
            if (engine.Panel is PanelSimulator)
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
        private void btStop_Click(object sender, EventArgs e)
        {
            engine.Stop();
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
