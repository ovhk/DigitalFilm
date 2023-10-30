using System;
using System.Drawing;

namespace DigitalFilm.Tools
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ColorToGrayScale(Color color)
        {
            // TODO : is it possible from a 24 bit RGB to get a more than 8 bit gray level? 
            int grayScale = (int)((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            return Color.FromArgb(color.A, grayScale, grayScale, grayScale);
        }
    }
}
