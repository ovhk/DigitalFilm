using System.Drawing;

namespace DigitalFilm.Tools
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
        public static void DrawLargestString(ref Graphics graphics, ref SolidBrush brush, string txt, Rectangle rectangle, double factor)
        {
            for (int i = 100; i > 0; i--)
            {
                using (Font font = new Font(DrawTools.DefaultFont, i, FontStyle.Regular, GraphicsUnit.Point, 0))
                {
                    SizeF stringSize = graphics.MeasureString(txt, font);

                    if (stringSize.Width < rectangle.Width * factor)
                    {
                        StringFormat stringFormat = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };

                        graphics.DrawString(txt, font, brush, rectangle, stringFormat);
                        break;
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="brush"></param>
        /// <param name="txt"></param>
        /// <param name="rectangle"></param>
        public static void DrawLargestString(ref Graphics graphics, ref SolidBrush brush, string txt, Rectangle rectangle)
        {
            DrawLargestString(ref graphics, ref brush, txt, rectangle, 0.8);
        }
    }
}
