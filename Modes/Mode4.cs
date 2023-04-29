﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Engine;
using DigitalDarkroom.Tools;

namespace DigitalDarkroom.Modes
{
    internal class Mode4 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Gray vs B&W Linear";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Compare a grayscale with B&W linear duration interval.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display duration in ms")]
        public int Duration
        { get; set; } = 11000;

        [Category("Configuration")]
        [Description("Gamma correction")]
        public double Gamma
        { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        private int MiniDurationMs = 40;

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
            if (Duration/256 <= MiniDurationMs)
            {
                Log.WriteLine("Interval is too short... {0}/256={1} <= {2} ms", Duration, Duration/256, MiniDurationMs);
                return false;
            }

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = (width / engine.Panel.NumberOfColors);

            // width / engine.Panel.NumberOfColors not round so we adjust...
            width = iWidth * engine.Panel.NumberOfColors;

            using (Bitmap b = new Bitmap(width, height))
            {
                Graphics gfx = Graphics.FromImage(b);
                // First, erase all
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
                }

                for (int i = 0; i < engine.Panel.NumberOfColors; i++)
                {
                    int color = engine.Panel.NumberOfColors - 1 - i;
                    // Gamma correction
                    double range = (double)color / 255;
                    double correction = 1d * Math.Pow(range, this.Gamma);
                    int filteredColor = (int)(correction * 255);

                    Color newColor = ColorTools.GetInvertedColor(filteredColor);

                    using (SolidBrush brush = new SolidBrush(newColor))
                    {
                        gfx.FillRectangle(brush, i * iWidth, 0, iWidth, height / 2);
                    }
                }

                SolidBrush brushBlack = new SolidBrush(Color.Black);
                SolidBrush brushWhite = new SolidBrush(Color.White);

                DrawTools.DrawLargestString(ref gfx, ref brushBlack, "γ=" + this.Gamma, new Rectangle(0, 0, width, height / 2), 0.2);

                for (int i = 0; i < engine.Panel.NumberOfColors; i++)
                {
                    for (int j = 0; j < engine.Panel.NumberOfColors; j++)
                    {
                        SolidBrush brush = (j > i) ? brushBlack : brushWhite;

                        gfx.FillRectangle(brush, j * iWidth, height / 2, iWidth, height / 2);
                    }

                    // new Bitmap because we need a copy, next iteration b will be changed
                    engine.PushImage(new Bitmap(b), (Duration / 256));
                }
                gfx.Dispose();
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
