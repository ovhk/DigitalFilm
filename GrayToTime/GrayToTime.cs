using DigitalFilm.Modes;
using DigitalFilm.Tools;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DigitalFilm.Engine
{
    /// <summary>
    /// 
    /// </summary>
    internal class GrayToTime
    {
        #region Custom Curve

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int[] ComputeTimingsCustom(int nbgray, string formula)
        {
            int[] timings = new int[nbgray];

            DynamicMathEvaluation dm = new DynamicMathEvaluation();
            
            dm.Compile(formula);

            for (int i = 0; i < timings.Length; i++)
            {
                ////y = 0,0001x4 - 0,0722x3 + 17,586x2 - 1890,5x + 89686
                //double ii = i;
                //timings[i] = (int)(
                //    0.0001 * Math.Pow(ii, 4) -
                //    0.0722 * Math.Pow(ii, 3) +
                //    17.586 * Math.Pow(ii, 2) -
                //    1890.5 * ii +
                //    89686.0);

                timings[i] = dm.Eval(i);
            }

            for (int i = 0; i < timings.Length - 1; i++)
            {
                timings[i] = timings[i] - timings[i + 1];
            }

            int cumulatedTimeMs = 0;

            for (int i = 0; i < timings.Length; i++)
            {
                cumulatedTimeMs += timings[i];
                //    Log.WriteLine("for gray " + i + ", time " + timings[i]);
            }

            Log.WriteLine("[GrayToTime] Custom : cumulatedTimeMs = " + cumulatedTimeMs);

            return timings;
        }

        #endregion

        #region PMUTH Curve

        /// <summary>
        /// 
        /// </summary>
        private static int[] _timingsPMUTH;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int[] TimingsPMUTH
        {
            get
            {
                if (_timingsPMUTH == null)
                {
                    _timingsPMUTH = computeTimingsPMUTH();
                }

                return _timingsPMUTH;
            }
        }

        /// <summary>
        /// This is Pierre MUTH algo : https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/ 
        /// </summary>
        /// <returns></returns>
        private static int[] computeTimingsPMUTH()
        {
            int[] timings = new int[256];

            /* It appears that another phenomenon should be taken in account: the paper takes 1 or 2 seconds to react. 
             * Then it is very sensible for the first 15 seconds, then it gradually looses reactivity, 
             * so it takes longer to have a slightly darker black.
             * */
            for (int i = 5; i < 256; i++)
            {
                // y = 17.06 * ln(x) + 96.417

                timings[i] = (int)(((-17.06 * Math.Log(i)) + 96.417) * 1000);
            }

            for (int i = 5; i < timings.Length - 1; i++)
            {
                timings[i] = timings[i] - timings[i + 1];
            }

            // on réhausse les 5 premiers
            for (int i = 0; i < 5; i++)
            {
                timings[i] = timings[5] + 100;
            }

            // on filtre les valeurs trop basses à 80 ms
            for (int i = 0; i < 256; i++)
            {
                if (timings[i] < 80)
                {
                    timings[i] = 80;
                }
            }

            // on fixe le dernier à 800 ms
            timings[255] = 800;

            int cumulatedTimeMs = 0;

            for (int i = 0; i < timings.Length; i++)
            {
                cumulatedTimeMs += timings[i];
                //    Log.WriteLine("for gray " + i + ", time " + timings[i]);
            }
            Log.WriteLine("[GrayToTime] PMUTH : cumulatedTimeMs = " + cumulatedTimeMs);

            return timings;
        }

        #endregion

        #region GetImageLayers from GrayToTime algo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="magickImage"></param>
        /// <param name="grayToTimeCurve"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static List<ImageLayer> GetImageLayers(MagickImage magickImage, GrayToTimeCurve grayToTimeCurve, string formula, int maxLayers)
        {
            List<ImageLayer> imageLayers = new List<ImageLayer>();

            int[] timings = null;

            int numberOfGray = magickImage.TotalColors;

            //IReadOnlyDictionary<IMagickColor<float>, int> histogram = magickImage.Histogram();
            //numberOfGray = histogram.Count; // Each color is one pixel.
            
            if (grayToTimeCurve == GrayToTimeCurve.PMuth)
            {
                if (numberOfGray > GrayToTime.TimingsPMUTH.Length)
                {
                    Log.WriteLine("Error, number of grays is different from timings array size...");
                    return null;
                }

                timings = GrayToTime.TimingsPMUTH;
            }
            else if (grayToTimeCurve == GrayToTimeCurve.Custom)
            {
                try
                {
                    timings = GrayToTime.ComputeTimingsCustom(numberOfGray, formula);
                }
                catch
                {
                    return null;
                }

                // some debug
                int[] timingsPMUTH = GrayToTime.TimingsPMUTH;

                for (int i = 0; i < timingsPMUTH.Length; i++)
                {
                    if (i < timings.Length - 1)
                    {
                        Log.WriteLine("[" + i + "] PMuth=" + timingsPMUTH[i] + ", Custom=" + timings[i]);
                    }
                }
            }

            // TODO : a tester !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            int grayIncrement = 1;

            if (numberOfGray > maxLayers)
            {
                grayIncrement = (int) Math.Ceiling((decimal)numberOfGray / maxLayers);
            }

            IPixelCollection<float> pixels = magickImage.GetPixels();

            for (int i = 0; i < timings.Length; i += grayIncrement)
            {
                using (DirectBitmap b = new DirectBitmap(magickImage.Width, magickImage.Height))
                {
                    foreach (Pixel p in pixels)
                    {
                        // Channels: 0=R, 1=G, 2=B
                        // In GrayScale, Channel 0 = value

                        //IMagickColor<float> magickColor = p.ToColor();
                        //int currentPixelColor2 = Convert.ToInt32(magickColor.R / (float)UInt16.MaxValue * 255.0);

                        // reduce to 255.0 because the color sensor is 0-255
                        int currentPixelColor = Convert.ToInt32(p.GetChannel(0) / (float)UInt16.MaxValue * 255.0);

                        if (currentPixelColor < i) // TODO : < or <= ?
                        {
                            b.SetPixel(p.X, p.Y, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            b.SetPixel(p.X, p.Y, Color.FromArgb(0, 0, 0));
                        }
                    }

                    imageLayers.Add(new ImageLayer(b.Bitmap, timings[i], i));
                }
            }

            return imageLayers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="nbgray"></param>
        /// <param name="grayToTimeCurve"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static List<ImageLayer> GetImageLayers(Bitmap bitmap, int nbgray, GrayToTimeCurve grayToTimeCurve, string formula)
        {
            List<ImageLayer> imageLayers = new List<ImageLayer>();

            int[] timings = null;

            if (grayToTimeCurve == GrayToTimeCurve.PMuth)
            {
                timings = GrayToTime.TimingsPMUTH;
            }
            else if (grayToTimeCurve == GrayToTimeCurve.Custom)
            {
                try
                {
                    timings = GrayToTime.ComputeTimingsCustom(nbgray, formula);
                }
                catch
                {
                    return null;
                }

                // some debug
                int[] timingsPMUTH = GrayToTime.TimingsPMUTH;

                for (int i = 0; i < timingsPMUTH.Length; i++)
                {
                    Log.WriteLine("[" + i + "] PMuth=" + timingsPMUTH[i] + ", Custom=" + timings[i]);
                }
            }

            for (int i = 0; i < timings.Length; i++)
            {
                using (DirectBitmap b = new DirectBitmap(bitmap.Width, bitmap.Height))
                {
                    for (int x = 0; x < b.Width; x++)
                    {
                        for (int y = 0; y < b.Height; y++)
                        {
                            Color c = bitmap.GetPixel(x, y);

                            // we use R but G or B are equal
                            if (c.R < i) // TODO : < or <= ?
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

            return imageLayers;
        }

        #endregion
    }
}
