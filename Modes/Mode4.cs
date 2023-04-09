using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    internal class Mode4 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Gray vs B&W Linear";

        // TODO : bug sur le temps, idem Mode 4 et 7 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Compare a grayscale with B&W time interval.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display duration")]
        public int Duration
        { get; set; } = 5000;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            if (Duration/256 <= 40)
            {
                Log.WriteLine("Interval is too short... {0}/256={1} <= 40 ms", Duration, Duration/256);
                return false;
            }

            using (Bitmap b = new Bitmap(engine.Panel.Width, engine.Panel.Height))
            {
                int width = engine.Panel.Width;
                int height = engine.Panel.Height;

                using (Graphics gfx = Graphics.FromImage(b))
                {
                    for (int i = 0; i < engine.Panel.NumberOfColors; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i)))
                        {
                            gfx.FillRectangle(brush, i * (width / engine.Panel.NumberOfColors), 0, width / engine.Panel.NumberOfColors, height / 2);
                        }
                    }

                    SolidBrush brushBlack = new SolidBrush(Color.Black);
                    SolidBrush brushWhite = new SolidBrush(Color.White);

                    int size = engine.Panel.NumberOfColors;

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            SolidBrush brush = (j > i) ? brushBlack : brushWhite;

                            gfx.FillRectangle(brush, j * (width / size), height / 2, width / size, height / 2);
                        }

                        // new Bitmap because we need a copy, next iteration b will be changed
                        engine.PushImage(new Bitmap(b), (Duration / 256));
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
