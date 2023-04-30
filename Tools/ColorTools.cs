using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color GetInvertedColor(Color color)
        {
            return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gray"></param>
        /// <returns></returns>
        public static Color GetInvertedColor(int gray)
        {
            return Color.FromArgb(255 - gray, 255 - gray, 255 - gray);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="gamma"></param>
        /// <returns></returns>
        public static int GetColorWithGamma(int color, double gamma)
        {
            // Gamma correction
            double range = (double)color / 255;
            double correction = 1d * Math.Pow(range, gamma);
            int filteredColor = (int)(correction * 255);

            return filteredColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="gamma"></param>
        /// <returns></returns>
        public static Color GetColorWithGamma(Color color, double gamma) 
        {
            // Gamma correction
            double range = (double)color.R / 255;
            double correction = 1d * Math.Pow(range, gamma);
            int filteredColor = (int)(correction * 255);

            return Color.FromArgb(filteredColor, filteredColor, filteredColor);
        }
    }
}
