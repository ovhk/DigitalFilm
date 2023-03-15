using DigitalDarkroom.Panels;
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
            cbPanels.Items.Add(new Panels.PanelSimulator());
            cbPanels.Items.Add(new Panels.WisecocoTOP103MONO8K01A());
            cbPanels.SelectedIndex = 0; // Select first panel in list


            engine = new DisplayEngine();
            engine.EngineStatusNotify += Engine_EngineStatusNotify;
            engine.OnNewImage += Engine_OnNewImage;
        }

        private void Engine_OnNewImage(Bitmap bmp)
        {
            SafeUpdate(() => this.pbDisplay.Image = bmp);
            SafeUpdate(() => this.Refresh());
            this.display.setImage(bmp); // TODO : FromDisplay doit s'abonner aussi à l'évènement
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            IPanel selectedPanel = (IPanel)this.cbPanels.SelectedItem;

            if (selectedPanel == null || !(selectedPanel is IPanel))
            {
                return;
            }

            int Width = selectedPanel.Width;
            int Height = selectedPanel.Height;

            display.setSize(Width, Height);
            display.Show();

            Image img = Image.FromFile(@"C:\Users\sectronic\Desktop\Digital-Picture-to-Analog-Darkroom-print-master\test.png");

            Bitmap a = new Bitmap(img);

            display.PushImage(a, 2000);

            Bitmap b = new Bitmap(Width, Height);

            for (int Xcount = 0; Xcount < b.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < b.Height; Ycount++)
                {
                    b.SetPixel(Xcount, Ycount, Color.Red);
                }
            }

            display.PushImage(new Bitmap(b), 2000);

            //display.PushImage(GenerateGreylevelBands8bit(Width, Height), 5000);
            //display.PushImage(GenerateGreylevelBands(Width, Height), 5000);

            GenerateMasquesTemps(Width, Height);

            display.Run();
        }

        private Bitmap GenerateGreylevelBands8bit(int width, int height)
        {
            Bitmap b = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            for (int i = 0; i < 256; i++)
            {
                FillIndexedRectangle(b, new Rectangle(i * (width / 256), 0, width / 256, height), Color.FromArgb(i, i, i));
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

        private void GenerateMasquesTemps(int width, int height)
        {
            Bitmap b = new Bitmap(width, height);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                for (int i = 0; i < 256; i++)
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(i, i, i)))
                    {
                        gfx.FillRectangle(brush, i * (width / 256), 0, width / 256, height / 2);
                    }
                }

                SolidBrush brushBlack = new SolidBrush(Color.Black);
                SolidBrush brushWhite = new SolidBrush(Color.White);

                int size = 40;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        SolidBrush brush = (j > i) ? brushBlack : brushWhite;
                        SolidBrush brushTxt = (j > i) ? brushWhite : brushBlack;

                        gfx.FillRectangle(brush, j * (width / size), height / 2, width / size, height / 2);

                        string str = (j + 1).ToString();

                        SizeF stringSize = new SizeF();
                        stringSize = gfx.MeasureString(str, DefaultFont);
                        int offset = size / 2 - (int)stringSize.Width / 2;

                        gfx.DrawString(str, DefaultFont, brushTxt, j * (width / size) + offset, height / 2 + 10);
                    }
                    display.PushImage(new Bitmap(b), 500);
                }
            }
        }

        void FillIndexedRectangle(Bitmap bmp8bpp, Rectangle rect, Color col)
        {
            var bitmapData =
                bmp8bpp.LockBits(new Rectangle(Point.Empty, bmp8bpp.Size),
                                 ImageLockMode.ReadWrite, bmp8bpp.PixelFormat);
            byte[] buffer = new byte[bmp8bpp.Width * bmp8bpp.Height];

            Marshal.Copy(bitmapData.Scan0, buffer, 0, buffer.Length);

            for (int y = rect.Y; y < rect.Bottom; y++)
            {
                for (int x = rect.X; x < rect.Right; x++)
                {
                    buffer[x + y * bmp8bpp.Width] = (byte)col.ToArgb();
                }
            }
            Marshal.Copy(buffer, 0, bitmapData.Scan0, buffer.Length);
            bmp8bpp.UnlockBits(bitmapData);
        }

        private void Engine_EngineStatusNotify(object sender, EngineStatus e)
        {
            switch (e)
            {
                case EngineStatus.Started:
                    this.SafeUpdate(() => this.btPlay.Enabled = false);
                    break;
                case EngineStatus.Running:
                    // TODO récupérer le temps écoulé pour l'afficher
                    Console.WriteLine("Running");
                    break;
                case EngineStatus.Stopped:
                    this.SafeUpdate(() => this.btPlay.Enabled = true);
                    break;
                case EngineStatus.Ended:
                    this.SafeUpdate(() => this.btPlay.Enabled = true);
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

        private void btTest1_Click(object sender, EventArgs e)
        {
            IPanel selectedPanel = (IPanel)this.cbPanels.SelectedItem;

            if (selectedPanel == null || !(selectedPanel is IPanel))
            {
                return;
            }

            Bitmap b = new Bitmap(selectedPanel.Width, selectedPanel.Height);

            using (Graphics gfx = Graphics.FromImage(b))
            {

                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    gfx.FillRectangle(brush, 0, 0, selectedPanel.Width / 2, selectedPanel.Height);
                }

                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, selectedPanel.Width / 2, 0, selectedPanel.Width / 2, selectedPanel.Height);
                }
            }

            engine.PushImage(b, 60000);
        }

        private void cbPanels_SelectedIndexChanged(object sender, EventArgs e)
        {
            IPanel selectedPanel = (IPanel)this.cbPanels.SelectedItem;

            if (selectedPanel == null || !(selectedPanel is IPanel))
            {
                return;
            }

            // Let update frmDisplat size following the selected panel in the list
            display.setSize(selectedPanel.Width, selectedPanel.Height);
        }

        private void btPlay_Click(object sender, EventArgs e)
        {
            display.Show();
            Thread.Sleep(100); // Just to be sure that the display frame is loaded
            engine.Start();
        }

        private void btPause_Click(object sender, EventArgs e)
        {

        }

        private void btStop_Click(object sender, EventArgs e)
        {
            engine.Stop();
            display.Hide();
        }

        private void btNext_Click(object sender, EventArgs e)
        {

        }

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
