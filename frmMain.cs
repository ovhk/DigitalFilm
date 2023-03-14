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
        public frmMain()
        {
            InitializeComponent();
        }

        frmDisplay display = new frmDisplay();

        private void buttonTest_Click(object sender, EventArgs e)
        {
            int Width = Panels.PanelSimulator.Width;
            int Height = Panels.PanelSimulator.Height;

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
                FillIndexedRectangle(b, new Rectangle(i*(width / 256), 0, width / 256, height), Color.FromArgb(i, i, i));
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
                        gfx.FillRectangle(brush, i * (width / 256), 0, width / 256, height/2);
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


    }
}
