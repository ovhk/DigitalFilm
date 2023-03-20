using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom
{
    internal class ToolsTODO
    {

        private Bitmap GenerateGreylevelBands8bit(int width, int height)
        {
            Bitmap b = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            for (int i = 0; i < 256; i++)
            {
                ToolsTODO.FillIndexedRectangle(b, new Rectangle(i * (width / 256), 0, width / 256, height), Color.FromArgb(i, i, i));
            }

            return b;
        }

        public static void FillIndexedRectangle(Bitmap bmp8bpp, Rectangle rect, Color col)
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
