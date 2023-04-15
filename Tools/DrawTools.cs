﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Tools
{
    public class DrawTools
    {
        /// <summary>
        /// Application Default Font
        /// </summary>
        public static string DefaultFont = "Trebuchet MS";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="brush"></param>
        /// <param name="txt"></param>
        /// <param name="rectangle"></param>
        public static void DrawLargestString(ref Graphics graphics, ref SolidBrush brush, string txt, Rectangle rectangle)
        {
            SizeF stringSize = new SizeF();

            for (int i = 100; i > 0; i--)
            {
                using (Font font = new Font(DrawTools.DefaultFont, i, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))))
                {
                    stringSize = graphics.MeasureString(txt, font);

                    if (stringSize.Width < rectangle.Width * 0.8)
                    {
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        graphics.DrawString(txt, font, brush, rectangle, stringFormat);
                        break;
                    }

                }
            }
        }
    }
}
