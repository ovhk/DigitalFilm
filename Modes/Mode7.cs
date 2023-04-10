using DigitalDarkroom.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    internal class Mode7 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Gray vs GrayToTime";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Compare a grayscale with B&W GrayToTime duration interval.";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            int[] gttTimings = GrayToTime.GetTimings();

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = (width / gttTimings.Length);

            // width / gttTimings.Length not round so we adjust...
            width = iWidth * gttTimings.Length;

            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);

            using (Bitmap b = new Bitmap(width, height))
            {
                using (Graphics gfx = Graphics.FromImage(b))
                {
                    for (int i = 0; i < gttTimings.Length; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i)))
                        {
                            gfx.FillRectangle(brush, i * iWidth, 0, iWidth, height / 2);
                        }
                    }

                    for (int i = 0; i < gttTimings.Length; i++)
                    {
                        if (i == 1)
                        {
                            //gfx.FillRectangle(brushBlack, 0, 0, width, height / 2);
                        }

                       // gfx.FillRectangle(brushBlack, 0, height / 2, width, height / 2);

                        gfx.FillRectangle(brushWhite, i * iWidth, height / 2, iWidth, height / 2);

                        // new Bitmap because we need a copy, next iteration b will be changed
                        engine.PushImage(new Bitmap(b), gttTimings[i]);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Unload()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();
            engine.Clear();

            return true;
        }

        /// <summary>
        /// Return the Name parameter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
