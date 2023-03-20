using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Tools
{
    internal class GrayToTime
    {
        private static int[] timings = new int[256];


        // This is Pierre MUTH algo : https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/ 

        private static void computeTimings()
        {
            /* It appears that another phenomenon should be taken in account: the paper takes 1 or 2 seconds to react. 
             * Then it is very sensible for the first 15 seconds, then it gradually looses reactivity, 
             * so it takes longer to have a slightly darker black.
             * */
            int cumulatedTimeMs = 0;

            for (int i = 5; i < 256; i++)
            {
                // y=17.06*ln(x)+96.417
                cumulatedTimeMs = (int)((-17.06 * Math.Log((double)i) + 96.417) * 1000);
                //System.out.println("for gray " + i + ", exposure " + cumulatedTimeMs);
                timings[i] = cumulatedTimeMs;
            }

            for (int i = 5; i < timings.Length - 1; i++)
            {
                timings[i] = timings[i] - timings[i + 1];

                if (timings[i] < 80)
                {
                    timings[i] = 80;
                }
                //System.out.println("for gray " + i + ", time " + timings[i]);
            }

            for (int i = 0; i < 5; i++)
            {
                timings[i] = timings[5] + 100;
            }

            timings[255] = 800;
        }

        public static List<ImageLayer> GetImageLayers(Bitmap bitmap, int width, int height)
        {
            List<ImageLayer> imageLayers = new List<ImageLayer>();
            Bitmap[] stepsImages = new Bitmap[256];

            computeTimings();

            for (int i = 0; i < stepsImages.Length; i++)
            {
                DirectBitmap b = new DirectBitmap(width, height);

                for (int x = 0; x < b.Width; x++)
                {
                    for (int y = 0; y < b.Height; y++)
                    {
                        Color c = bitmap.GetPixel(x, y);
                        if (c.R < i)
                        {
                            b.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            b.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                    }
                }

                imageLayers.Add(new ImageLayer(b.Bitmap, timings[i]));
            }

            return imageLayers;
        }
    }
}
