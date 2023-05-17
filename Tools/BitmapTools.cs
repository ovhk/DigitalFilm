﻿using DigitalFilm.Papers;
using System.Drawing;
using System.Drawing.Imaging;

namespace DigitalFilm.Tools
{
    public class BitmapTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="paper"></param>
        /// <returns></returns>
        public static Bitmap BitmapToPaper(Bitmap bitmap, Paper paper)
        {
            DirectBitmap bbw = new DirectBitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);

                    int c = paper.Data[color.R];
                    Color gammaColor = Color.FromArgb(c, c, c);

                    bbw.SetPixel(x, y, gammaColor);
                }
            }
            return bbw.Bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="paper"></param>
        /// <returns></returns>
        public static Bitmap BitmapFromPaper(Bitmap bitmap, Paper paper)
        {
            DirectBitmap bbw = new DirectBitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);

                    int c = paper.InvertedData[color.R];
                    Color gammaColor = Color.FromArgb(c, c, c);

                    bbw.SetPixel(x, y, gammaColor);
                }
            }
            return bbw.Bitmap;
        }

        /// <summary>
        /// Invert pixels color in the bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap GetInvertedBitmap(Bitmap bitmap)
        {
            DirectBitmap bbw = new DirectBitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color c = bitmap.GetPixel(x, y);

                    // invert
                    bbw.SetPixel(x, y, ColorTools.GetInvertedColor(c));
                }
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               { // Values from https://en.wikipedia.org/wiki/Grayscale
                 new float[] {.299f, .299f, .299f, 0, 0},
                 new float[] {.587f, .587f, .587f, 0, 0},
                 new float[] {.114f, .114f, .114f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        /// <summary>
        /// Adapted from https://stackoverflow.com/questions/20055024/draw-histogram-from-points-array
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Histogram(Bitmap bitmap)
        {
            int[] histogram_r = new int[256];
            float max = 0;

            // find max value
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    int redValue = bitmap.GetPixel(i, j).R;

                    histogram_r[redValue]++;

                    if (max < histogram_r[redValue])
                    {
                        max = histogram_r[redValue];
                    }
                }
            }

            int histHeight = 128;

            Bitmap img = new Bitmap(256, histHeight + 10);

            using (Graphics g = Graphics.FromImage(img))
            {
                for (int i = 0; i < histogram_r.Length; i++)
                {
                    float pct = histogram_r[i] / max;   // What percentage of the max is this value?

                    g.DrawLine(Pens.WhiteSmoke,
                        new Point(i, img.Height - 5),
                        new Point(i, img.Height - 5 - (int)(pct * histHeight))  // Use that percentage of the height
                        );
                }
            }

            return img;
        }

        /// <summary>
        /// Based on http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        /// </summary>
        /// <returns></returns>
        static ColorPalette GetGrayScalePalette()
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

            ColorPalette monoPalette = bmp.Palette;

            Color[] entries = monoPalette.Entries;

            for (int i = 0; i < 256; i++)
            {
                entries[i] = Color.FromArgb(i, i, i);
            }

            return monoPalette;
        }
    }
}
