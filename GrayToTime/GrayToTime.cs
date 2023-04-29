using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalDarkroom.Engine
{
    /// <summary>
    /// 
    /// </summary>
    internal class GrayToTime
    {
        #region OVH Curve

        /// <summary>
        /// 
        /// </summary>
        private static int[] _timingsOVH;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int[] TimingsOVH
        {
            get
            {
                if (_timingsOVH == null)
                {
                    int[] timingsPMUTH = computeTimingsPMUTH();
                    int[] timingsOVH = computeTimingsOVH();

                    for (int i = 0; i < timingsPMUTH.Length; i++)
                    {
                        Log.WriteLine("[" + i + "] " + timingsPMUTH[i] + ", " + timingsOVH[i]);
                    }

                    _timingsOVH = timingsOVH;
                }

                return _timingsOVH;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static int[] computeTimingsOVH()
        {
            int[] timings = new int[256];

            for (int i = 0; i < timings.Length; i++)
            {
                //y = 0,0001x4 - 0,0722x3 + 17,586x2 - 1890,5x + 89686
                double ii = i;
                timings[i] = (int)(
                    0.0001 * Math.Pow(ii, 4) - 
                    0.0722 * Math.Pow(ii, 3) + 
                    17.586 * Math.Pow(ii, 2) - 
                    1890.5 * ii + 
                    89686.0);
            }

            for (int i = 0; i < timings.Length - 1; i++)
            {
                timings[i] = timings[i] - timings[i + 1];
            }

            // on fixe le dernier à 800 ms
            timings[255] = 800;

            int cumulatedTimeMs = 0;

            for (int i = 0; i < timings.Length; i++)
            {
                cumulatedTimeMs += timings[i];
                //    Log.WriteLine("for gray " + i + ", time " + timings[i]);
            }
            Log.WriteLine("[GrayToTime] OVH : cumulatedTimeMs = " + cumulatedTimeMs);

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

                timings[i] = (int)((-17.06 * Math.Log((double)i) + 96.417) * 1000);
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
    }
}
