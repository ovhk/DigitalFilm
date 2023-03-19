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
