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
    /// // This is Pierre MUTH algo : https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/ 
    /// </summary>
    internal class GrayToTime
    {
        /// <summary>
        /// 
        /// </summary>
        private static int[] _timings;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int[] Timings
        {
            get
            {
                if (_timings == null)
                {
                    _timings = computeTimings();
                }

                return _timings;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static int[] computeTimings()
        {
            int[] timings = new int[256];

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

            return timings;
        }
    }
}
