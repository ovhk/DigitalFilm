using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Tools
{
    public class BitmapTools
    {
        /// <summary>
        /// Invert pixels color in the bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap GetInvertedBitmap(Bitmap bitmap)
        {
            DirectBitmap bbw = new DirectBitmap(bitmap.Width, bitmap.Height);

            //Parallel.For(0, bitmap.Width, x =>
            for (int x = 0; x < bitmap.Width; x++)
            {
                //Parallel.For(0, bitmap.Height, y =>
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color c = bitmap.GetPixel(x, y); // Parallel : object used elsewhere

                    // invert
                    bbw.SetPixel(x, y, ColorTools.GetInvertedColor(c));
                }//);
            }//);
            return bbw.Bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="gamma"></param>
        /// <returns></returns>
        public static Bitmap GetBitmapWithGamma(Bitmap bitmap, double gamma)
        {
            DirectBitmap bbw = new DirectBitmap(bitmap.Width, bitmap.Height);

            //Parallel.For(0, bitmap.Width, x =>
            for (int x = 0; x < bitmap.Width; x++)
            {
                //Parallel.For(0, bitmap.Height, y =>
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y); // Parallel : object used elsewhere

                    Color gammaColor = ColorTools.GetColorWithGamma(color, gamma);

                    // invert
                    bbw.SetPixel(x, y, gammaColor);
                }//);
            }//);
            return bbw.Bitmap;
        }
    }
}
