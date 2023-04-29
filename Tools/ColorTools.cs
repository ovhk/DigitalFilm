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
    }
}
