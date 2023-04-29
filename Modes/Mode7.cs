﻿using DigitalDarkroom.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Engine;

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
        [Category("Mode GrayToTime")]
        [Description("Select GrayToTime curve")]
        public GrayToTimeCurve Curve
        { get; set; } = GrayToTimeCurve.PMuth;

        /// <summary>
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            int[] timings = (Curve == GrayToTimeCurve.PMuth) ? GrayToTime.TimingsPMUTH : GrayToTime.TimingsOVH;

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = (width / timings.Length);

            // width / timings.Length not round so we adjust...
            width = iWidth * timings.Length;

            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);

            using (Bitmap b = new Bitmap(width, height))
            {
                using (Graphics gfx = Graphics.FromImage(b))
                {
                    // First, erase all
                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
                    }

                    for (int i = 0; i < timings.Length; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i)))
                        {
                            gfx.FillRectangle(brush, i * iWidth, 0, iWidth, height / 2);
                        }
                    }

                    for (int i = 0; i < timings.Length; i++)
                    {
                        if (i == 1)
                        {
                            //gfx.FillRectangle(brushBlack, 0, 0, width, height / 2);
                        }

                       // gfx.FillRectangle(brushBlack, 0, height / 2, width, height / 2);

                        gfx.FillRectangle(brushWhite, i * iWidth, height / 2, iWidth, height / 2);

                        // new Bitmap because we need a copy, next iteration b will be changed
                        engine.PushImage(new Bitmap(b), timings[i]);
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
