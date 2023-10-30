using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DigitalFilm.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorTools
    {
        //create the grayscale ColorMatrix
        public static ColorMatrix GrayscaleMatrix = new ColorMatrix(
           new float[][]
           { // Values from https://en.wikipedia.org/wiki/Grayscale
                 new float[] {.299f, .299f, .299f, 0, 0},
                 new float[] {.587f, .587f, .587f, 0, 0},
                 new float[] {.114f, .114f, .114f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
           });

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
            float fR = GrayscaleMatrix[0, 0]; // 0.299f
            float fG = GrayscaleMatrix[1, 0]; // 0.587f
            float fB = GrayscaleMatrix[2, 0]; // 0.114f
            
            int grayScale = (int)((color.R * fR) + (color.G * fG) + (color.B * fB));
            
            return Color.FromArgb(color.A, grayScale, grayScale, grayScale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static UInt16 ColorToGrayScale16(Color color)
        {
            float fR = GrayscaleMatrix[0, 0]; // 0.299f
            float fG = GrayscaleMatrix[1, 0]; // 0.587f
            float fB = GrayscaleMatrix[2, 0]; // 0.114f

            // TODO : is it the better way?
            float R = color.R * UInt16.MaxValue / 255; // TODO : A TESTER 
            float G = color.G * UInt16.MaxValue / 255;
            float B = color.B * UInt16.MaxValue / 255;

            return (UInt16)((R * fR) + (G * fG) + (B * fB));
        }
    }
}
