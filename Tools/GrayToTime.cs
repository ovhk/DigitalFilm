using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalDarkroom.Tools
{
    /// <summary>
    /// // This is Pierre MUTH algo : https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/ 
    /// </summary>
    internal class GrayToTime
    {
        /// <summary>
        /// 
        /// </summary>
        private static int[] timings;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int[] GetTimings()
        { 
            if( timings == null )
            {
                computeTimings();
            }

            return timings; 
        } 

        /// <summary>
        /// 
        /// </summary>
        private static void computeTimings()
        {
            timings = new int[256];

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

                // on fixe le mini à 80 ms
                if (timings[i] < 80)
                {
                    timings[i] = 80;
                }
                //System.out.println("for gray " + i + ", time " + timings[i]);
            }

            // on fixe les 5 premiers
            for (int i = 0; i < 5; i++)
            {
                timings[i] = timings[5] + 100;
            }

            // on fixe le dernier à 800 ms
            timings[255] = 800;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static List<ImageLayer> GetImageLayers(Bitmap bitmap, int width, int height)
        {
            List<ImageLayer> imageLayers = new List<ImageLayer>();

            computeTimings();

#if TEST_PARALELLE
            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, timings.Length, i =>
#else
            for (int i = 0; i < timings.Length; i++)
#endif
            {
                using (DirectBitmap b = new DirectBitmap(width, height))
                {
                    for (int x = 0; x < b.Width; x++)
                    {
                        for (int y = 0; y < b.Height; y++)
                        {
                            Color c = bitmap.GetPixel(x, y); ;

                            // we use R but G or B are equal
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

                    imageLayers.Add(new ImageLayer(b.Bitmap, timings[i], i));
                }
            }

#if TEST_PARALELLE
            );

            stopwatch.Stop();

        //For: 8727,5777
            Log.WriteLine("For : {0}", stopwatch.Elapsed.TotalMilliseconds);
#endif
            return imageLayers;
        }
    }
}
