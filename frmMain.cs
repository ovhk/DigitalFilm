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

        private void buttonTest_Click(object sender, EventArgs e)
        {
            frmDisplay display = new frmDisplay();
            display.setSize(Panels.Wisecoco1038k.Width, Panels.Wisecoco1038k.Height);
            display.Show();

            Image img = Image.FromFile(@"C:\Users\sectronic\Desktop\Digital-Picture-to-Analog-Darkroom-print-master\test.png");

            // http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale

            Bitmap a = new Bitmap(img);
            /*
            Bitmap b = MakeGrayscale3(a);

            Thread.Sleep(2000);

            display.setImage(a);

            Thread.Sleep(2000);

            display.setImage(b);
            */

            display.PushImage(a, 2000);

            Bitmap b = new Bitmap(Panels.Wisecoco1038k.Width, Panels.Wisecoco1038k.Height);

            for (int Xcount = 0; Xcount < b.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < b.Height; Ycount++)
                {
                    b.SetPixel(Xcount, Ycount, Color.Red);
                }
            }

            display.PushImage(new Bitmap(b), 2000);

            //display.setImage(b);

            //Thread.Sleep(2000);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                using (SolidBrush brush = new SolidBrush(Color.Yellow))
                {
                    gfx.FillRectangle(brush, 0, 0, 50, 50);
                }
            }

            display.PushImage(b, 2000);

            //display.setImage(b);

            //Thread.Sleep(2000);

            //display.setImage(GenerateGreylevelBands(Panels.Wisecoco1038k.Width, Panels.Wisecoco1038k.Height));

            display.PushImage(GenerateGreylevelBands(Panels.Wisecoco1038k.Width, Panels.Wisecoco1038k.Height), 5000);
            display.Run();
        }

        private Bitmap GenerateGreylevelBands(int width, int height)
        {
            Bitmap b = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            for (int i = 0; i < 256; i++)
            {
                FillIndexedRectangle(b, new Rectangle(i*(width / 256), 0, width / 256, height), Color.FromArgb(i, i, i));
            }

            return b;
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
